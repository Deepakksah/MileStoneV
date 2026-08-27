using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Configuration;

[Serializable]
[DesignerCategory("code")]
[ToolboxItem(true)]
[XmlSchemaProvider("GetTypedDataSetSchema")]
[XmlRoot("ConfigurationDataSet")]
[HelpKeyword("vs.data.DataSet")]
public class ConfigurationDataSet : DataSet
{
	public class ServerGroupRow : DataRow
	{
		private ServerGroupDataTable tableServerGroup;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDServerGroup
		{
			get
			{
				return (string)base[tableServerGroup.IDServerGroupColumn];
			}
			set
			{
				base[tableServerGroup.IDServerGroupColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableServerGroup.NameColumn];
			}
			set
			{
				base[tableServerGroup.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				try
				{
					return (string)base[tableServerGroup.DescriptionColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Description' in table 'ServerGroup' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableServerGroup.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDParentGroup
		{
			get
			{
				return (string)base[tableServerGroup.IDParentGroupColumn];
			}
			set
			{
				base[tableServerGroup.IDParentGroupColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string StructureType
		{
			get
			{
				return (string)base[tableServerGroup.StructureTypeColumn];
			}
			set
			{
				base[tableServerGroup.StructureTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int OrderNo
		{
			get
			{
				return (int)base[tableServerGroup.OrderNoColumn];
			}
			set
			{
				base[tableServerGroup.OrderNoColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int Level
		{
			get
			{
				return (int)base[tableServerGroup.LevelColumn];
			}
			set
			{
				base[tableServerGroup.LevelColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Path
		{
			get
			{
				return (string)base[tableServerGroup.PathColumn];
			}
			set
			{
				base[tableServerGroup.PathColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime CreatedDate
		{
			get
			{
				return (DateTime)base[tableServerGroup.CreatedDateColumn];
			}
			set
			{
				base[tableServerGroup.CreatedDateColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableServerGroup.LastModifiedColumn];
			}
			set
			{
				base[tableServerGroup.LastModifiedColumn] = value;
			}
		}

		public string GetIDServerGroup(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableServerGroup.IDServerGroupColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupRow(DataRowBuilder rb)
			: base(rb)
		{
			tableServerGroup = (ServerGroupDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsDescriptionNull()
		{
			return IsNull(tableServerGroup.DescriptionColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetDescriptionNull()
		{
			base[tableServerGroup.DescriptionColumn] = Convert.DBNull;
		}
	}

	public class ServerGroupMembershipRow : DataRow
	{
		private ServerGroupMembershipDataTable tableServerGroupMembership;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDServerGroup
		{
			get
			{
				return (string)base[tableServerGroupMembership.IDServerGroupColumn];
			}
			set
			{
				base[tableServerGroupMembership.IDServerGroupColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDObject
		{
			get
			{
				return (string)base[tableServerGroupMembership.IDObjectColumn];
			}
			set
			{
				base[tableServerGroupMembership.IDObjectColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string ObjectType
		{
			get
			{
				return (string)base[tableServerGroupMembership.ObjectTypeColumn];
			}
			set
			{
				base[tableServerGroupMembership.ObjectTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableServerGroupMembership.LastModifiedColumn];
			}
			set
			{
				base[tableServerGroupMembership.LastModifiedColumn] = value;
			}
		}

		public string GetIDServerGroup(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableServerGroupMembership.IDServerGroupColumn, version);
		}

		public string GetIDObject(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableServerGroupMembership.IDObjectColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupMembershipRow(DataRowBuilder rb)
			: base(rb)
		{
			tableServerGroupMembership = (ServerGroupMembershipDataTable)base.Table;
		}
	}

	public class EventTypesRow : DataRow
	{
		private EventTypesDataTable tableEventTypes;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableEventTypes.LastModifiedColumn];
			}
			set
			{
				base[tableEventTypes.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDEventType
		{
			get
			{
				return (string)base[tableEventTypes.IDEventTypeColumn];
			}
			set
			{
				base[tableEventTypes.IDEventTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Definition
		{
			get
			{
				return (string)base[tableEventTypes.DefinitionColumn];
			}
			set
			{
				base[tableEventTypes.DefinitionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string GeneratorType
		{
			get
			{
				return (string)base[tableEventTypes.GeneratorTypeColumn];
			}
			set
			{
				base[tableEventTypes.GeneratorTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDGenerator
		{
			get
			{
				try
				{
					return (string)base[tableEventTypes.IDGeneratorColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'IDGenerator' in table 'EventTypes' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableEventTypes.IDGeneratorColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Builtin
		{
			get
			{
				return (bool)base[tableEventTypes.BuiltinColumn];
			}
			set
			{
				base[tableEventTypes.BuiltinColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableEventTypes.NameColumn];
			}
			set
			{
				base[tableEventTypes.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string GeneratorSubType
		{
			get
			{
				return (string)base[tableEventTypes.GeneratorSubTypeColumn];
			}
			set
			{
				base[tableEventTypes.GeneratorSubTypeColumn] = value;
			}
		}

		public string GetIDEventType(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableEventTypes.IDEventTypeColumn, version);
		}

		public string GetGeneratorType(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableEventTypes.GeneratorTypeColumn, version);
		}

		public string GetGeneratorSubType(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableEventTypes.GeneratorSubTypeColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public EventTypesRow(DataRowBuilder rb)
			: base(rb)
		{
			tableEventTypes = (EventTypesDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsIDGeneratorNull()
		{
			return IsNull(tableEventTypes.IDGeneratorColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetIDGeneratorNull()
		{
			base[tableEventTypes.IDGeneratorColumn] = Convert.DBNull;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class DeviceDataDistributionDataTable : TypedTableBase<DeviceDataDistributionRow>
	{
		private DataColumn columndevice_id;

		private DataColumn columnrecorder_id;

		private DataColumn columntrack_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn device_idColumn => columndevice_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recorder_idColumn => columnrecorder_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn track_idColumn => columntrack_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceDataDistributionRow this[int index] => (DeviceDataDistributionRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceDataDistributionRowChangeEventHandler DeviceDataDistributionRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceDataDistributionRowChangeEventHandler DeviceDataDistributionRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceDataDistributionRowChangeEventHandler DeviceDataDistributionRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceDataDistributionRowChangeEventHandler DeviceDataDistributionRowDeleted;

		public DeviceDataDistributionRow[] FindByTrack(Guid trackId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columntrack_id.ColumnName, trackId);
			return (DeviceDataDistributionRow[])Select(filterExpression);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceDataDistributionDataTable()
		{
			base.TableName = "DeviceDataDistribution";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceDataDistributionDataTable(DataTable table)
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
		protected DeviceDataDistributionDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDeviceDataDistributionRow(DeviceDataDistributionRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceDataDistributionRow AddDeviceDataDistributionRow(string device_id, string recorder_id, Guid track_id)
		{
			DeviceDataDistributionRow deviceDataDistributionRow = (DeviceDataDistributionRow)NewRow();
			object[] itemArray = new object[3] { device_id, recorder_id, track_id };
			deviceDataDistributionRow.ItemArray = itemArray;
			base.Rows.Add(deviceDataDistributionRow);
			return deviceDataDistributionRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceDataDistributionRow FindByrecorder_idtrack_id(string recorder_id, Guid track_id)
		{
			return (DeviceDataDistributionRow)base.Rows.Find(new object[2] { recorder_id, track_id });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DeviceDataDistributionDataTable obj = (DeviceDataDistributionDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DeviceDataDistributionDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columndevice_id = base.Columns["device_id"];
			columnrecorder_id = base.Columns["recorder_id"];
			columntrack_id = base.Columns["track_id"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columndevice_id = new DataColumn("device_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columndevice_id);
			columnrecorder_id = new DataColumn("recorder_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnrecorder_id);
			columntrack_id = new DataColumn("track_id", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columntrack_id);
			base.Constraints.Add(new UniqueConstraint("PK_DeviceDataDistribution", new DataColumn[2] { columnrecorder_id, columntrack_id }, isPrimaryKey: true));
			columndevice_id.AllowDBNull = false;
			columndevice_id.MaxLength = 36;
			columnrecorder_id.AllowDBNull = false;
			columnrecorder_id.MaxLength = 36;
			columntrack_id.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceDataDistributionRow NewDeviceDataDistributionRow()
		{
			return (DeviceDataDistributionRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DeviceDataDistributionRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DeviceDataDistributionRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DeviceDataDistributionRowChanged != null)
			{
				this.DeviceDataDistributionRowChanged(this, new DeviceDataDistributionRowChangeEvent((DeviceDataDistributionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DeviceDataDistributionRowChanging != null)
			{
				this.DeviceDataDistributionRowChanging(this, new DeviceDataDistributionRowChangeEvent((DeviceDataDistributionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DeviceDataDistributionRowDeleted != null)
			{
				this.DeviceDataDistributionRowDeleted(this, new DeviceDataDistributionRowChangeEvent((DeviceDataDistributionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DeviceDataDistributionRowDeleting != null)
			{
				this.DeviceDataDistributionRowDeleting(this, new DeviceDataDistributionRowChangeEvent((DeviceDataDistributionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDeviceDataDistributionRow(DeviceDataDistributionRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DeviceDataDistributionDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class DeviceDataDistributionRow : DataRow
	{
		private DeviceDataDistributionDataTable tableDeviceDataDistribution;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string device_id
		{
			get
			{
				return (string)base[tableDeviceDataDistribution.device_idColumn];
			}
			set
			{
				base[tableDeviceDataDistribution.device_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string recorder_id
		{
			get
			{
				return (string)base[tableDeviceDataDistribution.recorder_idColumn];
			}
			set
			{
				base[tableDeviceDataDistribution.recorder_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid track_id
		{
			get
			{
				return (Guid)base[tableDeviceDataDistribution.track_idColumn];
			}
			set
			{
				base[tableDeviceDataDistribution.track_idColumn] = value;
			}
		}

		public string GetDeviceId()
		{
			return this.GetColumnValue<string>(tableDeviceDataDistribution.device_idColumn);
		}

		public string GetDeviceId(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDeviceDataDistribution.device_idColumn, version);
		}

		public string GetRecorderId()
		{
			return this.GetColumnValue<string>(tableDeviceDataDistribution.recorder_idColumn);
		}

		public string GetRecorderId(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDeviceDataDistribution.recorder_idColumn, version);
		}

		public Guid GetTrackId()
		{
			return this.GetColumnValue<Guid>(tableDeviceDataDistribution.track_idColumn);
		}

		public Guid GetTrackId(DataRowVersion version)
		{
			return this.GetColumnValue<Guid>(tableDeviceDataDistribution.track_idColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceDataDistributionRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDeviceDataDistribution = (DeviceDataDistributionDataTable)base.Table;
		}
	}

	public class DeviceTrackEvidenceLockRow : DataRow
	{
		private DeviceTrackEvidenceLockDataTable tableDeviceTrackEvidenceLock;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDMarkedData
		{
			get
			{
				return (Guid)base[tableDeviceTrackEvidenceLock.IDMarkedDataColumn];
			}
			set
			{
				base[tableDeviceTrackEvidenceLock.IDMarkedDataColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDevice
		{
			get
			{
				return (string)base[tableDeviceTrackEvidenceLock.IDDeviceColumn];
			}
			set
			{
				base[tableDeviceTrackEvidenceLock.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDTrack
		{
			get
			{
				return (Guid)base[tableDeviceTrackEvidenceLock.IDTrackColumn];
			}
			set
			{
				base[tableDeviceTrackEvidenceLock.IDTrackColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public long StartTime
		{
			get
			{
				return (long)base[tableDeviceTrackEvidenceLock.StartTimeColumn];
			}
			set
			{
				base[tableDeviceTrackEvidenceLock.StartTimeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public long EndTime
		{
			get
			{
				return (long)base[tableDeviceTrackEvidenceLock.EndTimeColumn];
			}
			set
			{
				base[tableDeviceTrackEvidenceLock.EndTimeColumn] = value;
			}
		}

		public Guid GetIDMarkedData()
		{
			return this.GetColumnValue<Guid>(tableDeviceTrackEvidenceLock.IDMarkedDataColumn);
		}

		public string GetIDDevice()
		{
			return this.GetColumnValue<string>(tableDeviceTrackEvidenceLock.IDDeviceColumn);
		}

		public string GetIDTrack()
		{
			return this.GetColumnValue<Guid>(tableDeviceTrackEvidenceLock.IDTrackColumn).ToString();
		}

		public DateTime GetStartTime()
		{
			return new DateTime(this.GetColumnValue<long>(tableDeviceTrackEvidenceLock.StartTimeColumn), DateTimeKind.Utc);
		}

		public DateTime GetEndTime()
		{
			return new DateTime(this.GetColumnValue<long>(tableDeviceTrackEvidenceLock.EndTimeColumn), DateTimeKind.Utc);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackEvidenceLockRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDeviceTrackEvidenceLock = (DeviceTrackEvidenceLockDataTable)base.Table;
		}
	}

	public class DeviceRestrictedMediaRow : DataRow
	{
		private DeviceRestrictedMediaDataTable tableDeviceRestrictedMedia;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDRestrictedMedia
		{
			get
			{
				return (Guid)base[tableDeviceRestrictedMedia.IDRestrictedMediaColumn];
			}
			set
			{
				base[tableDeviceRestrictedMedia.IDRestrictedMediaColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDevice
		{
			get
			{
				return (string)base[tableDeviceRestrictedMedia.IDDeviceColumn];
			}
			set
			{
				base[tableDeviceRestrictedMedia.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRecorder
		{
			get
			{
				return (string)base[tableDeviceRestrictedMedia.IDRecorderColumn];
			}
			set
			{
				base[tableDeviceRestrictedMedia.IDRecorderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime StartTime
		{
			get
			{
				return (DateTime)base[tableDeviceRestrictedMedia.StartTimeColumn];
			}
			set
			{
				base[tableDeviceRestrictedMedia.StartTimeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime EndTime
		{
			get
			{
				try
				{
					return (DateTime)base[tableDeviceRestrictedMedia.EndTimeColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'EndTime' in table 'DeviceRestrictedMedia' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableDeviceRestrictedMedia.EndTimeColumn] = value;
			}
		}

		public Guid GetIDRestrictedMedia()
		{
			return this.GetColumnValue<Guid>(tableDeviceRestrictedMedia.IDRestrictedMediaColumn);
		}

		public string GetIDDevice()
		{
			return this.GetColumnValue<string>(tableDeviceRestrictedMedia.IDDeviceColumn);
		}

		public string GetIDRecorder()
		{
			return this.GetColumnValue<Guid>(tableDeviceRestrictedMedia.IDRecorderColumn).ToString();
		}

		public DateTime GetStartTime()
		{
			return DateTime.SpecifyKind(this.GetColumnValue<DateTime>(tableDeviceRestrictedMedia.StartTimeColumn), DateTimeKind.Utc);
		}

		public DateTime? GetEndTime()
		{
			DateTime? columnValue = this.GetColumnValue<DateTime?>(tableDeviceRestrictedMedia.EndTimeColumn);
			if (columnValue.HasValue)
			{
				return DateTime.SpecifyKind(columnValue.Value, DateTimeKind.Utc);
			}
			return null;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceRestrictedMediaRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDeviceRestrictedMedia = (DeviceRestrictedMediaDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsEndTimeNull()
		{
			return IsNull(tableDeviceRestrictedMedia.EndTimeColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetEndTimeNull()
		{
			base[tableDeviceRestrictedMedia.EndTimeColumn] = Convert.DBNull;
		}
	}

	public class DeviceTrackInfoRow : DataRow
	{
		private DeviceTrackInfoDataTable tableDeviceTrackInfo;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDRecorder
		{
			get
			{
				return (Guid)base[tableDeviceTrackInfo.IDRecorderColumn];
			}
			set
			{
				base[tableDeviceTrackInfo.IDRecorderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDDevice
		{
			get
			{
				try
				{
					return (Guid)base[tableDeviceTrackInfo.IDDeviceColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'IDDevice' in table 'DeviceTrackInfo' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableDeviceTrackInfo.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDTrack
		{
			get
			{
				return (Guid)base[tableDeviceTrackInfo.IDTrackColumn];
			}
			set
			{
				base[tableDeviceTrackInfo.IDTrackColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableDeviceTrackInfo.NameColumn];
			}
			set
			{
				base[tableDeviceTrackInfo.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string DeviceType
		{
			get
			{
				return (string)base[tableDeviceTrackInfo.DeviceTypeColumn];
			}
			set
			{
				base[tableDeviceTrackInfo.DeviceTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDTrackDefinition
		{
			get
			{
				return (Guid)base[tableDeviceTrackInfo.IDTrackDefinitionColumn];
			}
			set
			{
				base[tableDeviceTrackInfo.IDTrackDefinitionColumn] = value;
			}
		}

		public Guid GetIdRecorder()
		{
			return this.GetColumnValue<Guid>(tableDeviceTrackInfo.IDRecorderColumn);
		}

		public string GetIdTrack()
		{
			return this.GetColumnValue<Guid>(tableDeviceTrackInfo.IDTrackColumn).ToString();
		}

		public string GetIdDevice()
		{
			return this.GetColumnValue<Guid>(tableDeviceTrackInfo.IDDeviceColumn).ToString();
		}

		public DateTime GetDeviceName()
		{
			return new DateTime(this.GetColumnValue<long>(tableDeviceTrackInfo.NameColumn), DateTimeKind.Utc);
		}

		public DateTime GetDeviceType()
		{
			return new DateTime(this.GetColumnValue<long>(tableDeviceTrackInfo.DeviceTypeColumn), DateTimeKind.Utc);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackInfoRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDeviceTrackInfo = (DeviceTrackInfoDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsIDDeviceNull()
		{
			return IsNull(tableDeviceTrackInfo.IDDeviceColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetIDDeviceNull()
		{
			base[tableDeviceTrackInfo.IDDeviceColumn] = Convert.DBNull;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class DeviceGroupsDataTable : TypedTableBase<DeviceGroupsRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDDeviceGroup;

		private DataColumn columnIDParentDeviceGroup;

		private DataColumn columnDeviceType;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnBuiltIn;

		private DataColumn columnAutoInsertNew;

		private DataColumn columnIsUserControlled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceGroupColumn => columnIDDeviceGroup;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDParentDeviceGroupColumn => columnIDParentDeviceGroup;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DeviceTypeColumn => columnDeviceType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn BuiltInColumn => columnBuiltIn;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn AutoInsertNewColumn => columnAutoInsertNew;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IsUserControlledColumn => columnIsUserControlled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupsRow this[int index] => (DeviceGroupsRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceGroupsRowChangeEventHandler DeviceGroupsRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceGroupsRowChangeEventHandler DeviceGroupsRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceGroupsRowChangeEventHandler DeviceGroupsRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceGroupsRowChangeEventHandler DeviceGroupsRowDeleted;

		public DataRow[] SortRowsForDelete(DataRow[] dataRows)
		{
			return DataSetUtility.SortRowsForDelete(dataRows, columnIDDeviceGroup.ColumnName, columnIDParentDeviceGroup.ColumnName);
		}

		public DataRow[] SortRowsForInsertOrUpdate(DataRow[] dataRows)
		{
			return DataSetUtility.SortRowsForInsertOrUpdate(dataRows, columnIDDeviceGroup.ColumnName, columnIDParentDeviceGroup.ColumnName);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupsDataTable()
		{
			base.TableName = "DeviceGroups";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupsDataTable(DataTable table)
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
		protected DeviceGroupsDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDeviceGroupsRow(DeviceGroupsRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupsRow AddDeviceGroupsRow(DateTime LastModified, string IDDeviceGroup, string IDParentDeviceGroup, string DeviceType, string Name, string Description, bool BuiltIn, bool AutoInsertNew, bool IsUserControlled)
		{
			DeviceGroupsRow deviceGroupsRow = (DeviceGroupsRow)NewRow();
			object[] itemArray = new object[9] { LastModified, IDDeviceGroup, IDParentDeviceGroup, DeviceType, Name, Description, BuiltIn, AutoInsertNew, IsUserControlled };
			deviceGroupsRow.ItemArray = itemArray;
			base.Rows.Add(deviceGroupsRow);
			return deviceGroupsRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupsRow FindByIDDeviceGroup(string IDDeviceGroup)
		{
			return (DeviceGroupsRow)base.Rows.Find(new object[1] { IDDeviceGroup });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DeviceGroupsDataTable obj = (DeviceGroupsDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DeviceGroupsDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDDeviceGroup = base.Columns["IDDeviceGroup"];
			columnIDParentDeviceGroup = base.Columns["IDParentDeviceGroup"];
			columnDeviceType = base.Columns["DeviceType"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnBuiltIn = base.Columns["BuiltIn"];
			columnAutoInsertNew = base.Columns["AutoInsertNew"];
			columnIsUserControlled = base.Columns["IsUserControlled"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDDeviceGroup = new DataColumn("IDDeviceGroup", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDeviceGroup);
			columnIDParentDeviceGroup = new DataColumn("IDParentDeviceGroup", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDParentDeviceGroup);
			columnDeviceType = new DataColumn("DeviceType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDeviceType);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnBuiltIn = new DataColumn("BuiltIn", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnBuiltIn);
			columnAutoInsertNew = new DataColumn("AutoInsertNew", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnAutoInsertNew);
			columnIsUserControlled = new DataColumn("IsUserControlled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnIsUserControlled);
			base.Constraints.Add(new UniqueConstraint("PK_DeviceGroups", new DataColumn[1] { columnIDDeviceGroup }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDDeviceGroup.AllowDBNull = false;
			columnIDDeviceGroup.Unique = true;
			columnIDDeviceGroup.MaxLength = 36;
			columnIDParentDeviceGroup.MaxLength = 36;
			columnDeviceType.AllowDBNull = false;
			columnDeviceType.MaxLength = 100;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
			columnBuiltIn.AllowDBNull = false;
			columnAutoInsertNew.AllowDBNull = false;
			columnIsUserControlled.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupsRow NewDeviceGroupsRow()
		{
			return (DeviceGroupsRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DeviceGroupsRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DeviceGroupsRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DeviceGroupsRowChanged != null)
			{
				this.DeviceGroupsRowChanged(this, new DeviceGroupsRowChangeEvent((DeviceGroupsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DeviceGroupsRowChanging != null)
			{
				this.DeviceGroupsRowChanging(this, new DeviceGroupsRowChangeEvent((DeviceGroupsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DeviceGroupsRowDeleted != null)
			{
				this.DeviceGroupsRowDeleted(this, new DeviceGroupsRowChangeEvent((DeviceGroupsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DeviceGroupsRowDeleting != null)
			{
				this.DeviceGroupsRowDeleting(this, new DeviceGroupsRowChangeEvent((DeviceGroupsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDeviceGroupsRow(DeviceGroupsRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DeviceGroupsDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class DevicesDataTable : TypedTableBase<DevicesRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDDevice;

		private DataColumn columnIDHardware;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnLocation;

		private DataColumn columnDeviceType;

		private DataColumn columnEnabled;

		private DataColumn columnSettings;

		private DataColumn columnPresets;

		private DataColumn columnShortcut;

		private DataColumn columnMulticastEnabled;

		private DataColumn columnStreams;

		private DataColumn columnDeviceIndex;

		private DataColumn columnSubType;

		private DataColumn columnFailoverLiveOnly;

		private DataColumn columnDisableOnFailover;

		private DataColumn columnPtzManualSessionTimeout;

		private DataColumn columnPtzReservedSessionTimeout;

		private DataColumn columnPtzExtendedManualSessionTimeout;

		private DataColumn columnGisPointString;

		private DataColumn columnShortName;

		private DataColumn columnIcon;

		private DataColumn columnCoverageDirection;

		private DataColumn columnCoverageDepth;

		private DataColumn columnCoverageFieldOfView;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDHardwareColumn => columnIDHardware;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LocationColumn => columnLocation;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DeviceTypeColumn => columnDeviceType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SettingsColumn => columnSettings;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PresetsColumn => columnPresets;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ShortcutColumn => columnShortcut;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn MulticastEnabledColumn => columnMulticastEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn StreamsColumn => columnStreams;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DeviceIndexColumn => columnDeviceIndex;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SubTypeColumn => columnSubType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn FailoverLiveOnlyColumn => columnFailoverLiveOnly;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DisableOnFailoverColumn => columnDisableOnFailover;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PtzManualSessionTimeoutColumn => columnPtzManualSessionTimeout;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PtzReservedSessionTimeoutColumn => columnPtzReservedSessionTimeout;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PtzExtendedManualSessionTimeoutColumn => columnPtzExtendedManualSessionTimeout;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn GisPointStringColumn => columnGisPointString;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ShortNameColumn => columnShortName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IconColumn => columnIcon;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn CoverageDirectionColumn => columnCoverageDirection;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn CoverageDepthColumn => columnCoverageDepth;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn CoverageFieldOfViewColumn => columnCoverageFieldOfView;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicesRow this[int index] => (DevicesRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DevicesRowChangeEventHandler DevicesRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DevicesRowChangeEventHandler DevicesRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DevicesRowChangeEventHandler DevicesRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DevicesRowChangeEventHandler DevicesRowDeleted;

		public DevicesRow FindByIDDevice(Guid deviceId)
		{
			return FindByIDDevice(deviceId.ToString());
		}

		public DevicesRow[] FindByIDHardware(Guid hardwareId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnIDHardware.ColumnName, hardwareId);
			return (DevicesRow[])Select(filterExpression);
		}

		public DevicesRow[] FindByIDHardwareAndDeviceType(Guid hardwareId, string deviceType)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' AND {2} = '{3}'", columnIDHardware.ColumnName, hardwareId, columnDeviceType.ColumnName, deviceType);
			return (DevicesRow[])Select(filterExpression);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicesDataTable()
		{
			base.TableName = "Devices";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicesDataTable(DataTable table)
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
		protected DevicesDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDevicesRow(DevicesRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicesRow AddDevicesRow(DateTime LastModified, string IDDevice, string IDHardware, string Name, string Description, string Location, string DeviceType, bool Enabled, string Settings, string Presets, string Shortcut, bool MulticastEnabled, string Streams, int DeviceIndex, int SubType, bool FailoverLiveOnly, bool DisableOnFailover, int PtzManualSessionTimeout, int PtzReservedSessionTimeout, int PtzExtendedManualSessionTimeout, string GisPointString, string ShortName, int Icon, double CoverageDirection, double CoverageDepth, double CoverageFieldOfView)
		{
			DevicesRow devicesRow = (DevicesRow)NewRow();
			object[] itemArray = new object[26]
			{
				LastModified, IDDevice, IDHardware, Name, Description, Location, DeviceType, Enabled, Settings, Presets,
				Shortcut, MulticastEnabled, Streams, DeviceIndex, SubType, FailoverLiveOnly, DisableOnFailover, PtzManualSessionTimeout, PtzReservedSessionTimeout, PtzExtendedManualSessionTimeout,
				GisPointString, ShortName, Icon, CoverageDirection, CoverageDepth, CoverageFieldOfView
			};
			devicesRow.ItemArray = itemArray;
			base.Rows.Add(devicesRow);
			return devicesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicesRow FindByIDDevice(string IDDevice)
		{
			return (DevicesRow)base.Rows.Find(new object[1] { IDDevice });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DevicesDataTable obj = (DevicesDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DevicesDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDDevice = base.Columns["IDDevice"];
			columnIDHardware = base.Columns["IDHardware"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnLocation = base.Columns["Location"];
			columnDeviceType = base.Columns["DeviceType"];
			columnEnabled = base.Columns["Enabled"];
			columnSettings = base.Columns["Settings"];
			columnPresets = base.Columns["Presets"];
			columnShortcut = base.Columns["Shortcut"];
			columnMulticastEnabled = base.Columns["MulticastEnabled"];
			columnStreams = base.Columns["Streams"];
			columnDeviceIndex = base.Columns["DeviceIndex"];
			columnSubType = base.Columns["SubType"];
			columnFailoverLiveOnly = base.Columns["FailoverLiveOnly"];
			columnDisableOnFailover = base.Columns["DisableOnFailover"];
			columnPtzManualSessionTimeout = base.Columns["PtzManualSessionTimeout"];
			columnPtzReservedSessionTimeout = base.Columns["PtzReservedSessionTimeout"];
			columnPtzExtendedManualSessionTimeout = base.Columns["PtzExtendedManualSessionTimeout"];
			columnGisPointString = base.Columns["GisPointString"];
			columnShortName = base.Columns["ShortName"];
			columnIcon = base.Columns["Icon"];
			columnCoverageDirection = base.Columns["CoverageDirection"];
			columnCoverageDepth = base.Columns["CoverageDepth"];
			columnCoverageFieldOfView = base.Columns["CoverageFieldOfView"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDDevice = new DataColumn("IDDevice", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnIDHardware = new DataColumn("IDHardware", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDHardware);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnLocation = new DataColumn("Location", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnLocation);
			columnDeviceType = new DataColumn("DeviceType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDeviceType);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			columnSettings = new DataColumn("Settings", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSettings);
			columnPresets = new DataColumn("Presets", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnPresets);
			columnShortcut = new DataColumn("Shortcut", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnShortcut);
			columnMulticastEnabled = new DataColumn("MulticastEnabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnMulticastEnabled);
			columnStreams = new DataColumn("Streams", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnStreams);
			columnDeviceIndex = new DataColumn("DeviceIndex", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnDeviceIndex);
			columnSubType = new DataColumn("SubType", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnSubType);
			columnFailoverLiveOnly = new DataColumn("FailoverLiveOnly", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnFailoverLiveOnly);
			columnDisableOnFailover = new DataColumn("DisableOnFailover", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnDisableOnFailover);
			columnPtzManualSessionTimeout = new DataColumn("PtzManualSessionTimeout", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnPtzManualSessionTimeout);
			columnPtzReservedSessionTimeout = new DataColumn("PtzReservedSessionTimeout", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnPtzReservedSessionTimeout);
			columnPtzExtendedManualSessionTimeout = new DataColumn("PtzExtendedManualSessionTimeout", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnPtzExtendedManualSessionTimeout);
			columnGisPointString = new DataColumn("GisPointString", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnGisPointString);
			columnShortName = new DataColumn("ShortName", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnShortName);
			columnIcon = new DataColumn("Icon", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnIcon);
			columnCoverageDirection = new DataColumn("CoverageDirection", typeof(double), null, MappingType.Element);
			base.Columns.Add(columnCoverageDirection);
			columnCoverageDepth = new DataColumn("CoverageDepth", typeof(double), null, MappingType.Element);
			base.Columns.Add(columnCoverageDepth);
			columnCoverageFieldOfView = new DataColumn("CoverageFieldOfView", typeof(double), null, MappingType.Element);
			base.Columns.Add(columnCoverageFieldOfView);
			base.Constraints.Add(new UniqueConstraint("PK_Devices", new DataColumn[1] { columnIDDevice }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.Unique = true;
			columnIDDevice.MaxLength = 36;
			columnIDHardware.AllowDBNull = false;
			columnIDHardware.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
			columnLocation.AllowDBNull = false;
			columnLocation.MaxLength = 1073741823;
			columnDeviceType.AllowDBNull = false;
			columnDeviceType.MaxLength = 100;
			columnEnabled.AllowDBNull = false;
			columnSettings.MaxLength = 1073741823;
			columnPresets.MaxLength = 1073741823;
			columnShortcut.AllowDBNull = false;
			columnShortcut.MaxLength = 128;
			columnMulticastEnabled.AllowDBNull = false;
			columnStreams.MaxLength = 1073741823;
			columnDeviceIndex.AllowDBNull = false;
			columnSubType.AllowDBNull = false;
			columnFailoverLiveOnly.AllowDBNull = false;
			columnDisableOnFailover.AllowDBNull = false;
			columnPtzManualSessionTimeout.AllowDBNull = false;
			columnPtzReservedSessionTimeout.AllowDBNull = false;
			columnPtzExtendedManualSessionTimeout.AllowDBNull = false;
			columnGisPointString.MaxLength = int.MaxValue;
			columnShortName.AllowDBNull = false;
			columnShortName.MaxLength = 128;
			columnIcon.AllowDBNull = false;
			columnCoverageDirection.AllowDBNull = false;
			columnCoverageDepth.AllowDBNull = false;
			columnCoverageFieldOfView.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicesRow NewDevicesRow()
		{
			return (DevicesRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DevicesRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DevicesRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DevicesRowChanged != null)
			{
				this.DevicesRowChanged(this, new DevicesRowChangeEvent((DevicesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DevicesRowChanging != null)
			{
				this.DevicesRowChanging(this, new DevicesRowChangeEvent((DevicesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DevicesRowDeleted != null)
			{
				this.DevicesRowDeleted(this, new DevicesRowChangeEvent((DevicesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DevicesRowDeleting != null)
			{
				this.DevicesRowDeleting(this, new DevicesRowChangeEvent((DevicesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDevicesRow(DevicesRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DevicesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class DeviceTrackDataTable : TypedTableBase<DeviceTrackRow>
	{
		private DataColumn columnIDDevice;

		private DataColumn columnIDTrack;

		private DataColumn columnIDTrackDefinition;

		private DataColumn columnMergeEdge;

		private DataColumn columnDefaultPlayback;

		private DataColumn columnInUse;

		private DataColumn columnOnlyUseMediaAfter;

		private DataColumn columnCreatedDate;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDTrackColumn => columnIDTrack;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDTrackDefinitionColumn => columnIDTrackDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn MergeEdgeColumn => columnMergeEdge;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DefaultPlaybackColumn => columnDefaultPlayback;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn InUseColumn => columnInUse;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn OnlyUseMediaAfterColumn => columnOnlyUseMediaAfter;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn CreatedDateColumn => columnCreatedDate;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackRow this[int index] => (DeviceTrackRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackRowChangeEventHandler DeviceTrackRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackRowChangeEventHandler DeviceTrackRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackRowChangeEventHandler DeviceTrackRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackRowChangeEventHandler DeviceTrackRowDeleted;

		public DeviceTrackRow[] FindByIDDevice(Guid deviceId)
		{
			return FindByIDDevice(deviceId.ToString());
		}

		public DeviceTrackRow[] FindByIDDevice(string deviceId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnIDDevice.ColumnName, deviceId.ToString());
			return (DeviceTrackRow[])Select(filterExpression);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDataTable()
		{
			base.TableName = "DeviceTrack";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDataTable(DataTable table)
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
		protected DeviceTrackDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDeviceTrackRow(DeviceTrackRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackRow AddDeviceTrackRow(string IDDevice, Guid IDTrack, Guid IDTrackDefinition, bool MergeEdge, bool DefaultPlayback, bool InUse, DateTime OnlyUseMediaAfter, DateTime CreatedDate)
		{
			DeviceTrackRow deviceTrackRow = (DeviceTrackRow)NewRow();
			object[] itemArray = new object[8] { IDDevice, IDTrack, IDTrackDefinition, MergeEdge, DefaultPlayback, InUse, OnlyUseMediaAfter, CreatedDate };
			deviceTrackRow.ItemArray = itemArray;
			base.Rows.Add(deviceTrackRow);
			return deviceTrackRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackRow FindByIDTrack(Guid IDTrack)
		{
			return (DeviceTrackRow)base.Rows.Find(new object[1] { IDTrack });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DeviceTrackDataTable obj = (DeviceTrackDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DeviceTrackDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDDevice = base.Columns["IDDevice"];
			columnIDTrack = base.Columns["IDTrack"];
			columnIDTrackDefinition = base.Columns["IDTrackDefinition"];
			columnMergeEdge = base.Columns["MergeEdge"];
			columnDefaultPlayback = base.Columns["DefaultPlayback"];
			columnInUse = base.Columns["InUse"];
			columnOnlyUseMediaAfter = base.Columns["OnlyUseMediaAfter"];
			columnCreatedDate = base.Columns["CreatedDate"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDDevice = new DataColumn("IDDevice", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnIDTrack = new DataColumn("IDTrack", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDTrack);
			columnIDTrackDefinition = new DataColumn("IDTrackDefinition", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDTrackDefinition);
			columnMergeEdge = new DataColumn("MergeEdge", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnMergeEdge);
			columnDefaultPlayback = new DataColumn("DefaultPlayback", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnDefaultPlayback);
			columnInUse = new DataColumn("InUse", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnInUse);
			columnOnlyUseMediaAfter = new DataColumn("OnlyUseMediaAfter", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnOnlyUseMediaAfter);
			columnCreatedDate = new DataColumn("CreatedDate", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnCreatedDate);
			base.Constraints.Add(new UniqueConstraint("PK_DeviceTrack", new DataColumn[1] { columnIDTrack }, isPrimaryKey: true));
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.MaxLength = 36;
			columnIDTrack.AllowDBNull = false;
			columnIDTrack.Unique = true;
			columnIDTrackDefinition.AllowDBNull = false;
			columnMergeEdge.AllowDBNull = false;
			columnDefaultPlayback.AllowDBNull = false;
			columnInUse.AllowDBNull = false;
			columnOnlyUseMediaAfter.AllowDBNull = false;
			columnOnlyUseMediaAfter.DateTimeMode = DataSetDateTime.Utc;
			columnCreatedDate.AllowDBNull = false;
			columnCreatedDate.DateTimeMode = DataSetDateTime.Utc;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackRow NewDeviceTrackRow()
		{
			return (DeviceTrackRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DeviceTrackRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DeviceTrackRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DeviceTrackRowChanged != null)
			{
				this.DeviceTrackRowChanged(this, new DeviceTrackRowChangeEvent((DeviceTrackRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DeviceTrackRowChanging != null)
			{
				this.DeviceTrackRowChanging(this, new DeviceTrackRowChangeEvent((DeviceTrackRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DeviceTrackRowDeleted != null)
			{
				this.DeviceTrackRowDeleted(this, new DeviceTrackRowChangeEvent((DeviceTrackRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DeviceTrackRowDeleting != null)
			{
				this.DeviceTrackRowDeleting(this, new DeviceTrackRowChangeEvent((DeviceTrackRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDeviceTrackRow(DeviceTrackRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DeviceTrackDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class DeviceTrackRow : DataRow
	{
		private DeviceTrackDataTable tableDeviceTrack;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDevice
		{
			get
			{
				return (string)base[tableDeviceTrack.IDDeviceColumn];
			}
			set
			{
				base[tableDeviceTrack.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDTrack
		{
			get
			{
				return (Guid)base[tableDeviceTrack.IDTrackColumn];
			}
			set
			{
				base[tableDeviceTrack.IDTrackColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDTrackDefinition
		{
			get
			{
				return (Guid)base[tableDeviceTrack.IDTrackDefinitionColumn];
			}
			set
			{
				base[tableDeviceTrack.IDTrackDefinitionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool MergeEdge
		{
			get
			{
				return (bool)base[tableDeviceTrack.MergeEdgeColumn];
			}
			set
			{
				base[tableDeviceTrack.MergeEdgeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool DefaultPlayback
		{
			get
			{
				return (bool)base[tableDeviceTrack.DefaultPlaybackColumn];
			}
			set
			{
				base[tableDeviceTrack.DefaultPlaybackColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool InUse
		{
			get
			{
				return (bool)base[tableDeviceTrack.InUseColumn];
			}
			set
			{
				base[tableDeviceTrack.InUseColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime OnlyUseMediaAfter
		{
			get
			{
				return (DateTime)base[tableDeviceTrack.OnlyUseMediaAfterColumn];
			}
			set
			{
				base[tableDeviceTrack.OnlyUseMediaAfterColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime CreatedDate
		{
			get
			{
				return (DateTime)base[tableDeviceTrack.CreatedDateColumn];
			}
			set
			{
				base[tableDeviceTrack.CreatedDateColumn] = value;
			}
		}

		public string GetIDDevice()
		{
			return this.GetColumnValue<string>(tableDeviceTrack.IDDeviceColumn);
		}

		public Guid GetIDTrack()
		{
			return this.GetColumnValue<Guid>(tableDeviceTrack.IDTrackColumn);
		}

		public Guid GetIDTrackDefinition()
		{
			return this.GetColumnValue<Guid>(tableDeviceTrack.IDTrackDefinitionColumn);
		}

		public bool GetDefaultPlayback()
		{
			return this.GetColumnValue<bool>(tableDeviceTrack.DefaultPlaybackColumn);
		}

		public bool GetMergeEdge()
		{
			return this.GetColumnValue<bool>(tableDeviceTrack.MergeEdgeColumn);
		}

		public bool GetInUse()
		{
			return this.GetColumnValue<bool>(tableDeviceTrack.InUseColumn);
		}

		public DateTime GetOnlyUseMediaAfter()
		{
			return this.GetColumnValue<DateTime>(tableDeviceTrack.OnlyUseMediaAfterColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDeviceTrack = (DeviceTrackDataTable)base.Table;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class DevicePrivacyMaskDataTable : TypedTableBase<DevicePrivacyMaskRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDDevice;

		private DataColumn columnPrivacyMask;

		private DataColumn columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PrivacyMaskColumn => columnPrivacyMask;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicePrivacyMaskRow this[int index] => (DevicePrivacyMaskRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DevicePrivacyMaskRowChangeEventHandler DevicePrivacyMaskRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DevicePrivacyMaskRowChangeEventHandler DevicePrivacyMaskRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DevicePrivacyMaskRowChangeEventHandler DevicePrivacyMaskRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DevicePrivacyMaskRowChangeEventHandler DevicePrivacyMaskRowDeleted;

		public DevicePrivacyMaskRow[] FindByIDDevice(Guid deviceId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", IDDeviceColumn.ColumnName, deviceId);
			return (DevicePrivacyMaskRow[])Select(filterExpression);
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicePrivacyMaskDataTable()
		{
			base.TableName = "DevicePrivacyMask";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicePrivacyMaskDataTable(DataTable table)
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
		protected DevicePrivacyMaskDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDevicePrivacyMaskRow(DevicePrivacyMaskRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicePrivacyMaskRow AddDevicePrivacyMaskRow(DateTime LastModified, string IDDevice, string PrivacyMask, bool Enabled)
		{
			DevicePrivacyMaskRow devicePrivacyMaskRow = (DevicePrivacyMaskRow)NewRow();
			object[] itemArray = new object[4] { LastModified, IDDevice, PrivacyMask, Enabled };
			devicePrivacyMaskRow.ItemArray = itemArray;
			base.Rows.Add(devicePrivacyMaskRow);
			return devicePrivacyMaskRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicePrivacyMaskRow FindByLastModifiedIDDevice(DateTime LastModified, string IDDevice)
		{
			return (DevicePrivacyMaskRow)base.Rows.Find(new object[2] { LastModified, IDDevice });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DevicePrivacyMaskDataTable obj = (DevicePrivacyMaskDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DevicePrivacyMaskDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDDevice = base.Columns["IDDevice"];
			columnPrivacyMask = base.Columns["PrivacyMask"];
			columnEnabled = base.Columns["Enabled"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDDevice = new DataColumn("IDDevice", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnPrivacyMask = new DataColumn("PrivacyMask", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnPrivacyMask);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			base.Constraints.Add(new UniqueConstraint("PK_DevicePrivacyMask", new DataColumn[2] { columnLastModified, columnIDDevice }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnLastModified.DateTimeMode = DataSetDateTime.Utc;
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.MaxLength = 36;
			columnPrivacyMask.AllowDBNull = false;
			columnPrivacyMask.MaxLength = int.MaxValue;
			columnEnabled.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicePrivacyMaskRow NewDevicePrivacyMaskRow()
		{
			return (DevicePrivacyMaskRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DevicePrivacyMaskRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DevicePrivacyMaskRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DevicePrivacyMaskRowChanged != null)
			{
				this.DevicePrivacyMaskRowChanged(this, new DevicePrivacyMaskRowChangeEvent((DevicePrivacyMaskRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DevicePrivacyMaskRowChanging != null)
			{
				this.DevicePrivacyMaskRowChanging(this, new DevicePrivacyMaskRowChangeEvent((DevicePrivacyMaskRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DevicePrivacyMaskRowDeleted != null)
			{
				this.DevicePrivacyMaskRowDeleted(this, new DevicePrivacyMaskRowChangeEvent((DevicePrivacyMaskRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DevicePrivacyMaskRowDeleting != null)
			{
				this.DevicePrivacyMaskRowDeleting(this, new DevicePrivacyMaskRowChangeEvent((DevicePrivacyMaskRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDevicePrivacyMaskRow(DevicePrivacyMaskRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DevicePrivacyMaskDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class DeviceRestrictedMediaDataTable : TypedTableBase<DeviceRestrictedMediaRow>
	{
		private DataColumn columnIDRestrictedMedia;

		private DataColumn columnIDDevice;

		private DataColumn columnIDRecorder;

		private DataColumn columnStartTime;

		private DataColumn columnEndTime;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRestrictedMediaColumn => columnIDRestrictedMedia;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderColumn => columnIDRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn StartTimeColumn => columnStartTime;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EndTimeColumn => columnEndTime;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceRestrictedMediaRow this[int index] => (DeviceRestrictedMediaRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceRestrictedMediaRowChangeEventHandler DeviceRestrictedMediaRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceRestrictedMediaRowChangeEventHandler DeviceRestrictedMediaRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceRestrictedMediaRowChangeEventHandler DeviceRestrictedMediaRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceRestrictedMediaRowChangeEventHandler DeviceRestrictedMediaRowDeleted;

		public DeviceRestrictedMediaRow[] FindByIDDevice(Guid deviceId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", IDDeviceColumn.ColumnName, deviceId);
			return (DeviceRestrictedMediaRow[])Select(filterExpression);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceRestrictedMediaDataTable()
		{
			base.TableName = "DeviceRestrictedMedia";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceRestrictedMediaDataTable(DataTable table)
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
		protected DeviceRestrictedMediaDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDeviceRestrictedMediaRow(DeviceRestrictedMediaRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceRestrictedMediaRow AddDeviceRestrictedMediaRow(Guid IDRestrictedMedia, string IDDevice, string IDRecorder, DateTime StartTime, DateTime EndTime)
		{
			DeviceRestrictedMediaRow deviceRestrictedMediaRow = (DeviceRestrictedMediaRow)NewRow();
			object[] itemArray = new object[5] { IDRestrictedMedia, IDDevice, IDRecorder, StartTime, EndTime };
			deviceRestrictedMediaRow.ItemArray = itemArray;
			base.Rows.Add(deviceRestrictedMediaRow);
			return deviceRestrictedMediaRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceRestrictedMediaRow FindByIDRestrictedMediaIDDevice(Guid IDRestrictedMedia, string IDDevice)
		{
			return (DeviceRestrictedMediaRow)base.Rows.Find(new object[2] { IDRestrictedMedia, IDDevice });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DeviceRestrictedMediaDataTable obj = (DeviceRestrictedMediaDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DeviceRestrictedMediaDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDRestrictedMedia = base.Columns["IDRestrictedMedia"];
			columnIDDevice = base.Columns["IDDevice"];
			columnIDRecorder = base.Columns["IDRecorder"];
			columnStartTime = base.Columns["StartTime"];
			columnEndTime = base.Columns["EndTime"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDRestrictedMedia = new DataColumn("IDRestrictedMedia", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDRestrictedMedia);
			columnIDDevice = new DataColumn("IDDevice", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnIDRecorder = new DataColumn("IDRecorder", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRecorder);
			columnStartTime = new DataColumn("StartTime", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnStartTime);
			columnEndTime = new DataColumn("EndTime", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnEndTime);
			base.Constraints.Add(new UniqueConstraint("PK_DeviceRestrictedMedia", new DataColumn[2] { columnIDRestrictedMedia, columnIDDevice }, isPrimaryKey: true));
			columnIDRestrictedMedia.AllowDBNull = false;
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.MaxLength = 36;
			columnIDRecorder.AllowDBNull = false;
			columnIDRecorder.MaxLength = 36;
			columnStartTime.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceRestrictedMediaRow NewDeviceRestrictedMediaRow()
		{
			return (DeviceRestrictedMediaRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DeviceRestrictedMediaRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DeviceRestrictedMediaRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DeviceRestrictedMediaRowChanged != null)
			{
				this.DeviceRestrictedMediaRowChanged(this, new DeviceRestrictedMediaRowChangeEvent((DeviceRestrictedMediaRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DeviceRestrictedMediaRowChanging != null)
			{
				this.DeviceRestrictedMediaRowChanging(this, new DeviceRestrictedMediaRowChangeEvent((DeviceRestrictedMediaRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DeviceRestrictedMediaRowDeleted != null)
			{
				this.DeviceRestrictedMediaRowDeleted(this, new DeviceRestrictedMediaRowChangeEvent((DeviceRestrictedMediaRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DeviceRestrictedMediaRowDeleting != null)
			{
				this.DeviceRestrictedMediaRowDeleting(this, new DeviceRestrictedMediaRowChangeEvent((DeviceRestrictedMediaRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDeviceRestrictedMediaRow(DeviceRestrictedMediaRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DeviceRestrictedMediaDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class DeviceLinksDataTable : TypedTableBase<DeviceLinksRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDDevice;

		private DataColumn columnIDDeviceLinked;

		private DataColumn columnPriority;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceLinkedColumn => columnIDDeviceLinked;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PriorityColumn => columnPriority;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceLinksRow this[int index] => (DeviceLinksRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceLinksRowChangeEventHandler DeviceLinksRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceLinksRowChangeEventHandler DeviceLinksRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceLinksRowChangeEventHandler DeviceLinksRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceLinksRowChangeEventHandler DeviceLinksRowDeleted;

		public DeviceLinksRow[] FindByDeviceId(Guid deviceId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}' OR {2} = '{3}'", columnIDDevice.ColumnName, deviceId, columnIDDeviceLinked.ColumnName, deviceId);
			return (DeviceLinksRow[])Select(filterExpression);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceLinksDataTable()
		{
			base.TableName = "DeviceLinks";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceLinksDataTable(DataTable table)
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
		protected DeviceLinksDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDeviceLinksRow(DeviceLinksRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceLinksRow AddDeviceLinksRow(DateTime LastModified, string IDDevice, string IDDeviceLinked, int Priority)
		{
			DeviceLinksRow deviceLinksRow = (DeviceLinksRow)NewRow();
			object[] itemArray = new object[4] { LastModified, IDDevice, IDDeviceLinked, Priority };
			deviceLinksRow.ItemArray = itemArray;
			base.Rows.Add(deviceLinksRow);
			return deviceLinksRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceLinksRow FindByIDDeviceIDDeviceLinked(string IDDevice, string IDDeviceLinked)
		{
			return (DeviceLinksRow)base.Rows.Find(new object[2] { IDDevice, IDDeviceLinked });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DeviceLinksDataTable obj = (DeviceLinksDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DeviceLinksDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDDevice = base.Columns["IDDevice"];
			columnIDDeviceLinked = base.Columns["IDDeviceLinked"];
			columnPriority = base.Columns["Priority"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDDevice = new DataColumn("IDDevice", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnIDDeviceLinked = new DataColumn("IDDeviceLinked", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDeviceLinked);
			columnPriority = new DataColumn("Priority", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnPriority);
			base.Constraints.Add(new UniqueConstraint("PK_DeviceLinks", new DataColumn[2] { columnIDDevice, columnIDDeviceLinked }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.MaxLength = 36;
			columnIDDeviceLinked.AllowDBNull = false;
			columnIDDeviceLinked.MaxLength = 36;
			columnPriority.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceLinksRow NewDeviceLinksRow()
		{
			return (DeviceLinksRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DeviceLinksRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DeviceLinksRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DeviceLinksRowChanged != null)
			{
				this.DeviceLinksRowChanged(this, new DeviceLinksRowChangeEvent((DeviceLinksRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DeviceLinksRowChanging != null)
			{
				this.DeviceLinksRowChanging(this, new DeviceLinksRowChangeEvent((DeviceLinksRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DeviceLinksRowDeleted != null)
			{
				this.DeviceLinksRowDeleted(this, new DeviceLinksRowChangeEvent((DeviceLinksRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DeviceLinksRowDeleting != null)
			{
				this.DeviceLinksRowDeleting(this, new DeviceLinksRowChangeEvent((DeviceLinksRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDeviceLinksRow(DeviceLinksRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DeviceLinksDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class DeviceGroupsRow : DataRow
	{
		private DeviceGroupsDataTable tableDeviceGroups;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableDeviceGroups.LastModifiedColumn];
			}
			set
			{
				base[tableDeviceGroups.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDeviceGroup
		{
			get
			{
				return (string)base[tableDeviceGroups.IDDeviceGroupColumn];
			}
			set
			{
				base[tableDeviceGroups.IDDeviceGroupColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDParentDeviceGroup
		{
			get
			{
				try
				{
					return (string)base[tableDeviceGroups.IDParentDeviceGroupColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'IDParentDeviceGroup' in table 'DeviceGroups' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableDeviceGroups.IDParentDeviceGroupColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string DeviceType
		{
			get
			{
				return (string)base[tableDeviceGroups.DeviceTypeColumn];
			}
			set
			{
				base[tableDeviceGroups.DeviceTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableDeviceGroups.NameColumn];
			}
			set
			{
				base[tableDeviceGroups.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableDeviceGroups.DescriptionColumn];
			}
			set
			{
				base[tableDeviceGroups.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool BuiltIn
		{
			get
			{
				return (bool)base[tableDeviceGroups.BuiltInColumn];
			}
			set
			{
				base[tableDeviceGroups.BuiltInColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool AutoInsertNew
		{
			get
			{
				return (bool)base[tableDeviceGroups.AutoInsertNewColumn];
			}
			set
			{
				base[tableDeviceGroups.AutoInsertNewColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsUserControlled
		{
			get
			{
				return (bool)base[tableDeviceGroups.IsUserControlledColumn];
			}
			set
			{
				base[tableDeviceGroups.IsUserControlledColumn] = value;
			}
		}

		public string GetIDDeviceGroup(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDeviceGroups.IDDeviceGroupColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupsRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDeviceGroups = (DeviceGroupsDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsIDParentDeviceGroupNull()
		{
			return IsNull(tableDeviceGroups.IDParentDeviceGroupColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetIDParentDeviceGroupNull()
		{
			base[tableDeviceGroups.IDParentDeviceGroupColumn] = Convert.DBNull;
		}
	}

	public class DeviceGroupMembersRow : DataRow
	{
		private DeviceGroupMembersDataTable tableDeviceGroupMembers;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableDeviceGroupMembers.LastModifiedColumn];
			}
			set
			{
				base[tableDeviceGroupMembers.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDeviceGroup
		{
			get
			{
				return (string)base[tableDeviceGroupMembers.IDDeviceGroupColumn];
			}
			set
			{
				base[tableDeviceGroupMembers.IDDeviceGroupColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDevice
		{
			get
			{
				return (string)base[tableDeviceGroupMembers.IDDeviceColumn];
			}
			set
			{
				base[tableDeviceGroupMembers.IDDeviceColumn] = value;
			}
		}

		public string GetIDDevice(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDeviceGroupMembers.IDDeviceColumn, version);
		}

		public string GetIDDeviceGroup(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDeviceGroupMembers.IDDeviceGroupColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupMembersRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDeviceGroupMembers = (DeviceGroupMembersDataTable)base.Table;
		}
	}

	public class DevicesRow : DataRow
	{
		private DevicesDataTable tableDevices;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableDevices.LastModifiedColumn];
			}
			set
			{
				base[tableDevices.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDevice
		{
			get
			{
				return (string)base[tableDevices.IDDeviceColumn];
			}
			set
			{
				base[tableDevices.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDHardware
		{
			get
			{
				return (string)base[tableDevices.IDHardwareColumn];
			}
			set
			{
				base[tableDevices.IDHardwareColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableDevices.NameColumn];
			}
			set
			{
				base[tableDevices.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableDevices.DescriptionColumn];
			}
			set
			{
				base[tableDevices.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Location
		{
			get
			{
				return (string)base[tableDevices.LocationColumn];
			}
			set
			{
				base[tableDevices.LocationColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string DeviceType
		{
			get
			{
				return (string)base[tableDevices.DeviceTypeColumn];
			}
			set
			{
				base[tableDevices.DeviceTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Enabled
		{
			get
			{
				return (bool)base[tableDevices.EnabledColumn];
			}
			set
			{
				base[tableDevices.EnabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Settings
		{
			get
			{
				try
				{
					return (string)base[tableDevices.SettingsColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Settings' in table 'Devices' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableDevices.SettingsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Presets
		{
			get
			{
				try
				{
					return (string)base[tableDevices.PresetsColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Presets' in table 'Devices' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableDevices.PresetsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Shortcut
		{
			get
			{
				return (string)base[tableDevices.ShortcutColumn];
			}
			set
			{
				base[tableDevices.ShortcutColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool MulticastEnabled
		{
			get
			{
				return (bool)base[tableDevices.MulticastEnabledColumn];
			}
			set
			{
				base[tableDevices.MulticastEnabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Streams
		{
			get
			{
				try
				{
					return (string)base[tableDevices.StreamsColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Streams' in table 'Devices' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableDevices.StreamsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int DeviceIndex
		{
			get
			{
				return (int)base[tableDevices.DeviceIndexColumn];
			}
			set
			{
				base[tableDevices.DeviceIndexColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int SubType
		{
			get
			{
				return (int)base[tableDevices.SubTypeColumn];
			}
			set
			{
				base[tableDevices.SubTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool FailoverLiveOnly
		{
			get
			{
				return (bool)base[tableDevices.FailoverLiveOnlyColumn];
			}
			set
			{
				base[tableDevices.FailoverLiveOnlyColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool DisableOnFailover
		{
			get
			{
				return (bool)base[tableDevices.DisableOnFailoverColumn];
			}
			set
			{
				base[tableDevices.DisableOnFailoverColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int PtzManualSessionTimeout
		{
			get
			{
				return (int)base[tableDevices.PtzManualSessionTimeoutColumn];
			}
			set
			{
				base[tableDevices.PtzManualSessionTimeoutColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int PtzReservedSessionTimeout
		{
			get
			{
				return (int)base[tableDevices.PtzReservedSessionTimeoutColumn];
			}
			set
			{
				base[tableDevices.PtzReservedSessionTimeoutColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int PtzExtendedManualSessionTimeout
		{
			get
			{
				return (int)base[tableDevices.PtzExtendedManualSessionTimeoutColumn];
			}
			set
			{
				base[tableDevices.PtzExtendedManualSessionTimeoutColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string GisPointString
		{
			get
			{
				try
				{
					return (string)base[tableDevices.GisPointStringColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'GisPointString' in table 'Devices' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableDevices.GisPointStringColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string ShortName
		{
			get
			{
				return (string)base[tableDevices.ShortNameColumn];
			}
			set
			{
				base[tableDevices.ShortNameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int Icon
		{
			get
			{
				return (int)base[tableDevices.IconColumn];
			}
			set
			{
				base[tableDevices.IconColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public double CoverageDirection
		{
			get
			{
				return (double)base[tableDevices.CoverageDirectionColumn];
			}
			set
			{
				base[tableDevices.CoverageDirectionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public double CoverageDepth
		{
			get
			{
				return (double)base[tableDevices.CoverageDepthColumn];
			}
			set
			{
				base[tableDevices.CoverageDepthColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public double CoverageFieldOfView
		{
			get
			{
				return (double)base[tableDevices.CoverageFieldOfViewColumn];
			}
			set
			{
				base[tableDevices.CoverageFieldOfViewColumn] = value;
			}
		}

		public string GetIDDevice()
		{
			return this.GetColumnValue<string>(tableDevices.IDDeviceColumn);
		}

		public string GetIDDevice(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDevices.IDDeviceColumn, version);
		}

		public string GetIDHardware()
		{
			return this.GetColumnValue<string>(tableDevices.IDHardwareColumn);
		}

		public string GetIDHardware(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDevices.IDHardwareColumn, version);
		}

		public string GetName()
		{
			return this.GetColumnValue<string>(tableDevices.NameColumn);
		}

		public string GetName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDevices.NameColumn, version);
		}

		public bool GetEnabled()
		{
			return this.GetColumnValue<bool>(tableDevices.EnabledColumn);
		}

		public bool GetEnabled(DataRowVersion version)
		{
			return this.GetColumnValue<bool>(tableDevices.EnabledColumn, version);
		}

		public string GetDeviceType()
		{
			return this.GetColumnValue<string>(tableDevices.DeviceTypeColumn);
		}

		public string GetDeviceType(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDevices.DeviceTypeColumn, version);
		}

		public bool GetDisableOnFailover(DataRowVersion version)
		{
			return this.GetColumnValue<bool>(tableDevices.DisableOnFailoverColumn, version);
		}

		public int GetDeviceIndex(DataRowVersion version)
		{
			return this.GetColumnValue<int>(tableDevices.DeviceIndexColumn, version);
		}

		public string GetSettings(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDevices.SettingsColumn, version);
		}

		public string GetStreamSettings(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDevices.StreamsColumn, version);
		}

		public string GetPresetSettings(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDevices.PresetsColumn, version);
		}

		public string GetGisPointString(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDevices.GisPointStringColumn, version);
		}

		public string GetShortName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDevices.ShortNameColumn, version);
		}

		public int GetIcon(DataRowVersion version)
		{
			return this.GetColumnValue<int>(tableDevices.IconColumn, version);
		}

		public double GetCoverageDirection(DataRowVersion version)
		{
			return this.GetColumnValue<double>(tableDevices.CoverageDirectionColumn, version);
		}

		public double GetCoverageDepth(DataRowVersion version)
		{
			return this.GetColumnValue<double>(tableDevices.CoverageDepthColumn, version);
		}

		public double GetCoverageFieldOfView(DataRowVersion version)
		{
			return this.GetColumnValue<double>(tableDevices.CoverageFieldOfViewColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicesRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDevices = (DevicesDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsSettingsNull()
		{
			return IsNull(tableDevices.SettingsColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetSettingsNull()
		{
			base[tableDevices.SettingsColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsPresetsNull()
		{
			return IsNull(tableDevices.PresetsColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetPresetsNull()
		{
			base[tableDevices.PresetsColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsStreamsNull()
		{
			return IsNull(tableDevices.StreamsColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetStreamsNull()
		{
			base[tableDevices.StreamsColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsGisPointStringNull()
		{
			return IsNull(tableDevices.GisPointStringColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetGisPointStringNull()
		{
			base[tableDevices.GisPointStringColumn] = Convert.DBNull;
		}
	}

	public class DeviceLinksRow : DataRow
	{
		private DeviceLinksDataTable tableDeviceLinks;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableDeviceLinks.LastModifiedColumn];
			}
			set
			{
				base[tableDeviceLinks.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDevice
		{
			get
			{
				return (string)base[tableDeviceLinks.IDDeviceColumn];
			}
			set
			{
				base[tableDeviceLinks.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDeviceLinked
		{
			get
			{
				return (string)base[tableDeviceLinks.IDDeviceLinkedColumn];
			}
			set
			{
				base[tableDeviceLinks.IDDeviceLinkedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int Priority
		{
			get
			{
				return (int)base[tableDeviceLinks.PriorityColumn];
			}
			set
			{
				base[tableDeviceLinks.PriorityColumn] = value;
			}
		}

		public string GetIDDevice(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDeviceLinks.IDDeviceColumn, version);
		}

		public string GetIDDeviceLinked(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableDeviceLinks.IDDeviceLinkedColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceLinksRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDeviceLinks = (DeviceLinksDataTable)base.Table;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class HardwareDataTable : TypedTableBase<HardwareRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDHardware;

		private DataColumn columnIDHardwareDriver;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnURI;

		private DataColumn columnLoginID;

		private DataColumn columnEncryptedPassword;

		private DataColumn columnIDRecorder;

		private DataColumn columnEnabled;

		private DataColumn columnSettings;

		private DataColumn columnEventSettings;

		private DataColumn columnVendorProductID;

		private DataColumn columnC18NDigest;

		private DataColumn columnDriverNumber;

		private DataColumn columnDriverProxyName;

		private DataColumn columnHostedHardware_id;

		private DataColumn columnCameraData;

		private DataColumn columnEdgeSettings;

		private DataColumn columnSubType;

		private DataColumn columnCredentialsLastModified;

		private DataColumn columnHardwareDataLastUpdated;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDHardwareColumn => columnIDHardware;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDHardwareDriverColumn => columnIDHardwareDriver;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn URIColumn => columnURI;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LoginIDColumn => columnLoginID;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EncryptedPasswordColumn => columnEncryptedPassword;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderColumn => columnIDRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SettingsColumn => columnSettings;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EventSettingsColumn => columnEventSettings;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn VendorProductIDColumn => columnVendorProductID;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn C18NDigestColumn => columnC18NDigest;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DriverNumberColumn => columnDriverNumber;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DriverProxyNameColumn => columnDriverProxyName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn HostedHardware_idColumn => columnHostedHardware_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn CameraDataColumn => columnCameraData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EdgeSettingsColumn => columnEdgeSettings;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SubTypeColumn => columnSubType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn CredentialsLastModifiedColumn => columnCredentialsLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn HardwareDataLastUpdatedColumn => columnHardwareDataLastUpdated;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareRow this[int index] => (HardwareRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareRowChangeEventHandler HardwareRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareRowChangeEventHandler HardwareRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareRowChangeEventHandler HardwareRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareRowChangeEventHandler HardwareRowDeleted;

		public HardwareRow FindByIDHardware(Guid hardwareId)
		{
			return FindByIDHardware(hardwareId.ToString());
		}

		public HardwareRow[] FindByIDRecorder(Guid recorderId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnIDRecorder.ColumnName, recorderId);
			return (HardwareRow[])Select(filterExpression);
		}

		public HardwareRow[] FindByIDRecorder(string recorderId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnIDRecorder.ColumnName, recorderId);
			return (HardwareRow[])Select(filterExpression);
		}

		public new HardwareRow[] Select()
		{
			return (HardwareRow[])base.Select();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDataTable()
		{
			base.TableName = "Hardware";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDataTable(DataTable table)
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
		protected HardwareDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddHardwareRow(HardwareRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareRow AddHardwareRow(DateTime LastModified, string IDHardware, string IDHardwareDriver, string Name, string Description, string URI, string LoginID, string EncryptedPassword, string IDRecorder, bool Enabled, string Settings, string EventSettings, string VendorProductID, string C18NDigest, int DriverNumber, string DriverProxyName, int HostedHardware_id, string CameraData, string EdgeSettings, int SubType, DateTime CredentialsLastModified, DateTime HardwareDataLastUpdated)
		{
			HardwareRow hardwareRow = (HardwareRow)NewRow();
			object[] itemArray = new object[22]
			{
				LastModified, IDHardware, IDHardwareDriver, Name, Description, URI, LoginID, EncryptedPassword, IDRecorder, Enabled,
				Settings, EventSettings, VendorProductID, C18NDigest, DriverNumber, DriverProxyName, HostedHardware_id, CameraData, EdgeSettings, SubType,
				CredentialsLastModified, HardwareDataLastUpdated
			};
			hardwareRow.ItemArray = itemArray;
			base.Rows.Add(hardwareRow);
			return hardwareRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareRow FindByIDHardware(string IDHardware)
		{
			return (HardwareRow)base.Rows.Find(new object[1] { IDHardware });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			HardwareDataTable obj = (HardwareDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new HardwareDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDHardware = base.Columns["IDHardware"];
			columnIDHardwareDriver = base.Columns["IDHardwareDriver"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnURI = base.Columns["URI"];
			columnLoginID = base.Columns["LoginID"];
			columnEncryptedPassword = base.Columns["EncryptedPassword"];
			columnIDRecorder = base.Columns["IDRecorder"];
			columnEnabled = base.Columns["Enabled"];
			columnSettings = base.Columns["Settings"];
			columnEventSettings = base.Columns["EventSettings"];
			columnVendorProductID = base.Columns["VendorProductID"];
			columnC18NDigest = base.Columns["C18NDigest"];
			columnDriverNumber = base.Columns["DriverNumber"];
			columnDriverProxyName = base.Columns["DriverProxyName"];
			columnHostedHardware_id = base.Columns["HostedHardware_id"];
			columnCameraData = base.Columns["CameraData"];
			columnEdgeSettings = base.Columns["EdgeSettings"];
			columnSubType = base.Columns["SubType"];
			columnCredentialsLastModified = base.Columns["CredentialsLastModified"];
			columnHardwareDataLastUpdated = base.Columns["HardwareDataLastUpdated"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDHardware = new DataColumn("IDHardware", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDHardware);
			columnIDHardwareDriver = new DataColumn("IDHardwareDriver", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDHardwareDriver);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnURI = new DataColumn("URI", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnURI);
			columnLoginID = new DataColumn("LoginID", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnLoginID);
			columnEncryptedPassword = new DataColumn("EncryptedPassword", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnEncryptedPassword);
			columnIDRecorder = new DataColumn("IDRecorder", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRecorder);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			columnSettings = new DataColumn("Settings", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSettings);
			columnEventSettings = new DataColumn("EventSettings", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnEventSettings);
			columnVendorProductID = new DataColumn("VendorProductID", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnVendorProductID);
			columnC18NDigest = new DataColumn("C18NDigest", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnC18NDigest);
			columnDriverNumber = new DataColumn("DriverNumber", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnDriverNumber);
			columnDriverProxyName = new DataColumn("DriverProxyName", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDriverProxyName);
			columnHostedHardware_id = new DataColumn("HostedHardware_id", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnHostedHardware_id);
			columnCameraData = new DataColumn("CameraData", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnCameraData);
			columnEdgeSettings = new DataColumn("EdgeSettings", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnEdgeSettings);
			columnSubType = new DataColumn("SubType", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnSubType);
			columnCredentialsLastModified = new DataColumn("CredentialsLastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnCredentialsLastModified);
			columnHardwareDataLastUpdated = new DataColumn("HardwareDataLastUpdated", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnHardwareDataLastUpdated);
			base.Constraints.Add(new UniqueConstraint("PK_Hardware", new DataColumn[1] { columnIDHardware }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDHardware.AllowDBNull = false;
			columnIDHardware.Unique = true;
			columnIDHardware.MaxLength = 36;
			columnIDHardwareDriver.AllowDBNull = false;
			columnIDHardwareDriver.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
			columnURI.AllowDBNull = false;
			columnURI.MaxLength = 1073741823;
			columnLoginID.AllowDBNull = false;
			columnLoginID.MaxLength = 256;
			columnEncryptedPassword.AllowDBNull = false;
			columnEncryptedPassword.MaxLength = 256;
			columnIDRecorder.AllowDBNull = false;
			columnIDRecorder.MaxLength = 36;
			columnEnabled.AllowDBNull = false;
			columnSettings.MaxLength = 1073741823;
			columnEventSettings.MaxLength = 1073741823;
			columnVendorProductID.MaxLength = 256;
			columnC18NDigest.AllowDBNull = false;
			columnC18NDigest.MaxLength = 44;
			columnDriverNumber.AllowDBNull = false;
			columnDriverProxyName.AllowDBNull = false;
			columnDriverProxyName.MaxLength = 256;
			columnCameraData.MaxLength = int.MaxValue;
			columnEdgeSettings.MaxLength = int.MaxValue;
			columnSubType.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareRow NewHardwareRow()
		{
			return (HardwareRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new HardwareRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(HardwareRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.HardwareRowChanged != null)
			{
				this.HardwareRowChanged(this, new HardwareRowChangeEvent((HardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.HardwareRowChanging != null)
			{
				this.HardwareRowChanging(this, new HardwareRowChangeEvent((HardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.HardwareRowDeleted != null)
			{
				this.HardwareRowDeleted(this, new HardwareRowChangeEvent((HardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.HardwareRowDeleting != null)
			{
				this.HardwareRowDeleting(this, new HardwareRowChangeEvent((HardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveHardwareRow(HardwareRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "HardwareDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class HardwareRow : DataRow
	{
		private HardwareDataTable tableHardware;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableHardware.LastModifiedColumn];
			}
			set
			{
				base[tableHardware.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDHardware
		{
			get
			{
				return (string)base[tableHardware.IDHardwareColumn];
			}
			set
			{
				base[tableHardware.IDHardwareColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDHardwareDriver
		{
			get
			{
				return (string)base[tableHardware.IDHardwareDriverColumn];
			}
			set
			{
				base[tableHardware.IDHardwareDriverColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableHardware.NameColumn];
			}
			set
			{
				base[tableHardware.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableHardware.DescriptionColumn];
			}
			set
			{
				base[tableHardware.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string URI
		{
			get
			{
				return (string)base[tableHardware.URIColumn];
			}
			set
			{
				base[tableHardware.URIColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string LoginID
		{
			get
			{
				return (string)base[tableHardware.LoginIDColumn];
			}
			set
			{
				base[tableHardware.LoginIDColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string EncryptedPassword
		{
			get
			{
				return (string)base[tableHardware.EncryptedPasswordColumn];
			}
			set
			{
				base[tableHardware.EncryptedPasswordColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRecorder
		{
			get
			{
				return (string)base[tableHardware.IDRecorderColumn];
			}
			set
			{
				base[tableHardware.IDRecorderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Enabled
		{
			get
			{
				return (bool)base[tableHardware.EnabledColumn];
			}
			set
			{
				base[tableHardware.EnabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Settings
		{
			get
			{
				try
				{
					return (string)base[tableHardware.SettingsColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Settings' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.SettingsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string EventSettings
		{
			get
			{
				try
				{
					return (string)base[tableHardware.EventSettingsColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'EventSettings' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.EventSettingsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string VendorProductID
		{
			get
			{
				try
				{
					return (string)base[tableHardware.VendorProductIDColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'VendorProductID' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.VendorProductIDColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string C18NDigest
		{
			get
			{
				return (string)base[tableHardware.C18NDigestColumn];
			}
			set
			{
				base[tableHardware.C18NDigestColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int DriverNumber
		{
			get
			{
				return (int)base[tableHardware.DriverNumberColumn];
			}
			set
			{
				base[tableHardware.DriverNumberColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string DriverProxyName
		{
			get
			{
				return (string)base[tableHardware.DriverProxyNameColumn];
			}
			set
			{
				base[tableHardware.DriverProxyNameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int HostedHardware_id
		{
			get
			{
				try
				{
					return (int)base[tableHardware.HostedHardware_idColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'HostedHardware_id' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.HostedHardware_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string CameraData
		{
			get
			{
				try
				{
					return (string)base[tableHardware.CameraDataColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'CameraData' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.CameraDataColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string EdgeSettings
		{
			get
			{
				try
				{
					return (string)base[tableHardware.EdgeSettingsColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'EdgeSettings' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.EdgeSettingsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int SubType
		{
			get
			{
				return (int)base[tableHardware.SubTypeColumn];
			}
			set
			{
				base[tableHardware.SubTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime CredentialsLastModified
		{
			get
			{
				try
				{
					return (DateTime)base[tableHardware.CredentialsLastModifiedColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'CredentialsLastModified' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.CredentialsLastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime HardwareDataLastUpdated
		{
			get
			{
				try
				{
					return (DateTime)base[tableHardware.HardwareDataLastUpdatedColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'HardwareDataLastUpdated' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.HardwareDataLastUpdatedColumn] = value;
			}
		}

		public bool IsEventSettingsNull(DataRowVersion version)
		{
			return IsNull(tableHardware.EventSettingsColumn, version);
		}

		public string GetIDHardware()
		{
			return this.GetColumnValue<string>(tableHardware.IDHardwareColumn);
		}

		public string GetIDHardware(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHardware.IDHardwareColumn, version);
		}

		public string GetName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHardware.NameColumn, version);
		}

		public string GetEncryptedPassword(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHardware.EncryptedPasswordColumn, version);
		}

		public string GetLoginID(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHardware.LoginIDColumn, version);
		}

		public string GetEventSettings(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHardware.EventSettingsColumn, version);
		}

		public string GetDefinitionId(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHardware.C18NDigestColumn, version);
		}

		public string GetURI(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHardware.URIColumn, version);
		}

		public bool GetEnabled(DataRowVersion version)
		{
			return this.GetColumnValue<bool>(tableHardware.EnabledColumn, version);
		}

		public string GetSettings(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHardware.SettingsColumn, version);
		}

		public string ResolveCameraData(string userName, string password)
		{
			return CameraData.Replace("%username%", userName).Replace("%password%", password);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareRow(DataRowBuilder rb)
			: base(rb)
		{
			tableHardware = (HardwareDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsSettingsNull()
		{
			return IsNull(tableHardware.SettingsColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetSettingsNull()
		{
			base[tableHardware.SettingsColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsEventSettingsNull()
		{
			return IsNull(tableHardware.EventSettingsColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetEventSettingsNull()
		{
			base[tableHardware.EventSettingsColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsVendorProductIDNull()
		{
			return IsNull(tableHardware.VendorProductIDColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetVendorProductIDNull()
		{
			base[tableHardware.VendorProductIDColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsHostedHardware_idNull()
		{
			return IsNull(tableHardware.HostedHardware_idColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetHostedHardware_idNull()
		{
			base[tableHardware.HostedHardware_idColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsCameraDataNull()
		{
			return IsNull(tableHardware.CameraDataColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetCameraDataNull()
		{
			base[tableHardware.CameraDataColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsEdgeSettingsNull()
		{
			return IsNull(tableHardware.EdgeSettingsColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetEdgeSettingsNull()
		{
			base[tableHardware.EdgeSettingsColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsCredentialsLastModifiedNull()
		{
			return IsNull(tableHardware.CredentialsLastModifiedColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetCredentialsLastModifiedNull()
		{
			base[tableHardware.CredentialsLastModifiedColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsHardwareDataLastUpdatedNull()
		{
			return IsNull(tableHardware.HardwareDataLastUpdatedColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetHardwareDataLastUpdatedNull()
		{
			base[tableHardware.HardwareDataLastUpdatedColumn] = Convert.DBNull;
		}
	}

	public class HardwareBlockRow : DataRow
	{
		private HardwareBlockDataTable tableHardwareBlock;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int pkid
		{
			get
			{
				return (int)base[tableHardwareBlock.pkidColumn];
			}
			set
			{
				base[tableHardwareBlock.pkidColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string hardware_id
		{
			get
			{
				return (string)base[tableHardwareBlock.hardware_idColumn];
			}
			set
			{
				base[tableHardwareBlock.hardware_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime expires
		{
			get
			{
				return (DateTime)base[tableHardwareBlock.expiresColumn];
			}
			set
			{
				base[tableHardwareBlock.expiresColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string owner
		{
			get
			{
				return (string)base[tableHardwareBlock.ownerColumn];
			}
			set
			{
				base[tableHardwareBlock.ownerColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string data
		{
			get
			{
				return (string)base[tableHardwareBlock.dataColumn];
			}
			set
			{
				base[tableHardwareBlock.dataColumn] = value;
			}
		}

		public int GetId()
		{
			return this.GetColumnValue<int>(tableHardwareBlock.pkidColumn);
		}

		public int GetId(DataRowVersion version)
		{
			return this.GetColumnValue<int>(tableHardwareBlock.pkidColumn, version);
		}

		public string GetHardwareId()
		{
			return this.GetColumnValue<string>(tableHardwareBlock.hardware_idColumn);
		}

		public string GetHardwareId(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHardwareBlock.hardware_idColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareBlockRow(DataRowBuilder rb)
			: base(rb)
		{
			tableHardwareBlock = (HardwareBlockDataTable)base.Table;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class ArchiveStorageDataTable : TypedTableBase<ArchiveStorageRow>
	{
		private DataColumn columnpkid;

		private DataColumn columnrecordingstorage_id;

		private DataColumn columnname;

		private DataColumn columndescription;

		private DataColumn columnpath;

		private DataColumn columnretain_minutes;

		private DataColumn columnmax_size_mb;

		private DataColumn columntarget_framerate;

		private DataColumn columnschedule;

		private DataColumn columncreated_date;

		private DataColumn columnlast_modified;

		private DataColumn columnfolder;

		private DataColumn columnframerate_reduction_enabled;

		private DataColumn columnbank_id;

		private DataColumn columnmounted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn pkidColumn => columnpkid;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recordingstorage_idColumn => columnrecordingstorage_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn nameColumn => columnname;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn descriptionColumn => columndescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn pathColumn => columnpath;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn retain_minutesColumn => columnretain_minutes;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn max_size_mbColumn => columnmax_size_mb;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn target_framerateColumn => columntarget_framerate;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn scheduleColumn => columnschedule;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn created_dateColumn => columncreated_date;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn last_modifiedColumn => columnlast_modified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn folderColumn => columnfolder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn framerate_reduction_enabledColumn => columnframerate_reduction_enabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn bank_idColumn => columnbank_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn mountedColumn => columnmounted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ArchiveStorageRow this[int index] => (ArchiveStorageRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ArchiveStorageRowChangeEventHandler ArchiveStorageRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ArchiveStorageRowChangeEventHandler ArchiveStorageRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ArchiveStorageRowChangeEventHandler ArchiveStorageRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ArchiveStorageRowChangeEventHandler ArchiveStorageRowDeleted;

		public ArchiveStorageRow[] FindByRecordingStorageId(Guid recordingStorageId)
		{
			return FindByRecordingStorageId(recordingStorageId.ToString());
		}

		public ArchiveStorageRow[] FindByRecordingStorageId(string recordingStorageId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnrecordingstorage_id.ColumnName, recordingStorageId);
			return (ArchiveStorageRow[])Select(filterExpression);
		}

		public ArchiveStorageRow FindByBankId(Guid bankId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnbank_id.ColumnName, bankId);
			DataRow[] array = Select(filterExpression);
			if (array.Length == 0)
			{
				return null;
			}
			if (array.Length > 1)
			{
				throw new InvalidConfigurationException("Only one archive storage should exist with a given bank id.");
			}
			return (ArchiveStorageRow)array[0];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ArchiveStorageDataTable()
		{
			base.TableName = "ArchiveStorage";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ArchiveStorageDataTable(DataTable table)
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
		protected ArchiveStorageDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddArchiveStorageRow(ArchiveStorageRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ArchiveStorageRow AddArchiveStorageRow(string pkid, string recordingstorage_id, string name, string description, string path, int retain_minutes, long max_size_mb, double target_framerate, string schedule, DateTime created_date, DateTime last_modified, string folder, bool framerate_reduction_enabled, Guid bank_id, bool mounted)
		{
			ArchiveStorageRow archiveStorageRow = (ArchiveStorageRow)NewRow();
			object[] itemArray = new object[15]
			{
				pkid, recordingstorage_id, name, description, path, retain_minutes, max_size_mb, target_framerate, schedule, created_date,
				last_modified, folder, framerate_reduction_enabled, bank_id, mounted
			};
			archiveStorageRow.ItemArray = itemArray;
			base.Rows.Add(archiveStorageRow);
			return archiveStorageRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ArchiveStorageRow FindBypkid(string pkid)
		{
			return (ArchiveStorageRow)base.Rows.Find(new object[1] { pkid });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			ArchiveStorageDataTable obj = (ArchiveStorageDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new ArchiveStorageDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnpkid = base.Columns["pkid"];
			columnrecordingstorage_id = base.Columns["recordingstorage_id"];
			columnname = base.Columns["name"];
			columndescription = base.Columns["description"];
			columnpath = base.Columns["path"];
			columnretain_minutes = base.Columns["retain_minutes"];
			columnmax_size_mb = base.Columns["max_size_mb"];
			columntarget_framerate = base.Columns["target_framerate"];
			columnschedule = base.Columns["schedule"];
			columncreated_date = base.Columns["created_date"];
			columnlast_modified = base.Columns["last_modified"];
			columnfolder = base.Columns["folder"];
			columnframerate_reduction_enabled = base.Columns["framerate_reduction_enabled"];
			columnbank_id = base.Columns["bank_id"];
			columnmounted = base.Columns["mounted"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnpkid = new DataColumn("pkid", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnpkid);
			columnrecordingstorage_id = new DataColumn("recordingstorage_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnrecordingstorage_id);
			columnname = new DataColumn("name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnname);
			columndescription = new DataColumn("description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columndescription);
			columnpath = new DataColumn("path", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnpath);
			columnretain_minutes = new DataColumn("retain_minutes", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnretain_minutes);
			columnmax_size_mb = new DataColumn("max_size_mb", typeof(long), null, MappingType.Element);
			base.Columns.Add(columnmax_size_mb);
			columntarget_framerate = new DataColumn("target_framerate", typeof(double), null, MappingType.Element);
			base.Columns.Add(columntarget_framerate);
			columnschedule = new DataColumn("schedule", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnschedule);
			columncreated_date = new DataColumn("created_date", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columncreated_date);
			columnlast_modified = new DataColumn("last_modified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnlast_modified);
			columnfolder = new DataColumn("folder", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnfolder);
			columnframerate_reduction_enabled = new DataColumn("framerate_reduction_enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnframerate_reduction_enabled);
			columnbank_id = new DataColumn("bank_id", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnbank_id);
			columnmounted = new DataColumn("mounted", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnmounted);
			base.Constraints.Add(new UniqueConstraint("PK_ArchiveStorage", new DataColumn[1] { columnpkid }, isPrimaryKey: true));
			columnpkid.AllowDBNull = false;
			columnpkid.Unique = true;
			columnpkid.MaxLength = 36;
			columnrecordingstorage_id.AllowDBNull = false;
			columnrecordingstorage_id.MaxLength = 36;
			columnname.AllowDBNull = false;
			columnname.MaxLength = 256;
			columndescription.AllowDBNull = false;
			columndescription.MaxLength = int.MaxValue;
			columnpath.AllowDBNull = false;
			columnpath.MaxLength = 512;
			columnretain_minutes.AllowDBNull = false;
			columnmax_size_mb.AllowDBNull = false;
			columntarget_framerate.AllowDBNull = false;
			columnschedule.AllowDBNull = false;
			columnschedule.MaxLength = int.MaxValue;
			columncreated_date.AllowDBNull = false;
			columnlast_modified.AllowDBNull = false;
			columnfolder.AllowDBNull = false;
			columnfolder.MaxLength = 36;
			columnframerate_reduction_enabled.AllowDBNull = false;
			columnbank_id.AllowDBNull = false;
			columnmounted.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ArchiveStorageRow NewArchiveStorageRow()
		{
			return (ArchiveStorageRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new ArchiveStorageRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(ArchiveStorageRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.ArchiveStorageRowChanged != null)
			{
				this.ArchiveStorageRowChanged(this, new ArchiveStorageRowChangeEvent((ArchiveStorageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.ArchiveStorageRowChanging != null)
			{
				this.ArchiveStorageRowChanging(this, new ArchiveStorageRowChangeEvent((ArchiveStorageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.ArchiveStorageRowDeleted != null)
			{
				this.ArchiveStorageRowDeleted(this, new ArchiveStorageRowChangeEvent((ArchiveStorageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.ArchiveStorageRowDeleting != null)
			{
				this.ArchiveStorageRowDeleting(this, new ArchiveStorageRowChangeEvent((ArchiveStorageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveArchiveStorageRow(ArchiveStorageRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "ArchiveStorageDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class RecordingStorageRow : DataRow
	{
		private RecordingStorageDataTable tableRecordingStorage;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string pkid
		{
			get
			{
				return (string)base[tableRecordingStorage.pkidColumn];
			}
			set
			{
				base[tableRecordingStorage.pkidColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string recordingserver_id
		{
			get
			{
				return (string)base[tableRecordingStorage.recordingserver_idColumn];
			}
			set
			{
				base[tableRecordingStorage.recordingserver_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string name
		{
			get
			{
				return (string)base[tableRecordingStorage.nameColumn];
			}
			set
			{
				base[tableRecordingStorage.nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string description
		{
			get
			{
				return (string)base[tableRecordingStorage.descriptionColumn];
			}
			set
			{
				base[tableRecordingStorage.descriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string path
		{
			get
			{
				return (string)base[tableRecordingStorage.pathColumn];
			}
			set
			{
				base[tableRecordingStorage.pathColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool is_default
		{
			get
			{
				return (bool)base[tableRecordingStorage.is_defaultColumn];
			}
			set
			{
				base[tableRecordingStorage.is_defaultColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int retain_minutes
		{
			get
			{
				return (int)base[tableRecordingStorage.retain_minutesColumn];
			}
			set
			{
				base[tableRecordingStorage.retain_minutesColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public long max_size_mb
		{
			get
			{
				return (long)base[tableRecordingStorage.max_size_mbColumn];
			}
			set
			{
				base[tableRecordingStorage.max_size_mbColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime created_date
		{
			get
			{
				return (DateTime)base[tableRecordingStorage.created_dateColumn];
			}
			set
			{
				base[tableRecordingStorage.created_dateColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime last_modified
		{
			get
			{
				return (DateTime)base[tableRecordingStorage.last_modifiedColumn];
			}
			set
			{
				base[tableRecordingStorage.last_modifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string encryption_method
		{
			get
			{
				return (string)base[tableRecordingStorage.encryption_methodColumn];
			}
			set
			{
				base[tableRecordingStorage.encryption_methodColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string encryption_password
		{
			get
			{
				return (string)base[tableRecordingStorage.encryption_passwordColumn];
			}
			set
			{
				base[tableRecordingStorage.encryption_passwordColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string encryption_password_new
		{
			get
			{
				return (string)base[tableRecordingStorage.encryption_password_newColumn];
			}
			set
			{
				base[tableRecordingStorage.encryption_password_newColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool signing
		{
			get
			{
				return (bool)base[tableRecordingStorage.signingColumn];
			}
			set
			{
				base[tableRecordingStorage.signingColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid bank_id
		{
			get
			{
				return (Guid)base[tableRecordingStorage.bank_idColumn];
			}
			set
			{
				base[tableRecordingStorage.bank_idColumn] = value;
			}
		}

		public string GetRecordingServerId(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecordingStorage.recordingserver_idColumn, version);
		}

		public bool IsMaxSizeMBChanged()
		{
			long num = (long)base[tableRecordingStorage.max_size_mbColumn, DataRowVersion.Original];
			long num2 = (long)base[tableRecordingStorage.max_size_mbColumn, DataRowVersion.Current];
			return num != num2;
		}

		public bool IsRetainMinutesChanged()
		{
			int num = (int)base[tableRecordingStorage.retain_minutesColumn, DataRowVersion.Original];
			int num2 = (int)base[tableRecordingStorage.retain_minutesColumn, DataRowVersion.Current];
			return num != num2;
		}

		public bool IsNameChanged()
		{
			string a = (string)base[tableRecordingStorage.nameColumn, DataRowVersion.Original];
			string b = (string)base[tableRecordingStorage.nameColumn, DataRowVersion.Current];
			return !string.Equals(a, b, StringComparison.Ordinal);
		}

		public bool IsSigningChanged()
		{
			bool num = (bool)base[tableRecordingStorage.signingColumn, DataRowVersion.Original];
			bool flag = (bool)base[tableRecordingStorage.signingColumn, DataRowVersion.Current];
			return num != flag;
		}

		public bool IsEncryptionMethodChanged()
		{
			string a = (string)base[tableRecordingStorage.encryption_methodColumn, DataRowVersion.Original];
			string b = (string)base[tableRecordingStorage.encryption_methodColumn, DataRowVersion.Current];
			return !string.Equals(a, b, StringComparison.Ordinal);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRecordingStorage = (RecordingStorageDataTable)base.Table;
		}
	}

	public class ArchiveStorageRow : DataRow
	{
		private ArchiveStorageDataTable tableArchiveStorage;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string pkid
		{
			get
			{
				return (string)base[tableArchiveStorage.pkidColumn];
			}
			set
			{
				base[tableArchiveStorage.pkidColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string recordingstorage_id
		{
			get
			{
				return (string)base[tableArchiveStorage.recordingstorage_idColumn];
			}
			set
			{
				base[tableArchiveStorage.recordingstorage_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string name
		{
			get
			{
				return (string)base[tableArchiveStorage.nameColumn];
			}
			set
			{
				base[tableArchiveStorage.nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string description
		{
			get
			{
				return (string)base[tableArchiveStorage.descriptionColumn];
			}
			set
			{
				base[tableArchiveStorage.descriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string path
		{
			get
			{
				return (string)base[tableArchiveStorage.pathColumn];
			}
			set
			{
				base[tableArchiveStorage.pathColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int retain_minutes
		{
			get
			{
				return (int)base[tableArchiveStorage.retain_minutesColumn];
			}
			set
			{
				base[tableArchiveStorage.retain_minutesColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public long max_size_mb
		{
			get
			{
				return (long)base[tableArchiveStorage.max_size_mbColumn];
			}
			set
			{
				base[tableArchiveStorage.max_size_mbColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public double target_framerate
		{
			get
			{
				return (double)base[tableArchiveStorage.target_framerateColumn];
			}
			set
			{
				base[tableArchiveStorage.target_framerateColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string schedule
		{
			get
			{
				return (string)base[tableArchiveStorage.scheduleColumn];
			}
			set
			{
				base[tableArchiveStorage.scheduleColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime created_date
		{
			get
			{
				return (DateTime)base[tableArchiveStorage.created_dateColumn];
			}
			set
			{
				base[tableArchiveStorage.created_dateColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime last_modified
		{
			get
			{
				return (DateTime)base[tableArchiveStorage.last_modifiedColumn];
			}
			set
			{
				base[tableArchiveStorage.last_modifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string folder
		{
			get
			{
				return (string)base[tableArchiveStorage.folderColumn];
			}
			set
			{
				base[tableArchiveStorage.folderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool framerate_reduction_enabled
		{
			get
			{
				return (bool)base[tableArchiveStorage.framerate_reduction_enabledColumn];
			}
			set
			{
				base[tableArchiveStorage.framerate_reduction_enabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid bank_id
		{
			get
			{
				return (Guid)base[tableArchiveStorage.bank_idColumn];
			}
			set
			{
				base[tableArchiveStorage.bank_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool mounted
		{
			get
			{
				return (bool)base[tableArchiveStorage.mountedColumn];
			}
			set
			{
				base[tableArchiveStorage.mountedColumn] = value;
			}
		}

		public string GetRecordingStorageId(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableArchiveStorage.recordingstorage_idColumn, version);
		}

		public ArchiveData CreateArchiveData()
		{
			return new ArchiveData
			{
				Path = path,
				Folder = folder,
				Schedule = ScheduleData.FromXml(schedule),
				Cleanup = new CleanupData
				{
					MaxMinutes = retain_minutes,
					MaxSize = max_size_mb
				}
			};
		}

		public bool IsFramerateReductionEnabledChanged()
		{
			bool num = (bool)base[tableArchiveStorage.framerate_reduction_enabledColumn, DataRowVersion.Original];
			bool flag = (bool)base[tableArchiveStorage.framerate_reduction_enabledColumn, DataRowVersion.Current];
			return num != flag;
		}

		public bool IsMaxSizeMBChanged()
		{
			long num = (long)base[tableArchiveStorage.max_size_mbColumn, DataRowVersion.Original];
			long num2 = (long)base[tableArchiveStorage.max_size_mbColumn, DataRowVersion.Current];
			return num != num2;
		}

		public bool IsRetainMinutesChanged()
		{
			int num = (int)base[tableArchiveStorage.retain_minutesColumn, DataRowVersion.Original];
			int num2 = (int)base[tableArchiveStorage.retain_minutesColumn, DataRowVersion.Current];
			return num != num2;
		}

		public bool IsScheduleChanged()
		{
			string a = (string)base[tableArchiveStorage.scheduleColumn, DataRowVersion.Original];
			string b = (string)base[tableArchiveStorage.scheduleColumn, DataRowVersion.Current];
			return !string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
		}

		public bool IsTargetFramerateChanged()
		{
			double num = (double)base[tableArchiveStorage.target_framerateColumn, DataRowVersion.Original];
			double obj = (double)base[tableArchiveStorage.target_framerateColumn, DataRowVersion.Current];
			return !num.Equals(obj);
		}

		public bool IsNameChanged()
		{
			string a = (string)base[tableArchiveStorage.nameColumn, DataRowVersion.Original];
			string b = (string)base[tableArchiveStorage.nameColumn, DataRowVersion.Current];
			return !string.Equals(a, b, StringComparison.Ordinal);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ArchiveStorageRow(DataRowBuilder rb)
			: base(rb)
		{
			tableArchiveStorage = (ArchiveStorageDataTable)base.Table;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class RecordingStorageDeviceDataTable : TypedTableBase<RecordingStorageDeviceRow>
	{
		private DataColumn columnrecordingstorage_id;

		private DataColumn columndevice_id;

		private DataColumn columnrecording_enabled;

		private DataColumn columnrecording_framerate;

		private DataColumn columnprebuffer_enabled;

		private DataColumn columnprebuffer_seconds;

		private DataColumn columnprebuffer_in_memory;

		private DataColumn columnedgestorage_enabled;

		private DataColumn columnedgestorage_playback;

		private DataColumn columnrecord_keyframes;

		private DataColumn columnlast_modified;

		private DataColumn columnmanual_timeout_enabled;

		private DataColumn columnmanual_timeout;

		private DataColumn columnrecord_on_related_devices;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recordingstorage_idColumn => columnrecordingstorage_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn device_idColumn => columndevice_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recording_enabledColumn => columnrecording_enabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recording_framerateColumn => columnrecording_framerate;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn prebuffer_enabledColumn => columnprebuffer_enabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn prebuffer_secondsColumn => columnprebuffer_seconds;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn prebuffer_in_memoryColumn => columnprebuffer_in_memory;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn edgestorage_enabledColumn => columnedgestorage_enabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn edgestorage_playbackColumn => columnedgestorage_playback;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn record_keyframesColumn => columnrecord_keyframes;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn last_modifiedColumn => columnlast_modified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn manual_timeout_enabledColumn => columnmanual_timeout_enabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn manual_timeoutColumn => columnmanual_timeout;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn record_on_related_devicesColumn => columnrecord_on_related_devices;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDeviceRow this[int index] => (RecordingStorageDeviceRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingStorageDeviceRowChangeEventHandler RecordingStorageDeviceRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingStorageDeviceRowChangeEventHandler RecordingStorageDeviceRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingStorageDeviceRowChangeEventHandler RecordingStorageDeviceRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingStorageDeviceRowChangeEventHandler RecordingStorageDeviceRowDeleted;

		public RecordingStorageDeviceRow FindByIDDevice(Guid deviceId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columndevice_id.ColumnName, deviceId);
			DataRow[] array = Select(filterExpression);
			if (array.Length == 0)
			{
				return null;
			}
			if (array.Length > 1)
			{
				throw new InvalidConfigurationException("A device should only be associated with one recording storage.");
			}
			return (RecordingStorageDeviceRow)array[0];
		}

		public RecordingStorageDeviceRow[] FindByRecordingStorageId(Guid recordingStorageId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnrecordingstorage_id.ColumnName, recordingStorageId);
			return (RecordingStorageDeviceRow[])Select(filterExpression);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDeviceDataTable()
		{
			base.TableName = "RecordingStorageDevice";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDeviceDataTable(DataTable table)
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
		protected RecordingStorageDeviceDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRecordingStorageDeviceRow(RecordingStorageDeviceRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDeviceRow AddRecordingStorageDeviceRow(string recordingstorage_id, string device_id, bool recording_enabled, double recording_framerate, bool prebuffer_enabled, int prebuffer_seconds, bool prebuffer_in_memory, bool edgestorage_enabled, bool edgestorage_playback, bool record_keyframes, DateTime last_modified, bool manual_timeout_enabled, int manual_timeout, bool record_on_related_devices)
		{
			RecordingStorageDeviceRow recordingStorageDeviceRow = (RecordingStorageDeviceRow)NewRow();
			object[] itemArray = new object[14]
			{
				recordingstorage_id, device_id, recording_enabled, recording_framerate, prebuffer_enabled, prebuffer_seconds, prebuffer_in_memory, edgestorage_enabled, edgestorage_playback, record_keyframes,
				last_modified, manual_timeout_enabled, manual_timeout, record_on_related_devices
			};
			recordingStorageDeviceRow.ItemArray = itemArray;
			base.Rows.Add(recordingStorageDeviceRow);
			return recordingStorageDeviceRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDeviceRow FindBydevice_id(string device_id)
		{
			return (RecordingStorageDeviceRow)base.Rows.Find(new object[1] { device_id });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RecordingStorageDeviceDataTable obj = (RecordingStorageDeviceDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RecordingStorageDeviceDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnrecordingstorage_id = base.Columns["recordingstorage_id"];
			columndevice_id = base.Columns["device_id"];
			columnrecording_enabled = base.Columns["recording_enabled"];
			columnrecording_framerate = base.Columns["recording_framerate"];
			columnprebuffer_enabled = base.Columns["prebuffer_enabled"];
			columnprebuffer_seconds = base.Columns["prebuffer_seconds"];
			columnprebuffer_in_memory = base.Columns["prebuffer_in_memory"];
			columnedgestorage_enabled = base.Columns["edgestorage_enabled"];
			columnedgestorage_playback = base.Columns["edgestorage_playback"];
			columnrecord_keyframes = base.Columns["record_keyframes"];
			columnlast_modified = base.Columns["last_modified"];
			columnmanual_timeout_enabled = base.Columns["manual_timeout_enabled"];
			columnmanual_timeout = base.Columns["manual_timeout"];
			columnrecord_on_related_devices = base.Columns["record_on_related_devices"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnrecordingstorage_id = new DataColumn("recordingstorage_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnrecordingstorage_id);
			columndevice_id = new DataColumn("device_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columndevice_id);
			columnrecording_enabled = new DataColumn("recording_enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnrecording_enabled);
			columnrecording_framerate = new DataColumn("recording_framerate", typeof(double), null, MappingType.Element);
			base.Columns.Add(columnrecording_framerate);
			columnprebuffer_enabled = new DataColumn("prebuffer_enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnprebuffer_enabled);
			columnprebuffer_seconds = new DataColumn("prebuffer_seconds", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnprebuffer_seconds);
			columnprebuffer_in_memory = new DataColumn("prebuffer_in_memory", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnprebuffer_in_memory);
			columnedgestorage_enabled = new DataColumn("edgestorage_enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnedgestorage_enabled);
			columnedgestorage_playback = new DataColumn("edgestorage_playback", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnedgestorage_playback);
			columnrecord_keyframes = new DataColumn("record_keyframes", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnrecord_keyframes);
			columnlast_modified = new DataColumn("last_modified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnlast_modified);
			columnmanual_timeout_enabled = new DataColumn("manual_timeout_enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnmanual_timeout_enabled);
			columnmanual_timeout = new DataColumn("manual_timeout", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnmanual_timeout);
			columnrecord_on_related_devices = new DataColumn("record_on_related_devices", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnrecord_on_related_devices);
			base.Constraints.Add(new UniqueConstraint("PK_RecordingStorageDevice", new DataColumn[1] { columndevice_id }, isPrimaryKey: true));
			columnrecordingstorage_id.AllowDBNull = false;
			columnrecordingstorage_id.MaxLength = 36;
			columndevice_id.AllowDBNull = false;
			columndevice_id.Unique = true;
			columndevice_id.MaxLength = 36;
			columnrecording_enabled.AllowDBNull = false;
			columnrecording_framerate.AllowDBNull = false;
			columnprebuffer_enabled.AllowDBNull = false;
			columnprebuffer_seconds.AllowDBNull = false;
			columnprebuffer_in_memory.AllowDBNull = false;
			columnedgestorage_enabled.AllowDBNull = false;
			columnedgestorage_playback.AllowDBNull = false;
			columnrecord_keyframes.AllowDBNull = false;
			columnlast_modified.AllowDBNull = false;
			columnmanual_timeout_enabled.AllowDBNull = false;
			columnmanual_timeout.AllowDBNull = false;
			columnrecord_on_related_devices.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDeviceRow NewRecordingStorageDeviceRow()
		{
			return (RecordingStorageDeviceRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RecordingStorageDeviceRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RecordingStorageDeviceRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RecordingStorageDeviceRowChanged != null)
			{
				this.RecordingStorageDeviceRowChanged(this, new RecordingStorageDeviceRowChangeEvent((RecordingStorageDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RecordingStorageDeviceRowChanging != null)
			{
				this.RecordingStorageDeviceRowChanging(this, new RecordingStorageDeviceRowChangeEvent((RecordingStorageDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RecordingStorageDeviceRowDeleted != null)
			{
				this.RecordingStorageDeviceRowDeleted(this, new RecordingStorageDeviceRowChangeEvent((RecordingStorageDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RecordingStorageDeviceRowDeleting != null)
			{
				this.RecordingStorageDeviceRowDeleting(this, new RecordingStorageDeviceRowChangeEvent((RecordingStorageDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRecordingStorageDeviceRow(RecordingStorageDeviceRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RecordingStorageDeviceDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class RecordingStorageDeviceRow : DataRow
	{
		private RecordingStorageDeviceDataTable tableRecordingStorageDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string recordingstorage_id
		{
			get
			{
				return (string)base[tableRecordingStorageDevice.recordingstorage_idColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.recordingstorage_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string device_id
		{
			get
			{
				return (string)base[tableRecordingStorageDevice.device_idColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.device_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool recording_enabled
		{
			get
			{
				return (bool)base[tableRecordingStorageDevice.recording_enabledColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.recording_enabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public double recording_framerate
		{
			get
			{
				return (double)base[tableRecordingStorageDevice.recording_framerateColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.recording_framerateColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool prebuffer_enabled
		{
			get
			{
				return (bool)base[tableRecordingStorageDevice.prebuffer_enabledColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.prebuffer_enabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int prebuffer_seconds
		{
			get
			{
				return (int)base[tableRecordingStorageDevice.prebuffer_secondsColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.prebuffer_secondsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool prebuffer_in_memory
		{
			get
			{
				return (bool)base[tableRecordingStorageDevice.prebuffer_in_memoryColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.prebuffer_in_memoryColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool edgestorage_enabled
		{
			get
			{
				return (bool)base[tableRecordingStorageDevice.edgestorage_enabledColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.edgestorage_enabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool edgestorage_playback
		{
			get
			{
				return (bool)base[tableRecordingStorageDevice.edgestorage_playbackColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.edgestorage_playbackColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool record_keyframes
		{
			get
			{
				return (bool)base[tableRecordingStorageDevice.record_keyframesColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.record_keyframesColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime last_modified
		{
			get
			{
				return (DateTime)base[tableRecordingStorageDevice.last_modifiedColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.last_modifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool manual_timeout_enabled
		{
			get
			{
				return (bool)base[tableRecordingStorageDevice.manual_timeout_enabledColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.manual_timeout_enabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int manual_timeout
		{
			get
			{
				return (int)base[tableRecordingStorageDevice.manual_timeoutColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.manual_timeoutColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool record_on_related_devices
		{
			get
			{
				return (bool)base[tableRecordingStorageDevice.record_on_related_devicesColumn];
			}
			set
			{
				base[tableRecordingStorageDevice.record_on_related_devicesColumn] = value;
			}
		}

		public string GetRecordingStorageId(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecordingStorageDevice.recordingstorage_idColumn, version);
		}

		public string GetDeviceId(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecordingStorageDevice.device_idColumn, version);
		}

		public bool IsRecordingEnabledChanged()
		{
			bool num = (bool)base[tableRecordingStorageDevice.recording_enabledColumn, DataRowVersion.Original];
			bool flag = (bool)base[tableRecordingStorageDevice.recording_enabledColumn, DataRowVersion.Current];
			return num != flag;
		}

		public bool IsRecordingFramerateChanged()
		{
			double num = (double)base[tableRecordingStorageDevice.recording_framerateColumn, DataRowVersion.Original];
			double num2 = (double)base[tableRecordingStorageDevice.recording_framerateColumn, DataRowVersion.Current];
			return num != num2;
		}

		public bool IsPrebufferEnabledChanged()
		{
			bool num = (bool)base[tableRecordingStorageDevice.prebuffer_enabledColumn, DataRowVersion.Original];
			bool flag = (bool)base[tableRecordingStorageDevice.prebuffer_enabledColumn, DataRowVersion.Current];
			return num != flag;
		}

		public bool IsPrebufferInMemoryChanged()
		{
			bool num = (bool)base[tableRecordingStorageDevice.prebuffer_in_memoryColumn, DataRowVersion.Original];
			bool flag = (bool)base[tableRecordingStorageDevice.prebuffer_in_memoryColumn, DataRowVersion.Current];
			return num != flag;
		}

		public bool IsPrebufferSecondsChanged()
		{
			int num = (int)base[tableRecordingStorageDevice.prebuffer_secondsColumn, DataRowVersion.Original];
			int num2 = (int)base[tableRecordingStorageDevice.prebuffer_secondsColumn, DataRowVersion.Current];
			return num != num2;
		}

		public bool IsEdgeStorageEnabledChanged()
		{
			bool num = (bool)base[tableRecordingStorageDevice.edgestorage_enabledColumn, DataRowVersion.Original];
			bool flag = (bool)base[tableRecordingStorageDevice.edgestorage_enabledColumn, DataRowVersion.Current];
			return num != flag;
		}

		public bool IsEdgeStoragePlaybackChanged()
		{
			bool num = (bool)base[tableRecordingStorageDevice.edgestorage_playbackColumn, DataRowVersion.Original];
			bool flag = (bool)base[tableRecordingStorageDevice.edgestorage_playbackColumn, DataRowVersion.Current];
			return num != flag;
		}

		public bool IsRecordKeyframesOnlyChanged()
		{
			bool num = (bool)base[tableRecordingStorageDevice.record_keyframesColumn, DataRowVersion.Original];
			bool flag = (bool)base[tableRecordingStorageDevice.record_keyframesColumn, DataRowVersion.Current];
			return num != flag;
		}

		public bool IsManualTimeoutEnabledChanged()
		{
			bool num = (bool)base[tableRecordingStorageDevice.manual_timeout_enabledColumn, DataRowVersion.Original];
			bool flag = (bool)base[tableRecordingStorageDevice.manual_timeout_enabledColumn, DataRowVersion.Current];
			return num != flag;
		}

		public bool IsManualTimeoutMinutesChanged()
		{
			int num = (int)base[tableRecordingStorageDevice.manual_timeoutColumn, DataRowVersion.Original];
			int num2 = (int)base[tableRecordingStorageDevice.manual_timeoutColumn, DataRowVersion.Current];
			return num != num2;
		}

		public bool IsRecordingStorageIdChanged()
		{
			string a = base[tableRecordingStorageDevice.recordingstorage_idColumn, DataRowVersion.Original] as string;
			string b = base[tableRecordingStorageDevice.recordingstorage_idColumn, DataRowVersion.Current] as string;
			return !string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
		}

		public bool IsRecordOnRelatedDevicesChanged()
		{
			bool num = (bool)base[tableRecordingStorageDevice.record_on_related_devicesColumn, DataRowVersion.Original];
			bool flag = (bool)base[tableRecordingStorageDevice.record_on_related_devicesColumn, DataRowVersion.Current];
			return num != flag;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDeviceRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRecordingStorageDevice = (RecordingStorageDeviceDataTable)base.Table;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class RecordingStorageDataTable : TypedTableBase<RecordingStorageRow>
	{
		private DataColumn columnpkid;

		private DataColumn columnrecordingserver_id;

		private DataColumn columnname;

		private DataColumn columndescription;

		private DataColumn columnpath;

		private DataColumn columnis_default;

		private DataColumn columnretain_minutes;

		private DataColumn columnmax_size_mb;

		private DataColumn columncreated_date;

		private DataColumn columnlast_modified;

		private DataColumn columnencryption_method;

		private DataColumn columnencryption_password;

		private DataColumn columnencryption_password_new;

		private DataColumn columnsigning;

		private DataColumn columnbank_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn pkidColumn => columnpkid;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recordingserver_idColumn => columnrecordingserver_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn nameColumn => columnname;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn descriptionColumn => columndescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn pathColumn => columnpath;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn is_defaultColumn => columnis_default;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn retain_minutesColumn => columnretain_minutes;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn max_size_mbColumn => columnmax_size_mb;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn created_dateColumn => columncreated_date;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn last_modifiedColumn => columnlast_modified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn encryption_methodColumn => columnencryption_method;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn encryption_passwordColumn => columnencryption_password;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn encryption_password_newColumn => columnencryption_password_new;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn signingColumn => columnsigning;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn bank_idColumn => columnbank_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageRow this[int index] => (RecordingStorageRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingStorageRowChangeEventHandler RecordingStorageRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingStorageRowChangeEventHandler RecordingStorageRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingStorageRowChangeEventHandler RecordingStorageRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingStorageRowChangeEventHandler RecordingStorageRowDeleted;

		public RecordingStorageRow FindByRecordingStorageId(Guid recordingStorageId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnpkid.ColumnName, recordingStorageId);
			DataRow[] array = Select(filterExpression);
			if (array.Length == 0)
			{
				return null;
			}
			if (array.Length > 1)
			{
				throw new InvalidConfigurationException("Only one recording storage should exist with a given id.");
			}
			return (RecordingStorageRow)array[0];
		}

		public RecordingStorageRow FindByBankId(Guid bankId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnbank_id.ColumnName, bankId);
			DataRow[] array = Select(filterExpression);
			if (array.Length == 0)
			{
				return null;
			}
			if (array.Length > 1)
			{
				throw new InvalidConfigurationException("Only one recording storage should exist with a given bank id.");
			}
			return (RecordingStorageRow)array[0];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDataTable()
		{
			base.TableName = "RecordingStorage";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDataTable(DataTable table)
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
		protected RecordingStorageDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRecordingStorageRow(RecordingStorageRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageRow AddRecordingStorageRow(string pkid, string recordingserver_id, string name, string description, string path, bool is_default, int retain_minutes, long max_size_mb, DateTime created_date, DateTime last_modified, string encryption_method, string encryption_password, string encryption_password_new, bool signing, Guid bank_id)
		{
			RecordingStorageRow recordingStorageRow = (RecordingStorageRow)NewRow();
			object[] itemArray = new object[15]
			{
				pkid, recordingserver_id, name, description, path, is_default, retain_minutes, max_size_mb, created_date, last_modified,
				encryption_method, encryption_password, encryption_password_new, signing, bank_id
			};
			recordingStorageRow.ItemArray = itemArray;
			base.Rows.Add(recordingStorageRow);
			return recordingStorageRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageRow FindBypkid(string pkid)
		{
			return (RecordingStorageRow)base.Rows.Find(new object[1] { pkid });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RecordingStorageDataTable obj = (RecordingStorageDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RecordingStorageDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnpkid = base.Columns["pkid"];
			columnrecordingserver_id = base.Columns["recordingserver_id"];
			columnname = base.Columns["name"];
			columndescription = base.Columns["description"];
			columnpath = base.Columns["path"];
			columnis_default = base.Columns["is_default"];
			columnretain_minutes = base.Columns["retain_minutes"];
			columnmax_size_mb = base.Columns["max_size_mb"];
			columncreated_date = base.Columns["created_date"];
			columnlast_modified = base.Columns["last_modified"];
			columnencryption_method = base.Columns["encryption_method"];
			columnencryption_password = base.Columns["encryption_password"];
			columnencryption_password_new = base.Columns["encryption_password_new"];
			columnsigning = base.Columns["signing"];
			columnbank_id = base.Columns["bank_id"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnpkid = new DataColumn("pkid", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnpkid);
			columnrecordingserver_id = new DataColumn("recordingserver_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnrecordingserver_id);
			columnname = new DataColumn("name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnname);
			columndescription = new DataColumn("description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columndescription);
			columnpath = new DataColumn("path", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnpath);
			columnis_default = new DataColumn("is_default", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnis_default);
			columnretain_minutes = new DataColumn("retain_minutes", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnretain_minutes);
			columnmax_size_mb = new DataColumn("max_size_mb", typeof(long), null, MappingType.Element);
			base.Columns.Add(columnmax_size_mb);
			columncreated_date = new DataColumn("created_date", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columncreated_date);
			columnlast_modified = new DataColumn("last_modified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnlast_modified);
			columnencryption_method = new DataColumn("encryption_method", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnencryption_method);
			columnencryption_password = new DataColumn("encryption_password", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnencryption_password);
			columnencryption_password_new = new DataColumn("encryption_password_new", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnencryption_password_new);
			columnsigning = new DataColumn("signing", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnsigning);
			columnbank_id = new DataColumn("bank_id", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnbank_id);
			base.Constraints.Add(new UniqueConstraint("PK_RecordingStorage", new DataColumn[1] { columnpkid }, isPrimaryKey: true));
			columnpkid.AllowDBNull = false;
			columnpkid.Unique = true;
			columnpkid.MaxLength = 36;
			columnrecordingserver_id.AllowDBNull = false;
			columnrecordingserver_id.MaxLength = 36;
			columnname.AllowDBNull = false;
			columnname.MaxLength = 256;
			columndescription.AllowDBNull = false;
			columndescription.MaxLength = int.MaxValue;
			columnpath.AllowDBNull = false;
			columnpath.MaxLength = 512;
			columnis_default.AllowDBNull = false;
			columnretain_minutes.AllowDBNull = false;
			columnmax_size_mb.AllowDBNull = false;
			columncreated_date.AllowDBNull = false;
			columnlast_modified.AllowDBNull = false;
			columnencryption_method.AllowDBNull = false;
			columnencryption_method.MaxLength = 100;
			columnencryption_password.AllowDBNull = false;
			columnencryption_password.MaxLength = int.MaxValue;
			columnencryption_password_new.AllowDBNull = false;
			columnencryption_password_new.MaxLength = int.MaxValue;
			columnsigning.AllowDBNull = false;
			columnbank_id.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageRow NewRecordingStorageRow()
		{
			return (RecordingStorageRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RecordingStorageRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RecordingStorageRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RecordingStorageRowChanged != null)
			{
				this.RecordingStorageRowChanged(this, new RecordingStorageRowChangeEvent((RecordingStorageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RecordingStorageRowChanging != null)
			{
				this.RecordingStorageRowChanging(this, new RecordingStorageRowChangeEvent((RecordingStorageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RecordingStorageRowDeleted != null)
			{
				this.RecordingStorageRowDeleted(this, new RecordingStorageRowChangeEvent((RecordingStorageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RecordingStorageRowDeleting != null)
			{
				this.RecordingStorageRowDeleting(this, new RecordingStorageRowChangeEvent((RecordingStorageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRecordingStorageRow(RecordingStorageRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RecordingStorageDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class SystemSettingsRow : DataRow
	{
		private SystemSettingsDataTable tableSystemSettings;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableSystemSettings.LastModifiedColumn];
			}
			set
			{
				base[tableSystemSettings.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableSystemSettings.NameColumn];
			}
			set
			{
				base[tableSystemSettings.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Setting
		{
			get
			{
				return (string)base[tableSystemSettings.SettingColumn];
			}
			set
			{
				base[tableSystemSettings.SettingColumn] = value;
			}
		}

		public string GetName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableSystemSettings.NameColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SystemSettingsRow(DataRowBuilder rb)
			: base(rb)
		{
			tableSystemSettings = (SystemSettingsDataTable)base.Table;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class SystemSettingsDataTable : TypedTableBase<SystemSettingsRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnName;

		private DataColumn columnSetting;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SettingColumn => columnSetting;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SystemSettingsRow this[int index] => (SystemSettingsRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SystemSettingsRowChangeEventHandler SystemSettingsRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SystemSettingsRowChangeEventHandler SystemSettingsRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SystemSettingsRowChangeEventHandler SystemSettingsRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event SystemSettingsRowChangeEventHandler SystemSettingsRowDeleted;

		public SystemSettingsRow FindByNameSafe(string settingName)
		{
			foreach (SystemSettingsRow row in base.Rows)
			{
				if (row.RowState == DataRowState.Deleted)
				{
					if (string.Equals(row.GetName(DataRowVersion.Original), settingName, StringComparison.OrdinalIgnoreCase))
					{
						return row;
					}
				}
				else if (string.Equals(row.Name, settingName, StringComparison.OrdinalIgnoreCase))
				{
					return row;
				}
			}
			return null;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SystemSettingsDataTable()
		{
			base.TableName = "SystemSettings";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SystemSettingsDataTable(DataTable table)
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
		protected SystemSettingsDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddSystemSettingsRow(SystemSettingsRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SystemSettingsRow AddSystemSettingsRow(DateTime LastModified, string Name, string Setting)
		{
			SystemSettingsRow systemSettingsRow = (SystemSettingsRow)NewRow();
			object[] itemArray = new object[3] { LastModified, Name, Setting };
			systemSettingsRow.ItemArray = itemArray;
			base.Rows.Add(systemSettingsRow);
			return systemSettingsRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SystemSettingsRow FindByName(string Name)
		{
			return (SystemSettingsRow)base.Rows.Find(new object[1] { Name });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			SystemSettingsDataTable obj = (SystemSettingsDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new SystemSettingsDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnName = base.Columns["Name"];
			columnSetting = base.Columns["Setting"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnSetting = new DataColumn("Setting", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSetting);
			base.Constraints.Add(new UniqueConstraint("PK_SystemSettings", new DataColumn[1] { columnName }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnName.AllowDBNull = false;
			columnName.Unique = true;
			columnName.MaxLength = 256;
			columnSetting.AllowDBNull = false;
			columnSetting.MaxLength = 1073741823;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SystemSettingsRow NewSystemSettingsRow()
		{
			return (SystemSettingsRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new SystemSettingsRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(SystemSettingsRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.SystemSettingsRowChanged != null)
			{
				this.SystemSettingsRowChanged(this, new SystemSettingsRowChangeEvent((SystemSettingsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.SystemSettingsRowChanging != null)
			{
				this.SystemSettingsRowChanging(this, new SystemSettingsRowChangeEvent((SystemSettingsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.SystemSettingsRowDeleted != null)
			{
				this.SystemSettingsRowDeleted(this, new SystemSettingsRowChangeEvent((SystemSettingsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.SystemSettingsRowDeleting != null)
			{
				this.SystemSettingsRowDeleting(this, new SystemSettingsRowChangeEvent((SystemSettingsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveSystemSettingsRow(SystemSettingsRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "SystemSettingsDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class NotificationSettingsDataTable : TypedTableBase<NotificationSettingsRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDNotificationSetting;

		private DataColumn columnSettings;

		private DataColumn columnIDNotificationType;

		private DataColumn columnIDNotificationProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDNotificationSettingColumn => columnIDNotificationSetting;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SettingsColumn => columnSettings;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDNotificationTypeColumn => columnIDNotificationType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDNotificationProfileColumn => columnIDNotificationProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationSettingsRow this[int index] => (NotificationSettingsRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationSettingsRowChangeEventHandler NotificationSettingsRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationSettingsRowChangeEventHandler NotificationSettingsRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationSettingsRowChangeEventHandler NotificationSettingsRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationSettingsRowChangeEventHandler NotificationSettingsRowDeleted;

		public NotificationSettingsRow[] FindByNotificationProfile(Guid notificationProfileId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnIDNotificationProfile.ColumnName, notificationProfileId);
			return (NotificationSettingsRow[])Select(filterExpression);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationSettingsDataTable()
		{
			base.TableName = "NotificationSettings";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationSettingsDataTable(DataTable table)
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
		protected NotificationSettingsDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddNotificationSettingsRow(NotificationSettingsRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationSettingsRow AddNotificationSettingsRow(DateTime LastModified, string IDNotificationSetting, string Settings, string IDNotificationType, string IDNotificationProfile)
		{
			NotificationSettingsRow notificationSettingsRow = (NotificationSettingsRow)NewRow();
			object[] itemArray = new object[5] { LastModified, IDNotificationSetting, Settings, IDNotificationType, IDNotificationProfile };
			notificationSettingsRow.ItemArray = itemArray;
			base.Rows.Add(notificationSettingsRow);
			return notificationSettingsRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationSettingsRow FindByIDNotificationSetting(string IDNotificationSetting)
		{
			return (NotificationSettingsRow)base.Rows.Find(new object[1] { IDNotificationSetting });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			NotificationSettingsDataTable obj = (NotificationSettingsDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new NotificationSettingsDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDNotificationSetting = base.Columns["IDNotificationSetting"];
			columnSettings = base.Columns["Settings"];
			columnIDNotificationType = base.Columns["IDNotificationType"];
			columnIDNotificationProfile = base.Columns["IDNotificationProfile"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDNotificationSetting = new DataColumn("IDNotificationSetting", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDNotificationSetting);
			columnSettings = new DataColumn("Settings", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSettings);
			columnIDNotificationType = new DataColumn("IDNotificationType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDNotificationType);
			columnIDNotificationProfile = new DataColumn("IDNotificationProfile", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDNotificationProfile);
			base.Constraints.Add(new UniqueConstraint("PK_NotificationSettings", new DataColumn[1] { columnIDNotificationSetting }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDNotificationSetting.AllowDBNull = false;
			columnIDNotificationSetting.Unique = true;
			columnIDNotificationSetting.MaxLength = 36;
			columnSettings.MaxLength = 1073741823;
			columnIDNotificationType.AllowDBNull = false;
			columnIDNotificationType.MaxLength = 36;
			columnIDNotificationProfile.AllowDBNull = false;
			columnIDNotificationProfile.MaxLength = 36;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationSettingsRow NewNotificationSettingsRow()
		{
			return (NotificationSettingsRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new NotificationSettingsRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(NotificationSettingsRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.NotificationSettingsRowChanged != null)
			{
				this.NotificationSettingsRowChanged(this, new NotificationSettingsRowChangeEvent((NotificationSettingsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.NotificationSettingsRowChanging != null)
			{
				this.NotificationSettingsRowChanging(this, new NotificationSettingsRowChangeEvent((NotificationSettingsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.NotificationSettingsRowDeleted != null)
			{
				this.NotificationSettingsRowDeleted(this, new NotificationSettingsRowChangeEvent((NotificationSettingsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.NotificationSettingsRowDeleting != null)
			{
				this.NotificationSettingsRowDeleting(this, new NotificationSettingsRowChangeEvent((NotificationSettingsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveNotificationSettingsRow(NotificationSettingsRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "NotificationSettingsDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class MatrixRow : DataRow
	{
		private MatrixDataTable tableMatrix;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDMatrix
		{
			get
			{
				return (string)base[tableMatrix.IDMatrixColumn];
			}
			set
			{
				base[tableMatrix.IDMatrixColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableMatrix.NameColumn];
			}
			set
			{
				base[tableMatrix.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableMatrix.DescriptionColumn];
			}
			set
			{
				base[tableMatrix.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Address
		{
			get
			{
				return (string)base[tableMatrix.AddressColumn];
			}
			set
			{
				base[tableMatrix.AddressColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int Port
		{
			get
			{
				return (int)base[tableMatrix.PortColumn];
			}
			set
			{
				base[tableMatrix.PortColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Password
		{
			get
			{
				try
				{
					return (string)base[tableMatrix.PasswordColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Password' in table 'Matrix' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableMatrix.PasswordColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastUpdated
		{
			get
			{
				return (DateTime)base[tableMatrix.LastUpdatedColumn];
			}
			set
			{
				base[tableMatrix.LastUpdatedColumn] = value;
			}
		}

		public string GetIDMatrix(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableMatrix.IDMatrixColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MatrixRow(DataRowBuilder rb)
			: base(rb)
		{
			tableMatrix = (MatrixDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsPasswordNull()
		{
			return IsNull(tableMatrix.PasswordColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetPasswordNull()
		{
			base[tableMatrix.PasswordColumn] = Convert.DBNull;
		}
	}

	public class PatrollingProfilesRow : DataRow
	{
		private PatrollingProfilesDataTable tablePatrollingProfiles;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tablePatrollingProfiles.LastModifiedColumn];
			}
			set
			{
				base[tablePatrollingProfiles.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDPatrollingProfile
		{
			get
			{
				return (string)base[tablePatrollingProfiles.IDPatrollingProfileColumn];
			}
			set
			{
				base[tablePatrollingProfiles.IDPatrollingProfileColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDevice
		{
			get
			{
				return (string)base[tablePatrollingProfiles.IDDeviceColumn];
			}
			set
			{
				base[tablePatrollingProfiles.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tablePatrollingProfiles.NameColumn];
			}
			set
			{
				base[tablePatrollingProfiles.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tablePatrollingProfiles.DescriptionColumn];
			}
			set
			{
				base[tablePatrollingProfiles.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Definition
		{
			get
			{
				return (string)base[tablePatrollingProfiles.DefinitionColumn];
			}
			set
			{
				base[tablePatrollingProfiles.DefinitionColumn] = value;
			}
		}

		public string GetIDDevice(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tablePatrollingProfiles.IDDeviceColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public PatrollingProfilesRow(DataRowBuilder rb)
			: base(rb)
		{
			tablePatrollingProfiles = (PatrollingProfilesDataTable)base.Table;
		}
	}

	public class RecorderPluginDeviceRow : DataRow
	{
		private RecorderPluginDeviceDataTable tableRecorderPluginDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableRecorderPluginDevice.LastModifiedColumn];
			}
			set
			{
				base[tableRecorderPluginDevice.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDevice
		{
			get
			{
				return (string)base[tableRecorderPluginDevice.IDDeviceColumn];
			}
			set
			{
				base[tableRecorderPluginDevice.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int ProcessOrder
		{
			get
			{
				return (int)base[tableRecorderPluginDevice.ProcessOrderColumn];
			}
			set
			{
				base[tableRecorderPluginDevice.ProcessOrderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRecorderPlugin
		{
			get
			{
				return (string)base[tableRecorderPluginDevice.IDRecorderPluginColumn];
			}
			set
			{
				base[tableRecorderPluginDevice.IDRecorderPluginColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Settings
		{
			get
			{
				return (string)base[tableRecorderPluginDevice.SettingsColumn];
			}
			set
			{
				base[tableRecorderPluginDevice.SettingsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Enabled
		{
			get
			{
				return (bool)base[tableRecorderPluginDevice.EnabledColumn];
			}
			set
			{
				base[tableRecorderPluginDevice.EnabledColumn] = value;
			}
		}

		public string GetIDDevice()
		{
			return this.GetColumnValue<string>(tableRecorderPluginDevice.IDDeviceColumn);
		}

		public string GetIDDevice(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecorderPluginDevice.IDDeviceColumn, version);
		}

		public string GetIDRecorderPlugin(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecorderPluginDevice.IDRecorderPluginColumn, version);
		}

		public bool GetEnabled(DataRowVersion version)
		{
			return this.GetColumnValue<bool>(tableRecorderPluginDevice.EnabledColumn, version);
		}

		public string GetSettings(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecorderPluginDevice.SettingsColumn, version);
		}

		public bool IsSettingsChanged()
		{
			string settings = GetSettings(DataRowVersion.Original);
			string settings2 = GetSettings(DataRowVersion.Current);
			return !string.Equals(settings, settings2, StringComparison.OrdinalIgnoreCase);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginDeviceRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRecorderPluginDevice = (RecorderPluginDeviceDataTable)base.Table;
		}
	}

	public class RulesRow : DataRow
	{
		private RulesDataTable tableRules;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableRules.LastModifiedColumn];
			}
			set
			{
				base[tableRules.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRule
		{
			get
			{
				return (string)base[tableRules.IDRuleColumn];
			}
			set
			{
				base[tableRules.IDRuleColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableRules.NameColumn];
			}
			set
			{
				base[tableRules.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableRules.DescriptionColumn];
			}
			set
			{
				base[tableRules.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Definition
		{
			get
			{
				return (string)base[tableRules.DefinitionColumn];
			}
			set
			{
				base[tableRules.DefinitionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Enabled
		{
			get
			{
				return (bool)base[tableRules.EnabledColumn];
			}
			set
			{
				base[tableRules.EnabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int Priority
		{
			get
			{
				return (int)base[tableRules.PriorityColumn];
			}
			set
			{
				base[tableRules.PriorityColumn] = value;
			}
		}

		public string GetIDRule(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRules.IDRuleColumn, version);
		}

		public string GetName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRules.NameColumn, version);
		}

		public bool GetEnabled(DataRowVersion version)
		{
			return this.GetColumnValue<bool>(tableRules.EnabledColumn, version);
		}

		public int GetPriority(DataRowVersion version)
		{
			return this.GetColumnValue<int>(tableRules.PriorityColumn, version);
		}

		public string GetDefinition(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRules.DefinitionColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RulesRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRules = (RulesDataTable)base.Table;
		}
	}

	public class AudioMessageRow : DataRow
	{
		private AudioMessageDataTable tableAudioMessage;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableAudioMessage.LastModifiedColumn];
			}
			set
			{
				base[tableAudioMessage.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string ID
		{
			get
			{
				return (string)base[tableAudioMessage.IDColumn];
			}
			set
			{
				base[tableAudioMessage.IDColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableAudioMessage.NameColumn];
			}
			set
			{
				base[tableAudioMessage.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableAudioMessage.DescriptionColumn];
			}
			set
			{
				base[tableAudioMessage.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string OriginalFileName
		{
			get
			{
				return (string)base[tableAudioMessage.OriginalFileNameColumn];
			}
			set
			{
				base[tableAudioMessage.OriginalFileNameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public byte[] Content
		{
			get
			{
				return (byte[])base[tableAudioMessage.ContentColumn];
			}
			set
			{
				base[tableAudioMessage.ContentColumn] = value;
			}
		}

		public string GetID()
		{
			return this.GetColumnValue<string>(tableAudioMessage.IDColumn);
		}

		public string GetID(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableAudioMessage.IDColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public AudioMessageRow(DataRowBuilder rb)
			: base(rb)
		{
			tableAudioMessage = (AudioMessageDataTable)base.Table;
		}
	}

	public class TimeProfileRow : DataRow
	{
		private TimeProfileDataTable tableTimeProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string name
		{
			get
			{
				return (string)base[tableTimeProfile.nameColumn];
			}
			set
			{
				base[tableTimeProfile.nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string description
		{
			get
			{
				try
				{
					return (string)base[tableTimeProfile.descriptionColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'description' in table 'TimeProfile' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableTimeProfile.descriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string type
		{
			get
			{
				return (string)base[tableTimeProfile.typeColumn];
			}
			set
			{
				base[tableTimeProfile.typeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string pkid
		{
			get
			{
				return (string)base[tableTimeProfile.pkidColumn];
			}
			set
			{
				base[tableTimeProfile.pkidColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string xml_data
		{
			get
			{
				return (string)base[tableTimeProfile.xml_dataColumn];
			}
			set
			{
				base[tableTimeProfile.xml_dataColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime created_date
		{
			get
			{
				return (DateTime)base[tableTimeProfile.created_dateColumn];
			}
			set
			{
				base[tableTimeProfile.created_dateColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime last_modified
		{
			get
			{
				return (DateTime)base[tableTimeProfile.last_modifiedColumn];
			}
			set
			{
				base[tableTimeProfile.last_modifiedColumn] = value;
			}
		}

		public string GetPkid(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableTimeProfile.pkidColumn, version);
		}

		public string GetName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableTimeProfile.nameColumn, version);
		}

		public string GetDefinition(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableTimeProfile.xml_dataColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileRow(DataRowBuilder rb)
			: base(rb)
		{
			tableTimeProfile = (TimeProfileDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsdescriptionNull()
		{
			return IsNull(tableTimeProfile.descriptionColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetdescriptionNull()
		{
			base[tableTimeProfile.descriptionColumn] = Convert.DBNull;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class RecordingServerServiceDataTable : TypedTableBase<RecordingServerServiceRow>
	{
		private DataColumn columnlast_modified;

		private DataColumn columnpkid;

		private DataColumn columnhost_name;

		private DataColumn columnscan_ranges;

		private DataColumn columndevicepack_info;

		private DataColumn columnfailover_udp_port;

		private DataColumn columnx64;

		private DataColumn columntime_zone_name;

		private DataColumn columndatabase_port;

		private DataColumn columncertificate_configured;

		private DataColumn columnwebserver_hostname;

		private DataColumn columnwebserver_port;

		private DataColumn columnversion;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn last_modifiedColumn => columnlast_modified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn pkidColumn => columnpkid;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn host_nameColumn => columnhost_name;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn scan_rangesColumn => columnscan_ranges;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn devicepack_infoColumn => columndevicepack_info;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn failover_udp_portColumn => columnfailover_udp_port;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn x64Column => columnx64;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn time_zone_nameColumn => columntime_zone_name;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn database_portColumn => columndatabase_port;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn certificate_configuredColumn => columncertificate_configured;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn webserver_hostnameColumn => columnwebserver_hostname;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn webserver_portColumn => columnwebserver_port;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn versionColumn => columnversion;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRow this[int index] => (RecordingServerServiceRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceRowChangeEventHandler RecordingServerServiceRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceRowChangeEventHandler RecordingServerServiceRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceRowChangeEventHandler RecordingServerServiceRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceRowChangeEventHandler RecordingServerServiceRowDeleted;

		public RecordingServerServiceRow FindById(Guid id)
		{
			return FindById(id.ToString());
		}

		public RecordingServerServiceRow FindById(string id)
		{
			return (RecordingServerServiceRow)base.Rows.Find(new object[1] { id });
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceDataTable()
		{
			base.TableName = "RecordingServerService";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceDataTable(DataTable table)
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
		protected RecordingServerServiceDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRecordingServerServiceRow(RecordingServerServiceRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRow AddRecordingServerServiceRow(DateTime last_modified, string pkid, string host_name, string scan_ranges, string devicepack_info, int failover_udp_port, bool x64, string time_zone_name, int database_port, bool certificate_configured, string webserver_hostname, int webserver_port, string version)
		{
			RecordingServerServiceRow recordingServerServiceRow = (RecordingServerServiceRow)NewRow();
			object[] itemArray = new object[13]
			{
				last_modified, pkid, host_name, scan_ranges, devicepack_info, failover_udp_port, x64, time_zone_name, database_port, certificate_configured,
				webserver_hostname, webserver_port, version
			};
			recordingServerServiceRow.ItemArray = itemArray;
			base.Rows.Add(recordingServerServiceRow);
			return recordingServerServiceRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRow FindBypkid(string pkid)
		{
			return (RecordingServerServiceRow)base.Rows.Find(new object[1] { pkid });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RecordingServerServiceDataTable obj = (RecordingServerServiceDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RecordingServerServiceDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnlast_modified = base.Columns["last_modified"];
			columnpkid = base.Columns["pkid"];
			columnhost_name = base.Columns["host_name"];
			columnscan_ranges = base.Columns["scan_ranges"];
			columndevicepack_info = base.Columns["devicepack_info"];
			columnfailover_udp_port = base.Columns["failover_udp_port"];
			columnx64 = base.Columns["x64"];
			columntime_zone_name = base.Columns["time_zone_name"];
			columndatabase_port = base.Columns["database_port"];
			columncertificate_configured = base.Columns["certificate_configured"];
			columnwebserver_hostname = base.Columns["webserver_hostname"];
			columnwebserver_port = base.Columns["webserver_port"];
			columnversion = base.Columns["version"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnlast_modified = new DataColumn("last_modified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnlast_modified);
			columnpkid = new DataColumn("pkid", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnpkid);
			columnhost_name = new DataColumn("host_name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnhost_name);
			columnscan_ranges = new DataColumn("scan_ranges", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnscan_ranges);
			columndevicepack_info = new DataColumn("devicepack_info", typeof(string), null, MappingType.Element);
			base.Columns.Add(columndevicepack_info);
			columnfailover_udp_port = new DataColumn("failover_udp_port", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnfailover_udp_port);
			columnx64 = new DataColumn("x64", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnx64);
			columntime_zone_name = new DataColumn("time_zone_name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columntime_zone_name);
			columndatabase_port = new DataColumn("database_port", typeof(int), null, MappingType.Element);
			base.Columns.Add(columndatabase_port);
			columncertificate_configured = new DataColumn("certificate_configured", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columncertificate_configured);
			columnwebserver_hostname = new DataColumn("webserver_hostname", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnwebserver_hostname);
			columnwebserver_port = new DataColumn("webserver_port", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnwebserver_port);
			columnversion = new DataColumn("version", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnversion);
			base.Constraints.Add(new UniqueConstraint("PK_RecordingServerService", new DataColumn[1] { columnpkid }, isPrimaryKey: true));
			columnlast_modified.AllowDBNull = false;
			columnpkid.AllowDBNull = false;
			columnpkid.Unique = true;
			columnpkid.MaxLength = 36;
			columnhost_name.AllowDBNull = false;
			columnhost_name.MaxLength = 256;
			columnscan_ranges.MaxLength = int.MaxValue;
			columndevicepack_info.MaxLength = int.MaxValue;
			columnx64.AllowDBNull = false;
			columntime_zone_name.AllowDBNull = false;
			columntime_zone_name.MaxLength = 256;
			columndatabase_port.AllowDBNull = false;
			columncertificate_configured.AllowDBNull = false;
			columnwebserver_hostname.AllowDBNull = false;
			columnwebserver_hostname.MaxLength = 256;
			columnwebserver_port.AllowDBNull = false;
			columnversion.AllowDBNull = false;
			columnversion.MaxLength = 256;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRow NewRecordingServerServiceRow()
		{
			return (RecordingServerServiceRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RecordingServerServiceRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RecordingServerServiceRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RecordingServerServiceRowChanged != null)
			{
				this.RecordingServerServiceRowChanged(this, new RecordingServerServiceRowChangeEvent((RecordingServerServiceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RecordingServerServiceRowChanging != null)
			{
				this.RecordingServerServiceRowChanging(this, new RecordingServerServiceRowChangeEvent((RecordingServerServiceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RecordingServerServiceRowDeleted != null)
			{
				this.RecordingServerServiceRowDeleted(this, new RecordingServerServiceRowChangeEvent((RecordingServerServiceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RecordingServerServiceRowDeleting != null)
			{
				this.RecordingServerServiceRowDeleting(this, new RecordingServerServiceRowChangeEvent((RecordingServerServiceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRecordingServerServiceRow(RecordingServerServiceRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RecordingServerServiceDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class RecordingServerServiceRow : DataRow
	{
		private RecordingServerServiceDataTable tableRecordingServerService;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime last_modified
		{
			get
			{
				return (DateTime)base[tableRecordingServerService.last_modifiedColumn];
			}
			set
			{
				base[tableRecordingServerService.last_modifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string pkid
		{
			get
			{
				return (string)base[tableRecordingServerService.pkidColumn];
			}
			set
			{
				base[tableRecordingServerService.pkidColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string host_name
		{
			get
			{
				return (string)base[tableRecordingServerService.host_nameColumn];
			}
			set
			{
				base[tableRecordingServerService.host_nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string scan_ranges
		{
			get
			{
				try
				{
					return (string)base[tableRecordingServerService.scan_rangesColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'scan_ranges' in table 'RecordingServerService' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableRecordingServerService.scan_rangesColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string devicepack_info
		{
			get
			{
				try
				{
					return (string)base[tableRecordingServerService.devicepack_infoColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'devicepack_info' in table 'RecordingServerService' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableRecordingServerService.devicepack_infoColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int failover_udp_port
		{
			get
			{
				try
				{
					return (int)base[tableRecordingServerService.failover_udp_portColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'failover_udp_port' in table 'RecordingServerService' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableRecordingServerService.failover_udp_portColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool x64
		{
			get
			{
				return (bool)base[tableRecordingServerService.x64Column];
			}
			set
			{
				base[tableRecordingServerService.x64Column] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string time_zone_name
		{
			get
			{
				return (string)base[tableRecordingServerService.time_zone_nameColumn];
			}
			set
			{
				base[tableRecordingServerService.time_zone_nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int database_port
		{
			get
			{
				return (int)base[tableRecordingServerService.database_portColumn];
			}
			set
			{
				base[tableRecordingServerService.database_portColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool certificate_configured
		{
			get
			{
				return (bool)base[tableRecordingServerService.certificate_configuredColumn];
			}
			set
			{
				base[tableRecordingServerService.certificate_configuredColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string webserver_hostname
		{
			get
			{
				return (string)base[tableRecordingServerService.webserver_hostnameColumn];
			}
			set
			{
				base[tableRecordingServerService.webserver_hostnameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int webserver_port
		{
			get
			{
				return (int)base[tableRecordingServerService.webserver_portColumn];
			}
			set
			{
				base[tableRecordingServerService.webserver_portColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string version
		{
			get
			{
				return (string)base[tableRecordingServerService.versionColumn];
			}
			set
			{
				base[tableRecordingServerService.versionColumn] = value;
			}
		}

		public bool IsFailoverUdpPortChanged()
		{
			int? num = null;
			try
			{
				num = (int)base[tableRecordingServerService.failover_udp_portColumn, DataRowVersion.Original];
			}
			catch (InvalidCastException)
			{
			}
			int? num2 = null;
			try
			{
				num2 = (int)base[tableRecordingServerService.failover_udp_portColumn, DataRowVersion.Current];
			}
			catch (InvalidCastException)
			{
			}
			return num != num2;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRecordingServerService = (RecordingServerServiceDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Isscan_rangesNull()
		{
			return IsNull(tableRecordingServerService.scan_rangesColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void Setscan_rangesNull()
		{
			base[tableRecordingServerService.scan_rangesColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Isdevicepack_infoNull()
		{
			return IsNull(tableRecordingServerService.devicepack_infoColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void Setdevicepack_infoNull()
		{
			base[tableRecordingServerService.devicepack_infoColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Isfailover_udp_portNull()
		{
			return IsNull(tableRecordingServerService.failover_udp_portColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void Setfailover_udp_portNull()
		{
			base[tableRecordingServerService.failover_udp_portColumn] = Convert.DBNull;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class RecordingServerServiceRecorderDataTable : TypedTableBase<RecordingServerServiceRecorderRow>
	{
		private DataColumn columnlast_modified;

		private DataColumn columnrecorder_id;

		private DataColumn columntype;

		private DataColumn columnrecording_server_service_id;

		private DataColumn columnactive;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn last_modifiedColumn => columnlast_modified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recorder_idColumn => columnrecorder_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn typeColumn => columntype;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recording_server_service_idColumn => columnrecording_server_service_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn activeColumn => columnactive;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRecorderRow this[int index] => (RecordingServerServiceRecorderRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceRecorderRowChangeEventHandler RecordingServerServiceRecorderRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceRecorderRowChangeEventHandler RecordingServerServiceRecorderRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceRecorderRowChangeEventHandler RecordingServerServiceRecorderRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceRecorderRowChangeEventHandler RecordingServerServiceRecorderRowDeleted;

		public RecordingServerServiceRecorderRow[] FindByRecorderId(Guid recorderId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnrecorder_id.ColumnName, recorderId);
			return (RecordingServerServiceRecorderRow[])Select(filterExpression);
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRecorderDataTable()
		{
			base.TableName = "RecordingServerServiceRecorder";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRecorderDataTable(DataTable table)
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
		protected RecordingServerServiceRecorderDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRecordingServerServiceRecorderRow(RecordingServerServiceRecorderRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRecorderRow AddRecordingServerServiceRecorderRow(DateTime last_modified, string recorder_id, int type, string recording_server_service_id, bool active)
		{
			RecordingServerServiceRecorderRow recordingServerServiceRecorderRow = (RecordingServerServiceRecorderRow)NewRow();
			object[] itemArray = new object[5] { last_modified, recorder_id, type, recording_server_service_id, active };
			recordingServerServiceRecorderRow.ItemArray = itemArray;
			base.Rows.Add(recordingServerServiceRecorderRow);
			return recordingServerServiceRecorderRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRecorderRow FindByrecording_server_service_id(string recording_server_service_id)
		{
			return (RecordingServerServiceRecorderRow)base.Rows.Find(new object[1] { recording_server_service_id });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RecordingServerServiceRecorderDataTable obj = (RecordingServerServiceRecorderDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RecordingServerServiceRecorderDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnlast_modified = base.Columns["last_modified"];
			columnrecorder_id = base.Columns["recorder_id"];
			columntype = base.Columns["type"];
			columnrecording_server_service_id = base.Columns["recording_server_service_id"];
			columnactive = base.Columns["active"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnlast_modified = new DataColumn("last_modified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnlast_modified);
			columnrecorder_id = new DataColumn("recorder_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnrecorder_id);
			columntype = new DataColumn("type", typeof(int), null, MappingType.Element);
			base.Columns.Add(columntype);
			columnrecording_server_service_id = new DataColumn("recording_server_service_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnrecording_server_service_id);
			columnactive = new DataColumn("active", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnactive);
			base.Constraints.Add(new UniqueConstraint("PK_RecordingServerServiceRecorder", new DataColumn[1] { columnrecording_server_service_id }, isPrimaryKey: true));
			columnlast_modified.AllowDBNull = false;
			columnrecorder_id.AllowDBNull = false;
			columnrecorder_id.MaxLength = 36;
			columntype.AllowDBNull = false;
			columnrecording_server_service_id.AllowDBNull = false;
			columnrecording_server_service_id.Unique = true;
			columnrecording_server_service_id.MaxLength = 36;
			columnactive.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRecorderRow NewRecordingServerServiceRecorderRow()
		{
			return (RecordingServerServiceRecorderRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RecordingServerServiceRecorderRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RecordingServerServiceRecorderRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RecordingServerServiceRecorderRowChanged != null)
			{
				this.RecordingServerServiceRecorderRowChanged(this, new RecordingServerServiceRecorderRowChangeEvent((RecordingServerServiceRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RecordingServerServiceRecorderRowChanging != null)
			{
				this.RecordingServerServiceRecorderRowChanging(this, new RecordingServerServiceRecorderRowChangeEvent((RecordingServerServiceRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RecordingServerServiceRecorderRowDeleted != null)
			{
				this.RecordingServerServiceRecorderRowDeleted(this, new RecordingServerServiceRecorderRowChangeEvent((RecordingServerServiceRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RecordingServerServiceRecorderRowDeleting != null)
			{
				this.RecordingServerServiceRecorderRowDeleting(this, new RecordingServerServiceRecorderRowChangeEvent((RecordingServerServiceRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRecordingServerServiceRecorderRow(RecordingServerServiceRecorderRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RecordingServerServiceRecorderDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class RecorderInfoDataTable : TypedTableBase<RecorderInfoRow>
	{
		private DataColumn columnrecorder_id;

		private DataColumn columnrecording_server_service_id;

		private DataColumn columnhost_name;

		private DataColumn columndatabase_port;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recorder_idColumn => columnrecorder_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recording_server_service_idColumn => columnrecording_server_service_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn host_nameColumn => columnhost_name;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn database_portColumn => columndatabase_port;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderInfoRow this[int index] => (RecorderInfoRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderInfoRowChangeEventHandler RecorderInfoRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderInfoRowChangeEventHandler RecorderInfoRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderInfoRowChangeEventHandler RecorderInfoRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderInfoRowChangeEventHandler RecorderInfoRowDeleted;

		public RecorderInfoRow FindByRecorderId(Guid recorderId)
		{
			string filterExpression = string.Format(CultureInfo.InvariantCulture, "{0} = '{1}'", columnrecorder_id.ColumnName, recorderId);
			return (RecorderInfoRow)Select(filterExpression).FirstOrDefault();
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderInfoDataTable()
		{
			base.TableName = "RecorderInfo";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderInfoDataTable(DataTable table)
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
		protected RecorderInfoDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRecorderInfoRow(RecorderInfoRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderInfoRow AddRecorderInfoRow(string recorder_id, string recording_server_service_id, string host_name, int database_port)
		{
			RecorderInfoRow recorderInfoRow = (RecorderInfoRow)NewRow();
			object[] itemArray = new object[4] { recorder_id, recording_server_service_id, host_name, database_port };
			recorderInfoRow.ItemArray = itemArray;
			base.Rows.Add(recorderInfoRow);
			return recorderInfoRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderInfoRow FindByrecording_server_service_id(string recording_server_service_id)
		{
			return (RecorderInfoRow)base.Rows.Find(new object[1] { recording_server_service_id });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RecorderInfoDataTable obj = (RecorderInfoDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RecorderInfoDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnrecorder_id = base.Columns["recorder_id"];
			columnrecording_server_service_id = base.Columns["recording_server_service_id"];
			columnhost_name = base.Columns["host_name"];
			columndatabase_port = base.Columns["database_port"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnrecorder_id = new DataColumn("recorder_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnrecorder_id);
			columnrecording_server_service_id = new DataColumn("recording_server_service_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnrecording_server_service_id);
			columnhost_name = new DataColumn("host_name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnhost_name);
			columndatabase_port = new DataColumn("database_port", typeof(int), null, MappingType.Element);
			base.Columns.Add(columndatabase_port);
			base.Constraints.Add(new UniqueConstraint("PK_RecorderInfo", new DataColumn[1] { columnrecording_server_service_id }, isPrimaryKey: true));
			columnrecorder_id.AllowDBNull = false;
			columnrecorder_id.MaxLength = 36;
			columnrecording_server_service_id.AllowDBNull = false;
			columnrecording_server_service_id.Unique = true;
			columnrecording_server_service_id.MaxLength = 36;
			columnhost_name.AllowDBNull = false;
			columnhost_name.MaxLength = 256;
			columndatabase_port.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderInfoRow NewRecorderInfoRow()
		{
			return (RecorderInfoRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RecorderInfoRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RecorderInfoRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RecorderInfoRowChanged != null)
			{
				this.RecorderInfoRowChanged(this, new RecorderInfoRowChangeEvent((RecorderInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RecorderInfoRowChanging != null)
			{
				this.RecorderInfoRowChanging(this, new RecorderInfoRowChangeEvent((RecorderInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RecorderInfoRowDeleted != null)
			{
				this.RecorderInfoRowDeleted(this, new RecorderInfoRowChangeEvent((RecorderInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RecorderInfoRowDeleting != null)
			{
				this.RecorderInfoRowDeleting(this, new RecorderInfoRowChangeEvent((RecorderInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRecorderInfoRow(RecorderInfoRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RecorderInfoDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class RecorderInfoRow : DataRow
	{
		private RecorderInfoDataTable tableRecorderInfo;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string recorder_id
		{
			get
			{
				return (string)base[tableRecorderInfo.recorder_idColumn];
			}
			set
			{
				base[tableRecorderInfo.recorder_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string recording_server_service_id
		{
			get
			{
				return (string)base[tableRecorderInfo.recording_server_service_idColumn];
			}
			set
			{
				base[tableRecorderInfo.recording_server_service_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string host_name
		{
			get
			{
				return (string)base[tableRecorderInfo.host_nameColumn];
			}
			set
			{
				base[tableRecorderInfo.host_nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int database_port
		{
			get
			{
				return (int)base[tableRecorderInfo.database_portColumn];
			}
			set
			{
				base[tableRecorderInfo.database_portColumn] = value;
			}
		}

		public string GetRecorderId()
		{
			return this.GetColumnValue<string>(tableRecorderInfo.recorder_idColumn);
		}

		public string GetRecorderId(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecorderInfo.recorder_idColumn, version);
		}

		public string GetHostName()
		{
			return this.GetColumnValue<string>(tableRecorderInfo.host_nameColumn);
		}

		public string GetHostName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecorderInfo.host_nameColumn, version);
		}

		public int GetDatabasePort()
		{
			return this.GetColumnValue<int>(tableRecorderInfo.database_portColumn);
		}

		public int GetDatabasePort(DataRowVersion version)
		{
			return this.GetColumnValue<int>(tableRecorderInfo.database_portColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderInfoRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRecorderInfo = (RecorderInfoDataTable)base.Table;
		}
	}

	public class RecordersRow : DataRow
	{
		private RecordersDataTable tableRecorders;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableRecorders.LastModifiedColumn];
			}
			set
			{
				base[tableRecorders.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRecorder
		{
			get
			{
				return (string)base[tableRecorders.IDRecorderColumn];
			}
			set
			{
				base[tableRecorders.IDRecorderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Enabled
		{
			get
			{
				return (bool)base[tableRecorders.EnabledColumn];
			}
			set
			{
				base[tableRecorders.EnabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableRecorders.NameColumn];
			}
			set
			{
				base[tableRecorders.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableRecorders.DescriptionColumn];
			}
			set
			{
				base[tableRecorders.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDFailoverPrimaryGroup
		{
			get
			{
				try
				{
					return (string)base[tableRecorders.IDFailoverPrimaryGroupColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'IDFailoverPrimaryGroup' in table 'Recorders' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableRecorders.IDFailoverPrimaryGroupColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDFailoverSecondaryGroup
		{
			get
			{
				try
				{
					return (string)base[tableRecorders.IDFailoverSecondaryGroupColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'IDFailoverSecondaryGroup' in table 'Recorders' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableRecorders.IDFailoverSecondaryGroupColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string MulticastSettings
		{
			get
			{
				return (string)base[tableRecorders.MulticastSettingsColumn];
			}
			set
			{
				base[tableRecorders.MulticastSettingsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int SynchronizationTime
		{
			get
			{
				return (int)base[tableRecorders.SynchronizationTimeColumn];
			}
			set
			{
				base[tableRecorders.SynchronizationTimeColumn] = value;
			}
		}

		public string GetIdRecorder(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecorders.IDRecorderColumn, version);
		}

		public string GetName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecorders.NameColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRecorders = (RecordersDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsIDFailoverPrimaryGroupNull()
		{
			return IsNull(tableRecorders.IDFailoverPrimaryGroupColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetIDFailoverPrimaryGroupNull()
		{
			base[tableRecorders.IDFailoverPrimaryGroupColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsIDFailoverSecondaryGroupNull()
		{
			return IsNull(tableRecorders.IDFailoverSecondaryGroupColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetIDFailoverSecondaryGroupNull()
		{
			base[tableRecorders.IDFailoverSecondaryGroupColumn] = Convert.DBNull;
		}
	}

	public class RecordingServerServiceConfigurableRow : DataRow
	{
		private RecordingServerServiceConfigurableDataTable tableRecordingServerServiceConfigurable;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableRecordingServerServiceConfigurable.LastModifiedColumn];
			}
			set
			{
				base[tableRecordingServerServiceConfigurable.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string PKID
		{
			get
			{
				return (string)base[tableRecordingServerServiceConfigurable.PKIDColumn];
			}
			set
			{
				base[tableRecordingServerServiceConfigurable.PKIDColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string MulticastServerAddress
		{
			get
			{
				return (string)base[tableRecordingServerServiceConfigurable.MulticastServerAddressColumn];
			}
			set
			{
				base[tableRecordingServerServiceConfigurable.MulticastServerAddressColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool ShutdownOnStorageFailure
		{
			get
			{
				return (bool)base[tableRecordingServerServiceConfigurable.ShutdownOnStorageFailureColumn];
			}
			set
			{
				base[tableRecordingServerServiceConfigurable.ShutdownOnStorageFailureColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool PublicAccessEnabled
		{
			get
			{
				return (bool)base[tableRecordingServerServiceConfigurable.PublicAccessEnabledColumn];
			}
			set
			{
				base[tableRecordingServerServiceConfigurable.PublicAccessEnabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string PublicWebServerHostname
		{
			get
			{
				return (string)base[tableRecordingServerServiceConfigurable.PublicWebServerHostnameColumn];
			}
			set
			{
				base[tableRecordingServerServiceConfigurable.PublicWebServerHostnameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int PublicWebServerPort
		{
			get
			{
				return (int)base[tableRecordingServerServiceConfigurable.PublicWebServerPortColumn];
			}
			set
			{
				base[tableRecordingServerServiceConfigurable.PublicWebServerPortColumn] = value;
			}
		}

		public string GetPKID()
		{
			return this.GetColumnValue<string>(tableRecordingServerServiceConfigurable.PKIDColumn);
		}

		public string GetPKID(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableRecordingServerServiceConfigurable.PKIDColumn, version);
		}

		public bool GetShutdownOnStorageFailure(DataRowVersion version)
		{
			return this.GetColumnValue<bool>(tableRecordingServerServiceConfigurable.ShutdownOnStorageFailureColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceConfigurableRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRecordingServerServiceConfigurable = (RecordingServerServiceConfigurableDataTable)base.Table;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class HostingServerDataTable : TypedTableBase<HostingServerRow>
	{
		private DataColumn columnpkid;

		private DataColumn columncreated_date;

		private DataColumn columnlast_modified;

		private DataColumn columntype;

		private DataColumn columnname;

		private DataColumn columndescription;

		private DataColumn columnuser_name;

		private DataColumn columnpassword;

		private DataColumn columninternal_address;

		private DataColumn columnexternal_address;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn pkidColumn => columnpkid;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn created_dateColumn => columncreated_date;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn last_modifiedColumn => columnlast_modified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn typeColumn => columntype;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn nameColumn => columnname;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn descriptionColumn => columndescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn user_nameColumn => columnuser_name;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn passwordColumn => columnpassword;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn internal_addressColumn => columninternal_address;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn external_addressColumn => columnexternal_address;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostingServerRow this[int index] => (HostingServerRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HostingServerRowChangeEventHandler HostingServerRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HostingServerRowChangeEventHandler HostingServerRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HostingServerRowChangeEventHandler HostingServerRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HostingServerRowChangeEventHandler HostingServerRowDeleted;

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostingServerDataTable()
		{
			base.TableName = "HostingServer";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostingServerDataTable(DataTable table)
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
		protected HostingServerDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddHostingServerRow(HostingServerRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostingServerRow AddHostingServerRow(DateTime created_date, DateTime last_modified, string type, string name, string description, string user_name, string password, string internal_address, string external_address)
		{
			HostingServerRow hostingServerRow = (HostingServerRow)NewRow();
			object[] itemArray = new object[10] { null, created_date, last_modified, type, name, description, user_name, password, internal_address, external_address };
			hostingServerRow.ItemArray = itemArray;
			base.Rows.Add(hostingServerRow);
			return hostingServerRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostingServerRow FindBypkid(int pkid)
		{
			return (HostingServerRow)base.Rows.Find(new object[1] { pkid });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			HostingServerDataTable obj = (HostingServerDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new HostingServerDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnpkid = base.Columns["pkid"];
			columncreated_date = base.Columns["created_date"];
			columnlast_modified = base.Columns["last_modified"];
			columntype = base.Columns["type"];
			columnname = base.Columns["name"];
			columndescription = base.Columns["description"];
			columnuser_name = base.Columns["user_name"];
			columnpassword = base.Columns["password"];
			columninternal_address = base.Columns["internal_address"];
			columnexternal_address = base.Columns["external_address"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnpkid = new DataColumn("pkid", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnpkid);
			columncreated_date = new DataColumn("created_date", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columncreated_date);
			columnlast_modified = new DataColumn("last_modified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnlast_modified);
			columntype = new DataColumn("type", typeof(string), null, MappingType.Element);
			base.Columns.Add(columntype);
			columnname = new DataColumn("name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnname);
			columndescription = new DataColumn("description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columndescription);
			columnuser_name = new DataColumn("user_name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnuser_name);
			columnpassword = new DataColumn("password", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnpassword);
			columninternal_address = new DataColumn("internal_address", typeof(string), null, MappingType.Element);
			base.Columns.Add(columninternal_address);
			columnexternal_address = new DataColumn("external_address", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnexternal_address);
			base.Constraints.Add(new UniqueConstraint("PK_HostingServer", new DataColumn[1] { columnpkid }, isPrimaryKey: true));
			columnpkid.AutoIncrement = true;
			columnpkid.AutoIncrementSeed = -1L;
			columnpkid.AutoIncrementStep = -1L;
			columnpkid.AllowDBNull = false;
			columnpkid.ReadOnly = true;
			columnpkid.Unique = true;
			columncreated_date.AllowDBNull = false;
			columnlast_modified.AllowDBNull = false;
			columntype.AllowDBNull = false;
			columntype.MaxLength = 256;
			columnname.AllowDBNull = false;
			columnname.MaxLength = 256;
			columndescription.AllowDBNull = false;
			columndescription.MaxLength = int.MaxValue;
			columnuser_name.AllowDBNull = false;
			columnuser_name.MaxLength = 256;
			columnpassword.AllowDBNull = false;
			columnpassword.MaxLength = 256;
			columninternal_address.AllowDBNull = false;
			columninternal_address.MaxLength = 256;
			columnexternal_address.AllowDBNull = false;
			columnexternal_address.MaxLength = 256;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostingServerRow NewHostingServerRow()
		{
			return (HostingServerRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new HostingServerRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(HostingServerRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.HostingServerRowChanged != null)
			{
				this.HostingServerRowChanged(this, new HostingServerRowChangeEvent((HostingServerRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.HostingServerRowChanging != null)
			{
				this.HostingServerRowChanging(this, new HostingServerRowChangeEvent((HostingServerRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.HostingServerRowDeleted != null)
			{
				this.HostingServerRowDeleted(this, new HostingServerRowChangeEvent((HostingServerRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.HostingServerRowDeleting != null)
			{
				this.HostingServerRowDeleting(this, new HostingServerRowChangeEvent((HostingServerRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveHostingServerRow(HostingServerRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "HostingServerDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class DevicePrivacyMaskRow : DataRow
	{
		private DevicePrivacyMaskDataTable tableDevicePrivacyMask;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableDevicePrivacyMask.LastModifiedColumn];
			}
			set
			{
				base[tableDevicePrivacyMask.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDDevice
		{
			get
			{
				return (string)base[tableDevicePrivacyMask.IDDeviceColumn];
			}
			set
			{
				base[tableDevicePrivacyMask.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string PrivacyMask
		{
			get
			{
				return (string)base[tableDevicePrivacyMask.PrivacyMaskColumn];
			}
			set
			{
				base[tableDevicePrivacyMask.PrivacyMaskColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Enabled
		{
			get
			{
				return (bool)base[tableDevicePrivacyMask.EnabledColumn];
			}
			set
			{
				base[tableDevicePrivacyMask.EnabledColumn] = value;
			}
		}

		public string GetIDDevice()
		{
			return this.GetColumnValue<string>(tableDevicePrivacyMask.IDDeviceColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicePrivacyMaskRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDevicePrivacyMask = (DevicePrivacyMaskDataTable)base.Table;
		}
	}

	public class FailoverInfoRow : DataRow
	{
		private FailoverInfoDataTable tableFailoverInfo;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string recording_server_service_id
		{
			get
			{
				return (string)base[tableFailoverInfo.recording_server_service_idColumn];
			}
			set
			{
				base[tableFailoverInfo.recording_server_service_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string host_name
		{
			get
			{
				return (string)base[tableFailoverInfo.host_nameColumn];
			}
			set
			{
				base[tableFailoverInfo.host_nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int database_port
		{
			get
			{
				return (int)base[tableFailoverInfo.database_portColumn];
			}
			set
			{
				base[tableFailoverInfo.database_portColumn] = value;
			}
		}

		public string GetHostName()
		{
			return this.GetColumnValue<string>(tableFailoverInfo.host_nameColumn);
		}

		public string GetHostName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableFailoverInfo.host_nameColumn, version);
		}

		public int GetDatabasePort()
		{
			return this.GetColumnValue<int>(tableFailoverInfo.database_portColumn);
		}

		public int GetDatabasePort(DataRowVersion version)
		{
			return this.GetColumnValue<int>(tableFailoverInfo.database_portColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public FailoverInfoRow(DataRowBuilder rb)
			: base(rb)
		{
			tableFailoverInfo = (FailoverInfoDataTable)base.Table;
		}
	}

	public class HostingServerRow : DataRow
	{
		private HostingServerDataTable tableHostingServer;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int pkid
		{
			get
			{
				return (int)base[tableHostingServer.pkidColumn];
			}
			set
			{
				base[tableHostingServer.pkidColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime created_date
		{
			get
			{
				return (DateTime)base[tableHostingServer.created_dateColumn];
			}
			set
			{
				base[tableHostingServer.created_dateColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime last_modified
		{
			get
			{
				return (DateTime)base[tableHostingServer.last_modifiedColumn];
			}
			set
			{
				base[tableHostingServer.last_modifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string type
		{
			get
			{
				return (string)base[tableHostingServer.typeColumn];
			}
			set
			{
				base[tableHostingServer.typeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string name
		{
			get
			{
				return (string)base[tableHostingServer.nameColumn];
			}
			set
			{
				base[tableHostingServer.nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string description
		{
			get
			{
				return (string)base[tableHostingServer.descriptionColumn];
			}
			set
			{
				base[tableHostingServer.descriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string user_name
		{
			get
			{
				return (string)base[tableHostingServer.user_nameColumn];
			}
			set
			{
				base[tableHostingServer.user_nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string password
		{
			get
			{
				return (string)base[tableHostingServer.passwordColumn];
			}
			set
			{
				base[tableHostingServer.passwordColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string internal_address
		{
			get
			{
				return (string)base[tableHostingServer.internal_addressColumn];
			}
			set
			{
				base[tableHostingServer.internal_addressColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string external_address
		{
			get
			{
				return (string)base[tableHostingServer.external_addressColumn];
			}
			set
			{
				base[tableHostingServer.external_addressColumn] = value;
			}
		}

		public int GetId(DataRowVersion version)
		{
			return this.GetColumnValue<int>(tableHostingServer.pkidColumn, version);
		}

		public string GetUserName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHostingServer.user_nameColumn, version);
		}

		public string GetPassword(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHostingServer.passwordColumn, version);
		}

		public string GetInternalAddress(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHostingServer.internal_addressColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostingServerRow(DataRowBuilder rb)
			: base(rb)
		{
			tableHostingServer = (HostingServerDataTable)base.Table;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class HostedHardwareDataTable : TypedTableBase<HostedHardwareRow>
	{
		private DataColumn columnpkid;

		private DataColumn columncreated_date;

		private DataColumn columnlast_modified;

		private DataColumn columntype;

		private DataColumn columnname;

		private DataColumn columndescription;

		private DataColumn columnuser_name;

		private DataColumn columnpassword;

		private DataColumn columnhosting_info;

		private DataColumn columnmanufacturer_id;

		private DataColumn columncustom_data;

		private DataColumn columnhosting_server_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn pkidColumn => columnpkid;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn created_dateColumn => columncreated_date;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn last_modifiedColumn => columnlast_modified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn typeColumn => columntype;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn nameColumn => columnname;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn descriptionColumn => columndescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn user_nameColumn => columnuser_name;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn passwordColumn => columnpassword;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn hosting_infoColumn => columnhosting_info;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn manufacturer_idColumn => columnmanufacturer_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn custom_dataColumn => columncustom_data;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn hosting_server_idColumn => columnhosting_server_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostedHardwareRow this[int index] => (HostedHardwareRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HostedHardwareRowChangeEventHandler HostedHardwareRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HostedHardwareRowChangeEventHandler HostedHardwareRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HostedHardwareRowChangeEventHandler HostedHardwareRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HostedHardwareRowChangeEventHandler HostedHardwareRowDeleted;

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostedHardwareDataTable()
		{
			base.TableName = "HostedHardware";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostedHardwareDataTable(DataTable table)
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
		protected HostedHardwareDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddHostedHardwareRow(HostedHardwareRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostedHardwareRow AddHostedHardwareRow(DateTime created_date, DateTime last_modified, string type, string name, string description, string user_name, string password, string hosting_info, string manufacturer_id, string custom_data, int hosting_server_id)
		{
			HostedHardwareRow hostedHardwareRow = (HostedHardwareRow)NewRow();
			object[] itemArray = new object[12]
			{
				null, created_date, last_modified, type, name, description, user_name, password, hosting_info, manufacturer_id,
				custom_data, hosting_server_id
			};
			hostedHardwareRow.ItemArray = itemArray;
			base.Rows.Add(hostedHardwareRow);
			return hostedHardwareRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostedHardwareRow FindBypkid(int pkid)
		{
			return (HostedHardwareRow)base.Rows.Find(new object[1] { pkid });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			HostedHardwareDataTable obj = (HostedHardwareDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new HostedHardwareDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnpkid = base.Columns["pkid"];
			columncreated_date = base.Columns["created_date"];
			columnlast_modified = base.Columns["last_modified"];
			columntype = base.Columns["type"];
			columnname = base.Columns["name"];
			columndescription = base.Columns["description"];
			columnuser_name = base.Columns["user_name"];
			columnpassword = base.Columns["password"];
			columnhosting_info = base.Columns["hosting_info"];
			columnmanufacturer_id = base.Columns["manufacturer_id"];
			columncustom_data = base.Columns["custom_data"];
			columnhosting_server_id = base.Columns["hosting_server_id"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnpkid = new DataColumn("pkid", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnpkid);
			columncreated_date = new DataColumn("created_date", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columncreated_date);
			columnlast_modified = new DataColumn("last_modified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnlast_modified);
			columntype = new DataColumn("type", typeof(string), null, MappingType.Element);
			base.Columns.Add(columntype);
			columnname = new DataColumn("name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnname);
			columndescription = new DataColumn("description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columndescription);
			columnuser_name = new DataColumn("user_name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnuser_name);
			columnpassword = new DataColumn("password", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnpassword);
			columnhosting_info = new DataColumn("hosting_info", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnhosting_info);
			columnmanufacturer_id = new DataColumn("manufacturer_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnmanufacturer_id);
			columncustom_data = new DataColumn("custom_data", typeof(string), null, MappingType.Element);
			base.Columns.Add(columncustom_data);
			columnhosting_server_id = new DataColumn("hosting_server_id", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnhosting_server_id);
			base.Constraints.Add(new UniqueConstraint("PK_HostedHardware", new DataColumn[1] { columnpkid }, isPrimaryKey: true));
			columnpkid.AutoIncrement = true;
			columnpkid.AutoIncrementSeed = -1L;
			columnpkid.AutoIncrementStep = -1L;
			columnpkid.AllowDBNull = false;
			columnpkid.ReadOnly = true;
			columnpkid.Unique = true;
			columncreated_date.AllowDBNull = false;
			columnlast_modified.AllowDBNull = false;
			columntype.AllowDBNull = false;
			columntype.MaxLength = 256;
			columnname.AllowDBNull = false;
			columnname.MaxLength = 256;
			columndescription.AllowDBNull = false;
			columndescription.MaxLength = int.MaxValue;
			columnuser_name.AllowDBNull = false;
			columnuser_name.MaxLength = 256;
			columnpassword.AllowDBNull = false;
			columnpassword.MaxLength = 256;
			columnhosting_info.AllowDBNull = false;
			columnhosting_info.MaxLength = int.MaxValue;
			columnmanufacturer_id.AllowDBNull = false;
			columnmanufacturer_id.MaxLength = int.MaxValue;
			columncustom_data.AllowDBNull = false;
			columncustom_data.MaxLength = int.MaxValue;
			columnhosting_server_id.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostedHardwareRow NewHostedHardwareRow()
		{
			return (HostedHardwareRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new HostedHardwareRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(HostedHardwareRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.HostedHardwareRowChanged != null)
			{
				this.HostedHardwareRowChanged(this, new HostedHardwareRowChangeEvent((HostedHardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.HostedHardwareRowChanging != null)
			{
				this.HostedHardwareRowChanging(this, new HostedHardwareRowChangeEvent((HostedHardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.HostedHardwareRowDeleted != null)
			{
				this.HostedHardwareRowDeleted(this, new HostedHardwareRowChangeEvent((HostedHardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.HostedHardwareRowDeleting != null)
			{
				this.HostedHardwareRowDeleting(this, new HostedHardwareRowChangeEvent((HostedHardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveHostedHardwareRow(HostedHardwareRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "HostedHardwareDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class HostedHardwareRow : DataRow
	{
		private HostedHardwareDataTable tableHostedHardware;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int pkid
		{
			get
			{
				return (int)base[tableHostedHardware.pkidColumn];
			}
			set
			{
				base[tableHostedHardware.pkidColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime created_date
		{
			get
			{
				return (DateTime)base[tableHostedHardware.created_dateColumn];
			}
			set
			{
				base[tableHostedHardware.created_dateColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime last_modified
		{
			get
			{
				return (DateTime)base[tableHostedHardware.last_modifiedColumn];
			}
			set
			{
				base[tableHostedHardware.last_modifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string type
		{
			get
			{
				return (string)base[tableHostedHardware.typeColumn];
			}
			set
			{
				base[tableHostedHardware.typeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string name
		{
			get
			{
				return (string)base[tableHostedHardware.nameColumn];
			}
			set
			{
				base[tableHostedHardware.nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string description
		{
			get
			{
				return (string)base[tableHostedHardware.descriptionColumn];
			}
			set
			{
				base[tableHostedHardware.descriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string user_name
		{
			get
			{
				return (string)base[tableHostedHardware.user_nameColumn];
			}
			set
			{
				base[tableHostedHardware.user_nameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string password
		{
			get
			{
				return (string)base[tableHostedHardware.passwordColumn];
			}
			set
			{
				base[tableHostedHardware.passwordColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string hosting_info
		{
			get
			{
				return (string)base[tableHostedHardware.hosting_infoColumn];
			}
			set
			{
				base[tableHostedHardware.hosting_infoColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string manufacturer_id
		{
			get
			{
				return (string)base[tableHostedHardware.manufacturer_idColumn];
			}
			set
			{
				base[tableHostedHardware.manufacturer_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string custom_data
		{
			get
			{
				return (string)base[tableHostedHardware.custom_dataColumn];
			}
			set
			{
				base[tableHostedHardware.custom_dataColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int hosting_server_id
		{
			get
			{
				return (int)base[tableHostedHardware.hosting_server_idColumn];
			}
			set
			{
				base[tableHostedHardware.hosting_server_idColumn] = value;
			}
		}

		public int GetId(DataRowVersion version)
		{
			return this.GetColumnValue<int>(tableHostedHardware.pkidColumn, version);
		}

		public string GetUserName(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHostedHardware.user_nameColumn, version);
		}

		public string GetPassword(DataRowVersion version)
		{
			return this.GetColumnValue<string>(tableHostedHardware.passwordColumn, version);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostedHardwareRow(DataRowBuilder rb)
			: base(rb)
		{
			tableHostedHardware = (HostedHardwareDataTable)base.Table;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void ArchiveStorageRowChangeEventHandler(object sender, ArchiveStorageRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void AudioMessageRowChangeEventHandler(object sender, AudioMessageRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void CompressorsRowChangeEventHandler(object sender, CompressorsRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DeviceDataDistributionRowChangeEventHandler(object sender, DeviceDataDistributionRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DeviceGroupMembersRowChangeEventHandler(object sender, DeviceGroupMembersRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DeviceGroupsRowChangeEventHandler(object sender, DeviceGroupsRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DeviceLinksRowChangeEventHandler(object sender, DeviceLinksRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DevicePrivacyMaskRowChangeEventHandler(object sender, DevicePrivacyMaskRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DeviceRestrictedMediaRowChangeEventHandler(object sender, DeviceRestrictedMediaRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DevicesRowChangeEventHandler(object sender, DevicesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DeviceTrackDefinitionRowChangeEventHandler(object sender, DeviceTrackDefinitionRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DeviceTrackEvidenceLockRowChangeEventHandler(object sender, DeviceTrackEvidenceLockRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DeviceTrackInfoRowChangeEventHandler(object sender, DeviceTrackInfoRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DeviceTrackRowChangeEventHandler(object sender, DeviceTrackRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DriverNumbersRowChangeEventHandler(object sender, DriverNumbersRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void EventTypesRowChangeEventHandler(object sender, EventTypesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void ExternalPluginsRowChangeEventHandler(object sender, ExternalPluginsRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void FailoverInfoRowChangeEventHandler(object sender, FailoverInfoRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void HardwareBlockRowChangeEventHandler(object sender, HardwareBlockRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void HardwareDefinitionRowChangeEventHandler(object sender, HardwareDefinitionRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void HardwareDriverRecorderRowChangeEventHandler(object sender, HardwareDriverRecorderRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void HardwareRowChangeEventHandler(object sender, HardwareRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void HostedHardwareRowChangeEventHandler(object sender, HostedHardwareRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void HostingServerRowChangeEventHandler(object sender, HostingServerRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void LicenseRowChangeEventHandler(object sender, LicenseRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void MatrixRowChangeEventHandler(object sender, MatrixRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void MIPObjectRowChangeEventHandler(object sender, MIPObjectRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void NotificationProfilesRowChangeEventHandler(object sender, NotificationProfilesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void NotificationSettingsRowChangeEventHandler(object sender, NotificationSettingsRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void NotificationTypesRowChangeEventHandler(object sender, NotificationTypesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void PatrollingProfilesRowChangeEventHandler(object sender, PatrollingProfilesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RecorderInfoRowChangeEventHandler(object sender, RecorderInfoRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RecorderPluginDeviceRowChangeEventHandler(object sender, RecorderPluginDeviceRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RecorderPluginsRowChangeEventHandler(object sender, RecorderPluginsRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RecordersRowChangeEventHandler(object sender, RecordersRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RecordingServerServiceConfigurableRowChangeEventHandler(object sender, RecordingServerServiceConfigurableRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RecordingServerServiceRecorderRowChangeEventHandler(object sender, RecordingServerServiceRecorderRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RecordingServerServiceRowChangeEventHandler(object sender, RecordingServerServiceRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RecordingStorageDeviceRowChangeEventHandler(object sender, RecordingStorageDeviceRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RecordingStorageRowChangeEventHandler(object sender, RecordingStorageRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RegisteredDeviceTypesRowChangeEventHandler(object sender, RegisteredDeviceTypesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RegisteredServersRowChangeEventHandler(object sender, RegisteredServersRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RulesRowChangeEventHandler(object sender, RulesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RuleVariablesRowChangeEventHandler(object sender, RuleVariablesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void ServerGroupMembershipRowChangeEventHandler(object sender, ServerGroupMembershipRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void ServerGroupRowChangeEventHandler(object sender, ServerGroupRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void SystemSettingsRowChangeEventHandler(object sender, SystemSettingsRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void TimeProfileRowChangeEventHandler(object sender, TimeProfileRowChangeEvent e);

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class AudioMessageDataTable : TypedTableBase<AudioMessageRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnID;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnOriginalFileName;

		private DataColumn columnContent;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDColumn => columnID;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn OriginalFileNameColumn => columnOriginalFileName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ContentColumn => columnContent;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public AudioMessageRow this[int index] => (AudioMessageRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event AudioMessageRowChangeEventHandler AudioMessageRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event AudioMessageRowChangeEventHandler AudioMessageRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event AudioMessageRowChangeEventHandler AudioMessageRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event AudioMessageRowChangeEventHandler AudioMessageRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public AudioMessageDataTable()
		{
			base.TableName = "AudioMessage";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public AudioMessageDataTable(DataTable table)
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
		protected AudioMessageDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddAudioMessageRow(AudioMessageRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public AudioMessageRow AddAudioMessageRow(DateTime LastModified, string ID, string Name, string Description, string OriginalFileName, byte[] Content)
		{
			AudioMessageRow audioMessageRow = (AudioMessageRow)NewRow();
			object[] itemArray = new object[6] { LastModified, ID, Name, Description, OriginalFileName, Content };
			audioMessageRow.ItemArray = itemArray;
			base.Rows.Add(audioMessageRow);
			return audioMessageRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public AudioMessageRow FindByID(string ID)
		{
			return (AudioMessageRow)base.Rows.Find(new object[1] { ID });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			AudioMessageDataTable obj = (AudioMessageDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new AudioMessageDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnID = base.Columns["ID"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnOriginalFileName = base.Columns["OriginalFileName"];
			columnContent = base.Columns["Content"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnID = new DataColumn("ID", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnID);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnOriginalFileName = new DataColumn("OriginalFileName", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnOriginalFileName);
			columnContent = new DataColumn("Content", typeof(byte[]), null, MappingType.Element);
			base.Columns.Add(columnContent);
			base.Constraints.Add(new UniqueConstraint("PK_AudioMessage", new DataColumn[1] { columnID }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnID.AllowDBNull = false;
			columnID.Unique = true;
			columnID.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = int.MaxValue;
			columnOriginalFileName.AllowDBNull = false;
			columnOriginalFileName.MaxLength = 1024;
			columnContent.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public AudioMessageRow NewAudioMessageRow()
		{
			return (AudioMessageRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new AudioMessageRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(AudioMessageRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.AudioMessageRowChanged != null)
			{
				this.AudioMessageRowChanged(this, new AudioMessageRowChangeEvent((AudioMessageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.AudioMessageRowChanging != null)
			{
				this.AudioMessageRowChanging(this, new AudioMessageRowChangeEvent((AudioMessageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.AudioMessageRowDeleted != null)
			{
				this.AudioMessageRowDeleted(this, new AudioMessageRowChangeEvent((AudioMessageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.AudioMessageRowDeleting != null)
			{
				this.AudioMessageRowDeleting(this, new AudioMessageRowChangeEvent((AudioMessageRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveAudioMessageRow(AudioMessageRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "AudioMessageDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class CompressorsDataTable : TypedTableBase<CompressorsRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDCodec;

		private DataColumn columnHandler;

		private DataColumn columnFlags;

		private DataColumn columnQuality;

		private DataColumn columnKeyFrames;

		private DataColumn columnDataRate;

		private DataColumn columnConfigData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDCodecColumn => columnIDCodec;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn HandlerColumn => columnHandler;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn FlagsColumn => columnFlags;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn QualityColumn => columnQuality;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn KeyFramesColumn => columnKeyFrames;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DataRateColumn => columnDataRate;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ConfigDataColumn => columnConfigData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public CompressorsRow this[int index] => (CompressorsRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event CompressorsRowChangeEventHandler CompressorsRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event CompressorsRowChangeEventHandler CompressorsRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event CompressorsRowChangeEventHandler CompressorsRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event CompressorsRowChangeEventHandler CompressorsRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public CompressorsDataTable()
		{
			base.TableName = "Compressors";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public CompressorsDataTable(DataTable table)
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
		protected CompressorsDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddCompressorsRow(CompressorsRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public CompressorsRow AddCompressorsRow(DateTime LastModified, string IDCodec, int Handler, int Flags, int Quality, int KeyFrames, int DataRate, byte[] ConfigData)
		{
			CompressorsRow compressorsRow = (CompressorsRow)NewRow();
			object[] itemArray = new object[8] { LastModified, IDCodec, Handler, Flags, Quality, KeyFrames, DataRate, ConfigData };
			compressorsRow.ItemArray = itemArray;
			base.Rows.Add(compressorsRow);
			return compressorsRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public CompressorsRow FindByIDCodec(string IDCodec)
		{
			return (CompressorsRow)base.Rows.Find(new object[1] { IDCodec });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			CompressorsDataTable obj = (CompressorsDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new CompressorsDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDCodec = base.Columns["IDCodec"];
			columnHandler = base.Columns["Handler"];
			columnFlags = base.Columns["Flags"];
			columnQuality = base.Columns["Quality"];
			columnKeyFrames = base.Columns["KeyFrames"];
			columnDataRate = base.Columns["DataRate"];
			columnConfigData = base.Columns["ConfigData"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDCodec = new DataColumn("IDCodec", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDCodec);
			columnHandler = new DataColumn("Handler", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnHandler);
			columnFlags = new DataColumn("Flags", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnFlags);
			columnQuality = new DataColumn("Quality", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnQuality);
			columnKeyFrames = new DataColumn("KeyFrames", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnKeyFrames);
			columnDataRate = new DataColumn("DataRate", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnDataRate);
			columnConfigData = new DataColumn("ConfigData", typeof(byte[]), null, MappingType.Element);
			base.Columns.Add(columnConfigData);
			base.Constraints.Add(new UniqueConstraint("PK_Compressors", new DataColumn[1] { columnIDCodec }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDCodec.AllowDBNull = false;
			columnIDCodec.Unique = true;
			columnIDCodec.MaxLength = 36;
			columnHandler.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public CompressorsRow NewCompressorsRow()
		{
			return (CompressorsRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new CompressorsRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(CompressorsRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.CompressorsRowChanged != null)
			{
				this.CompressorsRowChanged(this, new CompressorsRowChangeEvent((CompressorsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.CompressorsRowChanging != null)
			{
				this.CompressorsRowChanging(this, new CompressorsRowChangeEvent((CompressorsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.CompressorsRowDeleted != null)
			{
				this.CompressorsRowDeleted(this, new CompressorsRowChangeEvent((CompressorsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.CompressorsRowDeleting != null)
			{
				this.CompressorsRowDeleting(this, new CompressorsRowChangeEvent((CompressorsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveCompressorsRow(CompressorsRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "CompressorsDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class DeviceGroupMembersDataTable : TypedTableBase<DeviceGroupMembersRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDDeviceGroup;

		private DataColumn columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceGroupColumn => columnIDDeviceGroup;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupMembersRow this[int index] => (DeviceGroupMembersRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceGroupMembersRowChangeEventHandler DeviceGroupMembersRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceGroupMembersRowChangeEventHandler DeviceGroupMembersRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceGroupMembersRowChangeEventHandler DeviceGroupMembersRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceGroupMembersRowChangeEventHandler DeviceGroupMembersRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupMembersDataTable()
		{
			base.TableName = "DeviceGroupMembers";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupMembersDataTable(DataTable table)
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
		protected DeviceGroupMembersDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDeviceGroupMembersRow(DeviceGroupMembersRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupMembersRow AddDeviceGroupMembersRow(DateTime LastModified, string IDDeviceGroup, string IDDevice)
		{
			DeviceGroupMembersRow deviceGroupMembersRow = (DeviceGroupMembersRow)NewRow();
			object[] itemArray = new object[3] { LastModified, IDDeviceGroup, IDDevice };
			deviceGroupMembersRow.ItemArray = itemArray;
			base.Rows.Add(deviceGroupMembersRow);
			return deviceGroupMembersRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupMembersRow FindByIDDeviceGroupIDDevice(string IDDeviceGroup, string IDDevice)
		{
			return (DeviceGroupMembersRow)base.Rows.Find(new object[2] { IDDeviceGroup, IDDevice });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DeviceGroupMembersDataTable obj = (DeviceGroupMembersDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DeviceGroupMembersDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDDeviceGroup = base.Columns["IDDeviceGroup"];
			columnIDDevice = base.Columns["IDDevice"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDDeviceGroup = new DataColumn("IDDeviceGroup", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDeviceGroup);
			columnIDDevice = new DataColumn("IDDevice", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			base.Constraints.Add(new UniqueConstraint("PK_DeviceGroupMembers", new DataColumn[2] { columnIDDeviceGroup, columnIDDevice }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDDeviceGroup.AllowDBNull = false;
			columnIDDeviceGroup.MaxLength = 36;
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.MaxLength = 36;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupMembersRow NewDeviceGroupMembersRow()
		{
			return (DeviceGroupMembersRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DeviceGroupMembersRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DeviceGroupMembersRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DeviceGroupMembersRowChanged != null)
			{
				this.DeviceGroupMembersRowChanged(this, new DeviceGroupMembersRowChangeEvent((DeviceGroupMembersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DeviceGroupMembersRowChanging != null)
			{
				this.DeviceGroupMembersRowChanging(this, new DeviceGroupMembersRowChangeEvent((DeviceGroupMembersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DeviceGroupMembersRowDeleted != null)
			{
				this.DeviceGroupMembersRowDeleted(this, new DeviceGroupMembersRowChangeEvent((DeviceGroupMembersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DeviceGroupMembersRowDeleting != null)
			{
				this.DeviceGroupMembersRowDeleting(this, new DeviceGroupMembersRowChangeEvent((DeviceGroupMembersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDeviceGroupMembersRow(DeviceGroupMembersRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DeviceGroupMembersDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class DeviceTrackDefinitionDataTable : TypedTableBase<DeviceTrackDefinitionRow>
	{
		private DataColumn columnIDTrackDefinition;

		private DataColumn columnIsPrimary;

		private DataColumn columnIsEdge;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDTrackDefinitionColumn => columnIDTrackDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IsPrimaryColumn => columnIsPrimary;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IsEdgeColumn => columnIsEdge;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDefinitionRow this[int index] => (DeviceTrackDefinitionRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackDefinitionRowChangeEventHandler DeviceTrackDefinitionRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackDefinitionRowChangeEventHandler DeviceTrackDefinitionRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackDefinitionRowChangeEventHandler DeviceTrackDefinitionRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackDefinitionRowChangeEventHandler DeviceTrackDefinitionRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDefinitionDataTable()
		{
			base.TableName = "DeviceTrackDefinition";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDefinitionDataTable(DataTable table)
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
		protected DeviceTrackDefinitionDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDeviceTrackDefinitionRow(DeviceTrackDefinitionRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDefinitionRow AddDeviceTrackDefinitionRow(Guid IDTrackDefinition, bool IsPrimary, bool IsEdge)
		{
			DeviceTrackDefinitionRow deviceTrackDefinitionRow = (DeviceTrackDefinitionRow)NewRow();
			object[] itemArray = new object[3] { IDTrackDefinition, IsPrimary, IsEdge };
			deviceTrackDefinitionRow.ItemArray = itemArray;
			base.Rows.Add(deviceTrackDefinitionRow);
			return deviceTrackDefinitionRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDefinitionRow FindByIDTrackDefinition(Guid IDTrackDefinition)
		{
			return (DeviceTrackDefinitionRow)base.Rows.Find(new object[1] { IDTrackDefinition });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DeviceTrackDefinitionDataTable obj = (DeviceTrackDefinitionDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DeviceTrackDefinitionDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDTrackDefinition = base.Columns["IDTrackDefinition"];
			columnIsPrimary = base.Columns["IsPrimary"];
			columnIsEdge = base.Columns["IsEdge"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDTrackDefinition = new DataColumn("IDTrackDefinition", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDTrackDefinition);
			columnIsPrimary = new DataColumn("IsPrimary", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnIsPrimary);
			columnIsEdge = new DataColumn("IsEdge", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnIsEdge);
			base.Constraints.Add(new UniqueConstraint("PK_DeviceTrackDefinition", new DataColumn[1] { columnIDTrackDefinition }, isPrimaryKey: true));
			columnIDTrackDefinition.AllowDBNull = false;
			columnIDTrackDefinition.Unique = true;
			columnIsPrimary.AllowDBNull = false;
			columnIsEdge.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDefinitionRow NewDeviceTrackDefinitionRow()
		{
			return (DeviceTrackDefinitionRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DeviceTrackDefinitionRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DeviceTrackDefinitionRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DeviceTrackDefinitionRowChanged != null)
			{
				this.DeviceTrackDefinitionRowChanged(this, new DeviceTrackDefinitionRowChangeEvent((DeviceTrackDefinitionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DeviceTrackDefinitionRowChanging != null)
			{
				this.DeviceTrackDefinitionRowChanging(this, new DeviceTrackDefinitionRowChangeEvent((DeviceTrackDefinitionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DeviceTrackDefinitionRowDeleted != null)
			{
				this.DeviceTrackDefinitionRowDeleted(this, new DeviceTrackDefinitionRowChangeEvent((DeviceTrackDefinitionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DeviceTrackDefinitionRowDeleting != null)
			{
				this.DeviceTrackDefinitionRowDeleting(this, new DeviceTrackDefinitionRowChangeEvent((DeviceTrackDefinitionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDeviceTrackDefinitionRow(DeviceTrackDefinitionRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DeviceTrackDefinitionDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class DeviceTrackEvidenceLockDataTable : TypedTableBase<DeviceTrackEvidenceLockRow>
	{
		private DataColumn columnIDMarkedData;

		private DataColumn columnIDDevice;

		private DataColumn columnIDTrack;

		private DataColumn columnStartTime;

		private DataColumn columnEndTime;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDMarkedDataColumn => columnIDMarkedData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDTrackColumn => columnIDTrack;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn StartTimeColumn => columnStartTime;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EndTimeColumn => columnEndTime;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackEvidenceLockRow this[int index] => (DeviceTrackEvidenceLockRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackEvidenceLockRowChangeEventHandler DeviceTrackEvidenceLockRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackEvidenceLockRowChangeEventHandler DeviceTrackEvidenceLockRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackEvidenceLockRowChangeEventHandler DeviceTrackEvidenceLockRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackEvidenceLockRowChangeEventHandler DeviceTrackEvidenceLockRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackEvidenceLockDataTable()
		{
			base.TableName = "DeviceTrackEvidenceLock";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackEvidenceLockDataTable(DataTable table)
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
		protected DeviceTrackEvidenceLockDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDeviceTrackEvidenceLockRow(DeviceTrackEvidenceLockRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackEvidenceLockRow AddDeviceTrackEvidenceLockRow(Guid IDMarkedData, string IDDevice, Guid IDTrack, long StartTime, long EndTime)
		{
			DeviceTrackEvidenceLockRow deviceTrackEvidenceLockRow = (DeviceTrackEvidenceLockRow)NewRow();
			object[] itemArray = new object[5] { IDMarkedData, IDDevice, IDTrack, StartTime, EndTime };
			deviceTrackEvidenceLockRow.ItemArray = itemArray;
			base.Rows.Add(deviceTrackEvidenceLockRow);
			return deviceTrackEvidenceLockRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackEvidenceLockRow FindByIDMarkedDataIDDeviceIDTrack(Guid IDMarkedData, string IDDevice, Guid IDTrack)
		{
			return (DeviceTrackEvidenceLockRow)base.Rows.Find(new object[3] { IDMarkedData, IDDevice, IDTrack });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DeviceTrackEvidenceLockDataTable obj = (DeviceTrackEvidenceLockDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DeviceTrackEvidenceLockDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDMarkedData = base.Columns["IDMarkedData"];
			columnIDDevice = base.Columns["IDDevice"];
			columnIDTrack = base.Columns["IDTrack"];
			columnStartTime = base.Columns["StartTime"];
			columnEndTime = base.Columns["EndTime"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDMarkedData = new DataColumn("IDMarkedData", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDMarkedData);
			columnIDDevice = new DataColumn("IDDevice", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnIDTrack = new DataColumn("IDTrack", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDTrack);
			columnStartTime = new DataColumn("StartTime", typeof(long), null, MappingType.Element);
			base.Columns.Add(columnStartTime);
			columnEndTime = new DataColumn("EndTime", typeof(long), null, MappingType.Element);
			base.Columns.Add(columnEndTime);
			base.Constraints.Add(new UniqueConstraint("PK_DeviceTrackEvidenceLock", new DataColumn[3] { columnIDMarkedData, columnIDDevice, columnIDTrack }, isPrimaryKey: true));
			columnIDMarkedData.AllowDBNull = false;
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.MaxLength = 36;
			columnIDTrack.AllowDBNull = false;
			columnStartTime.AllowDBNull = false;
			columnEndTime.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackEvidenceLockRow NewDeviceTrackEvidenceLockRow()
		{
			return (DeviceTrackEvidenceLockRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DeviceTrackEvidenceLockRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DeviceTrackEvidenceLockRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DeviceTrackEvidenceLockRowChanged != null)
			{
				this.DeviceTrackEvidenceLockRowChanged(this, new DeviceTrackEvidenceLockRowChangeEvent((DeviceTrackEvidenceLockRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DeviceTrackEvidenceLockRowChanging != null)
			{
				this.DeviceTrackEvidenceLockRowChanging(this, new DeviceTrackEvidenceLockRowChangeEvent((DeviceTrackEvidenceLockRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DeviceTrackEvidenceLockRowDeleted != null)
			{
				this.DeviceTrackEvidenceLockRowDeleted(this, new DeviceTrackEvidenceLockRowChangeEvent((DeviceTrackEvidenceLockRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DeviceTrackEvidenceLockRowDeleting != null)
			{
				this.DeviceTrackEvidenceLockRowDeleting(this, new DeviceTrackEvidenceLockRowChangeEvent((DeviceTrackEvidenceLockRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDeviceTrackEvidenceLockRow(DeviceTrackEvidenceLockRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DeviceTrackEvidenceLockDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class DeviceTrackInfoDataTable : TypedTableBase<DeviceTrackInfoRow>
	{
		private DataColumn columnIDRecorder;

		private DataColumn columnIDDevice;

		private DataColumn columnIDTrack;

		private DataColumn columnName;

		private DataColumn columnDeviceType;

		private DataColumn columnIDTrackDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderColumn => columnIDRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDTrackColumn => columnIDTrack;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DeviceTypeColumn => columnDeviceType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDTrackDefinitionColumn => columnIDTrackDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackInfoRow this[int index] => (DeviceTrackInfoRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackInfoRowChangeEventHandler DeviceTrackInfoRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackInfoRowChangeEventHandler DeviceTrackInfoRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackInfoRowChangeEventHandler DeviceTrackInfoRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DeviceTrackInfoRowChangeEventHandler DeviceTrackInfoRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackInfoDataTable()
		{
			base.TableName = "DeviceTrackInfo";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackInfoDataTable(DataTable table)
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
		protected DeviceTrackInfoDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDeviceTrackInfoRow(DeviceTrackInfoRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackInfoRow AddDeviceTrackInfoRow(Guid IDRecorder, Guid IDDevice, Guid IDTrack, string Name, string DeviceType, Guid IDTrackDefinition)
		{
			DeviceTrackInfoRow deviceTrackInfoRow = (DeviceTrackInfoRow)NewRow();
			object[] itemArray = new object[6] { IDRecorder, IDDevice, IDTrack, Name, DeviceType, IDTrackDefinition };
			deviceTrackInfoRow.ItemArray = itemArray;
			base.Rows.Add(deviceTrackInfoRow);
			return deviceTrackInfoRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackInfoRow FindByIDRecorderIDTrack(Guid IDRecorder, Guid IDTrack)
		{
			return (DeviceTrackInfoRow)base.Rows.Find(new object[2] { IDRecorder, IDTrack });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DeviceTrackInfoDataTable obj = (DeviceTrackInfoDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DeviceTrackInfoDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDRecorder = base.Columns["IDRecorder"];
			columnIDDevice = base.Columns["IDDevice"];
			columnIDTrack = base.Columns["IDTrack"];
			columnName = base.Columns["Name"];
			columnDeviceType = base.Columns["DeviceType"];
			columnIDTrackDefinition = base.Columns["IDTrackDefinition"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDRecorder = new DataColumn("IDRecorder", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDRecorder);
			columnIDDevice = new DataColumn("IDDevice", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnIDTrack = new DataColumn("IDTrack", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDTrack);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDeviceType = new DataColumn("DeviceType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDeviceType);
			columnIDTrackDefinition = new DataColumn("IDTrackDefinition", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDTrackDefinition);
			base.Constraints.Add(new UniqueConstraint("PK_DeviceTrackInfo", new DataColumn[2] { columnIDRecorder, columnIDTrack }, isPrimaryKey: true));
			columnIDRecorder.AllowDBNull = false;
			columnIDRecorder.ReadOnly = true;
			columnIDDevice.ReadOnly = true;
			columnIDTrack.AllowDBNull = false;
			columnIDTrack.ReadOnly = true;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDeviceType.AllowDBNull = false;
			columnDeviceType.MaxLength = 100;
			columnIDTrackDefinition.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackInfoRow NewDeviceTrackInfoRow()
		{
			return (DeviceTrackInfoRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DeviceTrackInfoRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DeviceTrackInfoRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DeviceTrackInfoRowChanged != null)
			{
				this.DeviceTrackInfoRowChanged(this, new DeviceTrackInfoRowChangeEvent((DeviceTrackInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DeviceTrackInfoRowChanging != null)
			{
				this.DeviceTrackInfoRowChanging(this, new DeviceTrackInfoRowChangeEvent((DeviceTrackInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DeviceTrackInfoRowDeleted != null)
			{
				this.DeviceTrackInfoRowDeleted(this, new DeviceTrackInfoRowChangeEvent((DeviceTrackInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DeviceTrackInfoRowDeleting != null)
			{
				this.DeviceTrackInfoRowDeleting(this, new DeviceTrackInfoRowChangeEvent((DeviceTrackInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDeviceTrackInfoRow(DeviceTrackInfoRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DeviceTrackInfoDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class DriverNumbersDataTable : TypedTableBase<DriverNumbersRow>
	{
		private DataColumn columnIDDriver;

		private DataColumn columnDriverNumber;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDriverColumn => columnIDDriver;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DriverNumberColumn => columnDriverNumber;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DriverNumbersRow this[int index] => (DriverNumbersRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DriverNumbersRowChangeEventHandler DriverNumbersRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DriverNumbersRowChangeEventHandler DriverNumbersRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DriverNumbersRowChangeEventHandler DriverNumbersRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event DriverNumbersRowChangeEventHandler DriverNumbersRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DriverNumbersDataTable()
		{
			base.TableName = "DriverNumbers";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DriverNumbersDataTable(DataTable table)
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
		protected DriverNumbersDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddDriverNumbersRow(DriverNumbersRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DriverNumbersRow AddDriverNumbersRow(Guid IDDriver)
		{
			DriverNumbersRow driverNumbersRow = (DriverNumbersRow)NewRow();
			object[] itemArray = new object[2] { IDDriver, null };
			driverNumbersRow.ItemArray = itemArray;
			base.Rows.Add(driverNumbersRow);
			return driverNumbersRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DriverNumbersRow FindByIDDriver(Guid IDDriver)
		{
			return (DriverNumbersRow)base.Rows.Find(new object[1] { IDDriver });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DriverNumbersDataTable obj = (DriverNumbersDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new DriverNumbersDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDDriver = base.Columns["IDDriver"];
			columnDriverNumber = base.Columns["DriverNumber"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDDriver = new DataColumn("IDDriver", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDDriver);
			columnDriverNumber = new DataColumn("DriverNumber", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnDriverNumber);
			base.Constraints.Add(new UniqueConstraint("PK_DriverNumbers", new DataColumn[1] { columnIDDriver }, isPrimaryKey: true));
			columnIDDriver.AllowDBNull = false;
			columnIDDriver.Unique = true;
			columnDriverNumber.AutoIncrement = true;
			columnDriverNumber.AutoIncrementSeed = 50001L;
			columnDriverNumber.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DriverNumbersRow NewDriverNumbersRow()
		{
			return (DriverNumbersRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new DriverNumbersRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(DriverNumbersRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.DriverNumbersRowChanged != null)
			{
				this.DriverNumbersRowChanged(this, new DriverNumbersRowChangeEvent((DriverNumbersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.DriverNumbersRowChanging != null)
			{
				this.DriverNumbersRowChanging(this, new DriverNumbersRowChangeEvent((DriverNumbersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.DriverNumbersRowDeleted != null)
			{
				this.DriverNumbersRowDeleted(this, new DriverNumbersRowChangeEvent((DriverNumbersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.DriverNumbersRowDeleting != null)
			{
				this.DriverNumbersRowDeleting(this, new DriverNumbersRowChangeEvent((DriverNumbersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveDriverNumbersRow(DriverNumbersRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DriverNumbersDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class EventTypesDataTable : TypedTableBase<EventTypesRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDEventType;

		private DataColumn columnDefinition;

		private DataColumn columnGeneratorType;

		private DataColumn columnIDGenerator;

		private DataColumn columnBuiltin;

		private DataColumn columnName;

		private DataColumn columnGeneratorSubType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDEventTypeColumn => columnIDEventType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DefinitionColumn => columnDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn GeneratorTypeColumn => columnGeneratorType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDGeneratorColumn => columnIDGenerator;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn BuiltinColumn => columnBuiltin;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn GeneratorSubTypeColumn => columnGeneratorSubType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public EventTypesRow this[int index] => (EventTypesRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event EventTypesRowChangeEventHandler EventTypesRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event EventTypesRowChangeEventHandler EventTypesRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event EventTypesRowChangeEventHandler EventTypesRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event EventTypesRowChangeEventHandler EventTypesRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public EventTypesDataTable()
		{
			base.TableName = "EventTypes";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public EventTypesDataTable(DataTable table)
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
		protected EventTypesDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddEventTypesRow(EventTypesRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public EventTypesRow AddEventTypesRow(DateTime LastModified, string IDEventType, string Definition, string GeneratorType, string IDGenerator, bool Builtin, string Name, string GeneratorSubType)
		{
			EventTypesRow eventTypesRow = (EventTypesRow)NewRow();
			object[] itemArray = new object[8] { LastModified, IDEventType, Definition, GeneratorType, IDGenerator, Builtin, Name, GeneratorSubType };
			eventTypesRow.ItemArray = itemArray;
			base.Rows.Add(eventTypesRow);
			return eventTypesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public EventTypesRow FindByIDEventType(string IDEventType)
		{
			return (EventTypesRow)base.Rows.Find(new object[1] { IDEventType });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			EventTypesDataTable obj = (EventTypesDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new EventTypesDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDEventType = base.Columns["IDEventType"];
			columnDefinition = base.Columns["Definition"];
			columnGeneratorType = base.Columns["GeneratorType"];
			columnIDGenerator = base.Columns["IDGenerator"];
			columnBuiltin = base.Columns["Builtin"];
			columnName = base.Columns["Name"];
			columnGeneratorSubType = base.Columns["GeneratorSubType"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDEventType = new DataColumn("IDEventType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDEventType);
			columnDefinition = new DataColumn("Definition", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDefinition);
			columnGeneratorType = new DataColumn("GeneratorType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnGeneratorType);
			columnIDGenerator = new DataColumn("IDGenerator", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDGenerator);
			columnBuiltin = new DataColumn("Builtin", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnBuiltin);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnGeneratorSubType = new DataColumn("GeneratorSubType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnGeneratorSubType);
			base.Constraints.Add(new UniqueConstraint("PK_EventTypes", new DataColumn[1] { columnIDEventType }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDEventType.AllowDBNull = false;
			columnIDEventType.Unique = true;
			columnIDEventType.MaxLength = 36;
			columnDefinition.AllowDBNull = false;
			columnDefinition.MaxLength = 1073741823;
			columnGeneratorType.AllowDBNull = false;
			columnGeneratorType.MaxLength = 100;
			columnIDGenerator.MaxLength = 36;
			columnBuiltin.AllowDBNull = false;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnGeneratorSubType.AllowDBNull = false;
			columnGeneratorSubType.MaxLength = 36;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public EventTypesRow NewEventTypesRow()
		{
			return (EventTypesRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new EventTypesRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(EventTypesRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.EventTypesRowChanged != null)
			{
				this.EventTypesRowChanged(this, new EventTypesRowChangeEvent((EventTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.EventTypesRowChanging != null)
			{
				this.EventTypesRowChanging(this, new EventTypesRowChangeEvent((EventTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.EventTypesRowDeleted != null)
			{
				this.EventTypesRowDeleted(this, new EventTypesRowChangeEvent((EventTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.EventTypesRowDeleting != null)
			{
				this.EventTypesRowDeleting(this, new EventTypesRowChangeEvent((EventTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveEventTypesRow(EventTypesRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "EventTypesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class ExternalPluginsDataTable : TypedTableBase<ExternalPluginsRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnPluginID;

		private DataColumn columnPluginType;

		private DataColumn columnData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PluginIDColumn => columnPluginID;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PluginTypeColumn => columnPluginType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DataColumn => columnData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ExternalPluginsRow this[int index] => (ExternalPluginsRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ExternalPluginsRowChangeEventHandler ExternalPluginsRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ExternalPluginsRowChangeEventHandler ExternalPluginsRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ExternalPluginsRowChangeEventHandler ExternalPluginsRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ExternalPluginsRowChangeEventHandler ExternalPluginsRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ExternalPluginsDataTable()
		{
			base.TableName = "ExternalPlugins";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ExternalPluginsDataTable(DataTable table)
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
		protected ExternalPluginsDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddExternalPluginsRow(ExternalPluginsRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ExternalPluginsRow AddExternalPluginsRow(DateTime LastModified, string PluginID, string PluginType, string Data)
		{
			ExternalPluginsRow externalPluginsRow = (ExternalPluginsRow)NewRow();
			object[] itemArray = new object[4] { LastModified, PluginID, PluginType, Data };
			externalPluginsRow.ItemArray = itemArray;
			base.Rows.Add(externalPluginsRow);
			return externalPluginsRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ExternalPluginsRow FindByPluginID(string PluginID)
		{
			return (ExternalPluginsRow)base.Rows.Find(new object[1] { PluginID });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			ExternalPluginsDataTable obj = (ExternalPluginsDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new ExternalPluginsDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnPluginID = base.Columns["PluginID"];
			columnPluginType = base.Columns["PluginType"];
			columnData = base.Columns["Data"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnPluginID = new DataColumn("PluginID", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnPluginID);
			columnPluginType = new DataColumn("PluginType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnPluginType);
			columnData = new DataColumn("Data", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnData);
			base.Constraints.Add(new UniqueConstraint("PK_ExternalPlugins", new DataColumn[1] { columnPluginID }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnPluginID.AllowDBNull = false;
			columnPluginID.Unique = true;
			columnPluginID.MaxLength = 36;
			columnPluginType.AllowDBNull = false;
			columnPluginType.MaxLength = 100;
			columnData.AllowDBNull = false;
			columnData.MaxLength = 1073741823;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ExternalPluginsRow NewExternalPluginsRow()
		{
			return (ExternalPluginsRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new ExternalPluginsRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(ExternalPluginsRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.ExternalPluginsRowChanged != null)
			{
				this.ExternalPluginsRowChanged(this, new ExternalPluginsRowChangeEvent((ExternalPluginsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.ExternalPluginsRowChanging != null)
			{
				this.ExternalPluginsRowChanging(this, new ExternalPluginsRowChangeEvent((ExternalPluginsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.ExternalPluginsRowDeleted != null)
			{
				this.ExternalPluginsRowDeleted(this, new ExternalPluginsRowChangeEvent((ExternalPluginsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.ExternalPluginsRowDeleting != null)
			{
				this.ExternalPluginsRowDeleting(this, new ExternalPluginsRowChangeEvent((ExternalPluginsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveExternalPluginsRow(ExternalPluginsRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "ExternalPluginsDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class FailoverInfoDataTable : TypedTableBase<FailoverInfoRow>
	{
		private DataColumn columnrecording_server_service_id;

		private DataColumn columnhost_name;

		private DataColumn columndatabase_port;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn recording_server_service_idColumn => columnrecording_server_service_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn host_nameColumn => columnhost_name;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn database_portColumn => columndatabase_port;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public FailoverInfoRow this[int index] => (FailoverInfoRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event FailoverInfoRowChangeEventHandler FailoverInfoRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event FailoverInfoRowChangeEventHandler FailoverInfoRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event FailoverInfoRowChangeEventHandler FailoverInfoRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event FailoverInfoRowChangeEventHandler FailoverInfoRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public FailoverInfoDataTable()
		{
			base.TableName = "FailoverInfo";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public FailoverInfoDataTable(DataTable table)
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
		protected FailoverInfoDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddFailoverInfoRow(FailoverInfoRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public FailoverInfoRow AddFailoverInfoRow(string recording_server_service_id, string host_name, int database_port)
		{
			FailoverInfoRow failoverInfoRow = (FailoverInfoRow)NewRow();
			object[] itemArray = new object[3] { recording_server_service_id, host_name, database_port };
			failoverInfoRow.ItemArray = itemArray;
			base.Rows.Add(failoverInfoRow);
			return failoverInfoRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public FailoverInfoRow FindByrecording_server_service_id(string recording_server_service_id)
		{
			return (FailoverInfoRow)base.Rows.Find(new object[1] { recording_server_service_id });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			FailoverInfoDataTable obj = (FailoverInfoDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new FailoverInfoDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnrecording_server_service_id = base.Columns["recording_server_service_id"];
			columnhost_name = base.Columns["host_name"];
			columndatabase_port = base.Columns["database_port"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnrecording_server_service_id = new DataColumn("recording_server_service_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnrecording_server_service_id);
			columnhost_name = new DataColumn("host_name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnhost_name);
			columndatabase_port = new DataColumn("database_port", typeof(int), null, MappingType.Element);
			base.Columns.Add(columndatabase_port);
			base.Constraints.Add(new UniqueConstraint("PK_Failoverinfo", new DataColumn[1] { columnrecording_server_service_id }, isPrimaryKey: true));
			columnrecording_server_service_id.AllowDBNull = false;
			columnrecording_server_service_id.Unique = true;
			columnrecording_server_service_id.MaxLength = 36;
			columnhost_name.AllowDBNull = false;
			columnhost_name.MaxLength = 256;
			columndatabase_port.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public FailoverInfoRow NewFailoverInfoRow()
		{
			return (FailoverInfoRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new FailoverInfoRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(FailoverInfoRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.FailoverInfoRowChanged != null)
			{
				this.FailoverInfoRowChanged(this, new FailoverInfoRowChangeEvent((FailoverInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.FailoverInfoRowChanging != null)
			{
				this.FailoverInfoRowChanging(this, new FailoverInfoRowChangeEvent((FailoverInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.FailoverInfoRowDeleted != null)
			{
				this.FailoverInfoRowDeleted(this, new FailoverInfoRowChangeEvent((FailoverInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.FailoverInfoRowDeleting != null)
			{
				this.FailoverInfoRowDeleting(this, new FailoverInfoRowChangeEvent((FailoverInfoRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveFailoverInfoRow(FailoverInfoRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "FailoverInfoDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class HardwareBlockDataTable : TypedTableBase<HardwareBlockRow>
	{
		private DataColumn columnpkid;

		private DataColumn columnhardware_id;

		private DataColumn columnexpires;

		private DataColumn columnowner;

		private DataColumn columndata;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn pkidColumn => columnpkid;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn hardware_idColumn => columnhardware_id;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn expiresColumn => columnexpires;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ownerColumn => columnowner;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn dataColumn => columndata;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareBlockRow this[int index] => (HardwareBlockRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareBlockRowChangeEventHandler HardwareBlockRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareBlockRowChangeEventHandler HardwareBlockRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareBlockRowChangeEventHandler HardwareBlockRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareBlockRowChangeEventHandler HardwareBlockRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareBlockDataTable()
		{
			base.TableName = "HardwareBlock";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareBlockDataTable(DataTable table)
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
		protected HardwareBlockDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddHardwareBlockRow(HardwareBlockRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareBlockRow AddHardwareBlockRow(string hardware_id, DateTime expires, string owner, string data)
		{
			HardwareBlockRow hardwareBlockRow = (HardwareBlockRow)NewRow();
			object[] itemArray = new object[5] { null, hardware_id, expires, owner, data };
			hardwareBlockRow.ItemArray = itemArray;
			base.Rows.Add(hardwareBlockRow);
			return hardwareBlockRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareBlockRow FindBypkid(int pkid)
		{
			return (HardwareBlockRow)base.Rows.Find(new object[1] { pkid });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			HardwareBlockDataTable obj = (HardwareBlockDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new HardwareBlockDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnpkid = base.Columns["pkid"];
			columnhardware_id = base.Columns["hardware_id"];
			columnexpires = base.Columns["expires"];
			columnowner = base.Columns["owner"];
			columndata = base.Columns["data"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnpkid = new DataColumn("pkid", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnpkid);
			columnhardware_id = new DataColumn("hardware_id", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnhardware_id);
			columnexpires = new DataColumn("expires", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnexpires);
			columnowner = new DataColumn("owner", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnowner);
			columndata = new DataColumn("data", typeof(string), null, MappingType.Element);
			base.Columns.Add(columndata);
			base.Constraints.Add(new UniqueConstraint("PK_HardwareBlock", new DataColumn[1] { columnpkid }, isPrimaryKey: true));
			columnpkid.AutoIncrement = true;
			columnpkid.AutoIncrementSeed = -1L;
			columnpkid.AutoIncrementStep = -1L;
			columnpkid.AllowDBNull = false;
			columnpkid.ReadOnly = true;
			columnpkid.Unique = true;
			columnhardware_id.AllowDBNull = false;
			columnhardware_id.MaxLength = 36;
			columnexpires.AllowDBNull = false;
			columnowner.AllowDBNull = false;
			columnowner.MaxLength = 256;
			columndata.AllowDBNull = false;
			columndata.MaxLength = 4000;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareBlockRow NewHardwareBlockRow()
		{
			return (HardwareBlockRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new HardwareBlockRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(HardwareBlockRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.HardwareBlockRowChanged != null)
			{
				this.HardwareBlockRowChanged(this, new HardwareBlockRowChangeEvent((HardwareBlockRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.HardwareBlockRowChanging != null)
			{
				this.HardwareBlockRowChanging(this, new HardwareBlockRowChangeEvent((HardwareBlockRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.HardwareBlockRowDeleted != null)
			{
				this.HardwareBlockRowDeleted(this, new HardwareBlockRowChangeEvent((HardwareBlockRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.HardwareBlockRowDeleting != null)
			{
				this.HardwareBlockRowDeleting(this, new HardwareBlockRowChangeEvent((HardwareBlockRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveHardwareBlockRow(HardwareBlockRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "HardwareBlockDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class HardwareDefinitionDataTable : TypedTableBase<HardwareDefinitionRow>
	{
		private DataColumn columnC18NDigest;

		private DataColumn columnDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn C18NDigestColumn => columnC18NDigest;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DefinitionColumn => columnDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDefinitionRow this[int index] => (HardwareDefinitionRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareDefinitionRowChangeEventHandler HardwareDefinitionRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareDefinitionRowChangeEventHandler HardwareDefinitionRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareDefinitionRowChangeEventHandler HardwareDefinitionRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareDefinitionRowChangeEventHandler HardwareDefinitionRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDefinitionDataTable()
		{
			base.TableName = "HardwareDefinition";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDefinitionDataTable(DataTable table)
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
		protected HardwareDefinitionDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddHardwareDefinitionRow(HardwareDefinitionRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDefinitionRow AddHardwareDefinitionRow(string C18NDigest, string Definition)
		{
			HardwareDefinitionRow hardwareDefinitionRow = (HardwareDefinitionRow)NewRow();
			object[] itemArray = new object[2] { C18NDigest, Definition };
			hardwareDefinitionRow.ItemArray = itemArray;
			base.Rows.Add(hardwareDefinitionRow);
			return hardwareDefinitionRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDefinitionRow FindByC18NDigest(string C18NDigest)
		{
			return (HardwareDefinitionRow)base.Rows.Find(new object[1] { C18NDigest });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			HardwareDefinitionDataTable obj = (HardwareDefinitionDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new HardwareDefinitionDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnC18NDigest = base.Columns["C18NDigest"];
			columnDefinition = base.Columns["Definition"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnC18NDigest = new DataColumn("C18NDigest", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnC18NDigest);
			columnDefinition = new DataColumn("Definition", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDefinition);
			base.Constraints.Add(new UniqueConstraint("PK_HardwareDefinition", new DataColumn[1] { columnC18NDigest }, isPrimaryKey: true));
			columnC18NDigest.AllowDBNull = false;
			columnC18NDigest.Unique = true;
			columnC18NDigest.MaxLength = 44;
			columnDefinition.AllowDBNull = false;
			columnDefinition.MaxLength = int.MaxValue;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDefinitionRow NewHardwareDefinitionRow()
		{
			return (HardwareDefinitionRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new HardwareDefinitionRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(HardwareDefinitionRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.HardwareDefinitionRowChanged != null)
			{
				this.HardwareDefinitionRowChanged(this, new HardwareDefinitionRowChangeEvent((HardwareDefinitionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.HardwareDefinitionRowChanging != null)
			{
				this.HardwareDefinitionRowChanging(this, new HardwareDefinitionRowChangeEvent((HardwareDefinitionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.HardwareDefinitionRowDeleted != null)
			{
				this.HardwareDefinitionRowDeleted(this, new HardwareDefinitionRowChangeEvent((HardwareDefinitionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.HardwareDefinitionRowDeleting != null)
			{
				this.HardwareDefinitionRowDeleting(this, new HardwareDefinitionRowChangeEvent((HardwareDefinitionRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveHardwareDefinitionRow(HardwareDefinitionRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "HardwareDefinitionDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class HardwareDriverRecorderDataTable : TypedTableBase<HardwareDriverRecorderRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDHardwareDriver;

		private DataColumn columnDriverName;

		private DataColumn columnDriverNumber;

		private DataColumn columnDriverVersion;

		private DataColumn columnXmlVersion;

		private DataColumn columnDllName;

		private DataColumn columnGroupName;

		private DataColumn columnIDRecordingServerService;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDHardwareDriverColumn => columnIDHardwareDriver;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DriverNameColumn => columnDriverName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DriverNumberColumn => columnDriverNumber;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DriverVersionColumn => columnDriverVersion;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn XmlVersionColumn => columnXmlVersion;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DllNameColumn => columnDllName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn GroupNameColumn => columnGroupName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecordingServerServiceColumn => columnIDRecordingServerService;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDriverRecorderRow this[int index] => (HardwareDriverRecorderRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareDriverRecorderRowChangeEventHandler HardwareDriverRecorderRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareDriverRecorderRowChangeEventHandler HardwareDriverRecorderRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareDriverRecorderRowChangeEventHandler HardwareDriverRecorderRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event HardwareDriverRecorderRowChangeEventHandler HardwareDriverRecorderRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDriverRecorderDataTable()
		{
			base.TableName = "HardwareDriverRecorder";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDriverRecorderDataTable(DataTable table)
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
		protected HardwareDriverRecorderDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddHardwareDriverRecorderRow(HardwareDriverRecorderRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDriverRecorderRow AddHardwareDriverRecorderRow(DateTime LastModified, string IDHardwareDriver, string DriverName, int DriverNumber, string DriverVersion, string XmlVersion, string DllName, string GroupName, string IDRecordingServerService)
		{
			HardwareDriverRecorderRow hardwareDriverRecorderRow = (HardwareDriverRecorderRow)NewRow();
			object[] itemArray = new object[9] { LastModified, IDHardwareDriver, DriverName, DriverNumber, DriverVersion, XmlVersion, DllName, GroupName, IDRecordingServerService };
			hardwareDriverRecorderRow.ItemArray = itemArray;
			base.Rows.Add(hardwareDriverRecorderRow);
			return hardwareDriverRecorderRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDriverRecorderRow FindByIDHardwareDriverIDRecordingServerService(string IDHardwareDriver, string IDRecordingServerService)
		{
			return (HardwareDriverRecorderRow)base.Rows.Find(new object[2] { IDHardwareDriver, IDRecordingServerService });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			HardwareDriverRecorderDataTable obj = (HardwareDriverRecorderDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new HardwareDriverRecorderDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDHardwareDriver = base.Columns["IDHardwareDriver"];
			columnDriverName = base.Columns["DriverName"];
			columnDriverNumber = base.Columns["DriverNumber"];
			columnDriverVersion = base.Columns["DriverVersion"];
			columnXmlVersion = base.Columns["XmlVersion"];
			columnDllName = base.Columns["DllName"];
			columnGroupName = base.Columns["GroupName"];
			columnIDRecordingServerService = base.Columns["IDRecordingServerService"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDHardwareDriver = new DataColumn("IDHardwareDriver", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDHardwareDriver);
			columnDriverName = new DataColumn("DriverName", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDriverName);
			columnDriverNumber = new DataColumn("DriverNumber", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnDriverNumber);
			columnDriverVersion = new DataColumn("DriverVersion", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDriverVersion);
			columnXmlVersion = new DataColumn("XmlVersion", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnXmlVersion);
			columnDllName = new DataColumn("DllName", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDllName);
			columnGroupName = new DataColumn("GroupName", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnGroupName);
			columnIDRecordingServerService = new DataColumn("IDRecordingServerService", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRecordingServerService);
			base.Constraints.Add(new UniqueConstraint("PK_HardwareDriverRecorder", new DataColumn[2] { columnIDHardwareDriver, columnIDRecordingServerService }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDHardwareDriver.AllowDBNull = false;
			columnIDHardwareDriver.MaxLength = 36;
			columnDriverName.AllowDBNull = false;
			columnDriverName.MaxLength = 256;
			columnDriverNumber.AllowDBNull = false;
			columnDriverVersion.AllowDBNull = false;
			columnDriverVersion.MaxLength = 128;
			columnXmlVersion.AllowDBNull = false;
			columnXmlVersion.MaxLength = 128;
			columnDllName.AllowDBNull = false;
			columnDllName.MaxLength = 128;
			columnGroupName.AllowDBNull = false;
			columnGroupName.MaxLength = 128;
			columnIDRecordingServerService.AllowDBNull = false;
			columnIDRecordingServerService.MaxLength = 36;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDriverRecorderRow NewHardwareDriverRecorderRow()
		{
			return (HardwareDriverRecorderRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new HardwareDriverRecorderRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(HardwareDriverRecorderRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.HardwareDriverRecorderRowChanged != null)
			{
				this.HardwareDriverRecorderRowChanged(this, new HardwareDriverRecorderRowChangeEvent((HardwareDriverRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.HardwareDriverRecorderRowChanging != null)
			{
				this.HardwareDriverRecorderRowChanging(this, new HardwareDriverRecorderRowChangeEvent((HardwareDriverRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.HardwareDriverRecorderRowDeleted != null)
			{
				this.HardwareDriverRecorderRowDeleted(this, new HardwareDriverRecorderRowChangeEvent((HardwareDriverRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.HardwareDriverRecorderRowDeleting != null)
			{
				this.HardwareDriverRecorderRowDeleting(this, new HardwareDriverRecorderRowChangeEvent((HardwareDriverRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveHardwareDriverRecorderRow(HardwareDriverRecorderRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "HardwareDriverRecorderDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class LicenseDataTable : TypedTableBase<LicenseRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDLicense;

		private DataColumn columnData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDLicenseColumn => columnIDLicense;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DataColumn => columnData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRow this[int index] => (LicenseRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseRowChangeEventHandler LicenseRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseRowChangeEventHandler LicenseRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseRowChangeEventHandler LicenseRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseRowChangeEventHandler LicenseRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDataTable()
		{
			base.TableName = "License";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDataTable(DataTable table)
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
		protected LicenseDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddLicenseRow(LicenseRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRow AddLicenseRow(DateTime LastModified, Guid IDLicense, byte[] Data)
		{
			LicenseRow licenseRow = (LicenseRow)NewRow();
			object[] itemArray = new object[3] { LastModified, IDLicense, Data };
			licenseRow.ItemArray = itemArray;
			base.Rows.Add(licenseRow);
			return licenseRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRow FindByIDLicense(Guid IDLicense)
		{
			return (LicenseRow)base.Rows.Find(new object[1] { IDLicense });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			LicenseDataTable obj = (LicenseDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new LicenseDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDLicense = base.Columns["IDLicense"];
			columnData = base.Columns["Data"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDLicense = new DataColumn("IDLicense", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDLicense);
			columnData = new DataColumn("Data", typeof(byte[]), null, MappingType.Element);
			base.Columns.Add(columnData);
			base.Constraints.Add(new UniqueConstraint("PK_License", new DataColumn[1] { columnIDLicense }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDLicense.AllowDBNull = false;
			columnIDLicense.Unique = true;
			columnData.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRow NewLicenseRow()
		{
			return (LicenseRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new LicenseRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(LicenseRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.LicenseRowChanged != null)
			{
				this.LicenseRowChanged(this, new LicenseRowChangeEvent((LicenseRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.LicenseRowChanging != null)
			{
				this.LicenseRowChanging(this, new LicenseRowChangeEvent((LicenseRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.LicenseRowDeleted != null)
			{
				this.LicenseRowDeleted(this, new LicenseRowChangeEvent((LicenseRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.LicenseRowDeleting != null)
			{
				this.LicenseRowDeleting(this, new LicenseRowChangeEvent((LicenseRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveLicenseRow(LicenseRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "LicenseDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class MatrixDataTable : TypedTableBase<MatrixRow>
	{
		private DataColumn columnIDMatrix;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnAddress;

		private DataColumn columnPort;

		private DataColumn columnPassword;

		private DataColumn columnLastUpdated;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDMatrixColumn => columnIDMatrix;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn AddressColumn => columnAddress;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PortColumn => columnPort;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PasswordColumn => columnPassword;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastUpdatedColumn => columnLastUpdated;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MatrixRow this[int index] => (MatrixRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event MatrixRowChangeEventHandler MatrixRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event MatrixRowChangeEventHandler MatrixRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event MatrixRowChangeEventHandler MatrixRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event MatrixRowChangeEventHandler MatrixRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MatrixDataTable()
		{
			base.TableName = "Matrix";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MatrixDataTable(DataTable table)
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
		protected MatrixDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddMatrixRow(MatrixRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MatrixRow AddMatrixRow(string IDMatrix, string Name, string Description, string Address, int Port, string Password, DateTime LastUpdated)
		{
			MatrixRow matrixRow = (MatrixRow)NewRow();
			object[] itemArray = new object[7] { IDMatrix, Name, Description, Address, Port, Password, LastUpdated };
			matrixRow.ItemArray = itemArray;
			base.Rows.Add(matrixRow);
			return matrixRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MatrixRow FindByIDMatrix(string IDMatrix)
		{
			return (MatrixRow)base.Rows.Find(new object[1] { IDMatrix });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			MatrixDataTable obj = (MatrixDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new MatrixDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDMatrix = base.Columns["IDMatrix"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnAddress = base.Columns["Address"];
			columnPort = base.Columns["Port"];
			columnPassword = base.Columns["Password"];
			columnLastUpdated = base.Columns["LastUpdated"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDMatrix = new DataColumn("IDMatrix", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDMatrix);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnAddress = new DataColumn("Address", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnAddress);
			columnPort = new DataColumn("Port", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnPort);
			columnPassword = new DataColumn("Password", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnPassword);
			columnLastUpdated = new DataColumn("LastUpdated", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastUpdated);
			base.Constraints.Add(new UniqueConstraint("PK_Matrix", new DataColumn[1] { columnIDMatrix }, isPrimaryKey: true));
			columnIDMatrix.AllowDBNull = false;
			columnIDMatrix.Unique = true;
			columnIDMatrix.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
			columnAddress.AllowDBNull = false;
			columnAddress.MaxLength = 1073741823;
			columnPort.AllowDBNull = false;
			columnPassword.MaxLength = 256;
			columnLastUpdated.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MatrixRow NewMatrixRow()
		{
			return (MatrixRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new MatrixRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(MatrixRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.MatrixRowChanged != null)
			{
				this.MatrixRowChanged(this, new MatrixRowChangeEvent((MatrixRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.MatrixRowChanging != null)
			{
				this.MatrixRowChanging(this, new MatrixRowChangeEvent((MatrixRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.MatrixRowDeleted != null)
			{
				this.MatrixRowDeleted(this, new MatrixRowChangeEvent((MatrixRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.MatrixRowDeleting != null)
			{
				this.MatrixRowDeleting(this, new MatrixRowChangeEvent((MatrixRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveMatrixRow(MatrixRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "MatrixDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class MIPObjectDataTable : TypedTableBase<MIPObjectRow>
	{
		private DataColumn columnIDMIPObject;

		private DataColumn columnLastModified;

		private DataColumn columnCreatedDate;

		private DataColumn columnName;

		private DataColumn columnKind;

		private DataColumn columnSubKind;

		private DataColumn columnPluginId;

		private DataColumn columnCustomData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDMIPObjectColumn => columnIDMIPObject;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn CreatedDateColumn => columnCreatedDate;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn KindColumn => columnKind;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SubKindColumn => columnSubKind;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PluginIdColumn => columnPluginId;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn CustomDataColumn => columnCustomData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MIPObjectRow this[int index] => (MIPObjectRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event MIPObjectRowChangeEventHandler MIPObjectRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event MIPObjectRowChangeEventHandler MIPObjectRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event MIPObjectRowChangeEventHandler MIPObjectRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event MIPObjectRowChangeEventHandler MIPObjectRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MIPObjectDataTable()
		{
			base.TableName = "MIPObject";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MIPObjectDataTable(DataTable table)
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
		protected MIPObjectDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddMIPObjectRow(MIPObjectRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MIPObjectRow AddMIPObjectRow(Guid IDMIPObject, DateTime LastModified, DateTime CreatedDate, string Name, Guid Kind, Guid SubKind, Guid PluginId, string CustomData)
		{
			MIPObjectRow mIPObjectRow = (MIPObjectRow)NewRow();
			object[] itemArray = new object[8] { IDMIPObject, LastModified, CreatedDate, Name, Kind, SubKind, PluginId, CustomData };
			mIPObjectRow.ItemArray = itemArray;
			base.Rows.Add(mIPObjectRow);
			return mIPObjectRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MIPObjectRow FindByIDMIPObject(Guid IDMIPObject)
		{
			return (MIPObjectRow)base.Rows.Find(new object[1] { IDMIPObject });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			MIPObjectDataTable obj = (MIPObjectDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new MIPObjectDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDMIPObject = base.Columns["IDMIPObject"];
			columnLastModified = base.Columns["LastModified"];
			columnCreatedDate = base.Columns["CreatedDate"];
			columnName = base.Columns["Name"];
			columnKind = base.Columns["Kind"];
			columnSubKind = base.Columns["SubKind"];
			columnPluginId = base.Columns["PluginId"];
			columnCustomData = base.Columns["CustomData"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDMIPObject = new DataColumn("IDMIPObject", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDMIPObject);
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnCreatedDate = new DataColumn("CreatedDate", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnCreatedDate);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnKind = new DataColumn("Kind", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnKind);
			columnSubKind = new DataColumn("SubKind", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnSubKind);
			columnPluginId = new DataColumn("PluginId", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnPluginId);
			columnCustomData = new DataColumn("CustomData", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnCustomData);
			base.Constraints.Add(new UniqueConstraint("PK_MIPObject", new DataColumn[1] { columnIDMIPObject }, isPrimaryKey: true));
			columnIDMIPObject.AllowDBNull = false;
			columnIDMIPObject.Unique = true;
			columnLastModified.AllowDBNull = false;
			columnCreatedDate.AllowDBNull = false;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnKind.AllowDBNull = false;
			columnSubKind.AllowDBNull = false;
			columnPluginId.AllowDBNull = false;
			columnCustomData.MaxLength = int.MaxValue;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MIPObjectRow NewMIPObjectRow()
		{
			return (MIPObjectRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new MIPObjectRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(MIPObjectRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.MIPObjectRowChanged != null)
			{
				this.MIPObjectRowChanged(this, new MIPObjectRowChangeEvent((MIPObjectRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.MIPObjectRowChanging != null)
			{
				this.MIPObjectRowChanging(this, new MIPObjectRowChangeEvent((MIPObjectRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.MIPObjectRowDeleted != null)
			{
				this.MIPObjectRowDeleted(this, new MIPObjectRowChangeEvent((MIPObjectRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.MIPObjectRowDeleting != null)
			{
				this.MIPObjectRowDeleting(this, new MIPObjectRowChangeEvent((MIPObjectRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveMIPObjectRow(MIPObjectRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "MIPObjectDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class NotificationProfilesDataTable : TypedTableBase<NotificationProfilesRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDNotificationProfile;

		private DataColumn columnName;

		private DataColumn columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDNotificationProfileColumn => columnIDNotificationProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationProfilesRow this[int index] => (NotificationProfilesRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationProfilesRowChangeEventHandler NotificationProfilesRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationProfilesRowChangeEventHandler NotificationProfilesRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationProfilesRowChangeEventHandler NotificationProfilesRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationProfilesRowChangeEventHandler NotificationProfilesRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationProfilesDataTable()
		{
			base.TableName = "NotificationProfiles";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationProfilesDataTable(DataTable table)
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
		protected NotificationProfilesDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddNotificationProfilesRow(NotificationProfilesRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationProfilesRow AddNotificationProfilesRow(DateTime LastModified, string IDNotificationProfile, string Name, string Description)
		{
			NotificationProfilesRow notificationProfilesRow = (NotificationProfilesRow)NewRow();
			object[] itemArray = new object[4] { LastModified, IDNotificationProfile, Name, Description };
			notificationProfilesRow.ItemArray = itemArray;
			base.Rows.Add(notificationProfilesRow);
			return notificationProfilesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationProfilesRow FindByIDNotificationProfile(string IDNotificationProfile)
		{
			return (NotificationProfilesRow)base.Rows.Find(new object[1] { IDNotificationProfile });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			NotificationProfilesDataTable obj = (NotificationProfilesDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new NotificationProfilesDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDNotificationProfile = base.Columns["IDNotificationProfile"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDNotificationProfile = new DataColumn("IDNotificationProfile", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDNotificationProfile);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			base.Constraints.Add(new UniqueConstraint("PK_NotificationProfiles", new DataColumn[1] { columnIDNotificationProfile }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDNotificationProfile.AllowDBNull = false;
			columnIDNotificationProfile.Unique = true;
			columnIDNotificationProfile.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationProfilesRow NewNotificationProfilesRow()
		{
			return (NotificationProfilesRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new NotificationProfilesRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(NotificationProfilesRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.NotificationProfilesRowChanged != null)
			{
				this.NotificationProfilesRowChanged(this, new NotificationProfilesRowChangeEvent((NotificationProfilesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.NotificationProfilesRowChanging != null)
			{
				this.NotificationProfilesRowChanging(this, new NotificationProfilesRowChangeEvent((NotificationProfilesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.NotificationProfilesRowDeleted != null)
			{
				this.NotificationProfilesRowDeleted(this, new NotificationProfilesRowChangeEvent((NotificationProfilesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.NotificationProfilesRowDeleting != null)
			{
				this.NotificationProfilesRowDeleting(this, new NotificationProfilesRowChangeEvent((NotificationProfilesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveNotificationProfilesRow(NotificationProfilesRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "NotificationProfilesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class NotificationTypesDataTable : TypedTableBase<NotificationTypesRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDNotificationType;

		private DataColumn columnName;

		private DataColumn columnPageName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDNotificationTypeColumn => columnIDNotificationType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PageNameColumn => columnPageName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationTypesRow this[int index] => (NotificationTypesRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationTypesRowChangeEventHandler NotificationTypesRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationTypesRowChangeEventHandler NotificationTypesRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationTypesRowChangeEventHandler NotificationTypesRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event NotificationTypesRowChangeEventHandler NotificationTypesRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationTypesDataTable()
		{
			base.TableName = "NotificationTypes";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationTypesDataTable(DataTable table)
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
		protected NotificationTypesDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddNotificationTypesRow(NotificationTypesRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationTypesRow AddNotificationTypesRow(DateTime LastModified, string IDNotificationType, string Name, string PageName)
		{
			NotificationTypesRow notificationTypesRow = (NotificationTypesRow)NewRow();
			object[] itemArray = new object[4] { LastModified, IDNotificationType, Name, PageName };
			notificationTypesRow.ItemArray = itemArray;
			base.Rows.Add(notificationTypesRow);
			return notificationTypesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationTypesRow FindByIDNotificationType(string IDNotificationType)
		{
			return (NotificationTypesRow)base.Rows.Find(new object[1] { IDNotificationType });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			NotificationTypesDataTable obj = (NotificationTypesDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new NotificationTypesDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDNotificationType = base.Columns["IDNotificationType"];
			columnName = base.Columns["Name"];
			columnPageName = base.Columns["PageName"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDNotificationType = new DataColumn("IDNotificationType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDNotificationType);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnPageName = new DataColumn("PageName", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnPageName);
			base.Constraints.Add(new UniqueConstraint("PK_NotificationTypes", new DataColumn[1] { columnIDNotificationType }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDNotificationType.AllowDBNull = false;
			columnIDNotificationType.Unique = true;
			columnIDNotificationType.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnPageName.AllowDBNull = false;
			columnPageName.MaxLength = 256;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationTypesRow NewNotificationTypesRow()
		{
			return (NotificationTypesRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new NotificationTypesRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(NotificationTypesRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.NotificationTypesRowChanged != null)
			{
				this.NotificationTypesRowChanged(this, new NotificationTypesRowChangeEvent((NotificationTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.NotificationTypesRowChanging != null)
			{
				this.NotificationTypesRowChanging(this, new NotificationTypesRowChangeEvent((NotificationTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.NotificationTypesRowDeleted != null)
			{
				this.NotificationTypesRowDeleted(this, new NotificationTypesRowChangeEvent((NotificationTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.NotificationTypesRowDeleting != null)
			{
				this.NotificationTypesRowDeleting(this, new NotificationTypesRowChangeEvent((NotificationTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveNotificationTypesRow(NotificationTypesRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "NotificationTypesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class PatrollingProfilesDataTable : TypedTableBase<PatrollingProfilesRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDPatrollingProfile;

		private DataColumn columnIDDevice;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDPatrollingProfileColumn => columnIDPatrollingProfile;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DefinitionColumn => columnDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public PatrollingProfilesRow this[int index] => (PatrollingProfilesRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event PatrollingProfilesRowChangeEventHandler PatrollingProfilesRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event PatrollingProfilesRowChangeEventHandler PatrollingProfilesRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event PatrollingProfilesRowChangeEventHandler PatrollingProfilesRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event PatrollingProfilesRowChangeEventHandler PatrollingProfilesRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public PatrollingProfilesDataTable()
		{
			base.TableName = "PatrollingProfiles";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public PatrollingProfilesDataTable(DataTable table)
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
		protected PatrollingProfilesDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddPatrollingProfilesRow(PatrollingProfilesRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public PatrollingProfilesRow AddPatrollingProfilesRow(DateTime LastModified, string IDPatrollingProfile, string IDDevice, string Name, string Description, string Definition)
		{
			PatrollingProfilesRow patrollingProfilesRow = (PatrollingProfilesRow)NewRow();
			object[] itemArray = new object[6] { LastModified, IDPatrollingProfile, IDDevice, Name, Description, Definition };
			patrollingProfilesRow.ItemArray = itemArray;
			base.Rows.Add(patrollingProfilesRow);
			return patrollingProfilesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public PatrollingProfilesRow FindByIDPatrollingProfile(string IDPatrollingProfile)
		{
			return (PatrollingProfilesRow)base.Rows.Find(new object[1] { IDPatrollingProfile });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			PatrollingProfilesDataTable obj = (PatrollingProfilesDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new PatrollingProfilesDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDPatrollingProfile = base.Columns["IDPatrollingProfile"];
			columnIDDevice = base.Columns["IDDevice"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnDefinition = base.Columns["Definition"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDPatrollingProfile = new DataColumn("IDPatrollingProfile", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDPatrollingProfile);
			columnIDDevice = new DataColumn("IDDevice", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnDefinition = new DataColumn("Definition", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDefinition);
			base.Constraints.Add(new UniqueConstraint("PK_PatrollingProfiles", new DataColumn[1] { columnIDPatrollingProfile }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDPatrollingProfile.AllowDBNull = false;
			columnIDPatrollingProfile.Unique = true;
			columnIDPatrollingProfile.MaxLength = 36;
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
			columnDefinition.AllowDBNull = false;
			columnDefinition.MaxLength = 1073741823;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public PatrollingProfilesRow NewPatrollingProfilesRow()
		{
			return (PatrollingProfilesRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new PatrollingProfilesRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(PatrollingProfilesRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.PatrollingProfilesRowChanged != null)
			{
				this.PatrollingProfilesRowChanged(this, new PatrollingProfilesRowChangeEvent((PatrollingProfilesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.PatrollingProfilesRowChanging != null)
			{
				this.PatrollingProfilesRowChanging(this, new PatrollingProfilesRowChangeEvent((PatrollingProfilesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.PatrollingProfilesRowDeleted != null)
			{
				this.PatrollingProfilesRowDeleted(this, new PatrollingProfilesRowChangeEvent((PatrollingProfilesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.PatrollingProfilesRowDeleting != null)
			{
				this.PatrollingProfilesRowDeleting(this, new PatrollingProfilesRowChangeEvent((PatrollingProfilesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemovePatrollingProfilesRow(PatrollingProfilesRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "PatrollingProfilesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class RecorderPluginDeviceDataTable : TypedTableBase<RecorderPluginDeviceRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDDevice;

		private DataColumn columnProcessOrder;

		private DataColumn columnIDRecorderPlugin;

		private DataColumn columnSettings;

		private DataColumn columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ProcessOrderColumn => columnProcessOrder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderPluginColumn => columnIDRecorderPlugin;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SettingsColumn => columnSettings;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginDeviceRow this[int index] => (RecorderPluginDeviceRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderPluginDeviceRowChangeEventHandler RecorderPluginDeviceRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderPluginDeviceRowChangeEventHandler RecorderPluginDeviceRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderPluginDeviceRowChangeEventHandler RecorderPluginDeviceRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderPluginDeviceRowChangeEventHandler RecorderPluginDeviceRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginDeviceDataTable()
		{
			base.TableName = "RecorderPluginDevice";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginDeviceDataTable(DataTable table)
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
		protected RecorderPluginDeviceDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRecorderPluginDeviceRow(RecorderPluginDeviceRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginDeviceRow AddRecorderPluginDeviceRow(DateTime LastModified, string IDDevice, int ProcessOrder, string IDRecorderPlugin, string Settings, bool Enabled)
		{
			RecorderPluginDeviceRow recorderPluginDeviceRow = (RecorderPluginDeviceRow)NewRow();
			object[] itemArray = new object[6] { LastModified, IDDevice, ProcessOrder, IDRecorderPlugin, Settings, Enabled };
			recorderPluginDeviceRow.ItemArray = itemArray;
			base.Rows.Add(recorderPluginDeviceRow);
			return recorderPluginDeviceRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginDeviceRow FindByIDDeviceIDRecorderPlugin(string IDDevice, string IDRecorderPlugin)
		{
			return (RecorderPluginDeviceRow)base.Rows.Find(new object[2] { IDDevice, IDRecorderPlugin });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RecorderPluginDeviceDataTable obj = (RecorderPluginDeviceDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RecorderPluginDeviceDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDDevice = base.Columns["IDDevice"];
			columnProcessOrder = base.Columns["ProcessOrder"];
			columnIDRecorderPlugin = base.Columns["IDRecorderPlugin"];
			columnSettings = base.Columns["Settings"];
			columnEnabled = base.Columns["Enabled"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDDevice = new DataColumn("IDDevice", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnProcessOrder = new DataColumn("ProcessOrder", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnProcessOrder);
			columnIDRecorderPlugin = new DataColumn("IDRecorderPlugin", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRecorderPlugin);
			columnSettings = new DataColumn("Settings", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSettings);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			base.Constraints.Add(new UniqueConstraint("PK_RecorderPluginDevice", new DataColumn[2] { columnIDDevice, columnIDRecorderPlugin }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.MaxLength = 36;
			columnProcessOrder.AllowDBNull = false;
			columnIDRecorderPlugin.AllowDBNull = false;
			columnIDRecorderPlugin.MaxLength = 36;
			columnSettings.AllowDBNull = false;
			columnSettings.MaxLength = 1073741823;
			columnEnabled.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginDeviceRow NewRecorderPluginDeviceRow()
		{
			return (RecorderPluginDeviceRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RecorderPluginDeviceRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RecorderPluginDeviceRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RecorderPluginDeviceRowChanged != null)
			{
				this.RecorderPluginDeviceRowChanged(this, new RecorderPluginDeviceRowChangeEvent((RecorderPluginDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RecorderPluginDeviceRowChanging != null)
			{
				this.RecorderPluginDeviceRowChanging(this, new RecorderPluginDeviceRowChangeEvent((RecorderPluginDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RecorderPluginDeviceRowDeleted != null)
			{
				this.RecorderPluginDeviceRowDeleted(this, new RecorderPluginDeviceRowChangeEvent((RecorderPluginDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RecorderPluginDeviceRowDeleting != null)
			{
				this.RecorderPluginDeviceRowDeleting(this, new RecorderPluginDeviceRowChangeEvent((RecorderPluginDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRecorderPluginDeviceRow(RecorderPluginDeviceRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RecorderPluginDeviceDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class RecorderPluginsDataTable : TypedTableBase<RecorderPluginsRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDRecorderPlugin;

		private DataColumn columnName;

		private DataColumn columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderPluginColumn => columnIDRecorderPlugin;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginsRow this[int index] => (RecorderPluginsRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderPluginsRowChangeEventHandler RecorderPluginsRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderPluginsRowChangeEventHandler RecorderPluginsRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderPluginsRowChangeEventHandler RecorderPluginsRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecorderPluginsRowChangeEventHandler RecorderPluginsRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginsDataTable()
		{
			base.TableName = "RecorderPlugins";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginsDataTable(DataTable table)
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
		protected RecorderPluginsDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRecorderPluginsRow(RecorderPluginsRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginsRow AddRecorderPluginsRow(DateTime LastModified, string IDRecorderPlugin, string Name, string Description)
		{
			RecorderPluginsRow recorderPluginsRow = (RecorderPluginsRow)NewRow();
			object[] itemArray = new object[4] { LastModified, IDRecorderPlugin, Name, Description };
			recorderPluginsRow.ItemArray = itemArray;
			base.Rows.Add(recorderPluginsRow);
			return recorderPluginsRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginsRow FindByIDRecorderPlugin(string IDRecorderPlugin)
		{
			return (RecorderPluginsRow)base.Rows.Find(new object[1] { IDRecorderPlugin });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RecorderPluginsDataTable obj = (RecorderPluginsDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RecorderPluginsDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDRecorderPlugin = base.Columns["IDRecorderPlugin"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDRecorderPlugin = new DataColumn("IDRecorderPlugin", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRecorderPlugin);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			base.Constraints.Add(new UniqueConstraint("PK_RecorderPlugins", new DataColumn[1] { columnIDRecorderPlugin }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDRecorderPlugin.AllowDBNull = false;
			columnIDRecorderPlugin.Unique = true;
			columnIDRecorderPlugin.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginsRow NewRecorderPluginsRow()
		{
			return (RecorderPluginsRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RecorderPluginsRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RecorderPluginsRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RecorderPluginsRowChanged != null)
			{
				this.RecorderPluginsRowChanged(this, new RecorderPluginsRowChangeEvent((RecorderPluginsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RecorderPluginsRowChanging != null)
			{
				this.RecorderPluginsRowChanging(this, new RecorderPluginsRowChangeEvent((RecorderPluginsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RecorderPluginsRowDeleted != null)
			{
				this.RecorderPluginsRowDeleted(this, new RecorderPluginsRowChangeEvent((RecorderPluginsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RecorderPluginsRowDeleting != null)
			{
				this.RecorderPluginsRowDeleting(this, new RecorderPluginsRowChangeEvent((RecorderPluginsRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRecorderPluginsRow(RecorderPluginsRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RecorderPluginsDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class RecordersDataTable : TypedTableBase<RecordersRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDRecorder;

		private DataColumn columnEnabled;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnIDFailoverPrimaryGroup;

		private DataColumn columnIDFailoverSecondaryGroup;

		private DataColumn columnMulticastSettings;

		private DataColumn columnSynchronizationTime;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderColumn => columnIDRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDFailoverPrimaryGroupColumn => columnIDFailoverPrimaryGroup;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDFailoverSecondaryGroupColumn => columnIDFailoverSecondaryGroup;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn MulticastSettingsColumn => columnMulticastSettings;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SynchronizationTimeColumn => columnSynchronizationTime;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersRow this[int index] => (RecordersRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordersRowChangeEventHandler RecordersRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordersRowChangeEventHandler RecordersRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordersRowChangeEventHandler RecordersRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordersRowChangeEventHandler RecordersRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersDataTable()
		{
			base.TableName = "Recorders";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersDataTable(DataTable table)
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
		protected RecordersDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRecordersRow(RecordersRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersRow AddRecordersRow(DateTime LastModified, string IDRecorder, bool Enabled, string Name, string Description, string IDFailoverPrimaryGroup, string IDFailoverSecondaryGroup, string MulticastSettings, int SynchronizationTime)
		{
			RecordersRow recordersRow = (RecordersRow)NewRow();
			object[] itemArray = new object[9] { LastModified, IDRecorder, Enabled, Name, Description, IDFailoverPrimaryGroup, IDFailoverSecondaryGroup, MulticastSettings, SynchronizationTime };
			recordersRow.ItemArray = itemArray;
			base.Rows.Add(recordersRow);
			return recordersRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersRow FindByIDRecorder(string IDRecorder)
		{
			return (RecordersRow)base.Rows.Find(new object[1] { IDRecorder });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RecordersDataTable obj = (RecordersDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RecordersDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDRecorder = base.Columns["IDRecorder"];
			columnEnabled = base.Columns["Enabled"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnIDFailoverPrimaryGroup = base.Columns["IDFailoverPrimaryGroup"];
			columnIDFailoverSecondaryGroup = base.Columns["IDFailoverSecondaryGroup"];
			columnMulticastSettings = base.Columns["MulticastSettings"];
			columnSynchronizationTime = base.Columns["SynchronizationTime"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDRecorder = new DataColumn("IDRecorder", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRecorder);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnIDFailoverPrimaryGroup = new DataColumn("IDFailoverPrimaryGroup", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDFailoverPrimaryGroup);
			columnIDFailoverSecondaryGroup = new DataColumn("IDFailoverSecondaryGroup", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDFailoverSecondaryGroup);
			columnMulticastSettings = new DataColumn("MulticastSettings", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnMulticastSettings);
			columnSynchronizationTime = new DataColumn("SynchronizationTime", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnSynchronizationTime);
			base.Constraints.Add(new UniqueConstraint("PK_Recorders", new DataColumn[1] { columnIDRecorder }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDRecorder.AllowDBNull = false;
			columnIDRecorder.Unique = true;
			columnIDRecorder.MaxLength = 36;
			columnEnabled.AllowDBNull = false;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
			columnIDFailoverPrimaryGroup.MaxLength = 36;
			columnIDFailoverSecondaryGroup.MaxLength = 36;
			columnMulticastSettings.AllowDBNull = false;
			columnMulticastSettings.MaxLength = 1073741823;
			columnSynchronizationTime.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersRow NewRecordersRow()
		{
			return (RecordersRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RecordersRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RecordersRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RecordersRowChanged != null)
			{
				this.RecordersRowChanged(this, new RecordersRowChangeEvent((RecordersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RecordersRowChanging != null)
			{
				this.RecordersRowChanging(this, new RecordersRowChangeEvent((RecordersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RecordersRowDeleted != null)
			{
				this.RecordersRowDeleted(this, new RecordersRowChangeEvent((RecordersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RecordersRowDeleting != null)
			{
				this.RecordersRowDeleting(this, new RecordersRowChangeEvent((RecordersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRecordersRow(RecordersRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RecordersDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class RecordingServerServiceConfigurableDataTable : TypedTableBase<RecordingServerServiceConfigurableRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnPKID;

		private DataColumn columnMulticastServerAddress;

		private DataColumn columnShutdownOnStorageFailure;

		private DataColumn columnPublicAccessEnabled;

		private DataColumn columnPublicWebServerHostname;

		private DataColumn columnPublicWebServerPort;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PKIDColumn => columnPKID;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn MulticastServerAddressColumn => columnMulticastServerAddress;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ShutdownOnStorageFailureColumn => columnShutdownOnStorageFailure;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PublicAccessEnabledColumn => columnPublicAccessEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PublicWebServerHostnameColumn => columnPublicWebServerHostname;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PublicWebServerPortColumn => columnPublicWebServerPort;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceConfigurableRow this[int index] => (RecordingServerServiceConfigurableRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceConfigurableRowChangeEventHandler RecordingServerServiceConfigurableRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceConfigurableRowChangeEventHandler RecordingServerServiceConfigurableRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceConfigurableRowChangeEventHandler RecordingServerServiceConfigurableRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RecordingServerServiceConfigurableRowChangeEventHandler RecordingServerServiceConfigurableRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceConfigurableDataTable()
		{
			base.TableName = "RecordingServerServiceConfigurable";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceConfigurableDataTable(DataTable table)
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
		protected RecordingServerServiceConfigurableDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRecordingServerServiceConfigurableRow(RecordingServerServiceConfigurableRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceConfigurableRow AddRecordingServerServiceConfigurableRow(DateTime LastModified, string PKID, string MulticastServerAddress, bool ShutdownOnStorageFailure, bool PublicAccessEnabled, string PublicWebServerHostname, int PublicWebServerPort)
		{
			RecordingServerServiceConfigurableRow recordingServerServiceConfigurableRow = (RecordingServerServiceConfigurableRow)NewRow();
			object[] itemArray = new object[7] { LastModified, PKID, MulticastServerAddress, ShutdownOnStorageFailure, PublicAccessEnabled, PublicWebServerHostname, PublicWebServerPort };
			recordingServerServiceConfigurableRow.ItemArray = itemArray;
			base.Rows.Add(recordingServerServiceConfigurableRow);
			return recordingServerServiceConfigurableRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceConfigurableRow FindByPKID(string PKID)
		{
			return (RecordingServerServiceConfigurableRow)base.Rows.Find(new object[1] { PKID });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RecordingServerServiceConfigurableDataTable obj = (RecordingServerServiceConfigurableDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RecordingServerServiceConfigurableDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnPKID = base.Columns["PKID"];
			columnMulticastServerAddress = base.Columns["MulticastServerAddress"];
			columnShutdownOnStorageFailure = base.Columns["ShutdownOnStorageFailure"];
			columnPublicAccessEnabled = base.Columns["PublicAccessEnabled"];
			columnPublicWebServerHostname = base.Columns["PublicWebServerHostname"];
			columnPublicWebServerPort = base.Columns["PublicWebServerPort"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnPKID = new DataColumn("PKID", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnPKID);
			columnMulticastServerAddress = new DataColumn("MulticastServerAddress", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnMulticastServerAddress);
			columnShutdownOnStorageFailure = new DataColumn("ShutdownOnStorageFailure", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnShutdownOnStorageFailure);
			columnPublicAccessEnabled = new DataColumn("PublicAccessEnabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnPublicAccessEnabled);
			columnPublicWebServerHostname = new DataColumn("PublicWebServerHostname", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnPublicWebServerHostname);
			columnPublicWebServerPort = new DataColumn("PublicWebServerPort", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnPublicWebServerPort);
			base.Constraints.Add(new UniqueConstraint("PK_RecordingServerServiceConfigurable", new DataColumn[1] { columnPKID }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnPKID.AllowDBNull = false;
			columnPKID.Unique = true;
			columnPKID.MaxLength = 36;
			columnMulticastServerAddress.AllowDBNull = false;
			columnMulticastServerAddress.MaxLength = 256;
			columnShutdownOnStorageFailure.AllowDBNull = false;
			columnPublicAccessEnabled.AllowDBNull = false;
			columnPublicWebServerHostname.AllowDBNull = false;
			columnPublicWebServerHostname.MaxLength = 256;
			columnPublicWebServerPort.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceConfigurableRow NewRecordingServerServiceConfigurableRow()
		{
			return (RecordingServerServiceConfigurableRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RecordingServerServiceConfigurableRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RecordingServerServiceConfigurableRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RecordingServerServiceConfigurableRowChanged != null)
			{
				this.RecordingServerServiceConfigurableRowChanged(this, new RecordingServerServiceConfigurableRowChangeEvent((RecordingServerServiceConfigurableRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RecordingServerServiceConfigurableRowChanging != null)
			{
				this.RecordingServerServiceConfigurableRowChanging(this, new RecordingServerServiceConfigurableRowChangeEvent((RecordingServerServiceConfigurableRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RecordingServerServiceConfigurableRowDeleted != null)
			{
				this.RecordingServerServiceConfigurableRowDeleted(this, new RecordingServerServiceConfigurableRowChangeEvent((RecordingServerServiceConfigurableRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RecordingServerServiceConfigurableRowDeleting != null)
			{
				this.RecordingServerServiceConfigurableRowDeleting(this, new RecordingServerServiceConfigurableRowChangeEvent((RecordingServerServiceConfigurableRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRecordingServerServiceConfigurableRow(RecordingServerServiceConfigurableRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RecordingServerServiceConfigurableDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class RegisteredDeviceTypesDataTable : TypedTableBase<RegisteredDeviceTypesRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnDeviceType;

		private DataColumn columnDescription;

		private DataColumn columnSortOrder;

		private DataColumn columnPrimaryIconNumber;

		private DataColumn columnIsRecordingDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DeviceTypeColumn => columnDeviceType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SortOrderColumn => columnSortOrder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PrimaryIconNumberColumn => columnPrimaryIconNumber;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IsRecordingDeviceColumn => columnIsRecordingDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredDeviceTypesRow this[int index] => (RegisteredDeviceTypesRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RegisteredDeviceTypesRowChangeEventHandler RegisteredDeviceTypesRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RegisteredDeviceTypesRowChangeEventHandler RegisteredDeviceTypesRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RegisteredDeviceTypesRowChangeEventHandler RegisteredDeviceTypesRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RegisteredDeviceTypesRowChangeEventHandler RegisteredDeviceTypesRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredDeviceTypesDataTable()
		{
			base.TableName = "RegisteredDeviceTypes";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredDeviceTypesDataTable(DataTable table)
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
		protected RegisteredDeviceTypesDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRegisteredDeviceTypesRow(RegisteredDeviceTypesRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredDeviceTypesRow AddRegisteredDeviceTypesRow(DateTime LastModified, string DeviceType, string Description, int SortOrder, int PrimaryIconNumber, bool IsRecordingDevice)
		{
			RegisteredDeviceTypesRow registeredDeviceTypesRow = (RegisteredDeviceTypesRow)NewRow();
			object[] itemArray = new object[6] { LastModified, DeviceType, Description, SortOrder, PrimaryIconNumber, IsRecordingDevice };
			registeredDeviceTypesRow.ItemArray = itemArray;
			base.Rows.Add(registeredDeviceTypesRow);
			return registeredDeviceTypesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredDeviceTypesRow FindByDeviceType(string DeviceType)
		{
			return (RegisteredDeviceTypesRow)base.Rows.Find(new object[1] { DeviceType });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RegisteredDeviceTypesDataTable obj = (RegisteredDeviceTypesDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RegisteredDeviceTypesDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnDeviceType = base.Columns["DeviceType"];
			columnDescription = base.Columns["Description"];
			columnSortOrder = base.Columns["SortOrder"];
			columnPrimaryIconNumber = base.Columns["PrimaryIconNumber"];
			columnIsRecordingDevice = base.Columns["IsRecordingDevice"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnDeviceType = new DataColumn("DeviceType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDeviceType);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnSortOrder = new DataColumn("SortOrder", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnSortOrder);
			columnPrimaryIconNumber = new DataColumn("PrimaryIconNumber", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnPrimaryIconNumber);
			columnIsRecordingDevice = new DataColumn("IsRecordingDevice", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnIsRecordingDevice);
			base.Constraints.Add(new UniqueConstraint("PK_RegisteredDeviceTypes", new DataColumn[1] { columnDeviceType }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnDeviceType.AllowDBNull = false;
			columnDeviceType.Unique = true;
			columnDeviceType.MaxLength = 100;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
			columnSortOrder.AllowDBNull = false;
			columnPrimaryIconNumber.AllowDBNull = false;
			columnIsRecordingDevice.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredDeviceTypesRow NewRegisteredDeviceTypesRow()
		{
			return (RegisteredDeviceTypesRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RegisteredDeviceTypesRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RegisteredDeviceTypesRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RegisteredDeviceTypesRowChanged != null)
			{
				this.RegisteredDeviceTypesRowChanged(this, new RegisteredDeviceTypesRowChangeEvent((RegisteredDeviceTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RegisteredDeviceTypesRowChanging != null)
			{
				this.RegisteredDeviceTypesRowChanging(this, new RegisteredDeviceTypesRowChangeEvent((RegisteredDeviceTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RegisteredDeviceTypesRowDeleted != null)
			{
				this.RegisteredDeviceTypesRowDeleted(this, new RegisteredDeviceTypesRowChangeEvent((RegisteredDeviceTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RegisteredDeviceTypesRowDeleting != null)
			{
				this.RegisteredDeviceTypesRowDeleting(this, new RegisteredDeviceTypesRowChangeEvent((RegisteredDeviceTypesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRegisteredDeviceTypesRow(RegisteredDeviceTypesRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RegisteredDeviceTypesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class RegisteredServersDataTable : TypedTableBase<RegisteredServersRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDServer;

		private DataColumn columnType;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnAddresses;

		private DataColumn columnTrusted;

		private DataColumn columnCustomData;

		private DataColumn columnEnabled;

		private DataColumn columnEndpointDefinitions;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDServerColumn => columnIDServer;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn TypeColumn => columnType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn AddressesColumn => columnAddresses;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn TrustedColumn => columnTrusted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn CustomDataColumn => columnCustomData;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EndpointDefinitionsColumn => columnEndpointDefinitions;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredServersRow this[int index] => (RegisteredServersRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RegisteredServersRowChangeEventHandler RegisteredServersRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RegisteredServersRowChangeEventHandler RegisteredServersRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RegisteredServersRowChangeEventHandler RegisteredServersRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RegisteredServersRowChangeEventHandler RegisteredServersRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredServersDataTable()
		{
			base.TableName = "RegisteredServers";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredServersDataTable(DataTable table)
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
		protected RegisteredServersDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRegisteredServersRow(RegisteredServersRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredServersRow AddRegisteredServersRow(DateTime LastModified, string IDServer, string Type, string Name, string Description, string Addresses, bool Trusted, string CustomData, bool Enabled, string EndpointDefinitions)
		{
			RegisteredServersRow registeredServersRow = (RegisteredServersRow)NewRow();
			object[] itemArray = new object[10] { LastModified, IDServer, Type, Name, Description, Addresses, Trusted, CustomData, Enabled, EndpointDefinitions };
			registeredServersRow.ItemArray = itemArray;
			base.Rows.Add(registeredServersRow);
			return registeredServersRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredServersRow FindByIDServer(string IDServer)
		{
			return (RegisteredServersRow)base.Rows.Find(new object[1] { IDServer });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RegisteredServersDataTable obj = (RegisteredServersDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RegisteredServersDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDServer = base.Columns["IDServer"];
			columnType = base.Columns["Type"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnAddresses = base.Columns["Addresses"];
			columnTrusted = base.Columns["Trusted"];
			columnCustomData = base.Columns["CustomData"];
			columnEnabled = base.Columns["Enabled"];
			columnEndpointDefinitions = base.Columns["EndpointDefinitions"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDServer = new DataColumn("IDServer", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDServer);
			columnType = new DataColumn("Type", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnType);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnAddresses = new DataColumn("Addresses", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnAddresses);
			columnTrusted = new DataColumn("Trusted", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnTrusted);
			columnCustomData = new DataColumn("CustomData", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnCustomData);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			columnEndpointDefinitions = new DataColumn("EndpointDefinitions", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnEndpointDefinitions);
			base.Constraints.Add(new UniqueConstraint("PK_RegisteredServers", new DataColumn[1] { columnIDServer }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDServer.AllowDBNull = false;
			columnIDServer.Unique = true;
			columnIDServer.MaxLength = 36;
			columnType.AllowDBNull = false;
			columnType.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
			columnAddresses.AllowDBNull = false;
			columnAddresses.MaxLength = 1073741823;
			columnTrusted.AllowDBNull = false;
			columnCustomData.AllowDBNull = false;
			columnCustomData.MaxLength = 1073741823;
			columnEndpointDefinitions.AllowDBNull = false;
			columnEndpointDefinitions.MaxLength = int.MaxValue;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredServersRow NewRegisteredServersRow()
		{
			return (RegisteredServersRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RegisteredServersRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RegisteredServersRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RegisteredServersRowChanged != null)
			{
				this.RegisteredServersRowChanged(this, new RegisteredServersRowChangeEvent((RegisteredServersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RegisteredServersRowChanging != null)
			{
				this.RegisteredServersRowChanging(this, new RegisteredServersRowChangeEvent((RegisteredServersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RegisteredServersRowDeleted != null)
			{
				this.RegisteredServersRowDeleted(this, new RegisteredServersRowChangeEvent((RegisteredServersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RegisteredServersRowDeleting != null)
			{
				this.RegisteredServersRowDeleting(this, new RegisteredServersRowChangeEvent((RegisteredServersRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRegisteredServersRow(RegisteredServersRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RegisteredServersDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class RulesDataTable : TypedTableBase<RulesRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDRule;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnDefinition;

		private DataColumn columnEnabled;

		private DataColumn columnPriority;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRuleColumn => columnIDRule;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DefinitionColumn => columnDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PriorityColumn => columnPriority;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RulesRow this[int index] => (RulesRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RulesRowChangeEventHandler RulesRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RulesRowChangeEventHandler RulesRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RulesRowChangeEventHandler RulesRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RulesRowChangeEventHandler RulesRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RulesDataTable()
		{
			base.TableName = "Rules";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RulesDataTable(DataTable table)
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
		protected RulesDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRulesRow(RulesRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RulesRow AddRulesRow(DateTime LastModified, string IDRule, string Name, string Description, string Definition, bool Enabled, int Priority)
		{
			RulesRow rulesRow = (RulesRow)NewRow();
			object[] itemArray = new object[7] { LastModified, IDRule, Name, Description, Definition, Enabled, Priority };
			rulesRow.ItemArray = itemArray;
			base.Rows.Add(rulesRow);
			return rulesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RulesRow FindByIDRule(string IDRule)
		{
			return (RulesRow)base.Rows.Find(new object[1] { IDRule });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RulesDataTable obj = (RulesDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RulesDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDRule = base.Columns["IDRule"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnDefinition = base.Columns["Definition"];
			columnEnabled = base.Columns["Enabled"];
			columnPriority = base.Columns["Priority"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDRule = new DataColumn("IDRule", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDRule);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnDefinition = new DataColumn("Definition", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDefinition);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			columnPriority = new DataColumn("Priority", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnPriority);
			base.Constraints.Add(new UniqueConstraint("PK_Rules", new DataColumn[1] { columnIDRule }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDRule.AllowDBNull = false;
			columnIDRule.Unique = true;
			columnIDRule.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = 1073741823;
			columnDefinition.AllowDBNull = false;
			columnDefinition.MaxLength = 1073741823;
			columnEnabled.AllowDBNull = false;
			columnPriority.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RulesRow NewRulesRow()
		{
			return (RulesRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RulesRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RulesRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RulesRowChanged != null)
			{
				this.RulesRowChanged(this, new RulesRowChangeEvent((RulesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RulesRowChanging != null)
			{
				this.RulesRowChanging(this, new RulesRowChangeEvent((RulesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RulesRowDeleted != null)
			{
				this.RulesRowDeleted(this, new RulesRowChangeEvent((RulesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RulesRowDeleting != null)
			{
				this.RulesRowDeleting(this, new RulesRowChangeEvent((RulesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRulesRow(RulesRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RulesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class RuleVariablesDataTable : TypedTableBase<RuleVariablesRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDVariable;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnValue;

		private DataColumn columnReset;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDVariableColumn => columnIDVariable;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ValueColumn => columnValue;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ResetColumn => columnReset;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RuleVariablesRow this[int index] => (RuleVariablesRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RuleVariablesRowChangeEventHandler RuleVariablesRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RuleVariablesRowChangeEventHandler RuleVariablesRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RuleVariablesRowChangeEventHandler RuleVariablesRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event RuleVariablesRowChangeEventHandler RuleVariablesRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RuleVariablesDataTable()
		{
			base.TableName = "RuleVariables";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RuleVariablesDataTable(DataTable table)
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
		protected RuleVariablesDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddRuleVariablesRow(RuleVariablesRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RuleVariablesRow AddRuleVariablesRow(DateTime LastModified, string IDVariable, string Name, string Description, string Value, bool Reset)
		{
			RuleVariablesRow ruleVariablesRow = (RuleVariablesRow)NewRow();
			object[] itemArray = new object[6] { LastModified, IDVariable, Name, Description, Value, Reset };
			ruleVariablesRow.ItemArray = itemArray;
			base.Rows.Add(ruleVariablesRow);
			return ruleVariablesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RuleVariablesRow FindByIDVariable(string IDVariable)
		{
			return (RuleVariablesRow)base.Rows.Find(new object[1] { IDVariable });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RuleVariablesDataTable obj = (RuleVariablesDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new RuleVariablesDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDVariable = base.Columns["IDVariable"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnValue = base.Columns["Value"];
			columnReset = base.Columns["Reset"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDVariable = new DataColumn("IDVariable", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDVariable);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnValue = new DataColumn("Value", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnValue);
			columnReset = new DataColumn("Reset", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnReset);
			base.Constraints.Add(new UniqueConstraint("PK_RuleVariables", new DataColumn[1] { columnIDVariable }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDVariable.AllowDBNull = false;
			columnIDVariable.Unique = true;
			columnIDVariable.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.AllowDBNull = false;
			columnDescription.MaxLength = int.MaxValue;
			columnValue.AllowDBNull = false;
			columnValue.MaxLength = int.MaxValue;
			columnReset.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RuleVariablesRow NewRuleVariablesRow()
		{
			return (RuleVariablesRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new RuleVariablesRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(RuleVariablesRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.RuleVariablesRowChanged != null)
			{
				this.RuleVariablesRowChanged(this, new RuleVariablesRowChangeEvent((RuleVariablesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.RuleVariablesRowChanging != null)
			{
				this.RuleVariablesRowChanging(this, new RuleVariablesRowChangeEvent((RuleVariablesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.RuleVariablesRowDeleted != null)
			{
				this.RuleVariablesRowDeleted(this, new RuleVariablesRowChangeEvent((RuleVariablesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.RuleVariablesRowDeleting != null)
			{
				this.RuleVariablesRowDeleting(this, new RuleVariablesRowChangeEvent((RuleVariablesRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveRuleVariablesRow(RuleVariablesRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RuleVariablesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class ServerGroupMembershipDataTable : TypedTableBase<ServerGroupMembershipRow>
	{
		private DataColumn columnIDServerGroup;

		private DataColumn columnIDObject;

		private DataColumn columnObjectType;

		private DataColumn columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDServerGroupColumn => columnIDServerGroup;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDObjectColumn => columnIDObject;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn ObjectTypeColumn => columnObjectType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupMembershipRow this[int index] => (ServerGroupMembershipRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ServerGroupMembershipRowChangeEventHandler ServerGroupMembershipRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ServerGroupMembershipRowChangeEventHandler ServerGroupMembershipRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ServerGroupMembershipRowChangeEventHandler ServerGroupMembershipRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ServerGroupMembershipRowChangeEventHandler ServerGroupMembershipRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupMembershipDataTable()
		{
			base.TableName = "ServerGroupMembership";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupMembershipDataTable(DataTable table)
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
		protected ServerGroupMembershipDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddServerGroupMembershipRow(ServerGroupMembershipRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupMembershipRow AddServerGroupMembershipRow(string IDServerGroup, string IDObject, string ObjectType, DateTime LastModified)
		{
			ServerGroupMembershipRow serverGroupMembershipRow = (ServerGroupMembershipRow)NewRow();
			object[] itemArray = new object[4] { IDServerGroup, IDObject, ObjectType, LastModified };
			serverGroupMembershipRow.ItemArray = itemArray;
			base.Rows.Add(serverGroupMembershipRow);
			return serverGroupMembershipRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupMembershipRow FindByIDServerGroupIDObject(string IDServerGroup, string IDObject)
		{
			return (ServerGroupMembershipRow)base.Rows.Find(new object[2] { IDServerGroup, IDObject });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			ServerGroupMembershipDataTable obj = (ServerGroupMembershipDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new ServerGroupMembershipDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDServerGroup = base.Columns["IDServerGroup"];
			columnIDObject = base.Columns["IDObject"];
			columnObjectType = base.Columns["ObjectType"];
			columnLastModified = base.Columns["LastModified"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDServerGroup = new DataColumn("IDServerGroup", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDServerGroup);
			columnIDObject = new DataColumn("IDObject", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDObject);
			columnObjectType = new DataColumn("ObjectType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnObjectType);
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			base.Constraints.Add(new UniqueConstraint("PK_ServerGroupMembership", new DataColumn[2] { columnIDServerGroup, columnIDObject }, isPrimaryKey: true));
			columnIDServerGroup.AllowDBNull = false;
			columnIDServerGroup.MaxLength = 36;
			columnIDObject.AllowDBNull = false;
			columnIDObject.MaxLength = 36;
			columnObjectType.AllowDBNull = false;
			columnObjectType.MaxLength = 256;
			columnLastModified.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupMembershipRow NewServerGroupMembershipRow()
		{
			return (ServerGroupMembershipRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new ServerGroupMembershipRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(ServerGroupMembershipRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.ServerGroupMembershipRowChanged != null)
			{
				this.ServerGroupMembershipRowChanged(this, new ServerGroupMembershipRowChangeEvent((ServerGroupMembershipRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.ServerGroupMembershipRowChanging != null)
			{
				this.ServerGroupMembershipRowChanging(this, new ServerGroupMembershipRowChangeEvent((ServerGroupMembershipRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.ServerGroupMembershipRowDeleted != null)
			{
				this.ServerGroupMembershipRowDeleted(this, new ServerGroupMembershipRowChangeEvent((ServerGroupMembershipRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.ServerGroupMembershipRowDeleting != null)
			{
				this.ServerGroupMembershipRowDeleting(this, new ServerGroupMembershipRowChangeEvent((ServerGroupMembershipRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveServerGroupMembershipRow(ServerGroupMembershipRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "ServerGroupMembershipDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class ServerGroupDataTable : TypedTableBase<ServerGroupRow>
	{
		private DataColumn columnIDServerGroup;

		private DataColumn columnName;

		private DataColumn columnDescription;

		private DataColumn columnIDParentGroup;

		private DataColumn columnStructureType;

		private DataColumn columnOrderNo;

		private DataColumn columnLevel;

		private DataColumn columnPath;

		private DataColumn columnCreatedDate;

		private DataColumn columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDServerGroupColumn => columnIDServerGroup;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DescriptionColumn => columnDescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDParentGroupColumn => columnIDParentGroup;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn StructureTypeColumn => columnStructureType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn OrderNoColumn => columnOrderNo;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LevelColumn => columnLevel;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn PathColumn => columnPath;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn CreatedDateColumn => columnCreatedDate;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupRow this[int index] => (ServerGroupRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ServerGroupRowChangeEventHandler ServerGroupRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ServerGroupRowChangeEventHandler ServerGroupRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ServerGroupRowChangeEventHandler ServerGroupRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event ServerGroupRowChangeEventHandler ServerGroupRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupDataTable()
		{
			base.TableName = "ServerGroup";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupDataTable(DataTable table)
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
		protected ServerGroupDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddServerGroupRow(ServerGroupRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupRow AddServerGroupRow(string IDServerGroup, string Name, string Description, string IDParentGroup, string StructureType, int OrderNo, int Level, string Path, DateTime CreatedDate, DateTime LastModified)
		{
			ServerGroupRow serverGroupRow = (ServerGroupRow)NewRow();
			object[] itemArray = new object[10] { IDServerGroup, Name, Description, IDParentGroup, StructureType, OrderNo, Level, Path, CreatedDate, LastModified };
			serverGroupRow.ItemArray = itemArray;
			base.Rows.Add(serverGroupRow);
			return serverGroupRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupRow FindByIDServerGroup(string IDServerGroup)
		{
			return (ServerGroupRow)base.Rows.Find(new object[1] { IDServerGroup });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			ServerGroupDataTable obj = (ServerGroupDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new ServerGroupDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnIDServerGroup = base.Columns["IDServerGroup"];
			columnName = base.Columns["Name"];
			columnDescription = base.Columns["Description"];
			columnIDParentGroup = base.Columns["IDParentGroup"];
			columnStructureType = base.Columns["StructureType"];
			columnOrderNo = base.Columns["OrderNo"];
			columnLevel = base.Columns["Level"];
			columnPath = base.Columns["Path"];
			columnCreatedDate = base.Columns["CreatedDate"];
			columnLastModified = base.Columns["LastModified"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDServerGroup = new DataColumn("IDServerGroup", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDServerGroup);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnDescription = new DataColumn("Description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDescription);
			columnIDParentGroup = new DataColumn("IDParentGroup", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnIDParentGroup);
			columnStructureType = new DataColumn("StructureType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnStructureType);
			columnOrderNo = new DataColumn("OrderNo", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnOrderNo);
			columnLevel = new DataColumn("Level", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnLevel);
			columnPath = new DataColumn("Path", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnPath);
			columnCreatedDate = new DataColumn("CreatedDate", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnCreatedDate);
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			base.Constraints.Add(new UniqueConstraint("PK_ServerGroup", new DataColumn[1] { columnIDServerGroup }, isPrimaryKey: true));
			columnIDServerGroup.AllowDBNull = false;
			columnIDServerGroup.Unique = true;
			columnIDServerGroup.MaxLength = 36;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnDescription.MaxLength = 1024;
			columnIDParentGroup.AllowDBNull = false;
			columnIDParentGroup.MaxLength = 36;
			columnStructureType.AllowDBNull = false;
			columnStructureType.MaxLength = 64;
			columnOrderNo.AllowDBNull = false;
			columnLevel.AllowDBNull = false;
			columnPath.AllowDBNull = false;
			columnPath.MaxLength = 4000;
			columnCreatedDate.AllowDBNull = false;
			columnLastModified.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupRow NewServerGroupRow()
		{
			return (ServerGroupRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new ServerGroupRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(ServerGroupRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.ServerGroupRowChanged != null)
			{
				this.ServerGroupRowChanged(this, new ServerGroupRowChangeEvent((ServerGroupRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.ServerGroupRowChanging != null)
			{
				this.ServerGroupRowChanging(this, new ServerGroupRowChangeEvent((ServerGroupRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.ServerGroupRowDeleted != null)
			{
				this.ServerGroupRowDeleted(this, new ServerGroupRowChangeEvent((ServerGroupRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.ServerGroupRowDeleting != null)
			{
				this.ServerGroupRowDeleting(this, new ServerGroupRowChangeEvent((ServerGroupRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveServerGroupRow(ServerGroupRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "ServerGroupDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
	public class TimeProfileDataTable : TypedTableBase<TimeProfileRow>
	{
		private DataColumn columnname;

		private DataColumn columndescription;

		private DataColumn columntype;

		private DataColumn columnpkid;

		private DataColumn columnxml_data;

		private DataColumn columncreated_date;

		private DataColumn columnlast_modified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn nameColumn => columnname;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn descriptionColumn => columndescription;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn typeColumn => columntype;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn pkidColumn => columnpkid;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn xml_dataColumn => columnxml_data;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn created_dateColumn => columncreated_date;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn last_modifiedColumn => columnlast_modified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileRow this[int index] => (TimeProfileRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event TimeProfileRowChangeEventHandler TimeProfileRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event TimeProfileRowChangeEventHandler TimeProfileRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event TimeProfileRowChangeEventHandler TimeProfileRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event TimeProfileRowChangeEventHandler TimeProfileRowDeleted;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDataTable()
		{
			base.TableName = "TimeProfile";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileDataTable(DataTable table)
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
		protected TimeProfileDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddTimeProfileRow(TimeProfileRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileRow AddTimeProfileRow(string name, string description, string type, string pkid, string xml_data, DateTime created_date, DateTime last_modified)
		{
			TimeProfileRow timeProfileRow = (TimeProfileRow)NewRow();
			object[] itemArray = new object[7] { name, description, type, pkid, xml_data, created_date, last_modified };
			timeProfileRow.ItemArray = itemArray;
			base.Rows.Add(timeProfileRow);
			return timeProfileRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileRow FindBypkid(string pkid)
		{
			return (TimeProfileRow)base.Rows.Find(new object[1] { pkid });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			TimeProfileDataTable obj = (TimeProfileDataTable)base.Clone();
			obj.InitVars();
			return obj;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new TimeProfileDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnname = base.Columns["name"];
			columndescription = base.Columns["description"];
			columntype = base.Columns["type"];
			columnpkid = base.Columns["pkid"];
			columnxml_data = base.Columns["xml_data"];
			columncreated_date = base.Columns["created_date"];
			columnlast_modified = base.Columns["last_modified"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnname = new DataColumn("name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnname);
			columndescription = new DataColumn("description", typeof(string), null, MappingType.Element);
			base.Columns.Add(columndescription);
			columntype = new DataColumn("type", typeof(string), null, MappingType.Element);
			base.Columns.Add(columntype);
			columnpkid = new DataColumn("pkid", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnpkid);
			columnxml_data = new DataColumn("xml_data", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnxml_data);
			columncreated_date = new DataColumn("created_date", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columncreated_date);
			columnlast_modified = new DataColumn("last_modified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnlast_modified);
			base.Constraints.Add(new UniqueConstraint("PK_TimeProfile", new DataColumn[1] { columnpkid }, isPrimaryKey: true));
			columnname.AllowDBNull = false;
			columnname.MaxLength = 256;
			columndescription.MaxLength = int.MaxValue;
			columntype.AllowDBNull = false;
			columntype.MaxLength = 15;
			columnpkid.AllowDBNull = false;
			columnpkid.Unique = true;
			columnpkid.MaxLength = 36;
			columnxml_data.AllowDBNull = false;
			columnxml_data.MaxLength = int.MaxValue;
			columncreated_date.AllowDBNull = false;
			columnlast_modified.AllowDBNull = false;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileRow NewTimeProfileRow()
		{
			return (TimeProfileRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new TimeProfileRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(TimeProfileRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.TimeProfileRowChanged != null)
			{
				this.TimeProfileRowChanged(this, new TimeProfileRowChangeEvent((TimeProfileRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.TimeProfileRowChanging != null)
			{
				this.TimeProfileRowChanging(this, new TimeProfileRowChangeEvent((TimeProfileRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.TimeProfileRowDeleted != null)
			{
				this.TimeProfileRowDeleted(this, new TimeProfileRowChangeEvent((TimeProfileRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.TimeProfileRowDeleting != null)
			{
				this.TimeProfileRowDeleting(this, new TimeProfileRowChangeEvent((TimeProfileRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveTimeProfileRow(TimeProfileRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
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
			xmlSchemaAttribute.FixedValue = configurationDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "TimeProfileDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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

	public class CompressorsRow : DataRow
	{
		private CompressorsDataTable tableCompressors;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableCompressors.LastModifiedColumn];
			}
			set
			{
				base[tableCompressors.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDCodec
		{
			get
			{
				return (string)base[tableCompressors.IDCodecColumn];
			}
			set
			{
				base[tableCompressors.IDCodecColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int Handler
		{
			get
			{
				return (int)base[tableCompressors.HandlerColumn];
			}
			set
			{
				base[tableCompressors.HandlerColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int Flags
		{
			get
			{
				try
				{
					return (int)base[tableCompressors.FlagsColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Flags' in table 'Compressors' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableCompressors.FlagsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int Quality
		{
			get
			{
				try
				{
					return (int)base[tableCompressors.QualityColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Quality' in table 'Compressors' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableCompressors.QualityColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int KeyFrames
		{
			get
			{
				try
				{
					return (int)base[tableCompressors.KeyFramesColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'KeyFrames' in table 'Compressors' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableCompressors.KeyFramesColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int DataRate
		{
			get
			{
				try
				{
					return (int)base[tableCompressors.DataRateColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'DataRate' in table 'Compressors' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableCompressors.DataRateColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public byte[] ConfigData
		{
			get
			{
				try
				{
					return (byte[])base[tableCompressors.ConfigDataColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'ConfigData' in table 'Compressors' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableCompressors.ConfigDataColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public CompressorsRow(DataRowBuilder rb)
			: base(rb)
		{
			tableCompressors = (CompressorsDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsFlagsNull()
		{
			return IsNull(tableCompressors.FlagsColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetFlagsNull()
		{
			base[tableCompressors.FlagsColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsQualityNull()
		{
			return IsNull(tableCompressors.QualityColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetQualityNull()
		{
			base[tableCompressors.QualityColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsKeyFramesNull()
		{
			return IsNull(tableCompressors.KeyFramesColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetKeyFramesNull()
		{
			base[tableCompressors.KeyFramesColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsDataRateNull()
		{
			return IsNull(tableCompressors.DataRateColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetDataRateNull()
		{
			base[tableCompressors.DataRateColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsConfigDataNull()
		{
			return IsNull(tableCompressors.ConfigDataColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetConfigDataNull()
		{
			base[tableCompressors.ConfigDataColumn] = Convert.DBNull;
		}
	}

	public class DeviceTrackDefinitionRow : DataRow
	{
		private DeviceTrackDefinitionDataTable tableDeviceTrackDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDTrackDefinition
		{
			get
			{
				return (Guid)base[tableDeviceTrackDefinition.IDTrackDefinitionColumn];
			}
			set
			{
				base[tableDeviceTrackDefinition.IDTrackDefinitionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsPrimary
		{
			get
			{
				return (bool)base[tableDeviceTrackDefinition.IsPrimaryColumn];
			}
			set
			{
				base[tableDeviceTrackDefinition.IsPrimaryColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsEdge
		{
			get
			{
				return (bool)base[tableDeviceTrackDefinition.IsEdgeColumn];
			}
			set
			{
				base[tableDeviceTrackDefinition.IsEdgeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDefinitionRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDeviceTrackDefinition = (DeviceTrackDefinitionDataTable)base.Table;
		}
	}

	public class DriverNumbersRow : DataRow
	{
		private DriverNumbersDataTable tableDriverNumbers;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDDriver
		{
			get
			{
				return (Guid)base[tableDriverNumbers.IDDriverColumn];
			}
			set
			{
				base[tableDriverNumbers.IDDriverColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int DriverNumber
		{
			get
			{
				return (int)base[tableDriverNumbers.DriverNumberColumn];
			}
			set
			{
				base[tableDriverNumbers.DriverNumberColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DriverNumbersRow(DataRowBuilder rb)
			: base(rb)
		{
			tableDriverNumbers = (DriverNumbersDataTable)base.Table;
		}
	}

	public class ExternalPluginsRow : DataRow
	{
		private ExternalPluginsDataTable tableExternalPlugins;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableExternalPlugins.LastModifiedColumn];
			}
			set
			{
				base[tableExternalPlugins.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string PluginID
		{
			get
			{
				return (string)base[tableExternalPlugins.PluginIDColumn];
			}
			set
			{
				base[tableExternalPlugins.PluginIDColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string PluginType
		{
			get
			{
				return (string)base[tableExternalPlugins.PluginTypeColumn];
			}
			set
			{
				base[tableExternalPlugins.PluginTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Data
		{
			get
			{
				return (string)base[tableExternalPlugins.DataColumn];
			}
			set
			{
				base[tableExternalPlugins.DataColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ExternalPluginsRow(DataRowBuilder rb)
			: base(rb)
		{
			tableExternalPlugins = (ExternalPluginsDataTable)base.Table;
		}
	}

	public class HardwareDefinitionRow : DataRow
	{
		private HardwareDefinitionDataTable tableHardwareDefinition;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string C18NDigest
		{
			get
			{
				return (string)base[tableHardwareDefinition.C18NDigestColumn];
			}
			set
			{
				base[tableHardwareDefinition.C18NDigestColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Definition
		{
			get
			{
				return (string)base[tableHardwareDefinition.DefinitionColumn];
			}
			set
			{
				base[tableHardwareDefinition.DefinitionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDefinitionRow(DataRowBuilder rb)
			: base(rb)
		{
			tableHardwareDefinition = (HardwareDefinitionDataTable)base.Table;
		}
	}

	public class HardwareDriverRecorderRow : DataRow
	{
		private HardwareDriverRecorderDataTable tableHardwareDriverRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableHardwareDriverRecorder.LastModifiedColumn];
			}
			set
			{
				base[tableHardwareDriverRecorder.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDHardwareDriver
		{
			get
			{
				return (string)base[tableHardwareDriverRecorder.IDHardwareDriverColumn];
			}
			set
			{
				base[tableHardwareDriverRecorder.IDHardwareDriverColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string DriverName
		{
			get
			{
				return (string)base[tableHardwareDriverRecorder.DriverNameColumn];
			}
			set
			{
				base[tableHardwareDriverRecorder.DriverNameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int DriverNumber
		{
			get
			{
				return (int)base[tableHardwareDriverRecorder.DriverNumberColumn];
			}
			set
			{
				base[tableHardwareDriverRecorder.DriverNumberColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string DriverVersion
		{
			get
			{
				return (string)base[tableHardwareDriverRecorder.DriverVersionColumn];
			}
			set
			{
				base[tableHardwareDriverRecorder.DriverVersionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string XmlVersion
		{
			get
			{
				return (string)base[tableHardwareDriverRecorder.XmlVersionColumn];
			}
			set
			{
				base[tableHardwareDriverRecorder.XmlVersionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string DllName
		{
			get
			{
				return (string)base[tableHardwareDriverRecorder.DllNameColumn];
			}
			set
			{
				base[tableHardwareDriverRecorder.DllNameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string GroupName
		{
			get
			{
				return (string)base[tableHardwareDriverRecorder.GroupNameColumn];
			}
			set
			{
				base[tableHardwareDriverRecorder.GroupNameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRecordingServerService
		{
			get
			{
				return (string)base[tableHardwareDriverRecorder.IDRecordingServerServiceColumn];
			}
			set
			{
				base[tableHardwareDriverRecorder.IDRecordingServerServiceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDriverRecorderRow(DataRowBuilder rb)
			: base(rb)
		{
			tableHardwareDriverRecorder = (HardwareDriverRecorderDataTable)base.Table;
		}
	}

	public class LicenseRow : DataRow
	{
		private LicenseDataTable tableLicense;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableLicense.LastModifiedColumn];
			}
			set
			{
				base[tableLicense.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDLicense
		{
			get
			{
				return (Guid)base[tableLicense.IDLicenseColumn];
			}
			set
			{
				base[tableLicense.IDLicenseColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public byte[] Data
		{
			get
			{
				return (byte[])base[tableLicense.DataColumn];
			}
			set
			{
				base[tableLicense.DataColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRow(DataRowBuilder rb)
			: base(rb)
		{
			tableLicense = (LicenseDataTable)base.Table;
		}
	}

	public class MIPObjectRow : DataRow
	{
		private MIPObjectDataTable tableMIPObject;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDMIPObject
		{
			get
			{
				return (Guid)base[tableMIPObject.IDMIPObjectColumn];
			}
			set
			{
				base[tableMIPObject.IDMIPObjectColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableMIPObject.LastModifiedColumn];
			}
			set
			{
				base[tableMIPObject.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime CreatedDate
		{
			get
			{
				return (DateTime)base[tableMIPObject.CreatedDateColumn];
			}
			set
			{
				base[tableMIPObject.CreatedDateColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableMIPObject.NameColumn];
			}
			set
			{
				base[tableMIPObject.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid Kind
		{
			get
			{
				return (Guid)base[tableMIPObject.KindColumn];
			}
			set
			{
				base[tableMIPObject.KindColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid SubKind
		{
			get
			{
				return (Guid)base[tableMIPObject.SubKindColumn];
			}
			set
			{
				base[tableMIPObject.SubKindColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid PluginId
		{
			get
			{
				return (Guid)base[tableMIPObject.PluginIdColumn];
			}
			set
			{
				base[tableMIPObject.PluginIdColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string CustomData
		{
			get
			{
				try
				{
					return (string)base[tableMIPObject.CustomDataColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'CustomData' in table 'MIPObject' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableMIPObject.CustomDataColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MIPObjectRow(DataRowBuilder rb)
			: base(rb)
		{
			tableMIPObject = (MIPObjectDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsCustomDataNull()
		{
			return IsNull(tableMIPObject.CustomDataColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetCustomDataNull()
		{
			base[tableMIPObject.CustomDataColumn] = Convert.DBNull;
		}
	}

	public class NotificationProfilesRow : DataRow
	{
		private NotificationProfilesDataTable tableNotificationProfiles;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableNotificationProfiles.LastModifiedColumn];
			}
			set
			{
				base[tableNotificationProfiles.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDNotificationProfile
		{
			get
			{
				return (string)base[tableNotificationProfiles.IDNotificationProfileColumn];
			}
			set
			{
				base[tableNotificationProfiles.IDNotificationProfileColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableNotificationProfiles.NameColumn];
			}
			set
			{
				base[tableNotificationProfiles.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableNotificationProfiles.DescriptionColumn];
			}
			set
			{
				base[tableNotificationProfiles.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationProfilesRow(DataRowBuilder rb)
			: base(rb)
		{
			tableNotificationProfiles = (NotificationProfilesDataTable)base.Table;
		}
	}

	public class NotificationSettingsRow : DataRow
	{
		private NotificationSettingsDataTable tableNotificationSettings;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableNotificationSettings.LastModifiedColumn];
			}
			set
			{
				base[tableNotificationSettings.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDNotificationSetting
		{
			get
			{
				return (string)base[tableNotificationSettings.IDNotificationSettingColumn];
			}
			set
			{
				base[tableNotificationSettings.IDNotificationSettingColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Settings
		{
			get
			{
				try
				{
					return (string)base[tableNotificationSettings.SettingsColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Settings' in table 'NotificationSettings' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableNotificationSettings.SettingsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDNotificationType
		{
			get
			{
				return (string)base[tableNotificationSettings.IDNotificationTypeColumn];
			}
			set
			{
				base[tableNotificationSettings.IDNotificationTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDNotificationProfile
		{
			get
			{
				return (string)base[tableNotificationSettings.IDNotificationProfileColumn];
			}
			set
			{
				base[tableNotificationSettings.IDNotificationProfileColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationSettingsRow(DataRowBuilder rb)
			: base(rb)
		{
			tableNotificationSettings = (NotificationSettingsDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsSettingsNull()
		{
			return IsNull(tableNotificationSettings.SettingsColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetSettingsNull()
		{
			base[tableNotificationSettings.SettingsColumn] = Convert.DBNull;
		}
	}

	public class NotificationTypesRow : DataRow
	{
		private NotificationTypesDataTable tableNotificationTypes;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableNotificationTypes.LastModifiedColumn];
			}
			set
			{
				base[tableNotificationTypes.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDNotificationType
		{
			get
			{
				return (string)base[tableNotificationTypes.IDNotificationTypeColumn];
			}
			set
			{
				base[tableNotificationTypes.IDNotificationTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableNotificationTypes.NameColumn];
			}
			set
			{
				base[tableNotificationTypes.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string PageName
		{
			get
			{
				return (string)base[tableNotificationTypes.PageNameColumn];
			}
			set
			{
				base[tableNotificationTypes.PageNameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationTypesRow(DataRowBuilder rb)
			: base(rb)
		{
			tableNotificationTypes = (NotificationTypesDataTable)base.Table;
		}
	}

	public class RecorderPluginsRow : DataRow
	{
		private RecorderPluginsDataTable tableRecorderPlugins;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableRecorderPlugins.LastModifiedColumn];
			}
			set
			{
				base[tableRecorderPlugins.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDRecorderPlugin
		{
			get
			{
				return (string)base[tableRecorderPlugins.IDRecorderPluginColumn];
			}
			set
			{
				base[tableRecorderPlugins.IDRecorderPluginColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableRecorderPlugins.NameColumn];
			}
			set
			{
				base[tableRecorderPlugins.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableRecorderPlugins.DescriptionColumn];
			}
			set
			{
				base[tableRecorderPlugins.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginsRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRecorderPlugins = (RecorderPluginsDataTable)base.Table;
		}
	}

	public class RecordingServerServiceRecorderRow : DataRow
	{
		private RecordingServerServiceRecorderDataTable tableRecordingServerServiceRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime last_modified
		{
			get
			{
				return (DateTime)base[tableRecordingServerServiceRecorder.last_modifiedColumn];
			}
			set
			{
				base[tableRecordingServerServiceRecorder.last_modifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string recorder_id
		{
			get
			{
				return (string)base[tableRecordingServerServiceRecorder.recorder_idColumn];
			}
			set
			{
				base[tableRecordingServerServiceRecorder.recorder_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int type
		{
			get
			{
				return (int)base[tableRecordingServerServiceRecorder.typeColumn];
			}
			set
			{
				base[tableRecordingServerServiceRecorder.typeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string recording_server_service_id
		{
			get
			{
				return (string)base[tableRecordingServerServiceRecorder.recording_server_service_idColumn];
			}
			set
			{
				base[tableRecordingServerServiceRecorder.recording_server_service_idColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool active
		{
			get
			{
				return (bool)base[tableRecordingServerServiceRecorder.activeColumn];
			}
			set
			{
				base[tableRecordingServerServiceRecorder.activeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRecorderRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRecordingServerServiceRecorder = (RecordingServerServiceRecorderDataTable)base.Table;
		}
	}

	public class RegisteredDeviceTypesRow : DataRow
	{
		private RegisteredDeviceTypesDataTable tableRegisteredDeviceTypes;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableRegisteredDeviceTypes.LastModifiedColumn];
			}
			set
			{
				base[tableRegisteredDeviceTypes.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string DeviceType
		{
			get
			{
				return (string)base[tableRegisteredDeviceTypes.DeviceTypeColumn];
			}
			set
			{
				base[tableRegisteredDeviceTypes.DeviceTypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableRegisteredDeviceTypes.DescriptionColumn];
			}
			set
			{
				base[tableRegisteredDeviceTypes.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int SortOrder
		{
			get
			{
				return (int)base[tableRegisteredDeviceTypes.SortOrderColumn];
			}
			set
			{
				base[tableRegisteredDeviceTypes.SortOrderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int PrimaryIconNumber
		{
			get
			{
				return (int)base[tableRegisteredDeviceTypes.PrimaryIconNumberColumn];
			}
			set
			{
				base[tableRegisteredDeviceTypes.PrimaryIconNumberColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsRecordingDevice
		{
			get
			{
				return (bool)base[tableRegisteredDeviceTypes.IsRecordingDeviceColumn];
			}
			set
			{
				base[tableRegisteredDeviceTypes.IsRecordingDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredDeviceTypesRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRegisteredDeviceTypes = (RegisteredDeviceTypesDataTable)base.Table;
		}
	}

	public class RegisteredServersRow : DataRow
	{
		private RegisteredServersDataTable tableRegisteredServers;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableRegisteredServers.LastModifiedColumn];
			}
			set
			{
				base[tableRegisteredServers.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDServer
		{
			get
			{
				return (string)base[tableRegisteredServers.IDServerColumn];
			}
			set
			{
				base[tableRegisteredServers.IDServerColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Type
		{
			get
			{
				return (string)base[tableRegisteredServers.TypeColumn];
			}
			set
			{
				base[tableRegisteredServers.TypeColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableRegisteredServers.NameColumn];
			}
			set
			{
				base[tableRegisteredServers.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableRegisteredServers.DescriptionColumn];
			}
			set
			{
				base[tableRegisteredServers.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Addresses
		{
			get
			{
				return (string)base[tableRegisteredServers.AddressesColumn];
			}
			set
			{
				base[tableRegisteredServers.AddressesColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Trusted
		{
			get
			{
				return (bool)base[tableRegisteredServers.TrustedColumn];
			}
			set
			{
				base[tableRegisteredServers.TrustedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string CustomData
		{
			get
			{
				return (string)base[tableRegisteredServers.CustomDataColumn];
			}
			set
			{
				base[tableRegisteredServers.CustomDataColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Enabled
		{
			get
			{
				try
				{
					return (bool)base[tableRegisteredServers.EnabledColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'Enabled' in table 'RegisteredServers' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableRegisteredServers.EnabledColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string EndpointDefinitions
		{
			get
			{
				return (string)base[tableRegisteredServers.EndpointDefinitionsColumn];
			}
			set
			{
				base[tableRegisteredServers.EndpointDefinitionsColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredServersRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRegisteredServers = (RegisteredServersDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsEnabledNull()
		{
			return IsNull(tableRegisteredServers.EnabledColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetEnabledNull()
		{
			base[tableRegisteredServers.EnabledColumn] = Convert.DBNull;
		}
	}

	public class RuleVariablesRow : DataRow
	{
		private RuleVariablesDataTable tableRuleVariables;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableRuleVariables.LastModifiedColumn];
			}
			set
			{
				base[tableRuleVariables.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string IDVariable
		{
			get
			{
				return (string)base[tableRuleVariables.IDVariableColumn];
			}
			set
			{
				base[tableRuleVariables.IDVariableColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Name
		{
			get
			{
				return (string)base[tableRuleVariables.NameColumn];
			}
			set
			{
				base[tableRuleVariables.NameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Description
		{
			get
			{
				return (string)base[tableRuleVariables.DescriptionColumn];
			}
			set
			{
				base[tableRuleVariables.DescriptionColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string Value
		{
			get
			{
				return (string)base[tableRuleVariables.ValueColumn];
			}
			set
			{
				base[tableRuleVariables.ValueColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool Reset
		{
			get
			{
				return (bool)base[tableRuleVariables.ResetColumn];
			}
			set
			{
				base[tableRuleVariables.ResetColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RuleVariablesRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRuleVariables = (RuleVariablesDataTable)base.Table;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class ArchiveStorageRowChangeEvent : EventArgs
	{
		private ArchiveStorageRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ArchiveStorageRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ArchiveStorageRowChangeEvent(ArchiveStorageRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class AudioMessageRowChangeEvent : EventArgs
	{
		private AudioMessageRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public AudioMessageRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public AudioMessageRowChangeEvent(AudioMessageRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class CompressorsRowChangeEvent : EventArgs
	{
		private CompressorsRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public CompressorsRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public CompressorsRowChangeEvent(CompressorsRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DeviceDataDistributionRowChangeEvent : EventArgs
	{
		private DeviceDataDistributionRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceDataDistributionRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceDataDistributionRowChangeEvent(DeviceDataDistributionRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DeviceGroupMembersRowChangeEvent : EventArgs
	{
		private DeviceGroupMembersRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupMembersRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupMembersRowChangeEvent(DeviceGroupMembersRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DeviceGroupsRowChangeEvent : EventArgs
	{
		private DeviceGroupsRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupsRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceGroupsRowChangeEvent(DeviceGroupsRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DeviceLinksRowChangeEvent : EventArgs
	{
		private DeviceLinksRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceLinksRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceLinksRowChangeEvent(DeviceLinksRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DevicePrivacyMaskRowChangeEvent : EventArgs
	{
		private DevicePrivacyMaskRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicePrivacyMaskRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicePrivacyMaskRowChangeEvent(DevicePrivacyMaskRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DeviceRestrictedMediaRowChangeEvent : EventArgs
	{
		private DeviceRestrictedMediaRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceRestrictedMediaRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceRestrictedMediaRowChangeEvent(DeviceRestrictedMediaRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DevicesRowChangeEvent : EventArgs
	{
		private DevicesRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicesRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicesRowChangeEvent(DevicesRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DeviceTrackDefinitionRowChangeEvent : EventArgs
	{
		private DeviceTrackDefinitionRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDefinitionRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackDefinitionRowChangeEvent(DeviceTrackDefinitionRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DeviceTrackEvidenceLockRowChangeEvent : EventArgs
	{
		private DeviceTrackEvidenceLockRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackEvidenceLockRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackEvidenceLockRowChangeEvent(DeviceTrackEvidenceLockRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DeviceTrackInfoRowChangeEvent : EventArgs
	{
		private DeviceTrackInfoRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackInfoRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackInfoRowChangeEvent(DeviceTrackInfoRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DeviceTrackRowChangeEvent : EventArgs
	{
		private DeviceTrackRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DeviceTrackRowChangeEvent(DeviceTrackRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class DriverNumbersRowChangeEvent : EventArgs
	{
		private DriverNumbersRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DriverNumbersRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DriverNumbersRowChangeEvent(DriverNumbersRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class EventTypesRowChangeEvent : EventArgs
	{
		private EventTypesRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public EventTypesRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public EventTypesRowChangeEvent(EventTypesRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class ExternalPluginsRowChangeEvent : EventArgs
	{
		private ExternalPluginsRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ExternalPluginsRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ExternalPluginsRowChangeEvent(ExternalPluginsRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class FailoverInfoRowChangeEvent : EventArgs
	{
		private FailoverInfoRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public FailoverInfoRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public FailoverInfoRowChangeEvent(FailoverInfoRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class HardwareBlockRowChangeEvent : EventArgs
	{
		private HardwareBlockRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareBlockRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareBlockRowChangeEvent(HardwareBlockRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class HardwareDefinitionRowChangeEvent : EventArgs
	{
		private HardwareDefinitionRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDefinitionRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDefinitionRowChangeEvent(HardwareDefinitionRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class HardwareDriverRecorderRowChangeEvent : EventArgs
	{
		private HardwareDriverRecorderRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDriverRecorderRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareDriverRecorderRowChangeEvent(HardwareDriverRecorderRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class HardwareRowChangeEvent : EventArgs
	{
		private HardwareRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareRowChangeEvent(HardwareRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class HostedHardwareRowChangeEvent : EventArgs
	{
		private HostedHardwareRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostedHardwareRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostedHardwareRowChangeEvent(HostedHardwareRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class HostingServerRowChangeEvent : EventArgs
	{
		private HostingServerRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostingServerRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HostingServerRowChangeEvent(HostingServerRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class LicenseRowChangeEvent : EventArgs
	{
		private LicenseRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRowChangeEvent(LicenseRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class MatrixRowChangeEvent : EventArgs
	{
		private MatrixRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MatrixRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MatrixRowChangeEvent(MatrixRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class MIPObjectRowChangeEvent : EventArgs
	{
		private MIPObjectRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MIPObjectRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public MIPObjectRowChangeEvent(MIPObjectRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class NotificationProfilesRowChangeEvent : EventArgs
	{
		private NotificationProfilesRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationProfilesRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationProfilesRowChangeEvent(NotificationProfilesRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class NotificationSettingsRowChangeEvent : EventArgs
	{
		private NotificationSettingsRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationSettingsRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationSettingsRowChangeEvent(NotificationSettingsRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class NotificationTypesRowChangeEvent : EventArgs
	{
		private NotificationTypesRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationTypesRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public NotificationTypesRowChangeEvent(NotificationTypesRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class PatrollingProfilesRowChangeEvent : EventArgs
	{
		private PatrollingProfilesRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public PatrollingProfilesRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public PatrollingProfilesRowChangeEvent(PatrollingProfilesRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RecorderInfoRowChangeEvent : EventArgs
	{
		private RecorderInfoRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderInfoRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderInfoRowChangeEvent(RecorderInfoRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RecorderPluginDeviceRowChangeEvent : EventArgs
	{
		private RecorderPluginDeviceRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginDeviceRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginDeviceRowChangeEvent(RecorderPluginDeviceRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RecorderPluginsRowChangeEvent : EventArgs
	{
		private RecorderPluginsRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginsRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecorderPluginsRowChangeEvent(RecorderPluginsRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RecordersRowChangeEvent : EventArgs
	{
		private RecordersRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersRowChangeEvent(RecordersRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RecordingServerServiceConfigurableRowChangeEvent : EventArgs
	{
		private RecordingServerServiceConfigurableRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceConfigurableRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceConfigurableRowChangeEvent(RecordingServerServiceConfigurableRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RecordingServerServiceRecorderRowChangeEvent : EventArgs
	{
		private RecordingServerServiceRecorderRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRecorderRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRecorderRowChangeEvent(RecordingServerServiceRecorderRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RecordingServerServiceRowChangeEvent : EventArgs
	{
		private RecordingServerServiceRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingServerServiceRowChangeEvent(RecordingServerServiceRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RecordingStorageDeviceRowChangeEvent : EventArgs
	{
		private RecordingStorageDeviceRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDeviceRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageDeviceRowChangeEvent(RecordingStorageDeviceRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RecordingStorageRowChangeEvent : EventArgs
	{
		private RecordingStorageRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordingStorageRowChangeEvent(RecordingStorageRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RegisteredDeviceTypesRowChangeEvent : EventArgs
	{
		private RegisteredDeviceTypesRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredDeviceTypesRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredDeviceTypesRowChangeEvent(RegisteredDeviceTypesRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RegisteredServersRowChangeEvent : EventArgs
	{
		private RegisteredServersRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredServersRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RegisteredServersRowChangeEvent(RegisteredServersRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RulesRowChangeEvent : EventArgs
	{
		private RulesRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RulesRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RulesRowChangeEvent(RulesRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class RuleVariablesRowChangeEvent : EventArgs
	{
		private RuleVariablesRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RuleVariablesRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RuleVariablesRowChangeEvent(RuleVariablesRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class ServerGroupMembershipRowChangeEvent : EventArgs
	{
		private ServerGroupMembershipRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupMembershipRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupMembershipRowChangeEvent(ServerGroupMembershipRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class ServerGroupRowChangeEvent : EventArgs
	{
		private ServerGroupRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public ServerGroupRowChangeEvent(ServerGroupRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class SystemSettingsRowChangeEvent : EventArgs
	{
		private SystemSettingsRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SystemSettingsRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SystemSettingsRowChangeEvent(SystemSettingsRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class TimeProfileRowChangeEvent : EventArgs
	{
		private TimeProfileRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public TimeProfileRowChangeEvent(TimeProfileRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	private static ConfigurationDataSet readOnlyInstance;

	private object _dataChangedEventLock = new object();

	private ArchiveStorageDataTable tableArchiveStorage;

	private AudioMessageDataTable tableAudioMessage;

	private CompressorsDataTable tableCompressors;

	private DeviceDataDistributionDataTable tableDeviceDataDistribution;

	private DeviceGroupMembersDataTable tableDeviceGroupMembers;

	private DeviceGroupsDataTable tableDeviceGroups;

	private DeviceLinksDataTable tableDeviceLinks;

	private DevicePrivacyMaskDataTable tableDevicePrivacyMask;

	private DeviceRestrictedMediaDataTable tableDeviceRestrictedMedia;

	private DevicesDataTable tableDevices;

	private DeviceTrackDefinitionDataTable tableDeviceTrackDefinition;

	private DeviceTrackEvidenceLockDataTable tableDeviceTrackEvidenceLock;

	private DeviceTrackInfoDataTable tableDeviceTrackInfo;

	private DeviceTrackDataTable tableDeviceTrack;

	private DriverNumbersDataTable tableDriverNumbers;

	private EventTypesDataTable tableEventTypes;

	private ExternalPluginsDataTable tableExternalPlugins;

	private FailoverInfoDataTable tableFailoverInfo;

	private HardwareBlockDataTable tableHardwareBlock;

	private HardwareDefinitionDataTable tableHardwareDefinition;

	private HardwareDriverRecorderDataTable tableHardwareDriverRecorder;

	private HardwareDataTable tableHardware;

	private HostedHardwareDataTable tableHostedHardware;

	private HostingServerDataTable tableHostingServer;

	private LicenseDataTable tableLicense;

	private MatrixDataTable tableMatrix;

	private MIPObjectDataTable tableMIPObject;

	private NotificationProfilesDataTable tableNotificationProfiles;

	private NotificationSettingsDataTable tableNotificationSettings;

	private NotificationTypesDataTable tableNotificationTypes;

	private PatrollingProfilesDataTable tablePatrollingProfiles;

	private RecorderInfoDataTable tableRecorderInfo;

	private RecorderPluginDeviceDataTable tableRecorderPluginDevice;

	private RecorderPluginsDataTable tableRecorderPlugins;

	private RecordersDataTable tableRecorders;

	private RecordingServerServiceConfigurableDataTable tableRecordingServerServiceConfigurable;

	private RecordingServerServiceRecorderDataTable tableRecordingServerServiceRecorder;

	private RecordingServerServiceDataTable tableRecordingServerService;

	private RecordingStorageDeviceDataTable tableRecordingStorageDevice;

	private RecordingStorageDataTable tableRecordingStorage;

	private RegisteredDeviceTypesDataTable tableRegisteredDeviceTypes;

	private RegisteredServersDataTable tableRegisteredServers;

	private RulesDataTable tableRules;

	private RuleVariablesDataTable tableRuleVariables;

	private ServerGroupMembershipDataTable tableServerGroupMembership;

	private ServerGroupDataTable tableServerGroup;

	private SystemSettingsDataTable tableSystemSettings;

	private TimeProfileDataTable tableTimeProfile;

	private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

	public static ConfigurationDataSet ReadOnlyInstance
	{
		get
		{
			if (readOnlyInstance == null)
			{
				readOnlyInstance = new ConfigurationDataSet();
			}
			return readOnlyInstance;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ArchiveStorageDataTable ArchiveStorage => tableArchiveStorage;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public AudioMessageDataTable AudioMessage => tableAudioMessage;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public CompressorsDataTable Compressors => tableCompressors;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DeviceDataDistributionDataTable DeviceDataDistribution => tableDeviceDataDistribution;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DeviceGroupMembersDataTable DeviceGroupMembers => tableDeviceGroupMembers;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DeviceGroupsDataTable DeviceGroups => tableDeviceGroups;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DeviceLinksDataTable DeviceLinks => tableDeviceLinks;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DevicePrivacyMaskDataTable DevicePrivacyMask => tableDevicePrivacyMask;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DeviceRestrictedMediaDataTable DeviceRestrictedMedia => tableDeviceRestrictedMedia;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DevicesDataTable Devices => tableDevices;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DeviceTrackDefinitionDataTable DeviceTrackDefinition => tableDeviceTrackDefinition;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DeviceTrackEvidenceLockDataTable DeviceTrackEvidenceLock => tableDeviceTrackEvidenceLock;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DeviceTrackInfoDataTable DeviceTrackInfo => tableDeviceTrackInfo;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DeviceTrackDataTable DeviceTrack => tableDeviceTrack;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DriverNumbersDataTable DriverNumbers => tableDriverNumbers;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public EventTypesDataTable EventTypes => tableEventTypes;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ExternalPluginsDataTable ExternalPlugins => tableExternalPlugins;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public FailoverInfoDataTable FailoverInfo => tableFailoverInfo;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public HardwareBlockDataTable HardwareBlock => tableHardwareBlock;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public HardwareDefinitionDataTable HardwareDefinition => tableHardwareDefinition;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public HardwareDriverRecorderDataTable HardwareDriverRecorder => tableHardwareDriverRecorder;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public HardwareDataTable Hardware => tableHardware;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public HostedHardwareDataTable HostedHardware => tableHostedHardware;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public HostingServerDataTable HostingServer => tableHostingServer;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public LicenseDataTable License => tableLicense;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public MatrixDataTable Matrix => tableMatrix;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public MIPObjectDataTable MIPObject => tableMIPObject;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public NotificationProfilesDataTable NotificationProfiles => tableNotificationProfiles;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public NotificationSettingsDataTable NotificationSettings => tableNotificationSettings;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public NotificationTypesDataTable NotificationTypes => tableNotificationTypes;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public PatrollingProfilesDataTable PatrollingProfiles => tablePatrollingProfiles;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RecorderInfoDataTable RecorderInfo => tableRecorderInfo;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RecorderPluginDeviceDataTable RecorderPluginDevice => tableRecorderPluginDevice;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RecorderPluginsDataTable RecorderPlugins => tableRecorderPlugins;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RecordersDataTable Recorders => tableRecorders;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RecordingServerServiceConfigurableDataTable RecordingServerServiceConfigurable => tableRecordingServerServiceConfigurable;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RecordingServerServiceRecorderDataTable RecordingServerServiceRecorder => tableRecordingServerServiceRecorder;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RecordingServerServiceDataTable RecordingServerService => tableRecordingServerService;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RecordingStorageDeviceDataTable RecordingStorageDevice => tableRecordingStorageDevice;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RecordingStorageDataTable RecordingStorage => tableRecordingStorage;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RegisteredDeviceTypesDataTable RegisteredDeviceTypes => tableRegisteredDeviceTypes;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RegisteredServersDataTable RegisteredServers => tableRegisteredServers;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RulesDataTable Rules => tableRules;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RuleVariablesDataTable RuleVariables => tableRuleVariables;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ServerGroupMembershipDataTable ServerGroupMembership => tableServerGroupMembership;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ServerGroupDataTable ServerGroup => tableServerGroup;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SystemSettingsDataTable SystemSettings => tableSystemSettings;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public TimeProfileDataTable TimeProfile => tableTimeProfile;

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

	public new ConfigurationDataSet GetChanges()
	{
		return (ConfigurationDataSet)base.GetChanges();
	}

	public new ConfigurationDataSet GetChanges(DataRowState rowStates)
	{
		return (ConfigurationDataSet)base.GetChanges(rowStates);
	}

	protected virtual void OnDataChanged(object sender, DataRowChangeEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		if (e.Row.RowState != DataRowState.Unchanged || e.Action == DataRowAction.Rollback)
		{
			EventHandler<DataRowChangeEventArgs> eventHandler;
			lock (_dataChangedEventLock)
			{
				eventHandler = this._dataChanged;
			}
			eventHandler?.Invoke(sender, e);
		}
	}

	public static void UpdateEventTypes(ConfigurationManager configurationManager, string eventDefinitions)
	{
		if (configurationManager == null)
		{
			throw new ArgumentNullException("configurationManager");
		}
		if (eventDefinitions == null)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(eventDefinitions);
		XmlElement documentElement = xmlDocument.DocumentElement;
		if (documentElement == null)
		{
			return;
		}
		foreach (XmlNode childNode in documentElement.ChildNodes)
		{
			EventDefinition eventDefinition = new EventDefinition(childNode.OuterXml);
			EventType eventType = configurationManager.EventTypes[eventDefinition.ID];
			if (eventType == null)
			{
				configurationManager.EventTypes.Add(eventDefinition.ID, eventDefinition.Name, eventDefinition.SaveAsXml(), eventDefinition.GeneratorType, eventDefinition.GeneratorSubtype, eventDefinition.GeneratorID, builtIn: false);
				continue;
			}
			EventDefinition definition = eventType.Definition;
			bool flag = false;
			EventSource[] sources = eventDefinition.Sources;
			foreach (EventSource eventSource in sources)
			{
				if (!definition.SourceListContains(eventSource.Type))
				{
					EventSource[] array = new EventSource[definition.Sources.Length + 1];
					definition.Sources.CopyTo(array, 0);
					array[definition.Sources.Length] = eventSource;
					definition.Sources = array;
					flag = true;
					continue;
				}
				if (eventSource.Filter == EventSource.FilterTypes.None)
				{
					EventSource eventSource2 = null;
					EventSource[] sources2 = definition.Sources;
					foreach (EventSource eventSource3 in sources2)
					{
						if (eventSource3.Type == eventSource.Type)
						{
							eventSource2 = eventSource3;
							break;
						}
					}
					if (eventSource2 != null && eventSource2.Filter != EventSource.FilterTypes.None)
					{
						eventSource2.Filter = EventSource.FilterTypes.None;
						flag = true;
					}
				}
				if (definition.DisplayName != eventDefinition.DisplayName)
				{
					definition.DisplayName = eventDefinition.DisplayName;
					flag = true;
				}
			}
			if (definition.CounterEventID != eventDefinition.CounterEventID && definition.CounterEventID == definition.ID)
			{
				definition.CounterEventID = eventDefinition.CounterEventID;
				flag = true;
			}
			if (flag)
			{
				eventType.DefinitionXml = definition.SaveAsXml();
			}
		}
	}

	public void UpdateLastModified()
	{
		DataSetUtility.UpdateLastModified(this, onlyUpdateUnmodifiedTimestamps: true);
	}

	public string ToDiffGramXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (StringWriter w = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
		{
			using XmlTextWriter writer = new XmlTextWriter(w);
			WriteXml(writer, XmlWriteMode.DiffGram);
		}
		return stringBuilder.ToString();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public ConfigurationDataSet()
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
	protected ConfigurationDataSet(SerializationInfo info, StreamingContext context)
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
			if (dataSet.Tables["ArchiveStorage"] != null)
			{
				base.Tables.Add(new ArchiveStorageDataTable(dataSet.Tables["ArchiveStorage"]));
			}
			if (dataSet.Tables["AudioMessage"] != null)
			{
				base.Tables.Add(new AudioMessageDataTable(dataSet.Tables["AudioMessage"]));
			}
			if (dataSet.Tables["Compressors"] != null)
			{
				base.Tables.Add(new CompressorsDataTable(dataSet.Tables["Compressors"]));
			}
			if (dataSet.Tables["DeviceDataDistribution"] != null)
			{
				base.Tables.Add(new DeviceDataDistributionDataTable(dataSet.Tables["DeviceDataDistribution"]));
			}
			if (dataSet.Tables["DeviceGroupMembers"] != null)
			{
				base.Tables.Add(new DeviceGroupMembersDataTable(dataSet.Tables["DeviceGroupMembers"]));
			}
			if (dataSet.Tables["DeviceGroups"] != null)
			{
				base.Tables.Add(new DeviceGroupsDataTable(dataSet.Tables["DeviceGroups"]));
			}
			if (dataSet.Tables["DeviceLinks"] != null)
			{
				base.Tables.Add(new DeviceLinksDataTable(dataSet.Tables["DeviceLinks"]));
			}
			if (dataSet.Tables["DevicePrivacyMask"] != null)
			{
				base.Tables.Add(new DevicePrivacyMaskDataTable(dataSet.Tables["DevicePrivacyMask"]));
			}
			if (dataSet.Tables["DeviceRestrictedMedia"] != null)
			{
				base.Tables.Add(new DeviceRestrictedMediaDataTable(dataSet.Tables["DeviceRestrictedMedia"]));
			}
			if (dataSet.Tables["Devices"] != null)
			{
				base.Tables.Add(new DevicesDataTable(dataSet.Tables["Devices"]));
			}
			if (dataSet.Tables["DeviceTrackDefinition"] != null)
			{
				base.Tables.Add(new DeviceTrackDefinitionDataTable(dataSet.Tables["DeviceTrackDefinition"]));
			}
			if (dataSet.Tables["DeviceTrackEvidenceLock"] != null)
			{
				base.Tables.Add(new DeviceTrackEvidenceLockDataTable(dataSet.Tables["DeviceTrackEvidenceLock"]));
			}
			if (dataSet.Tables["DeviceTrackInfo"] != null)
			{
				base.Tables.Add(new DeviceTrackInfoDataTable(dataSet.Tables["DeviceTrackInfo"]));
			}
			if (dataSet.Tables["DeviceTrack"] != null)
			{
				base.Tables.Add(new DeviceTrackDataTable(dataSet.Tables["DeviceTrack"]));
			}
			if (dataSet.Tables["DriverNumbers"] != null)
			{
				base.Tables.Add(new DriverNumbersDataTable(dataSet.Tables["DriverNumbers"]));
			}
			if (dataSet.Tables["EventTypes"] != null)
			{
				base.Tables.Add(new EventTypesDataTable(dataSet.Tables["EventTypes"]));
			}
			if (dataSet.Tables["ExternalPlugins"] != null)
			{
				base.Tables.Add(new ExternalPluginsDataTable(dataSet.Tables["ExternalPlugins"]));
			}
			if (dataSet.Tables["FailoverInfo"] != null)
			{
				base.Tables.Add(new FailoverInfoDataTable(dataSet.Tables["FailoverInfo"]));
			}
			if (dataSet.Tables["HardwareBlock"] != null)
			{
				base.Tables.Add(new HardwareBlockDataTable(dataSet.Tables["HardwareBlock"]));
			}
			if (dataSet.Tables["HardwareDefinition"] != null)
			{
				base.Tables.Add(new HardwareDefinitionDataTable(dataSet.Tables["HardwareDefinition"]));
			}
			if (dataSet.Tables["HardwareDriverRecorder"] != null)
			{
				base.Tables.Add(new HardwareDriverRecorderDataTable(dataSet.Tables["HardwareDriverRecorder"]));
			}
			if (dataSet.Tables["Hardware"] != null)
			{
				base.Tables.Add(new HardwareDataTable(dataSet.Tables["Hardware"]));
			}
			if (dataSet.Tables["HostedHardware"] != null)
			{
				base.Tables.Add(new HostedHardwareDataTable(dataSet.Tables["HostedHardware"]));
			}
			if (dataSet.Tables["HostingServer"] != null)
			{
				base.Tables.Add(new HostingServerDataTable(dataSet.Tables["HostingServer"]));
			}
			if (dataSet.Tables["License"] != null)
			{
				base.Tables.Add(new LicenseDataTable(dataSet.Tables["License"]));
			}
			if (dataSet.Tables["Matrix"] != null)
			{
				base.Tables.Add(new MatrixDataTable(dataSet.Tables["Matrix"]));
			}
			if (dataSet.Tables["MIPObject"] != null)
			{
				base.Tables.Add(new MIPObjectDataTable(dataSet.Tables["MIPObject"]));
			}
			if (dataSet.Tables["NotificationProfiles"] != null)
			{
				base.Tables.Add(new NotificationProfilesDataTable(dataSet.Tables["NotificationProfiles"]));
			}
			if (dataSet.Tables["NotificationSettings"] != null)
			{
				base.Tables.Add(new NotificationSettingsDataTable(dataSet.Tables["NotificationSettings"]));
			}
			if (dataSet.Tables["NotificationTypes"] != null)
			{
				base.Tables.Add(new NotificationTypesDataTable(dataSet.Tables["NotificationTypes"]));
			}
			if (dataSet.Tables["PatrollingProfiles"] != null)
			{
				base.Tables.Add(new PatrollingProfilesDataTable(dataSet.Tables["PatrollingProfiles"]));
			}
			if (dataSet.Tables["RecorderInfo"] != null)
			{
				base.Tables.Add(new RecorderInfoDataTable(dataSet.Tables["RecorderInfo"]));
			}
			if (dataSet.Tables["RecorderPluginDevice"] != null)
			{
				base.Tables.Add(new RecorderPluginDeviceDataTable(dataSet.Tables["RecorderPluginDevice"]));
			}
			if (dataSet.Tables["RecorderPlugins"] != null)
			{
				base.Tables.Add(new RecorderPluginsDataTable(dataSet.Tables["RecorderPlugins"]));
			}
			if (dataSet.Tables["Recorders"] != null)
			{
				base.Tables.Add(new RecordersDataTable(dataSet.Tables["Recorders"]));
			}
			if (dataSet.Tables["RecordingServerServiceConfigurable"] != null)
			{
				base.Tables.Add(new RecordingServerServiceConfigurableDataTable(dataSet.Tables["RecordingServerServiceConfigurable"]));
			}
			if (dataSet.Tables["RecordingServerServiceRecorder"] != null)
			{
				base.Tables.Add(new RecordingServerServiceRecorderDataTable(dataSet.Tables["RecordingServerServiceRecorder"]));
			}
			if (dataSet.Tables["RecordingServerService"] != null)
			{
				base.Tables.Add(new RecordingServerServiceDataTable(dataSet.Tables["RecordingServerService"]));
			}
			if (dataSet.Tables["RecordingStorageDevice"] != null)
			{
				base.Tables.Add(new RecordingStorageDeviceDataTable(dataSet.Tables["RecordingStorageDevice"]));
			}
			if (dataSet.Tables["RecordingStorage"] != null)
			{
				base.Tables.Add(new RecordingStorageDataTable(dataSet.Tables["RecordingStorage"]));
			}
			if (dataSet.Tables["RegisteredDeviceTypes"] != null)
			{
				base.Tables.Add(new RegisteredDeviceTypesDataTable(dataSet.Tables["RegisteredDeviceTypes"]));
			}
			if (dataSet.Tables["RegisteredServers"] != null)
			{
				base.Tables.Add(new RegisteredServersDataTable(dataSet.Tables["RegisteredServers"]));
			}
			if (dataSet.Tables["Rules"] != null)
			{
				base.Tables.Add(new RulesDataTable(dataSet.Tables["Rules"]));
			}
			if (dataSet.Tables["RuleVariables"] != null)
			{
				base.Tables.Add(new RuleVariablesDataTable(dataSet.Tables["RuleVariables"]));
			}
			if (dataSet.Tables["ServerGroupMembership"] != null)
			{
				base.Tables.Add(new ServerGroupMembershipDataTable(dataSet.Tables["ServerGroupMembership"]));
			}
			if (dataSet.Tables["ServerGroup"] != null)
			{
				base.Tables.Add(new ServerGroupDataTable(dataSet.Tables["ServerGroup"]));
			}
			if (dataSet.Tables["SystemSettings"] != null)
			{
				base.Tables.Add(new SystemSettingsDataTable(dataSet.Tables["SystemSettings"]));
			}
			if (dataSet.Tables["TimeProfile"] != null)
			{
				base.Tables.Add(new TimeProfileDataTable(dataSet.Tables["TimeProfile"]));
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
		ConfigurationDataSet obj = (ConfigurationDataSet)base.Clone();
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
			if (dataSet.Tables["ArchiveStorage"] != null)
			{
				base.Tables.Add(new ArchiveStorageDataTable(dataSet.Tables["ArchiveStorage"]));
			}
			if (dataSet.Tables["AudioMessage"] != null)
			{
				base.Tables.Add(new AudioMessageDataTable(dataSet.Tables["AudioMessage"]));
			}
			if (dataSet.Tables["Compressors"] != null)
			{
				base.Tables.Add(new CompressorsDataTable(dataSet.Tables["Compressors"]));
			}
			if (dataSet.Tables["DeviceDataDistribution"] != null)
			{
				base.Tables.Add(new DeviceDataDistributionDataTable(dataSet.Tables["DeviceDataDistribution"]));
			}
			if (dataSet.Tables["DeviceGroupMembers"] != null)
			{
				base.Tables.Add(new DeviceGroupMembersDataTable(dataSet.Tables["DeviceGroupMembers"]));
			}
			if (dataSet.Tables["DeviceGroups"] != null)
			{
				base.Tables.Add(new DeviceGroupsDataTable(dataSet.Tables["DeviceGroups"]));
			}
			if (dataSet.Tables["DeviceLinks"] != null)
			{
				base.Tables.Add(new DeviceLinksDataTable(dataSet.Tables["DeviceLinks"]));
			}
			if (dataSet.Tables["DevicePrivacyMask"] != null)
			{
				base.Tables.Add(new DevicePrivacyMaskDataTable(dataSet.Tables["DevicePrivacyMask"]));
			}
			if (dataSet.Tables["DeviceRestrictedMedia"] != null)
			{
				base.Tables.Add(new DeviceRestrictedMediaDataTable(dataSet.Tables["DeviceRestrictedMedia"]));
			}
			if (dataSet.Tables["Devices"] != null)
			{
				base.Tables.Add(new DevicesDataTable(dataSet.Tables["Devices"]));
			}
			if (dataSet.Tables["DeviceTrackDefinition"] != null)
			{
				base.Tables.Add(new DeviceTrackDefinitionDataTable(dataSet.Tables["DeviceTrackDefinition"]));
			}
			if (dataSet.Tables["DeviceTrackEvidenceLock"] != null)
			{
				base.Tables.Add(new DeviceTrackEvidenceLockDataTable(dataSet.Tables["DeviceTrackEvidenceLock"]));
			}
			if (dataSet.Tables["DeviceTrackInfo"] != null)
			{
				base.Tables.Add(new DeviceTrackInfoDataTable(dataSet.Tables["DeviceTrackInfo"]));
			}
			if (dataSet.Tables["DeviceTrack"] != null)
			{
				base.Tables.Add(new DeviceTrackDataTable(dataSet.Tables["DeviceTrack"]));
			}
			if (dataSet.Tables["DriverNumbers"] != null)
			{
				base.Tables.Add(new DriverNumbersDataTable(dataSet.Tables["DriverNumbers"]));
			}
			if (dataSet.Tables["EventTypes"] != null)
			{
				base.Tables.Add(new EventTypesDataTable(dataSet.Tables["EventTypes"]));
			}
			if (dataSet.Tables["ExternalPlugins"] != null)
			{
				base.Tables.Add(new ExternalPluginsDataTable(dataSet.Tables["ExternalPlugins"]));
			}
			if (dataSet.Tables["FailoverInfo"] != null)
			{
				base.Tables.Add(new FailoverInfoDataTable(dataSet.Tables["FailoverInfo"]));
			}
			if (dataSet.Tables["HardwareBlock"] != null)
			{
				base.Tables.Add(new HardwareBlockDataTable(dataSet.Tables["HardwareBlock"]));
			}
			if (dataSet.Tables["HardwareDefinition"] != null)
			{
				base.Tables.Add(new HardwareDefinitionDataTable(dataSet.Tables["HardwareDefinition"]));
			}
			if (dataSet.Tables["HardwareDriverRecorder"] != null)
			{
				base.Tables.Add(new HardwareDriverRecorderDataTable(dataSet.Tables["HardwareDriverRecorder"]));
			}
			if (dataSet.Tables["Hardware"] != null)
			{
				base.Tables.Add(new HardwareDataTable(dataSet.Tables["Hardware"]));
			}
			if (dataSet.Tables["HostedHardware"] != null)
			{
				base.Tables.Add(new HostedHardwareDataTable(dataSet.Tables["HostedHardware"]));
			}
			if (dataSet.Tables["HostingServer"] != null)
			{
				base.Tables.Add(new HostingServerDataTable(dataSet.Tables["HostingServer"]));
			}
			if (dataSet.Tables["License"] != null)
			{
				base.Tables.Add(new LicenseDataTable(dataSet.Tables["License"]));
			}
			if (dataSet.Tables["Matrix"] != null)
			{
				base.Tables.Add(new MatrixDataTable(dataSet.Tables["Matrix"]));
			}
			if (dataSet.Tables["MIPObject"] != null)
			{
				base.Tables.Add(new MIPObjectDataTable(dataSet.Tables["MIPObject"]));
			}
			if (dataSet.Tables["NotificationProfiles"] != null)
			{
				base.Tables.Add(new NotificationProfilesDataTable(dataSet.Tables["NotificationProfiles"]));
			}
			if (dataSet.Tables["NotificationSettings"] != null)
			{
				base.Tables.Add(new NotificationSettingsDataTable(dataSet.Tables["NotificationSettings"]));
			}
			if (dataSet.Tables["NotificationTypes"] != null)
			{
				base.Tables.Add(new NotificationTypesDataTable(dataSet.Tables["NotificationTypes"]));
			}
			if (dataSet.Tables["PatrollingProfiles"] != null)
			{
				base.Tables.Add(new PatrollingProfilesDataTable(dataSet.Tables["PatrollingProfiles"]));
			}
			if (dataSet.Tables["RecorderInfo"] != null)
			{
				base.Tables.Add(new RecorderInfoDataTable(dataSet.Tables["RecorderInfo"]));
			}
			if (dataSet.Tables["RecorderPluginDevice"] != null)
			{
				base.Tables.Add(new RecorderPluginDeviceDataTable(dataSet.Tables["RecorderPluginDevice"]));
			}
			if (dataSet.Tables["RecorderPlugins"] != null)
			{
				base.Tables.Add(new RecorderPluginsDataTable(dataSet.Tables["RecorderPlugins"]));
			}
			if (dataSet.Tables["Recorders"] != null)
			{
				base.Tables.Add(new RecordersDataTable(dataSet.Tables["Recorders"]));
			}
			if (dataSet.Tables["RecordingServerServiceConfigurable"] != null)
			{
				base.Tables.Add(new RecordingServerServiceConfigurableDataTable(dataSet.Tables["RecordingServerServiceConfigurable"]));
			}
			if (dataSet.Tables["RecordingServerServiceRecorder"] != null)
			{
				base.Tables.Add(new RecordingServerServiceRecorderDataTable(dataSet.Tables["RecordingServerServiceRecorder"]));
			}
			if (dataSet.Tables["RecordingServerService"] != null)
			{
				base.Tables.Add(new RecordingServerServiceDataTable(dataSet.Tables["RecordingServerService"]));
			}
			if (dataSet.Tables["RecordingStorageDevice"] != null)
			{
				base.Tables.Add(new RecordingStorageDeviceDataTable(dataSet.Tables["RecordingStorageDevice"]));
			}
			if (dataSet.Tables["RecordingStorage"] != null)
			{
				base.Tables.Add(new RecordingStorageDataTable(dataSet.Tables["RecordingStorage"]));
			}
			if (dataSet.Tables["RegisteredDeviceTypes"] != null)
			{
				base.Tables.Add(new RegisteredDeviceTypesDataTable(dataSet.Tables["RegisteredDeviceTypes"]));
			}
			if (dataSet.Tables["RegisteredServers"] != null)
			{
				base.Tables.Add(new RegisteredServersDataTable(dataSet.Tables["RegisteredServers"]));
			}
			if (dataSet.Tables["Rules"] != null)
			{
				base.Tables.Add(new RulesDataTable(dataSet.Tables["Rules"]));
			}
			if (dataSet.Tables["RuleVariables"] != null)
			{
				base.Tables.Add(new RuleVariablesDataTable(dataSet.Tables["RuleVariables"]));
			}
			if (dataSet.Tables["ServerGroupMembership"] != null)
			{
				base.Tables.Add(new ServerGroupMembershipDataTable(dataSet.Tables["ServerGroupMembership"]));
			}
			if (dataSet.Tables["ServerGroup"] != null)
			{
				base.Tables.Add(new ServerGroupDataTable(dataSet.Tables["ServerGroup"]));
			}
			if (dataSet.Tables["SystemSettings"] != null)
			{
				base.Tables.Add(new SystemSettingsDataTable(dataSet.Tables["SystemSettings"]));
			}
			if (dataSet.Tables["TimeProfile"] != null)
			{
				base.Tables.Add(new TimeProfileDataTable(dataSet.Tables["TimeProfile"]));
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
		tableArchiveStorage = (ArchiveStorageDataTable)base.Tables["ArchiveStorage"];
		if (initTable && tableArchiveStorage != null)
		{
			tableArchiveStorage.InitVars();
		}
		tableAudioMessage = (AudioMessageDataTable)base.Tables["AudioMessage"];
		if (initTable && tableAudioMessage != null)
		{
			tableAudioMessage.InitVars();
		}
		tableCompressors = (CompressorsDataTable)base.Tables["Compressors"];
		if (initTable && tableCompressors != null)
		{
			tableCompressors.InitVars();
		}
		tableDeviceDataDistribution = (DeviceDataDistributionDataTable)base.Tables["DeviceDataDistribution"];
		if (initTable && tableDeviceDataDistribution != null)
		{
			tableDeviceDataDistribution.InitVars();
		}
		tableDeviceGroupMembers = (DeviceGroupMembersDataTable)base.Tables["DeviceGroupMembers"];
		if (initTable && tableDeviceGroupMembers != null)
		{
			tableDeviceGroupMembers.InitVars();
		}
		tableDeviceGroups = (DeviceGroupsDataTable)base.Tables["DeviceGroups"];
		if (initTable && tableDeviceGroups != null)
		{
			tableDeviceGroups.InitVars();
		}
		tableDeviceLinks = (DeviceLinksDataTable)base.Tables["DeviceLinks"];
		if (initTable && tableDeviceLinks != null)
		{
			tableDeviceLinks.InitVars();
		}
		tableDevicePrivacyMask = (DevicePrivacyMaskDataTable)base.Tables["DevicePrivacyMask"];
		if (initTable && tableDevicePrivacyMask != null)
		{
			tableDevicePrivacyMask.InitVars();
		}
		tableDeviceRestrictedMedia = (DeviceRestrictedMediaDataTable)base.Tables["DeviceRestrictedMedia"];
		if (initTable && tableDeviceRestrictedMedia != null)
		{
			tableDeviceRestrictedMedia.InitVars();
		}
		tableDevices = (DevicesDataTable)base.Tables["Devices"];
		if (initTable && tableDevices != null)
		{
			tableDevices.InitVars();
		}
		tableDeviceTrackDefinition = (DeviceTrackDefinitionDataTable)base.Tables["DeviceTrackDefinition"];
		if (initTable && tableDeviceTrackDefinition != null)
		{
			tableDeviceTrackDefinition.InitVars();
		}
		tableDeviceTrackEvidenceLock = (DeviceTrackEvidenceLockDataTable)base.Tables["DeviceTrackEvidenceLock"];
		if (initTable && tableDeviceTrackEvidenceLock != null)
		{
			tableDeviceTrackEvidenceLock.InitVars();
		}
		tableDeviceTrackInfo = (DeviceTrackInfoDataTable)base.Tables["DeviceTrackInfo"];
		if (initTable && tableDeviceTrackInfo != null)
		{
			tableDeviceTrackInfo.InitVars();
		}
		tableDeviceTrack = (DeviceTrackDataTable)base.Tables["DeviceTrack"];
		if (initTable && tableDeviceTrack != null)
		{
			tableDeviceTrack.InitVars();
		}
		tableDriverNumbers = (DriverNumbersDataTable)base.Tables["DriverNumbers"];
		if (initTable && tableDriverNumbers != null)
		{
			tableDriverNumbers.InitVars();
		}
		tableEventTypes = (EventTypesDataTable)base.Tables["EventTypes"];
		if (initTable && tableEventTypes != null)
		{
			tableEventTypes.InitVars();
		}
		tableExternalPlugins = (ExternalPluginsDataTable)base.Tables["ExternalPlugins"];
		if (initTable && tableExternalPlugins != null)
		{
			tableExternalPlugins.InitVars();
		}
		tableFailoverInfo = (FailoverInfoDataTable)base.Tables["FailoverInfo"];
		if (initTable && tableFailoverInfo != null)
		{
			tableFailoverInfo.InitVars();
		}
		tableHardwareBlock = (HardwareBlockDataTable)base.Tables["HardwareBlock"];
		if (initTable && tableHardwareBlock != null)
		{
			tableHardwareBlock.InitVars();
		}
		tableHardwareDefinition = (HardwareDefinitionDataTable)base.Tables["HardwareDefinition"];
		if (initTable && tableHardwareDefinition != null)
		{
			tableHardwareDefinition.InitVars();
		}
		tableHardwareDriverRecorder = (HardwareDriverRecorderDataTable)base.Tables["HardwareDriverRecorder"];
		if (initTable && tableHardwareDriverRecorder != null)
		{
			tableHardwareDriverRecorder.InitVars();
		}
		tableHardware = (HardwareDataTable)base.Tables["Hardware"];
		if (initTable && tableHardware != null)
		{
			tableHardware.InitVars();
		}
		tableHostedHardware = (HostedHardwareDataTable)base.Tables["HostedHardware"];
		if (initTable && tableHostedHardware != null)
		{
			tableHostedHardware.InitVars();
		}
		tableHostingServer = (HostingServerDataTable)base.Tables["HostingServer"];
		if (initTable && tableHostingServer != null)
		{
			tableHostingServer.InitVars();
		}
		tableLicense = (LicenseDataTable)base.Tables["License"];
		if (initTable && tableLicense != null)
		{
			tableLicense.InitVars();
		}
		tableMatrix = (MatrixDataTable)base.Tables["Matrix"];
		if (initTable && tableMatrix != null)
		{
			tableMatrix.InitVars();
		}
		tableMIPObject = (MIPObjectDataTable)base.Tables["MIPObject"];
		if (initTable && tableMIPObject != null)
		{
			tableMIPObject.InitVars();
		}
		tableNotificationProfiles = (NotificationProfilesDataTable)base.Tables["NotificationProfiles"];
		if (initTable && tableNotificationProfiles != null)
		{
			tableNotificationProfiles.InitVars();
		}
		tableNotificationSettings = (NotificationSettingsDataTable)base.Tables["NotificationSettings"];
		if (initTable && tableNotificationSettings != null)
		{
			tableNotificationSettings.InitVars();
		}
		tableNotificationTypes = (NotificationTypesDataTable)base.Tables["NotificationTypes"];
		if (initTable && tableNotificationTypes != null)
		{
			tableNotificationTypes.InitVars();
		}
		tablePatrollingProfiles = (PatrollingProfilesDataTable)base.Tables["PatrollingProfiles"];
		if (initTable && tablePatrollingProfiles != null)
		{
			tablePatrollingProfiles.InitVars();
		}
		tableRecorderInfo = (RecorderInfoDataTable)base.Tables["RecorderInfo"];
		if (initTable && tableRecorderInfo != null)
		{
			tableRecorderInfo.InitVars();
		}
		tableRecorderPluginDevice = (RecorderPluginDeviceDataTable)base.Tables["RecorderPluginDevice"];
		if (initTable && tableRecorderPluginDevice != null)
		{
			tableRecorderPluginDevice.InitVars();
		}
		tableRecorderPlugins = (RecorderPluginsDataTable)base.Tables["RecorderPlugins"];
		if (initTable && tableRecorderPlugins != null)
		{
			tableRecorderPlugins.InitVars();
		}
		tableRecorders = (RecordersDataTable)base.Tables["Recorders"];
		if (initTable && tableRecorders != null)
		{
			tableRecorders.InitVars();
		}
		tableRecordingServerServiceConfigurable = (RecordingServerServiceConfigurableDataTable)base.Tables["RecordingServerServiceConfigurable"];
		if (initTable && tableRecordingServerServiceConfigurable != null)
		{
			tableRecordingServerServiceConfigurable.InitVars();
		}
		tableRecordingServerServiceRecorder = (RecordingServerServiceRecorderDataTable)base.Tables["RecordingServerServiceRecorder"];
		if (initTable && tableRecordingServerServiceRecorder != null)
		{
			tableRecordingServerServiceRecorder.InitVars();
		}
		tableRecordingServerService = (RecordingServerServiceDataTable)base.Tables["RecordingServerService"];
		if (initTable && tableRecordingServerService != null)
		{
			tableRecordingServerService.InitVars();
		}
		tableRecordingStorageDevice = (RecordingStorageDeviceDataTable)base.Tables["RecordingStorageDevice"];
		if (initTable && tableRecordingStorageDevice != null)
		{
			tableRecordingStorageDevice.InitVars();
		}
		tableRecordingStorage = (RecordingStorageDataTable)base.Tables["RecordingStorage"];
		if (initTable && tableRecordingStorage != null)
		{
			tableRecordingStorage.InitVars();
		}
		tableRegisteredDeviceTypes = (RegisteredDeviceTypesDataTable)base.Tables["RegisteredDeviceTypes"];
		if (initTable && tableRegisteredDeviceTypes != null)
		{
			tableRegisteredDeviceTypes.InitVars();
		}
		tableRegisteredServers = (RegisteredServersDataTable)base.Tables["RegisteredServers"];
		if (initTable && tableRegisteredServers != null)
		{
			tableRegisteredServers.InitVars();
		}
		tableRules = (RulesDataTable)base.Tables["Rules"];
		if (initTable && tableRules != null)
		{
			tableRules.InitVars();
		}
		tableRuleVariables = (RuleVariablesDataTable)base.Tables["RuleVariables"];
		if (initTable && tableRuleVariables != null)
		{
			tableRuleVariables.InitVars();
		}
		tableServerGroupMembership = (ServerGroupMembershipDataTable)base.Tables["ServerGroupMembership"];
		if (initTable && tableServerGroupMembership != null)
		{
			tableServerGroupMembership.InitVars();
		}
		tableServerGroup = (ServerGroupDataTable)base.Tables["ServerGroup"];
		if (initTable && tableServerGroup != null)
		{
			tableServerGroup.InitVars();
		}
		tableSystemSettings = (SystemSettingsDataTable)base.Tables["SystemSettings"];
		if (initTable && tableSystemSettings != null)
		{
			tableSystemSettings.InitVars();
		}
		tableTimeProfile = (TimeProfileDataTable)base.Tables["TimeProfile"];
		if (initTable && tableTimeProfile != null)
		{
			tableTimeProfile.InitVars();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private void InitClass()
	{
		base.DataSetName = "ConfigurationDataSet";
		base.Prefix = "";
		base.Namespace = "http://www.VideoOS.net/4/ConfigurationDataSet.xsd";
		base.Locale = new CultureInfo("da-DK");
		base.EnforceConstraints = false;
		SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
		tableArchiveStorage = new ArchiveStorageDataTable();
		base.Tables.Add(tableArchiveStorage);
		tableAudioMessage = new AudioMessageDataTable();
		base.Tables.Add(tableAudioMessage);
		tableCompressors = new CompressorsDataTable();
		base.Tables.Add(tableCompressors);
		tableDeviceDataDistribution = new DeviceDataDistributionDataTable();
		base.Tables.Add(tableDeviceDataDistribution);
		tableDeviceGroupMembers = new DeviceGroupMembersDataTable();
		base.Tables.Add(tableDeviceGroupMembers);
		tableDeviceGroups = new DeviceGroupsDataTable();
		base.Tables.Add(tableDeviceGroups);
		tableDeviceLinks = new DeviceLinksDataTable();
		base.Tables.Add(tableDeviceLinks);
		tableDevicePrivacyMask = new DevicePrivacyMaskDataTable();
		base.Tables.Add(tableDevicePrivacyMask);
		tableDeviceRestrictedMedia = new DeviceRestrictedMediaDataTable();
		base.Tables.Add(tableDeviceRestrictedMedia);
		tableDevices = new DevicesDataTable();
		base.Tables.Add(tableDevices);
		tableDeviceTrackDefinition = new DeviceTrackDefinitionDataTable();
		base.Tables.Add(tableDeviceTrackDefinition);
		tableDeviceTrackEvidenceLock = new DeviceTrackEvidenceLockDataTable();
		base.Tables.Add(tableDeviceTrackEvidenceLock);
		tableDeviceTrackInfo = new DeviceTrackInfoDataTable();
		base.Tables.Add(tableDeviceTrackInfo);
		tableDeviceTrack = new DeviceTrackDataTable();
		base.Tables.Add(tableDeviceTrack);
		tableDriverNumbers = new DriverNumbersDataTable();
		base.Tables.Add(tableDriverNumbers);
		tableEventTypes = new EventTypesDataTable();
		base.Tables.Add(tableEventTypes);
		tableExternalPlugins = new ExternalPluginsDataTable();
		base.Tables.Add(tableExternalPlugins);
		tableFailoverInfo = new FailoverInfoDataTable();
		base.Tables.Add(tableFailoverInfo);
		tableHardwareBlock = new HardwareBlockDataTable();
		base.Tables.Add(tableHardwareBlock);
		tableHardwareDefinition = new HardwareDefinitionDataTable();
		base.Tables.Add(tableHardwareDefinition);
		tableHardwareDriverRecorder = new HardwareDriverRecorderDataTable();
		base.Tables.Add(tableHardwareDriverRecorder);
		tableHardware = new HardwareDataTable();
		base.Tables.Add(tableHardware);
		tableHostedHardware = new HostedHardwareDataTable();
		base.Tables.Add(tableHostedHardware);
		tableHostingServer = new HostingServerDataTable();
		base.Tables.Add(tableHostingServer);
		tableLicense = new LicenseDataTable();
		base.Tables.Add(tableLicense);
		tableMatrix = new MatrixDataTable();
		base.Tables.Add(tableMatrix);
		tableMIPObject = new MIPObjectDataTable();
		base.Tables.Add(tableMIPObject);
		tableNotificationProfiles = new NotificationProfilesDataTable();
		base.Tables.Add(tableNotificationProfiles);
		tableNotificationSettings = new NotificationSettingsDataTable();
		base.Tables.Add(tableNotificationSettings);
		tableNotificationTypes = new NotificationTypesDataTable();
		base.Tables.Add(tableNotificationTypes);
		tablePatrollingProfiles = new PatrollingProfilesDataTable();
		base.Tables.Add(tablePatrollingProfiles);
		tableRecorderInfo = new RecorderInfoDataTable();
		base.Tables.Add(tableRecorderInfo);
		tableRecorderPluginDevice = new RecorderPluginDeviceDataTable();
		base.Tables.Add(tableRecorderPluginDevice);
		tableRecorderPlugins = new RecorderPluginsDataTable();
		base.Tables.Add(tableRecorderPlugins);
		tableRecorders = new RecordersDataTable();
		base.Tables.Add(tableRecorders);
		tableRecordingServerServiceConfigurable = new RecordingServerServiceConfigurableDataTable();
		base.Tables.Add(tableRecordingServerServiceConfigurable);
		tableRecordingServerServiceRecorder = new RecordingServerServiceRecorderDataTable();
		base.Tables.Add(tableRecordingServerServiceRecorder);
		tableRecordingServerService = new RecordingServerServiceDataTable();
		base.Tables.Add(tableRecordingServerService);
		tableRecordingStorageDevice = new RecordingStorageDeviceDataTable();
		base.Tables.Add(tableRecordingStorageDevice);
		tableRecordingStorage = new RecordingStorageDataTable();
		base.Tables.Add(tableRecordingStorage);
		tableRegisteredDeviceTypes = new RegisteredDeviceTypesDataTable();
		base.Tables.Add(tableRegisteredDeviceTypes);
		tableRegisteredServers = new RegisteredServersDataTable();
		base.Tables.Add(tableRegisteredServers);
		tableRules = new RulesDataTable();
		base.Tables.Add(tableRules);
		tableRuleVariables = new RuleVariablesDataTable();
		base.Tables.Add(tableRuleVariables);
		tableServerGroupMembership = new ServerGroupMembershipDataTable();
		base.Tables.Add(tableServerGroupMembership);
		tableServerGroup = new ServerGroupDataTable();
		base.Tables.Add(tableServerGroup);
		tableSystemSettings = new SystemSettingsDataTable();
		base.Tables.Add(tableSystemSettings);
		tableTimeProfile = new TimeProfileDataTable();
		base.Tables.Add(tableTimeProfile);
		ForeignKeyConstraint foreignKeyConstraint = new ForeignKeyConstraint("FK_ArchiveStorage_RecordingStorage", new DataColumn[1] { tableRecordingStorage.pkidColumn }, new DataColumn[1] { tableArchiveStorage.recordingstorage_idColumn });
		tableArchiveStorage.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DeviceDataDistribution_Recorders", new DataColumn[1] { tableRecorders.IDRecorderColumn }, new DataColumn[1] { tableDeviceDataDistribution.recorder_idColumn });
		tableDeviceDataDistribution.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DeviceDataDistribution_DeviceTrack", new DataColumn[1] { tableDeviceTrack.IDTrackColumn }, new DataColumn[1] { tableDeviceDataDistribution.track_idColumn });
		tableDeviceDataDistribution.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DeviceDataDistribution_Devices", new DataColumn[1] { tableDevices.IDDeviceColumn }, new DataColumn[1] { tableDeviceDataDistribution.device_idColumn });
		tableDeviceDataDistribution.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DeviceGroupMembers_Devices", new DataColumn[1] { tableDevices.IDDeviceColumn }, new DataColumn[1] { tableDeviceGroupMembers.IDDeviceColumn });
		tableDeviceGroupMembers.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DeviceGroupMembers_DeviceGroups", new DataColumn[1] { tableDeviceGroups.IDDeviceGroupColumn }, new DataColumn[1] { tableDeviceGroupMembers.IDDeviceGroupColumn });
		tableDeviceGroupMembers.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DeviceGroups_ParentDeviceGroup", new DataColumn[1] { tableDeviceGroups.IDDeviceGroupColumn }, new DataColumn[1] { tableDeviceGroups.IDParentDeviceGroupColumn });
		tableDeviceGroups.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DeviceLinks_Devices_Linked", new DataColumn[1] { tableDevices.IDDeviceColumn }, new DataColumn[1] { tableDeviceLinks.IDDeviceLinkedColumn });
		tableDeviceLinks.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DeviceLinks_Devices", new DataColumn[1] { tableDevices.IDDeviceColumn }, new DataColumn[1] { tableDeviceLinks.IDDeviceColumn });
		tableDeviceLinks.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DevicePrivacyMask_Devices", new DataColumn[1] { tableDevices.IDDeviceColumn }, new DataColumn[1] { tableDevicePrivacyMask.IDDeviceColumn });
		tableDevicePrivacyMask.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_Devices_RegisteredDeviceTypes", new DataColumn[1] { tableRegisteredDeviceTypes.DeviceTypeColumn }, new DataColumn[1] { tableDevices.DeviceTypeColumn });
		tableDevices.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_Devices_Hardware", new DataColumn[1] { tableHardware.IDHardwareColumn }, new DataColumn[1] { tableDevices.IDHardwareColumn });
		tableDevices.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DeviceTrack_TrackDefinition", new DataColumn[1] { tableDeviceTrackDefinition.IDTrackDefinitionColumn }, new DataColumn[1] { tableDeviceTrack.IDTrackDefinitionColumn });
		tableDeviceTrack.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_DeviceTrack_Devices", new DataColumn[1] { tableDevices.IDDeviceColumn }, new DataColumn[1] { tableDeviceTrack.IDDeviceColumn });
		tableDeviceTrack.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_HardwareBlock_Hardware", new DataColumn[1] { tableHardware.IDHardwareColumn }, new DataColumn[1] { tableHardwareBlock.hardware_idColumn });
		tableHardwareBlock.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_HardwareDriverRecorder_RecordingServerService", new DataColumn[1] { tableRecordingServerService.pkidColumn }, new DataColumn[1] { tableHardwareDriverRecorder.IDRecordingServerServiceColumn });
		tableHardwareDriverRecorder.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_HardwareDefinition_Hardware", new DataColumn[1] { tableHardwareDefinition.C18NDigestColumn }, new DataColumn[1] { tableHardware.C18NDigestColumn });
		tableHardware.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_Hardware_Recorders", new DataColumn[1] { tableRecorders.IDRecorderColumn }, new DataColumn[1] { tableHardware.IDRecorderColumn });
		tableHardware.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_Hardware_HostedHardware", new DataColumn[1] { tableHostedHardware.pkidColumn }, new DataColumn[1] { tableHardware.HostedHardware_idColumn });
		tableHardware.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_HostedHardware_HostingServer", new DataColumn[1] { tableHostingServer.pkidColumn }, new DataColumn[1] { tableHostedHardware.hosting_server_idColumn });
		tableHostedHardware.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_PatrollingProfiles_Devices", new DataColumn[1] { tableDevices.IDDeviceColumn }, new DataColumn[1] { tablePatrollingProfiles.IDDeviceColumn });
		tablePatrollingProfiles.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_RecorderPlugins_RecorderPluginDevice", new DataColumn[1] { tableRecorderPlugins.IDRecorderPluginColumn }, new DataColumn[1] { tableRecorderPluginDevice.IDRecorderPluginColumn });
		tableRecorderPluginDevice.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_RecorderPluginDevice_Devices", new DataColumn[1] { tableDevices.IDDeviceColumn }, new DataColumn[1] { tableRecorderPluginDevice.IDDeviceColumn });
		tableRecorderPluginDevice.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_RecordingServerServiceConfigurable_RecordingServerService", new DataColumn[1] { tableRecordingServerService.pkidColumn }, new DataColumn[1] { tableRecordingServerServiceConfigurable.PKIDColumn });
		tableRecordingServerServiceConfigurable.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_RecordingServerServiceRecorder_RecordingServerService", new DataColumn[1] { tableRecordingServerService.pkidColumn }, new DataColumn[1] { tableRecordingServerServiceRecorder.recording_server_service_idColumn });
		tableRecordingServerServiceRecorder.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_RecordingServerServiceRecorder_Recorder", new DataColumn[1] { tableRecorders.IDRecorderColumn }, new DataColumn[1] { tableRecordingServerServiceRecorder.recorder_idColumn });
		tableRecordingServerServiceRecorder.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_RecordingStorageDevice_RecordingStorage", new DataColumn[1] { tableRecordingStorage.pkidColumn }, new DataColumn[1] { tableRecordingStorageDevice.recordingstorage_idColumn });
		tableRecordingStorageDevice.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_RecordingStorageDevice_Devices", new DataColumn[1] { tableDevices.IDDeviceColumn }, new DataColumn[1] { tableRecordingStorageDevice.device_idColumn });
		tableRecordingStorageDevice.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_RecordingStorage_Recorders", new DataColumn[1] { tableRecorders.IDRecorderColumn }, new DataColumn[1] { tableRecordingStorage.recordingserver_idColumn });
		tableRecordingStorage.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_ServerGroupMembership_ServerGroup", new DataColumn[1] { tableServerGroup.IDServerGroupColumn }, new DataColumn[1] { tableServerGroupMembership.IDServerGroupColumn });
		tableServerGroupMembership.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = System.Data.Rule.None;
		foreignKeyConstraint.UpdateRule = System.Data.Rule.None;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeArchiveStorage()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeAudioMessage()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeCompressors()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDeviceDataDistribution()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDeviceGroupMembers()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDeviceGroups()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDeviceLinks()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDevicePrivacyMask()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDeviceRestrictedMedia()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDevices()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDeviceTrackDefinition()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDeviceTrackEvidenceLock()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDeviceTrackInfo()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDeviceTrack()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDriverNumbers()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeEventTypes()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeExternalPlugins()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeFailoverInfo()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeHardwareBlock()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeHardwareDefinition()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeHardwareDriverRecorder()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeHardware()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeHostedHardware()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeHostingServer()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeLicense()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeMatrix()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeMIPObject()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeNotificationProfiles()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeNotificationSettings()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeNotificationTypes()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializePatrollingProfiles()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRecorderInfo()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRecorderPluginDevice()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRecorderPlugins()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRecorders()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRecordingServerServiceConfigurable()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRecordingServerServiceRecorder()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRecordingServerService()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRecordingStorageDevice()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRecordingStorage()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRegisteredDeviceTypes()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRegisteredServers()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRules()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeRuleVariables()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeServerGroupMembership()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeServerGroup()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeSystemSettings()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeTimeProfile()
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
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
		XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
		XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
		xmlSchemaAny.Namespace = configurationDataSet.Namespace;
		xmlSchemaSequence.Items.Add(xmlSchemaAny);
		xmlSchemaComplexType.Particle = xmlSchemaSequence;
		XmlSchema schemaSerializable = configurationDataSet.GetSchemaSerializable();
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
