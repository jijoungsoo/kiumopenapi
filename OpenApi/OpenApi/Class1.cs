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

    public class Class1
    {
        private static Boolean _multiThread = true;
        private int _scrNum = 5000;
        private int _scrNumAnyTime = 1000;
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
        


        string ftpUser =   "jijs";
        string ftpPwd = Environment.GetEnvironmentVariable("FTP_PASSWORD");
        public static string connStr = "Server=192.168.0.30;Database=stockWeb_development;Uid=root;Pwd="+ Environment.GetEnvironmentVariable("FTP_PASSWORD") + ";CHARSET=utf8";

        public int iEOS = 0;
        public string endDateEos = DateTime.Now.ToString("yyyyMMdd");

        private static string path = System.Reflection.Assembly.GetExecutingAssembly().Location;


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
            axKHOpenAPI.Dispose();
            //axKHOpenAPI.CommTerminate();
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

        }

        private Class1()
        {
            path = System.IO.Path.GetDirectoryName(path) + "\\";
            t1 = new Thread(new ThreadStart(sendMessageReceived));
            t2 = new Thread(new ThreadStart(orderReceivedMessage));
            t3 = new Thread(new ThreadStart(EOS_CompressZip));
            t4 = new Thread(new ThreadStart(UploadEosFiles));
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
                FileLog.PrintF("AddSpellDictionary key=>" + key);
                if (!spellDictionary.ContainsKey(key)) {
                    spellDictionary.Add(key, value);
                }
            }
        }

        public  OpenApi.Spell.SpellOpt getSpell(String key)
        {
            //'System.Collections.Generic.KeyNotFoundException'
            lock(lockerSpellDictionary) {
                FileLog.PrintF("getSpell key=>" + key);
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
                    FileLog.PrintF("AddStockCodeDictionary  key:" + key);
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
            //'System.Collections.Generic.KeyNotFoundException'
            //  remove를 하면 안되겠다. 로그를 찍어보니 중복으로 나오는경우가 있다.
            // 보내는쪽에서는 중복으로 보내는게 없는데..
            // 뭔가 에러창이 뜨는것이 있는데.. 그게 뜨면 다시 읽기를 시도하는것같다 kiwoom ocx가.
            // 가져가면서 지우는것은 하면 안되겠다..
            lock(lockerStockDictionary) {
                FileLog.PrintF("getStockCode  get key:" + key);
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
                            String path = tmp.GetFileName();
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
                        String tmpPath = prevTmp.GetCheckZipFileName();
                        System.IO.StreamWriter tmpFile = new System.IO.StreamWriter(tmpPath, true);
                        tmpFile.Write(endDateEos);
                        tmpFile.Close();
                    } else if (prevTmp != null && prevTmp.sTrCode.Equals("OPT10059") && prevTmp.sTrCode.Equals(tmp.sTrCode) 
                                && 
                               ! (prevTmp.priceOrAmount + "_"+ prevTmp.buyOrSell).Equals((tmp.priceOrAmount + "_"+tmp.buyOrSell))
                    )
                    {
                        String tmpPath = prevTmp.GetCheckZipFileName();
                        System.IO.StreamWriter tmpFile = new System.IO.StreamWriter(tmpPath, true);
                        tmpFile.Write(endDateEos);
                        tmpFile.Close();
                    }
                    
                    String path = tmp.GetFileName();
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
                        String tmpPath = tmp.GetCheckZipFileName();
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

        public void EOS_CompressZip()
        {
            while(true) {
                FileLog.PrintF("EOS_CompressZip");
                //1. 주식일봉차트조회
                string zipPath = path + "OPT10081_" + endDateEos + ".zip";
                string zipCheckPath = path + "OPT10081.dat";
                string ftpCheckPath = path + "OPT10081_FTP.dat";
                if (File.Exists(zipPath)==false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10081 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10081 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10081_", "");
                        DeleteFile(zipPath, "OPT10081_", "");
                        File.Move(zipCheckPath,ftpCheckPath);
                    }
                }
                //2. 일별거래상세요청
                zipPath = path + "OPT10015_" + endDateEos + ".zip";
                zipCheckPath = path + "OPT10015.dat";
                ftpCheckPath = path + "OPT10015_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10015 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10015 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10015_", "");
                        DeleteFile(zipPath, "OPT10015_", "");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                //3. 종목별투자자기관별차트
                zipPath = path + "OPT10059_" + endDateEos + "_1_1.zip";
                zipCheckPath = path + "OPT10059_1_1.dat";
                ftpCheckPath = path + "OPT10059_1_1_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10059_1_1 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10059_1_1 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10059_", "_1_1");
                        DeleteFile(zipPath, "OPT10059_", "_1_1");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                zipPath = path + "OPT10059_" + endDateEos + "_1_2.zip";
                zipCheckPath = path + "OPT10059_1_2.dat";
                ftpCheckPath = path + "OPT10059_1_2_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10059_1_2 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10059_1_2 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10059_", "_1_2");
                        DeleteFile(zipPath, "OPT10059_", "_1_2");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                zipPath = path + "OPT10059_" + endDateEos + "_2_1.zip";
                zipCheckPath = path + "OPT10059_2_1.dat";
                ftpCheckPath = path + "OPT10059_2_1_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10059_2_1 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10059_2_1 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10059_", "_2_1");
                        DeleteFile(zipPath, "OPT10059_", "_2_1");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                zipPath = path + "OPT10059_" + endDateEos + "_2_2.zip";
                zipCheckPath = path + "OPT10059_2_2.dat";
                ftpCheckPath = path + "OPT10059_2_2_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10059_2_2 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10059_2_2 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10059_", "_2_2");
                        DeleteFile(zipPath, "OPT10059_", "_2_2");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                //4. 공매도추이요청
                zipPath = path + "OPT10014_" + endDateEos + ".zip";
                zipCheckPath = path + "OPT10014.dat";
                ftpCheckPath = path + "OPT10014_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10014 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10014 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10014_", "");
                        DeleteFile(zipPath, "OPT10014_", "");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }

                //5. 주식일봉차트조회요청
                zipPath = path + "OPT10080_" + endDateEos + ".zip";
                zipCheckPath = path + "OPT10080.dat";
                ftpCheckPath = path + "OPT10080_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10080 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10080 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10080_", "");
                        DeleteFile(zipPath, "OPT10080_", "");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }

                //5. 주식일봉차트조회요청
                zipPath = path + "OPT10001_" + endDateEos + ".zip";
                zipCheckPath = path + "OPT10001.dat";
                ftpCheckPath = path + "OPT10001_FTP.dat";
                if (File.Exists(zipPath) == false)
                {
                    if (File.Exists(zipCheckPath))
                    {
                        FileLog.PrintF("EOS_CompressZip OPT10001 zipPath=" + zipPath);
                        FileLog.PrintF("EOS_CompressZip OPT10001 zipCheckPath=" + zipCheckPath);
                        CompressZip(zipPath, "OPT10001_", "");
                        DeleteFile(zipPath, "OPT10001_", "");
                        File.Move(zipCheckPath, ftpCheckPath);
                    }
                }
                Thread.Sleep(300000);//5분이면 압축이 되겠지???
            }
        }

        private void  DeleteFile(string zipPath, string startsWith, string endsWith)
        {
            FileLog.PrintF("DeleteFile zipPath=" + zipPath);
            FileLog.PrintF("DeleteFile startsWith=" + startsWith);
            FileLog.PrintF("DeleteFile endsWith=" + endsWith);
            DirectoryInfo directorySelected = new DirectoryInfo(path);
            //Directory.GetFiles(path,"*.txt",SearchOption.TopDirectoryOnly).Where(s=>s.StartsWith(startsWith));
            FileInfo[] fileList = directorySelected.GetFiles(startsWith + "*" + endsWith + ".txt");
            if (fileList.Count() > 0)
            {
                foreach (FileInfo file in fileList)
                {
                    file.Delete();
                }
                
            }

        }

        private void UploadEosFiles()
        {
            while(true) {
                FileLog.PrintF("UploadEosFiles");
                uploadZipFiles("OPT10015_FTP.dat", "OPT10015","");
                uploadZipFiles("OPT10059_1_1_FTP.dat", "OPT10059", "_1_1");
                uploadZipFiles("OPT10059_1_2_FTP.dat", "OPT10059", "_1_2");
                uploadZipFiles("OPT10059_2_1_FTP.dat", "OPT10059", "_2_1");
                uploadZipFiles("OPT10059_2_2_FTP.dat", "OPT10059", "_2_2");
                uploadZipFiles("OPT10014_FTP.dat", "OPT10014", "");
                uploadZipFiles("OPT10080_FTP.dat", "OPT10080", "");
                uploadZipFiles("OPT10081_FTP.dat", "OPT10081", "");
                uploadZipFiles("OPT10001_FTP.dat", "OPT10001", "");
                Thread.Sleep(300000); // 5분에 한번씩 검사해서 ftp 업로드
            }
        }

        private void uploadZipFiles(String ftpCheckFileName, string startsWithZip, string endsWithZip)
        {
            FtpUtil ftpUtil = new FtpUtil("192.168.0.30", ftpUser, ftpPwd, "21");
            DirectoryInfo directorySelected = new DirectoryInfo(path);
            String ftpCheckPath = path + ftpCheckFileName;
            FileInfo checkFile = new FileInfo(ftpCheckPath);
            FileLog.PrintF("uploadZipFiles=> " + ftpCheckPath);
            if (File.Exists(ftpCheckPath) == true)
            {
                FileInfo[] fileList = directorySelected.GetFiles(startsWithZip + "*" + endsWithZip + ".zip");
                foreach (FileInfo file in fileList)
                {
                    bool ret1 = ftpUtil.Upload("/home/jijs/rubytest/mysqloader/BACKUP", file.FullName);
                    FileLog.PrintF("UploadEosFiles ftpUtil.Upload=>" + file.Name + "_" + ret1.ToString());
                    if (ret1 == true)
                    {
                        File.Move(file.FullName, path + "BACKUP\\" + file.Name);
                    }
                }
                bool ret2 = ftpUtil.Upload("/home/jijs/rubytest/mysqloader/BACKUP", checkFile.FullName);
                FileLog.PrintF("UploadEosFiles ftpUtil.Upload=>" + checkFile.Name + "_" + ret2.ToString());
                if (ret2 == true)
                {
                    File.Delete(checkFile.FullName);
                }
            }
        }
        

        public void PushFtpZipFiles()
        {
            DirectoryInfo directorySelected = new DirectoryInfo(path);
            FileInfo[] fileList = directorySelected.GetFiles("*.zip");
            FtpUtil ftpUtil = new FtpUtil("192.168.0.30", ftpUser, ftpPwd, "21");
            /*FTP 서버에 있는 파일들 읽어오기
            String[] test = ftpUtil.GetFileList("/home/jijs/rubytest/mysqloader/BACKUP/");
            for (int i = 0; i < test.Length; i++)
            {
                Console.WriteLine(test[i]);
            }
            */
            /*
            //파일 다운로드 테스트.
            String dirPath = System.IO.Path.GetDirectoryName(path);
            String tmp = dirPath + "\\OPT10081_20160208.zip";
            Console.WriteLine("dirPath =" + dirPath);
            Console.WriteLine("tmp =" + tmp);
            bool ret = ftpUtil.Download(tmp, "/home/jijs/rubytest/mysqloader/data/OPT10081_20160208.zip");
            Console.WriteLine("ftpUtil.Download ret =" + ret);
            */

            /*
            //파일 업로드 테스트.
            String dirPath1 = System.IO.Path.GetDirectoryName(path);
            String tmp1 = dirPath + "\\OPT10081_000020_20160209.log";
            Console.WriteLine("dirPath1 =" + dirPath1);
            Console.WriteLine("tmp1 =" + tmp1);
            //  String fileName= Path.GetFileName(tmp1);
            //Console.WriteLine("ftpUtil.Upload fileName =" + fileName);
            bool ret1 = ftpUtil.Upload("/home/jijs/rubytest/mysqloader/data", tmp1);
            Console.WriteLine("ftpUtil.Upload ret1 =" + ret1);
            */

            foreach (FileInfo file in fileList)
            {
                bool ret1 = ftpUtil.Upload("/home/jijs/rubytest/mysqloader/BACKUP", file.FullName);
                FileLog.PrintF("PushFtpZipFiles ftpUtil.Upload file.Name_ret1=" + file.Name +"_"+ret1.ToString());
            }

            
        }

        private void CompressZip(string zipPath,string startsWith,string endsWith)
        {
            DirectoryInfo directorySelected = new DirectoryInfo(path);
            //Directory.GetFiles(path,"*.txt",SearchOption.TopDirectoryOnly).Where(s=>s.StartsWith(startsWith));
            FileInfo[] fileList= directorySelected.GetFiles(startsWith + "*" + endsWith + ".txt");
            
            
            if (fileList.Count() > 0 && getStockCodeList().Count() <= fileList.Count())
            {
                //카운트가 맞을때만 압축하는것도 좋은 예지만 이것만으로는 TEXT파일에 끝까지데이터가 있다는걸 보장을 할수 없다.

                FileLog.PrintF("CompressZip zipPath=" + zipPath);
                FileLog.PrintF("CompressZip startsWith=" + startsWith);
                FileLog.PrintF("CompressZip endsWith=" + endsWith);
                using (FileStream zipToCreate = new FileStream(zipPath, FileMode.Create))
                {
                    using (ZipArchive archive = new ZipArchive(zipToCreate, ZipArchiveMode.Create))
                    {
                        foreach (FileInfo fileToCompress in fileList)
                        {
                            archive.CreateEntryFromFile(fileToCompress.FullName, fileToCompress.Name);
                        }
                    }
                }
                
            }
            
        }

        private void DecompressZip(string zipPath,string extractPath,string startsWith)
        {
            //*이 메소드는 쓸일이 없긴한데..*/
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (entry.FullName.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase)) //startsWith에 해당하는것만 압축이 풀리도록
                    {
                        entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
                    }
                }
            }
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
        private static Object _object1  = new Object();
        public static Class1 getClass1Instance() {
            if (_multiThread == false) {
                if (_class1 == null) {
                    _class1 = new Class1();
                }               
            }else{
                if (_class1 == null) {
                    lock (_object1) {
                        _class1 = new Class1();
                    }
                }                
            }
            return _class1;
        }


        // EOS 연결 종료
        public void DisconnectAllEOSData() {
            FileLog.PrintF("DisconnectAllEOSData");
            for (int i = _scrNum; i > 5000; i--) {
                axKHOpenAPI.DisconnectRealData(i.ToString());
            }
            _scrNum = 5000;
            axKHOpenAPI.CommTerminate();
        }


        //EOS 화면번호 생산
        public string GetEosScrNum() {
            if (_scrNum < 9999)
                _scrNum++;
            else
                _scrNum = 5000;

            return _scrNum.ToString();
        }


        // 실시간 연결 종료
        public void DisconnectAllAnyTimeData()
        {
            FileLog.PrintF("DisconnectAllAnyTimeData");
            for (int i = _scrNumAnyTime; i > 1000; i--)
            {
                axKHOpenAPI.DisconnectRealData(i.ToString());
            }
            _scrNumAnyTime = 1000;
            axKHOpenAPI.CommTerminate();
        }

        // 화면번호 생산
        public string GetAnyTimeScrNum()
        {
            if (_scrNumAnyTime < 5000)
                _scrNumAnyTime++;
            else
                _scrNumAnyTime = 1000;

            return _scrNumAnyTime.ToString();
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
            return ttt;
        }

        private List<String> setStockCodeListDb() {
            //디비에 있는걸 읽어오니.. 약간 문제가 있었다. elw가 가끔 키움에서 안가져오는것같다.
            List<String> tt = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connStr)) {
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
