using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenApi.Spell;
using AxKHOpenAPILib;

namespace OpenApi.ReceiveTrData
{
    public class OPT10015 : ReceiveTrData
    {

        public OPT10015() {
            FileLog.PrintF("OPT10015");
        }

        private static string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private static Dictionary<string, int> clearFlag = new Dictionary<string, int>();

        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("ReceivedData OPT10015");
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
                /*일자,종목코드,종가,전일대비기호,전일대비,등락율,거래량,거래대금,장전거래량,장전거래비중,장중거래량,장중거래비중,장후거래량,장후거래비중,합계3,기간중거래량*/
                sb.Append("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}");
                String tmp = sb.ToString();
                String 종목코드 = "XXXX";
                int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                );
                종목코드 = Class1.getClass1Instance().getStockCode(keyStockCode);

                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                    , 종목코드
                );

                spell = Class1.getClass1Instance().getSpell(key).ShallowCopy();
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
                        int 종가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종가").Trim());//[1]
                        int 전일대비기호 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비기호").Trim());//[2]
                        int 전일대비 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비").Trim());//[3]
                        float 등락율 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim());//[4]
                        int 거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim());//[5]
                        int 거래대금 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래대금").Trim());//[6]
                        int 장전거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장전거래량").Trim());//[7]
                        float 장전거래비중 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장전거래비중").Trim());//[8]
                        int 장중거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장중거래량").Trim());//[9]
                        float 장중거래비중 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장중거래비중").Trim());//[10]
                        int 장후거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장후거래량").Trim());//[11]
                        float 장후거래비중 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장후거래비중").Trim());//[12]
                        int 합계3 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "합계3").Trim());//[13]
                        int 기간중거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기간중거래량").Trim());//[14]
                    

                        String tmp1 = String.Format(tmp,
                            일자,
                            종목코드,
                            종가,
                            전일대비기호,
                            전일대비,
                            등락율,
                            거래량,
                            거래대금,
                            장전거래량,
                            장전거래비중,
                            장중거래량,
                            장중거래비중,
                            장후거래량,
                            장후거래비중,
                            합계3,
                            기간중거래량

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
                                }
                                else
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
                else {
                    종목코드 = "00000";
                }

                int prevNext = 0;
                if (!int.TryParse(e.sPrevNext, out prevNext))
                {
                    prevNext = 0;
                }
                this.DisconnectRealData(axKHOpenAPI, e.sScrNo);
                this.SetRealRemove(axKHOpenAPI, "ALL", "ALL");
                putReceivedQueueAndsetNextSpell(key, sbAll.ToString(), prevNext, lastStockDate);
            }
            catch (Exception ex)
            {
                FileLog.PrintF("[ALERT-ReceivedData-OPT10015]Exception ex=" + ex.Message);
            }

        }


        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell)
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
            FileLog.PrintF("OPT10015:Run sRQNAME=> " + spell.sRQNAME);
            FileLog.PrintF("OPT10015:Run sTrCode=> " + spell.sTrCode);
            FileLog.PrintF("OPT10015:Run nPrevNext=> " + spell.nPrevNext);
            FileLog.PrintF("OPT10015:Run sScreenNo=> " + spell.sScreenNo);
            FileLog.PrintF("OPT10015:Run 시작일자=> " + spell.endDate);
            FileLog.PrintF("OPT10015:Run 종목코드=> " + spell.stockCode);

            axKHOpenAPI.SetInputValue("종목코드", spell.stockCode);
            axKHOpenAPI.SetInputValue("시작일자", spell.endDate);
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }
    }
}