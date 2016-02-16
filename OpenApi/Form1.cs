using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using KiwoomCode;
using WcfServiceLibrary;
using OpenApi.ReceiveTrData;
using System.Threading;
using System.Collections;

namespace OpenApi
{
    public partial class Form1 : Form
    {
        private ServiceHost productsServiceHost;
        private Class1 class1;
        public Form1()
        {
            InitializeComponent();
            init();
            class1 = Class1.getClass1Instance();
            class1.setAxKHOpenAPIInstance(this.axKHOpenAPI);
        }
        private void init()
        {
            Dictionary<String, String> comboSource = new Dictionary<String, String>();
                   comboSource.Add("ACCOUNT_CNT", "전체계좌수");
                   comboSource.Add("ACCNO", "전체계좌");
                   comboSource.Add("USER_ID", "사용자ID");
                   comboSource.Add("USER_NAME", "사용자명");
                   comboSource.Add("KEY_BSECGB", "키보드보안 해지여부. 0:정상, 1:해지");
                   comboSource.Add("FIREW_SECGB", "방화벽 설정 여부. 0:미설정, 1:설정, 2:해지");

            cb_GetLoginInfo.DataSource = new BindingSource(comboSource, null);
            cb_GetLoginInfo.DisplayMember = "Value";
            cb_GetLoginInfo.ValueMember = "Key";
                        
            Dictionary<String, String> comboSource1 = new Dictionary<String, String>();
            comboSource1.Add("0", "장내");
            comboSource1.Add("3", "ELW");
            comboSource1.Add("4", "뮤추얼펀드");
            comboSource1.Add("5", "신주인수권");
            comboSource1.Add("6", "리츠");
            comboSource1.Add("8", "ETF");
            comboSource1.Add("9", "하이일드펀드");
            comboSource1.Add("10", "코스닥");
            comboSource1.Add("30", "제3시장");

            cb_GetCodeListByMarket.DataSource = new BindingSource(comboSource1, null);
            cb_GetCodeListByMarket.DisplayMember = "Value";
            cb_GetCodeListByMarket.ValueMember = "Key";
        }
        

        private void 로그인_Click(object sender, EventArgs e)
        {
            if (axKHOpenAPI.CommConnect() == 0)
            {
                Logger(Log.일반, "로그인창 열기 성공");
            }
            else
            {
                Logger(Log.에러, "로그인창 열기 실패");
            }
        }



        private void axKHOpenAPI_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveTrData===============================");
            ReceiveTrDataFactory rtf = new ReceiveTrDataFactory();
            rtf.getReceiveTrData(axKHOpenAPI, e);

            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveTrData===============================");
            
            int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);
            //FileLog.PrintF(String.Format("화면번호:{0} | RQName:{1} | TRCode:{2} | sRecordName:{3} |  sPreNext:{4}", e.sScrNo, e.sRQName, e.sTrCode, e.sRecordName, e.sPrevNext));

