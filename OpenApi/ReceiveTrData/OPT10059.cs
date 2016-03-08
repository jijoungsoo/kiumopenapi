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

    public class OPT10059 : ReceiveTrData
    {

        public OPT10059() {
            FileLog.PrintF("ReceivedData OPT10059");
        }


        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("ReceivedData OPT10059");
            try
            {
                /*
                sScrNo – 화면번호
                sRQName – 사용자구분 명
                sTrCode – Tran 명
                sRecordName – Record 명
                sPreNext – 연속조회 유무
                */
                StringBuilder sbAll = new StringBuilder();
                StringBuilder sb = new StringBuilder();
                //sb.Append("일자:{0}|종목코드:{1}|현재가:{2}|거래량:{3}|거래대금:{4}|시가:{5}|고가:{6}|저가:{7}");
                //sb.Append("stock_date:{0}|current_price:{1}|current_price:{2}|trade_quantity:{3}|trade_price:{4}|start_price:{5}|high_price:{6}|low_price:{7}");

                /*일자 ,현재가,대비기호,전일대비,등락율,누적거래대금,개인투자자,외국인투자자,기관계,금융투자,보험,투신,기타금융,은행,연기금등,사모펀드,국가,기타법인,내외국인,종목코드,매매구분,금액수량구분*/
                sb.Append("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}");
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
            
                spell = AppLib.getClass1Instance().getSpell(key);
                String startDate = spell.startDate;

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
                        int 일자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim());//[0]
                        int 현재가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim());//[1]
                        float 대비기호 = 0;//[2]

                        if(!float.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "대비기호").Trim(),out 대비기호))
                        {
                        }
                    
                        int 전일대비 = 0;//[3]
                        if (!Int32.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비").Trim(), out 전일대비))
                        {
                            전일대비 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비").Trim().Replace("--", "-"));
                        }
                        float 등락율 = 0;//[4]

                        if (!float.TryParse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim(), out 등락율))
                        {
                            등락율 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim());
                        }
                    
                        int 누적거래대금 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "누적거래대금").Trim());//[5
                        int 개인투자자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "개인투자자").Trim());//[6]
                        int 외국인투자자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "외국인투자자").Trim());//[7]
                        int 기관계 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기관계").Trim());//[8]
                        int 금융투자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "금융투자").Trim());//[9]
                        int 보험 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "보험").Trim());//[10]
                        int 투신 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "투신").Trim());//[11]
                        int 기타금융 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기타금융").Trim());//[12]
                        int 은행 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "은행").Trim());//[13]
                        int 연기금등 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "연기금등").Trim());//[14]
                        int 사모펀드 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "사모펀드").Trim());//[15]
                        int 국가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "국가").Trim());//[16]
                        int 기타법인 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기타법인").Trim());//[17]
                        int 내외국인 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "내외국인").Trim());//[18]

                        String tmp1 = String.Format(tmp,
                            일자 ,
                            현재가,
                            대비기호,
                            전일대비,
                            등락율,
                            누적거래대금,
                            개인투자자,
                            외국인투자자,
                            기관계,
                            금융투자,
                            보험,
                            투신,
                            기타금융,
                            은행,
                            연기금등,
                            사모펀드,
                            국가,
                            기타법인,
                            내외국인,
                            종목코드,
                            spell.buyOrSell,
                            spell.priceOrAmount
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
                }
                else
                {
                    종목코드 = "00000";
                }

                int prevNext = 0;
                if (!int.TryParse(e.sPrevNext, out prevNext))
                {
                    prevNext = 0;
                }

                ScreenNumber.getClass1Instance().DisconnectRealData( e.sScrNo);
                ScreenNumber.getClass1Instance().SetRealRemove( "ALL", "ALL");

                putReceivedQueueAndsetNextSpell(key,sbAll.ToString(), prevNext, lastStockDate);
            }
            catch (Exception ex)
            {
                FileLog.PrintF("[ALERT-ReceivedData-OPT10059]Exception ex=" + ex.Message);
            }
        }

        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell)
        {
            /*
             [ OPT10059 : 종목별투자자기관별요청 ]
              1. Open API 조회 함수 입력값을 설정합니다.
	            일자 = YYYYMMDD (20160101 연도4자리, 월 2자리, 일 2자리 형식)
	            SetInputValue("일자"	,  "입력값 1");
                종목코드 = 전문 조회할 종목코드
	            SetInputValue("종목코드"	,  "입력값 2");
                금액수량구분 = 1:금액, 2:수량
	            SetInputValue("금액수량구분"	,  "입력값 3");
                매매구분 = 0:순매수, 1:매수, 2:매도
	            SetInputValue("매매구분"	,  "입력값 4");
                단위구분 = 1000:천주, 1:단주
	            SetInputValue("단위구분"	,  "입력값 5");
            2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
	            CommRqData( "RQName"	,  "OPT10059"	,  "0"	,  "화면번호"); 
            */
            
            FileLog.PrintF("OPT10059:Run sRQNAME=> " + spell.sRQNAME);
            FileLog.PrintF("OPT10059:Run sTrCode=> " + spell.sTrCode);
            FileLog.PrintF("OPT10059:Run nPrevNext=> " + spell.nPrevNext);
            FileLog.PrintF("OPT10059:Run sScreenNo=> " + spell.sScreenNo);
            FileLog.PrintF("OPT10059:Run 일자=> " + spell.endDate);
            FileLog.PrintF("OPT10059:Run 종목코드=> " + spell.stockCode);
            FileLog.PrintF("OPT10059:Run 금액수량구분=> " + spell.priceOrAmount);
            FileLog.PrintF("OPT10059:Run 매매구분=> " + spell.buyOrSell);
            FileLog.PrintF("OPT10059:Run 단위구분=> 1");

            axKHOpenAPI.SetInputValue("일자", spell.endDate);
            axKHOpenAPI.SetInputValue("종목코드", spell.stockCode);           
            axKHOpenAPI.SetInputValue("금액수량구분", spell.priceOrAmount);   
            axKHOpenAPI.SetInputValue("매매구분", spell.buyOrSell); 
            axKHOpenAPI.SetInputValue("단위구분", "1"); /*단위구분 = 1000:천주, 1:단주   --수량단위구분 모조건 1   -금액은 무조건 백만원단위 */
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }

    }
}
