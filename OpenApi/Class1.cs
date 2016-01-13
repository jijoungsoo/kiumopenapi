using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxKHOpenAPILib;

namespace OpenApi
{

    public class Class1
    {
        private static Boolean _multiThread = false;
        private int _scrNum = 5000;
        private Class1()
        {

        }

        public AxKHOpenAPI getAxKHOpenAPIInstance()
        {
            return axKHOpenAPI;
        }

        public void setAxKHOpenAPIInstance(AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI = axKHOpenAPI;
        }

        private   AxKHOpenAPI axKHOpenAPI;

        private static Class1 _class1 = null;
        public static Class1 getClass1Instance()
        {
            if (_multiThread == false)
            {
                if (_class1 == null)
                {
                    _class1 = new Class1();
                }
               
            }else{
                if (_class1 == null)
                {
                    lock (_class1)
                    {
                        _class1 = new Class1();
                    }
                }
                
            }
            return _class1;

            
        }


        // 실시간 연결 종료
        public void DisconnectAllRealData()
        {
            for (int i = _scrNum; i > 5000; i--)
            {
                axKHOpenAPI.DisconnectRealData(i.ToString());
            }

            _scrNum = 5000;
            axKHOpenAPI.CommTerminate();
        }


        // 화면번호 생산
        public string GetScrNum()
        {
            if (_scrNum < 9999)
                _scrNum++;
            else
                _scrNum = 5000;

            return _scrNum.ToString();
        }

    }
}
