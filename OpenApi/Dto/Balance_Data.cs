using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class Balance_Data
    {
        public String 기록시간; // => "기록시간"
        public String 계좌번호; // => "계좌번호"
        public String 종목코드; // => "종목코드, 업종코드"
        public int 현재가; // => "현재가, 체결가, 실시간종가"
        public int 보유수량; // => "보유수량"
        public int 매입단가; // => "매입단가"
        public int 총매입가; // => "총매입가"
        public int 주문가능수량; // => "주문가능수량"
        public int 당일순매수량; // => "당일순매수량"
        public int 매도수구분; // => "매도 / 매수구분"
        public int 당일총매도손익; // => "당일 총 매도 손익"
        public int 예수금; // => "예수금"
        public int 매도호가; // => "(최우선)매도호가"
        public int 매수호가; // => "(최우선)매수호가"
        public int 기준가; //  => "기준가(어제종가)"
        public float 손익율; //  => "손익율"
        public String 주식옵션거래단위; //  => "주식옵션거래단위"
    }
}
