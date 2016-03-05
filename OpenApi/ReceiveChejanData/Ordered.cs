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
    class Ordered : ReceiveChejanData
    {
        private static readonly String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public Ordered()
        {
            FileLog.PrintF("Ordered");
        }


        public override void ReceivedData(AxKHOpenAPI axKHOpenAPI, _DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            /*주문체결
9201 계좌번호
9203 주문번호
9205 관리자사번
9001 종목코드, 업종코드
912 주문업무분류(JJ: 주식주문, FJ: 선물옵션, JG: 주식잔고, FG: 선물옵션잔고)
913 주문상태(10:원주문, 11:정정주문, 12:취소주문, 20:주문확인, 21:정정확인, 22:취소확인, 90 - 92:주문거부)
302 종목명
900 주문수량
901 주문가격
902 미체결수량
903 체결누계금액
904 원주문번호
905 주문구분(+현금내수, -현금매도…)
906 매매구분(보통, 시장가…)
907 매도수구분(1:매도, 2:매수)
908 주문 / 체결시간(HHMMSSMS)
909 체결번호
910 체결가
911 체결량
10 현재가, 체결가, 실시간종가
27(최우선)매도호가
28(최우선)매수호가
914 단위체결가
915 단위체결량
938 당일매매 수수료
939 당일매매세금
*/
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 구분 : 주문접수--통보");
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문/체결시간=>" + axKHOpenAPI.GetChejanData(908));   //[0]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 계좌번호=>" + axKHOpenAPI.GetChejanData(9201));   //[1]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문번호=>" + axKHOpenAPI.GetChejanData(9203));   //[2]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 관리자사번=>" + axKHOpenAPI.GetChejanData(9205));   //[3]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 종목코드, 업종코드=>" + axKHOpenAPI.GetChejanData(9001));   //[4]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문업무분류=>" + axKHOpenAPI.GetChejanData(912));   //[5]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문상태=>" + axKHOpenAPI.GetChejanData(913));   //[6]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 종목명=>" + axKHOpenAPI.GetChejanData(302));   //[7]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문수량=>" + axKHOpenAPI.GetChejanData(900));   //[8]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문가격=>" + axKHOpenAPI.GetChejanData(901));   //[9]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 미체결수량=>" + axKHOpenAPI.GetChejanData(902));   //[10]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 체결누계금액=>" + axKHOpenAPI.GetChejanData(903));   //[11]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 원주문번호=>" + axKHOpenAPI.GetChejanData(904));   //[12]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문구분(+현금내수, -현금매도…)=>" + axKHOpenAPI.GetChejanData(905));   //[13]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 매매구분(보통, 시장가…)=>" + axKHOpenAPI.GetChejanData(906));   //[14]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 매도수구분(1:매도, 2:매수)" + axKHOpenAPI.GetChejanData(907));   //[15]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 체결번호" + axKHOpenAPI.GetChejanData(909));   //[16]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 체결가=>" + axKHOpenAPI.GetChejanData(910));   //[17]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 체결량=>" + axKHOpenAPI.GetChejanData(911));   //[18]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 현재가, 체결가, 실시간종가=>" + axKHOpenAPI.GetChejanData(10));   //[19]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData (최우선)매도호가=>" + axKHOpenAPI.GetChejanData(27));   //[20]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData (최우선)매수호가=>" + axKHOpenAPI.GetChejanData(28));   //[21]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 단위체결가=>" + axKHOpenAPI.GetChejanData(914));   //[22]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 단위체결량=>" + axKHOpenAPI.GetChejanData(915));   //[23]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일매매 수수료=>" + axKHOpenAPI.GetChejanData(938));   //[24]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일매매세금=>" + axKHOpenAPI.GetChejanData(939));   //[25]

            /*카페 정보아래는 확인이 필요*/
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 화면번호=>" + axKHOpenAPI.GetChejanData(920));   //[26]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 터미널번호=>" + axKHOpenAPI.GetChejanData(921));   //[27]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 신용구분=>" + axKHOpenAPI.GetChejanData(922));   //[28]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 대출일=>" + axKHOpenAPI.GetChejanData(923));   //[29]
            Order_Data order_Data = new Order_Data();

            String 현재일자 = DateTime.Now.ToString("yyyy-MM-dd");
            String 체결시간TMP = axKHOpenAPI.GetChejanData(908).ToString().Trim();   //[0]
            //체결시간이 6자리이므로 HHMMSS ==> HH:MM:SS로 바꿔야한다.
            String 체결시간 = 체결시간TMP.Substring(0, 2) + ":" + 체결시간TMP.Substring(2, 2) + ":" + 체결시간TMP.Substring(4, 2);
            체결시간 = 현재일자 + " " + 체결시간;
            order_Data.체결시간 = 체결시간;
            order_Data.계좌번호 = axKHOpenAPI.GetChejanData(9201).ToString().Trim(); //[1]
            order_Data.주문번호 = axKHOpenAPI.GetChejanData(9203).ToString().Trim();  //[2]
            order_Data.관리자사번 = axKHOpenAPI.GetChejanData(9205).ToString().Trim();  //[3]
            order_Data.종목코드 = axKHOpenAPI.GetChejanData(9001).ToString().Trim();  //[4]
            order_Data.주문업무분류 = axKHOpenAPI.GetChejanData(912).ToString().Trim();   //[5]
            order_Data.주문상태 = axKHOpenAPI.GetChejanData(913).ToString().Trim();  //[6]
            order_Data.종목명 = axKHOpenAPI.GetChejanData(302).ToString().Trim();   //[7]
            order_Data.주문수량 = int.Parse(axKHOpenAPI.GetChejanData(900).ToString().Trim());  //[8]
            order_Data.주문가격 = int.Parse(axKHOpenAPI.GetChejanData(901).ToString().Trim());  //[9]
            order_Data.미체결수량 = int.Parse(axKHOpenAPI.GetChejanData(902).ToString().Trim());   //[10]
            order_Data.체결누계금액 = int.Parse(axKHOpenAPI.GetChejanData(903).ToString().Trim());  //[11]
            order_Data.원주문번호 = axKHOpenAPI.GetChejanData(904).ToString().Trim();   //[12]
            order_Data.주문구분 = axKHOpenAPI.GetChejanData(905).ToString().Trim();  //[13]
            order_Data.매매구분 = axKHOpenAPI.GetChejanData(906).ToString().Trim();   //[14]
            order_Data.매도수구분 = int.Parse(axKHOpenAPI.GetChejanData(907).ToString().Trim());   //[15]
            order_Data.체결번호 = axKHOpenAPI.GetChejanData(909).ToString().Trim();  //[16]
            order_Data.체결가 = int.Parse(axKHOpenAPI.GetChejanData(910).ToString().Trim());    //[17]
            order_Data.체결량 = int.Parse(axKHOpenAPI.GetChejanData(911).ToString().Trim());   //[18]
            order_Data.현재가 = int.Parse(axKHOpenAPI.GetChejanData(10).ToString().Trim());   //[19]
            order_Data.매도호가 = int.Parse(axKHOpenAPI.GetChejanData(27).ToString().Trim());   //[20]
            order_Data.매수호가 = int.Parse(axKHOpenAPI.GetChejanData(28).ToString().Trim());   //[21]
            order_Data.단위체결가 = int.Parse(axKHOpenAPI.GetChejanData(914).ToString().Trim());  //[22]
            order_Data.단위체결량 = int.Parse(axKHOpenAPI.GetChejanData(915).ToString().Trim());  //[23]
            order_Data.당일매매수수료 = int.Parse(axKHOpenAPI.GetChejanData(938).ToString().Trim());   //[24]
            order_Data.당일매매세금 = int.Parse(axKHOpenAPI.GetChejanData(939).ToString().Trim());    //[25]

            SendDirectFile(order_Data);
            SendDirectDb(order_Data);
        }
        private void SendDirectFile(Order_Data order_Data)
        {
            String tmp = "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}";
            String tmp1 = String.Format(tmp,
                order_Data.체결시간, //[0]
                order_Data.계좌번호, //[1]
                order_Data.주문번호, //[2]
                order_Data.관리자사번, //[3]
                order_Data.종목코드, //[4]
                order_Data.주문업무분류, //[5]
                order_Data.주문상태, //[6]
                order_Data.종목명, //[7]
                order_Data.주문수량, //[8]
                order_Data.주문가격, //[9]
                order_Data.미체결수량, //[10]
                order_Data.체결누계금액, //[11]
                order_Data.원주문번호, //[12]
                order_Data.주문구분, //[13]
                order_Data.매매구분, //[14]
                order_Data.매도수구분, //[15]
                order_Data.체결번호, //[16]
                order_Data.체결가, //[17]
                order_Data.체결량, //[18]
                order_Data.현재가, //[19]
                order_Data.매도호가, //[20]
                order_Data.매수호가, //[21]
                order_Data.단위체결가, //[22]
                order_Data.단위체결량, //[23]
                order_Data.당일매매수수료, //[24]
                order_Data.당일매매세금 //[25]
             );

            String path1 = path + "\\주문체결.txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path1, true);
            file.Write(tmp1.ToString());
            file.Close();
        }
        private void SendDirectDb(Order_Data order_Data)
        {
            using (MySqlConnection conn = new MySqlConnection(Class1.connStr))
            {
                string sql = @"INSERT into  ordereds (
contract_time
,account_number
,order_number
,manager_number
,stock_code
,order_business_classification
,order_status
,stock_name
,order_quantity
,order_price
,not_contract_quantity
,contract_total_amount
,last_order_number
,order_gubun
,trade_gubun
,order_type
,contract_number
,contract_price
,contract_quantity
,current_price
,offered_price
,bid_price
,contract_price_unit
,contract_price_quantity
,today_commission
,today_tax
)
VALUES
(
@체결시간
,@계좌번호
,@주문번호
,@관리자사번
,@종목코드
,@주문업무분류
,@주문상태
,@종목명
,@주문수량
,@주문가격
,@미체결수량
,@체결누계금액
,@원주문번호
,@주문구분
,@매매구분
,@매도수구분
,@체결번호
,@체결가
,@체결량
,@현재가
,@매도호가
,@매수호가
,@단위체결가
,@단위체결량
,@당일매매수수료
,@당일매매세금
);
";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@체결시간", order_Data.체결시간);
                cmd.Parameters.AddWithValue("@계좌번호", order_Data.계좌번호);
                cmd.Parameters.AddWithValue("@주문번호", order_Data.주문번호);
                cmd.Parameters.AddWithValue("@관리자사번", order_Data.관리자사번);
                cmd.Parameters.AddWithValue("@종목코드", order_Data.종목코드);
                cmd.Parameters.AddWithValue("@주문업무분류", order_Data.주문업무분류);
                cmd.Parameters.AddWithValue("@주문상태", order_Data.주문상태);
                cmd.Parameters.AddWithValue("@종목명", order_Data.종목명);
                cmd.Parameters.AddWithValue("@주문수량", order_Data.주문수량);
                cmd.Parameters.AddWithValue("@주문가격", order_Data.주문가격);
                cmd.Parameters.AddWithValue("@미체결수량", order_Data.미체결수량);
                cmd.Parameters.AddWithValue("@체결누계금액", order_Data.체결누계금액);
                cmd.Parameters.AddWithValue("@원주문번호", order_Data.원주문번호);
                cmd.Parameters.AddWithValue("@주문구분", order_Data.주문구분);
                cmd.Parameters.AddWithValue("@매매구분", order_Data.매매구분);
                cmd.Parameters.AddWithValue("@매도수구분", order_Data.매도수구분);
                cmd.Parameters.AddWithValue("@체결번호", order_Data.체결번호);
                cmd.Parameters.AddWithValue("@체결가", order_Data.체결가);
                cmd.Parameters.AddWithValue("@체결량", order_Data.체결량);
                cmd.Parameters.AddWithValue("@현재가", order_Data.현재가);
                cmd.Parameters.AddWithValue("@매도호가", order_Data.매도호가);
                cmd.Parameters.AddWithValue("@매수호가", order_Data.매수호가);
                cmd.Parameters.AddWithValue("@단위체결가", order_Data.단위체결가);
                cmd.Parameters.AddWithValue("@단위체결량", order_Data.단위체결량);
                cmd.Parameters.AddWithValue("@당일매매수수료", order_Data.당일매매수수료);
                cmd.Parameters.AddWithValue("@당일매매세금", order_Data.당일매매세금);

                cmd.ExecuteNonQuery();  
            }
        }
    }
}