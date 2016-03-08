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
using OpenApi.ReceiveTrData;
using System.IO.Compression;

namespace OpenApi
{

    public class AppLib
    {
        private static Boolean _multiThread = true;

        private List<String> stockCodeList;

        readonly object lockerSpellDictionary = new object();
        readonly object lockerStockDictionary = new object();
        readonly object lockerRunQueue = new object();
        readonly object lockerReceivedQueue = new object();
        readonly object lockerOrderedCodeCount = new object();
        readonly object lockerRanUniqStockCount = new object();
        readonly object lockerCurrentRunSpellOpt = new object();

        readonly object jijs = new object();
        
        readonly object lockerOrderQueue = new object();
        private ConcurrentQueue<OpenApi.Spell.SpellOpt> receivedQueue = new ConcurrentQueue<OpenApi.Spell.SpellOpt>();
        private ConcurrentQueue<OpenApi.Spell.SpellOpt> orderQueue = new ConcurrentQueue<OpenApi.Spell.SpellOpt>();
        private ConcurrentQueue<OpenApi.Spell.SpellOpt> runQueue = new ConcurrentQueue<OpenApi.Spell.SpellOpt>();
        private Dictionary<String, OpenApi.Spell.SpellOpt> spellDictionary = new Dictionary<String, OpenApi.Spell.SpellOpt>();
        private Dictionary<String, String> stockCodeDictionary = new Dictionary<String, String>();
        private int orderedCodeCount = 0;
        private int ranUniqStockCount = 0;
        private OpenApi.Spell.SpellOpt CurrentRunSpellOpt=null;
        
        public int iEOS = 0;
        public string endDateEos = DateTime.Now.ToString("yyyyMMdd");

        //opt10081s 입력을 받고 완료가 되때까지 기다리게 할목적의 거시기 ResetEvent();
        private AutoResetEvent _evtOpt10081 = new AutoResetEvent(true);

        public void ClosedAll()
        {
            FileLog.PrintF("End");
            t1.Abort();
            t2.Abort();
            t3.Abort();
            t4.Abort();
            t5.Abort();
            Stop();
            axKHOpenAPI.Dispose();
            
        }
        
        Thread t1;
        Thread t2;
        Thread t3;
        Thread t4;
        Thread t5;

        public void Start()
        {
            FileLog.PrintF("Start");
            stockCodeList = setStockCodeList();
            SetRegRealAll();
        }

        private void Stop()
        {

            ScreenNumber.getClass1Instance().DisconnectAllAnyTimeData();
            ScreenNumber.getClass1Instance().DisconnectAllEOSData();
            ScreenNumber.getClass1Instance().DisconnectAllRealTimeData();
        }

        private AppLib()
        {
            t1 = new Thread(new ThreadStart(sendMessageReceived));
            t2 = new Thread(new ThreadStart(orderReceivedMessage));
            t3 = new Thread(new ThreadStart(StockFile.EOS_CompressZip));
            t4 = new Thread(new ThreadStart(StockFile.UploadEosFiles));
            t5 = new Thread(new ThreadStart(CheckTimeCurrentRunSellOpt));
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
        }
        public void waitOneOpt10081(String sTrCode)
        {
             FileLog.PrintF("waitOneOpt10081 sTrCode=>"+ sTrCode);
             _evtOpt10081.WaitOne();
        }
        public void setOpt10081(String sTrCode)
        {
            FileLog.PrintF("setOpt10081 sTrCode=>" + sTrCode);
            _evtOpt10081.Set();
        }



        public OpenApi.Spell.SpellOpt DequeueByRunQueue()
        {
            OpenApi.Spell.SpellOpt tmp;
            if(!runQueue.TryDequeue(out tmp)){
                tmp = null;
            }
            return tmp;
        }
        
        public void EnqueueByRunQueue(OpenApi.Spell.SpellOpt message)
        {
                runQueue.Enqueue(message);
        }


        public OpenApi.Spell.SpellOpt DequeueByReceivedQueue()
        {
            //lock (lockerReceivedQueue)
            //{
            OpenApi.Spell.SpellOpt tmp;
            if (!receivedQueue.TryDequeue(out tmp))
            {
                tmp = null; 
            }
            return tmp;
            
        }


        public void EnqueueByReceivedQueue(OpenApi.Spell.SpellOpt message)
        {
           // lock (lockerReceivedQueue)
           // {
                receivedQueue.Enqueue(message);
           // }
        }




        public  void EnqueueByOrderQueue(OpenApi.Spell.SpellOpt message)
        {
            lock (lockerOrderQueue)
            {
                orderQueue.Enqueue(message);
            }
        }
    
        public void AddSpellDictionary(String key,  OpenApi.Spell.SpellOpt value)
        {
            lock (lockerSpellDictionary)
            {
                //FileLog.PrintF("AddSpellDictionary key=>" + key);
                if (!spellDictionary.ContainsKey(key)) {
                    spellDictionary.Add(key, value);
                }
            }
        }

        public  OpenApi.Spell.SpellOpt getSpell(String key)
        {
            //'System.Collections.Generic.KeyNotFoundException'
            lock(lockerSpellDictionary) {
            //    FileLog.PrintF("getSpell key=>" + key);
                OpenApi.Spell.SpellOpt tmp = spellDictionary[key];
                //spellDictionary.Remove(key);
                return tmp;
            }
        }

