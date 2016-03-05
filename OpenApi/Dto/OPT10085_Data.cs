using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class OPT10085_Data
    {
        public String 구매일자;
        public String 계좌번호;
        public String 종목코드;
        public String 종목명;
        public int 현재가;
        public int 매입가;
        public int 매입금액;
        public int 보유수량;
        public int 당일매도손익;
        public int 당일매매수수료;
        public int 당일매매세금;
        public String 신용구분;
        public String 대출일;
        public int 결제잔고;
        public int 청산가능수량;
        public int 신용금액;
        public int 신용이자;
        public String 만기일;
        public int 평가손익;
        public float 수익률;
        public int 평가금액;
        public int 수수료;
        public int 매입수수료;
        public int 매도수수료;
        public int 매도세금;
        public int 손익분기매입가;
        public int 손익금액;
        public float 손익율;
        public int 주문상태=1;  /*1<==보유,2<==주문접수,<==주문체결*/
    }
}
