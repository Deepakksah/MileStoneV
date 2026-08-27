#define TRACE
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Server.Licensing;

[Serializable]
[DesignerCategory("code")]
[ToolboxItem(true)]
[XmlSchemaProvider("GetTypedDataSetSchema")]
[XmlRoot("LicenseDataSet")]
[HelpKeyword("vs.data.DataSet")]
public class LicenseDataSet : DataSet
{
	public class HardwareRow : DataRow
	{
		private HardwareDataTable tableHardware;

		public bool IsInterconnectedProduct => ((Subtypes)SubType).HasFlag(Subtypes.InterconnectedProduct);

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDHardware
		{
			get
			{
				return (Guid)base[tableHardware.IDHardwareColumn];
			}
			set
			{
				base[tableHardware.IDHardwareColumn] = value;
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
		public Guid IDRecorder
		{
			get
			{
				return (Guid)base[tableHardware.IDRecorderColumn];
			}
			set
			{
				base[tableHardware.IDRecorderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string DriverName
		{
			get
			{
				try
				{
					return (string)base[tableHardware.DriverNameColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'DriverName' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.DriverNameColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string DriverVersion
		{
			get
			{
				try
				{
					return (string)base[tableHardware.DriverVersionColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'DriverVersion' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.DriverVersionColumn] = value;
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
		public string MacAddress
		{
			get
			{
				try
				{
					return (string)base[tableHardware.MacAddressColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'MacAddress' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.MacAddressColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string SerialNumber
		{
			get
			{
				try
				{
					return (string)base[tableHardware.SerialNumberColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'SerialNumber' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.SerialNumberColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int SubType
		{
			get
			{
				try
				{
					return (int)base[tableHardware.SubTypeColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'SubType' in table 'Hardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableHardware.SubTypeColumn] = value;
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
		public string DetectedModel
		{
			get
			{
				if (IsDetectedModelNull())
				{
					return null;
				}
				return (string)base[tableHardware.DetectedModelColumn];
			}
			set
			{
				base[tableHardware.DetectedModelColumn] = value;
			}
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
		public bool IsDriverNameNull()
		{
			return IsNull(tableHardware.DriverNameColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetDriverNameNull()
		{
			base[tableHardware.DriverNameColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsDriverVersionNull()
		{
			return IsNull(tableHardware.DriverVersionColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetDriverVersionNull()
		{
			base[tableHardware.DriverVersionColumn] = Convert.DBNull;
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
		public bool IsMacAddressNull()
		{
			return IsNull(tableHardware.MacAddressColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetMacAddressNull()
		{
			base[tableHardware.MacAddressColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsSerialNumberNull()
		{
			return IsNull(tableHardware.SerialNumberColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetSerialNumberNull()
		{
			base[tableHardware.SerialNumberColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsSubTypeNull()
		{
			return IsNull(tableHardware.SubTypeColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetSubTypeNull()
		{
			base[tableHardware.SubTypeColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsDetectedModelNull()
		{
			return IsNull(tableHardware.DetectedModelColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetDetectedModelNull()
		{
			base[tableHardware.DetectedModelColumn] = Convert.DBNull;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class HardwareDataTable : TypedTableBase<HardwareRow>
	{
		private class LocalIndexes : LinkedListWithIndexesBase<HardwareRow>
		{
			private readonly LinkedListIndex<Guid, HardwareRow> indexRecorderId;

			public LocalIndexes()
			{
				indexRecorderId = new LinkedListIndex<Guid, HardwareRow>((HardwareRow d) => d.IDRecorder);
			}

			public IEnumerable<HardwareRow> QueryByRecorderId(Guid recorderId)
			{
				return from e in indexRecorderId.GetItemsByKey(recorderId)
					select e.Value;
			}

			protected override LinkedListNode<HardwareRow> FindEntry(HardwareRow item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return indexRecorderId.GetItemsByKey(item.IDRecorder).FirstOrDefault((LinkedListNode<HardwareRow> h) => h.Value.IDHardware == item.IDHardware);
			}

			protected override void AddEntryToIndexes(LinkedListNode<HardwareRow> entry)
			{
				if (entry == null)
				{
					throw new ArgumentNullException("entry");
				}
				indexRecorderId.AddItem(entry);
			}

			protected override void RemoveEntryFromIndexes(LinkedListNode<HardwareRow> entry)
			{
				indexRecorderId.RemoveItem(entry);
			}
		}

		private volatile LocalIndexes idx;

		private DataColumn columnIDHardware;

		private DataColumn columnEnabled;

		private DataColumn columnName;

		private DataColumn columnIDRecorder;

		private DataColumn columnDriverName;

		private DataColumn columnDriverVersion;

		private DataColumn columnVendorProductID;

		private DataColumn columnMacAddress;

		private DataColumn columnSerialNumber;

		private DataColumn columnSubType;

		private DataColumn columnDriverNumber;

		private DataColumn columnDetectedModel;

		private LocalIndexes Indexes
		{
			get
			{
				if (idx == null)
				{
					Trace.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture) + " Building hardware index");
					LocalIndexes localIndexes = new LocalIndexes();
					using (IEnumerator<HardwareRow> enumerator = GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							HardwareRow current = enumerator.Current;
							localIndexes.Add(current);
						}
					}
					HardwareRowChanged += HardwareDataTable_HardwareRowChanged;
					base.RowDeleting += HardwareDataTable_RowDeleting;
					base.TableClearing += HardwareDataTable_TableClearing;
					idx = localIndexes;
					Trace.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture) + " Building hardware index DONE");
				}
				return idx;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDHardwareColumn => columnIDHardware;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn NameColumn => columnName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderColumn => columnIDRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DriverNameColumn => columnDriverName;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DriverVersionColumn => columnDriverVersion;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn VendorProductIDColumn => columnVendorProductID;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn MacAddressColumn => columnMacAddress;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SerialNumberColumn => columnSerialNumber;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn SubTypeColumn => columnSubType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DriverNumberColumn => columnDriverNumber;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DetectedModelColumn => columnDetectedModel;

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

		private void HardwareDataTable_HardwareRowChanged(object sender, HardwareRowChangeEvent e)
		{
			if (e.Action == DataRowAction.Add)
			{
				Indexes.Add(e.Row);
			}
		}

		private void HardwareDataTable_RowDeleting(object sender, DataRowChangeEventArgs e)
		{
			Indexes.Remove(e.Row as HardwareRow);
		}

		private void HardwareDataTable_TableClearing(object sender, DataTableClearEventArgs e)
		{
			idx = new LocalIndexes();
		}

		public HardwareRow[] FindByIDRecorder(Guid recorderId)
		{
			return Indexes.QueryByRecorderId(recorderId).ToArray();
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
		public HardwareRow AddHardwareRow(Guid IDHardware, bool Enabled, string Name, Guid IDRecorder, string DriverName, string DriverVersion, string VendorProductID, string MacAddress, string SerialNumber, int SubType, int DriverNumber, string DetectedModel)
		{
			HardwareRow hardwareRow = (HardwareRow)NewRow();
			object[] itemArray = new object[12]
			{
				IDHardware, Enabled, Name, IDRecorder, DriverName, DriverVersion, VendorProductID, MacAddress, SerialNumber, SubType,
				DriverNumber, DetectedModel
			};
			hardwareRow.ItemArray = itemArray;
			base.Rows.Add(hardwareRow);
			return hardwareRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public HardwareRow FindByIDHardware(Guid IDHardware)
		{
			return (HardwareRow)base.Rows.Find(new object[1] { IDHardware });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			HardwareDataTable hardwareDataTable = (HardwareDataTable)base.Clone();
			hardwareDataTable.InitVars();
			return hardwareDataTable;
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
			columnIDHardware = base.Columns["IDHardware"];
			columnEnabled = base.Columns["Enabled"];
			columnName = base.Columns["Name"];
			columnIDRecorder = base.Columns["IDRecorder"];
			columnDriverName = base.Columns["DriverName"];
			columnDriverVersion = base.Columns["DriverVersion"];
			columnVendorProductID = base.Columns["VendorProductID"];
			columnMacAddress = base.Columns["MacAddress"];
			columnSerialNumber = base.Columns["SerialNumber"];
			columnSubType = base.Columns["SubType"];
			columnDriverNumber = base.Columns["DriverNumber"];
			columnDetectedModel = base.Columns["DetectedModel"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDHardware = new DataColumn("IDHardware", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDHardware);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			columnName = new DataColumn("Name", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnName);
			columnIDRecorder = new DataColumn("IDRecorder", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDRecorder);
			columnDriverName = new DataColumn("DriverName", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDriverName);
			columnDriverVersion = new DataColumn("DriverVersion", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDriverVersion);
			columnVendorProductID = new DataColumn("VendorProductID", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnVendorProductID);
			columnMacAddress = new DataColumn("MacAddress", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnMacAddress);
			columnSerialNumber = new DataColumn("SerialNumber", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnSerialNumber);
			columnSubType = new DataColumn("SubType", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnSubType);
			columnDriverNumber = new DataColumn("DriverNumber", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnDriverNumber);
			columnDetectedModel = new DataColumn("DetectedModel", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDetectedModel);
			base.Constraints.Add(new UniqueConstraint("Constraint1", new DataColumn[1] { columnIDHardware }, isPrimaryKey: true));
			columnIDHardware.AllowDBNull = false;
			columnIDHardware.Unique = true;
			columnEnabled.AllowDBNull = false;
			columnName.AllowDBNull = false;
			columnName.MaxLength = 256;
			columnIDRecorder.AllowDBNull = false;
			columnVendorProductID.MaxLength = 256;
			columnDriverNumber.AllowDBNull = false;
			columnDetectedModel.ReadOnly = true;
			columnDetectedModel.MaxLength = 256;
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
			LicenseDataSet licenseDataSet = new LicenseDataSet();
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
			xmlSchemaAttribute.FixedValue = licenseDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "HardwareDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = licenseDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					XmlSchema xmlSchema = null;
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						xmlSchema = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						xmlSchema.Write(memoryStream2);
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
		private class LocalIndexes : LinkedListWithIndexesBase<DevicesRow>
		{
			private readonly LinkedListIndex<Guid, DevicesRow> indexHardwareId;

			public LocalIndexes()
			{
				indexHardwareId = new LinkedListIndex<Guid, DevicesRow>((DevicesRow d) => d.IDHardware);
			}

			public IEnumerable<DevicesRow> QueryByHardwareId(Guid hardwareId)
			{
				return from e in indexHardwareId.GetItemsByKey(hardwareId)
					select e.Value;
			}

			protected override LinkedListNode<DevicesRow> FindEntry(DevicesRow item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				return indexHardwareId.GetItemsByKey(item.IDHardware).FirstOrDefault((LinkedListNode<DevicesRow> d) => d.Value.IDDevice == item.IDDevice);
			}

			protected override void AddEntryToIndexes(LinkedListNode<DevicesRow> entry)
			{
				if (entry == null)
				{
					throw new ArgumentNullException("entry");
				}
				indexHardwareId.AddItem(entry);
			}

			protected override void RemoveEntryFromIndexes(LinkedListNode<DevicesRow> entry)
			{
				indexHardwareId.RemoveItem(entry);
			}
		}

		private volatile LocalIndexes idx;

		private DataColumn columnIDDevice;

		private DataColumn columnIDHardware;

		private DataColumn columnEnabled;

		private DataColumn columnDeviceType;

		private DataColumn columnDeviceIndex;

		private DataColumn columnOriginalIdDevice;

		private LocalIndexes Indexes
		{
			get
			{
				if (idx == null)
				{
					Trace.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture) + " Building device index");
					LocalIndexes localIndexes = new LocalIndexes();
					using (IEnumerator<DevicesRow> enumerator = GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DevicesRow current = enumerator.Current;
							localIndexes.Add(current);
						}
					}
					DevicesRowChanged += DevicesDataTable_DevicesRowChanged;
					base.RowDeleting += DevicesDataTable_RowDeleting;
					base.TableClearing += DevicesDataTable_TableClearing;
					idx = localIndexes;
					Trace.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture) + " Building hardware index DONE");
				}
				return idx;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDHardwareColumn => columnIDHardware;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DeviceTypeColumn => columnDeviceType;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DeviceIndexColumn => columnDeviceIndex;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn OriginalIdDeviceColumn => columnOriginalIdDevice;

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

		private void DevicesDataTable_DevicesRowChanged(object sender, DevicesRowChangeEvent e)
		{
			if (e.Action == DataRowAction.Add)
			{
				Indexes.Add(e.Row);
			}
		}

		private void DevicesDataTable_RowDeleting(object sender, DataRowChangeEventArgs e)
		{
			Indexes.Remove(e.Row as DevicesRow);
		}

		private void DevicesDataTable_TableClearing(object sender, DataTableClearEventArgs e)
		{
			idx = new LocalIndexes();
		}

		public DevicesRow[] FindByIDHardware(Guid hardwareId)
		{
			return Indexes.QueryByHardwareId(hardwareId).ToArray();
		}

		public DevicesRow[] FindByIDHardwareAndDeviceType(Guid hardwareId, string deviceType)
		{
			return (from d in Indexes.QueryByHardwareId(hardwareId)
				where string.Equals(d.DeviceType, deviceType, StringComparison.OrdinalIgnoreCase)
				select d).ToArray();
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
		public DevicesRow AddDevicesRow(Guid IDDevice, Guid IDHardware, bool Enabled, string DeviceType, int DeviceIndex, Guid OriginalIdDevice)
		{
			DevicesRow devicesRow = (DevicesRow)NewRow();
			object[] itemArray = new object[6] { IDDevice, IDHardware, Enabled, DeviceType, DeviceIndex, OriginalIdDevice };
			devicesRow.ItemArray = itemArray;
			base.Rows.Add(devicesRow);
			return devicesRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DevicesRow FindByIDDevice(Guid IDDevice)
		{
			return (DevicesRow)base.Rows.Find(new object[1] { IDDevice });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			DevicesDataTable devicesDataTable = (DevicesDataTable)base.Clone();
			devicesDataTable.InitVars();
			return devicesDataTable;
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
			columnIDDevice = base.Columns["IDDevice"];
			columnIDHardware = base.Columns["IDHardware"];
			columnEnabled = base.Columns["Enabled"];
			columnDeviceType = base.Columns["DeviceType"];
			columnDeviceIndex = base.Columns["DeviceIndex"];
			columnOriginalIdDevice = base.Columns["OriginalIdDevice"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDDevice = new DataColumn("IDDevice", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnIDHardware = new DataColumn("IDHardware", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDHardware);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			columnDeviceType = new DataColumn("DeviceType", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDeviceType);
			columnDeviceIndex = new DataColumn("DeviceIndex", typeof(int), null, MappingType.Element);
			base.Columns.Add(columnDeviceIndex);
			columnOriginalIdDevice = new DataColumn("OriginalIdDevice", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnOriginalIdDevice);
			base.Constraints.Add(new UniqueConstraint("Constraint1", new DataColumn[1] { columnIDDevice }, isPrimaryKey: true));
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.Unique = true;
			columnEnabled.AllowDBNull = false;
			columnDeviceType.AllowDBNull = false;
			columnDeviceType.MaxLength = 100;
			columnDeviceIndex.AllowDBNull = false;
			columnOriginalIdDevice.ReadOnly = true;
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
			LicenseDataSet licenseDataSet = new LicenseDataSet();
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
			xmlSchemaAttribute.FixedValue = licenseDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "DevicesDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = licenseDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					XmlSchema xmlSchema = null;
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						xmlSchema = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						xmlSchema.Write(memoryStream2);
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
	public class LicenseRecorderDataTable : TypedTableBase<LicenseRecorderRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderColumn => columnIDRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRecorderRow this[int index] => (LicenseRecorderRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseRecorderRowChangeEventHandler LicenseRecorderRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseRecorderRowChangeEventHandler LicenseRecorderRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseRecorderRowChangeEventHandler LicenseRecorderRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseRecorderRowChangeEventHandler LicenseRecorderRowDeleted;

		public bool IsRegistered(Guid IDRecorder)
		{
			LicenseRecorderRow licenseRecorderRow = FindByIDRecorder(IDRecorder);
			return licenseRecorderRow != null;
		}

		public void Register(Guid IDRecorder)
		{
			LicenseRecorderRow licenseRecorderRow = FindByIDRecorder(IDRecorder);
			if (licenseRecorderRow == null)
			{
				AddLicenseRecorderRow(DateTime.UtcNow, IDRecorder);
			}
		}

		public void Unregister(Guid IDRecorder)
		{
			FindByIDRecorder(IDRecorder)?.Delete();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRecorderDataTable()
		{
			base.TableName = "LicenseRecorder";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRecorderDataTable(DataTable table)
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
		protected LicenseRecorderDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddLicenseRecorderRow(LicenseRecorderRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRecorderRow AddLicenseRecorderRow(DateTime LastModified, Guid IDRecorder)
		{
			LicenseRecorderRow licenseRecorderRow = (LicenseRecorderRow)NewRow();
			object[] itemArray = new object[2] { LastModified, IDRecorder };
			licenseRecorderRow.ItemArray = itemArray;
			base.Rows.Add(licenseRecorderRow);
			return licenseRecorderRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRecorderRow FindByIDRecorder(Guid IDRecorder)
		{
			return (LicenseRecorderRow)base.Rows.Find(new object[1] { IDRecorder });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			LicenseRecorderDataTable licenseRecorderDataTable = (LicenseRecorderDataTable)base.Clone();
			licenseRecorderDataTable.InitVars();
			return licenseRecorderDataTable;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new LicenseRecorderDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDRecorder = base.Columns["IDRecorder"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDRecorder = new DataColumn("IDRecorder", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDRecorder);
			base.Constraints.Add(new UniqueConstraint("Constraint1", new DataColumn[1] { columnIDRecorder }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDRecorder.AllowDBNull = false;
			columnIDRecorder.Unique = true;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRecorderRow NewLicenseRecorderRow()
		{
			return (LicenseRecorderRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new LicenseRecorderRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(LicenseRecorderRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.LicenseRecorderRowChanged != null)
			{
				this.LicenseRecorderRowChanged(this, new LicenseRecorderRowChangeEvent((LicenseRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.LicenseRecorderRowChanging != null)
			{
				this.LicenseRecorderRowChanging(this, new LicenseRecorderRowChangeEvent((LicenseRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.LicenseRecorderRowDeleted != null)
			{
				this.LicenseRecorderRowDeleted(this, new LicenseRecorderRowChangeEvent((LicenseRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.LicenseRecorderRowDeleting != null)
			{
				this.LicenseRecorderRowDeleting(this, new LicenseRecorderRowChangeEvent((LicenseRecorderRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveLicenseRecorderRow(LicenseRecorderRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			LicenseDataSet licenseDataSet = new LicenseDataSet();
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
			xmlSchemaAttribute.FixedValue = licenseDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "LicenseRecorderDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = licenseDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					XmlSchema xmlSchema = null;
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						xmlSchema = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						xmlSchema.Write(memoryStream2);
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

	public class LicenseHardwareRow : DataRow
	{
		private LicenseHardwareDataTable tableLicenseHardware;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableLicenseHardware.LastModifiedColumn];
			}
			set
			{
				base[tableLicenseHardware.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDHardware
		{
			get
			{
				return (Guid)base[tableLicenseHardware.IDHardwareColumn];
			}
			set
			{
				base[tableLicenseHardware.IDHardwareColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public long License
		{
			get
			{
				return (long)base[tableLicenseHardware.LicenseColumn];
			}
			set
			{
				base[tableLicenseHardware.LicenseColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDRecorder
		{
			get
			{
				return (Guid)base[tableLicenseHardware.IDRecorderColumn];
			}
			set
			{
				base[tableLicenseHardware.IDRecorderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string UniqueId
		{
			get
			{
				try
				{
					return (string)base[tableLicenseHardware.UniqueIdColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'UniqueId' in table 'LicenseHardware' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableLicenseHardware.UniqueIdColumn] = value;
			}
		}

		public string GetUniqueId()
		{
			return this.GetColumnValue<string>(tableLicenseHardware.UniqueIdColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseHardwareRow(DataRowBuilder rb)
			: base(rb)
		{
			tableLicenseHardware = (LicenseHardwareDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsUniqueIdNull()
		{
			return IsNull(tableLicenseHardware.UniqueIdColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetUniqueIdNull()
		{
			base[tableLicenseHardware.UniqueIdColumn] = Convert.DBNull;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class LicenseHardwareDataTable : TypedTableBase<LicenseHardwareRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDHardware;

		private DataColumn columnLicense;

		private DataColumn columnIDRecorder;

		private DataColumn columnUniqueId;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDHardwareColumn => columnIDHardware;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LicenseColumn => columnLicense;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderColumn => columnIDRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn UniqueIdColumn => columnUniqueId;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseHardwareRow this[int index] => (LicenseHardwareRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseHardwareRowChangeEventHandler LicenseHardwareRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseHardwareRowChangeEventHandler LicenseHardwareRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseHardwareRowChangeEventHandler LicenseHardwareRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseHardwareRowChangeEventHandler LicenseHardwareRowDeleted;

		public void SetPersistedValue(Guid IDHardware, long value, string uniqueId)
		{
			LicenseHardwareRow licenseHardwareRow = this.Where((LicenseHardwareRow lh) => lh.GetUniqueId().Equals(uniqueId)).FirstOrDefault();
			if (value == 0L)
			{
				licenseHardwareRow?.Delete();
				return;
			}
			if (licenseHardwareRow != null && licenseHardwareRow.RowState == DataRowState.Deleted)
			{
				licenseHardwareRow.RejectChanges();
				if (licenseHardwareRow.RowState == DataRowState.Detached)
				{
					licenseHardwareRow = null;
				}
			}
			if (licenseHardwareRow == null)
			{
				AddLicenseHardwareRow(DateTime.UtcNow, IDHardware, value, Guid.Empty, uniqueId);
			}
			else if (licenseHardwareRow.License != value)
			{
				licenseHardwareRow.License = value;
				licenseHardwareRow.LastModified = DateTime.UtcNow;
				licenseHardwareRow.UniqueId = uniqueId;
			}
		}

		public long GetPersistedValue(Guid IDHardware)
		{
			return FindByIDHardware(IDHardware)?.License ?? 0;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseHardwareDataTable()
		{
			base.TableName = "LicenseHardware";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseHardwareDataTable(DataTable table)
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
		protected LicenseHardwareDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddLicenseHardwareRow(LicenseHardwareRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseHardwareRow AddLicenseHardwareRow(DateTime LastModified, Guid IDHardware, long License, Guid IDRecorder, string UniqueId)
		{
			LicenseHardwareRow licenseHardwareRow = (LicenseHardwareRow)NewRow();
			object[] itemArray = new object[5] { LastModified, IDHardware, License, IDRecorder, UniqueId };
			licenseHardwareRow.ItemArray = itemArray;
			base.Rows.Add(licenseHardwareRow);
			return licenseHardwareRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseHardwareRow FindByIDHardware(Guid IDHardware)
		{
			return (LicenseHardwareRow)base.Rows.Find(new object[1] { IDHardware });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			LicenseHardwareDataTable licenseHardwareDataTable = (LicenseHardwareDataTable)base.Clone();
			licenseHardwareDataTable.InitVars();
			return licenseHardwareDataTable;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new LicenseHardwareDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDHardware = base.Columns["IDHardware"];
			columnLicense = base.Columns["License"];
			columnIDRecorder = base.Columns["IDRecorder"];
			columnUniqueId = base.Columns["UniqueId"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDHardware = new DataColumn("IDHardware", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDHardware);
			columnLicense = new DataColumn("License", typeof(long), null, MappingType.Element);
			base.Columns.Add(columnLicense);
			columnIDRecorder = new DataColumn("IDRecorder", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDRecorder);
			columnUniqueId = new DataColumn("UniqueId", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnUniqueId);
			base.Constraints.Add(new UniqueConstraint("Constraint1", new DataColumn[1] { columnIDHardware }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDHardware.AllowDBNull = false;
			columnIDHardware.Unique = true;
			columnLicense.AllowDBNull = false;
			columnIDRecorder.AllowDBNull = false;
			columnIDRecorder.ReadOnly = true;
			columnUniqueId.MaxLength = 700;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseHardwareRow NewLicenseHardwareRow()
		{
			return (LicenseHardwareRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new LicenseHardwareRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(LicenseHardwareRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.LicenseHardwareRowChanged != null)
			{
				this.LicenseHardwareRowChanged(this, new LicenseHardwareRowChangeEvent((LicenseHardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.LicenseHardwareRowChanging != null)
			{
				this.LicenseHardwareRowChanging(this, new LicenseHardwareRowChangeEvent((LicenseHardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.LicenseHardwareRowDeleted != null)
			{
				this.LicenseHardwareRowDeleted(this, new LicenseHardwareRowChangeEvent((LicenseHardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.LicenseHardwareRowDeleting != null)
			{
				this.LicenseHardwareRowDeleting(this, new LicenseHardwareRowChangeEvent((LicenseHardwareRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveLicenseHardwareRow(LicenseHardwareRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			LicenseDataSet licenseDataSet = new LicenseDataSet();
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
			xmlSchemaAttribute.FixedValue = licenseDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "LicenseHardwareDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = licenseDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					XmlSchema xmlSchema = null;
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						xmlSchema = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						xmlSchema.Write(memoryStream2);
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

	public class LicenseDeviceRow : DataRow
	{
		private LicenseDeviceDataTable tableLicenseDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableLicenseDevice.LastModifiedColumn];
			}
			set
			{
				base[tableLicenseDevice.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDDevice
		{
			get
			{
				return (Guid)base[tableLicenseDevice.IDDeviceColumn];
			}
			set
			{
				base[tableLicenseDevice.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public long License
		{
			get
			{
				return (long)base[tableLicenseDevice.LicenseColumn];
			}
			set
			{
				base[tableLicenseDevice.LicenseColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDRecorder
		{
			get
			{
				return (Guid)base[tableLicenseDevice.IDRecorderColumn];
			}
			set
			{
				base[tableLicenseDevice.IDRecorderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public string UniqueId
		{
			get
			{
				try
				{
					return (string)base[tableLicenseDevice.UniqueIdColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'UniqueId' in table 'LicenseDevice' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableLicenseDevice.UniqueIdColumn] = value;
			}
		}

		public string GetUniqueId()
		{
			return this.GetColumnValue<string>(tableLicenseDevice.UniqueIdColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDeviceRow(DataRowBuilder rb)
			: base(rb)
		{
			tableLicenseDevice = (LicenseDeviceDataTable)base.Table;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsUniqueIdNull()
		{
			return IsNull(tableLicenseDevice.UniqueIdColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetUniqueIdNull()
		{
			base[tableLicenseDevice.UniqueIdColumn] = Convert.DBNull;
		}
	}

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class LicenseDeviceDataTable : TypedTableBase<LicenseDeviceRow>
	{
		private DataColumn columnLastModified;

		private DataColumn columnIDDevice;

		private DataColumn columnLicense;

		private DataColumn columnIDRecorder;

		private DataColumn columnUniqueId;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LastModifiedColumn => columnLastModified;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDDeviceColumn => columnIDDevice;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn LicenseColumn => columnLicense;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderColumn => columnIDRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn UniqueIdColumn => columnUniqueId;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		[Browsable(false)]
		public int Count => base.Rows.Count;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDeviceRow this[int index] => (LicenseDeviceRow)base.Rows[index];

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseDeviceRowChangeEventHandler LicenseDeviceRowChanging;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseDeviceRowChangeEventHandler LicenseDeviceRowChanged;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseDeviceRowChangeEventHandler LicenseDeviceRowDeleting;

		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public event LicenseDeviceRowChangeEventHandler LicenseDeviceRowDeleted;

		public void SetPersistedValue(Guid IDDevice, long value, string uniqueId)
		{
			LicenseDeviceRow licenseDeviceRow = this.Where((LicenseDeviceRow d) => d.GetUniqueId().Equals(uniqueId)).FirstOrDefault();
			if (value == 0L)
			{
				licenseDeviceRow?.Delete();
				return;
			}
			if (licenseDeviceRow != null && licenseDeviceRow.RowState == DataRowState.Deleted)
			{
				licenseDeviceRow.RejectChanges();
				if (licenseDeviceRow.RowState == DataRowState.Detached)
				{
					licenseDeviceRow = null;
				}
			}
			if (licenseDeviceRow == null)
			{
				AddLicenseDeviceRow(DateTime.UtcNow, IDDevice, value, Guid.Empty, uniqueId);
			}
			else if (licenseDeviceRow.License != value)
			{
				licenseDeviceRow.License = value;
				licenseDeviceRow.LastModified = DateTime.UtcNow;
				licenseDeviceRow.UniqueId = uniqueId;
			}
		}

		public long GetPersistedValue(Guid IDDevice)
		{
			return FindByIDDevice(IDDevice)?.License ?? 0;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDeviceDataTable()
		{
			base.TableName = "LicenseDevice";
			BeginInit();
			InitClass();
			EndInit();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDeviceDataTable(DataTable table)
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
		protected LicenseDeviceDataTable(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			InitVars();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void AddLicenseDeviceRow(LicenseDeviceRow row)
		{
			base.Rows.Add(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDeviceRow AddLicenseDeviceRow(DateTime LastModified, Guid IDDevice, long License, Guid IDRecorder, string UniqueId)
		{
			LicenseDeviceRow licenseDeviceRow = (LicenseDeviceRow)NewRow();
			object[] itemArray = new object[5] { LastModified, IDDevice, License, IDRecorder, UniqueId };
			licenseDeviceRow.ItemArray = itemArray;
			base.Rows.Add(licenseDeviceRow);
			return licenseDeviceRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDeviceRow FindByIDDevice(Guid IDDevice)
		{
			return (LicenseDeviceRow)base.Rows.Find(new object[1] { IDDevice });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			LicenseDeviceDataTable licenseDeviceDataTable = (LicenseDeviceDataTable)base.Clone();
			licenseDeviceDataTable.InitVars();
			return licenseDeviceDataTable;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataTable CreateInstance()
		{
			return new LicenseDeviceDataTable();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void InitVars()
		{
			columnLastModified = base.Columns["LastModified"];
			columnIDDevice = base.Columns["IDDevice"];
			columnLicense = base.Columns["License"];
			columnIDRecorder = base.Columns["IDRecorder"];
			columnUniqueId = base.Columns["UniqueId"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnLastModified = new DataColumn("LastModified", typeof(DateTime), null, MappingType.Element);
			base.Columns.Add(columnLastModified);
			columnIDDevice = new DataColumn("IDDevice", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDDevice);
			columnLicense = new DataColumn("License", typeof(long), null, MappingType.Element);
			base.Columns.Add(columnLicense);
			columnIDRecorder = new DataColumn("IDRecorder", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDRecorder);
			columnUniqueId = new DataColumn("UniqueId", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnUniqueId);
			base.Constraints.Add(new UniqueConstraint("Constraint1", new DataColumn[1] { columnIDDevice }, isPrimaryKey: true));
			columnLastModified.AllowDBNull = false;
			columnIDDevice.AllowDBNull = false;
			columnIDDevice.Unique = true;
			columnLicense.AllowDBNull = false;
			columnIDRecorder.AllowDBNull = false;
			columnIDRecorder.ReadOnly = true;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDeviceRow NewLicenseDeviceRow()
		{
			return (LicenseDeviceRow)NewRow();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new LicenseDeviceRow(builder);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override Type GetRowType()
		{
			return typeof(LicenseDeviceRow);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanged(DataRowChangeEventArgs e)
		{
			base.OnRowChanged(e);
			if (this.LicenseDeviceRowChanged != null)
			{
				this.LicenseDeviceRowChanged(this, new LicenseDeviceRowChangeEvent((LicenseDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowChanging(DataRowChangeEventArgs e)
		{
			base.OnRowChanging(e);
			if (this.LicenseDeviceRowChanging != null)
			{
				this.LicenseDeviceRowChanging(this, new LicenseDeviceRowChangeEvent((LicenseDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleted(DataRowChangeEventArgs e)
		{
			base.OnRowDeleted(e);
			if (this.LicenseDeviceRowDeleted != null)
			{
				this.LicenseDeviceRowDeleted(this, new LicenseDeviceRowChangeEvent((LicenseDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		protected override void OnRowDeleting(DataRowChangeEventArgs e)
		{
			base.OnRowDeleting(e);
			if (this.LicenseDeviceRowDeleting != null)
			{
				this.LicenseDeviceRowDeleting(this, new LicenseDeviceRowChangeEvent((LicenseDeviceRow)e.Row, e.Action));
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void RemoveLicenseDeviceRow(LicenseDeviceRow row)
		{
			base.Rows.Remove(row);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public static XmlSchemaComplexType GetTypedTableSchema(XmlSchemaSet xs)
		{
			XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
			XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
			LicenseDataSet licenseDataSet = new LicenseDataSet();
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
			xmlSchemaAttribute.FixedValue = licenseDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "LicenseDeviceDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = licenseDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					XmlSchema xmlSchema = null;
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						xmlSchema = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						xmlSchema.Write(memoryStream2);
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

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void DevicesRowChangeEventHandler(object sender, DevicesRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void HardwareRowChangeEventHandler(object sender, HardwareRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void LicenseDeviceRowChangeEventHandler(object sender, LicenseDeviceRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void LicenseHardwareRowChangeEventHandler(object sender, LicenseHardwareRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void LicenseRecorderRowChangeEventHandler(object sender, LicenseRecorderRowChangeEvent e);

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public delegate void RecordersRowChangeEventHandler(object sender, RecordersRowChangeEvent e);

	[Serializable]
	[XmlSchemaProvider("GetTypedTableSchema")]
	public class RecordersDataTable : TypedTableBase<RecordersRow>
	{
		private DataColumn columnIDRecorder;

		private DataColumn columnEnabled;

		private DataColumn columnDevicePackInfo;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn IDRecorderColumn => columnIDRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn EnabledColumn => columnEnabled;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataColumn DevicePackInfoColumn => columnDevicePackInfo;

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
		public RecordersRow AddRecordersRow(Guid IDRecorder, bool Enabled, string DevicePackInfo)
		{
			RecordersRow recordersRow = (RecordersRow)NewRow();
			object[] itemArray = new object[3] { IDRecorder, Enabled, DevicePackInfo };
			recordersRow.ItemArray = itemArray;
			base.Rows.Add(recordersRow);
			return recordersRow;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersRow FindByIDRecorder(Guid IDRecorder)
		{
			return (RecordersRow)base.Rows.Find(new object[1] { IDRecorder });
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public override DataTable Clone()
		{
			RecordersDataTable recordersDataTable = (RecordersDataTable)base.Clone();
			recordersDataTable.InitVars();
			return recordersDataTable;
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
			columnIDRecorder = base.Columns["IDRecorder"];
			columnEnabled = base.Columns["Enabled"];
			columnDevicePackInfo = base.Columns["DevicePackInfo"];
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private void InitClass()
		{
			columnIDRecorder = new DataColumn("IDRecorder", typeof(Guid), null, MappingType.Element);
			base.Columns.Add(columnIDRecorder);
			columnEnabled = new DataColumn("Enabled", typeof(bool), null, MappingType.Element);
			base.Columns.Add(columnEnabled);
			columnDevicePackInfo = new DataColumn("DevicePackInfo", typeof(string), null, MappingType.Element);
			base.Columns.Add(columnDevicePackInfo);
			base.Constraints.Add(new UniqueConstraint("Constraint1", new DataColumn[1] { columnIDRecorder }, isPrimaryKey: true));
			columnIDRecorder.AllowDBNull = false;
			columnIDRecorder.Unique = true;
			columnEnabled.AllowDBNull = false;
			columnDevicePackInfo.AllowDBNull = false;
			columnDevicePackInfo.MaxLength = 1073741823;
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
			LicenseDataSet licenseDataSet = new LicenseDataSet();
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
			xmlSchemaAttribute.FixedValue = licenseDataSet.Namespace;
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute);
			XmlSchemaAttribute xmlSchemaAttribute2 = new XmlSchemaAttribute();
			xmlSchemaAttribute2.Name = "tableTypeName";
			xmlSchemaAttribute2.FixedValue = "RecordersDataTable";
			xmlSchemaComplexType.Attributes.Add(xmlSchemaAttribute2);
			xmlSchemaComplexType.Particle = xmlSchemaSequence;
			XmlSchema schemaSerializable = licenseDataSet.GetSchemaSerializable();
			if (xs.Contains(schemaSerializable.TargetNamespace))
			{
				MemoryStream memoryStream = new MemoryStream();
				MemoryStream memoryStream2 = new MemoryStream();
				try
				{
					XmlSchema xmlSchema = null;
					schemaSerializable.Write(memoryStream);
					IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
					while (enumerator.MoveNext())
					{
						xmlSchema = (XmlSchema)enumerator.Current;
						memoryStream2.SetLength(0L);
						xmlSchema.Write(memoryStream2);
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

	public class DevicesRow : DataRow
	{
		private DevicesDataTable tableDevices;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDDevice
		{
			get
			{
				return (Guid)base[tableDevices.IDDeviceColumn];
			}
			set
			{
				base[tableDevices.IDDeviceColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDHardware
		{
			get
			{
				try
				{
					return (Guid)base[tableDevices.IDHardwareColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'IDHardware' in table 'Devices' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableDevices.IDHardwareColumn] = value;
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
		public Guid OriginalIdDevice
		{
			get
			{
				try
				{
					return (Guid)base[tableDevices.OriginalIdDeviceColumn];
				}
				catch (InvalidCastException innerException)
				{
					throw new StrongTypingException("The value for column 'OriginalIdDevice' in table 'Devices' is DBNull.", innerException);
				}
			}
			set
			{
				base[tableDevices.OriginalIdDeviceColumn] = value;
			}
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
		public bool IsIDHardwareNull()
		{
			return IsNull(tableDevices.IDHardwareColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetIDHardwareNull()
		{
			base[tableDevices.IDHardwareColumn] = Convert.DBNull;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public bool IsOriginalIdDeviceNull()
		{
			return IsNull(tableDevices.OriginalIdDeviceColumn);
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public void SetOriginalIdDeviceNull()
		{
			base[tableDevices.OriginalIdDeviceColumn] = Convert.DBNull;
		}
	}

	public class LicenseRecorderRow : DataRow
	{
		private LicenseRecorderDataTable tableLicenseRecorder;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DateTime LastModified
		{
			get
			{
				return (DateTime)base[tableLicenseRecorder.LastModifiedColumn];
			}
			set
			{
				base[tableLicenseRecorder.LastModifiedColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDRecorder
		{
			get
			{
				return (Guid)base[tableLicenseRecorder.IDRecorderColumn];
			}
			set
			{
				base[tableLicenseRecorder.IDRecorderColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRecorderRow(DataRowBuilder rb)
			: base(rb)
		{
			tableLicenseRecorder = (LicenseRecorderDataTable)base.Table;
		}
	}

	public class RecordersRow : DataRow
	{
		private RecordersDataTable tableRecorders;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public Guid IDRecorder
		{
			get
			{
				return (Guid)base[tableRecorders.IDRecorderColumn];
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
		public string DevicePackInfo
		{
			get
			{
				return (string)base[tableRecorders.DevicePackInfoColumn];
			}
			set
			{
				base[tableRecorders.DevicePackInfoColumn] = value;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public RecordersRow(DataRowBuilder rb)
			: base(rb)
		{
			tableRecorders = (RecordersDataTable)base.Table;
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
	public class LicenseDeviceRowChangeEvent : EventArgs
	{
		private LicenseDeviceRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDeviceRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseDeviceRowChangeEvent(LicenseDeviceRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class LicenseHardwareRowChangeEvent : EventArgs
	{
		private LicenseHardwareRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseHardwareRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseHardwareRowChangeEvent(LicenseHardwareRow row, DataRowAction action)
		{
			eventRow = row;
			eventAction = action;
		}
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public class LicenseRecorderRowChangeEvent : EventArgs
	{
		private LicenseRecorderRow eventRow;

		private DataRowAction eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRecorderRow Row => eventRow;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public DataRowAction Action => eventAction;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public LicenseRecorderRowChangeEvent(LicenseRecorderRow row, DataRowAction action)
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

	private DevicesDataTable tableDevices;

	private HardwareDataTable tableHardware;

	private LicenseDeviceDataTable tableLicenseDevice;

	private LicenseHardwareDataTable tableLicenseHardware;

	private LicenseRecorderDataTable tableLicenseRecorder;

	private RecordersDataTable tableRecorders;

	private SchemaSerializationMode _schemaSerializationMode = SchemaSerializationMode.IncludeSchema;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public DevicesDataTable Devices => tableDevices;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public HardwareDataTable Hardware => tableHardware;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public LicenseDeviceDataTable LicenseDevice => tableLicenseDevice;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public LicenseHardwareDataTable LicenseHardware => tableLicenseHardware;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public LicenseRecorderDataTable LicenseRecorder => tableLicenseRecorder;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RecordersDataTable Recorders => tableRecorders;

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

	public new LicenseDataSet GetChanges()
	{
		return (LicenseDataSet)base.GetChanges();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public LicenseDataSet()
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
	protected LicenseDataSet(SerializationInfo info, StreamingContext context)
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
			if (dataSet.Tables["Devices"] != null)
			{
				base.Tables.Add(new DevicesDataTable(dataSet.Tables["Devices"]));
			}
			if (dataSet.Tables["Hardware"] != null)
			{
				base.Tables.Add(new HardwareDataTable(dataSet.Tables["Hardware"]));
			}
			if (dataSet.Tables["LicenseDevice"] != null)
			{
				base.Tables.Add(new LicenseDeviceDataTable(dataSet.Tables["LicenseDevice"]));
			}
			if (dataSet.Tables["LicenseHardware"] != null)
			{
				base.Tables.Add(new LicenseHardwareDataTable(dataSet.Tables["LicenseHardware"]));
			}
			if (dataSet.Tables["LicenseRecorder"] != null)
			{
				base.Tables.Add(new LicenseRecorderDataTable(dataSet.Tables["LicenseRecorder"]));
			}
			if (dataSet.Tables["Recorders"] != null)
			{
				base.Tables.Add(new RecordersDataTable(dataSet.Tables["Recorders"]));
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
		LicenseDataSet licenseDataSet = (LicenseDataSet)base.Clone();
		licenseDataSet.InitVars();
		licenseDataSet.SchemaSerializationMode = SchemaSerializationMode;
		return licenseDataSet;
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
			if (dataSet.Tables["Devices"] != null)
			{
				base.Tables.Add(new DevicesDataTable(dataSet.Tables["Devices"]));
			}
			if (dataSet.Tables["Hardware"] != null)
			{
				base.Tables.Add(new HardwareDataTable(dataSet.Tables["Hardware"]));
			}
			if (dataSet.Tables["LicenseDevice"] != null)
			{
				base.Tables.Add(new LicenseDeviceDataTable(dataSet.Tables["LicenseDevice"]));
			}
			if (dataSet.Tables["LicenseHardware"] != null)
			{
				base.Tables.Add(new LicenseHardwareDataTable(dataSet.Tables["LicenseHardware"]));
			}
			if (dataSet.Tables["LicenseRecorder"] != null)
			{
				base.Tables.Add(new LicenseRecorderDataTable(dataSet.Tables["LicenseRecorder"]));
			}
			if (dataSet.Tables["Recorders"] != null)
			{
				base.Tables.Add(new RecordersDataTable(dataSet.Tables["Recorders"]));
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
		tableDevices = (DevicesDataTable)base.Tables["Devices"];
		if (initTable && tableDevices != null)
		{
			tableDevices.InitVars();
		}
		tableHardware = (HardwareDataTable)base.Tables["Hardware"];
		if (initTable && tableHardware != null)
		{
			tableHardware.InitVars();
		}
		tableLicenseDevice = (LicenseDeviceDataTable)base.Tables["LicenseDevice"];
		if (initTable && tableLicenseDevice != null)
		{
			tableLicenseDevice.InitVars();
		}
		tableLicenseHardware = (LicenseHardwareDataTable)base.Tables["LicenseHardware"];
		if (initTable && tableLicenseHardware != null)
		{
			tableLicenseHardware.InitVars();
		}
		tableLicenseRecorder = (LicenseRecorderDataTable)base.Tables["LicenseRecorder"];
		if (initTable && tableLicenseRecorder != null)
		{
			tableLicenseRecorder.InitVars();
		}
		tableRecorders = (RecordersDataTable)base.Tables["Recorders"];
		if (initTable && tableRecorders != null)
		{
			tableRecorders.InitVars();
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private void InitClass()
	{
		base.DataSetName = "LicenseDataSet";
		base.Prefix = "";
		base.Namespace = "http://tempuri.org/LicenseDataSet.xsd";
		base.EnforceConstraints = false;
		SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
		tableDevices = new DevicesDataTable();
		base.Tables.Add(tableDevices);
		tableHardware = new HardwareDataTable();
		base.Tables.Add(tableHardware);
		tableLicenseDevice = new LicenseDeviceDataTable();
		base.Tables.Add(tableLicenseDevice);
		tableLicenseHardware = new LicenseHardwareDataTable();
		base.Tables.Add(tableLicenseHardware);
		tableLicenseRecorder = new LicenseRecorderDataTable();
		base.Tables.Add(tableLicenseRecorder);
		tableRecorders = new RecordersDataTable();
		base.Tables.Add(tableRecorders);
		ForeignKeyConstraint foreignKeyConstraint = new ForeignKeyConstraint("FK_Devices_Hardware", new DataColumn[1] { tableHardware.IDHardwareColumn }, new DataColumn[1] { tableDevices.IDHardwareColumn });
		tableDevices.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = Rule.None;
		foreignKeyConstraint.UpdateRule = Rule.None;
		foreignKeyConstraint = new ForeignKeyConstraint("FK_Hardware_Recorders", new DataColumn[1] { tableRecorders.IDRecorderColumn }, new DataColumn[1] { tableHardware.IDRecorderColumn });
		tableHardware.Constraints.Add(foreignKeyConstraint);
		foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
		foreignKeyConstraint.DeleteRule = Rule.None;
		foreignKeyConstraint.UpdateRule = Rule.None;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeDevices()
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
	private bool ShouldSerializeLicenseDevice()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeLicenseHardware()
	{
		return false;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private bool ShouldSerializeLicenseRecorder()
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
		LicenseDataSet licenseDataSet = new LicenseDataSet();
		XmlSchemaComplexType xmlSchemaComplexType = new XmlSchemaComplexType();
		XmlSchemaSequence xmlSchemaSequence = new XmlSchemaSequence();
		XmlSchemaAny xmlSchemaAny = new XmlSchemaAny();
		xmlSchemaAny.Namespace = licenseDataSet.Namespace;
		xmlSchemaSequence.Items.Add(xmlSchemaAny);
		xmlSchemaComplexType.Particle = xmlSchemaSequence;
		XmlSchema schemaSerializable = licenseDataSet.GetSchemaSerializable();
		if (xs.Contains(schemaSerializable.TargetNamespace))
		{
			MemoryStream memoryStream = new MemoryStream();
			MemoryStream memoryStream2 = new MemoryStream();
			try
			{
				XmlSchema xmlSchema = null;
				schemaSerializable.Write(memoryStream);
				IEnumerator enumerator = xs.Schemas(schemaSerializable.TargetNamespace).GetEnumerator();
				while (enumerator.MoveNext())
				{
					xmlSchema = (XmlSchema)enumerator.Current;
					memoryStream2.SetLength(0L);
					xmlSchema.Write(memoryStream2);
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