            /*
            sScrNo – 화면번호
            sRQName – 사용자구분 명
            sTrCode – Tran 명
            sRecordName – Record 명
            sPreNext – 연속조회 유무
            */
            if ("[0346]실시간계좌관리(T)-잔고확인".Equals(e.sRQName))
            {
                FileLog.PrintF("====================axKHOpenAPI_OnReceiveTrData===============================1");
                String tmp= axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, 0, "예수금").Trim();
                FileLog.PrintF("====================axKHOpenAPI_OnReceiveTrData===============================1:" + tmp);


            }


            if (e.sRQName == "주식주문")
            {
                string s원주문번호 = axKHOpenAPI.GetCommData(e.sTrCode, "", 0, "").Trim();

                long n원주문번호 = 0;
                bool canConvert = long.TryParse(s원주문번호, out n원주문번호);

                if (canConvert == true)
                Logger(Log.에러, s원주문번호);
                else
                    Logger(Log.에러, "잘못된 원주문번호 입니다");

            }
            // OPT1001 : 주식기본정보
            else if (e.sRQName == "주식기본정보")
            {
                 nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);

                for (int i = 0; i < nCnt; i++)
                {
                    Logger(Log.조회, "{0} | 현재가:{1:N0} | 등락율:{2} | 거래량:{3:N0} ",
                        axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목명").Trim(),
                        Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim()),
                        axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim(),
                        Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim()));
                }
            }
        }

        private void axKHOpenAPI_OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnEventConnect===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnEventConnect===============================");
            if (Error.IsError(e.nErrCode))
            {
                Logger(Log.일반, "[로그인 처리결과] " + Error.GetErrorMessage());
            }
            else
            {
                Logger(Log.에러, "[로그인 처리결과] " + Error.GetErrorMessage());
            }
        }

        private void axKHOpenAPI_OnReceiveChejanData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveChejanData===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveChejanData===============================");
            if (e.sGubun == "0")
            {
                Logger(Log.실시간, "구분 : 주문체결통보");
                Logger(Log.실시간, "주문/체결시간 : " + axKHOpenAPI.GetChejanData(908));
                Logger(Log.실시간, "종목명 : " + axKHOpenAPI.GetChejanData(302));
                Logger(Log.실시간, "주문수량 : " + axKHOpenAPI.GetChejanData(900));
                Logger(Log.실시간, "주문가격 : " + axKHOpenAPI.GetChejanData(901));
                Logger(Log.실시간, "체결수량 : " + axKHOpenAPI.GetChejanData(911));
                Logger(Log.실시간, "체결가격 : " + axKHOpenAPI.GetChejanData(910));
                Logger(Log.실시간, "=======================================");
            }
            else if (e.sGubun == "1")
            {
                Logger(Log.실시간, "구분 : 잔고통보");
            }
            else if (e.sGubun == "3")
            {
                Logger(Log.실시간, "구분 : 특이신호");
            }

        }

        private void axKHOpenAPI_OnReceiveMsg(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveMsg===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveMsg===============================");
            Logger(Log.조회, "화면번호:{0} | RQName:{1} | TRCode:{2} | 메세지:{3}", e.sScrNo, e.sRQName, e.sTrCode, e.sMsg);
            FileLog.PrintF(String.Format("화면번호:{0} | RQName:{1} | TRCode:{2} | 메세지:{3}", e.sScrNo, e.sRQName, e.sTrCode, e.sMsg)) ;
            
        }

        private void axKHOpenAPI_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveRealData===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveRealData===============================");
            Logger(Log.실시간, "종목코드 : {0} | RealType : {1} | RealData : {2}",
                e.sRealKey, e.sRealType, e.sRealData);
            FileLog.PrintF(String.Format("종목코드 : {0} | RealType : {1} | RealData : {2}", e.sRealKey, e.sRealType, e.sRealData));
            if (e.sRealType == "주식시세")
            {
                Logger(Log.실시간, "종목코드 : {0} | 현재가 : {1:C} | 등락율 : {2} | 누적거래량 : {3:N0} ",
                        e.sRealKey,
                        Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 10).Trim()),
                        axKHOpenAPI.GetCommRealData(e.sRealType, 12).Trim(),
                        Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 13).Trim()));
            }

        }

        // 로그를 출력합니다.
        public void Logger(Log type, string format, params Object[] args)
        {
            string message = String.Format(format, args);

            switch (type)
            {
                case Log.조회:
                    if (lst조회.InvokeRequired)
                    {
                        lst조회.BeginInvoke(new Action(() => { 
                            lst조회.Items.Add(message);
                            if (lst조회.Items.Count > 20)
                            {
                                lst조회.Items.Clear();
                            }
                            lst조회.SelectedIndex = lst조회.Items.Count - 1;
                        }));

                    } else {
                        lst조회.Items.Add(message);
                        if (lst조회.Items.Count > 20)
                        {
                            lst조회.Items.Clear();
                        }
                        lst조회.SelectedIndex = lst조회.Items.Count - 1;
                    }
                    break;
                case Log.에러:
                    if (lst에러.InvokeRequired)
                    {
                        lst에러.BeginInvoke(new Action(() =>
                        {
                            lst에러.Items.Add(message);
                            if (lst에러.Items.Count > 20)
                            {
                                lst에러.Items.Clear();
                            }
                            lst에러.SelectedIndex = lst에러.Items.Count - 1;
                        }));
                     
                    } else
                    {
                        lst에러.Items.Add(message);
                        if (lst에러.Items.Count > 20)
                        {
                            lst에러.Items.Clear();
                        }
                        lst에러.SelectedIndex = lst에러.Items.Count - 1;

                    }
                    break;

                case Log.일반:
                    if (lst일반.InvokeRequired)
                    {
                        lst일반.BeginInvoke(new Action(() =>
                        {
                            lst일반.Items.Add(message);
                            if (lst일반.Items.Count > 20)
                            {
                                lst일반.Items.Clear();
                            }
                            lst일반.SelectedIndex = lst일반.Items.Count - 1;
                        }));
                    } else
                    {
                        lst일반.Items.Add(message);
                        if (lst일반.Items.Count > 20)
                        {
                            lst일반.Items.Clear();
                        }
                        lst일반.SelectedIndex = lst일반.Items.Count - 1;
                    }

                        
                    break;
                case Log.실시간:
                    if (lst실시간.InvokeRequired)
                    {
                        lst실시간.BeginInvoke(new Action(() =>
                        {
                            lst실시간.Items.Add(message);
                            if (lst실시간.Items.Count > 20)
                            {
                                lst실시간.Items.Clear();
                            }
                            lst실시간.SelectedIndex = lst실시간.Items.Count - 1;
                        }));
                    } else
                    {
                        lst실시간.Items.Add(message);
                        if (lst실시간.Items.Count > 20)
                        {
                            lst실시간.Items.Clear();
                        }
                        lst실시간.SelectedIndex = lst실시간.Items.Count - 1;
                    }
                        
                    break;
                case Log.디버깅:
                    if (lst디버깅.InvokeRequired)
                    {
                        lst디버깅.BeginInvoke(new Action(() =>
                        {
                            lst디버깅.Items.Add(message);
                            if (lst디버깅.Items.Count > 20)
                            {
                                lst디버깅.Items.Clear();
                            }
                            lst디버깅.SelectedIndex = lst디버깅.Items.Count - 1;
                        }));                        
                    } else
                    {
                        lst디버깅.Items.Add(message);
                        if (lst디버깅.Items.Count > 20)
                        {
                            lst디버깅.Items.Clear();
                        }
                        lst디버깅.SelectedIndex = lst디버깅.Items.Count - 1;
                    }
                        
                    break;
                default:
                    break;
            }
        }

        private void axKHOpenAPI_OnReceiveRealCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveRealCondition===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveRealCondition===============================");
            Logger(Log.실시간, "========= 조건조회 실시간 편입/이탈 ==========");
            Logger(Log.실시간, "[종목코드] : " + e.sTrCode);
            Logger(Log.실시간, "[실시간타입] : " + e.strType);
            Logger(Log.실시간, "[조건명] : " + e.strConditionName);
            Logger(Log.실시간, "[조건명 인덱스] : " + e.strConditionIndex);
        }

        private void axKHOpenAPI_OnReceiveTrCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveTrCondition===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveTrCondition===============================");
            Logger(Log.조회, "[화면번호] : " + e.sScrNo);
            Logger(Log.조회, "[종목리스트] : " + e.strCodeList);
            Logger(Log.조회, "[조건명] : " + e.strConditionName);
            Logger(Log.조회, "[조건명 인덱스 ] : " + e.nIndex.ToString());
            Logger(Log.조회, "[연속조회] : " + e.nNext.ToString());
        }


        private void axKHOpenAPI_OnReceiveConditionVer(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveConditionVer===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveConditionVer===============================");
            if (e.lRet == 1)
            {
                Logger(Log.일반, "[이벤트] 조건식 저장 성공");
            }
            else
            {
                Logger(Log.에러, "[이벤트] 조건식 저장 실패 : " + e.sMsg);
            }

        }

        private void 로그아웃_Click(object sender, EventArgs e)
        {
            class1.DisconnectAllRealData();
            Logger(Log.일반, "로그아웃");

        }

        private void 접속상태_Click(object sender, EventArgs e)
        {
            int ret = axKHOpenAPI.GetConnectState();
                Logger(Log.일반, "Open API 연결 : "+ ret);
            if (ret == 0){
                Logger(Log.일반, "Open API 연결 : 미연결");
            }else{
                Logger(Log.일반, "Open API 연결 : 연결중");
            }

        }

        private void debugCommunicationState(CommunicationState ret)
        {
            if (ret == CommunicationState.Created) {
                Logger(Log.디버깅, "CommunicationState.Created 상태");
            } else if (ret == CommunicationState.Opening) {
                Logger(Log.디버깅, "CommunicationState.Opening 상태");
                //     통신 개체에서 전환 중인 것을 나타냅니다는 System.ServiceModel.CommunicationState.Opening 상태에 있는
            } else if (ret == CommunicationState.Opened) {
                Logger(Log.디버깅, "CommunicationState.Opened 상태");
                //     통신 개체가 현재 열려 있고 사용할 준비가 되었습니다. 임을 나타냅니다.
            } else if (ret == CommunicationState.Closing) {
                Logger(Log.디버깅, "CommunicationState.Closing 상태");
            } else if (ret == CommunicationState.Closed) {
                Logger(Log.디버깅, "CommunicationState.Closed 상태");
                //     통신 개체가 닫혀 서 더 이상 사용할 수 없습니다 것을 나타냅니다.
            } else if (ret == CommunicationState.Faulted) {
                Logger(Log.디버깅, "CommunicationState.Faulted 상태");
                //     장애는 더 이상 사용할 수 없습니다와에서 복구할 수 없는 오류 또는 통신 개체를 발견 했음을 나타냅니다.
            }
        }

        // procedure to run on a new thread
        private void StartService()
        {

            if (productsServiceHost == null)
            {

                if (axKHOpenAPI != null)
                {
                    productsServiceHost = new ServiceHost(typeof(KiwoomOpenApiService));
                    productsServiceHost.Faulted += new EventHandler(wcfFaultHandler);
                    productsServiceHost.UnknownMessageReceived += new EventHandler<UnknownMessageReceivedEventArgs>(wcfUnknownMessageReceived);
                }
                else {
                    Logger(Log.디버깅, "WCF_ON : 로그인을 하지 않았습니다. 먼저 로그인을 해주세요");
                }

            }
            else {
                Logger(Log.디버깅, "WCF_ON : WCF 객체가 이미 생성되었음.");
            }
            CommunicationState ret = productsServiceHost.State;
            if (ret == CommunicationState.Faulted || ret == CommunicationState.Closed || ret == CommunicationState.Created)
            {
                productsServiceHost.Open();
                Logger(Log.디버깅, "WCF_ON: WCF 객체.OPEN  실행");
            }
            else {
                Logger(Log.디버깅, "WCF_ON: WCF 객체.OPEN 열려있는 상태입니다.");
            }
        }
        Thread t;
        private void WCF_ON_Click(object sender, EventArgs e)
        {
            // starting the thread
            if(t==null) { 
                t = new Thread(StartService);
                t.Start();
            }
        }

        private void WCF_OFF_Click(object sender, EventArgs e)
        {
            if (productsServiceHost != null)
            {
                productsServiceHost.Close();
                Logger(Log.디버깅, "WCF_OFF:호출");
            } else
            {
                Logger(Log.디버깅, "WCF_OFF:wcf 객체 null");
            }
            
        }

        private void WCF상태_Click(object sender, EventArgs e)
        {
            if (productsServiceHost != null) { 
                CommunicationState ret = productsServiceHost.State;
                debugCommunicationState(ret);
            } else {
                Logger(Log.디버깅, "WCF상태:wcf 객체 null");
            }

        }

        private void GetLoginInfo_Click(object sender, EventArgs e)
        {
            String sTag =cb_GetLoginInfo.SelectedValue.ToString();
            Logger(Log.디버깅, "cb_GetLoginIn.Value:"+ sTag);
            String tTag = cb_GetLoginInfo.Text;
            Logger(Log.디버깅, "cb_GetLoginIn.Text:" + tTag);
            if (sTag.Equals(""))
            {
                MessageBox.Show("사용자정보를 반환할 키값을 선택해주세요");
                return;
            }
            String ret=  axKHOpenAPI.GetLoginInfo(sTag);
            Logger(Log.디버깅, "GetLoginIn[" + sTag+"]:"+ ret);
            if(sTag.Equals("ACCNO"))
            {
                tx_account.Text = ret;
            }
        }

        private void GetAPIModulePath_Click(object sender, EventArgs e)
        {
            /*OpenAPI모듈의 경로를 반환한다.*/
            String ret= axKHOpenAPI.GetAPIModulePath();
            Logger(Log.디버깅, "GetAPIModulePath:" + ret);
        }

        private void GetCodeListByMarket_Click(object sender, EventArgs e)
        {
            String sTag = cb_GetCodeListByMarket.SelectedValue.ToString();
            Logger(Log.디버깅, "cb_GetCodeListByMarket.Value:" + sTag);
            String tTag = cb_GetCodeListByMarket.Text;
            Logger(Log.디버깅, "cb_GetCodeListByMarket.Text:" + tTag);
            if (sTag.Equals(""))
            {
                MessageBox.Show("시장구분을 선택해주세요");
                return;
            }
            String ret = axKHOpenAPI.GetCodeListByMarket(sTag);
            Logger(Log.디버깅, "GetCodeListByMarket[" + sTag + "]:" + ret);
        }
        private void wcfFaultHandler(object sender,EventArgs e)
        {

        }

        private void wcfUnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            FileLog.PrintF(e.Message.ToString());
        }

        private void GetBalance_Click(object sender, EventArgs e)
        {
            String account = tx_account.Text;
            String password = tx_password.Text;

            Logger(Log.디버깅, "tx_account.Text:" + account);
            Logger(Log.디버깅, "tx_password.Text:" + password);
            if (account.Equals(""))
            {
                MessageBox.Show("계좌번호를 입력해주세요.");
                return;
            }

            if (password.Equals(""))
            {
                MessageBox.Show("이 패스워드는 의미 없음 하단에 트레이아이콘에서 설정을 눌러서 비밀번호를 저장해야함.");
                return;
            }

            axKHOpenAPI.SetInputValue("계좌번호", account.Trim());
            axKHOpenAPI.SetInputValue("비밀번호", password.Trim());
            axKHOpenAPI.SetInputValue("상장폐지조회구분"    , "0");
            axKHOpenAPI.SetInputValue("비밀번호입력매체구분", "00");
 //int nRet = axKHOpenAPI.CommRqData("RQName", "OPW00004", 0, "0200");
            int nRet = axKHOpenAPI.CommRqData("[0346]실시간계좌관리(T)-잔고확인", "OPW00004", 0, "화면번호");
            if (Error.IsError(nRet))
            {
                Logger(Log.일반, "[OPW00004] : " + Error.GetErrorMessage());
            }
            else
            {
                Logger(Log.에러, "[OPW00004] : " + Error.GetErrorMessage());
            }
            Logger(Log.디버깅, "GetBalance_Click::CommRqData::ret:" + nRet);
            
        }

        private void lst조회_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender != lst조회) return;

            if (e.Control && e.KeyCode == Keys.C) 
                CopySelectedValuesToClipboard();
        }

        private void CopySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (ListViewItem item in lst조회.SelectedItems)
                builder.AppendLine(item.SubItems[1].Text);

            Clipboard.SetText(builder.ToString());
        }


        private void threadJob(OpenApi.Spell.spellOpt spellOpt)
        {
            List<String> stockCodeList = Class1.getClass1Instance().getStockCodeList();
            for (int i = 0; i < stockCodeList.Count; i++)
            {
                OpenApi.Spell.spellOpt tmp= spellOpt.ShallowCopy();
                String stockCode = stockCodeList[i];
                String sScrNo = Class1.getClass1Instance().GetScrNum();
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

              //  FileLog.PrintF("threadJob keyStockCode="+ keyStockCode);
              //  FileLog.PrintF("threadJob key=" + key);

                //String logDate = DateTime.Today.ToString("yyyyMMdd");
                
                String path = tmp.GetFileName();
                if (!System.IO.File.Exists(path))
                {
                    Class1.getClass1Instance().EnqueueByOrderQueue(tmp);
                    Class1.getClass1Instance().AddSpellDictionary(key, tmp);
                    Class1.getClass1Instance().AddStockCodeDictionary(keyStockCode, stockCode);
                }

                

            }
        }

        private void GetDayStock_Click(object sender, EventArgs e)
        {
            String startDate = tx_startDate.Text;
            String endDate = tx_endDate.Text;
            String sTrCode = "OPT10081";
            String sRQName = "주식일봉차트조회";

            OpenApi.Spell.spellOpt spellOpt10081 = new OpenApi.Spell.spellOpt();
            spellOpt10081.sRQNAME = sRQName;
            spellOpt10081.sTrCode = sTrCode;
            //spellOpt10081.stockCode = 종목코드;
            spellOpt10081.startDate = startDate;
            spellOpt10081.endDate = endDate;
            spellOpt10081.nPrevNext = 0;
            spellOpt10081.modifyGubun = "1";
            //spellOpt10081.sScreenNo = sScrNo;

            String 종목코드 = tx_stockCode.Text.Trim();
            if (종목코드.Equals("ALL"))
            {
                threadJob(spellOpt10081);
                FileLog.PrintF("GetDayStock_Click threadJob");

            }  else
            {
                String sScrNo = Class1.getClass1Instance().GetScrNum();
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , sRQName
                    , sTrCode
                    , sScrNo
                );
                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                   , sRQName
                    , sTrCode
                    , sScrNo
                    , 종목코드
                );
                FileLog.PrintF("keyStockCode  ==" + keyStockCode);

                axKHOpenAPI.SetInputValue("종목코드", 종목코드);
                axKHOpenAPI.SetInputValue("기준일자", endDate);
                axKHOpenAPI.SetInputValue("수정주가구분", "1");


                int nRet = axKHOpenAPI.CommRqData(sRQName, sTrCode, 0, sScrNo);

                if (Error.IsError(nRet))
                {
                    Logger(Log.일반, "[OPT10081] : " + Error.GetErrorMessage());
                }
                else
                {
                    Logger(Log.에러, "[OPT10081] : " + Error.GetErrorMessage());
                }
            }    
        }

        private void GetStockOrgan_Click(object sender, EventArgs e)
        {
            string 매매구분 = null;
            foreach (Control control in this.매매구분.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        매매구분 = radio.Tag.ToString();
                    }
                }
            }
            // Search second GroupBox.
            string 금액수량구분 = null;
            foreach (Control control in this.금액수량구분.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        금액수량구분 = radio.Tag.ToString();
                    }
                }
            }
            String stockCode=this.tx_stockCode_1.Text.ToString();
            String startDate = this.tx_startDate_1.Text.ToString();
            String endDate = this.tx_endDate_1.Text.ToString();
            String sRQName = "종목별투자자기관별차트요청";
            String sTrCode = "OPT10059";
            String 단위구분 = "1";


            FileLog.PrintF("GetStockOrgan_Click stockCode=" + stockCode);
            FileLog.PrintF("GetStockOrgan_Click startDate=" + startDate);
            FileLog.PrintF("GetStockOrgan_Click endDate=" + endDate);
            FileLog.PrintF("GetStockOrgan_Click 매매구분="+ 매매구분);
            FileLog.PrintF("GetStockOrgan_Click 금액수량구분=" + 금액수량구분);
            FileLog.PrintF("GetStockOrgan_Click 단위구분=" + 단위구분);

            OpenApi.Spell.spellOpt spellOpt10060 = new OpenApi.Spell.spellOpt();
            spellOpt10060.sRQNAME = sRQName;
            spellOpt10060.sTrCode = sTrCode;
            //spellOpt10060.stockCode = stockCode;
            spellOpt10060.startDate = startDate;
            spellOpt10060.endDate = endDate;
            spellOpt10060.nPrevNext = 0;
            spellOpt10060.priceOrAmount = 금액수량구분;
            spellOpt10060.buyOrSell = 매매구분;
            //spellOpt10060.sScreenNo = sScrNo;




            if (stockCode.Equals("ALL"))
            {
                threadJob(spellOpt10060);
                FileLog.PrintF("GetStockOrgan_Click threadJob");
            }
            else
            {
                String sScrNo = Class1.getClass1Instance().GetScrNum();
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , sRQName
                    , sTrCode
                    , sScrNo
                );
                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                   , sRQName
                    , sTrCode
                    , sScrNo
                    , stockCode
                );
                FileLog.PrintF("keyStockCode  ==" + keyStockCode);
                /*일자 = YYYYMMDD(20160101 연도4자리, 월 2자리, 일 2자리 형식)*/
                axKHOpenAPI.SetInputValue("일자", endDate);
                /*종목코드 = 전문 조회할 종목코드*/
                axKHOpenAPI.SetInputValue("종목코드", stockCode);
                /*금액수량구분 = 1:금액, 2:수량*/
                axKHOpenAPI.SetInputValue("금액수량구분"  , 금액수량구분);
                /*매매구분 = 0:순매수, 1:매수, 2:매도*/
                axKHOpenAPI.SetInputValue("매매구분"    , 매매구분);
                /*단위구분 = 1000:천주, 1:단주*/
                axKHOpenAPI.SetInputValue("단위구분"    , "1");


                int nRet = axKHOpenAPI.CommRqData(sRQName, sTrCode, 0, sScrNo);

                if (Error.IsError(nRet))
                {
                    Logger(Log.일반, "[OPT10060] : " + Error.GetErrorMessage());
                }
                else
                {
                    Logger(Log.에러, "[OPT10060] : " + Error.GetErrorMessage());
                }
            }


        }
    }

}
