using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MainErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            //OnlineUser ActiveUser = WCFService.onlineUsers.FirstOrDefault(u => u.operationContext == OperationContext.Current);
            Console.WriteLine($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToShortTimeString()}: {error.Message}");
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            if (error is FaultException)
                return;
            var flt = new FaultException(error.Message, new FaultCode("InnerServiceException"));
            var msg = flt.CreateMessageFault();
            fault = Message.CreateMessage(version, msg, "null");
        }
    }
}
