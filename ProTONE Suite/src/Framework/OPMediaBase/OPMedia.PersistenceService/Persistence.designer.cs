﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OPMedia.PersistenceService
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	public partial class Persistence : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertPersistedObjects(PersistedObjects instance);
    partial void UpdatePersistedObjects(PersistedObjects instance);
    partial void DeletePersistedObjects(PersistedObjects instance);
    #endregion
		
		public Persistence(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public Persistence(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public Persistence(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public Persistence(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<PersistedObjects> PersistedObjects
		{
			get
			{
				return this.GetTable<PersistedObjects>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute()]
	public partial class PersistedObjects : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _PersistenceID;
		
		private string _Content;
		
		private System.Nullable<bool> _Reserved;
		
		private string _PersistenceContext;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnPersistenceIDChanging(string value);
    partial void OnPersistenceIDChanged();
    partial void OnContentChanging(string value);
    partial void OnContentChanged();
    partial void OnReservedChanging(System.Nullable<bool> value);
    partial void OnReservedChanged();
    partial void OnPersistenceContextChanging(string value);
    partial void OnPersistenceContextChanged();
    #endregion
		
		public PersistedObjects()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PersistenceID", DbType="NVarChar(4000) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string PersistenceID
		{
			get
			{
				return this._PersistenceID;
			}
			set
			{
				if ((this._PersistenceID != value))
				{
					this.OnPersistenceIDChanging(value);
					this.SendPropertyChanging();
					this._PersistenceID = value;
					this.SendPropertyChanged("PersistenceID");
					this.OnPersistenceIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Content", DbType="NText", UpdateCheck=UpdateCheck.Never)]
		public string Content
		{
			get
			{
				return this._Content;
			}
			set
			{
				if ((this._Content != value))
				{
					this.OnContentChanging(value);
					this.SendPropertyChanging();
					this._Content = value;
					this.SendPropertyChanged("Content");
					this.OnContentChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Reserved", DbType="Bit")]
		public System.Nullable<bool> Reserved
		{
			get
			{
				return this._Reserved;
			}
			set
			{
				if ((this._Reserved != value))
				{
					this.OnReservedChanging(value);
					this.SendPropertyChanging();
					this._Reserved = value;
					this.SendPropertyChanged("Reserved");
					this.OnReservedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PersistenceContext", DbType="NVarChar(4000) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string PersistenceContext
		{
			get
			{
				return this._PersistenceContext;
			}
			set
			{
				if ((this._PersistenceContext != value))
				{
					this.OnPersistenceContextChanging(value);
					this.SendPropertyChanging();
					this._PersistenceContext = value;
					this.SendPropertyChanged("PersistenceContext");
					this.OnPersistenceContextChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
