﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.Service {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Profile", Namespace="http://schemas.datacontract.org/2004/07/Service")]
    [System.SerializableAttribute()]
    public partial class Profile : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int AccessRightField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<double> DiscountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Client.Service.Profile[] FriendsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LoginField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] MainImageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<double> MoneyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TelephoneField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool statusField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int AccessRight {
            get {
                return this.AccessRightField;
            }
            set {
                if ((this.AccessRightField.Equals(value) != true)) {
                    this.AccessRightField = value;
                    this.RaisePropertyChanged("AccessRight");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<double> Discount {
            get {
                return this.DiscountField;
            }
            set {
                if ((this.DiscountField.Equals(value) != true)) {
                    this.DiscountField = value;
                    this.RaisePropertyChanged("Discount");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Client.Service.Profile[] Friends {
            get {
                return this.FriendsField;
            }
            set {
                if ((object.ReferenceEquals(this.FriendsField, value) != true)) {
                    this.FriendsField = value;
                    this.RaisePropertyChanged("Friends");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Login {
            get {
                return this.LoginField;
            }
            set {
                if ((object.ReferenceEquals(this.LoginField, value) != true)) {
                    this.LoginField = value;
                    this.RaisePropertyChanged("Login");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Mail {
            get {
                return this.MailField;
            }
            set {
                if ((object.ReferenceEquals(this.MailField, value) != true)) {
                    this.MailField = value;
                    this.RaisePropertyChanged("Mail");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] MainImage {
            get {
                return this.MainImageField;
            }
            set {
                if ((object.ReferenceEquals(this.MainImageField, value) != true)) {
                    this.MainImageField = value;
                    this.RaisePropertyChanged("MainImage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<double> Money {
            get {
                return this.MoneyField;
            }
            set {
                if ((this.MoneyField.Equals(value) != true)) {
                    this.MoneyField = value;
                    this.RaisePropertyChanged("Money");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Telephone {
            get {
                return this.TelephoneField;
            }
            set {
                if ((object.ReferenceEquals(this.TelephoneField, value) != true)) {
                    this.TelephoneField = value;
                    this.RaisePropertyChanged("Telephone");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool status {
            get {
                return this.statusField;
            }
            set {
                if ((this.statusField.Equals(value) != true)) {
                    this.statusField = value;
                    this.RaisePropertyChanged("status");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Product", Namespace="http://schemas.datacontract.org/2004/07/Service")]
    [System.SerializableAttribute()]
    public partial class Product : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DeveloperField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] GameGenreField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] MainImageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] MinGameSysReqField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PublisherField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] RecGameSysReqField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime ReleaseDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double RetailPriceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[][] ScreenshotsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<double> WholesalePriceField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Developer {
            get {
                return this.DeveloperField;
            }
            set {
                if ((object.ReferenceEquals(this.DeveloperField, value) != true)) {
                    this.DeveloperField = value;
                    this.RaisePropertyChanged("Developer");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] GameGenre {
            get {
                return this.GameGenreField;
            }
            set {
                if ((object.ReferenceEquals(this.GameGenreField, value) != true)) {
                    this.GameGenreField = value;
                    this.RaisePropertyChanged("GameGenre");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] MainImage {
            get {
                return this.MainImageField;
            }
            set {
                if ((object.ReferenceEquals(this.MainImageField, value) != true)) {
                    this.MainImageField = value;
                    this.RaisePropertyChanged("MainImage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] MinGameSysReq {
            get {
                return this.MinGameSysReqField;
            }
            set {
                if ((object.ReferenceEquals(this.MinGameSysReqField, value) != true)) {
                    this.MinGameSysReqField = value;
                    this.RaisePropertyChanged("MinGameSysReq");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Publisher {
            get {
                return this.PublisherField;
            }
            set {
                if ((object.ReferenceEquals(this.PublisherField, value) != true)) {
                    this.PublisherField = value;
                    this.RaisePropertyChanged("Publisher");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] RecGameSysReq {
            get {
                return this.RecGameSysReqField;
            }
            set {
                if ((object.ReferenceEquals(this.RecGameSysReqField, value) != true)) {
                    this.RecGameSysReqField = value;
                    this.RaisePropertyChanged("RecGameSysReq");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime ReleaseDate {
            get {
                return this.ReleaseDateField;
            }
            set {
                if ((this.ReleaseDateField.Equals(value) != true)) {
                    this.ReleaseDateField = value;
                    this.RaisePropertyChanged("ReleaseDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double RetailPrice {
            get {
                return this.RetailPriceField;
            }
            set {
                if ((this.RetailPriceField.Equals(value) != true)) {
                    this.RetailPriceField = value;
                    this.RaisePropertyChanged("RetailPrice");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[][] Screenshots {
            get {
                return this.ScreenshotsField;
            }
            set {
                if ((object.ReferenceEquals(this.ScreenshotsField, value) != true)) {
                    this.ScreenshotsField = value;
                    this.RaisePropertyChanged("Screenshots");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<double> WholesalePrice {
            get {
                return this.WholesalePriceField;
            }
            set {
                if ((this.WholesalePriceField.Equals(value) != true)) {
                    this.WholesalePriceField = value;
                    this.RaisePropertyChanged("WholesalePrice");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Message", Namespace="http://schemas.datacontract.org/2004/07/Service")]
    [System.SerializableAttribute()]
    public partial class Message : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IDReceiverField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IDSenderField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime dateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string messageField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int IDReceiver {
            get {
                return this.IDReceiverField;
            }
            set {
                if ((this.IDReceiverField.Equals(value) != true)) {
                    this.IDReceiverField = value;
                    this.RaisePropertyChanged("IDReceiver");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int IDSender {
            get {
                return this.IDSenderField;
            }
            set {
                if ((this.IDSenderField.Equals(value) != true)) {
                    this.IDSenderField = value;
                    this.RaisePropertyChanged("IDSender");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime date {
            get {
                return this.dateField;
            }
            set {
                if ((this.dateField.Equals(value) != true)) {
                    this.dateField = value;
                    this.RaisePropertyChanged("date");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string message {
            get {
                return this.messageField;
            }
            set {
                if ((object.ReferenceEquals(this.messageField, value) != true)) {
                    this.messageField = value;
                    this.RaisePropertyChanged("message");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.IWCFService")]
    public interface IWCFService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWCFService/Connect", ReplyAction="http://tempuri.org/IWCFService/ConnectResponse")]
        Client.Service.Profile Connect(string Login, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWCFService/Connect", ReplyAction="http://tempuri.org/IWCFService/ConnectResponse")]
        System.Threading.Tasks.Task<Client.Service.Profile> ConnectAsync(string Login, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWCFService/Disconnect", ReplyAction="http://tempuri.org/IWCFService/DisconnectResponse")]
        void Disconnect(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWCFService/Disconnect", ReplyAction="http://tempuri.org/IWCFService/DisconnectResponse")]
        System.Threading.Tasks.Task DisconnectAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWCFService/AddProduct")]
        void AddProduct(Client.Service.Product product);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWCFService/AddProduct")]
        System.Threading.Tasks.Task AddProductAsync(Client.Service.Product product);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWCFService/GetProductTable", ReplyAction="http://tempuri.org/IWCFService/GetProductTableResponse")]
        Client.Service.Product[] GetProductTable();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWCFService/GetProductTable", ReplyAction="http://tempuri.org/IWCFService/GetProductTableResponse")]
        System.Threading.Tasks.Task<Client.Service.Product[]> GetProductTableAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWCFService/Register", ReplyAction="http://tempuri.org/IWCFService/RegisterResponse")]
        void Register(Client.Service.Profile profile, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWCFService/Register", ReplyAction="http://tempuri.org/IWCFService/RegisterResponse")]
        System.Threading.Tasks.Task RegisterAsync(Client.Service.Profile profile, string Password);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWCFService/AddFriend")]
        void AddFriend(int id, int idFriend);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWCFService/AddFriend")]
        System.Threading.Tasks.Task AddFriendAsync(int id, int idFriend);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWCFService/GetChat", ReplyAction="http://tempuri.org/IWCFService/GetChatResponse")]
        Client.Service.Message[] GetChat(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWCFService/GetChat", ReplyAction="http://tempuri.org/IWCFService/GetChatResponse")]
        System.Threading.Tasks.Task<Client.Service.Message[]> GetChatAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWCFService/BuyProduct")]
        void BuyProduct(Client.Service.Product[] Cart, int idProfile);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWCFService/BuyProduct")]
        System.Threading.Tasks.Task BuyProductAsync(Client.Service.Product[] Cart, int idProfile);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWCFService/BuyProductWholesale")]
        void BuyProductWholesale(System.Tuple<Client.Service.Product, int>[] Cart, int idProfile);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IWCFService/BuyProductWholesale")]
        System.Threading.Tasks.Task BuyProductWholesaleAsync(System.Tuple<Client.Service.Product, int>[] Cart, int idProfile);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWCFServiceChannel : Client.Service.IWCFService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WCFServiceClient : System.ServiceModel.ClientBase<Client.Service.IWCFService>, Client.Service.IWCFService {
        
        public WCFServiceClient() {
        }
        
        public WCFServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WCFServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WCFServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WCFServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Client.Service.Profile Connect(string Login, string Password) {
            return base.Channel.Connect(Login, Password);
        }
        
        public System.Threading.Tasks.Task<Client.Service.Profile> ConnectAsync(string Login, string Password) {
            return base.Channel.ConnectAsync(Login, Password);
        }
        
        public void Disconnect(int id) {
            base.Channel.Disconnect(id);
        }
        
        public System.Threading.Tasks.Task DisconnectAsync(int id) {
            return base.Channel.DisconnectAsync(id);
        }
        
        public void AddProduct(Client.Service.Product product) {
            base.Channel.AddProduct(product);
        }
        
        public System.Threading.Tasks.Task AddProductAsync(Client.Service.Product product) {
            return base.Channel.AddProductAsync(product);
        }
        
        public Client.Service.Product[] GetProductTable() {
            return base.Channel.GetProductTable();
        }
        
        public System.Threading.Tasks.Task<Client.Service.Product[]> GetProductTableAsync() {
            return base.Channel.GetProductTableAsync();
        }
        
        public void Register(Client.Service.Profile profile, string Password) {
            base.Channel.Register(profile, Password);
        }
        
        public System.Threading.Tasks.Task RegisterAsync(Client.Service.Profile profile, string Password) {
            return base.Channel.RegisterAsync(profile, Password);
        }
        
        public void AddFriend(int id, int idFriend) {
            base.Channel.AddFriend(id, idFriend);
        }
        
        public System.Threading.Tasks.Task AddFriendAsync(int id, int idFriend) {
            return base.Channel.AddFriendAsync(id, idFriend);
        }
        
        public Client.Service.Message[] GetChat(int id) {
            return base.Channel.GetChat(id);
        }
        
        public System.Threading.Tasks.Task<Client.Service.Message[]> GetChatAsync(int id) {
            return base.Channel.GetChatAsync(id);
        }
        
        public void BuyProduct(Client.Service.Product[] Cart, int idProfile) {
            base.Channel.BuyProduct(Cart, idProfile);
        }
        
        public System.Threading.Tasks.Task BuyProductAsync(Client.Service.Product[] Cart, int idProfile) {
            return base.Channel.BuyProductAsync(Cart, idProfile);
        }
        
        public void BuyProductWholesale(System.Tuple<Client.Service.Product, int>[] Cart, int idProfile) {
            base.Channel.BuyProductWholesale(Cart, idProfile);
        }
        
        public System.Threading.Tasks.Task BuyProductWholesaleAsync(System.Tuple<Client.Service.Product, int>[] Cart, int idProfile) {
            return base.Channel.BuyProductWholesaleAsync(Cart, idProfile);
        }
    }
}
