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
    ///  [ REAL10003 : 주식우선호가 ]
    ///</summary>
    public class REAL10003 : ReceiveRealData
    {
        private static readonly String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public REAL10003(){
            FileLog.PrintF("REAL10003");
        }
        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            /*
            
                [27] = (최우선)매도호가         //(0)
            	[28] = (최우선)매수호가         //(1)
            */
            FileLog.PrintF(String.Format("최우선_매도호가 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 27).Trim()));   //[0]
            FileLog.PrintF(String.Format("최우선_매수호가 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 28).Trim()));     //[1]
            FileLog.PrintF(String.Format("종목코드 : {0} ==>", e.sRealKey.ToString().Trim()));
            FileLog.PrintF(String.Format("RealName : {0} ==>", e.sRealType.ToString().Trim()));
            FileLog.PrintF(String.Format("sRealData : {0} ==>", e.sRealData.ToString().Trim()));
                        
            String 현재시간 = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            REAL10003_Data real10003_data = new REAL10003_Data();
            real10003_data.현재시간 = 현재시간;
            real10003_data.최우선_매도호가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 27).Trim());         //[0]
            real10003_data.최우선_매수호가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 28).Trim());         //[1]
            real10003_data.종목코드 = e.sRealKey.ToString().Trim(); //[2]
            real10003_data.RealName = e.sRealType.ToString().Trim(); //[3]
            
            if (1 == 2) {
                SendDirectFile(real10003_data);
            } else {
                SendDirectDb(real10003_data);
            }
        }

        private void SendDirectFile(REAL10003_Data real10003_data)
        {

            String tmp = "{0}|{1}|{2}|{3}|{4}";
            String tmp1 = String.Format(tmp,
                 real10003_data.현재시간,
                 real10003_data.종목코드,
                    real10003_data.최우선_매도호가,
                     real10003_data.최우선_매수호가,
                     real10003_data.RealName
             );

            String path1 = path + "\\주식우선호가.txt";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path1, true);
            file.Write(tmp1.ToString());
            file.Close();
        }
        private void SendDirectDb(REAL10003_Data real10003_data)
        {
            using (MySqlConnection conn = new MySqlConnection(Class1.connStr))
            {

                string sql = @"INSERT into realtime_best_offered_and_bids (
stock_date
,stock_code
,offered_price
,bid_price
,bid_price
)
VALUES
(
@현재시간
,@종목코드
,@최우선_매도호가
,@최우선_매수호가
);
";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@현재시간", real10003_data.현재시간);
                cmd.Parameters.AddWithValue("@종목코드", real10003_data.종목코드);
                cmd.Parameters.AddWithValue("@최우선_매도호가", real10003_data.최우선_매도호가);
                cmd.Parameters.AddWithValue("@최우선_매수호가", real10003_data.최우선_매수호가);
                cmd.ExecuteNonQuery();  //기존 계좌수익률을 삭제하고
            }
        }
    }
}
