using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(WCFService)))
            {
                host.Description.Behaviors.Add(new MainErrorHandlerBehaviour());
                host.Open();
                Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: Server start!");
                Console.ReadLine();
            }
        }
    }
}
