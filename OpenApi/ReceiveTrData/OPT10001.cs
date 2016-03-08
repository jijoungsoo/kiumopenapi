using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using OpenApi.Spell;
using AxKHOpenAPILib;

namespace OpenApi.ReceiveTrData
{

    public class OPT10001 : ReceiveTrData
    {
        public OPT10001(){
            FileLog.PrintF("OPT10001");
        }

        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("ReceivedData OPT10001");
            try { 
                /*
                sScrNo – 화면번호
                sRQName – 사용자구분 명
                sTrCode – Tran 명
                sRecordName – Record 명
                sPreNext – 연속조회 유무
                */
                StringBuilder sbAll = new StringBuilder();
                StringBuilder sb = new StringBuilder();

                /*일자,종목코드 ,종가,전일대비기호,전일대비,등락율,거래량,공매도량,매매비중,공매도거래대금,공매도평균가*/
                sb.Append("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}|{35}|{36}|{37}|{38}|{39}|{40}|{41}|{42}");
                String tmp = sb.ToString();
                String 종목코드 = "XXXX";
                int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                );
                종목코드 = AppLib.getClass1Instance().getStockCode(keyStockCode);

                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                    , 종목코드
                );
                String 일자 = DateTime.Now.ToString("yyyyMMdd");

                spell = AppLib.getClass1Instance().getSpell(key).ShallowCopy();

                //Array arr =(Array)axKHOpenAPI.GetCommDataEx(e.sTrCode, e.sRQName);
            
