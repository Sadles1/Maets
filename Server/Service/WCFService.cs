using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Service
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "WCFService" в коде и файле конфигурации.
    public class WCFService : IWCFService
    {
        public string Test()
        {
            Console.WriteLine("Hello");
            return "Hello";
        }
    }
}
