using Client.Data.Views;
using Client.Service;
using System;
using System.Windows;

namespace Client
{
    public class CallbackClass : IWCFServiceCallback
    {
        private ShopWindows shopWindows;

        public CallbackClass()
        {
        }

        public CallbackClass(ShopWindows shopWindows)
        {
            this.shopWindows = shopWindows;
        }

        /// <summary>
        /// Вызывается когда кто-то входит под такими же данными
        /// </summary>
        public void ConnectionFromAnotherDevice()
        {
            ShopWindows.client.Disconnect(shopWindows.profile.ID);
            shopWindows.Close();
            MessageBox.Show(string.Format("Connection from another device\nYou have been kicked"), "ERROR");
        }

        public void FriendOnline(int idUser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Вызывается при получении заявки в друзья
        /// </summary>
        /// <param name="idSender"></param>
        public void GetFriendRequest(int idSender)
        {

        }

        /// <summary>
        /// Вызывается при получении сообщения
        /// </summary>
        /// <param name="msg"></param>
        public void GetMessage(UserMessage msg)
        {

            Chat.chatnow.tbChat.Text+=msg.message;
        }
    }
}
