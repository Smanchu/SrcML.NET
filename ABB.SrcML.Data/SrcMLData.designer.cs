﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ABB.SrcML.Data
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
	
	
	public partial class SrcMLDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertDefinition(Definition instance);
    partial void UpdateDefinition(Definition instance);
    partial void DeleteDefinition(Definition instance);
    partial void InsertArchive(Archive instance);
    partial void UpdateArchive(Archive instance);
    partial void DeleteArchive(Archive instance);
    partial void InsertValidScope(ValidScope instance);
    partial void UpdateValidScope(ValidScope instance);
    partial void DeleteValidScope(ValidScope instance);
    partial void InsertMethodCall(MethodCall instance);
    partial void UpdateMethodCall(MethodCall instance);
    partial void DeleteMethodCall(MethodCall instance);
    #endregion
		
		public SrcMLDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SrcMLDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SrcMLDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public SrcMLDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Definition> Definitions
		{
			get
			{
				return this.GetTable<Definition>();
			}
		}
		
		public System.Data.Linq.Table<Archive> Archives
		{
			get
			{
				return this.GetTable<Archive>();
			}
		}
		
		public System.Data.Linq.Table<ValidScope> ValidScopes
		{
			get
			{
				return this.GetTable<ValidScope>();
			}
		}
		
		public System.Data.Linq.Table<MethodCall> MethodCalls
		{
			get
			{
				return this.GetTable<MethodCall>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	[global::System.Data.Linq.Mapping.InheritanceMappingAttribute(Code="0", Type=typeof(UnknownDefinition), IsDefault=true)]
	[global::System.Data.Linq.Mapping.InheritanceMappingAttribute(Code="5", Type=typeof(VariableDeclaration))]
	[global::System.Data.Linq.Mapping.InheritanceMappingAttribute(Code="6", Type=typeof(MethodDeclaration))]
	[global::System.Data.Linq.Mapping.InheritanceMappingAttribute(Code="7", Type=typeof(TypeDeclaration))]
	[global::System.Data.Linq.Mapping.InheritanceMappingAttribute(Code="1", Type=typeof(ScopeDefinition))]
	[global::System.Data.Linq.Mapping.InheritanceMappingAttribute(Code="3", Type=typeof(TypeDefinition))]
	[global::System.Data.Linq.Mapping.InheritanceMappingAttribute(Code="4", Type=typeof(MethodDefinition))]
	public partial class Definition : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _FileName;
		
		private int _LineNumber;
		
		private DefinitionType _ElementName;
		
		private string _XPath;
		
		private System.Xml.Linq.XElement _Xml;
		
		private int _ArchiveId;
		
		private string _ElementXName;
		
		private EntitySet<ValidScope> _ValidScopes;
		
		private EntitySet<MethodCall> _CallsFromMethod;
		
		private EntitySet<MethodCall> _CallsToMethod;
		
		private EntityRef<Archive> _Archive;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnFileNameChanging(string value);
    partial void OnFileNameChanged();
    partial void OnLineNumberChanging(int value);
    partial void OnLineNumberChanged();
    partial void OnDefinitionTypeIdChanging(DefinitionType value);
    partial void OnDefinitionTypeIdChanged();
    partial void OnXPathChanging(string value);
    partial void OnXPathChanged();
    partial void OnXmlChanging(System.Xml.Linq.XElement value);
    partial void OnXmlChanged();
    partial void OnArchiveIdChanging(int value);
    partial void OnArchiveIdChanged();
    partial void OnElementXNameChanging(string value);
    partial void OnElementXNameChanged();
    #endregion
		
		public Definition()
		{
			this._ValidScopes = new EntitySet<ValidScope>(new Action<ValidScope>(this.attach_ValidScopes), new Action<ValidScope>(this.detach_ValidScopes));
			this._CallsFromMethod = new EntitySet<MethodCall>(new Action<MethodCall>(this.attach_CallsFromMethod), new Action<MethodCall>(this.detach_CallsFromMethod));
			this._CallsToMethod = new EntitySet<MethodCall>(new Action<MethodCall>(this.attach_CallsToMethod), new Action<MethodCall>(this.detach_CallsToMethod));
			this._Archive = default(EntityRef<Archive>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public virtual int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FileName", DbType="NVARCHAR(MAX)", CanBeNull=false)]
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if ((this._FileName != value))
				{
					this.OnFileNameChanging(value);
					this.SendPropertyChanging();
					this._FileName = value;
					this.SendPropertyChanged("FileName");
					this.OnFileNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LineNumber")]
		public int LineNumber
		{
			get
			{
				return this._LineNumber;
			}
			set
			{
				if ((this._LineNumber != value))
				{
					this.OnLineNumberChanging(value);
					this.SendPropertyChanging();
					this._LineNumber = value;
					this.SendPropertyChanged("LineNumber");
					this.OnLineNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ElementName", DbType="int NOT NULL", CanBeNull=false, IsDiscriminator=true)]
		public DefinitionType DefinitionTypeId
		{
			get
			{
				return this._ElementName;
			}
			set
			{
				if ((this._ElementName != value))
				{
					this.OnDefinitionTypeIdChanging(value);
					this.SendPropertyChanging();
					this._ElementName = value;
					this.SendPropertyChanged("DefinitionTypeId");
					this.OnDefinitionTypeIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_XPath", DbType="NVARCHAR(MAX)", CanBeNull=false)]
		public string XPath
		{
			get
			{
				return this._XPath;
			}
			set
			{
				if ((this._XPath != value))
				{
					this.OnXPathChanging(value);
					this.SendPropertyChanging();
					this._XPath = value;
					this.SendPropertyChanged("XPath");
					this.OnXPathChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Xml", DbType="XML", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public System.Xml.Linq.XElement Xml
		{
			get
			{
				return this._Xml;
			}
			set
			{
				if ((this._Xml != value))
				{
					this.OnXmlChanging(value);
					this.SendPropertyChanging();
					this._Xml = value;
					this.SendPropertyChanged("Xml");
					this.OnXmlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ArchiveId")]
		public int ArchiveId
		{
			get
			{
				return this._ArchiveId;
			}
			set
			{
				if ((this._ArchiveId != value))
				{
					if (this._Archive.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnArchiveIdChanging(value);
					this.SendPropertyChanging();
					this._ArchiveId = value;
					this.SendPropertyChanged("ArchiveId");
					this.OnArchiveIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ElementXName", DbType="NVARCHAR(MAX)", CanBeNull=false)]
		public string ElementXName
		{
			get
			{
				return this._ElementXName;
			}
			set
			{
				if ((this._ElementXName != value))
				{
					this.OnElementXNameChanging(value);
					this.SendPropertyChanging();
					this._ElementXName = value;
					this.SendPropertyChanged("ElementXName");
					this.OnElementXNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Definition_ValidScope", Storage="_ValidScopes", ThisKey="Id", OtherKey="DefinitionId")]
		public EntitySet<ValidScope> ValidScopes
		{
			get
			{
				return this._ValidScopes;
			}
			set
			{
				this._ValidScopes.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Definition_MethodCall", Storage="_CallsFromMethod", ThisKey="Id", OtherKey="CallerId")]
		public EntitySet<MethodCall> CallsFromMethod
		{
			get
			{
				return this._CallsFromMethod;
			}
			set
			{
				this._CallsFromMethod.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Definition_MethodCall1", Storage="_CallsToMethod", ThisKey="Id", OtherKey="CalleeId")]
		public EntitySet<MethodCall> CallsToMethod
		{
			get
			{
				return this._CallsToMethod;
			}
			set
			{
				this._CallsToMethod.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Archive_Definition", Storage="_Archive", ThisKey="ArchiveId", OtherKey="Id", IsForeignKey=true)]
		public Archive Archive
		{
			get
			{
				return this._Archive.Entity;
			}
			set
			{
				Archive previousValue = this._Archive.Entity;
				if (((previousValue != value) 
							|| (this._Archive.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Archive.Entity = null;
						previousValue.Definitions.Remove(this);
					}
					this._Archive.Entity = value;
					if ((value != null))
					{
						value.Definitions.Add(this);
						this._ArchiveId = value.Id;
					}
					else
					{
						this._ArchiveId = default(int);
					}
					this.SendPropertyChanged("Archive");
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
		
		private void attach_ValidScopes(ValidScope entity)
		{
			this.SendPropertyChanging();
			entity.Definition = this;
		}
		
		private void detach_ValidScopes(ValidScope entity)
		{
			this.SendPropertyChanging();
			entity.Definition = null;
		}
		
		private void attach_CallsFromMethod(MethodCall entity)
		{
			this.SendPropertyChanging();
			entity.CallerDefinition = this;
		}
		
		private void detach_CallsFromMethod(MethodCall entity)
		{
			this.SendPropertyChanging();
			entity.CallerDefinition = null;
		}
		
		private void attach_CallsToMethod(MethodCall entity)
		{
			this.SendPropertyChanging();
			entity.CalleeDefinition = this;
		}
		
		private void detach_CallsToMethod(MethodCall entity)
		{
			this.SendPropertyChanging();
			entity.CalleeDefinition = null;
		}
	}
	
	public partial class UnknownDefinition : Definition
	{
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    #endregion
		
		public UnknownDefinition()
		{
			OnCreated();
		}
	}
	
	public abstract partial class Declaration : Definition
	{
		
		private string _VariableNameElement;
		
		private System.Nullable<bool> _IsGlobal;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDeclarationNameChanging(string value);
    partial void OnDeclarationNameChanged();
    partial void OnIsGlobalChanging(System.Nullable<bool> value);
    partial void OnIsGlobalChanged();
    #endregion
		
		public Declaration()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="VariableName", Storage="_VariableNameElement", DbType="NVARCHAR(1024)")]
		public string DeclarationName
		{
			get
			{
				return this._VariableNameElement;
			}
			set
			{
				if ((this._VariableNameElement != value))
				{
					this.OnDeclarationNameChanging(value);
					this.SendPropertyChanging();
					this._VariableNameElement = value;
					this.SendPropertyChanged("DeclarationName");
					this.OnDeclarationNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsGlobal")]
		public System.Nullable<bool> IsGlobal
		{
			get
			{
				return this._IsGlobal;
			}
			set
			{
				if ((this._IsGlobal != value))
				{
					this.OnIsGlobalChanging(value);
					this.SendPropertyChanging();
					this._IsGlobal = value;
					this.SendPropertyChanged("IsGlobal");
					this.OnIsGlobalChanged();
				}
			}
		}
	}
	
	public partial class VariableDeclaration : Declaration
	{
		
		private string _VariableTypeName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnVariableTypeNameChanging(string value);
    partial void OnVariableTypeNameChanged();
    #endregion
		
		public VariableDeclaration()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VariableTypeName", DbType="NVARCHAR(1024)")]
		public string VariableTypeName
		{
			get
			{
				return this._VariableTypeName;
			}
			set
			{
				if ((this._VariableTypeName != value))
				{
					this.OnVariableTypeNameChanging(value);
					this.SendPropertyChanging();
					this._VariableTypeName = value;
					this.SendPropertyChanged("VariableTypeName");
					this.OnVariableTypeNameChanged();
				}
			}
		}
	}
	
	public partial class MethodDeclaration : Declaration
	{
		
		private string _DeclarationClassName;
		
		private string _DeclarationReturnTypeName;
		
		private System.Nullable<int> _DeclarationNumberOfParameters;
		
		private System.Nullable<int> _DeclarationNumberOfParametersWithDefaults;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDeclarationClassNameChanging(string value);
    partial void OnDeclarationClassNameChanged();
    partial void OnDeclarationReturnTypeNameChanging(string value);
    partial void OnDeclarationReturnTypeNameChanged();
    partial void OnDeclarationNumberOfParametersChanging(System.Nullable<int> value);
    partial void OnDeclarationNumberOfParametersChanged();
    partial void OnDeclarationNumberOfParametersWithDefaultsChanging(System.Nullable<int> value);
    partial void OnDeclarationNumberOfParametersWithDefaultsChanged();
    #endregion
		
		public MethodDeclaration()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeclarationClassName", DbType="NVARCHAR(1024)")]
		public string DeclarationClassName
		{
			get
			{
				return this._DeclarationClassName;
			}
			set
			{
				if ((this._DeclarationClassName != value))
				{
					this.OnDeclarationClassNameChanging(value);
					this.SendPropertyChanging();
					this._DeclarationClassName = value;
					this.SendPropertyChanged("DeclarationClassName");
					this.OnDeclarationClassNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeclarationReturnTypeName", DbType="NVARCHAR(1024)")]
		public string DeclarationReturnTypeName
		{
			get
			{
				return this._DeclarationReturnTypeName;
			}
			set
			{
				if ((this._DeclarationReturnTypeName != value))
				{
					this.OnDeclarationReturnTypeNameChanging(value);
					this.SendPropertyChanging();
					this._DeclarationReturnTypeName = value;
					this.SendPropertyChanged("DeclarationReturnTypeName");
					this.OnDeclarationReturnTypeNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeclarationNumberOfParameters")]
		public System.Nullable<int> DeclarationNumberOfParameters
		{
			get
			{
				return this._DeclarationNumberOfParameters;
			}
			set
			{
				if ((this._DeclarationNumberOfParameters != value))
				{
					this.OnDeclarationNumberOfParametersChanging(value);
					this.SendPropertyChanging();
					this._DeclarationNumberOfParameters = value;
					this.SendPropertyChanged("DeclarationNumberOfParameters");
					this.OnDeclarationNumberOfParametersChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeclarationNumberOfParametersWithDefaults")]
		public System.Nullable<int> DeclarationNumberOfParametersWithDefaults
		{
			get
			{
				return this._DeclarationNumberOfParametersWithDefaults;
			}
			set
			{
				if ((this._DeclarationNumberOfParametersWithDefaults != value))
				{
					this.OnDeclarationNumberOfParametersWithDefaultsChanging(value);
					this.SendPropertyChanging();
					this._DeclarationNumberOfParametersWithDefaults = value;
					this.SendPropertyChanged("DeclarationNumberOfParametersWithDefaults");
					this.OnDeclarationNumberOfParametersWithDefaultsChanged();
				}
			}
		}
	}
	
	public partial class TypeDeclaration : Declaration
	{
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    #endregion
		
		public TypeDeclaration()
		{
			OnCreated();
		}
	}
	
	public partial class ScopeDefinition : Definition
	{
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    #endregion
		
		public ScopeDefinition()
		{
			OnCreated();
		}
	}
	
	public partial class TypeDefinition : ScopeDefinition
	{
		
		private string _TypeNameElement;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnTypeNameChanging(string value);
    partial void OnTypeNameChanged();
    #endregion
		
		public TypeDefinition()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TypeNameElement", DbType="NVARCHAR(1024)")]
		public string TypeName
		{
			get
			{
				return this._TypeNameElement;
			}
			set
			{
				if ((this._TypeNameElement != value))
				{
					this.OnTypeNameChanging(value);
					this.SendPropertyChanging();
					this._TypeNameElement = value;
					this.SendPropertyChanged("TypeName");
					this.OnTypeNameChanged();
				}
			}
		}
	}
	
	public partial class MethodDefinition : ScopeDefinition
	{
		
		private string _MethodNameElement;
		
		private string _MethodClassNameElement;
		
		private string _MethodReturnTypeName;
		
		private System.Nullable<int> _NumberOfMethodParameters;
		
		private System.Nullable<int> _NumberOfMethodParametersWithDefaults;
		
		private string _MethodSignature;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnMethodNameChanging(string value);
    partial void OnMethodNameChanged();
    partial void OnMethodClassNameChanging(string value);
    partial void OnMethodClassNameChanged();
    partial void OnMethodReturnTypeNameChanging(string value);
    partial void OnMethodReturnTypeNameChanged();
    partial void OnNumberOfMethodParametersChanging(System.Nullable<int> value);
    partial void OnNumberOfMethodParametersChanged();
    partial void OnNumberOfMethodParametersWithDefaultsChanging(System.Nullable<int> value);
    partial void OnNumberOfMethodParametersWithDefaultsChanged();
    partial void OnMethodSignatureChanging(string value);
    partial void OnMethodSignatureChanged();
    #endregion
		
		public MethodDefinition()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MethodNameElement", DbType="NVARCHAR(1024)")]
		public string MethodName
		{
			get
			{
				return this._MethodNameElement;
			}
			set
			{
				if ((this._MethodNameElement != value))
				{
					this.OnMethodNameChanging(value);
					this.SendPropertyChanging();
					this._MethodNameElement = value;
					this.SendPropertyChanged("MethodName");
					this.OnMethodNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MethodClassNameElement", DbType="NVARCHAR(1024)")]
		public string MethodClassName
		{
			get
			{
				return this._MethodClassNameElement;
			}
			set
			{
				if ((this._MethodClassNameElement != value))
				{
					this.OnMethodClassNameChanging(value);
					this.SendPropertyChanging();
					this._MethodClassNameElement = value;
					this.SendPropertyChanged("MethodClassName");
					this.OnMethodClassNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MethodReturnTypeName", DbType="NVARCHAR(1024)")]
		public string MethodReturnTypeName
		{
			get
			{
				return this._MethodReturnTypeName;
			}
			set
			{
				if ((this._MethodReturnTypeName != value))
				{
					this.OnMethodReturnTypeNameChanging(value);
					this.SendPropertyChanging();
					this._MethodReturnTypeName = value;
					this.SendPropertyChanged("MethodReturnTypeName");
					this.OnMethodReturnTypeNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NumberOfMethodParameters")]
		public System.Nullable<int> NumberOfMethodParameters
		{
			get
			{
				return this._NumberOfMethodParameters;
			}
			set
			{
				if ((this._NumberOfMethodParameters != value))
				{
					this.OnNumberOfMethodParametersChanging(value);
					this.SendPropertyChanging();
					this._NumberOfMethodParameters = value;
					this.SendPropertyChanged("NumberOfMethodParameters");
					this.OnNumberOfMethodParametersChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NumberOfMethodParametersWithDefaults")]
		public System.Nullable<int> NumberOfMethodParametersWithDefaults
		{
			get
			{
				return this._NumberOfMethodParametersWithDefaults;
			}
			set
			{
				if ((this._NumberOfMethodParametersWithDefaults != value))
				{
					this.OnNumberOfMethodParametersWithDefaultsChanging(value);
					this.SendPropertyChanging();
					this._NumberOfMethodParametersWithDefaults = value;
					this.SendPropertyChanged("NumberOfMethodParametersWithDefaults");
					this.OnNumberOfMethodParametersWithDefaultsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MethodSignature", DbType="NVARCHAR(MAX)")]
		public string MethodSignature
		{
			get
			{
				return this._MethodSignature;
			}
			set
			{
				if ((this._MethodSignature != value))
				{
					this.OnMethodSignatureChanging(value);
					this.SendPropertyChanging();
					this._MethodSignature = value;
					this.SendPropertyChanged("MethodSignature");
					this.OnMethodSignatureChanged();
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class Archive : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private string _Path;
		
		private System.DateTime _LastUpdated;
		
		private EntitySet<Definition> _Definitions;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnPathChanging(string value);
    partial void OnPathChanged();
    partial void OnLastUpdatedChanging(System.DateTime value);
    partial void OnLastUpdatedChanged();
    #endregion
		
		public Archive()
		{
			this._Definitions = new EntitySet<Definition>(new Action<Definition>(this.attach_Definitions), new Action<Definition>(this.detach_Definitions));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Path", DbType="NVARCHAR(MAX)", CanBeNull=false)]
		public string Path
		{
			get
			{
				return this._Path;
			}
			set
			{
				if ((this._Path != value))
				{
					this.OnPathChanging(value);
					this.SendPropertyChanging();
					this._Path = value;
					this.SendPropertyChanged("Path");
					this.OnPathChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastUpdated")]
		public System.DateTime LastUpdated
		{
			get
			{
				return this._LastUpdated;
			}
			set
			{
				if ((this._LastUpdated != value))
				{
					this.OnLastUpdatedChanging(value);
					this.SendPropertyChanging();
					this._LastUpdated = value;
					this.SendPropertyChanged("LastUpdated");
					this.OnLastUpdatedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Archive_Definition", Storage="_Definitions", ThisKey="Id", OtherKey="ArchiveId")]
		public EntitySet<Definition> Definitions
		{
			get
			{
				return this._Definitions;
			}
			set
			{
				this._Definitions.Assign(value);
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
		
		private void attach_Definitions(Definition entity)
		{
			this.SendPropertyChanging();
			entity.Archive = this;
		}
		
		private void detach_Definitions(Definition entity)
		{
			this.SendPropertyChanging();
			entity.Archive = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class ValidScope : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _DefinitionId;
		
		private string _XPath;
		
		private EntityRef<Definition> _Definition;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnDefinitionIdChanging(int value);
    partial void OnDefinitionIdChanged();
    partial void OnXPathChanging(string value);
    partial void OnXPathChanged();
    #endregion
		
		public ValidScope()
		{
			this._Definition = default(EntityRef<Definition>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="int NOT NULL IDENTITY", IsPrimaryKey=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DefinitionId")]
		public int DefinitionId
		{
			get
			{
				return this._DefinitionId;
			}
			set
			{
				if ((this._DefinitionId != value))
				{
					if (this._Definition.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnDefinitionIdChanging(value);
					this.SendPropertyChanging();
					this._DefinitionId = value;
					this.SendPropertyChanged("DefinitionId");
					this.OnDefinitionIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_XPath", DbType="NVARCHAR(MAX)", CanBeNull=false)]
		public string XPath
		{
			get
			{
				return this._XPath;
			}
			set
			{
				if ((this._XPath != value))
				{
					this.OnXPathChanging(value);
					this.SendPropertyChanging();
					this._XPath = value;
					this.SendPropertyChanged("XPath");
					this.OnXPathChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Definition_ValidScope", Storage="_Definition", ThisKey="DefinitionId", OtherKey="Id", IsForeignKey=true)]
		public Definition Definition
		{
			get
			{
				return this._Definition.Entity;
			}
			set
			{
				Definition previousValue = this._Definition.Entity;
				if (((previousValue != value) 
							|| (this._Definition.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Definition.Entity = null;
						previousValue.ValidScopes.Remove(this);
					}
					this._Definition.Entity = value;
					if ((value != null))
					{
						value.ValidScopes.Add(this);
						this._DefinitionId = value.Id;
					}
					else
					{
						this._DefinitionId = default(int);
					}
					this.SendPropertyChanged("Definition");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class MethodCall : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _CallerId;
		
		private int _CalleeId;
		
		private string _XPath;
		
		private int _LineNumber;
		
		private EntityRef<Definition> _CallerDefinition;
		
		private EntityRef<Definition> _CalleeDefinition;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCallerIdChanging(int value);
    partial void OnCallerIdChanged();
    partial void OnCalleeIdChanging(int value);
    partial void OnCalleeIdChanged();
    partial void OnXPathChanging(string value);
    partial void OnXPathChanged();
    partial void OnLineNumberChanging(int value);
    partial void OnLineNumberChanged();
    #endregion
		
		public MethodCall()
		{
			this._CallerDefinition = default(EntityRef<Definition>);
			this._CalleeDefinition = default(EntityRef<Definition>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CallerId")]
		public int CallerId
		{
			get
			{
				return this._CallerId;
			}
			set
			{
				if ((this._CallerId != value))
				{
					if (this._CallerDefinition.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnCallerIdChanging(value);
					this.SendPropertyChanging();
					this._CallerId = value;
					this.SendPropertyChanged("CallerId");
					this.OnCallerIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CalleeId")]
		public int CalleeId
		{
			get
			{
				return this._CalleeId;
			}
			set
			{
				if ((this._CalleeId != value))
				{
					if (this._CalleeDefinition.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnCalleeIdChanging(value);
					this.SendPropertyChanging();
					this._CalleeId = value;
					this.SendPropertyChanged("CalleeId");
					this.OnCalleeIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_XPath", CanBeNull=false)]
		public string XPath
		{
			get
			{
				return this._XPath;
			}
			set
			{
				if ((this._XPath != value))
				{
					this.OnXPathChanging(value);
					this.SendPropertyChanging();
					this._XPath = value;
					this.SendPropertyChanged("XPath");
					this.OnXPathChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LineNumber")]
		public int LineNumber
		{
			get
			{
				return this._LineNumber;
			}
			set
			{
				if ((this._LineNumber != value))
				{
					this.OnLineNumberChanging(value);
					this.SendPropertyChanging();
					this._LineNumber = value;
					this.SendPropertyChanged("LineNumber");
					this.OnLineNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Definition_MethodCall", Storage="_CallerDefinition", ThisKey="CallerId", OtherKey="Id", IsForeignKey=true)]
		public Definition CallerDefinition
		{
			get
			{
				return this._CallerDefinition.Entity;
			}
			set
			{
				Definition previousValue = this._CallerDefinition.Entity;
				if (((previousValue != value) 
							|| (this._CallerDefinition.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._CallerDefinition.Entity = null;
						previousValue.CallsFromMethod.Remove(this);
					}
					this._CallerDefinition.Entity = value;
					if ((value != null))
					{
						value.CallsFromMethod.Add(this);
						this._CallerId = value.Id;
					}
					else
					{
						this._CallerId = default(int);
					}
					this.SendPropertyChanged("CallerDefinition");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Definition_MethodCall1", Storage="_CalleeDefinition", ThisKey="CalleeId", OtherKey="Id", IsForeignKey=true)]
		public Definition CalleeDefinition
		{
			get
			{
				return this._CalleeDefinition.Entity;
			}
			set
			{
				Definition previousValue = this._CalleeDefinition.Entity;
				if (((previousValue != value) 
							|| (this._CalleeDefinition.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._CalleeDefinition.Entity = null;
						previousValue.CallsToMethod.Remove(this);
					}
					this._CalleeDefinition.Entity = value;
					if ((value != null))
					{
						value.CallsToMethod.Add(this);
						this._CalleeId = value.Id;
					}
					else
					{
						this._CalleeId = default(int);
					}
					this.SendPropertyChanged("CalleeDefinition");
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
