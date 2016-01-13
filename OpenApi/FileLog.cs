using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiwoomCode;

namespace OpenApi
{
    class FileLog
    {
        private static string path = System.Reflection.Assembly.GetExecutingAssembly().Location;

        public static void PrintF(String line)
        {
            path = System.IO.Path.GetDirectoryName(path);
            path = path + "\\log.log";
            System.IO.StreamWriter file = new System.IO.StreamWriter(path, true);
            file.WriteLine(line);
            file.Close();
        }



        // 로그를 출력합니다.
        public static void PrintF(Log type, string format, params Object[] args)
        {
            string message = String.Format(format, args);
            
            switch (type)
            {
                case Log.조회:
                    message = "[조회]" + message;
                    break;
                case Log.에러:
                    message = "[에러]" + message;
                    break;
                case Log.일반:
                    message = "[일반]" + message;
                    break;
                case Log.실시간:
                    message = "[실시간]" + message;
                    break;
                case Log.디버깅:
                    message = "[디버깅]" + message;
                    break;
                default:
                    break;
            }
            PrintF(message);
        }

        

        

        public static void alert(String message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}
