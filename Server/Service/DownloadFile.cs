using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    [MessageContract]
    public class DownloadFile
    {
        [MessageHeader]
        public string appRef { get; set; }

        [MessageBodyMember]
        public Stream data { get; set; }
    }
}