        public void InRanUniqStockCount()
        {
            lock (lockerRanUniqStockCount)
            {
                this.ranUniqStockCount = this.ranUniqStockCount + 1;
            }
        }

        public int GetRanUniqStockCount()
        {
            lock (lockerRanUniqStockCount)
            {
                return ranUniqStockCount;
            }
        }


        public void InCraeteOrderedCodeCount()
        {
            lock (lockerOrderedCodeCount)
            {
                this.orderedCodeCount = this.orderedCodeCount + 1;
            }
        }

        public int GetCntOrderedCodeCount()
        {
            lock (lockerOrderedCodeCount)
            {
                return orderedCodeCount;
            }
        }


        public void AddStockCodeDictionary(String key, String value)
        {
            lock (lockerStockDictionary)
            {
                
                if(!stockCodeDictionary.ContainsKey(key))
                {
               //     FileLog.PrintF("AddStockCodeDictionary  key:" + key);
                    stockCodeDictionary.Add(key, value);
                }
                
            }
        }

        public void removeStockCodeDictionary(String key)
        {
            lock (lockerStockDictionary)
            {
                stockCodeDictionary.Remove(key);
            }
        }


        public void removeSpellDictionary(String key)
        {
            lock (lockerSpellDictionary)
            {
                spellDictionary.Remove(key);
            }
        }

        public String getStockCode(String key)
        {
            lock(lockerStockDictionary) {
          //      FileLog.PrintF("getStockCode  get key:" + key);
                String tmp = stockCodeDictionary[key];
                //stockCodeDictionary.Remove(key);
            return tmp;
            }
        }
        
        private void orderReceivedMessage()
        {
            while (true)
            {
                OpenApi.Spell.SpellOpt tmp;
                while (orderQueue.TryDequeue(out tmp)){
                        FileLog.PrintF("orderReceivedMessage");

                        waitOneOpt10081(tmp.sTrCode);// 멈추기..    //그런데 이거 없으면 에러남 0.2초 딜레이에서 유지해야할듯.
                        lock(jijs) { 
                            ReceiveTrDataFactory rtf = ReceiveTrDataFactory.getClass1Instance();
                            ReceiveTrData.ReceiveTrData rt = rtf.getReceiveTrData(tmp.sTrCode);
                            int nRet = rt.Run(axKHOpenAPI, tmp);
                            tmp.startRunTime = DateTime.Now;
                            setCurrentRunSellOpt(tmp);
                            this.EnqueueByRunQueue(tmp);//실행중인 데이터크기를 대충알기위해서                       
                            this.InRanUniqStockCount();
                        }
                }
                Thread.Sleep(200); //0.2초에 한번씩 확인 
            }
        }
        
