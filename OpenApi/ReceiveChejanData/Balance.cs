using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxKHOpenAPILib;
using MySql.Data.MySqlClient;
using OpenApi.Dto;

namespace OpenApi.ReceiveChejanData
{
    class Balance : ReceiveChejanData
    {
        private static readonly String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public Balance()
        {
            FileLog.PrintF("Balance");
        }

        public override void ReceivedData(AxKHOpenAPI axKHOpenAPI, _DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 구분 : 잔고통보");
            //FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문/기록시간=>" + axKHOpenAPI.GetChejanData(908));
            //시간이 없음
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 계좌번호=>" + axKHOpenAPI.GetChejanData(9201));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 종목코드, 업종코드=>" + axKHOpenAPI.GetChejanData(9001));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 현재가, 체결가, 실시간종가=>" + axKHOpenAPI.GetChejanData(10));

            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 보유수량=>" + axKHOpenAPI.GetChejanData(930));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 매입단가=>" + axKHOpenAPI.GetChejanData(931));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 총매입가=>" + axKHOpenAPI.GetChejanData(932));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문가능수량=>" + axKHOpenAPI.GetChejanData(933));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일순매수량=>" + axKHOpenAPI.GetChejanData(945));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 매도 / 매수구분=>" + axKHOpenAPI.GetChejanData(946));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일 총 매도 손익=>" + axKHOpenAPI.GetChejanData(950));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 예수금=>" + axKHOpenAPI.GetChejanData(951));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData (최우선)매도호가=>" + axKHOpenAPI.GetChejanData(27));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData (최우선)매수호가=>" + axKHOpenAPI.GetChejanData(28));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 기준가=>" + axKHOpenAPI.GetChejanData(307));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 손익율=>" + axKHOpenAPI.GetChejanData(8019));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주식옵션거래단위=>" + axKHOpenAPI.GetChejanData(397));


            Balance_Data balance_Data = new Balance_Data();
            String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            balance_Data.기록시간 = dayTime;   //[0]
            balance_Data.계좌번호 = axKHOpenAPI.GetChejanData(9201).ToString().Trim(); //[1]
            balance_Data.종목코드 = axKHOpenAPI.GetChejanData(9001).ToString().Trim();  //[2]
            balance_Data.현재가 = int.Parse(axKHOpenAPI.GetChejanData(10).ToString().Trim());  //[3]
            balance_Data.보유수량 = int.Parse(axKHOpenAPI.GetChejanData(930).ToString().Trim());  //[4]
            balance_Data.매입단가 = int.Parse(axKHOpenAPI.GetChejanData(931).ToString().Trim());   //[5]
            balance_Data.총매입가 = int.Parse(axKHOpenAPI.GetChejanData(932).ToString().Trim());  //[6]
            balance_Data.주문가능수량 = int.Parse(axKHOpenAPI.GetChejanData(933).ToString().Trim());   //[7]
            balance_Data.당일순매수량 = int.Parse(axKHOpenAPI.GetChejanData(945).ToString().Trim());  //[8]
            balance_Data.매도수구분 = int.Parse(axKHOpenAPI.GetChejanData(946).ToString().Trim());  //[9]
            balance_Data.당일총매도손익 = int.Parse(axKHOpenAPI.GetChejanData(950).ToString().Trim());   //[10]
            balance_Data.예수금 = int.Parse(axKHOpenAPI.GetChejanData(951).ToString().Trim());  //[11]
            balance_Data.매도호가 = int.Parse(axKHOpenAPI.GetChejanData(27).ToString().Trim());  //[12]
            balance_Data.매수호가 = int.Parse(axKHOpenAPI.GetChejanData(28).ToString().Trim()); //[13]
            balance_Data.기준가 = int.Parse(axKHOpenAPI.GetChejanData(307).ToString().Trim());   //[14]
            balance_Data.손익율 = float.Parse(axKHOpenAPI.GetChejanData(8019).ToString().Trim());   //[15]
            balance_Data.주식옵션거래단위 = axKHOpenAPI.GetChejanData(397).ToString().Trim();  //[16]

            SendDirectFile(balance_Data);
            SendDirectDb(balance_Data);
        }
        private void SendDirectFile(Balance_Data balance_Data)
        {
            String tmp = "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}";
            String tmp1 = String.Format(tmp,
                 balance_Data.기록시간,  //[0]
                 balance_Data.계좌번호,  //[1]
                 balance_Data.종목코드,  //[2]
                 balance_Data.현재가,  //[3]
                 balance_Data.보유수량,  //[4]
                 balance_Data.매입단가,  //[5]
                 balance_Data.총매입가,  //[6]
                 balance_Data.주문가능수량,  //[7]
                 balance_Data.당일순매수량,  //[8]
                 balance_Data.매도수구분,  //[9]
                 balance_Data.당일총매도손익,  //[10]
                 balance_Data.예수금,  //[11]
                 balance_Data.매도호가,  //[12]
                 balance_Data.매수호가,  //[13]
                 balance_Data.기준가,  //[14]
                 balance_Data.손익율,  //[15]
                 balance_Data.주식옵션거래단위  //[16]
             );

            String path1 = path + "\\잔고.txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path1, true);
            file.Write(tmp1.ToString());
            file.Close();
        }
        private void SendDirectDb(Balance_Data balance_Data)
        {
            using (MySqlConnection conn = new MySqlConnection(Class1.connStr))
            {
                string sql = @"INSERT into  balances (
reg_time
,account_number
,stock_code
,current_price
,possession_quantity
,purchase_price
,total_amount_of_purchase
,order_possible_quantity
,today_net_buy_quantity
,order_type
,today_sell_profit_and_loss
,deposit
,offered_price
,bid_price
,yesterday_current_price
,not_commission_profit_and_loss_rate
,stock_option_trade_unit
)
VALUES
(
@기록시간
,@계좌번호
,@종목코드
,@현재가
,@보유수량
,@매입단가
,@총매입가
,@주문가능수량
,@당일순매수량
,@매도수구분
,@당일총매도손익
,@예수금
,@매도호가
,@매수호가
,@기준가
,@손익율
,@주식옵션거래단위
);
";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@기록시간", balance_Data.기록시간);
                cmd.Parameters.AddWithValue("@계좌번호", balance_Data.계좌번호);
                cmd.Parameters.AddWithValue("@종목코드", balance_Data.종목코드);
                cmd.Parameters.AddWithValue("@현재가", balance_Data.현재가);
                cmd.Parameters.AddWithValue("@보유수량", balance_Data.보유수량);
                cmd.Parameters.AddWithValue("@매입단가", balance_Data.매입단가);
                cmd.Parameters.AddWithValue("@총매입가", balance_Data.총매입가);
                cmd.Parameters.AddWithValue("@주문가능수량", balance_Data.주문가능수량);
                cmd.Parameters.AddWithValue("@당일순매수량", balance_Data.당일순매수량);
                cmd.Parameters.AddWithValue("@매도수구분", balance_Data.매도수구분);
                cmd.Parameters.AddWithValue("@당일총매도손익", balance_Data.당일총매도손익);
                cmd.Parameters.AddWithValue("@예수금", balance_Data.예수금);
                cmd.Parameters.AddWithValue("@매도호가", balance_Data.매도호가);
                cmd.Parameters.AddWithValue("@매수호가", balance_Data.매수호가);
                cmd.Parameters.AddWithValue("@기준가", balance_Data.기준가);
                cmd.Parameters.AddWithValue("@손익율", balance_Data.손익율);
                cmd.Parameters.AddWithValue("@주식옵션거래단위", balance_Data.주식옵션거래단위);
                cmd.ExecuteNonQuery();
            }
        }
    }
}