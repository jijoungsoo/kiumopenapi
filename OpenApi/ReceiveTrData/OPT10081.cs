using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.Collections.Concurrent;
using OpenApi.Spell;
using AxKHOpenAPILib;

namespace OpenApi.ReceiveTrData
{
    /// <summary>
    ///  [ OPT10081 : 주식일봉차트조회요청 ]
    ///[0613]주식일월차트
    ///</summary>
    public class OPT10081: ReceiveTrData
    {

        public OPT10081() {
            FileLog.PrintF("OPT10081");
        }


        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {

            /*
       sScrNo – 화면번호
       sRQName – 사용자구분 명
       sTrCode – Tran 명
       sRecordName – Record 명
       sPreNext – 연속조회 유무
       */
            FileLog.PrintF("ReceivedData OPT10081");
            try
            {
                StringBuilder sbAll = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                //sb.Append("일자:{0}|종목코드:{1}|현재가:{2}|거래량:{3}|거래대금:{4}|시가:{5}|고가:{6}|저가:{7}");
                //sb.Append("stock_date:{0}|stock_code:{1}|current_price:{2}|trade_quantity:{3}|trade_price:{4}|start_price:{5}|high_price:{6}|low_price:{7}");
                sb.Append("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}");
                String tmp = sb.ToString();
                String 종목코드="XXXX";
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

                FileLog.PrintF("keyStockCode1  ==" + keyStockCode);
                FileLog.PrintF("key1  ==" + key);

                spell = AppLib.getClass1Instance().getSpell(key).ShallowCopy();
                String startDate = spell.startDate;
                FileLog.PrintF("startDate  ==" + startDate);

                String lastStockDate = "";
                int startDate일자 = 0;
                if (!int.TryParse(startDate, out startDate일자))
                {
                    startDate일자 = 0;
                }

                if (nCnt > 0)
                {
                    for (int i = 0; i < nCnt; i++)
                    {
                        /*
                        if (i == 0) {
                            종목코드1 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목코드").Trim();
                        }
                        */

                    

                        String 종목코드1 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목코드").Trim();
                    
                        if (종목코드1 == null || 종목코드1.Trim().Equals(""))
                        {
                            종목코드1 = 종목코드;
                        }
                        // FileLog.PrintF("거래대금  ==" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래대금").Trim());

                        int 현재가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim());
                        int 거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim());
                        int 거래대금 = 0;  //이게 INF 로 나올때가 있다.. 주식코드는 013900 두원중공업
                        if(!Int32.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래대금").Trim(),out 거래대금) )
                        {
                            거래대금 = 0;
                        }
                        int 일자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim());
                        int 시가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "시가").Trim());
                        int 고가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "고가").Trim());
                        int 저가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "저가").Trim());
                        //              int 수정주가구분 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정주가구분").Trim());
                        //              int 수정비율 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정비율").Trim());
                        //              int 대업종구분 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "대업종구분").Trim());
                        //              int 소업종구분 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "소업종구분").Trim());
                        //              int 종목정보 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목정보").Trim());
                        //              int 수정주가이벤트 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "수정주가이벤트").Trim());
                        //             int 전일종가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일종가").Trim());

                        String tmp1 = String.Format(tmp
                            , 일자
                            , 종목코드
                            , 현재가
                            , 거래량
                            , 거래대금
                            , 시가
                            , 고가
                            , 저가
                        );
                        lastStockDate = 일자.ToString();

                        if (startDate != null)
                        {
                            if (startDate.Equals("ZERO") || startDate.Equals("TWO"))
                            {
                                sbAll.AppendLine(tmp1);
                            }
                            else
                            {
                                if (일자 >= startDate일자)
                                {
                                    sbAll.AppendLine(tmp1);
                                } else
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            sbAll.AppendLine(tmp1);
                        }
                    }
                } else
                {
                    종목코드 = "00000";
                }

                int prevNext = 0;
                if (!int.TryParse(e.sPrevNext, out prevNext))
                {
                    prevNext = 0;
                }
                ScreenNumber.getClass1Instance().DisconnectRealData(e.sScrNo);
                ScreenNumber.getClass1Instance().SetRealRemove("ALL", "ALL");
                putReceivedQueueAndsetNextSpell(key,sbAll.ToString(), prevNext, lastStockDate);
            }
            catch (Exception ex)
            {
                FileLog.PrintF("[ALERT-ReceivedData-OPT10081]Exception ex=" + ex.Message);
            }
        }  

        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell)
        {
            /*
1. Open API 조회 함수 입력값을 설정합니다.
종목코드 = 전문 조회할 종목코드
SetInputValue("종목코드"	,  "입력값 1");
기준일자 = YYYYMMDD(20160101 연도4자리, 월 2자리, 일 2자리 형식)
SetInputValue("기준일자"	,  "입력값 2");
수정주가구분 = 0 or 1, 수신데이터 1:유상증자, 2:무상증자, 4:배당락, 8:액면분할, 16:액면병합, 32:기업합병, 64:감자, 256:권리락
SetInputValue("수정주가구분"	,  "입력값 3");
2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
CommRqData( "RQName"	,  "opt10081"	,  "0"	,  "화면번호");
*/

            FileLog.PrintF("OPT10081:Run sRQNAME=> " + spell.sRQNAME);
            FileLog.PrintF("OPT10081:Run sTrCode=> " + spell.sTrCode);
            FileLog.PrintF("OPT10081:Run nPrevNext=> " + spell.nPrevNext);
            FileLog.PrintF("OPT10081:Run sScreenNo=> " + spell.sScreenNo);
            FileLog.PrintF("OPT10081:Run 종목코드=> " + spell.stockCode);
            FileLog.PrintF("OPT10081:Run 기준일자=> " + spell.endDate);
            FileLog.PrintF("OPT10081:Run 수정주가구분=> 1");

            axKHOpenAPI.SetInputValue("종목코드", spell.stockCode);
            axKHOpenAPI.SetInputValue("기준일자", spell.endDate);
            axKHOpenAPI.SetInputValue("수정주가구분", "1");/*수정주가 구분 이것도 무조건 1로 하자. */
                                                     /*  수정주가구분 = 0 or 1, 수신데이터 1:유상증자, 2:무상증자, 4:배당락, 8:액면분할, 16:액면병합, 32:기업합병, 64:감자, 256:권리락
                                                         SetInputValue("수정주가구분"	,  "입력값 3"); */
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }
    }
}
