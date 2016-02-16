using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxKHOpenAPILib;
using System.Threading;
using System.Net;
using System.IO;
using MySql.Data.MySqlClient;
using System.Collections.Concurrent;

namespace OpenApi
{

    public class Class1
    {
        private static Boolean _multiThread = false;
        private int _scrNum = 5000;
        private List<String> stockCodeList;
        readonly object locker = new object();
        readonly object lockerSpellDictionary = new object();
        readonly object lockerStockDictionary = new object();
        readonly object lockerRunQueue = new object();
        readonly object lockerReceivedQueue = new object();

        
        readonly object lockerOrderQueue = new object();
        private Queue<String> kiwoomQueue = new Queue<String>();
        private ConcurrentQueue<OpenApi.Spell.spellOpt> receivedQueue = new ConcurrentQueue<OpenApi.Spell.spellOpt>();
        private ConcurrentQueue<OpenApi.Spell.spellOpt> orderQueue = new ConcurrentQueue<OpenApi.Spell.spellOpt>();
        private ConcurrentQueue<OpenApi.Spell.spellOpt> runQueue = new ConcurrentQueue<OpenApi.Spell.spellOpt>();
        private Dictionary<String, OpenApi.Spell.spellOpt> spellDictionary = new Dictionary<String, OpenApi.Spell.spellOpt>();
        private Dictionary<String, String> stockCodeDictionary = new Dictionary<String, String>();
        private HashSet<String> stockCodeCount= new HashSet<String>();
        private HashSet<String> orderedCodeCount = new HashSet<String>();


        //string ftpPath = "ftp://192.168.0.30/rubytest/mysqloader/data";
        string ftpPath = "ftp://192.168.0.30/home/jijs/rubytest/mysqloader/data";
        string ftpUser =   "jijs";
        string ftpPwd = Environment.GetEnvironmentVariable("FTP_PASSWORD");
        string connStr = "Server=192.168.0.30;Database=stockWeb_production;Uid=root;Pwd="+ Environment.GetEnvironmentVariable("FTP_PASSWORD") + ";CHARSET=utf8";


        //opt10081s 입력을 받고 완료가 되때까지 기다리게 할목적의 거시기 ResetEvent();
        private AutoResetEvent _evtOpt10081 = new AutoResetEvent(true);

        
        private Class1()
        {

            stockCodeList = setStockCodeList();
            Thread t1 = new Thread(new ThreadStart(sendMessageReceived));
            Thread t2 = new Thread(new ThreadStart(orderReceivedMessage));
            t1.Start();
            t2.Start();

        }
        public void waitOneOpt10081()
        {
             //   FileLog.PrintF("waitOneOpt10081 [runQueue.Count] 1=" + runQueue.Count);
                //     FileLog.PrintF("waitOneOpt10081 [spellDictionary.Count]=" + spellDictionary.Count);
              //  if (runQueue.Count > 1)
              //  {  //2~7개일때 동일하게 1분에 134개정도 1개로 할때 56개 
              //더확인해보니 내가 슬립을 1초 주어서 56개가 나온것이고 슬립을 0.2초 주었더니 155개로 오히려 늘어남
              //오히려 슬립을 주지 않았더니  124개로 줄었다... 02초로 주는게 더빠름.
                    _evtOpt10081.WaitOne();
               //}
               // FileLog.PrintF("waitOneOpt10081 [runQueue.Count] 2=" + runQueue.Count);

                
        }
        public void setOpt10081()
        {
           //     FileLog.PrintF("setOpt10081 [runQueue.Count] 1=" + runQueue.Count);

          //      if (runQueue.Count <= 1)//3보다 작거나 같을때 풀어준다.
          //      {
                    _evtOpt10081.Set();
           //     }
          //      FileLog.PrintF("setOpt10081 [runQueue.Count] 2=" + runQueue.Count);
           
        }



        public OpenApi.Spell.spellOpt DequeueByRunQueue()
        {
            OpenApi.Spell.spellOpt tmp;
            if(!runQueue.TryDequeue(out tmp)){
                tmp = null;
            }
            return tmp;
        }


