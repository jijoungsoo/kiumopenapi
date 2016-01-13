﻿using System;
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


            
        /// <summary>
        /// [4]로그인한 사용자 정보를 반환한다.
        /// ACCOUNT_CNT – 전체 계좌 개수를 반환한다.
        ///ACCNO – 전체 계좌를 반환한다.계좌별 구분은 ‘;’이다.
        ///USER_ID - 사용자 ID를 반환한다.
        ///USER_NAME – 사용자명을 반환한다.
        ///KEY_BSECGB – 키보드보안 해지여부. 0:정상, 1:해지
        ///FIREW_SECGB – 방화벽 설정 여부. 0:미설정, 1:설정, 2:해지
        ///Ex) openApi.GetLoginInfo(“ACCOUNT_CNT”);
        [OperationContract]
        String GetLoginInfo(String sTag);



        /// <summary>
        /// [10]화면 내 모든 리얼데이터 요청을 제거한다.
        /// sScnNo – 화면번호[4]
        /// </summary>
        [OperationContract]
        void DisconnectRealData(String sScnNo);

        /// <summary>
        ///[13] 설명 OpenAPI모듈의 경로를 반환한다.
        ///반환값 경로
        /// </summary>
        [OperationContract]
        String GetAPIModulePath();

        /// <summary>
        ///[14] 설명 시장구분에 따른 종목코드를 반환한다.
        ///입력값 sMarket – 시장구분
        ///반환값 종목코드 리스트, 종목간 구분은 ’;’이다.
        ///비고 sMarket – 0:장내, 3:ELW, 4:뮤추얼펀드, 5:신주인수권, 6:리츠, 8:ETF, 9:하이일드펀드, 10:코스닥, 30:제3시장
        /// </summary>
        [OperationContract]
        String GetCodeListByMarket(String sMarket);



        /// <summary>
        ///[15] 현재접속상태를 반환한다.
        /// 0:미연결, 1:연결완료
        /// </summary>
        [OperationContract]
        int GetConnectState();


        /// <summary>
        ///[16]설명 종목코드의 한글명을 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 종목한글명
        /// 비고 장내외, 지수선옵, 주식선옵 검색 가능.
        /// </summary>
        [OperationContract]
        String GetMasterCodeName(String strCode);

        /// <summary>
        ///[17]설명 종목코드의 상장주식수를 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 상장주식수
        /// </summary>
        [OperationContract]
        int GetMasterListedStockCnt(String strCode);


        /// <summary>
        ///[18]설명 종목코드의 감리구분을 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 감리구분
        ///비고 감리구분 – 정상, 투자주의, 투자경고, 투자위험, 투자주의환기종목
        /// </summary>
        [OperationContract]
        String GetMasterConstruction(String strCode);

        /// <summary>
        ///[19]설명 종목코드의 상장일을 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 상장일
        ///비고 상장일 포멧 – xxxxxxxx[8]
        /// </summary>
        [OperationContract]
        String GetMasterListedStockDate(String strCode);

        /// <summary>
        ///[20]설명 종목코드의 전일가를 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 전일가
        /// </summary>
        [OperationContract]
        String GetMasterLastPrice(String strCode);



        /// <summary>
        ///[21]설명 종목코드의 종목상태를 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 종목상태
        ///비고 종목상태 – 정상, 증거금100%, 거래정지, 관리종목, 감리종목, 투자유의종목, 담보대출, 액면분할, 신용가능
        /// </summary>
        [OperationContract]
        String GetMasterStockState(String strCode);



        /// <summary>
        ///[27] 설명 테마코드와 테마명을 반환한다.
        ///입력값 nType – 정렬순서 (0:코드순, 1:테마순)
        ///반환값 코드와 코드명 리스트
        ///비고 반환값의 코드와 코드명 구분은 ‘|’ 코드의 구분은 ‘;’
        ///Ex) 100|태양광_폴리실리콘;152|합성섬유
        /// </summary>
        [OperationContract]
        String GetThemeGroupList(int nType);


    }
}
