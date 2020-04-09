using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Service
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        Profile Connect(string Login, string Password);

        [OperationContract]
        void Disconnect(int id);

        [OperationContract(IsOneWay = true)]
        void AddProduct(Product product);

        [OperationContract]
        List<Product> GetProductTable();

        [OperationContract]
        void Register(Profile profile, string Password);

        [OperationContract(IsOneWay = true)]
        void AddFriend(int id,int idFriend);

        [OperationContract]
        List<Message> GetChat(int id);

        [OperationContract(IsOneWay = true)]
        void BuyProduct(List<Product> Cart, int idProfile);

        [OperationContract(IsOneWay = true)]
        void BuyProductWholesale(List<Tuple<Product, int>> Cart, int idProfile);
    }
}