        public void EnqueueByRunQueue(OpenApi.Spell.spellOpt message)
        {
                runQueue.Enqueue(message);
        }


        public OpenApi.Spell.spellOpt DequeueByReceivedQueue()
        {
            //lock (lockerReceivedQueue)
            //{
            OpenApi.Spell.spellOpt tmp;
            if (!receivedQueue.TryDequeue(out tmp))
            {
                tmp = null; 
            }
            return tmp;
            
        }


        public void EnqueueByReceivedQueue(OpenApi.Spell.spellOpt message)
        {
           // lock (lockerReceivedQueue)
           // {
                receivedQueue.Enqueue(message);
           // }
        }




        public  void EnqueueByOrderQueue(OpenApi.Spell.spellOpt message)
        {
            lock (lockerOrderQueue)
            {
                orderQueue.Enqueue(message);
            }
        }
    
        public void AddSpellDictionary(String key,  OpenApi.Spell.spellOpt value)
        {
            lock (lockerSpellDictionary)
            {
                if (!spellDictionary.ContainsKey(key)) { 
                    spellDictionary.Add(key, value);
                }
            }
        }

        public  OpenApi.Spell.spellOpt getSpell(String key)
        {
            //'System.Collections.Generic.KeyNotFoundException'
            lock(lockerSpellDictionary) {
                OpenApi.Spell.spellOpt tmp = spellDictionary[key];
                //spellDictionary.Remove(key);
                return tmp;
            }
        }

        public void AddStockCodeDictionary(String key, String value)
        {
            lock (lockerStockDictionary)
            {
                
                if(!stockCodeDictionary.ContainsKey(key))
                {
                    //FileLog.PrintF("AddStockCodeDictionary[" + key + "]  ==" + value);
                    stockCodeDictionary.Add(key, value);
                }
                
            }
        }

        public void removeStockCodeDictionary(String key)
        {
            lock (lockerStockDictionary)
            {
                FileLog.PrintF("removeStockCodeDictionary[" + key + "]");
                stockCodeDictionary.Remove(key);
            }
        }


        public void removeSpellDictionary(String key)
        {
            lock (locker)
            {
                FileLog.PrintF("removeSpellDictionary[" + key + "]");
                spellDictionary.Remove(key);
            }
        }

        public String getStockCode(String key)
        {
            //'System.Collections.Generic.KeyNotFoundException'
            //  remove를 하면 안되겠다. 로그를 찍어보니 중복으로 나오는경우가 있다.
            // 보내는쪽에서는 중복으로 보내는게 없는데..
            // 뭔가 에러창이 뜨는것이 있는데.. 그게 뜨면 다시 읽기를 시도하는것같다 kiwoom ocx가.
            // 가져가면서 지우는것은 하면 안되겠다..
            lock(lockerStockDictionary) {
                //FileLog.PrintF("getStockCode  get key:" + key);
                String tmp = stockCodeDictionary[key];
                //stockCodeDictionary.Remove(key);
            return tmp;
            }
        }
        