                if (nCnt > 0) {
                    for (int i = 0; i < nCnt; i++) {
                        String 종목코드1 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목코드").Trim();//[0]
                        if (종목코드1.Equals(""))
                        {
                            종목코드1 = 종목코드;//일부 종목코드들은 아래 데이터가 아예 안나온다. ///51A077
                        }
                        String 종목명 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목명").Trim();//[1]
                        int 결산월 = 0;
                        String str결산월=axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "결산월");
                        if (isNotNull(str결산월)==true)
                        {
                            결산월 = Int32.Parse(str결산월.Trim());//[2]
                        }
                        String str액면가 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "액면가");
                        float 액면가 = 0;
                        if (isNotNull(str액면가) == true)
                        {
                            액면가 = float.Parse(str액면가.Trim());//[3]
                        }
                        String str자본금 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "자본금");
                        int 자본금 = 0;
                        if (isNotNull(str자본금) == true)
                        {
                            자본금 = Int32.Parse(str자본금.Trim());//[4]
                        }
                        String str상장주식 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "상장주식");
                        int 상장주식 = 0;
                        if (isNotNull(str상장주식) == true)
                        {
                            상장주식 = Int32.Parse(str상장주식.Trim());//[5]
                        }
                        String str신용비율 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "신용비율");
                        float 신용비율 = 0;
                        if (isNotNull(str신용비율) == true)
                        {
                            신용비율 = float.Parse(str신용비율.Trim());//[6]
                        }
                        String str연중최고 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "연중최고");
                        int 연중최고 = 0;
                        if (isNotNull(str연중최고) == true)
                        {
                            연중최고 = int.Parse(str연중최고.Trim());//[7]
                        }
                        String str연중최저 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "연중최저");
                        int 연중최저 = 0;
                        if (isNotNull(str연중최저) == true)
                        {
                            연중최저 = int.Parse(str연중최저.Trim());//[8]
                        }
                        String str시가총액 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시가총액");
                        int 시가총액 = 0;
                        if (isNotNull(str시가총액) == true)
                        {
                            시가총액 = int.Parse(str시가총액.Trim());//[9]
                        }
                        String str시가총액비중 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시가총액비중");
                        int 시가총액비중 = 0;
                        if (isNotNull(str시가총액비중) == true)
                        {
                            시가총액비중 = Int32.Parse(str시가총액비중.Trim());//[10]
                        }
                        String str외인소진률 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "외인소진률");
                        float 외인소진률 = 0;
                        if (isNotNull(str외인소진률) == true)
                        {
                            외인소진률 = float.Parse(str외인소진률.Trim());//[11]
                        }
                        String str대용가 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "대용가");
                        int 대용가 = 0;
                        if (isNotNull(str대용가) == true)
                        {
                            대용가 = int.Parse(str대용가.Trim());//[12]
                        }
                        String strPER = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "PER");
                        float PER = 0;
                        if (isNotNull(strPER) == true)
                        {
                            PER = float.Parse(strPER.Trim());//[13]
                        }
                        String strEPS = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "EPS");
                        int EPS = 0;
                        if (isNotNull(strEPS) == true)
                        {
                            EPS = int.Parse(strEPS.Trim());//[14]
                        }
                        String strROE = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "ROE");
                        float ROE = 0;
                        if (isNotNull(strROE) == true)
                        { 
                            ROE = float.Parse(strROE.Trim());//[15]    
                        }
                        String strPBR = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "PBR");
                        float PBR = 0;
                        if (isNotNull(strPBR) == true)
                        {
                            PBR = float.Parse(strPBR.Trim());//[16]    
                        }
                        String strEV = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "EV");
                        float EV = 0;
                        if (isNotNull(strEV) == true)
                        {
                            EV = float.Parse(strEV.Trim());//[17]    
                        }
                        String strBPS = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "BPS");
                        int BPS = 0;
                        if (isNotNull(strBPS) == true)
                        {
                            BPS = int.Parse(strBPS.Trim());//[18]    
                        }
                        String str매출액 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매출액");
                        int 매출액 = 0;
                        if (isNotNull(str매출액) == true)
                        { 
                            매출액 = Int32.Parse(str매출액.Trim());//[19]
                        }
                        String str영업이익 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "영업이익");
                        int 영업이익 = 0;
                        if (isNotNull(str영업이익) == true)
                        { 
                            영업이익 = Int32.Parse(str영업이익.Trim());//[20]
                        }
                        String strD250최고 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "250최고");
                        int D250최고 = 0;
                        if (isNotNull(strD250최고) == true)
                        {
                            D250최고 = Int32.Parse(strD250최고.Trim());//[21]
                        }
                        String strD250최저 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "250최저");
                        int D250최저 = 0;
                        if (isNotNull(strD250최저) == true)
                        {
                            D250최저 = Int32.Parse(strD250최저.Trim());//[22]
                        }


                    
                        int 시가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시가").Trim());//[23]
                        int 고가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "고가").Trim());//[24]
                        int 저가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "저가").Trim());//[25]
                        int 상한가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "상한가").Trim());//[26]
                        int 하한가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "하한가").Trim());//[27]
                        int 기준가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기준가").Trim());//[28]

                        String str예상체결가 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "예상체결가");
                        int 예상체결가 = 0;
                        if (isNotNull(str예상체결가) == true)
                        {
                            예상체결가 = Int32.Parse(str예상체결가.Trim());//[29]
                        }

                        String str예상체결수량 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "예상체결수량");
                        int 예상체결수량 = 0;
                        if (isNotNull(str예상체결수량) == true)
                        {
                            예상체결수량 = Int32.Parse(str예상체결수량.Trim());//[30]
                        }
                        String D250최고가일 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "250최고가일").Trim();//[31]
                        float D250최고가대비율 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "250최고가대비율").Trim());//[32]
                        String D250최저가일 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "250최저가일").Trim();//[33]
                        float D250최저가대비율 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "250최저가대비율").Trim());//[34]
                        int 현재가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim());//[35]
                        int 대비기호 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "대비기호").Trim());//[36]
                        int 전일대비 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비").Trim());//[37]
                        float 등락율 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim());//[38]
                        int 거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim());//[39]
                        float 거래대비 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래대비").Trim());//[40]
                        String 액면가단위 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "액면가단위").Trim();//[41]

                        String tmp1 = String.Format(tmp,
                            일자,
                            종목코드1,
                            종목명,
                            결산월,
                            액면가,
                            자본금,
                            상장주식,
                            신용비율,
                            연중최고,
                            연중최저,
                            시가총액,
                            시가총액비중,
                            외인소진률,
                            대용가,
                            PER,
                            EPS,
                            ROE,
                            PBR,
                            EV,
                            BPS,
                            매출액,
                            영업이익,
                            D250최고,
                            D250최저,
                            시가,
                            고가,
                            저가,
                            상한가,
                            하한가,
                            기준가,
                            예상체결가,
                            예상체결수량,
                            D250최고가일,
                            D250최고가대비율,
                            D250최저가일,
                            D250최저가대비율,
                            현재가,
                            대비기호,
                            전일대비,
                            등락율,
                            거래량,
                            거래대비,
                            액면가단위
                        );
                        sbAll.AppendLine(tmp1);
                    }
                } else {
                    종목코드 = "00000";
                }

                //이건 nPreNext가 항상 0이다. 당일 조회만 됨
                int prevNext = 0;
                if (!int.TryParse(e.sPrevNext, out prevNext)) {
                    prevNext = 0;
                }
                String lastStockDate = "";
                ScreenNumber.getClass1Instance().DisconnectRealData( e.sScrNo);
                ScreenNumber.getClass1Instance().SetRealRemove("ALL", "ALL");
                putReceivedQueueAndsetNextSpell(key,sbAll.ToString(), prevNext, lastStockDate);
            }
            catch(Exception ex)
            {
                FileLog.PrintF("[ALERT-ReceivedData-OPT10001]Exception ex=" + ex.Message);
            }
        }
        private Boolean isNotNull(String value)
        {
            if(value!=null && !value.Trim().Equals("")) { 
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell) {
            /*
 [ opt10001 : 주식기본정보요청 ]
  1. Open API 조회 함수 입력값을 설정합니다.
	종목코드 = 전문 조회할 종목코드
	SetInputValue("종목코드"	,  "181710;066570");    
 2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
	CommRqData( "RQName"	,  "opt10001"	,  "0"	,  "화면번호"); 
            */

            FileLog.PrintF("OPT10001:Run sRQNAME=>" + spell.sRQNAME);
            FileLog.PrintF("OPT10001:Run sTrCode=>" + spell.sTrCode);
            FileLog.PrintF("OPT10001:Run nPrevNext=>" + spell.nPrevNext);
            FileLog.PrintF("OPT10001:Run sScreenNo=>" + spell.sScreenNo);
            FileLog.PrintF("OPT10001:Run 종목코드=>" + spell.stockCode);

            axKHOpenAPI.SetInputValue("종목코드", spell.stockCode);
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }
    }
}
