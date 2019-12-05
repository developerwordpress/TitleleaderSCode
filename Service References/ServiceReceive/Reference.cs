﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TitleLeader.ServiceReceive {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ReceiveNoteData", Namespace="http://schemas.datacontract.org/2004/07/Adeptive.ResWare.Services")]
    [System.SerializableAttribute()]
    public partial class ReceiveNoteData : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> CoordinatorTypeIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int CurativeIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TitleLeader.ServiceReceive.ReceiveCurativeTypeEnum CurativeTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private TitleLeader.ServiceReceive.ReceiveNoteDocument[] DocumentsField;
        
        private string FileNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NoteBodyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NoteSubjectField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ToCoordinatorIDField;
        
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
        public System.Nullable<int> CoordinatorTypeID {
            get {
                return this.CoordinatorTypeIDField;
            }
            set {
                if ((this.CoordinatorTypeIDField.Equals(value) != true)) {
                    this.CoordinatorTypeIDField = value;
                    this.RaisePropertyChanged("CoordinatorTypeID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int CurativeID {
            get {
                return this.CurativeIDField;
            }
            set {
                if ((this.CurativeIDField.Equals(value) != true)) {
                    this.CurativeIDField = value;
                    this.RaisePropertyChanged("CurativeID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TitleLeader.ServiceReceive.ReceiveCurativeTypeEnum CurativeType {
            get {
                return this.CurativeTypeField;
            }
            set {
                if ((this.CurativeTypeField.Equals(value) != true)) {
                    this.CurativeTypeField = value;
                    this.RaisePropertyChanged("CurativeType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public TitleLeader.ServiceReceive.ReceiveNoteDocument[] Documents {
            get {
                return this.DocumentsField;
            }
            set {
                if ((object.ReferenceEquals(this.DocumentsField, value) != true)) {
                    this.DocumentsField = value;
                    this.RaisePropertyChanged("Documents");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string FileNumber {
            get {
                return this.FileNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.FileNumberField, value) != true)) {
                    this.FileNumberField = value;
                    this.RaisePropertyChanged("FileNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NoteBody {
            get {
                return this.NoteBodyField;
            }
            set {
                if ((object.ReferenceEquals(this.NoteBodyField, value) != true)) {
                    this.NoteBodyField = value;
                    this.RaisePropertyChanged("NoteBody");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NoteSubject {
            get {
                return this.NoteSubjectField;
            }
            set {
                if ((object.ReferenceEquals(this.NoteSubjectField, value) != true)) {
                    this.NoteSubjectField = value;
                    this.RaisePropertyChanged("NoteSubject");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ToCoordinatorID {
            get {
                return this.ToCoordinatorIDField;
            }
            set {
                if ((this.ToCoordinatorIDField.Equals(value) != true)) {
                    this.ToCoordinatorIDField = value;
                    this.RaisePropertyChanged("ToCoordinatorID");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ReceiveCurativeTypeEnum", Namespace="http://schemas.datacontract.org/2004/07/Adeptive.ResWare.Services")]
    public enum ReceiveCurativeTypeEnum : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PRE_CLOSING = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        POLICY = 1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ReceiveNoteDocument", Namespace="http://schemas.datacontract.org/2004/07/Adeptive.ResWare.Services")]
    [System.SerializableAttribute()]
    public partial class ReceiveNoteDocument : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescriptionField;
        
        private byte[] DocumentBodyField;
        
        private int DocumentTypeIDField;
        
        private string FileNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool InternalOnlyField;
        
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
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public byte[] DocumentBody {
            get {
                return this.DocumentBodyField;
            }
            set {
                if ((object.ReferenceEquals(this.DocumentBodyField, value) != true)) {
                    this.DocumentBodyField = value;
                    this.RaisePropertyChanged("DocumentBody");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int DocumentTypeID {
            get {
                return this.DocumentTypeIDField;
            }
            set {
                if ((this.DocumentTypeIDField.Equals(value) != true)) {
                    this.DocumentTypeIDField = value;
                    this.RaisePropertyChanged("DocumentTypeID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string FileName {
            get {
                return this.FileNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FileNameField, value) != true)) {
                    this.FileNameField = value;
                    this.RaisePropertyChanged("FileName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool InternalOnly {
            get {
                return this.InternalOnlyField;
            }
            set {
                if ((this.InternalOnlyField.Equals(value) != true)) {
                    this.InternalOnlyField = value;
                    this.RaisePropertyChanged("InternalOnly");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ReceiveNoteResponse", Namespace="http://schemas.datacontract.org/2004/07/Adeptive.ResWare.Services")]
    [System.SerializableAttribute()]
    public partial class ReceiveNoteResponse : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string MessageField;
        
        private TitleLeader.ServiceReceive.ReceiveNoteResponseCode ResponseCodeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public TitleLeader.ServiceReceive.ReceiveNoteResponseCode ResponseCode {
            get {
                return this.ResponseCodeField;
            }
            set {
                if ((this.ResponseCodeField.Equals(value) != true)) {
                    this.ResponseCodeField = value;
                    this.RaisePropertyChanged("ResponseCode");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ReceiveNoteResponseCode", Namespace="http://schemas.datacontract.org/2004/07/Adeptive.ResWare.Services")]
    public enum ReceiveNoteResponseCode : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SUCCESS = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        INVALID_LOGIN = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        INVALID_FILE_NUMBER = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DOWN_FOR_MAINTENANCE = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NOTE_ERROR = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DOCUMENT_ERROR = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SECURITY_ERROR = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UNEXPECTED_ERROR = 10,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReceive.IReceiveNoteService")]
    public interface IReceiveNoteService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReceiveNoteService/ReceiveNote", ReplyAction="http://tempuri.org/IReceiveNoteService/ReceiveNoteResponse")]
        TitleLeader.ServiceReceive.ReceiveNoteResponse ReceiveNote(TitleLeader.ServiceReceive.ReceiveNoteData NoteData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IReceiveNoteService/ReceiveNote", ReplyAction="http://tempuri.org/IReceiveNoteService/ReceiveNoteResponse")]
        System.Threading.Tasks.Task<TitleLeader.ServiceReceive.ReceiveNoteResponse> ReceiveNoteAsync(TitleLeader.ServiceReceive.ReceiveNoteData NoteData);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IReceiveNoteServiceChannel : TitleLeader.ServiceReceive.IReceiveNoteService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ReceiveNoteServiceClient : System.ServiceModel.ClientBase<TitleLeader.ServiceReceive.IReceiveNoteService>, TitleLeader.ServiceReceive.IReceiveNoteService {
        
        public ReceiveNoteServiceClient() {
        }
        
        public ReceiveNoteServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ReceiveNoteServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReceiveNoteServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ReceiveNoteServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public TitleLeader.ServiceReceive.ReceiveNoteResponse ReceiveNote(TitleLeader.ServiceReceive.ReceiveNoteData NoteData) {
            return base.Channel.ReceiveNote(NoteData);
        }
        
        public System.Threading.Tasks.Task<TitleLeader.ServiceReceive.ReceiveNoteResponse> ReceiveNoteAsync(TitleLeader.ServiceReceive.ReceiveNoteData NoteData) {
            return base.Channel.ReceiveNoteAsync(NoteData);
        }
    }
}