        private void orderReceivedMessage()
        {
            while (true)
            {
                OpenApi.Spell.spellOpt tmp;
                while (orderQueue.TryDequeue(out tmp)){
                    waitOneOpt10081();// 멈추기..
                    //파일이 있는건 통과.
                    if (tmp.sTrCode.Equals("OPT10081"))
                    {
                        axKHOpenAPI.SetInputValue("종목코드", tmp.stockCode);
                        axKHOpenAPI.SetInputValue("기준일자", tmp.endDate);
                        axKHOpenAPI.SetInputValue("수정주가구분", tmp.modifyGubun);
                    }
                    else if (tmp.sTrCode.Equals("OPT10059"))
                    {
                        axKHOpenAPI.SetInputValue("일자", tmp.endDate);
                        axKHOpenAPI.SetInputValue("종목코드", tmp.stockCode);
                        axKHOpenAPI.SetInputValue("금액수량구분", tmp.priceOrAmount);
                        axKHOpenAPI.SetInputValue("매매구분", tmp.buyOrSell);
                        axKHOpenAPI.SetInputValue("단위구분", tmp.priceGubun);
                    }
                    int nRet = axKHOpenAPI.CommRqData(tmp.sRQNAME, tmp.sTrCode, tmp.nPrevNext, tmp.sScreenNo);

                    FileLog.PrintF("orderReceivedMessage endDate" + tmp.endDate);
                    FileLog.PrintF("orderReceivedMessage stockCode" + tmp.stockCode);
                    FileLog.PrintF("orderReceivedMessage buyOrSell" + tmp.buyOrSell);
                    FileLog.PrintF("orderReceivedMessage priceGubun" + tmp.priceGubun);
                    FileLog.PrintF("orderReceivedMessage sRQNAME" + tmp.sRQNAME);
                    FileLog.PrintF("orderReceivedMessage sTrCode" + tmp.sTrCode);
                    FileLog.PrintF("orderReceivedMessage nPrevNext" + tmp.nPrevNext);
                    FileLog.PrintF("orderReceivedMessage sScreenNo" + tmp.sScreenNo);
                    /*여기부터 없네..*/
                    this.EnqueueByRunQueue(tmp);//실행중인 데이터크기를 대충알기위해서
                    orderedCodeCount.Add(tmp.stockCode);
                }
                Thread.Sleep(200); //0.2초에 한번씩 확인
            }
            
        }


