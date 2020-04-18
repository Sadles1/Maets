using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Service
{
    [ServiceContract(SessionMode = SessionMode.Required,CallbackContract = typeof(IWCFServiceCalbback))]
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
        List<UserMessage> GetChat(int idMain, int idComrade);

        [OperationContract(IsOneWay = true)]
        void BuyProduct(List<Product> Cart, int idProfile);

        [OperationContract(IsOneWay = true)]
        void BuyProductWholesale(List<Tuple<Product, int>> Cart, int idProfile);

        [OperationContract]
        Profile CheckFriend(int id);

        [OperationContract(IsOneWay = true)]
        void AddModerationProduct(Product product);

        [OperationContract(IsOneWay = true)]
        void DeleteAccount(int id);

        [OperationContract(IsOneWay = true)]
        void AddToBlacklist(int id, int idUserToBlacklist);

        [OperationContract(IsOneWay = true)]
        void RemoveFromBlacklist(int id, int idUserInBlacklist);

        [OperationContract(IsOneWay = true)]
        void SendMsg(UserMessage msg);

        [OperationContract]
        void DownloadProduct(Product pr);

        [OperationContract(IsOneWay = true)]
        void SaveCart(int idUser,List<Product> Cart);

        [OperationContract(IsOneWay = true)]
        void SendFriendRequest(int idSender, int idReceiver);

        [OperationContract]
        Profile CheckProfile(int idUser);
    }

    public interface IWCFServiceCalbback
    {
        void GetMessage(UserMessage msg);

        void ConnectionFromAnotherDevice();

        void GetFriendRequest(int idSender);

    }
}
