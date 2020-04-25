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

        [OperationContract]
        List<Product> BuyProductWholesale(List<Tuple<int, int>> Cart, int idProfile);

        [OperationContract]
        bool CheckBlacklist(int IdMainUser, int IdSeconUser);

        [OperationContract(IsOneWay = true)]
        void AddModerationProduct(Product product, List<byte[]> Images);

        [OperationContract(IsOneWay = true)]
        void DeleteAccount(int id);

        [OperationContract(IsOneWay = true)]
        void AddToBlacklist(int id, int idUserToBlacklist);

        [OperationContract(IsOneWay = true)]

        void DeleteFriendReqest(int id, int idRequest);

        [OperationContract(IsOneWay = true)]
        void DeleteFriend(int id, int idFriend);

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

        [OperationContract]
        List<UserMessage> GetNewMessages(int id);

        [OperationContract]
        List<Profile> GetFriendRequests(int id);

        [OperationContract]
        List<Product> GetUserGames(int id);

        [OperationContract]
        Profile CheckActiveProfile(int idUser);

        [OperationContract]

        List<Profile> GetProfileByFilter(string filter);

        [OperationContract]

        List<byte[]> GetGameImages(int id);


        [OperationContract]
        void AddComment(int idUSer, int idGame, string Comment, int Score);


        [OperationContract]
        List<Comment> GetAllGameComments(int idGame);

        [OperationContract]
        Profile GetEasyProfile(int id);

        [OperationContract]
        string GetWayToGame(int idUser, int idGame);

        [OperationContract]
        string CheckMail(string Mail);
        [OperationContract]
        string ResetPassword(string Mail);

        [OperationContract]
        List<Product> GetModerationProduct(int idUser);

        [OperationContract(IsOneWay = true)]
        void ChangeModerationStatus(int idUser, int idModerationProduct, bool result);

        [OperationContract(IsOneWay = true)]
        void changePassword(int idUser, string password, string newPassword);

        [OperationContract(IsOneWay = true)]
        void ChangeProfileInformation(Profile profile);

        [OperationContract(IsOneWay = true)]
        void DeleteComment(int idComment);

        [OperationContract(IsOneWay = true)]
        void changeProfileImage(int idUser, byte[] MainImage);

        [OperationContract(IsOneWay = true)]
        void SetMessageRead(int id, int idChatedUser);

        [OperationContract(IsOneWay = true)]
        void ActivateLicenseKey(int id, string Key);

        [OperationContract(IsOneWay = true)]
        void resetPassword(int idUser, string newPassword);

        [OperationContract]
        List<Product> GetProductByFilter(string filter);
    }

    public interface IWCFServiceCalbback
    {
        [OperationContract(IsOneWay = true)]
        void FriendOffline(int idUser);

        [OperationContract(IsOneWay = true)]
        void GetMessage(UserMessage msg);

        [OperationContract(IsOneWay = true)]
        void ConnectionFromAnotherDevice();

        [OperationContract(IsOneWay = true)]
        void GetFriendRequest(Profile pr);

        [OperationContract(IsOneWay = true)]
        void FriendOnline(int idUser);

        [OperationContract(IsOneWay = true)]
        void AcceptFriendRequest(Profile pr);


    }

    [ServiceContract(SessionMode = SessionMode.Allowed)]
    public interface IDownloadService
    {
        [OperationContract]
        Stream DownloadProduct(int idProduct, int idUser, string path, long startPoin);

        [OperationContract]
        long GetFileSize(int idProduct);
    }
}