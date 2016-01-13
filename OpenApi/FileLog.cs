using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void alert(String message)
        {
            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}
