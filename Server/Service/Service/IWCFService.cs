using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace Service
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IWCFServiceCalbback))]
    public interface IWCFService
    {
        [OperationContract]
        Profile Connect(string Login, string Password);

        [OperationContract(IsOneWay = true)]
        void AddProduct(Product product);

        [OperationContract(IsOneWay = true)]
        void Disconnect(int Id);

        [OperationContract]
        List<Product> GetProductTable();

        [OperationContract]
        void Register(Profile profile, string Password);

        [OperationContract(IsOneWay = true)]
        void AddFriend(int id, int idFriend);

        [OperationContract]
        List<UserMessage> GetChat(int idMain, int idComrade);

        [OperationContract(IsOneWay = true)]
        void BuyProduct(List<int> Cart, int idProfile);

        [OperationContract(IsOneWay = true)]
        void BuyProductWholesale(List<Tuple<int, int>> Cart, int idProfile);

        [OperationContract]
        bool CheckBlacklist(int IdMainUser, int IdSeconUser);

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

        [OperationContract(IsOneWay = true)]
        void SaveCart(int idUser, List<Product> Cart);

        [OperationContract(IsOneWay = true)]
        void SendFriendRequest(int idSender, int idReceiver);

        [OperationContract]
        Profile CheckProfile(int idUser);

        [OperationContract]
        List<Profile> GetAllUsers();
    }

    public interface IWCFServiceCalbback
    {
        [OperationContract(IsOneWay = true)]
        void GetMessage(UserMessage msg);

        [OperationContract(IsOneWay = true)]
        void ConnectionFromAnotherDevice();

        [OperationContract(IsOneWay = true)]
        void GetFriendRequest(int idSender);

        [OperationContract(IsOneWay = true)]
        void FriendOnline(int idUser);

    }

    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IDownloadService
    {
        [OperationContract]
        Stream DownloadProduct(int idProduct);
    }

    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IUploadService
    {
        [OperationContract]
        void UploadProduct(Stream product);
    }
}