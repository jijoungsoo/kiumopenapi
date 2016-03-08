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

    public class OPT10014 : ReceiveTrData
    {
        public OPT10014(){
            FileLog.PrintF("OPT10014");
        }

        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("ReceivedData OPT10014");
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

                /*일자,종목코드 ,종가,전일대비기호,전일대비,등락율,거래량,공매도량,매매비중,공매도거래대금,공매도평균가*/
                sb.Append("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}");
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

                spell = AppLib.getClass1Instance().getSpell(key).ShallowCopy();
                String startDate = spell.startDate;
                String lastStockDate = "";
                int startDate일자 = 0;
                if (!int.TryParse(startDate, out startDate일자))
                {
                    startDate일자 = 0;
                }

                if (nCnt > 0) {
                    for (int i = 0; i < nCnt; i++) {
                        int 일자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim());//[0]
                        int 종가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종가").Trim());//[1]
                        int 전일대비기호 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비기호").Trim());//[2]
                        int 전일대비 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비").Trim());//[3]
                        float 등락율 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim());//[4]
                        int 거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim());//[5]
                        int 공매도량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "공매도량").Trim());//[6]
                        float 매매비중 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매매비중").Trim());//[7]
                        int 공매도거래대금 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "공매도거래대금").Trim());//[8]
                        int 공매도평균가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "공매도평균가").Trim());//[9]

                        String tmp1 = String.Format(tmp,
                            일자,
                            종목코드,
                            종가,
                            전일대비기호,
                            전일대비,
                            등락율,
                            거래량,
                            공매도량,
                            매매비중,
                            공매도거래대금,
                            공매도평균가
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
                } else {
                    종목코드 = "00000";
                }

                int prevNext = 0;
                if (!int.TryParse(e.sPrevNext, out prevNext)) {
                    prevNext = 0;
                }
                ScreenNumber.getClass1Instance().DisconnectRealData(e.sScrNo);
                ScreenNumber.getClass1Instance().SetRealRemove("ALL", "ALL");
                putReceivedQueueAndsetNextSpell(key,sbAll.ToString(), prevNext, lastStockDate);
            }
            catch (Exception ex)
            {
                FileLog.PrintF("[ALERT-ReceivedData-OPT10014]Exception ex=" + ex.Message);
            }
        }
        
        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell) {
            /*
            [opt10014: 공매도추이요청]
            1. Open API 조회 함수 입력값을 설정합니다.
            종목코드 = 전문 조회할 종목코드
            SetInputValue("종목코드"	,  "입력값 1");
            시간구분 = 0:시작일, 1:기간
            SetInputValue("시간구분"	,  "입력값 2");
            시작일자 = YYYYMMDD(20160101 연도4자리, 월 2자리, 일 2자리 형식)
            SetInputValue("시작일자"	,  "입력값 3");
            종료일자 = YYYYMMDD(20160101 연도4자리, 월 2자리, 일 2자리 형식)
            SetInputValue("종료일자"	,  "입력값 4");
            2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
            CommRqData( "RQName"	,  "opt10014"	,  "0"	,  "화면번호");
            */
            FileLog.PrintF("OPT10014:Run sRQNAME=> " + spell.sRQNAME);
            FileLog.PrintF("OPT10014:Run sTrCode=> " + spell.sTrCode);
            FileLog.PrintF("OPT10014:Run nPrevNext=> " + spell.nPrevNext);
            FileLog.PrintF("OPT10014:Run sScreenNo=> " + spell.sScreenNo);
            FileLog.PrintF("OPT10014:Run 종목코드=> " + spell.stockCode);
            FileLog.PrintF("OPT10014:Run 시간구분=> " + "0");
            FileLog.PrintF("OPT10014:Run 시작일자=> " + spell.startDate);
            FileLog.PrintF("OPT10014:Run 종료일자=> " + spell.endDate);

            axKHOpenAPI.SetInputValue("종목코드", spell.stockCode);
            axKHOpenAPI.SetInputValue("시간구분", "0");  /*시간구분 = 0:시작일, 1:기간   --무조건 0으로 하자 1로할경우 범위가 넘어도 nPreNext가 2로 안나온다. */
            axKHOpenAPI.SetInputValue("시작일자", spell.startDate);
            axKHOpenAPI.SetInputValue("종료일자", spell.endDate);
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }

    }
}
