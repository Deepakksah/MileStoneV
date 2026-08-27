using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Security;

[Serializable]
[DesignerCategory("code")]
[ToolboxItem(true)]
[XmlSchemaProvider("GetTypedDataSetSchema")]
[XmlRoot("SecurityDataSet")]
[HelpKeyword("vs.data.DataSet")]
public class SecurityDataSet : DataSet
{
	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class SecurityPermissionsDataTable : TypedTableBase<SecurityPermissionsRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDRole;

		private DataColumn columnSecurityAttributeID;

		private DataColumn columnValue;

		private DataColumn columnIDTimeProfile;

		private DataColumn columnIsDefaultTimeProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRoleColumn => columnIDRole;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SecurityAttributeIDColumn => columnSecurityAttributeID;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ValueColumn => columnValue;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDTimeProfileColumn => columnIDTimeProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IsDefaultTimeProfileColumn => columnIsDefaultTimeProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsRow this[int index] => (SecurityPermissionsRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsRowChangeEventHandler SecurityPermissionsRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsRowChangeEventHandler SecurityPermissionsRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsRowChangeEventHandler SecurityPermissionsRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsRowChangeEventHandler SecurityPermissionsRowDeleted;

		public SecurityPermissionsRow[] FindByRole(Guid roleId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0}='{1}'", columnIDRole.ColumnName, roleId);
			return (SecurityPermissionsRow[])Select(filterExpression);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDataTable()
		{
			base.TableName = "SecurityPermissions";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDataTable(DataTable table)
		{
			base.TableName = table.TableName;
			if (table.CaseSensitive != table.DataSet.CaseSensitive)
			{
				base.CaseSensitive = table.CaseSensitive;
			}
			if (table.Locale.ToString() != table.DataSet.Locale.ToString())
			{
				base.Locale = table.Locale;
			}
			if (table.Namespace != table.DataSet.Namespace)
			{
				base.Namespace = table.Namespace;
			}
			base.Prefix = table.Prefix;
			base.MinimumCapacity = table.MinimumCapacity;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected SecurityPermissionsDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddSecurityPermissionsRow(SecurityPermissionsRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsRow AddSecurityPermissionsRow(DateTime LastModified, string IDRole, string SecurityAttributeID, string Value, string IDTimeProfile, bool IsDefaultTimeProfile)
		{
			SecurityPermissionsRow securityPermissionsRow = (SecurityPermissionsRow)NewRow();
			object[] itemArray = new object[6] { LastModified, IDRole, SecurityAttributeID, Value, IDTimeProfile, IsDefaultTimeProfile };
			securityPermissionsRow.ItemArray = itemArray;
			base.Rows.Add(securityPermissionsRow);
			return securityPermissionsRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsRow FindByIDRoleSecurityAttributeID(string IDRole, string SecurityAttributeID)
		{
			return (SecurityPermissionsRow)base.Rows.Find(new object[2] { IDRole, SecurityAttributeID });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			SecurityPermissionsDataTable obj = (SecurityPermissionsDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new SecurityPermissionsDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDRole = base.Columns["IDRole"];
			columnSecurityAttributeID = base.Columns["SecurityAttributeID"];
			columnValue = base.Columns["Value"];
			columnIDTimeProfile = base.Columns["IDTimeProfile"];
			columnIsDefaultTimeProfile = base.Columns["IsDefaultTimeProfile"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDRole = new DataColumn("IDRole", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRole);
			columnSecurityAttributeID = new DataColumn("SecurityAttributeID", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSecurityAttributeID);
			columnValue = new DataColumn("Value", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnValue);
			columnIDTimeProfile = new DataColumn("IDTimeProfile", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDTimeProfile);
			columnIsDefaultTimeProfile = new DataColumn("IsDefaultTimeProfile", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnIsDefaultTimeProfile);
			base.Constraints.Add(new UniqueConstraint("PK_SecurityPermissions", new DataColumn[2] { columnIDRole, columnSecurityAttributeID }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDRole.AllowDBNull = false;
			columnIDRole.MaxLength = 36;
			columnSecurityAttributeID.AllowDBNull = false;
			columnSecurityAttributeID.MaxLength = 100;
			columnValue.MaxLength = 256;
			columnIDTimeProfile.MaxLength = 36;
			columnIsDefaultTimeProfile.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsRow NewSecurityPermissionsRow()
		{
			return (SecurityPermissionsRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new SecurityPermissionsRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(SecurityPermissionsRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.SecurityPermissionsRowChanged != null)
			{
				this.SecurityPermissionsRowChanged(this, new SecurityPermissionsRowChangeEvent((SecurityPermissionsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.SecurityPermissionsRowChanging != null)
			{
				this.SecurityPermissionsRowChanging(this, new SecurityPermissionsRowChangeEvent((SecurityPermissionsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.SecurityPermissionsRowDeleted != null)
			{
				this.SecurityPermissionsRowDeleted(this, new SecurityPermissionsRowChangeEvent((SecurityPermissionsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.SecurityPermissionsRowDeleting != null)
			{
				this.SecurityPermissionsRowDeleting(this, new SecurityPermissionsRowChangeEvent((SecurityPermissionsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveSecurityPermissionsRow(SecurityPermissionsRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			SecurityDataSet securityDataSet = new SecurityDataSet();
			XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
			xmlSchemaAny.Namespace = "http://www.w3.org/2001/XMLSchema";
			xmlSchemaAny.MinOccurs = 0m;
			xmlSchemaAny.MaxOccurs = decimal.MaxValue;
			xmlSchemaAny.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny);
			XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
			xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
			xmlSchemaAny2.MinOccurs = 1m;
			xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny2);
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			xmlSchemaAttribute.Name = "namespace";
			xmlSchemaAttribute.FixedValue = securityDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "SecurityPermissionsDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = securityDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						XmlSchema obj = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						obj.Write(memoryStream2);
						if (memoryStream.Length == memoryStream2.Length)
						{
							memoryStream.Position = 0L;
							memoryStream2.Position = 0L;
							while (memoryStream.Position != memoryStream.Length && memoryStream.ReadByte() == memoryStream2.ReadByte())
							{
							}
							if (memoryStream.Position == memoryStream.Length)
							{
								return xmlSchemaComplexType;
							}
						}
					}
				}
				finally
				{
					memoryStream?.Close();
					memoryStream2?.Close();
				}
			}
			xs.Add(schemaSerializable);
			return xmlSchemaComplexType;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class SecurityPermissionsDeviceDataTable : TypedTableBase<SecurityPermissionsDeviceRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnSecurityAttributeID;

		private DataColumn columnIDRole;

		private DataColumn columnIDDevice;

		private DataColumn columnValue;

		private DataColumn columnIDTimeProfile;

		private DataColumn columnTimeBackward;

		private DataColumn columnIsDefaultTimeProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SecurityAttributeIDColumn => columnSecurityAttributeID;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRoleColumn => columnIDRole;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ValueColumn => columnValue;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDTimeProfileColumn => columnIDTimeProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn TimeBackwardColumn => columnTimeBackward;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IsDefaultTimeProfileColumn => columnIsDefaultTimeProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDeviceRow this[int index] => (SecurityPermissionsDeviceRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsDeviceRowChangeEventHandler SecurityPermissionsDeviceRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsDeviceRowChangeEventHandler SecurityPermissionsDeviceRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsDeviceRowChangeEventHandler SecurityPermissionsDeviceRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsDeviceRowChangeEventHandler SecurityPermissionsDeviceRowDeleted;

		public SecurityPermissionsDeviceRow[] FindByRole(Guid roleId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0}='{1}'", columnIDRole.ColumnName, roleId);
			return (SecurityPermissionsDeviceRow[])Select(filterExpression);
		}

		public SecurityPermissionsDeviceRow AddSecurityPermissionsDeviceRow(DateTime lastModified, string securityAttributeID, Guid roleId, Guid deviceId, string value, string timeProfileId, int timeBackward, bool isDefaultTimeProfile)
		{
			return AddSecurityPermissionsDeviceRow(lastModified, securityAttributeID, roleId.ToString(), deviceId.ToString(), value, timeProfileId, timeBackward, isDefaultTimeProfile);
		}

		public SecurityPermissionsDeviceRow FindBySecurityAttributeIDIDRoleIDDevice(string securityAttributeId, Guid roleId, Guid deviceId)
		{
			return FindBySecurityAttributeIDIDRoleIDDevice(securityAttributeId, roleId.ToString(), deviceId.ToString());
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDeviceDataTable()
		{
			base.TableName = "SecurityPermissionsDevice";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDeviceDataTable(DataTable table)
		{
			base.TableName = table.TableName;
			if (table.CaseSensitive != table.DataSet.CaseSensitive)
			{
				base.CaseSensitive = table.CaseSensitive;
			}
			if (table.Locale.ToString() != table.DataSet.Locale.ToString())
			{
				base.Locale = table.Locale;
			}
			if (table.Namespace != table.DataSet.Namespace)
			{
				base.Namespace = table.Namespace;
			}
			base.Prefix = table.Prefix;
			base.MinimumCapacity = table.MinimumCapacity;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected SecurityPermissionsDeviceDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddSecurityPermissionsDeviceRow(SecurityPermissionsDeviceRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDeviceRow AddSecurityPermissionsDeviceRow(DateTime LastModified, string SecurityAttributeID, string IDRole, string IDDevice, string Value, string IDTimeProfile, int TimeBackward, bool IsDefaultTimeProfile)
		{
			SecurityPermissionsDeviceRow securityPermissionsDeviceRow = (SecurityPermissionsDeviceRow)NewRow();
			object[] itemArray = new object[8] { LastModified, SecurityAttributeID, IDRole, IDDevice, Value, IDTimeProfile, TimeBackward, IsDefaultTimeProfile };
			securityPermissionsDeviceRow.ItemArray = itemArray;
			base.Rows.Add(securityPermissionsDeviceRow);
			return securityPermissionsDeviceRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDeviceRow FindBySecurityAttributeIDIDRoleIDDevice(string SecurityAttributeID, string IDRole, string IDDevice)
		{
			return (SecurityPermissionsDeviceRow)base.Rows.Find(new object[3] { SecurityAttributeID, IDRole, IDDevice });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			SecurityPermissionsDeviceDataTable obj = (SecurityPermissionsDeviceDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new SecurityPermissionsDeviceDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnSecurityAttributeID = base.Columns["SecurityAttributeID"];
			columnIDRole = base.Columns["IDRole"];
			columnIDDevice = base.Columns["IDDevice"];
			columnValue = base.Columns["Value"];
			columnIDTimeProfile = base.Columns["IDTimeProfile"];
			columnTimeBackward = base.Columns["TimeBackward"];
			columnIsDefaultTimeProfile = base.Columns["IsDefaultTimeProfile"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnSecurityAttributeID = new DataColumn("SecurityAttributeID", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSecurityAttributeID);
			columnIDRole = new DataColumn("IDRole", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRole);
			columnIDDevice = new DataColumn("IDDevice", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnValue = new DataColumn("Value", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnValue);
			columnIDTimeProfile = new DataColumn("IDTimeProfile", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDTimeProfile);
			columnTimeBackward = new DataColumn("TimeBackward", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnTimeBackward);
			columnIsDefaultTimeProfile = new DataColumn("IsDefaultTimeProfile", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnIsDefaultTimeProfile);
			base.Constraints.Add(new UniqueConstraint("PK_SecurityPermissionsDevice", new DataColumn[3] { columnSecurityAttributeID, columnIDRole, columnIDDevice }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnSecurityAttributeID.AllowDBNull = false;
			columnSecurityAttributeID.MaxLength = 100;
			columnIDRole.AllowDBNull = false;
			columnIDRole.MaxLength = 36;
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.MaxLength = 36;
			columnValue.MaxLength = 256;
			columnIDTimeProfile.MaxLength = 36;
			columnTimeBackward.AllowDBNull = false;
			columnIsDefaultTimeProfile.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDeviceRow NewSecurityPermissionsDeviceRow()
		{
			return (SecurityPermissionsDeviceRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new SecurityPermissionsDeviceRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(SecurityPermissionsDeviceRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.SecurityPermissionsDeviceRowChanged != null)
			{
				this.SecurityPermissionsDeviceRowChanged(this, new SecurityPermissionsDeviceRowChangeEvent((SecurityPermissionsDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.SecurityPermissionsDeviceRowChanging != null)
			{
				this.SecurityPermissionsDeviceRowChanging(this, new SecurityPermissionsDeviceRowChangeEvent((SecurityPermissionsDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.SecurityPermissionsDeviceRowDeleted != null)
			{
				this.SecurityPermissionsDeviceRowDeleted(this, new SecurityPermissionsDeviceRowChangeEvent((SecurityPermissionsDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.SecurityPermissionsDeviceRowDeleting != null)
			{
				this.SecurityPermissionsDeviceRowDeleting(this, new SecurityPermissionsDeviceRowChangeEvent((SecurityPermissionsDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveSecurityPermissionsDeviceRow(SecurityPermissionsDeviceRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			SecurityDataSet securityDataSet = new SecurityDataSet();
			XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
			xmlSchemaAny.Namespace = "http://www.w3.org/2001/XMLSchema";
			xmlSchemaAny.MinOccurs = 0m;
			xmlSchemaAny.MaxOccurs = decimal.MaxValue;
			xmlSchemaAny.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny);
			XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
			xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
			xmlSchemaAny2.MinOccurs = 1m;
			xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny2);
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			xmlSchemaAttribute.Name = "namespace";
			xmlSchemaAttribute.FixedValue = securityDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "SecurityPermissionsDeviceDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = securityDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						XmlSchema obj = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						obj.Write(memoryStream2);
						if (memoryStream.Length == memoryStream2.Length)
						{
							memoryStream.Position = 0L;
							memoryStream2.Position = 0L;
							while (memoryStream.Position != memoryStream.Length && memoryStream.ReadByte() == memoryStream2.ReadByte())
							{
							}
							if (memoryStream.Position == memoryStream.Length)
							{
								return xmlSchemaComplexType;
							}
						}
					}
				}
				finally
				{
					memoryStream?.Close();
					memoryStream2?.Close();
				}
			}
			xs.Add(schemaSerializable);
			return xmlSchemaComplexType;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class SecurityPermissionsMatrixDataTable : TypedTableBase<SecurityPermissionsMatrixRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnSecurityAttributeID;

		private DataColumn columnIDRole;

		private DataColumn columnIDMatrix;

		private DataColumn columnValue;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SecurityAttributeIDColumn => columnSecurityAttributeID;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRoleColumn => columnIDRole;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDMatrixColumn => columnIDMatrix;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ValueColumn => columnValue;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsMatrixRow this[int index] => (SecurityPermissionsMatrixRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsMatrixRowChangeEventHandler SecurityPermissionsMatrixRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsMatrixRowChangeEventHandler SecurityPermissionsMatrixRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsMatrixRowChangeEventHandler SecurityPermissionsMatrixRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityPermissionsMatrixRowChangeEventHandler SecurityPermissionsMatrixRowDeleted;

		public SecurityPermissionsMatrixRow[] FindByRole(Guid roleId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0}='{1}'", columnIDRole.ColumnName, roleId);
			return (SecurityPermissionsMatrixRow[])Select(filterExpression);
		}

		public SecurityPermissionsMatrixRow[] FindByRoleAndSecurityAttribute(Guid roleId, string securityAttributeId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0}='{1}' AND {2}='{3}'", columnIDRole.ColumnName, roleId, columnSecurityAttributeID.ColumnName, securityAttributeId);
			return (SecurityPermissionsMatrixRow[])Select(filterExpression);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsMatrixDataTable()
		{
			base.TableName = "SecurityPermissionsMatrix";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsMatrixDataTable(DataTable table)
		{
			base.TableName = table.TableName;
			if (table.CaseSensitive != table.DataSet.CaseSensitive)
			{
				base.CaseSensitive = table.CaseSensitive;
			}
			if (table.Locale.ToString() != table.DataSet.Locale.ToString())
			{
				base.Locale = table.Locale;
			}
			if (table.Namespace != table.DataSet.Namespace)
			{
				base.Namespace = table.Namespace;
			}
			base.Prefix = table.Prefix;
			base.MinimumCapacity = table.MinimumCapacity;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected SecurityPermissionsMatrixDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddSecurityPermissionsMatrixRow(SecurityPermissionsMatrixRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsMatrixRow AddSecurityPermissionsMatrixRow(DateTime LastModified, string SecurityAttributeID, string IDRole, string IDMatrix, string Value)
		{
			SecurityPermissionsMatrixRow securityPermissionsMatrixRow = (SecurityPermissionsMatrixRow)NewRow();
			object[] itemArray = new object[5] { LastModified, SecurityAttributeID, IDRole, IDMatrix, Value };
			securityPermissionsMatrixRow.ItemArray = itemArray;
			base.Rows.Add(securityPermissionsMatrixRow);
			return securityPermissionsMatrixRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsMatrixRow FindBySecurityAttributeIDIDRoleIDMatrix(string SecurityAttributeID, string IDRole, string IDMatrix)
		{
			return (SecurityPermissionsMatrixRow)base.Rows.Find(new object[3] { SecurityAttributeID, IDRole, IDMatrix });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			SecurityPermissionsMatrixDataTable obj = (SecurityPermissionsMatrixDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new SecurityPermissionsMatrixDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnSecurityAttributeID = base.Columns["SecurityAttributeID"];
			columnIDRole = base.Columns["IDRole"];
			columnIDMatrix = base.Columns["IDMatrix"];
			columnValue = base.Columns["Value"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnSecurityAttributeID = new DataColumn("SecurityAttributeID", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSecurityAttributeID);
			columnIDRole = new DataColumn("IDRole", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRole);
			columnIDMatrix = new DataColumn("IDMatrix", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDMatrix);
			columnValue = new DataColumn("Value", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnValue);
			base.Constraints.Add(new UniqueConstraint("PK_SecurityPermissionsMatrix", new DataColumn[3] { columnSecurityAttributeID, columnIDRole, columnIDMatrix }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnSecurityAttributeID.AllowDBNull = false;
			columnSecurityAttributeID.MaxLength = 100;
			columnIDRole.AllowDBNull = false;
			columnIDRole.MaxLength = 36;
			columnIDMatrix.AllowDBNull = false;
			columnIDMatrix.MaxLength = 36;
			columnValue.AllowDBNull = false;
			columnValue.MaxLength = 256;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsMatrixRow NewSecurityPermissionsMatrixRow()
		{
			return (SecurityPermissionsMatrixRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new SecurityPermissionsMatrixRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(SecurityPermissionsMatrixRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.SecurityPermissionsMatrixRowChanged != null)
			{
				this.SecurityPermissionsMatrixRowChanged(this, new SecurityPermissionsMatrixRowChangeEvent((SecurityPermissionsMatrixRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.SecurityPermissionsMatrixRowChanging != null)
			{
				this.SecurityPermissionsMatrixRowChanging(this, new SecurityPermissionsMatrixRowChangeEvent((SecurityPermissionsMatrixRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.SecurityPermissionsMatrixRowDeleted != null)
			{
				this.SecurityPermissionsMatrixRowDeleted(this, new SecurityPermissionsMatrixRowChangeEvent((SecurityPermissionsMatrixRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.SecurityPermissionsMatrixRowDeleting != null)
			{
				this.SecurityPermissionsMatrixRowDeleting(this, new SecurityPermissionsMatrixRowChangeEvent((SecurityPermissionsMatrixRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveSecurityPermissionsMatrixRow(SecurityPermissionsMatrixRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			SecurityDataSet securityDataSet = new SecurityDataSet();
			XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
			xmlSchemaAny.Namespace = "http://www.w3.org/2001/XMLSchema";
			xmlSchemaAny.MinOccurs = 0m;
			xmlSchemaAny.MaxOccurs = decimal.MaxValue;
			xmlSchemaAny.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny);
			XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
			xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
			xmlSchemaAny2.MinOccurs = 1m;
			xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny2);
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			xmlSchemaAttribute.Name = "namespace";
			xmlSchemaAttribute.FixedValue = securityDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "SecurityPermissionsMatrixDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = securityDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						XmlSchema obj = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						obj.Write(memoryStream2);
						if (memoryStream.Length == memoryStream2.Length)
						{
							memoryStream.Position = 0L;
							memoryStream2.Position = 0L;
							while (memoryStream.Position != memoryStream.Length && memoryStream.ReadByte() == memoryStream2.ReadByte())
							{
							}
							if (memoryStream.Position == memoryStream.Length)
							{
								return xmlSchemaComplexType;
							}
						}
					}
				}
				finally
				{
					memoryStream?.Close();
					memoryStream2?.Close();
				}
			}
			xs.Add(schemaSerializable);
			return xmlSchemaComplexType;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class TimeProfileDefaultRoleRelationDataTable : TypedTableBase<TimeProfileDefaultRoleRelationRow>
	{
		private DataColumn columnIDTimeProfile;

		private DataColumn columnIDRole;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDTimeProfileColumn => columnIDTimeProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRoleColumn => columnIDRole;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDefaultRoleRelationRow this[int index] => (TimeProfileDefaultRoleRelationRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event TimeProfileDefaultRoleRelationRowChangeEventHandler TimeProfileDefaultRoleRelationRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event TimeProfileDefaultRoleRelationRowChangeEventHandler TimeProfileDefaultRoleRelationRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event TimeProfileDefaultRoleRelationRowChangeEventHandler TimeProfileDefaultRoleRelationRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event TimeProfileDefaultRoleRelationRowChangeEventHandler TimeProfileDefaultRoleRelationRowDeleted;

		public TimeProfileDefaultRoleRelationRow[] FindByRole(Guid roleId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0}='{1}'", columnIDRole.ColumnName, roleId);
			return (TimeProfileDefaultRoleRelationRow[])Select(filterExpression);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDefaultRoleRelationDataTable()
		{
			base.TableName = "TimeProfileDefaultRoleRelation";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDefaultRoleRelationDataTable(DataTable table)
		{
			base.TableName = table.TableName;
			if (table.CaseSensitive != table.DataSet.CaseSensitive)
			{
				base.CaseSensitive = table.CaseSensitive;
			}
			if (table.Locale.ToString() != table.DataSet.Locale.ToString())
			{
				base.Locale = table.Locale;
			}
			if (table.Namespace != table.DataSet.Namespace)
			{
				base.Namespace = table.Namespace;
			}
			base.Prefix = table.Prefix;
			base.MinimumCapacity = table.MinimumCapacity;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected TimeProfileDefaultRoleRelationDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddTimeProfileDefaultRoleRelationRow(TimeProfileDefaultRoleRelationRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDefaultRoleRelationRow AddTimeProfileDefaultRoleRelationRow(string IDTimeProfile, string IDRole)
		{
			TimeProfileDefaultRoleRelationRow timeProfileDefaultRoleRelationRow = (TimeProfileDefaultRoleRelationRow)NewRow();
			object[] itemArray = new object[2] { IDTimeProfile, IDRole };
			timeProfileDefaultRoleRelationRow.ItemArray = itemArray;
			base.Rows.Add(timeProfileDefaultRoleRelationRow);
			return timeProfileDefaultRoleRelationRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDefaultRoleRelationRow FindByIDRole(string IDRole)
		{
			return (TimeProfileDefaultRoleRelationRow)base.Rows.Find(new object[1] { IDRole });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			TimeProfileDefaultRoleRelationDataTable obj = (TimeProfileDefaultRoleRelationDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new TimeProfileDefaultRoleRelationDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDTimeProfile = base.Columns["IDTimeProfile"];
			columnIDRole = base.Columns["IDRole"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDTimeProfile = new DataColumn("IDTimeProfile", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDTimeProfile);
			columnIDRole = new DataColumn("IDRole", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRole);
			base.Constraints.Add(new UniqueConstraint("PK_TimeProfileDefaultRoleAction", new DataColumn[1] { columnIDRole }, isPrimaryKey: true));
			columnIDTimeProfile.AllowDBNull = false;
			columnIDTimeProfile.MaxLength = 36;
			columnIDRole.AllowDBNull = false;
			columnIDRole.Unique = true;
			columnIDRole.MaxLength = 36;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDefaultRoleRelationRow NewTimeProfileDefaultRoleRelationRow()
		{
			return (TimeProfileDefaultRoleRelationRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new TimeProfileDefaultRoleRelationRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(TimeProfileDefaultRoleRelationRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.TimeProfileDefaultRoleRelationRowChanged != null)
			{
				this.TimeProfileDefaultRoleRelationRowChanged(this, new TimeProfileDefaultRoleRelationRowChangeEvent((TimeProfileDefaultRoleRelationRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.TimeProfileDefaultRoleRelationRowChanging != null)
			{
				this.TimeProfileDefaultRoleRelationRowChanging(this, new TimeProfileDefaultRoleRelationRowChangeEvent((TimeProfileDefaultRoleRelationRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.TimeProfileDefaultRoleRelationRowDeleted != null)
			{
				this.TimeProfileDefaultRoleRelationRowDeleted(this, new TimeProfileDefaultRoleRelationRowChangeEvent((TimeProfileDefaultRoleRelationRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.TimeProfileDefaultRoleRelationRowDeleting != null)
			{
				this.TimeProfileDefaultRoleRelationRowDeleting(this, new TimeProfileDefaultRoleRelationRowChangeEvent((TimeProfileDefaultRoleRelationRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveTimeProfileDefaultRoleRelationRow(TimeProfileDefaultRoleRelationRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			SecurityDataSet securityDataSet = new SecurityDataSet();
			XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
			xmlSchemaAny.Namespace = "http://www.w3.org/2001/XMLSchema";
			xmlSchemaAny.MinOccurs = 0m;
			xmlSchemaAny.MaxOccurs = decimal.MaxValue;
			xmlSchemaAny.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny);
			XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
			xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
			xmlSchemaAny2.MinOccurs = 1m;
			xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny2);
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			xmlSchemaAttribute.Name = "namespace";
			xmlSchemaAttribute.FixedValue = securityDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "TimeProfileDefaultRoleRelationDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = securityDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						XmlSchema obj = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						obj.Write(memoryStream2);
						if (memoryStream.Length == memoryStream2.Length)
						{
							memoryStream.Position = 0L;
							memoryStream2.Position = 0L;
							while (memoryStream.Position != memoryStream.Length && memoryStream.ReadByte() == memoryStream2.ReadByte())
							{
							}
							if (memoryStream.Position == memoryStream.Length)
							{
								return xmlSchemaComplexType;
							}
						}
					}
				}
				finally
				{
					memoryStream?.Close();
					memoryStream2?.Close();
				}
			}
			xs.Add(schemaSerializable);
			return xmlSchemaComplexType;
		}
	}

	public class SecurityPermissionsMatrixRow : DataRow
	{
		private SecurityPermissionsMatrixDataTable tableSecurityPermissionsMatrix;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableSecurityPermissionsMatrix.LastModifiedColumn];
			}
			set
			{
				base[tableSecurityPermissionsMatrix.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string SecurityAttributeID
		{
			get
			{
				return (string)base[tableSecurityPermissionsMatrix.SecurityAttributeIDColumn];
			}
			set
			{
				base[tableSecurityPermissionsMatrix.SecurityAttributeIDColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRole
		{
			get
			{
				return (string)base[tableSecurityPermissionsMatrix.IDRoleColumn];
			}
			set
			{
				base[tableSecurityPermissionsMatrix.IDRoleColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDMatrix
		{
			get
			{
				return (string)base[tableSecurityPermissionsMatrix.IDMatrixColumn];
			}
			set
			{
				base[tableSecurityPermissionsMatrix.IDMatrixColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Value
		{
			get
			{
				return (string)base[tableSecurityPermissionsMatrix.ValueColumn];
			}
			set
			{
				base[tableSecurityPermissionsMatrix.ValueColumn] = value;
			}
		}

		public string GetRoleId()
		{
			try
			{
				DataRowVersion version = ((base.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current);
				return (string)base[tableSecurityPermissionsMatrix.IDRoleColumn, version];
			}
			catch (InvalidCastException innerException)
			{
				throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", tableSecurityPermissionsMatrix.IDRoleColumn.ColumnName, tableSecurityPermissionsMatrix.TableName), innerException);
			}
		}

		public string GetMatrixId()
		{
			try
			{
				DataRowVersion version = ((base.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current);
				return (string)base[tableSecurityPermissionsMatrix.IDMatrixColumn, version];
			}
			catch (InvalidCastException innerException)
			{
				throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", tableSecurityPermissionsMatrix.IDMatrixColumn.ColumnName, tableSecurityPermissionsMatrix.TableName), innerException);
			}
		}

		public string GetSecurityAttributeId()
		{
			try
			{
				DataRowVersion version = ((base.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current);
				return (string)base[tableSecurityPermissionsMatrix.SecurityAttributeIDColumn, version];
			}
			catch (InvalidCastException innerException)
			{
				throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", tableSecurityPermissionsMatrix.SecurityAttributeIDColumn.ColumnName, tableSecurityPermissionsMatrix.TableName), innerException);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsMatrixRow(DataRowBuilder rb)
			: base(rb)
		{
			tableSecurityPermissionsMatrix = (SecurityPermissionsMatrixDataTable)base.Table;
		}
	}

	public class SecurityPermissionsRow : DataRow
	{
		private SecurityPermissionsDataTable tableSecurityPermissions;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableSecurityPermissions.LastModifiedColumn];
			}
			set
			{
				base[tableSecurityPermissions.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRole
		{
			get
			{
				return (string)base[tableSecurityPermissions.IDRoleColumn];
			}
			set
			{
				base[tableSecurityPermissions.IDRoleColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string SecurityAttributeID
		{
			get
			{
				return (string)base[tableSecurityPermissions.SecurityAttributeIDColumn];
			}
			set
			{
				base[tableSecurityPermissions.SecurityAttributeIDColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Value
		{
			get
			{
				try
				{
					return (string)base[tableSecurityPermissions.ValueColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Value' in table 'SecurityPermissions' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableSecurityPermissions.ValueColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDTimeProfile
		{
			get
			{
				try
				{
					return (string)base[tableSecurityPermissions.IDTimeProfileColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'IDTimeProfile' in table 'SecurityPermissions' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableSecurityPermissions.IDTimeProfileColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsDefaultTimeProfile
		{
			get
			{
				return (bool)base[tableSecurityPermissions.IsDefaultTimeProfileColumn];
			}
			set
			{
				base[tableSecurityPermissions.IsDefaultTimeProfileColumn] = value;
			}
		}

		public string GetRoleId()
		{
			try
			{
				DataRowVersion version = ((base.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current);
				return (string)base[tableSecurityPermissions.IDRoleColumn, version];
			}
			catch (InvalidCastException innerException)
			{
				throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", tableSecurityPermissions.IDRoleColumn.ColumnName, tableSecurityPermissions.TableName), innerException);
			}
		}

		public string GetTimeProfileId(DataRowVersion dataRowVersion)
		{
			return (string)base[tableSecurityPermissions.IDTimeProfileColumn, dataRowVersion];
		}

		public bool GetIsDefaultTimeProfile(DataRowVersion dataRowVersion)
		{
			return this.GetColumnValue<bool>(tableSecurityPermissions.IsDefaultTimeProfileColumn, dataRowVersion);
		}

		public bool IsIDTimeProfileNull(DataRowVersion dataRowVersion)
		{
			object obj = base[tableSecurityPermissions.IDTimeProfileColumn, dataRowVersion];
			if (!(obj is DBNull))
			{
				return obj == null;
			}
			return true;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsRow(DataRowBuilder rb)
			: base(rb)
		{
			tableSecurityPermissions = (SecurityPermissionsDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsValueNull()
		{
			return IsNull(tableSecurityPermissions.ValueColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetValueNull()
		{
			base[tableSecurityPermissions.ValueColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsIDTimeProfileNull()
		{
			return IsNull(tableSecurityPermissions.IDTimeProfileColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetIDTimeProfileNull()
		{
			base[tableSecurityPermissions.IDTimeProfileColumn] = Convert.DBNull;
		}
	}

	public class SecurityPermissionsDeviceRow : DataRow
	{
		private SecurityPermissionsDeviceDataTable tableSecurityPermissionsDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableSecurityPermissionsDevice.LastModifiedColumn];
			}
			set
			{
				base[tableSecurityPermissionsDevice.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string SecurityAttributeID
		{
			get
			{
				return (string)base[tableSecurityPermissionsDevice.SecurityAttributeIDColumn];
			}
			set
			{
				base[tableSecurityPermissionsDevice.SecurityAttributeIDColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRole
		{
			get
			{
				return (string)base[tableSecurityPermissionsDevice.IDRoleColumn];
			}
			set
			{
				base[tableSecurityPermissionsDevice.IDRoleColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDevice
		{
			get
			{
				return (string)base[tableSecurityPermissionsDevice.IDDeviceColumn];
			}
			set
			{
				base[tableSecurityPermissionsDevice.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Value
		{
			get
			{
				try
				{
					return (string)base[tableSecurityPermissionsDevice.ValueColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Value' in table 'SecurityPermissionsDevice' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableSecurityPermissionsDevice.ValueColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDTimeProfile
		{
			get
			{
				try
				{
					return (string)base[tableSecurityPermissionsDevice.IDTimeProfileColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'IDTimeProfile' in table 'SecurityPermissionsDevice' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableSecurityPermissionsDevice.IDTimeProfileColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int TimeBackward
		{
			get
			{
				return (int)base[tableSecurityPermissionsDevice.TimeBackwardColumn];
			}
			set
			{
				base[tableSecurityPermissionsDevice.TimeBackwardColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsDefaultTimeProfile
		{
			get
			{
				return (bool)base[tableSecurityPermissionsDevice.IsDefaultTimeProfileColumn];
			}
			set
			{
				base[tableSecurityPermissionsDevice.IsDefaultTimeProfileColumn] = value;
			}
		}

		public bool GetIsDefaultTimeProfile(DataRowVersion dataRowVersion)
		{
			return this.GetColumnValue<bool>(tableSecurityPermissionsDevice.IsDefaultTimeProfileColumn, dataRowVersion);
		}

		public bool IsIDTimeProfileNull(DataRowVersion dataRowVersion)
		{
			object obj = base[tableSecurityPermissionsDevice.IDTimeProfileColumn, dataRowVersion];
			if (!(obj is DBNull))
			{
				return obj == null;
			}
			return true;
		}

		public string GetIDTimeProfile(DataRowVersion dataRowVersion)
		{
			return this.GetColumnValue<string>(tableSecurityPermissionsDevice.IDTimeProfileColumn, dataRowVersion);
		}

		public string GetValue(DataRowVersion dataRowVersion)
		{
			return (string)base[tableSecurityPermissionsDevice.ValueColumn, dataRowVersion];
		}

		public int GetTimeBackward(DataRowVersion dataRowVersion)
		{
			return this.GetColumnValue<int>(tableSecurityPermissionsDevice.TimeBackwardColumn, dataRowVersion);
		}

		public string GetRoleId()
		{
			try
			{
				DataRowVersion version = ((base.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current);
				return (string)base[tableSecurityPermissionsDevice.IDRoleColumn, version];
			}
			catch (InvalidCastException innerException)
			{
				throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", tableSecurityPermissionsDevice.IDRoleColumn.ColumnName, tableSecurityPermissionsDevice.TableName), innerException);
			}
		}

		public string GetDeviceId()
		{
			try
			{
				DataRowVersion version = ((base.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current);
				return (string)base[tableSecurityPermissionsDevice.IDDeviceColumn, version];
			}
			catch (InvalidCastException innerException)
			{
				throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", tableSecurityPermissionsDevice.IDDeviceColumn.ColumnName, tableSecurityPermissionsDevice.TableName), innerException);
			}
		}

		public string GetSecurityAttributeId()
		{
			try
			{
				DataRowVersion version = ((base.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current);
				return (string)base[tableSecurityPermissionsDevice.SecurityAttributeIDColumn, version];
			}
			catch (InvalidCastException innerException)
			{
				throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", tableSecurityPermissionsDevice.SecurityAttributeIDColumn.ColumnName, tableSecurityPermissionsDevice.TableName), innerException);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDeviceRow(DataRowBuilder rb)
			: base(rb)
		{
			tableSecurityPermissionsDevice = (SecurityPermissionsDeviceDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsValueNull()
		{
			return IsNull(tableSecurityPermissionsDevice.ValueColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetValueNull()
		{
			base[tableSecurityPermissionsDevice.ValueColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsIDTimeProfileNull()
		{
			return IsNull(tableSecurityPermissionsDevice.IDTimeProfileColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetIDTimeProfileNull()
		{
			base[tableSecurityPermissionsDevice.IDTimeProfileColumn] = Convert.DBNull;
		}
	}

	public class TimeProfileDefaultRoleRelationRow : DataRow
	{
		private TimeProfileDefaultRoleRelationDataTable tableTimeProfileDefaultRoleRelation;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDTimeProfile
		{
			get
			{
				return (string)base[tableTimeProfileDefaultRoleRelation.IDTimeProfileColumn];
			}
			set
			{
				base[tableTimeProfileDefaultRoleRelation.IDTimeProfileColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRole
		{
			get
			{
				return (string)base[tableTimeProfileDefaultRoleRelation.IDRoleColumn];
			}
			set
			{
				base[tableTimeProfileDefaultRoleRelation.IDRoleColumn] = value;
			}
		}

		public string GetRoleId()
		{
			try
			{
				DataRowVersion version = ((base.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current);
				return (string)base[tableTimeProfileDefaultRoleRelation.IDRoleColumn, version];
			}
			catch (InvalidCastException innerException)
			{
				throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", tableTimeProfileDefaultRoleRelation.IDRoleColumn.ColumnName, tableTimeProfileDefaultRoleRelation.TableName), innerException);
			}
		}

		public string GetTimeProfileId(DataRowVersion dataRowVersion)
		{
			return (string)base[tableTimeProfileDefaultRoleRelation.IDTimeProfileColumn, dataRowVersion];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDefaultRoleRelationRow(DataRowBuilder rb)
			: base(rb)
		{
			tableTimeProfileDefaultRoleRelation = (TimeProfileDefaultRoleRelationDataTable)base.Table;
		}
	}

	public class SecurityRolesRow : DataRow
	{
		private SecurityRolesDataTable tableSecurityRoles;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableSecurityRoles.LastModifiedColumn];
			}
			set
			{
				base[tableSecurityRoles.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRole
		{
			get
			{
				return (string)base[tableSecurityRoles.IDRoleColumn];
			}
			set
			{
				base[tableSecurityRoles.IDRoleColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableSecurityRoles.NameColumn];
			}
			set
			{
				base[tableSecurityRoles.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableSecurityRoles.DescriptionColumn];
			}
			set
			{
				base[tableSecurityRoles.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Builtin
		{
			get
			{
				return (bool)base[tableSecurityRoles.BuiltinColumn];
			}
			set
			{
				base[tableSecurityRoles.BuiltinColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Enabled
		{
			get
			{
				return (bool)base[tableSecurityRoles.EnabledColumn];
			}
			set
			{
				base[tableSecurityRoles.EnabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public byte Type
		{
			get
			{
				return (byte)base[tableSecurityRoles.TypeColumn];
			}
			set
			{
				base[tableSecurityRoles.TypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool DualRequired
		{
			get
			{
				return (bool)base[tableSecurityRoles.DualRequiredColumn];
			}
			set
			{
				base[tableSecurityRoles.DualRequiredColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool PtzReservedAsAnonymous
		{
			get
			{
				try
				{
					return (bool)base[tableSecurityRoles.PtzReservedAsAnonymousColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'PtzReservedAsAnonymous' in table 'SecurityRoles' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableSecurityRoles.PtzReservedAsAnonymousColumn] = value;
			}
		}

		public string GetRoleId()
		{
			try
			{
				DataRowVersion version = ((base.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current);
				return (string)base[tableSecurityRoles.IDRoleColumn, version];
			}
			catch (InvalidCastException innerException)
			{
				throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", tableSecurityRoles.IDRoleColumn.ColumnName, tableSecurityRoles.TableName), innerException);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityRolesRow(DataRowBuilder rb)
			: base(rb)
		{
			tableSecurityRoles = (SecurityRolesDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsPtzReservedAsAnonymousNull()
		{
			return IsNull(tableSecurityRoles.PtzReservedAsAnonymousColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetPtzReservedAsAnonymousNull()
		{
			base[tableSecurityRoles.PtzReservedAsAnonymousColumn] = Convert.DBNull;
		}
	}

	public class SecurityGroupAccessEntryRow : DataRow
	{
		private SecurityGroupAccessEntryDataTable tableSecurityGroupAccessEntry;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string object_id
		{
			get
			{
				return (string)base[tableSecurityGroupAccessEntry.object_idColumn];
			}
			set
			{
				base[tableSecurityGroupAccessEntry.object_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid namespace_id
		{
			get
			{
				return (Guid)base[tableSecurityGroupAccessEntry.namespace_idColumn];
			}
			set
			{
				base[tableSecurityGroupAccessEntry.namespace_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string action_name
		{
			get
			{
				return (string)base[tableSecurityGroupAccessEntry.action_nameColumn];
			}
			set
			{
				base[tableSecurityGroupAccessEntry.action_nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string user_sid
		{
			get
			{
				return (string)base[tableSecurityGroupAccessEntry.user_sidColumn];
			}
			set
			{
				base[tableSecurityGroupAccessEntry.user_sidColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool access_deny
		{
			get
			{
				return (bool)base[tableSecurityGroupAccessEntry.access_denyColumn];
			}
			set
			{
				base[tableSecurityGroupAccessEntry.access_denyColumn] = value;
			}
		}

		public string GetUserSid()
		{
			try
			{
				DataRowVersion version = ((base.RowState == DataRowState.Deleted) ? DataRowVersion.Original : DataRowVersion.Current);
				return (string)base[tableSecurityGroupAccessEntry.user_sidColumn, version];
			}
			catch (InvalidCastException innerException)
			{
				throw new StrongTypingException(string.Format(CultureInfo.InvariantCulture, "The value for column '{0}' in table '{1}' is DBNull.", tableSecurityGroupAccessEntry.user_sidColumn.ColumnName, tableSecurityGroupAccessEntry.TableName), innerException);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityGroupAccessEntryRow(DataRowBuilder rb)
			: base(rb)
		{
			tableSecurityGroupAccessEntry = (SecurityGroupAccessEntryDataTable)base.Table;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void SecurityAttributesRowChangeEventHandler(object sender, SecurityAttributesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void SecurityGroupAccessEntryRowChangeEventHandler(object sender, SecurityGroupAccessEntryRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void SecurityPermissionsDeviceRowChangeEventHandler(object sender, SecurityPermissionsDeviceRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void SecurityPermissionsMatrixRowChangeEventHandler(object sender, SecurityPermissionsMatrixRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void SecurityPermissionsRowChangeEventHandler(object sender, SecurityPermissionsRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void SecurityRolesRowChangeEventHandler(object sender, SecurityRolesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void TimeProfileDefaultRoleRelationRowChangeEventHandler(object sender, TimeProfileDefaultRoleRelationRowChangeEvent e);

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class SecurityAttributesDataTable : TypedTableBase<SecurityAttributesRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnSecurityAttributeID;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnParentSecurityAttributeName;

		private DataColumn columnSecurityType;

		private DataColumn columnHasObject;

		private DataColumn columnDisplayOrder;

		private DataColumn columnDefinition;

		private DataColumn columnTimeProfileBased;

		private DataColumn columnTimeBackwardBased;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SecurityAttributeIDColumn => columnSecurityAttributeID;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ParentSecurityAttributeNameColumn => columnParentSecurityAttributeName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SecurityTypeColumn => columnSecurityType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn HasObjectColumn => columnHasObject;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DisplayOrderColumn => columnDisplayOrder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DefinitionColumn => columnDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn TimeProfileBasedColumn => columnTimeProfileBased;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn TimeBackwardBasedColumn => columnTimeBackwardBased;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityAttributesRow this[int index] => (SecurityAttributesRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityAttributesRowChangeEventHandler SecurityAttributesRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityAttributesRowChangeEventHandler SecurityAttributesRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityAttributesRowChangeEventHandler SecurityAttributesRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityAttributesRowChangeEventHandler SecurityAttributesRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityAttributesDataTable()
		{
			base.TableName = "SecurityAttributes";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityAttributesDataTable(DataTable table)
		{
			base.TableName = table.TableName;
			if (table.CaseSensitive != table.DataSet.CaseSensitive)
			{
				base.CaseSensitive = table.CaseSensitive;
			}
			if (table.Locale.ToString() != table.DataSet.Locale.ToString())
			{
				base.Locale = table.Locale;
			}
			if (table.Namespace != table.DataSet.Namespace)
			{
				base.Namespace = table.Namespace;
			}
			base.Prefix = table.Prefix;
			base.MinimumCapacity = table.MinimumCapacity;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected SecurityAttributesDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddSecurityAttributesRow(SecurityAttributesRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityAttributesRow AddSecurityAttributesRow(DateTime LastModified, string SecurityAttributeID, string Name, string Description, string ParentSecurityAttributeName, string SecurityType, bool HasObject, int DisplayOrder, string Definition, bool TimeProfileBased, bool TimeBackwardBased)
		{
			SecurityAttributesRow securityAttributesRow = (SecurityAttributesRow)NewRow();
			object[] itemArray = new object[11]
			{
				LastModified, SecurityAttributeID, Name, Description, ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, Definition, TimeProfileBased,
				TimeBackwardBased
			};
			securityAttributesRow.ItemArray = itemArray;
			base.Rows.Add(securityAttributesRow);
			return securityAttributesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityAttributesRow FindBySecurityAttributeID(string SecurityAttributeID)
		{
			return (SecurityAttributesRow)base.Rows.Find(new object[1] { SecurityAttributeID });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			SecurityAttributesDataTable obj = (SecurityAttributesDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new SecurityAttributesDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnSecurityAttributeID = base.Columns["SecurityAttributeID"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnParentSecurityAttributeName = base.Columns["ParentSecurityAttributeName"];
			columnSecurityType = base.Columns["SecurityType"];
			columnHasObject = base.Columns["HasObject"];
			columnDisplayOrder = base.Columns["DisplayOrder"];
			columnDefinition = base.Columns["Definition"];
			columnTimeProfileBased = base.Columns["TimeProfileBased"];
			columnTimeBackwardBased = base.Columns["TimeBackwardBased"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnSecurityAttributeID = new DataColumn("SecurityAttributeID", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSecurityAttributeID);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnParentSecurityAttributeName = new DataColumn("ParentSecurityAttributeName", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnParentSecurityAttributeName);
			columnSecurityType = new DataColumn("SecurityType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSecurityType);
			columnHasObject = new DataColumn("HasObject", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnHasObject);
			columnDisplayOrder = new DataColumn("DisplayOrder", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnDisplayOrder);
			columnDefinition = new DataColumn("Definition", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDefinition);
			columnTimeProfileBased = new DataColumn("TimeProfileBased", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnTimeProfileBased);
			columnTimeBackwardBased = new DataColumn("TimeBackwardBased", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnTimeBackwardBased);
			base.Constraints.Add(new UniqueConstraint("PK_SecurityAttributes", new DataColumn[1] { columnSecurityAttributeID }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnSecurityAttributeID.AllowDBNull = false;
			columnSecurityAttributeID.Unique = true;
			columnSecurityAttributeID.MaxLength = 100;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
			columnParentSecurityAttributeName.AllowDBNull = false;
			columnParentSecurityAttributeName.MaxLength = 100;
			columnSecurityType.AllowDBNull = false;
			columnSecurityType.MaxLength = 100;
			columnHasObject.AllowDBNull = false;
			columnDisplayOrder.AllowDBNull = false;
			columnDefinition.MaxLength = 1073741823;
			columnTimeProfileBased.AllowDBNull = false;
			columnTimeBackwardBased.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityAttributesRow NewSecurityAttributesRow()
		{
			return (SecurityAttributesRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new SecurityAttributesRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(SecurityAttributesRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.SecurityAttributesRowChanged != null)
			{
				this.SecurityAttributesRowChanged(this, new SecurityAttributesRowChangeEvent((SecurityAttributesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.SecurityAttributesRowChanging != null)
			{
				this.SecurityAttributesRowChanging(this, new SecurityAttributesRowChangeEvent((SecurityAttributesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.SecurityAttributesRowDeleted != null)
			{
				this.SecurityAttributesRowDeleted(this, new SecurityAttributesRowChangeEvent((SecurityAttributesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.SecurityAttributesRowDeleting != null)
			{
				this.SecurityAttributesRowDeleting(this, new SecurityAttributesRowChangeEvent((SecurityAttributesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveSecurityAttributesRow(SecurityAttributesRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			SecurityDataSet securityDataSet = new SecurityDataSet();
			XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
			xmlSchemaAny.Namespace = "http://www.w3.org/2001/XMLSchema";
			xmlSchemaAny.MinOccurs = 0m;
			xmlSchemaAny.MaxOccurs = decimal.MaxValue;
			xmlSchemaAny.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny);
			XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
			xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
			xmlSchemaAny2.MinOccurs = 1m;
			xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny2);
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			xmlSchemaAttribute.Name = "namespace";
			xmlSchemaAttribute.FixedValue = securityDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "SecurityAttributesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = securityDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						XmlSchema obj = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						obj.Write(memoryStream2);
						if (memoryStream.Length == memoryStream2.Length)
						{
							memoryStream.Position = 0L;
							memoryStream2.Position = 0L;
							while (memoryStream.Position != memoryStream.Length && memoryStream.ReadByte() == memoryStream2.ReadByte())
							{
							}
							if (memoryStream.Position == memoryStream.Length)
							{
								return xmlSchemaComplexType;
							}
						}
					}
				}
				finally
				{
					memoryStream?.Close();
					memoryStream2?.Close();
				}
			}
			xs.Add(schemaSerializable);
			return xmlSchemaComplexType;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class SecurityGroupAccessEntryDataTable : TypedTableBase<SecurityGroupAccessEntryRow>
	{
		private DataColumn columnobject_id;

		private DataColumn columnnamespace_id;

		private DataColumn columnaction_name;

		private DataColumn columnuser_sid;

		private DataColumn columnaccess_deny;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn object_idColumn => columnobject_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn namespace_idColumn => columnnamespace_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn action_nameColumn => columnaction_name;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn user_sidColumn => columnuser_sid;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn access_denyColumn => columnaccess_deny;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityGroupAccessEntryRow this[int index] => (SecurityGroupAccessEntryRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityGroupAccessEntryRowChangeEventHandler SecurityGroupAccessEntryRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityGroupAccessEntryRowChangeEventHandler SecurityGroupAccessEntryRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityGroupAccessEntryRowChangeEventHandler SecurityGroupAccessEntryRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityGroupAccessEntryRowChangeEventHandler SecurityGroupAccessEntryRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityGroupAccessEntryDataTable()
		{
			base.TableName = "SecurityGroupAccessEntry";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityGroupAccessEntryDataTable(DataTable table)
		{
			base.TableName = table.TableName;
			if (table.CaseSensitive != table.DataSet.CaseSensitive)
			{
				base.CaseSensitive = table.CaseSensitive;
			}
			if (table.Locale.ToString() != table.DataSet.Locale.ToString())
			{
				base.Locale = table.Locale;
			}
			if (table.Namespace != table.DataSet.Namespace)
			{
				base.Namespace = table.Namespace;
			}
			base.Prefix = table.Prefix;
			base.MinimumCapacity = table.MinimumCapacity;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected SecurityGroupAccessEntryDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddSecurityGroupAccessEntryRow(SecurityGroupAccessEntryRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityGroupAccessEntryRow AddSecurityGroupAccessEntryRow(string object_id, Guid namespace_id, string action_name, string user_sid, bool access_deny)
		{
			SecurityGroupAccessEntryRow securityGroupAccessEntryRow = (SecurityGroupAccessEntryRow)NewRow();
			object[] itemArray = new object[5] { object_id, namespace_id, action_name, user_sid, access_deny };
			securityGroupAccessEntryRow.ItemArray = itemArray;
			base.Rows.Add(securityGroupAccessEntryRow);
			return securityGroupAccessEntryRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityGroupAccessEntryRow FindByobject_idnamespace_idaction_nameuser_sidaccess_deny(string object_id, Guid namespace_id, string action_name, string user_sid, bool access_deny)
		{
			return (SecurityGroupAccessEntryRow)base.Rows.Find(new object[5] { object_id, namespace_id, action_name, user_sid, access_deny });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			SecurityGroupAccessEntryDataTable obj = (SecurityGroupAccessEntryDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new SecurityGroupAccessEntryDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnobject_id = base.Columns["object_id"];
			columnnamespace_id = base.Columns["namespace_id"];
			columnaction_name = base.Columns["action_name"];
			columnuser_sid = base.Columns["user_sid"];
			columnaccess_deny = base.Columns["access_deny"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnobject_id = new DataColumn("object_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnobject_id);
			columnnamespace_id = new DataColumn("namespace_id", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnnamespace_id);
			columnaction_name = new DataColumn("action_name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnaction_name);
			columnuser_sid = new DataColumn("user_sid", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnuser_sid);
			columnaccess_deny = new DataColumn("access_deny", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnaccess_deny);
			base.Constraints.Add(new UniqueConstraint("PK_SecurityGroupAccessEntry", new DataColumn[5] { columnobject_id, columnnamespace_id, columnaction_name, columnuser_sid, columnaccess_deny }, isPrimaryKey: true));
			columnobject_id.AllowDBNull = false;
			columnobject_id.MaxLength = 300;
			columnnamespace_id.AllowDBNull = false;
			columnaction_name.AllowDBNull = false;
			columnaction_name.MaxLength = 260;
			columnuser_sid.AllowDBNull = false;
			columnuser_sid.MaxLength = 256;
			columnaccess_deny.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityGroupAccessEntryRow NewSecurityGroupAccessEntryRow()
		{
			return (SecurityGroupAccessEntryRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new SecurityGroupAccessEntryRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(SecurityGroupAccessEntryRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.SecurityGroupAccessEntryRowChanged != null)
			{
				this.SecurityGroupAccessEntryRowChanged(this, new SecurityGroupAccessEntryRowChangeEvent((SecurityGroupAccessEntryRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.SecurityGroupAccessEntryRowChanging != null)
			{
				this.SecurityGroupAccessEntryRowChanging(this, new SecurityGroupAccessEntryRowChangeEvent((SecurityGroupAccessEntryRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.SecurityGroupAccessEntryRowDeleted != null)
			{
				this.SecurityGroupAccessEntryRowDeleted(this, new SecurityGroupAccessEntryRowChangeEvent((SecurityGroupAccessEntryRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.SecurityGroupAccessEntryRowDeleting != null)
			{
				this.SecurityGroupAccessEntryRowDeleting(this, new SecurityGroupAccessEntryRowChangeEvent((SecurityGroupAccessEntryRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveSecurityGroupAccessEntryRow(SecurityGroupAccessEntryRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			SecurityDataSet securityDataSet = new SecurityDataSet();
			XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
			xmlSchemaAny.Namespace = "http://www.w3.org/2001/XMLSchema";
			xmlSchemaAny.MinOccurs = 0m;
			xmlSchemaAny.MaxOccurs = decimal.MaxValue;
			xmlSchemaAny.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny);
			XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
			xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
			xmlSchemaAny2.MinOccurs = 1m;
			xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny2);
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			xmlSchemaAttribute.Name = "namespace";
			xmlSchemaAttribute.FixedValue = securityDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "SecurityGroupAccessEntryDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = securityDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						XmlSchema obj = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						obj.Write(memoryStream2);
						if (memoryStream.Length == memoryStream2.Length)
						{
							memoryStream.Position = 0L;
							memoryStream2.Position = 0L;
							while (memoryStream.Position != memoryStream.Length && memoryStream.ReadByte() == memoryStream2.ReadByte())
							{
							}
							if (memoryStream.Position == memoryStream.Length)
							{
								return xmlSchemaComplexType;
							}
						}
					}
				}
				finally
				{
					memoryStream?.Close();
					memoryStream2?.Close();
				}
			}
			xs.Add(schemaSerializable);
			return xmlSchemaComplexType;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class SecurityRolesDataTable : TypedTableBase<SecurityRolesRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDRole;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnBuiltin;

		private DataColumn columnEnabled;

		private DataColumn columnType;

		private DataColumn columnDualRequired;

		private DataColumn columnPtzReservedAsAnonymous;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRoleColumn => columnIDRole;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn BuiltinColumn => columnBuiltin;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn TypeColumn => columnType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DualRequiredColumn => columnDualRequired;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PtzReservedAsAnonymousColumn => columnPtzReservedAsAnonymous;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityRolesRow this[int index] => (SecurityRolesRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityRolesRowChangeEventHandler SecurityRolesRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityRolesRowChangeEventHandler SecurityRolesRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityRolesRowChangeEventHandler SecurityRolesRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SecurityRolesRowChangeEventHandler SecurityRolesRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityRolesDataTable()
		{
			base.TableName = "SecurityRoles";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityRolesDataTable(DataTable table)
		{
			base.TableName = table.TableName;
			if (table.CaseSensitive != table.DataSet.CaseSensitive)
			{
				base.CaseSensitive = table.CaseSensitive;
			}
			if (table.Locale.ToString() != table.DataSet.Locale.ToString())
			{
				base.Locale = table.Locale;
			}
			if (table.Namespace != table.DataSet.Namespace)
			{
				base.Namespace = table.Namespace;
			}
			base.Prefix = table.Prefix;
			base.MinimumCapacity = table.MinimumCapacity;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected SecurityRolesDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddSecurityRolesRow(SecurityRolesRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityRolesRow AddSecurityRolesRow(DateTime LastModified, string IDRole, string Name, string Description, bool Builtin, bool Enabled, byte Type, bool DualRequired, bool PtzReservedAsAnonymous)
		{
			SecurityRolesRow securityRolesRow = (SecurityRolesRow)NewRow();
			object[] itemArray = new object[9] { LastModified, IDRole, Name, Description, Builtin, Enabled, Type, DualRequired, PtzReservedAsAnonymous };
			securityRolesRow.ItemArray = itemArray;
			base.Rows.Add(securityRolesRow);
			return securityRolesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityRolesRow FindByIDRole(string IDRole)
		{
			return (SecurityRolesRow)base.Rows.Find(new object[1] { IDRole });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			SecurityRolesDataTable obj = (SecurityRolesDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new SecurityRolesDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDRole = base.Columns["IDRole"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnBuiltin = base.Columns["Builtin"];
			columnEnabled = base.Columns["Enabled"];
			columnType = base.Columns["Type"];
			columnDualRequired = base.Columns["DualRequired"];
			columnPtzReservedAsAnonymous = base.Columns["PtzReservedAsAnonymous"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDRole = new DataColumn("IDRole", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRole);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnBuiltin = new DataColumn("Builtin", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnBuiltin);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			columnType = new DataColumn("Type", typeof(byte), null, MappingType.Element);
			base.Columns.Add(columnType);
			columnDualRequired = new DataColumn("DualRequired", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnDualRequired);
			columnPtzReservedAsAnonymous = new DataColumn("PtzReservedAsAnonymous", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnPtzReservedAsAnonymous);
			base.Constraints.Add(new UniqueConstraint("PK_SecurityRoles", new DataColumn[1] { columnIDRole }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDRole.AllowDBNull = false;
			columnIDRole.Unique = true;
			columnIDRole.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = int.MaxValue;
			columnBuiltin.AllowDBNull = false;
			columnEnabled.AllowDBNull = false;
			columnType.AllowDBNull = false;
			columnDualRequired.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityRolesRow NewSecurityRolesRow()
		{
			return (SecurityRolesRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new SecurityRolesRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(SecurityRolesRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.SecurityRolesRowChanged != null)
			{
				this.SecurityRolesRowChanged(this, new SecurityRolesRowChangeEvent((SecurityRolesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.SecurityRolesRowChanging != null)
			{
				this.SecurityRolesRowChanging(this, new SecurityRolesRowChangeEvent((SecurityRolesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.SecurityRolesRowDeleted != null)
			{
				this.SecurityRolesRowDeleted(this, new SecurityRolesRowChangeEvent((SecurityRolesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.SecurityRolesRowDeleting != null)
			{
				this.SecurityRolesRowDeleting(this, new SecurityRolesRowChangeEvent((SecurityRolesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveSecurityRolesRow(SecurityRolesRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			SecurityDataSet securityDataSet = new SecurityDataSet();
			XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
			xmlSchemaAny.Namespace = "http://www.w3.org/2001/XMLSchema";
			xmlSchemaAny.MinOccurs = 0m;
			xmlSchemaAny.MaxOccurs = decimal.MaxValue;
			xmlSchemaAny.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny);
			XmlSchemaAny xmlSchemaAny2 = new XmlSchemaAny();
			xmlSchemaAny2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
			xmlSchemaAny2.MinOccurs = 1m;
			xmlSchemaAny2.ProcessContents = XmlSchemaContentProcessing.Lax;
			xmlSchemaSequence.Items.Add(xmlSchemaAny2);
			XmlSchemaAttribute xmlSchemaAttribute = new XmlSchemaAttribute();
			xmlSchemaAttribute.Name = "namespace";
			xmlSchemaAttribute.FixedValue = securityDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "SecurityRolesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = securityDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						XmlSchema obj = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						obj.Write(memoryStream2);
						if (memoryStream.Length == memoryStream2.Length)
						{
							memoryStream.Position = 0L;
							memoryStream2.Position = 0L;
							while (memoryStream.Position != memoryStream.Length && memoryStream.ReadByte() == memoryStream2.ReadByte())
							{
							}
							if (memoryStream.Position == memoryStream.Length)
							{
								return xmlSchemaComplexType;
							}
						}
					}
				}
				finally
				{
					memoryStream?.Close();
					memoryStream2?.Close();
				}
			}
			xs.Add(schemaSerializable);
			return xmlSchemaComplexType;
		}
	}

	public class SecurityAttributesRow : DataRow
	{
		private SecurityAttributesDataTable tableSecurityAttributes;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableSecurityAttributes.LastModifiedColumn];
			}
			set
			{
				base[tableSecurityAttributes.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string SecurityAttributeID
		{
			get
			{
				return (string)base[tableSecurityAttributes.SecurityAttributeIDColumn];
			}
			set
			{
				base[tableSecurityAttributes.SecurityAttributeIDColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableSecurityAttributes.NameColumn];
			}
			set
			{
				base[tableSecurityAttributes.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableSecurityAttributes.DescriptionColumn];
			}
			set
			{
				base[tableSecurityAttributes.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string ParentSecurityAttributeName
		{
			get
			{
				return (string)base[tableSecurityAttributes.ParentSecurityAttributeNameColumn];
			}
			set
			{
				base[tableSecurityAttributes.ParentSecurityAttributeNameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string SecurityType
		{
			get
			{
				return (string)base[tableSecurityAttributes.SecurityTypeColumn];
			}
			set
			{
				base[tableSecurityAttributes.SecurityTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool HasObject
		{
			get
			{
				return (bool)base[tableSecurityAttributes.HasObjectColumn];
			}
			set
			{
				base[tableSecurityAttributes.HasObjectColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int DisplayOrder
		{
			get
			{
				return (int)base[tableSecurityAttributes.DisplayOrderColumn];
			}
			set
			{
				base[tableSecurityAttributes.DisplayOrderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Definition
		{
			get
			{
				try
				{
					return (string)base[tableSecurityAttributes.DefinitionColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Definition' in table 'SecurityAttributes' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableSecurityAttributes.DefinitionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool TimeProfileBased
		{
			get
			{
				return (bool)base[tableSecurityAttributes.TimeProfileBasedColumn];
			}
			set
			{
				base[tableSecurityAttributes.TimeProfileBasedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool TimeBackwardBased
		{
			get
			{
				return (bool)base[tableSecurityAttributes.TimeBackwardBasedColumn];
			}
			set
			{
				base[tableSecurityAttributes.TimeBackwardBasedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityAttributesRow(DataRowBuilder rb)
			: base(rb)
		{
			tableSecurityAttributes = (SecurityAttributesDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsDefinitionNull()
		{
			return IsNull(tableSecurityAttributes.DefinitionColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetDefinitionNull()
		{
			base[tableSecurityAttributes.DefinitionColumn] = Convert.DBNull;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class SecurityAttributesRowChangeEvent : EventArgs
	{
		private SecurityAttributesRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityAttributesRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityAttributesRowChangeEvent(SecurityAttributesRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class SecurityGroupAccessEntryRowChangeEvent : EventArgs
	{
		private SecurityGroupAccessEntryRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityGroupAccessEntryRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityGroupAccessEntryRowChangeEvent(SecurityGroupAccessEntryRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class SecurityPermissionsDeviceRowChangeEvent : EventArgs
	{
		private SecurityPermissionsDeviceRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDeviceRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsDeviceRowChangeEvent(SecurityPermissionsDeviceRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class SecurityPermissionsMatrixRowChangeEvent : EventArgs
	{
		private SecurityPermissionsMatrixRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsMatrixRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsMatrixRowChangeEvent(SecurityPermissionsMatrixRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class SecurityPermissionsRowChangeEvent : EventArgs
	{
		private SecurityPermissionsRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityPermissionsRowChangeEvent(SecurityPermissionsRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class SecurityRolesRowChangeEvent : EventArgs
	{
		private SecurityRolesRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityRolesRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SecurityRolesRowChangeEvent(SecurityRolesRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class TimeProfileDefaultRoleRelationRowChangeEvent : EventArgs
	{
		private TimeProfileDefaultRoleRelationRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDefaultRoleRelationRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDefaultRoleRelationRowChangeEvent(TimeProfileDefaultRoleRelationRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	private object _dataChangedEventLock = new object();

	private SecurityAttributesDataTable tableSecurityAttributes;

	private SecurityGroupAccessEntryDataTable tableSecurityGroupAccessEntry;

	private SecurityPermissionsDeviceDataTable tableSecurityPermissionsDevice;

	private SecurityPermissionsMatrixDataTable tableSecurityPermissionsMatrix;

	private SecurityPermissionsDataTable tableSecurityPermissions;

	private SecurityRolesDataTable tableSecurityRoles;

	private TimeProfileDefaultRoleRelationDataTable tableTimeProfileDefaultRoleRelation;

	private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SecurityAttributesDataTable SecurityAttributes => tableSecurityAttributes;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SecurityGroupAccessEntryDataTable SecurityGroupAccessEntry => tableSecurityGroupAccessEntry;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SecurityPermissionsDeviceDataTable SecurityPermissionsDevice => tableSecurityPermissionsDevice;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SecurityPermissionsMatrixDataTable SecurityPermissionsMatrix => tableSecurityPermissionsMatrix;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SecurityPermissionsDataTable SecurityPermissions => tableSecurityPermissions;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SecurityRolesDataTable SecurityRoles => tableSecurityRoles;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public TimeProfileDefaultRoleRelationDataTable TimeProfileDefaultRoleRelation => tableTimeProfileDefaultRoleRelation;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override SchemaSerializationMode SchemaSerializationMode
	{
		get
		{
			return _schemaSerializationMode;
		}
		set
		{
			_schemaSerializationMode = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public new DataTableCollection Tables => base.Tables;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public new DataRelationCollection Relations => base.Relations;

	private event EventHandler<DataRowChangeEventArgs> _dataChanged;

	public event EventHandler<DataRowChangeEventArgs> DataChanged
	{
		add
		{
			lock (_dataChangedEventLock)
			{
				if (this._dataChanged == null)
				{
					foreach (DataTable table in Tables)
					{
						table.RowChanged += OnDataChanged;
						table.RowDeleted += OnDataChanged;
					}
				}
				_dataChanged += value;
			}
		}
		remove
		{
			lock (_dataChangedEventLock)
			{
				if (this._dataChanged == null)
				{
					return;
				}
				_dataChanged -= value;
				if (this._dataChanged != null)
				{
					return;
				}
				foreach (DataTable table in Tables)
				{
					table.RowChanged -= OnDataChanged;
					table.RowDeleted -= OnDataChanged;
				}
			}
		}
	}

	public bool HasData()
	{
		foreach (DataTable table in Tables)
		{
			if (table.Rows.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	public new SecurityDataSet GetChanges()
	{
		return (SecurityDataSet)base.GetChanges();
	}

	public new SecurityDataSet GetChanges(DataRowState rowStates)
	{
		return (SecurityDataSet)base.GetChanges(rowStates);
	}

	protected virtual void OnDataChanged(object sender, DataRowChangeEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		if (e.Row.RowState != DataRowState.Unchanged)
		{
			EventHandler<DataRowChangeEventArgs> eventHandler;
			lock (_dataChangedEventLock)
			{
				eventHandler = this._dataChanged;
			}
			eventHandler?.Invoke(sender, e);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public SecurityDataSet()
	{
		BeginInit();
		InitClass();
		CollectionChangeEventHandler value = SchemaChanged;
		base.Tables.CollectionChanged += value;
		base.Relations.CollectionChanged += value;
		EndInit();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected SecurityDataSet(SerializationInfo info, StreamingContext context)
		: base(info, context, ConstructSchema: false)
	{
		if (IsBinarySerialized(info, context))
		{
			InitVars(initTable: false);
			CollectionChangeEventHandler value = SchemaChanged;
			Tables.CollectionChanged += value;
			Relations.CollectionChanged += value;
			return;
		}
		string s = (string)info.GetValue("XmlSchema", typeof(string));
		if (DetermineSchemaSerializationMode(info, context) == SchemaSerializationMode.IncludeSchema)
		{
			DataSet dataSet = new DataSet();
			dataSet.ReadXmlSchema(new XmlTextReader(new StringReader(s)));
			if (dataSet.Tables["SecurityAttributes"] != null)
			{
				base.Tables.Add(new SecurityAttributesDataTable(dataSet.Tables["SecurityAttributes"]));
			}
			if (dataSet.Tables["SecurityGroupAccessEntry"] != null)
			{
				base.Tables.Add(new SecurityGroupAccessEntryDataTable(dataSet.Tables["SecurityGroupAccessEntry"]));
			}
			if (dataSet.Tables["SecurityPermissionsDevice"] != null)
			{
				base.Tables.Add(new SecurityPermissionsDeviceDataTable(dataSet.Tables["SecurityPermissionsDevice"]));
			}
			if (dataSet.Tables["SecurityPermissionsMatrix"] != null)
			{
				base.Tables.Add(new SecurityPermissionsMatrixDataTable(dataSet.Tables["SecurityPermissionsMatrix"]));
			}
			if (dataSet.Tables["SecurityPermissions"] != null)
			{
				base.Tables.Add(new SecurityPermissionsDataTable(dataSet.Tables["SecurityPermissions"]));
			}
			if (dataSet.Tables["SecurityRoles"] != null)
			{
				base.Tables.Add(new SecurityRolesDataTable(dataSet.Tables["SecurityRoles"]));
			}
			if (dataSet.Tables["TimeProfileDefaultRoleRelation"] != null)
			{
				base.Tables.Add(new TimeProfileDefaultRoleRelationDataTable(dataSet.Tables["TimeProfileDefaultRoleRelation"]));
			}
			base.DataSetName = dataSet.DataSetName;
			base.Prefix = dataSet.Prefix;
			base.Namespace = dataSet.Namespace;
			base.Locale = dataSet.Locale;
			base.CaseSensitive = dataSet.CaseSensitive;
			base.EnforceConstraints = dataSet.EnforceConstraints;
			Merge(dataSet, preserveChanges: false, MissingSchemaAction.Add);
			InitVars();
		}
		else
		{
			ReadXmlSchema(new XmlTextReader(new StringReader(s)));
		}
		GetSerializationData(info, context);
		CollectionChangeEventHandler value2 = SchemaChanged;
		base.Tables.CollectionChanged += value2;
		Relations.CollectionChanged += value2;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected override void InitializeDerivedDataSet()
	{
		BeginInit();
		InitClass();
		EndInit();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public override DataSet Clone()
	{
		SecurityDataSet obj = (SecurityDataSet)base.Clone();
		obj.InitVars();
		obj.SchemaSerializationMode = SchemaSerializationMode;
		return obj;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected override bool ShouldSerializeTables()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected override bool ShouldSerializeRelations()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected override void ReadXmlSerializable(XmlReader reader)
	{
		if (DetermineSchemaSerializationMode(reader) == SchemaSerializationMode.IncludeSchema)
		{
			Reset();
			DataSet dataSet = new DataSet();
			dataSet.ReadXml(reader);
			if (dataSet.Tables["SecurityAttributes"] != null)
			{
				base.Tables.Add(new SecurityAttributesDataTable(dataSet.Tables["SecurityAttributes"]));
			}
			if (dataSet.Tables["SecurityGroupAccessEntry"] != null)
			{
				base.Tables.Add(new SecurityGroupAccessEntryDataTable(dataSet.Tables["SecurityGroupAccessEntry"]));
			}
			if (dataSet.Tables["SecurityPermissionsDevice"] != null)
			{
				base.Tables.Add(new SecurityPermissionsDeviceDataTable(dataSet.Tables["SecurityPermissionsDevice"]));
			}
			if (dataSet.Tables["SecurityPermissionsMatrix"] != null)
			{
				base.Tables.Add(new SecurityPermissionsMatrixDataTable(dataSet.Tables["SecurityPermissionsMatrix"]));
			}
			if (dataSet.Tables["SecurityPermissions"] != null)
			{
				base.Tables.Add(new SecurityPermissionsDataTable(dataSet.Tables["SecurityPermissions"]));
			}
			if (dataSet.Tables["SecurityRoles"] != null)
			{
				base.Tables.Add(new SecurityRolesDataTable(dataSet.Tables["SecurityRoles"]));
			}
			if (dataSet.Tables["TimeProfileDefaultRoleRelation"] != null)
			{
				base.Tables.Add(new TimeProfileDefaultRoleRelationDataTable(dataSet.Tables["TimeProfileDefaultRoleRelation"]));
			}
			base.DataSetName = dataSet.DataSetName;
			base.Prefix = dataSet.Prefix;
			base.Namespace = dataSet.Namespace;
			base.Locale = dataSet.Locale;
			base.CaseSensitive = dataSet.CaseSensitive;
			base.EnforceConstraints = dataSet.EnforceConstraints;
			Merge(dataSet, preserveChanges: false, MissingSchemaAction.Add);
			InitVars();
		}
		else
		{
			ReadXml(reader);
			InitVars();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected override XmlSchema GetSchemaSerializable()
	{
		MemoryStream memoryStream = new MemoryStream();
		WriteXmlSchema(new XmlTextWriter(memoryStream, null));
		memoryStream.Position = 0L;
		return XmlSchema.Read(new XmlTextReader(memoryStream), null);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public void InitVars()
	{
		InitVars(initTable: true);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public void InitVars(bool initTable)
	{
		tableSecurityAttributes = (SecurityAttributesDataTable)base.Tables["SecurityAttributes"];
		if (initTable && tableSecurityAttributes != null)
		{
			tableSecurityAttributes.InitVars();
		}
		tableSecurityGroupAccessEntry = (SecurityGroupAccessEntryDataTable)base.Tables["SecurityGroupAccessEntry"];
		if (initTable && tableSecurityGroupAccessEntry != null)
		{
			tableSecurityGroupAccessEntry.InitVars();
		}
		tableSecurityPermissionsDevice = (SecurityPermissionsDeviceDataTable)base.Tables["SecurityPermissionsDevice"];
		if (initTable && tableSecurityPermissionsDevice != null)
		{
			tableSecurityPermissionsDevice.InitVars();
		}
		tableSecurityPermissionsMatrix = (SecurityPermissionsMatrixDataTable)base.Tables["SecurityPermissionsMatrix"];
		if (initTable && tableSecurityPermissionsMatrix != null)
		{
			tableSecurityPermissionsMatrix.InitVars();
		}
		tableSecurityPermissions = (SecurityPermissionsDataTable)base.Tables["SecurityPermissions"];
		if (initTable && tableSecurityPermissions != null)
		{
			tableSecurityPermissions.InitVars();
		}
		tableSecurityRoles = (SecurityRolesDataTable)base.Tables["SecurityRoles"];
		if (initTable && tableSecurityRoles != null)
		{
			tableSecurityRoles.InitVars();
		}
		tableTimeProfileDefaultRoleRelation = (TimeProfileDefaultRoleRelationDataTable)base.Tables["TimeProfileDefaultRoleRelation"];
		if (initTable && tableTimeProfileDefaultRoleRelation != null)
		{
			tableTimeProfileDefaultRoleRelation.InitVars();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private void InitClass()
	{
		base.DataSetName = "SecurityDataSet";
		base.Prefix = "";
		base.Namespace = "http://www.VideoOS.net/4/SecurityDataset.xsd";
		base.Locale = new CultureInfo("da-DK");
		base.EnforceConstraints = false;
		SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
		tableSecurityAttributes = new SecurityAttributesDataTable();
		base.Tables.Add(tableSecurityAttributes);
		tableSecurityGroupAccessEntry = new SecurityGroupAccessEntryDataTable();
		base.Tables.Add(tableSecurityGroupAccessEntry);
		tableSecurityPermissionsDevice = new SecurityPermissionsDeviceDataTable();
		base.Tables.Add(tableSecurityPermissionsDevice);
		tableSecurityPermissionsMatrix = new SecurityPermissionsMatrixDataTable();
		base.Tables.Add(tableSecurityPermissionsMatrix);
		tableSecurityPermissions = new SecurityPermissionsDataTable();
		base.Tables.Add(tableSecurityPermissions);
		tableSecurityRoles = new SecurityRolesDataTable();
		base.Tables.Add(tableSecurityRoles);
		tableTimeProfileDefaultRoleRelation = new TimeProfileDefaultRoleRelationDataTable();
		base.Tables.Add(tableTimeProfileDefaultRoleRelation);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeSecurityAttributes()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeSecurityGroupAccessEntry()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeSecurityPermissionsDevice()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeSecurityPermissionsMatrix()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeSecurityPermissions()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeSecurityRoles()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeTimeProfileDefaultRoleRelation()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private void SchemaChanged(object sender, CollectionChangeEventArgs e)
	{
		if (e.Action == CollectionChangeAction.Remove)
		{
			InitVars();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public static XmlSchemaComplexType GetTypedDataSetSchema(XmlSchemaSet xs)
	{
		SecurityDataSet securityDataSet = new SecurityDataSet();
		XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
		XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
		XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
		xmlSchemaAny.Namespace = securityDataSet.Namespace;
		xmlSchemaSequence.Items.Add(xmlSchemaAny);
		xmlSchemaComplexType.Particle = xmlSchemaSequence;
		XmlSchema schemaSerializable = securityDataSet.GetSchemaSerializable();
		if (xs.Contains(schemaSerializable.TargetNamespace))
		{
			MemoryStream memoryStream = new MemoryStream();
			MemoryStream memoryStream2 = new MemoryStream();
			try
			{
				schemaSerializable.Write(memoryStream);
				IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
				while (enumerator.MoveNext())
				{
					XmlSchema obj = (XmlSchema)enumerator.Current;
					memoryStream2.SetLength(0L);
					obj.Write(memoryStream2);
					if (memoryStream.Length == memoryStream2.Length)
					{
						memoryStream.Position = 0L;
						memoryStream2.Position = 0L;
						while (memoryStream.Position != memoryStream.Length && memoryStream.ReadByte() == memoryStream2.ReadByte())
						{
						}
						if (memoryStream.Position == memoryStream.Length)
						{
							return xmlSchemaComplexType;
						}
					}
				}
			}
			finally
			{
				memoryStream?.Close();
				memoryStream2?.Close();
			}
		}
		xs.Add(schemaSerializable);
		return xmlSchemaComplexType;
	}
}