        private void  sendMessageReceived()
        {
            int sendCount = 0;
            OpenApi.Spell.SpellOpt prevTmp=null;
            while (true) {
                OpenApi.Spell.SpellOpt tmp;
                while (receivedQueue.TryDequeue(out tmp))
                {
                    FileLog.PrintF("sendMessageReceived");
                    printRunTime(tmp);
                    //이걸 바꿀수가 없네..
                    String ret = tmp.value;
                    String key = tmp.key;

                    if (prevTmp != null) { 
                               FileLog.PrintF("prevTmp.sTrCode=" + prevTmp.sTrCode + ",tmp.sTrCode=" + tmp.sTrCode);
                    }

                    
                   if (prevTmp != null && !prevTmp.sTrCode.Equals(tmp.sTrCode))
                   {
                        String tmpPath = Config.GetPath()+prevTmp.GetCheckZipFileName();
                        System.IO.StreamWriter tmpFile = new System.IO.StreamWriter(tmpPath, true);
                        tmpFile.Write(endDateEos);
                        tmpFile.Close();
                    } else if (prevTmp != null && prevTmp.sTrCode.Equals("OPT10059") && prevTmp.sTrCode.Equals(tmp.sTrCode) 
                                && 
                               ! (prevTmp.priceOrAmount + "_"+ prevTmp.buyOrSell).Equals((tmp.priceOrAmount + "_"+tmp.buyOrSell))
                    )
                    {
                        String tmpPath = Config.GetPath() + prevTmp.GetCheckZipFileName();
                        System.IO.StreamWriter tmpFile = new System.IO.StreamWriter(tmpPath, true);
                        tmpFile.Write(endDateEos);
                        tmpFile.Close();
                    }
                    
                    String path =Config.GetPath()+tmp.GetFileName();
                    System.IO.StreamWriter file = new System.IO.StreamWriter(path, true);
                    file.Write(ret.ToString());
                    file.Close();
                    this.DequeueByRunQueue();

                    //멈추는걸 풀기전에.. 하나가 더있어야해..
                    //.nPrevNext 가 2로 넘어오는건 받을게 더 있다는 것이기 때문에..
                    FileLog.PrintF("tmpSpell.isNext()=" + tmp.isNext());
                    if (tmp.isNext() == true)
                    {
                        //주문을 백터에 넣는게 아니고 바로 하기 때문에..
                        //슬립을 주자
                        Thread.Sleep(200);
                        lock (jijs)
                        {
                            FileLog.PrintF("tmpSpell.nPrevNext > 0  tmp.nPrevNext=" + tmp.nPrevNext);
                            FileLog.PrintF("tmpSpell.startDate=" + tmp.startDate);
                            FileLog.PrintF("sendMessageReceived tmp.sTrCode =" + tmp.sTrCode);
                            ReceiveTrDataFactory rtf = ReceiveTrDataFactory.getClass1Instance();
                            ReceiveTrData.ReceiveTrData rt = rtf.getReceiveTrData(tmp.sTrCode);
                            int nRet = rt.Run(axKHOpenAPI, tmp);  //연속조회시
                            tmp.startRunTime = DateTime.Now;
                            setCurrentRunSellOpt(tmp);
                            this.EnqueueByRunQueue(tmp);//실행중인 데이터크기를 대충알기위해서2      
                        }
                    }
                    else {
                        lock (jijs)
                        {
                            FileLog.PrintF("tmpSpell.nPrevNext == 0  tmp.nPrevNext=" + tmp.nPrevNext);
                            removeSpellDictionary(key);
                            int position = key.LastIndexOf("|");
                            String key1 = key.Substring(0, position);
                            removeStockCodeDictionary(key1);
                            sendCount++;
                        }
                        setOpt10081(tmp.sTrCode); // ... 3/1쯤 느림  //그런데 이거 없으면 에러남 0.2초 딜레이에서
                    }

                    //처리하고 파일 압축을 위해
                    prevTmp = tmp.ShallowCopy();
                    if (sendCount == GetCntOrderedCodeCount() && iEOS == 1)
                    {
                        String tmpPath = Config.GetPath() + tmp.GetCheckZipFileName();
                        System.IO.StreamWriter tmpFile = new System.IO.StreamWriter(tmpPath, true);
                        tmpFile.Write(endDateEos);
                        tmpFile.Close();
                        iEOS = 0;
                    }

                    FileLog.PrintF("sendMessageReceived [receivedQueue.Count] =" + receivedQueue.Count);
                    FileLog.PrintF("sendMessageReceived [spellDictionary.Count]=" + spellDictionary.Count);
                    FileLog.PrintF("sendMessageReceived [stockCodeDictionary.Count]=" + stockCodeDictionary.Count);
                    FileLog.PrintF("sendMessageReceived [runQueue.Count]=" + runQueue.Count);
                    FileLog.PrintF("sendMessageReceived [sendCount]=" + sendCount);
                    FileLog.PrintF("sendMessageReceived [GetCntOrderedCodeCount]=" + this.GetCntOrderedCodeCount());
                    FileLog.PrintF("sendMessageReceived [GetRanUniqStockCount]=" + this.GetRanUniqStockCount());
                    FileLog.PrintF("sendMessageReceived [orderQueue.Count]=" + orderQueue.Count);
                    FileLog.PrintF("sendMessageReceived [iEOS]=" + iEOS);

                    if (sendCount == orderedCodeCount && iEOS == 1)
                    {
                        //  EOS_CompressZip();
                        //  iEOS = 0;
                    }
                }
                Thread.Sleep(200); //여기는 사실 지연이 없어도 되지않나요??
            }
        }

        private void setCurrentRunSellOpt(OpenApi.Spell.SpellOpt value)
        {
            lock (lockerCurrentRunSpellOpt)
            {
                this.CurrentRunSpellOpt = value;
            }
        }



        public static void threadJob(OpenApi.Spell.SpellOpt spellOpt)
        {
            List<String> stockCodeList = AppLib.getClass1Instance().getStockCodeList();
            for (int i = 0; i < stockCodeList.Count; i++)
            {
                OpenApi.Spell.SpellOpt tmp = spellOpt.ShallowCopy();
                String stockCode = stockCodeList[i];
                String sScrNo = ScreenNumber.getClass1Instance().GetEosScrNum();
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , tmp.sRQNAME
                    , tmp.sTrCode
                    , sScrNo
                );
                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                   , tmp.sRQNAME
                    , tmp.sTrCode
                    , sScrNo
                    , stockCode
                );
                tmp.sScreenNo = sScrNo;
                tmp.stockCode = stockCode;
                tmp.key = key;
                tmp.reportGubun = "FILE";

                //  FileLog.PrintF("threadJob keyStockCode="+ keyStockCode);
                //  FileLog.PrintF("threadJob key=" + key);

                //String logDate = DateTime.Today.ToString("yyyyMMdd");