        private void  sendMessageReceived()
        {
            int sendCount = 0;
            int notSendCount = 0;
            while (true)
            {
                        OpenApi.Spell.spellOpt tmp;
                        while (receivedQueue.TryDequeue(out tmp))
                        {

                            String ret = tmp.value;
                            String key = tmp.key;
                            if (ret != null && !ret.Equals(""))
                            {
                                String path = tmp.GetFileName();
                                System.IO.StreamWriter file = new System.IO.StreamWriter(path, true);
                                file.Write(ret.ToString());
                                file.Close();
                                stockCodeCount.Add(tmp.stockCode);

                            }
                            else
                            {
                                FileLog.PrintF("receive_" + tmp.sTrCode + "에 보낼 tmp.value가 비어있음 ");
                                notSendCount++;
                            }
                            //멈추는걸 풀기전에.. 하나가 더있어야해..
                            //.nPrevNext 가 2로 넘어오는건 받을게 더 있다는 것이기 때문에..
                            //
                            FileLog.PrintF("tmpSpell.isNext()=" + tmp.isNext());
                            if (tmp.isNext() == true)
                            {
                                /*
                                여러개를 많이 돌릴때 결국마지막에 nPrev 가 나중에 0으로나오길 기대하는데..
                                이어서 호출되는게 아니라 다시 처음부터 호출된다...
                                nPrevNext가 2로.. 이러면 안되는데..
                                이유가 뭘까 하나만 돌리면 정상적으로 끝까지 가는걸 보면..
                                바로 호출되어서 문제가 생기는 것 같지는 않다..
                                오히려 하나씩 호출되면 문제가 생기지 않을 꺼 같다.
                                일정개수 이상이 돌면 그전것이 초기화되는것 같다..
                                하나가 지나고 하나가 다른것이 껴서 초기화 되는것이라면 문제가 커진다...
                                확인했다.. 
                                stock_code가 5개 밖에 되지 않는 상황에서
                                무한 루프가 돈다. .원인은 연속적으로 호출되는 경우가 아닌경우
                                다시 처음으로 돌아오는것 같다.. 결국
                                이거는 다른 stock_code들이랑 같이 호출되면 안된다..
                                그러니까 여러개를 미리 받을수 없다...
                                하나씩만 처리하도록 수정하고 다시 받아보자..
                                */

                                FileLog.PrintF("tmpSpell.nPrevNext > 0  tmp.nPrevNext=" + tmp.nPrevNext);
                                FileLog.PrintF("tmpSpell.startDate=" + tmp.startDate);
                                FileLog.PrintF("sendMessageReceived tmp.sTrCode =" + tmp.sTrCode);
                                if (tmp.sTrCode.Equals("OPT10081"))
                                {
                                    //아래걸 풀면 다 가져오는 그런건데.. 이거 풀면 너무 많아서 풀수가 없다...
                                    axKHOpenAPI.SetInputValue("종목코드", tmp.stockCode);
                                    axKHOpenAPI.SetInputValue("기준일자", tmp.endDate);
                                    axKHOpenAPI.SetInputValue("수정주가구분", "1");
                                }
                                else if (tmp.sTrCode.Equals("OPT10059"))
                                {
                                    axKHOpenAPI.SetInputValue("일자", tmp.endDate);
                                    axKHOpenAPI.SetInputValue("종목코드", tmp.stockCode);
                                    axKHOpenAPI.SetInputValue("금액수량구분", tmp.priceOrAmount);
                                    axKHOpenAPI.SetInputValue("매매구분", tmp.buyOrSell);
                                    axKHOpenAPI.SetInputValue("단위구분", tmp.priceGubun);

                                    FileLog.PrintF("sendMessageReceived OPT10060 [일자] =" + tmp.endDate);
                                    FileLog.PrintF("sendMessageReceived OPT10060 [종목코드] =" + tmp.stockCode);
                                    FileLog.PrintF("sendMessageReceived OPT10060 [금액수량구분.Count]=" + tmp.priceOrAmount);
                                    FileLog.PrintF("sendMessageReceived OPT10060 [매매구분.Count]=" + tmp.buyOrSell);
                                    FileLog.PrintF("sendMessageReceived OPT10060 [단위구분]=" + tmp.priceGubun);
                                }
                                axKHOpenAPI.CommRqData(tmp.sRQNAME, tmp.sTrCode, tmp.nPrevNext, tmp.sScreenNo); // 연속조회시 
                            }
                            else
                            {
                                FileLog.PrintF("tmpSpell.nPrevNext == 0  tmp.nPrevNext=" + tmp.nPrevNext);
                                removeSpellDictionary(key);
                                int position = key.LastIndexOf("|");
                                String key1 = key.Substring(0, position);
                                removeStockCodeDictionary(key1);
                                FileLog.PrintF("sendMessageReceived [runQueue.Count] 11=" + runQueue.Count);
                                this.DequeueByRunQueue(); //실행중인 데이터크기를 대충알기위해서
                                                            /*자여기에  오면 파일작성이 완료된것이므로 ftp 를 열어서 파일을 전송하는 로직이 들어가야한다.*/
                                setOpt10081();//Queue 에 3개 있는걸로 바꾼후로 위에서 이리로 이동
                                sendCount++;

                            }
                            
                            FileLog.PrintF("sendMessageReceived [queueCount] =" + receivedQueue.Count);
                            FileLog.PrintF("sendMessageReceived [spellDictionary.Count]=" + spellDictionary.Count);
                            FileLog.PrintF("sendMessageReceived [stockCodeDictionary.Count]=" + stockCodeDictionary.Count);
                            FileLog.PrintF("sendMessageReceived [notSendCount]=" + notSendCount);
                            FileLog.PrintF("sendMessageReceived [runQueue.Count]=" + runQueue.Count);
                            
                            FileLog.PrintF("sendMessageReceived [sendCount]=" + sendCount);
                    FileLog.PrintF("sendMessageReceived [stockCodeCount] =" + stockCodeCount.Count);
                    FileLog.PrintF("sendMessageReceived [orderedCodeCount]=" + orderedCodeCount.Count);

                }
                Thread.Sleep(200); //보내는것은 0.2초에서 10초에 한번 파일로 만드는것으로 바꿔보자 어떤일이 일어나는지.
                //보내는것도 0.2초로 해야 빠르다.. 끝 더이상 이것에 대해 논쟁을 삼지말자 
                //알게된것 적어도 같은 함수,같은 코드를 호출할때 0.2초로 간격을 주는게 빠르다.
                //다른 함수 다중의 경우에 어떻게 되는지 파악 아직 안됨.
            }
        }

