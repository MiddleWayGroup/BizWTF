﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace BizWTF.Mocking.PipelineComponents.DBEntities
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class MgmtDbEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new MgmtDbEntities object using the connection string found in the 'MgmtDbEntities' section of the application configuration file.
        /// </summary>
        public MgmtDbEntities() : base("name=MgmtDbEntities", "MgmtDbEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new MgmtDbEntities object.
        /// </summary>
        public MgmtDbEntities(string connectionString) : base(connectionString, "MgmtDbEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new MgmtDbEntities object.
        /// </summary>
        public MgmtDbEntities(EntityConnection connection) : base(connection, "MgmtDbEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<bts_receiveport> bts_receiveport
        {
            get
            {
                if ((_bts_receiveport == null))
                {
                    _bts_receiveport = base.CreateObjectSet<bts_receiveport>("bts_receiveport");
                }
                return _bts_receiveport;
            }
        }
        private ObjectSet<bts_receiveport> _bts_receiveport;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the bts_receiveport EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddTobts_receiveport(bts_receiveport bts_receiveport)
        {
            base.AddObject("bts_receiveport", bts_receiveport);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="DBEntities", Name="bts_receiveport")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class bts_receiveport : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new bts_receiveport object.
        /// </summary>
        /// <param name="nID">Initial value of the nID property.</param>
        /// <param name="nvcName">Initial value of the nvcName property.</param>
        /// <param name="bTwoWay">Initial value of the bTwoWay property.</param>
        /// <param name="nAuthentication">Initial value of the nAuthentication property.</param>
        /// <param name="uidGUID">Initial value of the uidGUID property.</param>
        /// <param name="dateModified">Initial value of the DateModified property.</param>
        /// <param name="nApplicationID">Initial value of the nApplicationID property.</param>
        public static bts_receiveport Createbts_receiveport(global::System.Int32 nID, global::System.String nvcName, global::System.Boolean bTwoWay, global::System.Int32 nAuthentication, global::System.Guid uidGUID, global::System.DateTime dateModified, global::System.Int32 nApplicationID)
        {
            bts_receiveport bts_receiveport = new bts_receiveport();
            bts_receiveport.nID = nID;
            bts_receiveport.nvcName = nvcName;
            bts_receiveport.bTwoWay = bTwoWay;
            bts_receiveport.nAuthentication = nAuthentication;
            bts_receiveport.uidGUID = uidGUID;
            bts_receiveport.DateModified = dateModified;
            bts_receiveport.nApplicationID = nApplicationID;
            return bts_receiveport;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 nID
        {
            get
            {
                return _nID;
            }
            set
            {
                if (_nID != value)
                {
                    OnnIDChanging(value);
                    ReportPropertyChanging("nID");
                    _nID = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("nID");
                    OnnIDChanged();
                }
            }
        }
        private global::System.Int32 _nID;
        partial void OnnIDChanging(global::System.Int32 value);
        partial void OnnIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String nvcName
        {
            get
            {
                return _nvcName;
            }
            set
            {
                OnnvcNameChanging(value);
                ReportPropertyChanging("nvcName");
                _nvcName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("nvcName");
                OnnvcNameChanged();
            }
        }
        private global::System.String _nvcName;
        partial void OnnvcNameChanging(global::System.String value);
        partial void OnnvcNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Boolean bTwoWay
        {
            get
            {
                return _bTwoWay;
            }
            set
            {
                OnbTwoWayChanging(value);
                ReportPropertyChanging("bTwoWay");
                _bTwoWay = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("bTwoWay");
                OnbTwoWayChanged();
            }
        }
        private global::System.Boolean _bTwoWay;
        partial void OnbTwoWayChanging(global::System.Boolean value);
        partial void OnbTwoWayChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 nAuthentication
        {
            get
            {
                return _nAuthentication;
            }
            set
            {
                OnnAuthenticationChanging(value);
                ReportPropertyChanging("nAuthentication");
                _nAuthentication = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("nAuthentication");
                OnnAuthenticationChanged();
            }
        }
        private global::System.Int32 _nAuthentication;
        partial void OnnAuthenticationChanging(global::System.Int32 value);
        partial void OnnAuthenticationChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> nSendPipelineId
        {
            get
            {
                return _nSendPipelineId;
            }
            set
            {
                OnnSendPipelineIdChanging(value);
                ReportPropertyChanging("nSendPipelineId");
                _nSendPipelineId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("nSendPipelineId");
                OnnSendPipelineIdChanged();
            }
        }
        private Nullable<global::System.Int32> _nSendPipelineId;
        partial void OnnSendPipelineIdChanging(Nullable<global::System.Int32> value);
        partial void OnnSendPipelineIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String nvcSendPipelineData
        {
            get
            {
                return _nvcSendPipelineData;
            }
            set
            {
                OnnvcSendPipelineDataChanging(value);
                ReportPropertyChanging("nvcSendPipelineData");
                _nvcSendPipelineData = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("nvcSendPipelineData");
                OnnvcSendPipelineDataChanged();
            }
        }
        private global::System.String _nvcSendPipelineData;
        partial void OnnvcSendPipelineDataChanging(global::System.String value);
        partial void OnnvcSendPipelineDataChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> nTracking
        {
            get
            {
                return _nTracking;
            }
            set
            {
                OnnTrackingChanging(value);
                ReportPropertyChanging("nTracking");
                _nTracking = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("nTracking");
                OnnTrackingChanged();
            }
        }
        private Nullable<global::System.Int32> _nTracking;
        partial void OnnTrackingChanging(Nullable<global::System.Int32> value);
        partial void OnnTrackingChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid uidGUID
        {
            get
            {
                return _uidGUID;
            }
            set
            {
                OnuidGUIDChanging(value);
                ReportPropertyChanging("uidGUID");
                _uidGUID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("uidGUID");
                OnuidGUIDChanged();
            }
        }
        private global::System.Guid _uidGUID;
        partial void OnuidGUIDChanging(global::System.Guid value);
        partial void OnuidGUIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String nvcCustomData
        {
            get
            {
                return _nvcCustomData;
            }
            set
            {
                OnnvcCustomDataChanging(value);
                ReportPropertyChanging("nvcCustomData");
                _nvcCustomData = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("nvcCustomData");
                OnnvcCustomDataChanged();
            }
        }
        private global::System.String _nvcCustomData;
        partial void OnnvcCustomDataChanging(global::System.String value);
        partial void OnnvcCustomDataChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.DateTime DateModified
        {
            get
            {
                return _DateModified;
            }
            set
            {
                OnDateModifiedChanging(value);
                ReportPropertyChanging("DateModified");
                _DateModified = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("DateModified");
                OnDateModifiedChanged();
            }
        }
        private global::System.DateTime _DateModified;
        partial void OnDateModifiedChanging(global::System.DateTime value);
        partial void OnDateModifiedChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 nApplicationID
        {
            get
            {
                return _nApplicationID;
            }
            set
            {
                OnnApplicationIDChanging(value);
                ReportPropertyChanging("nApplicationID");
                _nApplicationID = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("nApplicationID");
                OnnApplicationIDChanged();
            }
        }
        private global::System.Int32 _nApplicationID;
        partial void OnnApplicationIDChanging(global::System.Int32 value);
        partial void OnnApplicationIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String nvcDescription
        {
            get
            {
                return _nvcDescription;
            }
            set
            {
                OnnvcDescriptionChanging(value);
                ReportPropertyChanging("nvcDescription");
                _nvcDescription = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("nvcDescription");
                OnnvcDescriptionChanged();
            }
        }
        private global::System.String _nvcDescription;
        partial void OnnvcDescriptionChanging(global::System.String value);
        partial void OnnvcDescriptionChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Boolean> bRouteFailedMessage
        {
            get
            {
                return _bRouteFailedMessage;
            }
            set
            {
                OnbRouteFailedMessageChanging(value);
                ReportPropertyChanging("bRouteFailedMessage");
                _bRouteFailedMessage = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("bRouteFailedMessage");
                OnbRouteFailedMessageChanged();
            }
        }
        private Nullable<global::System.Boolean> _bRouteFailedMessage;
        partial void OnbRouteFailedMessageChanging(Nullable<global::System.Boolean> value);
        partial void OnbRouteFailedMessageChanged();

        #endregion

    
    }

    #endregion

    
}
