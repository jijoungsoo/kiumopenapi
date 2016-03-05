using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace OpenApi.ReceiveRealData
{
    public class ReceiveRealDataFactory
    {
        private ReceiveRealDataFactory()
        {

        }
        
        private static Boolean _multiThread = true;
        private static Object object1 = new Object();
        private static ReceiveRealDataFactory _class1 = null;
        public static ReceiveRealDataFactory getClass1Instance() {
            if (_multiThread == false) {
                if (_class1 == null) {
                    _class1 = new ReceiveRealDataFactory();
                }
            } else {
                if (_class1 == null) {
                    lock (object1) {
                        _class1 = new ReceiveRealDataFactory();
                    }
                }
            }
            return _class1;
        }

        public void runReceiveRealData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e) {
            ReceiveRealData rt = getReceiveRealData(e.sRealType);
            if(rt!=null) { 
                rt.ReceivedData(axKHOpenAPI, e);
            }
        }

        public ReceiveRealData getReceiveRealData(String sRealType) {
            if ("주식시세".Equals(sRealType)) {
                return new REAL10001(); //[ REAL10001 : 주식시세 ] 
            } else if("주식체결".Equals(sRealType)) {
                return new REAL10002(); //[ REAL10002 : 주식체결 ] 
            } else if ("주식우선호가".Equals(sRealType)) {
                return new REAL10003(); //[ REAL10003 : 주식우선호가 ] 
            } else if ("주식호가잔량".Equals(sRealType)) {
                return new REAL10004(); //[ REAL10004 : 주식호가잔량 ] 
            }
            else {
                return null;
            }
        }
    }
}
