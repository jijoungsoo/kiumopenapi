using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.ReceiveTrData
{
    public class OPT10015 : ReceiveTrData
    {
        /*
        [OPT10015: 일별거래상세요청]
        1. Open API 조회 함수 입력값을 설정합니다.
        종목코드 = 전문 조회할 종목코드
        SetInputValue("종목코드"	,  "입력값 1");
        시작일자 = YYYYMMDD(20160101 연도4자리, 월 2자리, 일 2자리 형식)
        SetInputValue("시작일자"	,  "입력값 2");
        2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
        CommRqData( "RQName"	,  "OPT10015"	,  "0"	,  "화면번호");
    */
        private static string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private static Dictionary<string, int> clearFlag = new Dictionary<string, int>();
        public OPT10015(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {


        }
    }
}