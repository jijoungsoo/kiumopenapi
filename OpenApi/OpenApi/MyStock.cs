using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KiwoomCode;
using System.Collections.Concurrent;
using System.IO;
using OpenApi.Dto;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace OpenApi
{
    class MyStock
    {
        private MyStock()
        {

        }
        private static Object _lockStockList = new Object();

        public int profit_rate = 0;
        public int loss_rate = 0;
        public Boolean profit_status = false;
        public Boolean loss_status = false;
        public String accountNumber = "";

        private static Boolean _multiThread = true;
        private static MyStock _class1 = null;
        private static Object _object1 = new Object();
        public static MyStock getClass1Instance()
        {
            if (_multiThread == false)
            {
                if (_class1 == null)
                {
                    _class1 = new MyStock();
                }
            }
            else {
                if (_class1 == null)
                {
                    lock (_object1)
                    {
                        _class1 = new MyStock();
                    }
                }
            }
            return _class1;
        }

        private List<OPT10085_Data> stockList = new List<OPT10085_Data>();  //계좌를 하나만 사용할거라고 가정한다.
        public void UpdateStockList(REAL10002_Data real10002_data)
        {
           // FileLog.PrintF("MyStock UpdateStockList real10002_data.종목코드=>"+ real10002_data.종목코드);
            //FileLog.PrintF("MyStock UpdateStockList stockList.Count()=>" + stockList.Count());
            lock (_lockStockList)
            {
                foreach (OPT10085_Data stock in stockList)
                {
                   // FileLog.PrintF("MyStock UpdateStockList stock.종목코드=>" + stock.종목코드);
                    if (stock.종목코드.Trim().Equals(real10002_data.종목코드))
                    {
                       // FileLog.PrintF("MyStock UpdateStockList change");
                        int 현재가 = Math.Abs(real10002_data.현재가);
                        /*디비서 조회*/
                        int 보유수량 = stock.보유수량;
                        int 매입금액 = stock.매입금액;

                        int 평가금액 = 현재가 * 보유수량;
                        int 매입수수료 = Commission.GetKiwoomCommissionBuy(매입금액);
                        int 매도수수료 = Commission.GetKiwoomCommissionSell(평가금액);
                        int 수수료 = 매입수수료 + 매도수수료;
                        int 매도세금 = Commission.GetTaxSell(평가금액);
                        

                        int 손익분기매입가 = 0;
                        if (보유수량 != 0)//이게 0일경우가 있다 매도를 한상태일경우 보유수량이 0으로 리스트에 계속 존재한다.
                        {
                            손익분기매입가 = (매입수수료 + 매도수수료 + 매도세금 + 매입금액) / 보유수량;  // 무조건오림
                        }
                        int 평가손익 = 평가금액 - (매입금액 + 수수료 + 매도세금);
                        // float 수익률 = (평가손익 / 매입금액) * 100;   //int끼리 나눠서... 소수점을 버리는구나.. 이런..
                        float 수익률 = 0;
                        if (매입금액 != 0)
                        {
                            수익률 = ((float)평가손익 / (float)매입금액) * 100;
                        }
                        int 손익금액 = (평가금액 - 매입금액);

                        float 손익율 = 0;
                        if (매입금액 != 0)
                        {
                            손익율 = ((float)손익금액 / (float)매입금액) * 100;
                        }

                        stock.현재가 = 현재가;
                        stock.평가금액 = 평가금액;
                        stock.매도수수료 = 매도수수료;
                        stock.수수료 = 수수료;
                        stock.매도세금 = 매도세금;
                        stock.손익분기매입가 = 손익분기매입가;
                        stock.평가손익 = 평가손익;
                        stock.수익률 = 수익률;
                        stock.손익금액 = 손익금액;
                        stock.손익율 = 손익율;
                        dbUpdate(stock);
                        autoSale(stock);
                    }
                }
            }
        }
        private void dbUpdate(OPT10085_Data opt10085_data)
        {
            FileLog.PrintF("MyStock dbUpdate");
            using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr()))
            {
                String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = @"UPDATE opt10085s SET
 current_price=@현재가
,evaluated_price=@평가금액
,selling_commission=@매도수수료
,commission=@수수료
,selling_tax=@매도세금
,will_profit_price=@손익분기매입가
,valuation_profit_and_loss=@평가손익
,earnings_rate=@수익률
,not_commission_profit_and_loss=@손익금액
,not_commission_profit_and_loss_rate=@손익율
,updated_at=@업데이트날짜
WHERE stock_code=@종목코드
;
";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@현재가", opt10085_data.현재가);
                cmd.Parameters.AddWithValue("@평가금액", opt10085_data.평가금액);
                cmd.Parameters.AddWithValue("@매도수수료", opt10085_data.매도수수료);
                cmd.Parameters.AddWithValue("@수수료", opt10085_data.수수료);
                cmd.Parameters.AddWithValue("@매도세금", opt10085_data.매도세금);
                cmd.Parameters.AddWithValue("@손익분기매입가", opt10085_data.손익분기매입가);
                cmd.Parameters.AddWithValue("@평가손익", opt10085_data.평가손익);
                cmd.Parameters.AddWithValue("@수익률", opt10085_data.수익률);
                cmd.Parameters.AddWithValue("@손익금액", opt10085_data.손익금액);
                cmd.Parameters.AddWithValue("@손익율", opt10085_data.손익율);
                cmd.Parameters.AddWithValue("@업데이트날짜", dayTime);
                cmd.Parameters.AddWithValue("@종목코드", opt10085_data.종목코드);                
                cmd.ExecuteNonQuery();
            }
        }
        private void dbUpdateOrderStatus(OPT10085_Data opt10085_data)
        {
            FileLog.PrintF("MyStock dbUpdateOrderStatus");
            using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr()))
            {
                string sql = @"UPDATE opt10085s SET
 order_status=@주문상태
WHERE stock_code=@종목코드
AND account_number=@계좌번호
;
";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@주문상태", opt10085_data.주문상태);
                cmd.Parameters.AddWithValue("@종목코드", opt10085_data.종목코드);
                cmd.Parameters.AddWithValue("@계좌번호", opt10085_data.계좌번호);
                cmd.ExecuteNonQuery();
            }
        }
        public void reLoad(List<OPT10085_Data> opt10085_DataList)
        {
            SendDirectFile(opt10085_DataList);
            SendDirectDb(opt10085_DataList);
            String strCodeList = "";
            for (int i=0;i< opt10085_DataList.Count();i++)
            {
                strCodeList= strCodeList+ opt10085_DataList[i].종목코드;
                if(i!= opt10085_DataList.Count())
                {
                    strCodeList = strCodeList + ";";
                }

            }
            if (!strCodeList.Equals(""))
            {
                SetRealReg(strCodeList);
            }
            
        }

        public List<OPT10085_Data> getStockList()
        {
            lock (_lockStockList)
            {
                return stockList;
            }
        }

        private void SetRealReg(String strCodeList)
        {
            FileLog.PrintF("MyStock SetRealReg strCodeList=>" + strCodeList);
            String strScreenNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();
            String strFidList = "10"; //현재가만 있으면됨.
            String strRealType = "0"; //같은 화면에 실시간을 더하는지 유무 0 더하지 않는다(지우고새로) , 1 새로받는다.
            FileLog.PrintF("SetRealReg strScreenNo=>" + strScreenNo);
            FileLog.PrintF("SetRealReg strCodeList=>" + strCodeList);
            FileLog.PrintF("SetRealReg strFidList=>" + strFidList);
            FileLog.PrintF("SetRealReg strRealType=>" + strRealType);
            //Class1.getClass1Instance().getAxKHOpenAPIInstance().SetRealReg(strScreenNo, strCodeList, strFidList, strRealType);
        }

        private void autoSale(OPT10085_Data opt10085_data)
        {
            FileLog.PrintF("autoSale loss_status=>"+ loss_status+ ",loss_rate=>"+ loss_rate+ ",종목코드=>" + opt10085_data.종목코드 + ",손익율=>" + opt10085_data.손익율 + ",주문상태=>" + opt10085_data.주문상태);
            //주식체결 정보가 들어와서 주식 현재가가 변동이 있을때 자동 판매로직이 실행됨
            /*손절매*/
            if (loss_status == true)
            {
                if (opt10085_data.손익율 <= this.loss_rate && opt10085_data.주문상태==1)//주문상태가 1 즉 보여상태이어야한다.
                {
                    if (MyOrder.getClass1Instance().ExistsOrder(opt10085_data.종목코드) == false)
                    {
                        //매도주문
                        
                        int nOrderType = 2;//신규메도
                        String sCode = opt10085_data.종목코드;
                        String sScreenNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();
                        int nQty = opt10085_data.보유수량;
                        int nPrice = 0;//일단 시장가매도하자.
                        String sHogaGb = "03"; //시장가 매도
                        String sOrgOrderNo = "";//원주문번호는 공백
                        FileLog.PrintF("autoSale sScreenNo=>" + sScreenNo);
                        FileLog.PrintF("autoSale accountNumber=>" + accountNumber);
                        FileLog.PrintF("autoSale nOrderType=>" + nOrderType);
                        FileLog.PrintF("autoSale sCode=>" + sCode);
                        FileLog.PrintF("autoSale nQty=>" + nQty);
                        FileLog.PrintF("autoSale nPrice=>" + nPrice);
                        FileLog.PrintF("autoSale sHogaGb=>" + sHogaGb);
                        FileLog.PrintF("autoSale sOrgOrderNo=>" + sOrgOrderNo);

                        //아 이거 1초에 5번 즉 0.2초 제한이 여기도 있다. ㅠㅠ이렇게 바로 보내면 안된다...
                        int ret = AppLib.getClass1Instance().getAxKHOpenAPIInstance().SendOrder("손절매_매도주문", sScreenNo, accountNumber, nOrderType, sCode, nQty, nPrice, sHogaGb, sOrgOrderNo);
                        FileLog.PrintF("MyStock AutoSale ret=>" + ret);
                        //상태가 하나더 있어야겠다 주문접수를 한상태인거는 다시 매도를 시도하면 안되므로
                        opt10085_data.주문상태 = 2;
                        dbUpdateOrderStatus(opt10085_data);
                    }
                }

            }
            /*이익실현*/
            if (profit_status == true)
            {
                if (opt10085_data.손익율 >= this.profit_rate && opt10085_data.주문상태 == 1)//주문상태가 1 즉 보여상태이어야한다.
                {
                    //매도주문
                    if (MyOrder.getClass1Instance().ExistsOrder(opt10085_data.종목코드) == false)
                    {
                        
                        int nOrderType = 2;//신규메도
                        String sCode = opt10085_data.종목코드;
                        int nQty = opt10085_data.보유수량;
                        int nPrice = 0;//일단 시장가매도하자.
                        String sHogaGb = "03"; //시장가 매도
                        String sOrgOrderNo = "";//원주문번호는 공백

                        String sScreenNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();


                        FileLog.PrintF("autoSale sScreenNo=>" + sScreenNo);
                        FileLog.PrintF("autoSale accountNumber=>" + accountNumber);
                        FileLog.PrintF("autoSale nOrderType=>" + nOrderType);
                        FileLog.PrintF("autoSale sCode=>" + sCode);
                        FileLog.PrintF("autoSale nQty=>" + nQty);
                        FileLog.PrintF("autoSale nPrice=>" + nPrice);
                        FileLog.PrintF("autoSale sHogaGb=>" + sHogaGb);
                        FileLog.PrintF("autoSale sOrgOrderNo=>" + sOrgOrderNo);
                        int ret = AppLib.getClass1Instance().getAxKHOpenAPIInstance().SendOrder("이익매_매도주문", sScreenNo, accountNumber, nOrderType, sCode, nQty, nPrice, sHogaGb, sOrgOrderNo);
                        FileLog.PrintF("MyStock AutoSale ret=>" + ret);
                        opt10085_data.주문상태 = 2;
                        dbUpdateOrderStatus(opt10085_data);
                    }
                }
            }
        }

        private void SendDirectFile(List<OPT10085_Data> opt10085_DataList)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbAll = new StringBuilder();
            /*일자,계좌번호,종목코드,종목명,현재가,매입가,매입금액,보유수량,당일매도손익,당일매매수수료,당일매매세금,대출일,결제잔고,청산가능수량,신용금액,신용이자
            ,만기일,평가손익,수익률,평가금액,수수료,매입수수료,매도수수료,매도세금,손익분기매입가,손익금액,손익율*/
            sb.Append("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}");
            String tmp = sb.ToString();
            foreach (OPT10085_Data opt10085_Data in opt10085_DataList)
            {
                String tmp1 = String.Format(tmp,
                    opt10085_Data.구매일자,  //[0]
                    opt10085_Data.계좌번호,  //[1]
                    opt10085_Data.종목코드,  //[2]
                    opt10085_Data.종목명,  //[3]
                    opt10085_Data.현재가,  //[4]
                    opt10085_Data.매입가,  //[5]
                    opt10085_Data.매입금액,  //[6]
                    opt10085_Data.보유수량,  //[7]
                    opt10085_Data.당일매도손익,  //[8]
                    opt10085_Data.당일매매수수료,  //[9]
                    opt10085_Data.당일매매세금,  //[10]
                    opt10085_Data.신용구분,  //[11]
                    opt10085_Data.대출일,  //[12]
                    opt10085_Data.결제잔고,  //[13]
                    opt10085_Data.청산가능수량,  //[14]
                    opt10085_Data.신용금액,  //[15]
                    opt10085_Data.신용이자,  //[16]
                    opt10085_Data.만기일,  //[17]
                    opt10085_Data.평가손익,  //[18]
                    opt10085_Data.수익률,  //[19]
                    opt10085_Data.평가금액,  //[20]
                    opt10085_Data.수수료,  //[21]
                    opt10085_Data.매입수수료,  //[22]
                    opt10085_Data.매도수수료,  //[23]
                    opt10085_Data.매도세금,  //[24]
                    opt10085_Data.손익분기매입가,  //[25]
                    opt10085_Data.손익금액,  //[26]
                    opt10085_Data.손익율  //[27]
                );
                sbAll.AppendLine(tmp1);
            }

            String ret = sbAll.ToString();

            System.IO.StreamWriter file = new System.IO.StreamWriter(Config.GetPath() + "TEST_OPT10085.txt", true);
            file.WriteLine(ret.ToString());
            file.Close();
        }

        private void SendDirectDb(List<OPT10085_Data> opt10085_DataList)
        {
            lock (_lockStockList)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr()))
                    {
                         String sql1= "DELETE FROM opt10085s;";
                        conn.Open();
                        MySqlTransaction tr = conn.BeginTransaction();
                        String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand(sql1, conn, tr);
                            cmd.ExecuteNonQuery();
                            string sql2 = @"INSERT INTO opt10085s (
date_of_purchase  
,account_number
,stock_code
,stock_name
,current_price  
,purchase_price  
,total_amount_of_purchase 
,possession_quantity 
,today_sell_profit_and_loss 
,today_commission 
,today_tax 
,credit_gubun
,loan_date
,payment_balance 
,sellable_quantity 
,credit_amount 
,credit_interest 
,expiration_date
,valuation_profit_and_loss 
,earnings_rate
,evaluated_price 
,commission 
,buying_commission 
,selling_commission 
,selling_tax 
,will_profit_price 
,not_commission_profit_and_loss 
,not_commission_profit_and_loss_rate
,order_status
,created_at
,updated_at
)
VALUES";
String sql2_1= @"
(
@구매일자{0}
,@계좌번호{0}
,@종목코드{0}
,@종목명{0}
,@현재가{0}
,@매입가{0}
,@매입금액{0}
,@보유수량{0}
,@당일매도손익{0}
,@당일매매수수료{0}
,@당일매매세금{0}
,@신용구분{0}
,@대출일{0}
,@결제잔고{0}
,@청산가능수량{0}
,@신용금액{0}
,@신용이자{0}
,@만기일{0}
,@평가손익{0}
,@수익률{0}
,@평가금액{0}
,@수수료{0}
,@매입수수료{0}
,@매도수수료{0}
,@매도세금{0}
,@손익분기매입가{0}
,@손익금액{0}
,@손익율{0}
,@주문상태{0}
,@등록날짜{0}
,@업데이트날짜{0}
),";
                            StringBuilder queryBuilder = new StringBuilder(sql2);
                            for (int i = 0;  i< opt10085_DataList.Count(); i++)
                            {
                                queryBuilder.AppendFormat(sql2_1, i);
                                //once we're done looping we remove the last ',' and replace it with a ';'
                                if (i == opt10085_DataList.Count()-1)
                                {
                                    queryBuilder.Replace(',', ';', queryBuilder.Length - 1, 1);
                                }
                            }
                            String sql2_2 = queryBuilder.ToString();
                            FileLog.PrintF("SendDirectDb2 sql2_2:" + sql2_2.ToString());
                            cmd.CommandText = sql2_2;
                            for (int i = 0; i < opt10085_DataList.Count(); i++)
                            {
                                cmd.Parameters.AddWithValue("@구매일자" + i, opt10085_DataList[i].구매일자);
                                cmd.Parameters.AddWithValue("@계좌번호" + i, opt10085_DataList[i].계좌번호);
                                cmd.Parameters.AddWithValue("@종목코드" + i, opt10085_DataList[i].종목코드);
                                cmd.Parameters.AddWithValue("@종목명" + i, opt10085_DataList[i].종목명);
                                cmd.Parameters.AddWithValue("@현재가" + i, opt10085_DataList[i].현재가);
                                cmd.Parameters.AddWithValue("@매입가" + i, opt10085_DataList[i].매입가);
                                cmd.Parameters.AddWithValue("@매입금액" + i, opt10085_DataList[i].매입금액);
                                cmd.Parameters.AddWithValue("@보유수량" + i, opt10085_DataList[i].보유수량);
                                cmd.Parameters.AddWithValue("@당일매도손익" + i, opt10085_DataList[i].당일매도손익);
                                cmd.Parameters.AddWithValue("@당일매매수수료" + i, opt10085_DataList[i].당일매매수수료);
                                cmd.Parameters.AddWithValue("@당일매매세금" + i, opt10085_DataList[i].당일매매세금);
                                cmd.Parameters.AddWithValue("@신용구분" + i, opt10085_DataList[i].신용구분);
                                cmd.Parameters.AddWithValue("@대출일" + i, opt10085_DataList[i].대출일);
                                cmd.Parameters.AddWithValue("@결제잔고" + i, opt10085_DataList[i].결제잔고);
                                cmd.Parameters.AddWithValue("@청산가능수량" + i, opt10085_DataList[i].청산가능수량);
                                cmd.Parameters.AddWithValue("@신용금액" + i, opt10085_DataList[i].신용금액);
                                cmd.Parameters.AddWithValue("@신용이자" + i, opt10085_DataList[i].신용이자);
                                cmd.Parameters.AddWithValue("@만기일" + i, opt10085_DataList[i].만기일);
                                cmd.Parameters.AddWithValue("@평가손익" + i, opt10085_DataList[i].평가손익);
                                cmd.Parameters.AddWithValue("@수익률" + i, opt10085_DataList[i].수익률);
                                cmd.Parameters.AddWithValue("@평가금액" + i, opt10085_DataList[i].평가금액);
                                cmd.Parameters.AddWithValue("@수수료" + i, opt10085_DataList[i].수수료);
                                cmd.Parameters.AddWithValue("@매입수수료" + i, opt10085_DataList[i].매입수수료);
                                cmd.Parameters.AddWithValue("@매도수수료" + i, opt10085_DataList[i].매도수수료);
                                cmd.Parameters.AddWithValue("@매도세금" + i, opt10085_DataList[i].매도세금);
                                cmd.Parameters.AddWithValue("@손익분기매입가" + i, opt10085_DataList[i].손익분기매입가);
                                cmd.Parameters.AddWithValue("@손익금액" + i, opt10085_DataList[i].손익금액);
                                cmd.Parameters.AddWithValue("@손익율" + i, opt10085_DataList[i].손익율);
                                cmd.Parameters.AddWithValue("@주문상태" + i, 1);//1 은 보유를 의미
                                cmd.Parameters.AddWithValue("@업데이트날짜" + i, dayTime);
                                cmd.Parameters.AddWithValue("@등록날짜" + i, dayTime);
                            }
                            cmd.ExecuteNonQuery();
                            String sql3 = @"SELECT