                if (!(System.IO.File.Exists(Config.GetPath() + tmp.GetZipFileName()) || System.IO.File.Exists(Config.GetBackUpPath() + tmp.GetZipFileName())) )
                {
                    //    FileLog.PrintF("threadJob zipPath=" + zipPath);
                    if (!System.IO.File.Exists(Config.GetPath() + tmp.GetFileName()))
                    {
                        //      FileLog.PrintF("threadJob path=" + path);
                        AppLib.getClass1Instance().EnqueueByOrderQueue(tmp);
                        AppLib.getClass1Instance().AddSpellDictionary(key, tmp);
                        AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, stockCode);
                        AppLib.getClass1Instance().InCraeteOrderedCodeCount();

                    }
                }
            }
        }

        private void CheckTimeCurrentRunSellOpt()
        {
            while (true)
            {
                lock (lockerCurrentRunSpellOpt)
                {
                    if (this.CurrentRunSpellOpt != null)
                    {
                        DateTime startRunTime = this.CurrentRunSpellOpt.startRunTime;
                        DateTime checkRunTime = DateTime.Now;
                        TimeSpan gap = checkRunTime - startRunTime;
                        int iGap = gap.Seconds;
                        String key = "[ALERT]"+this.CurrentRunSpellOpt.sTrCode + "::" + this.CurrentRunSpellOpt.stockCode + "::" + this.CurrentRunSpellOpt.endDate + "::iGap=> " + iGap;
                        if (iGap > 30) { 
                            FileLog.PrintF(key);
                            OpenApi.Spell.SpellOpt tmp = this.CurrentRunSpellOpt.ShallowCopy();
                            ReceiveTrDataFactory rtf = ReceiveTrDataFactory.getClass1Instance();
                            ReceiveTrData.ReceiveTrData rt = rtf.getReceiveTrData(tmp.sTrCode);
                            int nRet = rt.Run(axKHOpenAPI, tmp);  //연속조회시
                            tmp.startRunTime = DateTime.Now;
                            setCurrentRunSellOpt(tmp);
                            this.DequeueByRunQueue();//안에 들어있는 걸빼고 
                            this.EnqueueByRunQueue(tmp);//지금 생성한것을 넣음
                            /*장애가 났다는것을 알릴수있는 어떤 함수가 필요하다고 생각함*/
                        }
                    } else
                    {
                        String key = "[ALERT2] 진행중인 데이터가 없다.";
                        FileLog.PrintF(key);
                    }
                }
                Thread.Sleep(300000); //5분에 한번씩 돔
            }

        }
        
        private void printRunTime(OpenApi.Spell.SpellOpt value)
        {
            DateTime startRunTime = value.startRunTime;
            DateTime checkRunTime = DateTime.Now;
            TimeSpan gap = checkRunTime - startRunTime;
            int iGap = gap.Milliseconds;
            String key ="[FINISH]"+ value.sTrCode + "::" + value.stockCode + "::" + value.endDate + "::iGap[Milliseconds]=> " + iGap;
            FileLog.PrintF(key);
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

        private static AppLib _class1 = null;
        private static Object _object1  = new Object();
        public static AppLib getClass1Instance() {
            if (_multiThread == false) {
                if (_class1 == null) {
                    _class1 = new AppLib();
                }               
            }else{
                if (_class1 == null) {
                    lock (_object1) {
                        _class1 = new AppLib();
                    }
                }                
            }
            return _class1;
        }


        public List<String> getStockCodeList() {
            return stockCodeList;
        }

        private List<String> setStockCodeList()
        {
            //약간 문제가 있었다. elw가 가끔 키움에서 안가져오는것같다. 그래서 장내랑,코스피만 하는걸로..
            List<String> ttt = new List<String>();

            FileLog.PrintF("setStockCodeList START");
            List<String> arrTmp = new List<String>();
            String tmp=axKHOpenAPI.GetCodeListByMarket("0");
            //FileLog.PrintF("setStockCodeList tmp=>"+ tmp);
            String[] arrT = tmp.Split(';');

            String tmp1 = axKHOpenAPI.GetCodeListByMarket("10");
            //FileLog.PrintF("setStockCodeList tmp1=>" + tmp1);
            String[] arrT1 = tmp1.Split(';');
            var tt1=arrT1.Concat(arrT).ToArray();
            Array.Sort(tt1);
            foreach (String tt in tt1)
            {
                //FileLog.PrintF("tt1 =" + tt);
                if (!tt.Equals(""))
                {
                    if(!ttt.Contains(tt))
                    ttt.Add(tt);
                }
            }
            FileLog.PrintF("setStockCodeList END");
            FileLog.PrintF("setStockCodeList11 ttt.count=>" + ttt.Count());
            ttt.Remove("069500");           //KODEX200
            ttt.Remove("069660");           //KOSEF200
            ttt.Remove("091160");           //KODEX반도체
            ttt.Remove("091170");           //KODEX은행
            ttt.Remove("091180");           //KODEX자동차
            ttt.Remove("099140");           //KODEXChinaH
            ttt.Remove("100910");           //KOSEFKRX100
            ttt.Remove("101280");           //KODEXJapan
            ttt.Remove("102780");           //KODEX삼성그룹
            ttt.Remove("102960");           //KODEX조선
            ttt.Remove("102970");           //KODEX증권
            ttt.Remove("104520");           //KOSEF블루칩
            ttt.Remove("104530");           //KOSEF고배당
            ttt.Remove("114260");           //KODEX국고채
            ttt.Remove("114470");           //KOSEF국고채
            ttt.Remove("114800");           //KODEX인버스
            ttt.Remove("117460");           //KODEX에너지화학
            ttt.Remove("117680");           //KODEX철강
            ttt.Remove("117700");           //KODEX건설
            ttt.Remove("122260");           //KOSEF통안채
            ttt.Remove("122630");           //KODEX레버리지
            ttt.Remove("130730");           //KOSEF단기자금
            ttt.Remove("132030");           //KODEX골드선물(H)
            ttt.Remove("136280");           //KODEX소비재
            ttt.Remove("138230");           //KOSEF달러선물
            ttt.Remove("138910");           //KODEX구리선물(H)
            ttt.Remove("138920");           //KODEX콩선물(H)
            ttt.Remove("139660");           //KOSEF달러인버스선물
            ttt.Remove("140700");           //KODEX보험
            ttt.Remove("140710");           //KODEX운송
            ttt.Remove("144600");           //KODEX은선물(H)
            ttt.Remove("148070");           //KOSEF10년국고채
            ttt.Remove("152280");           //KOSEF200선물
            ttt.Remove("152380");           //KODEX10년국채선물
            ttt.Remove("153130");           //KODEX단기채권
            ttt.Remove("153270");           //KOSEF100
            ttt.Remove("156080");           //KODEXMSCIKOREA
            ttt.Remove("167860");           //KOSEF10년국고채레버리지
            ttt.Remove("169950");           //KODEX중국본토A50
            ttt.Remove("176950");           //KODEX인버스국채선물10년
            ttt.Remove("185680");           //KODEX미국바이오(합성)
            ttt.Remove("200020");           //KODEX미국IT(합성)
            ttt.Remove("200030");           //KODEX미국산업재(합성)
            ttt.Remove("200040");           //KODEX미국금융(합성)
            ttt.Remove("200050");           //KODEXMSCI독일(합성)
            ttt.Remove("200250");           //KOSEF인디아(합성)
            ttt.Remove("204450");           //KODEXChinaH레버리지(H)
            ttt.Remove("211900");           //KODEX배당성장
            ttt.Remove("213610");           //KODEX삼성그룹밸류
            ttt.Remove("214980");           //KODEX단기채권PLUS
            ttt.Remove("218420");           //KODEX미국에너지(합성)
            ttt.Remove("219480");           //KODEXS&P500선물(H)
            ttt.Remove("223190");           //KODEX200내재가치
            ttt.Remove("225800");           //KOSEF미국달러선물레버리지(합성)
            ttt.Remove("226490");           //KODEX코스피
            ttt.Remove("226980");           //KODEX200중소형
            ttt.Remove("229200");           //KODEX코스닥150
            ttt.Remove("229720");           //KODEXKTOP30
            ttt.Remove("230480");           //KOSEF미국달러선물인버스2X(합성)
            ttt.Remove("233740");           //KODEX코스닥150레버리지
            ttt.Remove("237350");           //KODEX200대형
            ttt.Remove("237370");           //KODEX배당성장채권혼합
            ttt.Remove("083350");           //동북아10호
            ttt.Remove("083360");           //동북아11호
            ttt.Remove("083370");           //동북아12호
            ttt.Remove("083380");           //동북아13호
            ttt.Remove("083390");           //동북아14호
            ttt.Remove("083570");           //아시아10호
            ttt.Remove("083580");           //아시아11호
            ttt.Remove("083590");           //아시아12호
            ttt.Remove("083600");           //아시아13호
            ttt.Remove("083610");           //아시아14호
            ttt.Remove("083620");           //아시아15호
            ttt.Remove("088980");           //맥쿼리인플라
            ttt.Remove("090970");           //코리아01호
            ttt.Remove("090980");           //코리아02호
            ttt.Remove("090990");           //코리아03호
            ttt.Remove("091000");           //코리아04호
            ttt.Remove("091210");   //TIGERKRX100
            ttt.Remove("091220");   //TIGER은행
            ttt.Remove("091230");   //TIGER반도체
            ttt.Remove("092630");   //바다로3호
            ttt.Remove("094800");   //맵스리얼티1
            ttt.Remove("096300");   //베트남개발1
            ttt.Remove("097750");   //TREX중소형가치
            ttt.Remove("098560");   //TIGER미디어통신
            ttt.Remove("099340");   //하나니켈1호
            ttt.Remove("099350");   //하나니켈2호
            ttt.Remove("102110");   //TIGER200
            ttt.Remove("105010");   //TIGER라틴
            ttt.Remove("105190");   //KINDEX200
            ttt.Remove("105270");   //KINDEX성장대형F15
            ttt.Remove("105780");   //KStar5대그룹주
            ttt.Remove("107560");   //GIANT현대차그룹
            ttt.Remove("108440");   //KINDEX코스닥스타
            ttt.Remove("108450");   //KINDEX삼성그룹SW
            ttt.Remove("108590");   //TREX200
            ttt.Remove("108630");   //FIRST스타우량
            ttt.Remove("114100");   //KStar국고채
            ttt.Remove("114460");   //KINDEX국고채
            ttt.Remove("114820");   //TIGER국채3
            ttt.Remove("117690");   //TIGER차이나
            ttt.Remove("122090");   //ARIRANGKOSPI50
            ttt.Remove("122390");   //TIGER코스닥프리미어
            ttt.Remove("123310");   //TIGER인버스
            ttt.Remove("123320");   //TIGER레버리지
            ttt.Remove("123760");   //KStar레버리지
            ttt.Remove("130680");   //TIGER원유선물(H)
            ttt.Remove("131890");   //KINDEX삼성그룹EW
            ttt.Remove("133690");   //TIGER나스닥100
            ttt.Remove("136340");   //KStar우량회사채
            ttt.Remove("137610");   //TIGER농산물선물(H)
            ttt.Remove("137930");   //마이다스커버드콜
            ttt.Remove("138520");   //TIGER삼성그룹
            ttt.Remove("138530");   //TIGERLG그룹+
            ttt.Remove("138540");   //TIGER현대차그룹+
            ttt.Remove("139200");   //하이골드2호
            ttt.Remove("139220");   //TIGER200건설
            ttt.Remove("139230");   //TIGER200중공업
            ttt.Remove("139240");   //TIGER200철강소재
            ttt.Remove("139250");   //TIGER200에너지화학
            ttt.Remove("139260");   //TIGER200IT
            ttt.Remove("139270");   //TIGER200금융
            ttt.Remove("139280");   //TIGER경기방어
            ttt.Remove("139290");   //TIGER200경기소비재
            ttt.Remove("139310");   //TIGER금속선물(H)
            ttt.Remove("139320");   //TIGER금은선물(H)
            ttt.Remove("140570");   //KStar수출주
            ttt.Remove("140580");   //KStar우량업종
            ttt.Remove("140890");   //트러스제7호
            ttt.Remove("140910");   //광희리츠
            ttt.Remove("140950");   //파워K100
            ttt.Remove("141240");   //ARIRANGK100EW
            ttt.Remove("143460");   //KINDEX밸류대형
            ttt.Remove("143850");   //TIGERS&P500선물(H)
            ttt.Remove("143860");   //TIGER헬스케어
            ttt.Remove("145270");   //케이탑리츠
            ttt.Remove("145670");   //KINDEX인버스
            ttt.Remove("145850");   //TREX펀더멘탈200
            ttt.Remove("147970");   //TIGER모멘텀
            ttt.Remove("148020");   //KStar200
            ttt.Remove("148040");   //PIONEERSRI
            ttt.Remove("150460");   //TIGER중국소비테마
            ttt.Remove("152100");   //ARIRANG200
            ttt.Remove("152180");   //TIGER생활필수품
            ttt.Remove("152500");   //KINDEX레버리지
            ttt.Remove("152550");   //한국ANKOR유전
            ttt.Remove("152870");   //파워K200
            ttt.Remove("153360");   //하이골드3호
            ttt.Remove("155900");   //바다로19호
            ttt.Remove("157450");   //TIGER유동자금
            ttt.Remove("157490");   //TIGER소프트웨어
            ttt.Remove("157500");   //TIGER증권
            ttt.Remove("157510");   //TIGER자동차
            ttt.Remove("157520");   //TIGER화학
            ttt.Remove("159650");   //하이골드8호
            ttt.Remove("159800");   //마이티K100
            ttt.Remove("160580");   //TIGER구리실물
            ttt.Remove("161490");   //ARIRANG방어주
            ttt.Remove("161500");   //ARIRANG주도주
            ttt.Remove("161510");   //ARIRANG고배당주
            ttt.Remove("166400");   //TIGER커버드C200
            ttt.Remove("168300");   //KTOP50
            ttt.Remove("168490");   //한국패러랠
            ttt.Remove("168580");   //KINDEX중국본토CSI300
            ttt.Remove("170350");   //TIGER베타플러스
            ttt.Remove("172580");   //하이골드12호
            ttt.Remove("174350");   //TIGER로우볼
            ttt.Remove("174360");   //KStar중국본토대형주CSI100
            ttt.Remove("176710");   //파워국고채
            ttt.Remove("181450");   //KINDEX선진국하이일드(합성H)
            ttt.Remove("181480");   //KINDEX미국리츠부동산(합성H)
            ttt.Remove("182480");   //TIGERUS리츠(합성H)
            ttt.Remove("182490");   //TIGER단기선진하이일드(합성H)
            ttt.Remove("183700");   //KStar채권혼합
            ttt.Remove("183710");   //KStar주식혼합
            ttt.Remove("189400");   //ARIRANGAC월드(합성H)
            ttt.Remove("190150");   //ARIRANG바벨채권
            ttt.Remove("190160");   //ARIRANG단기유동성
            ttt.Remove("190620");   //KINDEX단기자금
            ttt.Remove("192090");   //TIGER차이나A300
            ttt.Remove("192720");   //파워고배당저변동성
            ttt.Remove("195920");   //TIGER일본(합성H)
            ttt.Remove("195930");   //TIGER유로스탁스50(합성H)
            ttt.Remove("195970");   //ARIRANG선진국(합성H)
            ttt.Remove("195980");   //ARIRANG신흥국(합성H)
            ttt.Remove("196030");   //KINDEX일본레버리지(H)
            ttt.Remove("196220");   //KStar일본레버리지(H)
            ttt.Remove("196230");   //KStar단기통안채
            ttt.Remove("203780");   //TIGER나스닥바이오
            ttt.Remove("204420");   //ARIRANG차이나H레버리지(합성H)
            ttt.Remove("204480");   //TIGER차이나A레버리지(합성)
            ttt.Remove("205720");   //KINDEX일본인버스(합성H)
            ttt.Remove("208470");   //SMARTMSCI선진국(합성H)
            ttt.Remove("210780");   //TIGER코스피고배당
            ttt.Remove("211210");   //마이티코스피고배당
            ttt.Remove("211260");   //KINDEX배당성장
            ttt.Remove("211560");   //TIGER배당성장
            ttt.Remove("213630");   //ARIRANG미국고배당주(합성H)
            ttt.Remove("215620");   //흥국S&P로우볼
            ttt.Remove("217770");   //TIGER원유인버스선물(H)
            ttt.Remove("217780");   //TIGER차이나A인버스(합성)
            ttt.Remove("217790");   //TIGER가격조정
            ttt.Remove("219390");   //KStar미국원유생산기업(합성H)
            ttt.Remove("219900");   //KINDEX중국본토레버리지(합성)
            ttt.Remove("220130");   //SMART중국본토중소형CSI500(합성H)
            ttt.Remove("222170");   //ARIRANGS&P배당성장
            ttt.Remove("222180");   //ARIRANG스마트베타Value
            ttt.Remove("222190");   //ARIRANG스마트베타Momentum
            ttt.Remove("222200");   //ARIRANG스마트베타Quality
            ttt.Remove("225030");   //TIGERS&P500인버스선물(H)
            ttt.Remove("225040");   //TIGERS&P500레버리지(합성H)
            ttt.Remove("225050");   //TIGER유로스탁스레버리지(합성H)
            ttt.Remove("225060");   //TIGER이머징마켓레버리지(합성H)
            ttt.Remove("225130");   //KINDEX골드선물레버리지(합성H)
            ttt.Remove("226380");   //KINDEX한류
            ttt.Remove("226810");   //파워단기채
            ttt.Remove("227540");   //TIGER200건강관리
            ttt.Remove("227550");   //TIGER200산업재
            ttt.Remove("227560");   //TIGER200생활소비재
            ttt.Remove("227570");   //TIGER우량가치
            ttt.Remove("227830");   //ARIRANG코스피
            ttt.Remove("227930");   //KINDEX코스닥150
            ttt.Remove("228790");   //TIGER화장품
            ttt.Remove("228800");   //TIGER여행레저
            ttt.Remove("228810");   //TIGER미디어컨텐츠
            ttt.Remove("228820");   //TIGERKTOP30
            ttt.Remove("232080");   //TIGER코스닥150
            ttt.Remove("232590");   //KINDEX골드선물인버스2X(합성H)
            ttt.Remove("233160");   //TIGER코스닥150레버리지
            ttt.Remove("234310");   //KStarV&S셀렉트밸류
            ttt.Remove("234790");   //KINDEX코스닥150레버리지
            ttt.Remove("236460");   //ARIRANG스마트베타LowVOL
            ttt.Remove("237440");   //TIGER경기방어채권혼합
            ttt.Remove("238670");   //ARIRANG스마트베타Quality채권혼합
            ttt.Remove("500007");   //신한인버스은선물ETN(H)
            ttt.Remove("500001");   //신한K200USD선물바이셀ETN
            ttt.Remove("500002");   //신한USDK200선물바이셀ETN
            ttt.Remove("500003");   //신한인버스WTI원유선물ETN(H)
            ttt.Remove("500004");   //신한브렌트원유선물ETN(H)
            ttt.Remove("500005");   //신한인버스브렌트원유선물ETN(H)
            ttt.Remove("500006");   //신한인버스금선물ETN(H)
            ttt.Remove("500008");   //신한인버스구리선물ETN(H)
            ttt.Remove("500009");   //신한다우존스지수선물ETN(H)
            ttt.Remove("500010");   //신한인버스다우존스지수선물ETN(H)
            ttt.Remove("500011");   //신한달러인덱스선물ETN(H)
            ttt.Remove("500012");   //신한인버스달러인덱스선물ETN(H)
            ttt.Remove("500013");   //신한옥수수선물ETN(H)
            ttt.Remove("500014");   //신한인버스옥수수선물ETN(H)
            ttt.Remove("500015");   //신한WTI원유선물ETN(H)
            ttt.Remove("500016");   //신한금선물ETN(H)
            ttt.Remove("500017");   //신한은선물ETN(H)
            ttt.Remove("500018");   //신한구리선물ETN(H)
            ttt.Remove("500019");   //신한레버리지WTI원유선물ETN(H)
            ttt.Remove("520001");   //대우로우볼ETN
            ttt.Remove("520004");   //대우전기전자Core5ETN
            ttt.Remove("520005");   //대우인버스전기전자Core5ETN
            ttt.Remove("520006");   //대우에너지화학Core5ETN
            ttt.Remove("520007");   //대우인버스에너지화학Core5ETN
            ttt.Remove("520002");   //대우차이나대표주15ETN(H)
            ttt.Remove("520003");   //대우원자재선물ETN(H)
            ttt.Remove("530003");   //삼성모멘텀탑픽ETN
            ttt.Remove("530004");   //삼성화장품테마주ETN
            ttt.Remove("530005");   //삼성바이오테마주ETN
            ttt.Remove("530006");   //삼성음식료테마주ETN
            ttt.Remove("530007");   //삼성레저테마주ETN
            ttt.Remove("530008");   //삼성미디어테마주ETN
            ttt.Remove("530009");   //삼성증권테마주ETN
            ttt.Remove("530010");   //삼성건축자재테마주ETN
            ttt.Remove("530011");   //삼성온라인쇼핑테마주ETN
            ttt.Remove("530012");   //삼성화학테마주ETN
            ttt.Remove("530013");   //삼성KTOP30ETN
            ttt.Remove("530015");   //삼성미국대형성장주ETN(H)
            ttt.Remove("530016");   //삼성미국대형가치주ETN(H)
            ttt.Remove("530017");   //삼성미국중소형성장주ETN(H)
            ttt.Remove("530018");   //삼성미국중소형가치주ETN(H)
            ttt.Remove("530019");   //삼성미국대형성장주ETN
            ttt.Remove("530020");   //삼성미국대형가치주ETN
            ttt.Remove("530021");   //삼성미국중소형성장주ETN
            ttt.Remove("530022");   //삼성미국중소형가치주ETN
            ttt.Remove("530001");   //삼성유럽고배당주식ETN(H)
            ttt.Remove("530002");   //삼성인버스ChinaA50선물ETN(H)
            ttt.Remove("530014");   //삼성ChinaA50선물ETN(H)
            ttt.Remove("550001");   //QVBigVolETN
            ttt.Remove("550002");   //QVWISE배당ETN
            ttt.Remove("550003");   //QV스마트리밸런싱250/3ETN
            ttt.Remove("550004");   //QV롱숏K150매수로우볼매도ETN
            ttt.Remove("550005");   //QV에너지TOP5ETN
            ttt.Remove("550006");   //QV내수소비TOP5ETN
            ttt.Remove("550007");   //QV조선TOP5ETN
            ttt.Remove("550008");   //QV소프트웨어TOP5ETN
            ttt.Remove("550009");   //QV하드웨어TOP5ETN
            ttt.Remove("550010");   //QV운송TOP5ETN
            ttt.Remove("550011");   //QV자동차TOP5ETN
            ttt.Remove("550012");   //QV의료TOP5ETN
            ttt.Remove("550013");   //QV화학TOP5ETN
            ttt.Remove("550014");   //QV바이오TOP5ETN
            ttt.Remove("550015");   //QV제약TOP5ETN
            ttt.Remove("550016");   //QV건설TOP5ETN
            ttt.Remove("550018");   //QVCHINEXTETN(H)
            ttt.Remove("570001");   //TRUE코스피선물매수콜매도ETN
            ttt.Remove("570002");   //TRUE코스피선물매도풋매도ETN
            ttt.Remove("570003");   //TRUE빅5동일가중ETN
            ttt.Remove("570005");   //TRUE목표변동성20코스피선물ETN
            ttt.Remove("570008");   //TRUE섹터탑픽ETN
            ttt.Remove("570009");   //TRUE코리아프리미어ETN
            ttt.Remove("570004");   //TRUE인버스유로스탁스50ETN(H)
            ttt.Remove("570007");   //TRUE위안화중국5년국채ETN
            ttt.Remove("570006");   //TRUE인버스차이나HETN(H)
            ttt.Remove("580001");   //able코스피200선물플러스ETN
            ttt.Remove("580002");   //ableQuant비중조절ETN
            ttt.Remove("580003");   //ableMonthlyBest11ETN
            ttt.Remove("580004");   //ableKQMonthlyBest11ETN
            ttt.Remove("580005");   //able우량주MonthlyBest11ETN
            ttt.Remove("590001");   //미래에셋미국바이백ETN(H)
            ttt.Remove("590002");	//미래에셋일본바이백ETN(H)

            FileLog.PrintF("setStockCodeList222 ttt.count=>"+ ttt.Count());
            return ttt;
        }

        private void SetRegRealAll()
        {
            FileLog.PrintF("SetRegRealAll stockCodeList.count=>" + stockCodeList.Count());
            String tmpStrFidList = "10";
            String strRealType = "0";
            List<String> strCodeList = new List<string>();
            int cnt = stockCodeList.Count();

            double tmp = cnt / 100f;
            double tmpCnt = Math.Ceiling(tmp);

            FileLog.PrintF("SetRegRealAll cnt=>" + cnt);
            FileLog.PrintF("SetRegRealAll tmp=>" + tmp);
            FileLog.PrintF("SetRegRealAll tmpCnt=>" + tmpCnt);

            for(int i = 1; i <=tmpCnt; i++)
            {
                int a = (i * 100)-100;
                int b = (i * 100);
                if (b > cnt)
                {
                    b = cnt%100-1;
                } else
                {
                    b = 100;
                }
                FileLog.PrintF(String.Format("SetRegRealAll a{0}~갯수{1}", a, b));
                List<String> tmpList = stockCodeList.GetRange(a, b);
                strCodeList.Add(string.Join(";", tmpList));
            }

            foreach (String strCode in strCodeList)
            {
                FileLog.PrintF(String.Format("SetRegRealAll strCode=>{0}", strCode));
                String tmpStrScreenNo = ScreenNumber.getClass1Instance().GetRealTimeScrNum();
                axKHOpenAPI.SetRealReg(tmpStrScreenNo, strCode, tmpStrFidList, strRealType);
                strRealType = "1";
            }                       
        }

        private List<String> setStockCodeListDb() {
            //디비에 있는걸 읽어오니.. 약간 문제가 있었다. elw가 가끔 키움에서 안가져오는것같다.
            List<String> tt = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr())) {
                //string sql = "SELECT STOCK_CODE FROM stocks WHERE ";
                String sql = "select distinct stock_code from stocks ta, market_stocks tb where ta.stock_code = tb.stock_code_id and market_code_id in (0,10) order by stock_code asc";
                //장내와 코스닥만 조회하도록; 기존에 ELW같은거 같이 조회했더니 키움에서 약간 문제가 있는듯하다.
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
