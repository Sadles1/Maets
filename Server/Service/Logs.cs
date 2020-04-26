using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    //Класс для ведения логов
    class Logs
    {
        private StreamWriter streamWriter = new StreamWriter($@"{BaseSettings.Default.SourcePath}\Logs.txt",true);
        public void WriteLog(string MethodName, string SessionId, int Thread)
        {
            string log = $"{ DateTime.Now.ToShortDateString()}, { DateTime.Now.ToShortTimeString()}: {SessionId} initiate _{MethodName}_ on thread {Thread}\n";
            Console.Write(log);
            streamWriter.Write(log);
            streamWriter.Flush();         
        }

        ~Logs()
        {
            streamWriter.Close();
        }
    }
}
