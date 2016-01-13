using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using OpenApi;
using AxKHOpenAPILib;

namespace WcfServiceLibrary
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 클래스 이름 "Service1"을 변경할 수 있습니다.
    public class KiwoomOpenApiService : IKiwoomOpenApi
    {
        private Class1 class2 = Class1.getClass1Instance();
        private AxKHOpenAPI axKHOpenAPI= null;

        public KiwoomOpenApiService(){
            axKHOpenAPI = class2.getAxKHOpenAPIInstance();
        }
        
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
        
        public string  GetCurrentStock(String 종목코드= "035420")
        {
            //035420   <--네이버종목번호\
            this.axKHOpenAPI.SetInputValue("종목코드", 종목코드);
            int nRet = axKHOpenAPI.CommRqData("주식기본정보", "OPT10001", 0, class2.GetScrNum());

            return "sdfsdf:";


        }
        public String test()
        {
            Console.WriteLine("test");
            return "asdffasdfasdf";
        }

        public string GetLoginInfo(String sTag = "ACCOUNT_CNT")
        {
            FileLog.PrintF("Get사용자정보");
            FileLog.PrintF(sTag);
            /*
            “ACCOUNT_CNT” – 전체 계좌 개수를 반환한다.
            "ACCNO" – 전체 계좌를 반환한다. 계좌별 구분은 ‘;’이다.
            “USER_ID” - 사용자 ID를 반환한다.
            “USER_NAME” – 사용자명을 반환한다.
            “KEY_BSECGB” – 키보드보안 해지여부. 0:정상, 1:해지
            “FIREW_SECGB” – 방화벽 설정 여부. 0:미설정, 1:설정, 2:해지
                Ex) openApi.GetLoginInfo(“ACCOUNT_CNT”);
            */
            FileLog.PrintF(sTag);

            String ret = axKHOpenAPI.GetLoginInfo(sTag);
            FileLog.PrintF(ret);
            return ret;


        }
               
    }
}
