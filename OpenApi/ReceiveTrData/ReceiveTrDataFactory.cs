using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.ReceiveTrData
{
    public class ReceiveTrDataFactory
    {
        public ReceiveTrDataFactory()
        {

        }
        public ReceiveTrData getReceiveTrData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            if ("OPW00004".Equals(e.sTrCode)) //[ OPW00004 : 계좌평가현황요청 ]
            {
                return new OPW00004(axKHOpenAPI, e);
            }
            else if ("OPT10081".Equals(e.sTrCode)) //[ OPT10081 : 주식일봉차트조회요청 ]
            {
                return new OPT10081(axKHOpenAPI, e);
            }
            else if ("OPT10059".Equals(e.sTrCode)) //[ OPT10060 : 종목별투자자기관별차트요청  ]
            {
                return new OPT10059(axKHOpenAPI, e);
            }
            else {
                return null;
            }
        }
    }
}
