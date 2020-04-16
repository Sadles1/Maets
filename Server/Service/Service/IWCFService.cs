using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Service
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IWCFService
    {
        [OperationContract]
        Profile Connect(string Login, string Password);

        [OperationContract(IsOneWay = true)]
        void AddProduct(Product product);

        [OperationContract]
        List<Product> GetProductTable();

        [OperationContract]
        void Register(Profile profile, string Password);

        [OperationContract(IsOneWay = true)]
        void AddFriend(int id,int idFriend);

        [OperationContract]
        List<Message> GetChat(int idMain, int idComrade);

        [OperationContract(IsOneWay = true)]
        void BuyProduct(List<Product> Cart, int idProfile);

        [OperationContract(IsOneWay = true)]
        void BuyProductWholesale(List<Tuple<Product, int>> Cart, int idProfile);

        [OperationContract]
        Profile CheckFriend(int id);

        [OperationContract]
        void AddModerationProduct(Product product);

        [OperationContract]
        void DeleteAccount(int id);

        [OperationContract]
        void AddToBlacklist(int id, int idUserToBlacklist);

        [OperationContract]
        void RemoveFromBlacklist(int id, int idUserInBlacklist);

        [OperationContract]
        void SendMsg(Message msg);

        [OperationContract]
        void DownloadProduct(Product pr);

    }

    public interface IWCFServiceCalbback
    {

    }
}
