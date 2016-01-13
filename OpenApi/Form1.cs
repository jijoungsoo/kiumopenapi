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
                int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);

                for (int i = 0; i < nCnt; i++)
                {
                    Logger(Log.조회, "{0} | 현재가:{1:N0} | 등락율:{2} | 거래량:{3:N0} ",
                        axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목명").Trim(),
                        Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim()),
                        axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim(),
                        Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim()));
                }
            }
            // OPT10081 : 주식일봉차트조회
            else if (e.sRQName == "주식일봉차트조회")
            {
                int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);

                for (int i = 0; i < nCnt; i++)
                {
                    Logger(Log.조회, "{0} | 현재가:{1:N0} | 거래량:{2:N0} | 시가:{3:N0} | 고가:{4:N0} | 저가:{5:N0} ",
                        axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim(),
                        Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim()),
                        Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim()),
                        Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시가").Trim()),
                        Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "고가").Trim()),
                        Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "저가").Trim()));
                }
            }

        }

        private void axKHOpenAPI_OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
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
            Logger(Log.조회, "===================================================");
            Logger(Log.조회, "화면번호:{0} | RQName:{1} | TRCode:{2} | 메세지:{3}", e.sScrNo, e.sRQName, e.sTrCode, e.sMsg);
        }

        private void axKHOpenAPI_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            Logger(Log.실시간, "종목코드 : {0} | RealType : {1} | RealData : {2}",
                e.sRealKey, e.sRealType, e.sRealData);

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
                    lst조회.Items.Add(message);
                    lst조회.SelectedIndex = lst조회.Items.Count - 1;
                    break;
                case Log.에러:
                    lst에러.Items.Add(message);
                    lst에러.SelectedIndex = lst에러.Items.Count - 1;
                    break;
                case Log.일반:
                    lst일반.Items.Add(message);
                    lst일반.SelectedIndex = lst일반.Items.Count - 1;
                    break;
                case Log.실시간:
                    lst실시간.Items.Add(message);
                    lst실시간.SelectedIndex = lst실시간.Items.Count - 1;
                    break;
                case Log.디버깅:
                    lst디버깅.Items.Add(message);
                    lst디버깅.SelectedIndex = lst실시간.Items.Count - 1;
                    break;
                default:
                    break;
            }
        }

        private void axKHOpenAPI_OnReceiveRealCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            Logger(Log.실시간, "========= 조건조회 실시간 편입/이탈 ==========");
            Logger(Log.실시간, "[종목코드] : " + e.sTrCode);
            Logger(Log.실시간, "[실시간타입] : " + e.strType);
            Logger(Log.실시간, "[조건명] : " + e.strConditionName);
            Logger(Log.실시간, "[조건명 인덱스] : " + e.strConditionIndex);
        }

        private void axKHOpenAPI_OnReceiveTrCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            Logger(Log.조회, "[화면번호] : " + e.sScrNo);
            Logger(Log.조회, "[종목리스트] : " + e.strCodeList);
            Logger(Log.조회, "[조건명] : " + e.strConditionName);
            Logger(Log.조회, "[조건명 인덱스 ] : " + e.nIndex.ToString());
            Logger(Log.조회, "[연속조회] : " + e.nNext.ToString());
        }


        private void axKHOpenAPI_OnReceiveConditionVer(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
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

        private void WCF_ON_Click(object sender, EventArgs e)
        {
            if(productsServiceHost==null) {

                if (axKHOpenAPI != null) {
                   productsServiceHost = new ServiceHost(typeof(KiwoomOpenApiService));
                    productsServiceHost.Faulted += new EventHandler(wcfFaultHandler);
                    productsServiceHost.UnknownMessageReceived += new EventHandler<UnknownMessageReceivedEventArgs>(wcfUnknownMessageReceived);
                    
                } else {
                    Logger(Log.디버깅, "WCF_ON : 로그인을 하지 않았습니다. 먼저 로그인을 해주세요");
                }
               
            } else {
                Logger(Log.디버깅, "WCF_ON : WCF 객체가 이미 생성되었음.");
            }
            CommunicationState ret = productsServiceHost.State;
            if (ret == CommunicationState.Faulted || ret == CommunicationState.Closed || ret == CommunicationState.Created) {
                productsServiceHost.Open();
                Logger(Log.디버깅, "WCF_ON: WCF 객체.OPEN  실행");
            } else             {
                Logger(Log.디버깅, "WCF_ON: WCF 객체.OPEN 열려있는 상태입니다.");
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

        
    }
}
