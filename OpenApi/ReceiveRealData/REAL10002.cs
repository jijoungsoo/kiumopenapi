using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using OpenApi.Spell;
using AxKHOpenAPILib;
using MySql.Data.MySqlClient;
using OpenApi.Dto;

namespace OpenApi.ReceiveRealData
{
    /// <summary>
    ///  [ REAL10002 : 주식체결 ]
    ///</summary>
    public class REAL10002 : ReceiveRealData
    {
        private static readonly String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public REAL10002(){
            FileLog.PrintF("REAL10002");
        }
        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            /*
            
                [20] = 체결시간            //(0)
            	[10] = 현재가              //(1)
	            [11] = 전일대비            //(2)
	            [12] = 등락율              //(3)
	            [27] = (최우선)매도호가    //(4)
	            [28] = (최우선)매수호가    //(5)
                [15] = 거래량              //(6)
	            [13] = 누적거래량          //(7)
	            [14] = 누적거래대금        //(8)    
	            [16] = 시가                //(9)
	            [17] = 고가                //(10)
	            [18] = 저가                //(11)
	            [25] = 전일대비기호        //(12)
	            [26] = 전일거래량대비(계약,주)   //(13)
	            [29] = 거래대금증감              //(14)
	            [30] = 전일거래량대비(비율)      //(15)
	            [31] = 거래회전율                //(16)
	            [32] = 거래비용                  //(17)
                [228] = 체결강도                 //(18)
	            [311] = 시가총액(억)             //(19)
                [290] = 장구분                   //(20)
                [691] = KO접근도                 //(21)
	            [567] = 상한가발생시간           //(22) 
	            [568] = 하한가발생시간           //(23)
                
            */
            FileLog.PrintF(String.Format("체결시간=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 20).Trim()));   //[0]
            FileLog.PrintF(String.Format("현재가=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 10).Trim()));     //[1]
            FileLog.PrintF(String.Format("전일대비=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 11).Trim()));   //[2]
            FileLog.PrintF(String.Format("등락율=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 12).Trim()));     //[3]
            FileLog.PrintF(String.Format("(최우선)매도호가=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 27).Trim()));   //[4]
            FileLog.PrintF(String.Format("(최우선)매수호가=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 28).Trim()));   //[5]
            FileLog.PrintF(String.Format("거래량=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 15).Trim()));      //[6]
            FileLog.PrintF(String.Format("누적거래량=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 13).Trim()));   //[7]
            FileLog.PrintF(String.Format("누적거래대금=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 14).Trim())); //[8]
            FileLog.PrintF(String.Format("시가=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 16).Trim()));          //[9]
            FileLog.PrintF(String.Format("고가=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 17).Trim()));          //[10]
            FileLog.PrintF(String.Format("저가=>{0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 18).Trim()));         //[11]
            FileLog.PrintF(String.Format("전일대비기호=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 25).Trim()));  //[12]
            FileLog.PrintF(String.Format("전일거래량대비_계약_주=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 26).Trim()));  //[13]
            FileLog.PrintF(String.Format("거래대금증감=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 29).Trim()));             //[14]
            FileLog.PrintF(String.Format("전일거래량대비_비율=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 30).Trim()));      //[15]
            FileLog.PrintF(String.Format("거래회전율=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 31).Trim()));               //[16]
            FileLog.PrintF(String.Format("거래비용=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 32).Trim()));               //[17]
            FileLog.PrintF(String.Format("체결강도=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 228).Trim()));               //[18]
            FileLog.PrintF(String.Format("시가총액_억=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 311).Trim()));               //[19]
            FileLog.PrintF(String.Format("장구분=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 290).Trim()));               //[20]
            FileLog.PrintF(String.Format("KO접근도=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 691).Trim()));               //[21]
            FileLog.PrintF(String.Format("상한가발생시간=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 567).Trim()));         //[22]
            FileLog.PrintF(String.Format("하한가발생시간=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 568).Trim()));         //[23]
            FileLog.PrintF(String.Format("종목코드=> {0} ", e.sRealKey.ToString().Trim()));
            FileLog.PrintF(String.Format("RealName=> {0} ", e.sRealType.ToString().Trim()));
            FileLog.PrintF(String.Format("sRealData=> {0} ", e.sRealData.ToString().Trim()));

            String 현재일자 = DateTime.Now.ToString("yyyy-MM-dd");
            String 체결시간TMP = axKHOpenAPI.GetCommRealData(e.sRealType, 20).Trim();           //[0]
            //체결시간이 6자리이므로 HHMMSS ==> HH:MM:SS로 바꿔야한다.
            String 체결시간 =체결시간TMP.Substring(0, 2)+":"+체결시간TMP.Substring(2, 2)+":"+체결시간TMP.Substring(4, 2);

            체결시간 = 현재일자 + " " + 체결시간;

            REAL10002_Data real10002_data = new REAL10002_Data();
            //String 현재시간 = DateTime.Now.ToString("yyyyMMdd HH:mm:ss:fff");
            real10002_data.체결시간 = 체결시간;
            real10002_data.현재가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 10).Trim());         //[1]
            real10002_data.전일대비 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 11).Trim());         //[2]
            real10002_data.등락율 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 12).Trim());         //[3]
            real10002_data.매도호가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 27).Trim());         //[4]
            real10002_data.매수호가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 28).Trim());         //[5]
            real10002_data.거래량 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 15).Trim());         //[6]
            real10002_data.누적거래량 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 13).Trim());  //[7]
            real10002_data.누적거래대금 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 14).Trim());  //[8]
            real10002_data.시가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 16).Trim());  //[9]
            real10002_data.고가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 17).Trim());  //[10]
            real10002_data.저가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 18).Trim());  //[11]
            real10002_data.전일대비기호 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 25).Trim());  //[12]
            real10002_data.전일거래량대비_계약_주 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 26).Trim()); //[13]
            real10002_data.거래대금증감 = decimal.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 29).Trim());  //[14]
            real10002_data.전일거래량대비_비율 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 30).Trim());  //[15]
            real10002_data.거래회전율 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 31).Trim());   //[16]
            real10002_data.거래비용 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 32).Trim());   //[17]
            real10002_data.체결강도 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 228).Trim());   //[18]
            real10002_data.시가총액_억 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 311).Trim()); //[19]
            real10002_data.장구분 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 290).Trim()); //[20]
            real10002_data.KO접근도 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 691).Trim()); //[21]
            real10002_data.상한가발생시간 = axKHOpenAPI.GetCommRealData(e.sRealType, 567).Trim(); //[22]
            real10002_data.하한가발생시간 = axKHOpenAPI.GetCommRealData(e.sRealType, 568).Trim(); //[23]
            real10002_data.종목코드 = e.sRealKey.ToString().Trim(); //[24]
            real10002_data.RealName = e.sRealType.ToString().Trim();

            MyStock.getClass1Instance().UpdateStockList(real10002_data);

            SendDirectFile(real10002_data);
            SendDirectDb(real10002_data);

        }

        private void SendDirectFile(REAL10002_Data real10002_data)
        {
            String tmp = "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}";
            String tmp1 = String.Format(tmp,
                    real10002_data.체결시간,   //[0]
                    real10002_data.종목코드,   //[1]
                     real10002_data.현재가,   //[2]
                     real10002_data.전일대비,   //[3]
                     real10002_data.등락율,   //[4]
                     real10002_data.매도호가,   //[5]
                     real10002_data.매수호가,   //[6]
                     real10002_data.거래량,   //[7]
                     real10002_data.누적거래량,   //[8]
                     real10002_data.누적거래대금,   //[9]
                     real10002_data.시가,   //[10]
                     real10002_data.고가,   //[11]
                     real10002_data.저가,   //[12]
                     real10002_data.전일대비기호,   //[13]
                     real10002_data.전일거래량대비_계약_주,   //[14]
                     real10002_data.거래대금증감,   //[15]
                     real10002_data.전일거래량대비_비율,   //[16]
                     real10002_data.거래회전율,   //[17]
                     real10002_data.거래비용,   //[18]
                     real10002_data.체결강도,   //[19]
                     real10002_data.시가총액_억,   //[20]
                     real10002_data.장구분,   //[21]
                     real10002_data.KO접근도,   //[22]
                     real10002_data.상한가발생시간,   //[23]
                     real10002_data.하한가발생시간,   //[24]
                     real10002_data.RealName   //[25]
             );
            String path1= path + "\\주식체결.txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path1, true);
            file.WriteLine(tmp1.ToString());
            file.Close();
        }
        private void SendDirectDb(REAL10002_Data real10002_data)
        {
            using (MySqlConnection conn = new MySqlConnection(Class1.connStr))
            {

                string sql = @"INSERT into realtime_contracts (
stock_date
,stock_code
,current_price
,contrast_yesterday
,fluctuation_rate
,offered_price
,bid_price
,trade_quantity
,accumulated_trade_quantity
,accumulated_trade_price
,start_price
,high_price
,low_price
,contrast_yesterday_symbol
,yesterday_contrast_trade_quantity
,trade_amount_variation
,yesterday_contrast_trade_rate
,trade_turnover_ratio
,trade_cost
,contract_strength
,total_market_price
,market_gubun
,ko_accessibility_rate
,upper_price_limit_time
,lower_price_limit_time
)
VALUES
(
@체결시간
,@종목코드
,@현재가
,@전일대비
,@등락율
,@매도호가
,@매수호가
,@거래량
,@누적거래량
,@누적거래대금
,@시가
,@고가
,@저가
,@전일대비기호
,@전일거래량대비_계약_주
,@거래대금증감
,@전일거래량대비_비율
,@거래회전율
,@거래비용
,@체결강도
,@시가총액_억
,@장구분
,@KO접근도
,@상한가발생시간
,@하한가발생시간
);
";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@체결시간", real10002_data.체결시간);
                cmd.Parameters.AddWithValue("@종목코드", real10002_data.종목코드);
                cmd.Parameters.AddWithValue("@현재가", real10002_data.현재가);
                cmd.Parameters.AddWithValue("@전일대비", real10002_data.전일대비);
                cmd.Parameters.AddWithValue("@등락율", real10002_data.등락율);
                cmd.Parameters.AddWithValue("@매도호가", real10002_data.매도호가);
                cmd.Parameters.AddWithValue("@매수호가", real10002_data.매수호가);
                cmd.Parameters.AddWithValue("@거래량", real10002_data.거래량);
                cmd.Parameters.AddWithValue("@누적거래량", real10002_data.누적거래량);
                cmd.Parameters.AddWithValue("@누적거래대금", real10002_data.누적거래대금);
                cmd.Parameters.AddWithValue("@시가", real10002_data.시가);
                cmd.Parameters.AddWithValue("@고가", real10002_data.고가);
                cmd.Parameters.AddWithValue("@저가", real10002_data.저가);
                cmd.Parameters.AddWithValue("@전일대비기호", real10002_data.전일대비기호);
                cmd.Parameters.AddWithValue("@전일거래량대비_계약_주", real10002_data.전일거래량대비_계약_주);
                
                /*
                MySqlParameter dir = new MySqlParameter("@거래대금증감", MySql.Data.MySqlClient.MySqlDbType.Decimal);
                dir.Value = real10002_data.거래대금증감;
                cmd.Parameters.Add(dir);
                아래가 문제가 아니라 디비스 키마가 decimal(10)이어서 10자리이상 표시안되었던거였음.
                */
                cmd.Parameters.AddWithValue("@거래대금증감", real10002_data.거래대금증감);
                cmd.Parameters.AddWithValue("@전일거래량대비_비율", real10002_data.전일거래량대비_비율);
                cmd.Parameters.AddWithValue("@거래회전율", real10002_data.거래회전율);
                cmd.Parameters.AddWithValue("@거래비용", real10002_data.거래비용);
                cmd.Parameters.AddWithValue("@체결강도", real10002_data.체결강도);
                cmd.Parameters.AddWithValue("@시가총액_억", real10002_data.시가총액_억);
                cmd.Parameters.AddWithValue("@장구분", real10002_data.장구분);
                cmd.Parameters.AddWithValue("@KO접근도", real10002_data.KO접근도);
                cmd.Parameters.AddWithValue("@상한가발생시간", real10002_data.상한가발생시간);
                cmd.Parameters.AddWithValue("@하한가발생시간", real10002_data.하한가발생시간);
                cmd.ExecuteNonQuery();  //기존 계좌수익률을 삭제하고
            }
        }
    

    }
    

}
