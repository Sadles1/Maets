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
            //string baseAddress = "net.tcp://" + Environment.MachineName + ":904";
            using (var host = new ServiceHost(typeof(WCFService)/*, new Uri(baseAddress)*/))
            {
                //NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                //binding.ReliableSession.Enabled = true;
                //ServiceEndpoint endpoint = host.AddServiceEndpoint(typeof(IWCFService), binding, "");
                //endpoint.Behaviors.Add(new ClientTrackerEndpointBehavior());

                host.Open();
                Console.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}: Server start!");
                Console.ReadLine();
            }
        }
        
    }
}
