using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace WcfServiceLibrary
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 인터페이스 이름 "IService1"을 변경할 수 있습니다.
    [ServiceContract]
    public interface IKiwoomOpenApi
    {
        [OperationContract]
        string GetData(int value);



        [OperationContract]
        string GetCurrentStock(String 주식종목);


        [OperationContract]
        [WebGet(UriTemplate="test", ResponseFormat = WebMessageFormat.Xml)]
        string test();

        // TODO: 여기에 서비스 작업을 추가합니다.


        [OperationContract]
        String GetLoginInfo(String sTag);

               
        
    }
}