        public void FtpUpload(String inputFilePath)
        {
            FileLog.PrintF("FtpUpload inputFilePath = "+ inputFilePath);

            FileLog.PrintF("FtpUpload ftpPath = " + ftpPath);
            FileLog.PrintF("FtpUpload ftpUser = " + ftpUser);
            FileLog.PrintF("FtpUpload ftpPwd = " + ftpPwd);
            // 코드 단순화를 위해 하드코드함
            // WebRequest.Create로 Http,Ftp,File Request 객체를 모두 생성할 수 있다.
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpPath);
            // FTP 업로드한다는 것을 표시
            req.Method = WebRequestMethods.Ftp.UploadFile;
            // 쓰기 권한이 있는 FTP 사용자 로그인 지정
            req.Credentials = new NetworkCredential(ftpUser, ftpPwd);

            try
            {
                req.GetResponse();
            }
            catch (WebException ex)
            {
                FileLog.PrintF("로그인이 안되어있다면 에러가 나겠지 ex.Message="+ ex.Message);
                return;
            }

            

            if (!File.Exists(inputFilePath))
            {
                FileLog.PrintF("FtpUpload 파일이 존재하지 않음=>" + inputFilePath);
                return;
            }

            FileLog.PrintF("FtpUpload inputFilePath 1 = " + inputFilePath);

            // 입력파일을 바이트 배열로 읽음
            byte[] data;
            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                data = Encoding.UTF8.GetBytes(reader.ReadToEnd());
            }

            FileLog.PrintF("FtpUpload inputFilePath 2 = " + inputFilePath);

            // RequestStream에 데이타를 쓴다
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
            }

            FileLog.PrintF("FtpUpload inputFilePath 3 = " + inputFilePath);

            // FTP Upload 실행
            using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
            {
                // FTP 결과 상태 출력
                Console.WriteLine("Upload: {0}", resp.StatusDescription);
                FileLog.PrintF("Upload = " + resp.StatusDescription);
            }

            FileLog.PrintF("FtpUpload inputFilePath 4 = " + inputFilePath);
        }

        public AxKHOpenAPI getAxKHOpenAPIInstance()
        {
            return axKHOpenAPI;
        }

        public void setAxKHOpenAPIInstance(AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI = axKHOpenAPI;
        }

        private   AxKHOpenAPI axKHOpenAPI;

        private static Class1 _class1 = null;
        public static Class1 getClass1Instance()
        {
            if (_multiThread == false)
            {
                if (_class1 == null)
                {
                    _class1 = new Class1();
                }
               
            }else{
                if (_class1 == null)
                {
                    lock (_class1)
                    {
                        _class1 = new Class1();
                    }
                }
                
            }
            return _class1;

            
        }


        // 실시간 연결 종료
        public void DisconnectAllRealData()
        {
            for (int i = _scrNum; i > 5000; i--)
            {
                axKHOpenAPI.DisconnectRealData(i.ToString());
            }

            _scrNum = 5000;
            axKHOpenAPI.CommTerminate();
        }


        // 화면번호 생산
        public string GetScrNum()
        {
            if (_scrNum < 9999)
                _scrNum++;
            else
                _scrNum = 5000;

            return _scrNum.ToString();
        }

        public List<String> getStockCodeList()
        {
            return stockCodeList;
        }



        private List<String> setStockCodeList()
        {
            List<String> tt = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                string sql = "SELECT STOCK_CODE FROM stocks";
                conn.Open();
                //ExecuteReader를 이용하여
                //연결 모드로 데이타 가져오기
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ///Console.WriteLine("{0}: {1}", rdr["Id"], rdr["STOCK_CODE"]);
                    String tmp = rdr["STOCK_CODE"].ToString();
                    tt.Add(tmp);
                    //FileLog.PrintF("setStockCodeList tmp = " + tmp);
                }
                rdr.Close();
            }
            return tt;
        }
    }
}
