using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Service
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IWCFService" в коде и файле конфигурации.
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        Profile Connect(string Login, string Password);
        [OperationContract]
        void Disconnect();
        [OperationContract]
        void AddProduct(Product product);
        [OperationContract]
        List<Product> GetProductTable();
        [OperationContract]
        Exception Register(Profile profile, string Password);
        [OperationContract]
        void UpdateChat(Message message);
    }
}