DATE_OF_PURCHASE  
,ACCOUNT_NUMBER
,STOCK_CODE
,STOCK_NAME
,CURRENT_PRICE  
,PURCHASE_PRICE  
,TOTAL_AMOUNT_OF_PURCHASE 
,POSSESSION_QUANTITY 
,TODAY_SELL_PROFIT_AND_LOSS 
,TODAY_COMMISSION 
,TODAY_TAX 
,CREDIT_GUBUN
,LOAN_DATE
,PAYMENT_BALANCE 
,SELLABLE_QUANTITY 
,CREDIT_AMOUNT 
,CREDIT_INTEREST 
,EXPIRATION_DATE
,VALUATION_PROFIT_AND_LOSS 
,EARNINGS_RATE
,EVALUATED_PRICE 
,COMMISSION 
,BUYING_COMMISSION 
,SELLING_COMMISSION 
,SELLING_TAX 
,WILL_PROFIT_PRICE 
,NOT_COMMISSION_PROFIT_AND_LOSS 
,NOT_COMMISSION_PROFIT_AND_LOSS_RATE
,ORDER_STATUS
FROM opt10085s
ORDER BY date_of_purchase DESC";
                            cmd.CommandText = sql3;
                            MySqlDataReader rdr=cmd.ExecuteReader();
                            // 다음 레코드 계속 가져와서 루핑
                            while (rdr.Read())
                            {
                                // C# 인덱서를 사용하여
                                // 필드 데이타 엑세스
                                OPT10085_Data tmp = new OPT10085_Data();
                                tmp.구매일자 = rdr["DATE_OF_PURCHASE"].ToString().Trim();
                                tmp.계좌번호 = rdr["ACCOUNT_NUMBER"].ToString().Trim();
                                tmp.종목코드 = rdr["STOCK_CODE"].ToString().Trim();
                                tmp.종목명 = rdr["STOCK_NAME"].ToString().Trim();
                                tmp.현재가 = int.Parse(rdr["CURRENT_PRICE"].ToString().Trim());
                                tmp.매입가 = int.Parse(rdr["PURCHASE_PRICE"].ToString().Trim());
                                tmp.매입금액 = int.Parse(rdr["TOTAL_AMOUNT_OF_PURCHASE"].ToString().Trim());
                                tmp.보유수량 = int.Parse(rdr["POSSESSION_QUANTITY"].ToString().Trim());
                                tmp.당일매도손익 = int.Parse(rdr["TODAY_SELL_PROFIT_AND_LOSS"].ToString().Trim());
                                tmp.당일매매수수료 = int.Parse(rdr["TODAY_COMMISSION"].ToString().Trim());
                                tmp.당일매매세금 = int.Parse(rdr["TODAY_TAX"].ToString().Trim());
                                tmp.신용구분 = rdr["CREDIT_GUBUN"].ToString().Trim();
                                tmp.대출일 = rdr["LOAN_DATE"].ToString().Trim();
                                tmp.결제잔고 = int.Parse(rdr["PAYMENT_BALANCE"].ToString().Trim());
                                tmp.청산가능수량 = int.Parse(rdr["SELLABLE_QUANTITY"].ToString().Trim());
                                tmp.신용금액 = int.Parse(rdr["CREDIT_AMOUNT"].ToString().Trim());
                                tmp.신용이자 = int.Parse(rdr["CREDIT_INTEREST"].ToString().Trim());
                                tmp.만기일 = rdr["EXPIRATION_DATE"].ToString().Trim();
                                tmp.평가손익 = int.Parse(rdr["VALUATION_PROFIT_AND_LOSS"].ToString().Trim());
                                tmp.수익률 = float.Parse(rdr["EARNINGS_RATE"].ToString().Trim());
                                tmp.평가금액 = int.Parse(rdr["EVALUATED_PRICE"].ToString().Trim());
                                tmp.수수료 = int.Parse(rdr["COMMISSION"].ToString().Trim());
                                tmp.매입수수료 = int.Parse(rdr["BUYING_COMMISSION"].ToString().Trim());
                                tmp.매도수수료 = int.Parse(rdr["SELLING_COMMISSION"].ToString().Trim());
                                tmp.매도세금 = int.Parse(rdr["SELLING_TAX"].ToString().Trim());
                                tmp.손익분기매입가 = int.Parse(rdr["WILL_PROFIT_PRICE"].ToString().Trim());
                                tmp.손익금액 = int.Parse(rdr["NOT_COMMISSION_PROFIT_AND_LOSS"].ToString().Trim());
                                tmp.손익율 = float.Parse(rdr["NOT_COMMISSION_PROFIT_AND_LOSS_RATE"].ToString().Trim());
                                tmp.주문상태 = int.Parse(rdr["ORDER_STATUS"].ToString().Trim());
                                stockList.Add(tmp);
                            }
                            // 사용후 닫음
                            rdr.Close();
                            tr.Commit();
                            
                        } catch (MySqlException ex2) {
                            try
                            {
                                tr.Rollback();

                            }
                            catch (MySqlException ex1)
                            {
                                FileLog.PrintF("SendDirectDb1 Error:" + ex1.ToString());
                            }
                            FileLog.PrintF("SendDirectDb2 Error:" + ex2.ToString());

                        }
                    }
                }
                       catch (MySqlException ex3)
                        {
                            FileLog.PrintF("SendDirectDb3 Error:" + ex3.ToString());
                        }
            }
        }
    }
}
