using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class Order_Data
    {
      public String 체결시간;  // => "주문/체결시간"
      public String 계좌번호;  // => "계좌번호"
      public String 주문번호;  // => "주문번호"
      public String 관리자사번;  // => "관리자사번"
      public String 종목코드;  // => "종목코드, 업종코드"
      public String 주문업무분류;  // => "주문업무분류"
      public String 주문상태;  // => "주문상태"
      public String 종목명;  // => "종목명"
      public int 주문수량;  // => "주문수량"
      public int 주문가격;  // => "주문가격"
      public int 미체결수량;  // => "미체결수량"
      public int 체결누계금액;  // => "체결누계금액"
      public String 원주문번호;  // => "원주문번호"
      public String 주문구분;  // => "주문구분(+현금내수, -현금매도…)"
      public String 매매구분;  // => "매매구분(보통, 시장가…)"
      public int 매도수구분;  // => "매도수구분(1:매도, 2:매수)"
      public String 체결번호;  // => "체결번호"
      public int 체결가;  // => "체결가"
      public int 체결량;  // => "체결가"
        public int 현재가;  // => "현재가, 체결가, 실시간종가"
      public int 매도호가;  // => "(최우선)매도호가"
      public int 매수호가;  // => "(최우선)매수호가"
      public int 단위체결가;  // => "단위체결가"
      public int 단위체결량;  // => "단위체결량"
      public int 당일매매수수료;  // => "당일매매수수료"
      public int 당일매매세금;  // => "당일매매세금"
    }
}
