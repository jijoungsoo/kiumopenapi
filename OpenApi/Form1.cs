using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KiwoomCode;

namespace OpenApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void 로그인_Click(object sender, EventArgs e)
        {
            if (axKHOpenAPI1.CommConnect() == 0)
            {
                Console.WriteLine("로그인");
            }
        }



        private void axKHOpenAPI_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {

            if (e.sRQName == "주식주문")
            {
                string s원주문번호 = axKHOpenAPI1.GetCommData(e.sTrCode, "", 0, "").Trim();

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
                int nCnt = axKHOpenAPI1.GetRepeatCnt(e.sTrCode, e.sRQName);

                for (int i = 0; i < nCnt; i++)
                {
                    Logger(Log.조회, "{0} | 현재가:{1:N0} | 등락율:{2} | 거래량:{3:N0} ",
                        axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, i, "종목명").Trim(),
                        Int32.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim()),
                        axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim(),
                        Int32.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim()));
                }
            }
            // OPT10081 : 주식일봉차트조회
            else if (e.sRQName == "주식일봉차트조회")
            {
                int nCnt = axKHOpenAPI1.GetRepeatCnt(e.sTrCode, e.sRQName);

                for (int i = 0; i < nCnt; i++)
                {
                    Logger(Log.조회, "{0} | 현재가:{1:N0} | 거래량:{2:N0} | 시가:{3:N0} | 고가:{4:N0} | 저가:{5:N0} ",
                        axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim(),
                        Int32.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim()),
                        Int32.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim()),
                        Int32.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, i, "시가").Trim()),
                        Int32.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, i, "고가").Trim()),
                        Int32.Parse(axKHOpenAPI1.CommGetData(e.sTrCode, "", e.sRQName, i, "저가").Trim()));
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
                Logger(Log.실시간, "주문/체결시간 : " + axKHOpenAPI1.GetChejanData(908));
                Logger(Log.실시간, "종목명 : " + axKHOpenAPI1.GetChejanData(302));
                Logger(Log.실시간, "주문수량 : " + axKHOpenAPI1.GetChejanData(900));
                Logger(Log.실시간, "주문가격 : " + axKHOpenAPI1.GetChejanData(901));
                Logger(Log.실시간, "체결수량 : " + axKHOpenAPI1.GetChejanData(911));
                Logger(Log.실시간, "체결가격 : " + axKHOpenAPI1.GetChejanData(910));
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
                        Int32.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 10).Trim()),
                        axKHOpenAPI1.GetCommRealData(e.sRealType, 12).Trim(),
                        Int32.Parse(axKHOpenAPI1.GetCommRealData(e.sRealType, 13).Trim()));
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

    }
}
