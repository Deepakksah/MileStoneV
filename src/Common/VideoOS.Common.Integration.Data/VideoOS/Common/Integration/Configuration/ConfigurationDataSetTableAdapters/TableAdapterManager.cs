using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

[DesignerCategory("code")]
[ToolboxItem(true)]
[Designer("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[HelpKeyword("vs.data.TableAdapterManager")]
public class TableAdapterManager : Component
{
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public enum UpdateOrderOption
	{
		InsertUpdateDelete,
		UpdateInsertDelete
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private class SelfReferenceComparer : IComparer<DataRow>
	{
		private DataRelation _relation;

		private int _childFirst;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SelfReferenceComparer(DataRelation relation, bool childFirst)
		{
			_relation = relation;
			if (childFirst)
			{
				_childFirst = -1;
			}
			else
			{
				_childFirst = 1;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private DataRow GetRoot(DataRow row, out int distance)
		{
			DataRow result = row;
			distance = 0;
			IDictionary<DataRow, DataRow> dictionary = new Dictionary<DataRow, DataRow>();
			dictionary[row] = row;
			DataRow parentRow = row.GetParentRow(_relation, DataRowVersion.Default);
			while (parentRow != null && !dictionary.ContainsKey(parentRow))
			{
				distance++;
				result = parentRow;
				dictionary[parentRow] = parentRow;
				parentRow = parentRow.GetParentRow(_relation, DataRowVersion.Default);
			}
			if (distance == 0)
			{
				dictionary.Clear();
				dictionary[row] = row;
				parentRow = row.GetParentRow(_relation, DataRowVersion.Original);
				while (parentRow != null && !dictionary.ContainsKey(parentRow))
				{
					distance++;
					result = parentRow;
					dictionary[parentRow] = parentRow;
					parentRow = parentRow.GetParentRow(_relation, DataRowVersion.Original);
				}
			}
			return result;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int Compare(DataRow row1, DataRow row2)
		{
			if (row1 == row2)
			{
				return 0;
			}
			if (row1 == null)
			{
				return -1;
			}
			if (row2 == null)
			{
				return 1;
			}
			int distance = 0;
			DataRow root = GetRoot(row1, out distance);
			int distance2 = 0;
			DataRow root2 = GetRoot(row2, out distance2);
			if (root == root2)
			{
				return _childFirst * distance.CompareTo(distance2);
			}
			if (root.Table.Rows.IndexOf(root) < root2.Table.Rows.IndexOf(root2))
			{
				return -1;
			}
			return 1;
		}
	}

	private UpdateOrderOption _updateOrder;

	private ArchiveStorageTableAdapter _archiveStorageTableAdapter;

	private AudioMessageTableAdapter _audioMessageTableAdapter;

	private CompressorsTableAdapter _compressorsTableAdapter;

	private DeviceDataDistributionTableAdapter _deviceDataDistributionTableAdapter;

	private DeviceGroupMembersTableAdapter _deviceGroupMembersTableAdapter;

	private DeviceGroupsTableAdapter _deviceGroupsTableAdapter;

	private DeviceLinksTableAdapter _deviceLinksTableAdapter;

	private DevicePrivacyMaskTableAdapter _devicePrivacyMaskTableAdapter;

	private DevicesTableAdapter _devicesTableAdapter;

	private DeviceTrackDefinitionTableAdapter _deviceTrackDefinitionTableAdapter;

	private DeviceTrackTableAdapter _deviceTrackTableAdapter;

	private DriverNumbersTableAdapter _driverNumbersTableAdapter;

	private EventTypesTableAdapter _eventTypesTableAdapter;

	private ExternalPluginsTableAdapter _externalPluginsTableAdapter;

	private HardwareBlockTableAdapter _hardwareBlockTableAdapter;

	private HardwareDefinitionTableAdapter _hardwareDefinitionTableAdapter;

	private HardwareDriverRecorderTableAdapter _hardwareDriverRecorderTableAdapter;

	private HardwareTableAdapter _hardwareTableAdapter;

	private HostedHardwareTableAdapter _hostedHardwareTableAdapter;

	private HostingServerTableAdapter _hostingServerTableAdapter;

	private LicenseTableAdapter _licenseTableAdapter;

	private MatrixTableAdapter _matrixTableAdapter;

	private MIPObjectTableAdapter _mIPObjectTableAdapter;

	private NotificationProfilesTableAdapter _notificationProfilesTableAdapter;

	private NotificationSettingsTableAdapter _notificationSettingsTableAdapter;

	private NotificationTypesTableAdapter _notificationTypesTableAdapter;

	private PatrollingProfilesTableAdapter _patrollingProfilesTableAdapter;

	private RecorderPluginDeviceTableAdapter _recorderPluginDeviceTableAdapter;

	private RecorderPluginsTableAdapter _recorderPluginsTableAdapter;

	private RecordersTableAdapter _recordersTableAdapter;

	private RecordingServerServiceConfigurableTableAdapter _recordingServerServiceConfigurableTableAdapter;

	private RecordingServerServiceRecorderTableAdapter _recordingServerServiceRecorderTableAdapter;

	private RecordingServerServiceTableAdapter _recordingServerServiceTableAdapter;

	private RecordingStorageDeviceTableAdapter _recordingStorageDeviceTableAdapter;

	private RecordingStorageTableAdapter _recordingStorageTableAdapter;

	private RegisteredDeviceTypesTableAdapter _registeredDeviceTypesTableAdapter;

	private RegisteredServersTableAdapter _registeredServersTableAdapter;

	private RulesTableAdapter _rulesTableAdapter;

	private RuleVariablesTableAdapter _ruleVariablesTableAdapter;

	private ServerGroupMembershipTableAdapter _serverGroupMembershipTableAdapter;

	private ServerGroupTableAdapter _serverGroupTableAdapter;

	private SystemSettingsTableAdapter _systemSettingsTableAdapter;

	private TimeProfileTableAdapter _timeProfileTableAdapter;

	private bool _backupDataSetBeforeUpdate;

	private IDbConnection _connection;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public UpdateOrderOption UpdateOrder
	{
		get
		{
			return _updateOrder;
		}
		set
		{
			_updateOrder = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public ArchiveStorageTableAdapter ArchiveStorageTableAdapter
	{
		get
		{
			return _archiveStorageTableAdapter;
		}
		set
		{
			_archiveStorageTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public AudioMessageTableAdapter AudioMessageTableAdapter
	{
		get
		{
			return _audioMessageTableAdapter;
		}
		set
		{
			_audioMessageTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public CompressorsTableAdapter CompressorsTableAdapter
	{
		get
		{
			return _compressorsTableAdapter;
		}
		set
		{
			_compressorsTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public DeviceDataDistributionTableAdapter DeviceDataDistributionTableAdapter
	{
		get
		{
			return _deviceDataDistributionTableAdapter;
		}
		set
		{
			_deviceDataDistributionTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public DeviceGroupMembersTableAdapter DeviceGroupMembersTableAdapter
	{
		get
		{
			return _deviceGroupMembersTableAdapter;
		}
		set
		{
			_deviceGroupMembersTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public DeviceGroupsTableAdapter DeviceGroupsTableAdapter
	{
		get
		{
			return _deviceGroupsTableAdapter;
		}
		set
		{
			_deviceGroupsTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public DeviceLinksTableAdapter DeviceLinksTableAdapter
	{
		get
		{
			return _deviceLinksTableAdapter;
		}
		set
		{
			_deviceLinksTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public DevicePrivacyMaskTableAdapter DevicePrivacyMaskTableAdapter
	{
		get
		{
			return _devicePrivacyMaskTableAdapter;
		}
		set
		{
			_devicePrivacyMaskTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public DevicesTableAdapter DevicesTableAdapter
	{
		get
		{
			return _devicesTableAdapter;
		}
		set
		{
			_devicesTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public DeviceTrackDefinitionTableAdapter DeviceTrackDefinitionTableAdapter
	{
		get
		{
			return _deviceTrackDefinitionTableAdapter;
		}
		set
		{
			_deviceTrackDefinitionTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public DeviceTrackTableAdapter DeviceTrackTableAdapter
	{
		get
		{
			return _deviceTrackTableAdapter;
		}
		set
		{
			_deviceTrackTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public DriverNumbersTableAdapter DriverNumbersTableAdapter
	{
		get
		{
			return _driverNumbersTableAdapter;
		}
		set
		{
			_driverNumbersTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public EventTypesTableAdapter EventTypesTableAdapter
	{
		get
		{
			return _eventTypesTableAdapter;
		}
		set
		{
			_eventTypesTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public ExternalPluginsTableAdapter ExternalPluginsTableAdapter
	{
		get
		{
			return _externalPluginsTableAdapter;
		}
		set
		{
			_externalPluginsTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public HardwareBlockTableAdapter HardwareBlockTableAdapter
	{
		get
		{
			return _hardwareBlockTableAdapter;
		}
		set
		{
			_hardwareBlockTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public HardwareDefinitionTableAdapter HardwareDefinitionTableAdapter
	{
		get
		{
			return _hardwareDefinitionTableAdapter;
		}
		set
		{
			_hardwareDefinitionTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public HardwareDriverRecorderTableAdapter HardwareDriverRecorderTableAdapter
	{
		get
		{
			return _hardwareDriverRecorderTableAdapter;
		}
		set
		{
			_hardwareDriverRecorderTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public HardwareTableAdapter HardwareTableAdapter
	{
		get
		{
			return _hardwareTableAdapter;
		}
		set
		{
			_hardwareTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public HostedHardwareTableAdapter HostedHardwareTableAdapter
	{
		get
		{
			return _hostedHardwareTableAdapter;
		}
		set
		{
			_hostedHardwareTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public HostingServerTableAdapter HostingServerTableAdapter
	{
		get
		{
			return _hostingServerTableAdapter;
		}
		set
		{
			_hostingServerTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public LicenseTableAdapter LicenseTableAdapter
	{
		get
		{
			return _licenseTableAdapter;
		}
		set
		{
			_licenseTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public MatrixTableAdapter MatrixTableAdapter
	{
		get
		{
			return _matrixTableAdapter;
		}
		set
		{
			_matrixTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public MIPObjectTableAdapter MIPObjectTableAdapter
	{
		get
		{
			return _mIPObjectTableAdapter;
		}
		set
		{
			_mIPObjectTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public NotificationProfilesTableAdapter NotificationProfilesTableAdapter
	{
		get
		{
			return _notificationProfilesTableAdapter;
		}
		set
		{
			_notificationProfilesTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public NotificationSettingsTableAdapter NotificationSettingsTableAdapter
	{
		get
		{
			return _notificationSettingsTableAdapter;
		}
		set
		{
			_notificationSettingsTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public NotificationTypesTableAdapter NotificationTypesTableAdapter
	{
		get
		{
			return _notificationTypesTableAdapter;
		}
		set
		{
			_notificationTypesTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public PatrollingProfilesTableAdapter PatrollingProfilesTableAdapter
	{
		get
		{
			return _patrollingProfilesTableAdapter;
		}
		set
		{
			_patrollingProfilesTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RecorderPluginDeviceTableAdapter RecorderPluginDeviceTableAdapter
	{
		get
		{
			return _recorderPluginDeviceTableAdapter;
		}
		set
		{
			_recorderPluginDeviceTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RecorderPluginsTableAdapter RecorderPluginsTableAdapter
	{
		get
		{
			return _recorderPluginsTableAdapter;
		}
		set
		{
			_recorderPluginsTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RecordersTableAdapter RecordersTableAdapter
	{
		get
		{
			return _recordersTableAdapter;
		}
		set
		{
			_recordersTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RecordingServerServiceConfigurableTableAdapter RecordingServerServiceConfigurableTableAdapter
	{
		get
		{
			return _recordingServerServiceConfigurableTableAdapter;
		}
		set
		{
			_recordingServerServiceConfigurableTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RecordingServerServiceRecorderTableAdapter RecordingServerServiceRecorderTableAdapter
	{
		get
		{
			return _recordingServerServiceRecorderTableAdapter;
		}
		set
		{
			_recordingServerServiceRecorderTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RecordingServerServiceTableAdapter RecordingServerServiceTableAdapter
	{
		get
		{
			return _recordingServerServiceTableAdapter;
		}
		set
		{
			_recordingServerServiceTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RecordingStorageDeviceTableAdapter RecordingStorageDeviceTableAdapter
	{
		get
		{
			return _recordingStorageDeviceTableAdapter;
		}
		set
		{
			_recordingStorageDeviceTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RecordingStorageTableAdapter RecordingStorageTableAdapter
	{
		get
		{
			return _recordingStorageTableAdapter;
		}
		set
		{
			_recordingStorageTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RegisteredDeviceTypesTableAdapter RegisteredDeviceTypesTableAdapter
	{
		get
		{
			return _registeredDeviceTypesTableAdapter;
		}
		set
		{
			_registeredDeviceTypesTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RegisteredServersTableAdapter RegisteredServersTableAdapter
	{
		get
		{
			return _registeredServersTableAdapter;
		}
		set
		{
			_registeredServersTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RulesTableAdapter RulesTableAdapter
	{
		get
		{
			return _rulesTableAdapter;
		}
		set
		{
			_rulesTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public RuleVariablesTableAdapter RuleVariablesTableAdapter
	{
		get
		{
			return _ruleVariablesTableAdapter;
		}
		set
		{
			_ruleVariablesTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public ServerGroupMembershipTableAdapter ServerGroupMembershipTableAdapter
	{
		get
		{
			return _serverGroupMembershipTableAdapter;
		}
		set
		{
			_serverGroupMembershipTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public ServerGroupTableAdapter ServerGroupTableAdapter
	{
		get
		{
			return _serverGroupTableAdapter;
		}
		set
		{
			_serverGroupTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public SystemSettingsTableAdapter SystemSettingsTableAdapter
	{
		get
		{
			return _systemSettingsTableAdapter;
		}
		set
		{
			_systemSettingsTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public TimeProfileTableAdapter TimeProfileTableAdapter
	{
		get
		{
			return _timeProfileTableAdapter;
		}
		set
		{
			_timeProfileTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public bool BackupDataSetBeforeUpdate
	{
		get
		{
			return _backupDataSetBeforeUpdate;
		}
		set
		{
			_backupDataSetBeforeUpdate = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	public IDbConnection Connection
	{
		get
		{
			if (_connection != null)
			{
				return _connection;
			}
			if (_archiveStorageTableAdapter != null && _archiveStorageTableAdapter.Connection != null)
			{
				return _archiveStorageTableAdapter.Connection;
			}
			if (_audioMessageTableAdapter != null && _audioMessageTableAdapter.Connection != null)
			{
				return _audioMessageTableAdapter.Connection;
			}
			if (_compressorsTableAdapter != null && _compressorsTableAdapter.Connection != null)
			{
				return _compressorsTableAdapter.Connection;
			}
			if (_deviceDataDistributionTableAdapter != null && _deviceDataDistributionTableAdapter.Connection != null)
			{
				return _deviceDataDistributionTableAdapter.Connection;
			}
			if (_deviceGroupMembersTableAdapter != null && _deviceGroupMembersTableAdapter.Connection != null)
			{
				return _deviceGroupMembersTableAdapter.Connection;
			}
			if (_deviceGroupsTableAdapter != null && _deviceGroupsTableAdapter.Connection != null)
			{
				return _deviceGroupsTableAdapter.Connection;
			}
			if (_deviceLinksTableAdapter != null && _deviceLinksTableAdapter.Connection != null)
			{
				return _deviceLinksTableAdapter.Connection;
			}
			if (_devicePrivacyMaskTableAdapter != null && _devicePrivacyMaskTableAdapter.Connection != null)
			{
				return _devicePrivacyMaskTableAdapter.Connection;
			}
			if (_devicesTableAdapter != null && _devicesTableAdapter.Connection != null)
			{
				return _devicesTableAdapter.Connection;
			}
			if (_deviceTrackDefinitionTableAdapter != null && _deviceTrackDefinitionTableAdapter.Connection != null)
			{
				return _deviceTrackDefinitionTableAdapter.Connection;
			}
			if (_deviceTrackTableAdapter != null && _deviceTrackTableAdapter.Connection != null)
			{
				return _deviceTrackTableAdapter.Connection;
			}
			if (_driverNumbersTableAdapter != null && _driverNumbersTableAdapter.Connection != null)
			{
				return _driverNumbersTableAdapter.Connection;
			}
			if (_eventTypesTableAdapter != null && _eventTypesTableAdapter.Connection != null)
			{
				return _eventTypesTableAdapter.Connection;
			}
			if (_externalPluginsTableAdapter != null && _externalPluginsTableAdapter.Connection != null)
			{
				return _externalPluginsTableAdapter.Connection;
			}
			if (_hardwareBlockTableAdapter != null && _hardwareBlockTableAdapter.Connection != null)
			{
				return _hardwareBlockTableAdapter.Connection;
			}
			if (_hardwareDefinitionTableAdapter != null && _hardwareDefinitionTableAdapter.Connection != null)
			{
				return _hardwareDefinitionTableAdapter.Connection;
			}
			if (_hardwareDriverRecorderTableAdapter != null && _hardwareDriverRecorderTableAdapter.Connection != null)
			{
				return _hardwareDriverRecorderTableAdapter.Connection;
			}
			if (_hardwareTableAdapter != null && _hardwareTableAdapter.Connection != null)
			{
				return _hardwareTableAdapter.Connection;
			}
			if (_hostedHardwareTableAdapter != null && _hostedHardwareTableAdapter.Connection != null)
			{
				return _hostedHardwareTableAdapter.Connection;
			}
			if (_hostingServerTableAdapter != null && _hostingServerTableAdapter.Connection != null)
			{
				return _hostingServerTableAdapter.Connection;
			}
			if (_licenseTableAdapter != null && _licenseTableAdapter.Connection != null)
			{
				return _licenseTableAdapter.Connection;
			}
			if (_matrixTableAdapter != null && _matrixTableAdapter.Connection != null)
			{
				return _matrixTableAdapter.Connection;
			}
			if (_mIPObjectTableAdapter != null && _mIPObjectTableAdapter.Connection != null)
			{
				return _mIPObjectTableAdapter.Connection;
			}
			if (_notificationProfilesTableAdapter != null && _notificationProfilesTableAdapter.Connection != null)
			{
				return _notificationProfilesTableAdapter.Connection;
			}
			if (_notificationSettingsTableAdapter != null && _notificationSettingsTableAdapter.Connection != null)
			{
				return _notificationSettingsTableAdapter.Connection;
			}
			if (_notificationTypesTableAdapter != null && _notificationTypesTableAdapter.Connection != null)
			{
				return _notificationTypesTableAdapter.Connection;
			}
			if (_patrollingProfilesTableAdapter != null && _patrollingProfilesTableAdapter.Connection != null)
			{
				return _patrollingProfilesTableAdapter.Connection;
			}
			if (_recorderPluginDeviceTableAdapter != null && _recorderPluginDeviceTableAdapter.Connection != null)
			{
				return _recorderPluginDeviceTableAdapter.Connection;
			}
			if (_recorderPluginsTableAdapter != null && _recorderPluginsTableAdapter.Connection != null)
			{
				return _recorderPluginsTableAdapter.Connection;
			}
			if (_recordersTableAdapter != null && _recordersTableAdapter.Connection != null)
			{
				return _recordersTableAdapter.Connection;
			}
			if (_recordingServerServiceConfigurableTableAdapter != null && _recordingServerServiceConfigurableTableAdapter.Connection != null)
			{
				return _recordingServerServiceConfigurableTableAdapter.Connection;
			}
			if (_recordingServerServiceRecorderTableAdapter != null && _recordingServerServiceRecorderTableAdapter.Connection != null)
			{
				return _recordingServerServiceRecorderTableAdapter.Connection;
			}
			if (_recordingServerServiceTableAdapter != null && _recordingServerServiceTableAdapter.Connection != null)
			{
				return _recordingServerServiceTableAdapter.Connection;
			}
			if (_recordingStorageDeviceTableAdapter != null && _recordingStorageDeviceTableAdapter.Connection != null)
			{
				return _recordingStorageDeviceTableAdapter.Connection;
			}
			if (_recordingStorageTableAdapter != null && _recordingStorageTableAdapter.Connection != null)
			{
				return _recordingStorageTableAdapter.Connection;
			}
			if (_registeredDeviceTypesTableAdapter != null && _registeredDeviceTypesTableAdapter.Connection != null)
			{
				return _registeredDeviceTypesTableAdapter.Connection;
			}
			if (_registeredServersTableAdapter != null && _registeredServersTableAdapter.Connection != null)
			{
				return _registeredServersTableAdapter.Connection;
			}
			if (_rulesTableAdapter != null && _rulesTableAdapter.Connection != null)
			{
				return _rulesTableAdapter.Connection;
			}
			if (_ruleVariablesTableAdapter != null && _ruleVariablesTableAdapter.Connection != null)
			{
				return _ruleVariablesTableAdapter.Connection;
			}
			if (_serverGroupMembershipTableAdapter != null && _serverGroupMembershipTableAdapter.Connection != null)
			{
				return _serverGroupMembershipTableAdapter.Connection;
			}
			if (_serverGroupTableAdapter != null && _serverGroupTableAdapter.Connection != null)
			{
				return _serverGroupTableAdapter.Connection;
			}
			if (_systemSettingsTableAdapter != null && _systemSettingsTableAdapter.Connection != null)
			{
				return _systemSettingsTableAdapter.Connection;
			}
			if (_timeProfileTableAdapter != null && _timeProfileTableAdapter.Connection != null)
			{
				return _timeProfileTableAdapter.Connection;
			}
			return null;
		}
		set
		{
			_connection = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	public int TableAdapterInstanceCount
	{
		get
		{
			int num = 0;
			if (_archiveStorageTableAdapter != null)
			{
				num++;
			}
			if (_audioMessageTableAdapter != null)
			{
				num++;
			}
			if (_compressorsTableAdapter != null)
			{
				num++;
			}
			if (_deviceDataDistributionTableAdapter != null)
			{
				num++;
			}
			if (_deviceGroupMembersTableAdapter != null)
			{
				num++;
			}
			if (_deviceGroupsTableAdapter != null)
			{
				num++;
			}
			if (_deviceLinksTableAdapter != null)
			{
				num++;
			}
			if (_devicePrivacyMaskTableAdapter != null)
			{
				num++;
			}
			if (_devicesTableAdapter != null)
			{
				num++;
			}
			if (_deviceTrackDefinitionTableAdapter != null)
			{
				num++;
			}
			if (_deviceTrackTableAdapter != null)
			{
				num++;
			}
			if (_driverNumbersTableAdapter != null)
			{
				num++;
			}
			if (_eventTypesTableAdapter != null)
			{
				num++;
			}
			if (_externalPluginsTableAdapter != null)
			{
				num++;
			}
			if (_hardwareBlockTableAdapter != null)
			{
				num++;
			}
			if (_hardwareDefinitionTableAdapter != null)
			{
				num++;
			}
			if (_hardwareDriverRecorderTableAdapter != null)
			{
				num++;
			}
			if (_hardwareTableAdapter != null)
			{
				num++;
			}
			if (_hostedHardwareTableAdapter != null)
			{
				num++;
			}
			if (_hostingServerTableAdapter != null)
			{
				num++;
			}
			if (_licenseTableAdapter != null)
			{
				num++;
			}
			if (_matrixTableAdapter != null)
			{
				num++;
			}
			if (_mIPObjectTableAdapter != null)
			{
				num++;
			}
			if (_notificationProfilesTableAdapter != null)
			{
				num++;
			}
			if (_notificationSettingsTableAdapter != null)
			{
				num++;
			}
			if (_notificationTypesTableAdapter != null)
			{
				num++;
			}
			if (_patrollingProfilesTableAdapter != null)
			{
				num++;
			}
			if (_recorderPluginDeviceTableAdapter != null)
			{
				num++;
			}
			if (_recorderPluginsTableAdapter != null)
			{
				num++;
			}
			if (_recordersTableAdapter != null)
			{
				num++;
			}
			if (_recordingServerServiceConfigurableTableAdapter != null)
			{
				num++;
			}
			if (_recordingServerServiceRecorderTableAdapter != null)
			{
				num++;
			}
			if (_recordingServerServiceTableAdapter != null)
			{
				num++;
			}
			if (_recordingStorageDeviceTableAdapter != null)
			{
				num++;
			}
			if (_recordingStorageTableAdapter != null)
			{
				num++;
			}
			if (_registeredDeviceTypesTableAdapter != null)
			{
				num++;
			}
			if (_registeredServersTableAdapter != null)
			{
				num++;
			}
			if (_rulesTableAdapter != null)
			{
				num++;
			}
			if (_ruleVariablesTableAdapter != null)
			{
				num++;
			}
			if (_serverGroupMembershipTableAdapter != null)
			{
				num++;
			}
			if (_serverGroupTableAdapter != null)
			{
				num++;
			}
			if (_systemSettingsTableAdapter != null)
			{
				num++;
			}
			if (_timeProfileTableAdapter != null)
			{
				num++;
			}
			return num;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private int UpdateUpdatedRows(ConfigurationDataSet dataSet, List<DataRow> allChangedRows, List<DataRow> allAddedRows)
	{
		int num = 0;
		if (_hostingServerTableAdapter != null)
		{
			DataRow[] updatedRows = ((DataTable)(object)dataSet.HostingServer).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows = GetRealUpdatedRows(updatedRows, allAddedRows);
			if (updatedRows != null && updatedRows.Length != 0)
			{
				num += _hostingServerTableAdapter.Update(updatedRows);
				allChangedRows.AddRange(updatedRows);
			}
		}
		if (_recordersTableAdapter != null)
		{
			DataRow[] updatedRows2 = ((DataTable)(object)dataSet.Recorders).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows2 = GetRealUpdatedRows(updatedRows2, allAddedRows);
			if (updatedRows2 != null && updatedRows2.Length != 0)
			{
				num += _recordersTableAdapter.Update(updatedRows2);
				allChangedRows.AddRange(updatedRows2);
			}
		}
		if (_hostedHardwareTableAdapter != null)
		{
			DataRow[] updatedRows3 = ((DataTable)(object)dataSet.HostedHardware).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows3 = GetRealUpdatedRows(updatedRows3, allAddedRows);
			if (updatedRows3 != null && updatedRows3.Length != 0)
			{
				num += _hostedHardwareTableAdapter.Update(updatedRows3);
				allChangedRows.AddRange(updatedRows3);
			}
		}
		if (_hardwareDefinitionTableAdapter != null)
		{
			DataRow[] updatedRows4 = ((DataTable)(object)dataSet.HardwareDefinition).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows4 = GetRealUpdatedRows(updatedRows4, allAddedRows);
			if (updatedRows4 != null && updatedRows4.Length != 0)
			{
				num += _hardwareDefinitionTableAdapter.Update(updatedRows4);
				allChangedRows.AddRange(updatedRows4);
			}
		}
		if (_registeredDeviceTypesTableAdapter != null)
		{
			DataRow[] updatedRows5 = ((DataTable)(object)dataSet.RegisteredDeviceTypes).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows5 = GetRealUpdatedRows(updatedRows5, allAddedRows);
			if (updatedRows5 != null && updatedRows5.Length != 0)
			{
				num += _registeredDeviceTypesTableAdapter.Update(updatedRows5);
				allChangedRows.AddRange(updatedRows5);
			}
		}
		if (_hardwareTableAdapter != null)
		{
			DataRow[] updatedRows6 = ((DataTable)(object)dataSet.Hardware).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows6 = GetRealUpdatedRows(updatedRows6, allAddedRows);
			if (updatedRows6 != null && updatedRows6.Length != 0)
			{
				num += _hardwareTableAdapter.Update(updatedRows6);
				allChangedRows.AddRange(updatedRows6);
			}
		}
		if (_devicesTableAdapter != null)
		{
			DataRow[] updatedRows7 = ((DataTable)(object)dataSet.Devices).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows7 = GetRealUpdatedRows(updatedRows7, allAddedRows);
			if (updatedRows7 != null && updatedRows7.Length != 0)
			{
				num += _devicesTableAdapter.Update(updatedRows7);
				allChangedRows.AddRange(updatedRows7);
			}
		}
		if (_deviceTrackDefinitionTableAdapter != null)
		{
			DataRow[] updatedRows8 = ((DataTable)(object)dataSet.DeviceTrackDefinition).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows8 = GetRealUpdatedRows(updatedRows8, allAddedRows);
			if (updatedRows8 != null && updatedRows8.Length != 0)
			{
				num += _deviceTrackDefinitionTableAdapter.Update(updatedRows8);
				allChangedRows.AddRange(updatedRows8);
			}
		}
		if (_serverGroupTableAdapter != null)
		{
			DataRow[] updatedRows9 = ((DataTable)(object)dataSet.ServerGroup).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows9 = GetRealUpdatedRows(updatedRows9, allAddedRows);
			if (updatedRows9 != null && updatedRows9.Length != 0)
			{
				num += _serverGroupTableAdapter.Update(updatedRows9);
				allChangedRows.AddRange(updatedRows9);
			}
		}
		if (_deviceGroupsTableAdapter != null)
		{
			DataRow[] updatedRows10 = ((DataTable)(object)dataSet.DeviceGroups).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows10 = GetRealUpdatedRows(updatedRows10, allAddedRows);
			if (updatedRows10 != null && updatedRows10.Length != 0)
			{
				num += _deviceGroupsTableAdapter.Update(updatedRows10);
				allChangedRows.AddRange(updatedRows10);
			}
		}
		if (_deviceTrackTableAdapter != null)
		{
			DataRow[] updatedRows11 = ((DataTable)(object)dataSet.DeviceTrack).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows11 = GetRealUpdatedRows(updatedRows11, allAddedRows);
			if (updatedRows11 != null && updatedRows11.Length != 0)
			{
				num += _deviceTrackTableAdapter.Update(updatedRows11);
				allChangedRows.AddRange(updatedRows11);
			}
		}
		if (_recordingStorageTableAdapter != null)
		{
			DataRow[] updatedRows12 = ((DataTable)(object)dataSet.RecordingStorage).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows12 = GetRealUpdatedRows(updatedRows12, allAddedRows);
			if (updatedRows12 != null && updatedRows12.Length != 0)
			{
				num += _recordingStorageTableAdapter.Update(updatedRows12);
				allChangedRows.AddRange(updatedRows12);
			}
		}
		if (_recordingServerServiceTableAdapter != null)
		{
			DataRow[] updatedRows13 = ((DataTable)(object)dataSet.RecordingServerService).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows13 = GetRealUpdatedRows(updatedRows13, allAddedRows);
			if (updatedRows13 != null && updatedRows13.Length != 0)
			{
				num += _recordingServerServiceTableAdapter.Update(updatedRows13);
				allChangedRows.AddRange(updatedRows13);
			}
		}
		if (_recorderPluginsTableAdapter != null)
		{
			DataRow[] updatedRows14 = ((DataTable)(object)dataSet.RecorderPlugins).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows14 = GetRealUpdatedRows(updatedRows14, allAddedRows);
			if (updatedRows14 != null && updatedRows14.Length != 0)
			{
				num += _recorderPluginsTableAdapter.Update(updatedRows14);
				allChangedRows.AddRange(updatedRows14);
			}
		}
		if (_notificationTypesTableAdapter != null)
		{
			DataRow[] updatedRows15 = ((DataTable)(object)dataSet.NotificationTypes).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows15 = GetRealUpdatedRows(updatedRows15, allAddedRows);
			if (updatedRows15 != null && updatedRows15.Length != 0)
			{
				num += _notificationTypesTableAdapter.Update(updatedRows15);
				allChangedRows.AddRange(updatedRows15);
			}
		}
		if (_recorderPluginDeviceTableAdapter != null)
		{
			DataRow[] updatedRows16 = ((DataTable)(object)dataSet.RecorderPluginDevice).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows16 = GetRealUpdatedRows(updatedRows16, allAddedRows);
			if (updatedRows16 != null && updatedRows16.Length != 0)
			{
				num += _recorderPluginDeviceTableAdapter.Update(updatedRows16);
				allChangedRows.AddRange(updatedRows16);
			}
		}
		if (_patrollingProfilesTableAdapter != null)
		{
			DataRow[] updatedRows17 = ((DataTable)(object)dataSet.PatrollingProfiles).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows17 = GetRealUpdatedRows(updatedRows17, allAddedRows);
			if (updatedRows17 != null && updatedRows17.Length != 0)
			{
				num += _patrollingProfilesTableAdapter.Update(updatedRows17);
				allChangedRows.AddRange(updatedRows17);
			}
		}
		if (_recordingServerServiceRecorderTableAdapter != null)
		{
			DataRow[] updatedRows18 = ((DataTable)(object)dataSet.RecordingServerServiceRecorder).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows18 = GetRealUpdatedRows(updatedRows18, allAddedRows);
			if (updatedRows18 != null && updatedRows18.Length != 0)
			{
				num += _recordingServerServiceRecorderTableAdapter.Update(updatedRows18);
				allChangedRows.AddRange(updatedRows18);
			}
		}
		if (_notificationSettingsTableAdapter != null)
		{
			DataRow[] updatedRows19 = ((DataTable)(object)dataSet.NotificationSettings).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows19 = GetRealUpdatedRows(updatedRows19, allAddedRows);
			if (updatedRows19 != null && updatedRows19.Length != 0)
			{
				num += _notificationSettingsTableAdapter.Update(updatedRows19);
				allChangedRows.AddRange(updatedRows19);
			}
		}
		if (_recordingStorageDeviceTableAdapter != null)
		{
			DataRow[] updatedRows20 = ((DataTable)(object)dataSet.RecordingStorageDevice).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows20 = GetRealUpdatedRows(updatedRows20, allAddedRows);
			if (updatedRows20 != null && updatedRows20.Length != 0)
			{
				num += _recordingStorageDeviceTableAdapter.Update(updatedRows20);
				allChangedRows.AddRange(updatedRows20);
			}
		}
		if (_registeredServersTableAdapter != null)
		{
			DataRow[] updatedRows21 = ((DataTable)(object)dataSet.RegisteredServers).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows21 = GetRealUpdatedRows(updatedRows21, allAddedRows);
			if (updatedRows21 != null && updatedRows21.Length != 0)
			{
				num += _registeredServersTableAdapter.Update(updatedRows21);
				allChangedRows.AddRange(updatedRows21);
			}
		}
		if (_rulesTableAdapter != null)
		{
			DataRow[] updatedRows22 = ((DataTable)(object)dataSet.Rules).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows22 = GetRealUpdatedRows(updatedRows22, allAddedRows);
			if (updatedRows22 != null && updatedRows22.Length != 0)
			{
				num += _rulesTableAdapter.Update(updatedRows22);
				allChangedRows.AddRange(updatedRows22);
			}
		}
		if (_ruleVariablesTableAdapter != null)
		{
			DataRow[] updatedRows23 = ((DataTable)(object)dataSet.RuleVariables).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows23 = GetRealUpdatedRows(updatedRows23, allAddedRows);
			if (updatedRows23 != null && updatedRows23.Length != 0)
			{
				num += _ruleVariablesTableAdapter.Update(updatedRows23);
				allChangedRows.AddRange(updatedRows23);
			}
		}
		if (_serverGroupMembershipTableAdapter != null)
		{
			DataRow[] updatedRows24 = ((DataTable)(object)dataSet.ServerGroupMembership).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows24 = GetRealUpdatedRows(updatedRows24, allAddedRows);
			if (updatedRows24 != null && updatedRows24.Length != 0)
			{
				num += _serverGroupMembershipTableAdapter.Update(updatedRows24);
				allChangedRows.AddRange(updatedRows24);
			}
		}
		if (_recordingServerServiceConfigurableTableAdapter != null)
		{
			DataRow[] updatedRows25 = ((DataTable)(object)dataSet.RecordingServerServiceConfigurable).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows25 = GetRealUpdatedRows(updatedRows25, allAddedRows);
			if (updatedRows25 != null && updatedRows25.Length != 0)
			{
				num += _recordingServerServiceConfigurableTableAdapter.Update(updatedRows25);
				allChangedRows.AddRange(updatedRows25);
			}
		}
		if (_notificationProfilesTableAdapter != null)
		{
			DataRow[] updatedRows26 = ((DataTable)(object)dataSet.NotificationProfiles).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows26 = GetRealUpdatedRows(updatedRows26, allAddedRows);
			if (updatedRows26 != null && updatedRows26.Length != 0)
			{
				num += _notificationProfilesTableAdapter.Update(updatedRows26);
				allChangedRows.AddRange(updatedRows26);
			}
		}
		if (_archiveStorageTableAdapter != null)
		{
			DataRow[] updatedRows27 = ((DataTable)(object)dataSet.ArchiveStorage).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows27 = GetRealUpdatedRows(updatedRows27, allAddedRows);
			if (updatedRows27 != null && updatedRows27.Length != 0)
			{
				num += _archiveStorageTableAdapter.Update(updatedRows27);
				allChangedRows.AddRange(updatedRows27);
			}
		}
		if (_matrixTableAdapter != null)
		{
			DataRow[] updatedRows28 = ((DataTable)(object)dataSet.Matrix).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows28 = GetRealUpdatedRows(updatedRows28, allAddedRows);
			if (updatedRows28 != null && updatedRows28.Length != 0)
			{
				num += _matrixTableAdapter.Update(updatedRows28);
				allChangedRows.AddRange(updatedRows28);
			}
		}
		if (_audioMessageTableAdapter != null)
		{
			DataRow[] updatedRows29 = ((DataTable)(object)dataSet.AudioMessage).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows29 = GetRealUpdatedRows(updatedRows29, allAddedRows);
			if (updatedRows29 != null && updatedRows29.Length != 0)
			{
				num += _audioMessageTableAdapter.Update(updatedRows29);
				allChangedRows.AddRange(updatedRows29);
			}
		}
		if (_compressorsTableAdapter != null)
		{
			DataRow[] updatedRows30 = ((DataTable)(object)dataSet.Compressors).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows30 = GetRealUpdatedRows(updatedRows30, allAddedRows);
			if (updatedRows30 != null && updatedRows30.Length != 0)
			{
				num += _compressorsTableAdapter.Update(updatedRows30);
				allChangedRows.AddRange(updatedRows30);
			}
		}
		if (_deviceDataDistributionTableAdapter != null)
		{
			DataRow[] updatedRows31 = ((DataTable)(object)dataSet.DeviceDataDistribution).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows31 = GetRealUpdatedRows(updatedRows31, allAddedRows);
			if (updatedRows31 != null && updatedRows31.Length != 0)
			{
				num += _deviceDataDistributionTableAdapter.Update(updatedRows31);
				allChangedRows.AddRange(updatedRows31);
			}
		}
		if (_deviceGroupMembersTableAdapter != null)
		{
			DataRow[] updatedRows32 = ((DataTable)(object)dataSet.DeviceGroupMembers).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows32 = GetRealUpdatedRows(updatedRows32, allAddedRows);
			if (updatedRows32 != null && updatedRows32.Length != 0)
			{
				num += _deviceGroupMembersTableAdapter.Update(updatedRows32);
				allChangedRows.AddRange(updatedRows32);
			}
		}
		if (_deviceLinksTableAdapter != null)
		{
			DataRow[] updatedRows33 = ((DataTable)(object)dataSet.DeviceLinks).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows33 = GetRealUpdatedRows(updatedRows33, allAddedRows);
			if (updatedRows33 != null && updatedRows33.Length != 0)
			{
				num += _deviceLinksTableAdapter.Update(updatedRows33);
				allChangedRows.AddRange(updatedRows33);
			}
		}
		if (_devicePrivacyMaskTableAdapter != null)
		{
			DataRow[] updatedRows34 = ((DataTable)(object)dataSet.DevicePrivacyMask).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows34 = GetRealUpdatedRows(updatedRows34, allAddedRows);
			if (updatedRows34 != null && updatedRows34.Length != 0)
			{
				num += _devicePrivacyMaskTableAdapter.Update(updatedRows34);
				allChangedRows.AddRange(updatedRows34);
			}
		}
		if (_driverNumbersTableAdapter != null)
		{
			DataRow[] updatedRows35 = ((DataTable)(object)dataSet.DriverNumbers).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows35 = GetRealUpdatedRows(updatedRows35, allAddedRows);
			if (updatedRows35 != null && updatedRows35.Length != 0)
			{
				num += _driverNumbersTableAdapter.Update(updatedRows35);
				allChangedRows.AddRange(updatedRows35);
			}
		}
		if (_eventTypesTableAdapter != null)
		{
			DataRow[] updatedRows36 = ((DataTable)(object)dataSet.EventTypes).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows36 = GetRealUpdatedRows(updatedRows36, allAddedRows);
			if (updatedRows36 != null && updatedRows36.Length != 0)
			{
				num += _eventTypesTableAdapter.Update(updatedRows36);
				allChangedRows.AddRange(updatedRows36);
			}
		}
		if (_externalPluginsTableAdapter != null)
		{
			DataRow[] updatedRows37 = ((DataTable)(object)dataSet.ExternalPlugins).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows37 = GetRealUpdatedRows(updatedRows37, allAddedRows);
			if (updatedRows37 != null && updatedRows37.Length != 0)
			{
				num += _externalPluginsTableAdapter.Update(updatedRows37);
				allChangedRows.AddRange(updatedRows37);
			}
		}
		if (_hardwareBlockTableAdapter != null)
		{
			DataRow[] updatedRows38 = ((DataTable)(object)dataSet.HardwareBlock).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows38 = GetRealUpdatedRows(updatedRows38, allAddedRows);
			if (updatedRows38 != null && updatedRows38.Length != 0)
			{
				num += _hardwareBlockTableAdapter.Update(updatedRows38);
				allChangedRows.AddRange(updatedRows38);
			}
		}
		if (_hardwareDriverRecorderTableAdapter != null)
		{
			DataRow[] updatedRows39 = ((DataTable)(object)dataSet.HardwareDriverRecorder).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows39 = GetRealUpdatedRows(updatedRows39, allAddedRows);
			if (updatedRows39 != null && updatedRows39.Length != 0)
			{
				num += _hardwareDriverRecorderTableAdapter.Update(updatedRows39);
				allChangedRows.AddRange(updatedRows39);
			}
		}
		if (_systemSettingsTableAdapter != null)
		{
			DataRow[] updatedRows40 = ((DataTable)(object)dataSet.SystemSettings).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows40 = GetRealUpdatedRows(updatedRows40, allAddedRows);
			if (updatedRows40 != null && updatedRows40.Length != 0)
			{
				num += _systemSettingsTableAdapter.Update(updatedRows40);
				allChangedRows.AddRange(updatedRows40);
			}
		}
		if (_licenseTableAdapter != null)
		{
			DataRow[] updatedRows41 = ((DataTable)(object)dataSet.License).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows41 = GetRealUpdatedRows(updatedRows41, allAddedRows);
			if (updatedRows41 != null && updatedRows41.Length != 0)
			{
				num += _licenseTableAdapter.Update(updatedRows41);
				allChangedRows.AddRange(updatedRows41);
			}
		}
		if (_mIPObjectTableAdapter != null)
		{
			DataRow[] updatedRows42 = ((DataTable)(object)dataSet.MIPObject).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows42 = GetRealUpdatedRows(updatedRows42, allAddedRows);
			if (updatedRows42 != null && updatedRows42.Length != 0)
			{
				num += _mIPObjectTableAdapter.Update(updatedRows42);
				allChangedRows.AddRange(updatedRows42);
			}
		}
		if (_timeProfileTableAdapter != null)
		{
			DataRow[] updatedRows43 = ((DataTable)(object)dataSet.TimeProfile).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows43 = GetRealUpdatedRows(updatedRows43, allAddedRows);
			if (updatedRows43 != null && updatedRows43.Length != 0)
			{
				num += _timeProfileTableAdapter.Update(updatedRows43);
				allChangedRows.AddRange(updatedRows43);
			}
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private int UpdateInsertedRows(ConfigurationDataSet dataSet, List<DataRow> allAddedRows)
	{
		int num = 0;
		if (_hostingServerTableAdapter != null)
		{
			DataRow[] array = ((DataTable)(object)dataSet.HostingServer).Select((string)null, (string)null, DataViewRowState.Added);
			if (array != null && array.Length != 0)
			{
				num += _hostingServerTableAdapter.Update(array);
				allAddedRows.AddRange(array);
			}
		}
		if (_recordersTableAdapter != null)
		{
			DataRow[] array2 = ((DataTable)(object)dataSet.Recorders).Select((string)null, (string)null, DataViewRowState.Added);
			if (array2 != null && array2.Length != 0)
			{
				num += _recordersTableAdapter.Update(array2);
				allAddedRows.AddRange(array2);
			}
		}
		if (_hostedHardwareTableAdapter != null)
		{
			DataRow[] array3 = ((DataTable)(object)dataSet.HostedHardware).Select((string)null, (string)null, DataViewRowState.Added);
			if (array3 != null && array3.Length != 0)
			{
				num += _hostedHardwareTableAdapter.Update(array3);
				allAddedRows.AddRange(array3);
			}
		}
		if (_hardwareDefinitionTableAdapter != null)
		{
			DataRow[] array4 = ((DataTable)(object)dataSet.HardwareDefinition).Select((string)null, (string)null, DataViewRowState.Added);
			if (array4 != null && array4.Length != 0)
			{
				num += _hardwareDefinitionTableAdapter.Update(array4);
				allAddedRows.AddRange(array4);
			}
		}
		if (_registeredDeviceTypesTableAdapter != null)
		{
			DataRow[] array5 = ((DataTable)(object)dataSet.RegisteredDeviceTypes).Select((string)null, (string)null, DataViewRowState.Added);
			if (array5 != null && array5.Length != 0)
			{
				num += _registeredDeviceTypesTableAdapter.Update(array5);
				allAddedRows.AddRange(array5);
			}
		}
		if (_hardwareTableAdapter != null)
		{
			DataRow[] array6 = ((DataTable)(object)dataSet.Hardware).Select((string)null, (string)null, DataViewRowState.Added);
			if (array6 != null && array6.Length != 0)
			{
				num += _hardwareTableAdapter.Update(array6);
				allAddedRows.AddRange(array6);
			}
		}
		if (_devicesTableAdapter != null)
		{
			DataRow[] array7 = ((DataTable)(object)dataSet.Devices).Select((string)null, (string)null, DataViewRowState.Added);
			if (array7 != null && array7.Length != 0)
			{
				num += _devicesTableAdapter.Update(array7);
				allAddedRows.AddRange(array7);
			}
		}
		if (_deviceTrackDefinitionTableAdapter != null)
		{
			DataRow[] array8 = ((DataTable)(object)dataSet.DeviceTrackDefinition).Select((string)null, (string)null, DataViewRowState.Added);
			if (array8 != null && array8.Length != 0)
			{
				num += _deviceTrackDefinitionTableAdapter.Update(array8);
				allAddedRows.AddRange(array8);
			}
		}
		if (_serverGroupTableAdapter != null)
		{
			DataRow[] array9 = ((DataTable)(object)dataSet.ServerGroup).Select((string)null, (string)null, DataViewRowState.Added);
			if (array9 != null && array9.Length != 0)
			{
				num += _serverGroupTableAdapter.Update(array9);
				allAddedRows.AddRange(array9);
			}
		}
		if (_deviceGroupsTableAdapter != null)
		{
			DataRow[] array10 = ((DataTable)(object)dataSet.DeviceGroups).Select((string)null, (string)null, DataViewRowState.Added);
			if (array10 != null && array10.Length != 0)
			{
				num += _deviceGroupsTableAdapter.Update(array10);
				allAddedRows.AddRange(array10);
			}
		}
		if (_deviceTrackTableAdapter != null)
		{
			DataRow[] array11 = ((DataTable)(object)dataSet.DeviceTrack).Select((string)null, (string)null, DataViewRowState.Added);
			if (array11 != null && array11.Length != 0)
			{
				num += _deviceTrackTableAdapter.Update(array11);
				allAddedRows.AddRange(array11);
			}
		}
		if (_recordingStorageTableAdapter != null)
		{
			DataRow[] array12 = ((DataTable)(object)dataSet.RecordingStorage).Select((string)null, (string)null, DataViewRowState.Added);
			if (array12 != null && array12.Length != 0)
			{
				num += _recordingStorageTableAdapter.Update(array12);
				allAddedRows.AddRange(array12);
			}
		}
		if (_recordingServerServiceTableAdapter != null)
		{
			DataRow[] array13 = ((DataTable)(object)dataSet.RecordingServerService).Select((string)null, (string)null, DataViewRowState.Added);
			if (array13 != null && array13.Length != 0)
			{
				num += _recordingServerServiceTableAdapter.Update(array13);
				allAddedRows.AddRange(array13);
			}
		}
		if (_recorderPluginsTableAdapter != null)
		{
			DataRow[] array14 = ((DataTable)(object)dataSet.RecorderPlugins).Select((string)null, (string)null, DataViewRowState.Added);
			if (array14 != null && array14.Length != 0)
			{
				num += _recorderPluginsTableAdapter.Update(array14);
				allAddedRows.AddRange(array14);
			}
		}
		if (_notificationTypesTableAdapter != null)
		{
			DataRow[] array15 = ((DataTable)(object)dataSet.NotificationTypes).Select((string)null, (string)null, DataViewRowState.Added);
			if (array15 != null && array15.Length != 0)
			{
				num += _notificationTypesTableAdapter.Update(array15);
				allAddedRows.AddRange(array15);
			}
		}
		if (_recorderPluginDeviceTableAdapter != null)
		{
			DataRow[] array16 = ((DataTable)(object)dataSet.RecorderPluginDevice).Select((string)null, (string)null, DataViewRowState.Added);
			if (array16 != null && array16.Length != 0)
			{
				num += _recorderPluginDeviceTableAdapter.Update(array16);
				allAddedRows.AddRange(array16);
			}
		}
		if (_patrollingProfilesTableAdapter != null)
		{
			DataRow[] array17 = ((DataTable)(object)dataSet.PatrollingProfiles).Select((string)null, (string)null, DataViewRowState.Added);
			if (array17 != null && array17.Length != 0)
			{
				num += _patrollingProfilesTableAdapter.Update(array17);
				allAddedRows.AddRange(array17);
			}
		}
		if (_recordingServerServiceRecorderTableAdapter != null)
		{
			DataRow[] array18 = ((DataTable)(object)dataSet.RecordingServerServiceRecorder).Select((string)null, (string)null, DataViewRowState.Added);
			if (array18 != null && array18.Length != 0)
			{
				num += _recordingServerServiceRecorderTableAdapter.Update(array18);
				allAddedRows.AddRange(array18);
			}
		}
		if (_notificationSettingsTableAdapter != null)
		{
			DataRow[] array19 = ((DataTable)(object)dataSet.NotificationSettings).Select((string)null, (string)null, DataViewRowState.Added);
			if (array19 != null && array19.Length != 0)
			{
				num += _notificationSettingsTableAdapter.Update(array19);
				allAddedRows.AddRange(array19);
			}
		}
		if (_recordingStorageDeviceTableAdapter != null)
		{
			DataRow[] array20 = ((DataTable)(object)dataSet.RecordingStorageDevice).Select((string)null, (string)null, DataViewRowState.Added);
			if (array20 != null && array20.Length != 0)
			{
				num += _recordingStorageDeviceTableAdapter.Update(array20);
				allAddedRows.AddRange(array20);
			}
		}
		if (_registeredServersTableAdapter != null)
		{
			DataRow[] array21 = ((DataTable)(object)dataSet.RegisteredServers).Select((string)null, (string)null, DataViewRowState.Added);
			if (array21 != null && array21.Length != 0)
			{
				num += _registeredServersTableAdapter.Update(array21);
				allAddedRows.AddRange(array21);
			}
		}
		if (_rulesTableAdapter != null)
		{
			DataRow[] array22 = ((DataTable)(object)dataSet.Rules).Select((string)null, (string)null, DataViewRowState.Added);
			if (array22 != null && array22.Length != 0)
			{
				num += _rulesTableAdapter.Update(array22);
				allAddedRows.AddRange(array22);
			}
		}
		if (_ruleVariablesTableAdapter != null)
		{
			DataRow[] array23 = ((DataTable)(object)dataSet.RuleVariables).Select((string)null, (string)null, DataViewRowState.Added);
			if (array23 != null && array23.Length != 0)
			{
				num += _ruleVariablesTableAdapter.Update(array23);
				allAddedRows.AddRange(array23);
			}
		}
		if (_serverGroupMembershipTableAdapter != null)
		{
			DataRow[] array24 = ((DataTable)(object)dataSet.ServerGroupMembership).Select((string)null, (string)null, DataViewRowState.Added);
			if (array24 != null && array24.Length != 0)
			{
				num += _serverGroupMembershipTableAdapter.Update(array24);
				allAddedRows.AddRange(array24);
			}
		}
		if (_recordingServerServiceConfigurableTableAdapter != null)
		{
			DataRow[] array25 = ((DataTable)(object)dataSet.RecordingServerServiceConfigurable).Select((string)null, (string)null, DataViewRowState.Added);
			if (array25 != null && array25.Length != 0)
			{
				num += _recordingServerServiceConfigurableTableAdapter.Update(array25);
				allAddedRows.AddRange(array25);
			}
		}
		if (_notificationProfilesTableAdapter != null)
		{
			DataRow[] array26 = ((DataTable)(object)dataSet.NotificationProfiles).Select((string)null, (string)null, DataViewRowState.Added);
			if (array26 != null && array26.Length != 0)
			{
				num += _notificationProfilesTableAdapter.Update(array26);
				allAddedRows.AddRange(array26);
			}
		}
		if (_archiveStorageTableAdapter != null)
		{
			DataRow[] array27 = ((DataTable)(object)dataSet.ArchiveStorage).Select((string)null, (string)null, DataViewRowState.Added);
			if (array27 != null && array27.Length != 0)
			{
				num += _archiveStorageTableAdapter.Update(array27);
				allAddedRows.AddRange(array27);
			}
		}
		if (_matrixTableAdapter != null)
		{
			DataRow[] array28 = ((DataTable)(object)dataSet.Matrix).Select((string)null, (string)null, DataViewRowState.Added);
			if (array28 != null && array28.Length != 0)
			{
				num += _matrixTableAdapter.Update(array28);
				allAddedRows.AddRange(array28);
			}
		}
		if (_audioMessageTableAdapter != null)
		{
			DataRow[] array29 = ((DataTable)(object)dataSet.AudioMessage).Select((string)null, (string)null, DataViewRowState.Added);
			if (array29 != null && array29.Length != 0)
			{
				num += _audioMessageTableAdapter.Update(array29);
				allAddedRows.AddRange(array29);
			}
		}
		if (_compressorsTableAdapter != null)
		{
			DataRow[] array30 = ((DataTable)(object)dataSet.Compressors).Select((string)null, (string)null, DataViewRowState.Added);
			if (array30 != null && array30.Length != 0)
			{
				num += _compressorsTableAdapter.Update(array30);
				allAddedRows.AddRange(array30);
			}
		}
		if (_deviceDataDistributionTableAdapter != null)
		{
			DataRow[] array31 = ((DataTable)(object)dataSet.DeviceDataDistribution).Select((string)null, (string)null, DataViewRowState.Added);
			if (array31 != null && array31.Length != 0)
			{
				num += _deviceDataDistributionTableAdapter.Update(array31);
				allAddedRows.AddRange(array31);
			}
		}
		if (_deviceGroupMembersTableAdapter != null)
		{
			DataRow[] array32 = ((DataTable)(object)dataSet.DeviceGroupMembers).Select((string)null, (string)null, DataViewRowState.Added);
			if (array32 != null && array32.Length != 0)
			{
				num += _deviceGroupMembersTableAdapter.Update(array32);
				allAddedRows.AddRange(array32);
			}
		}
		if (_deviceLinksTableAdapter != null)
		{
			DataRow[] array33 = ((DataTable)(object)dataSet.DeviceLinks).Select((string)null, (string)null, DataViewRowState.Added);
			if (array33 != null && array33.Length != 0)
			{
				num += _deviceLinksTableAdapter.Update(array33);
				allAddedRows.AddRange(array33);
			}
		}
		if (_devicePrivacyMaskTableAdapter != null)
		{
			DataRow[] array34 = ((DataTable)(object)dataSet.DevicePrivacyMask).Select((string)null, (string)null, DataViewRowState.Added);
			if (array34 != null && array34.Length != 0)
			{
				num += _devicePrivacyMaskTableAdapter.Update(array34);
				allAddedRows.AddRange(array34);
			}
		}
		if (_driverNumbersTableAdapter != null)
		{
			DataRow[] array35 = ((DataTable)(object)dataSet.DriverNumbers).Select((string)null, (string)null, DataViewRowState.Added);
			if (array35 != null && array35.Length != 0)
			{
				num += _driverNumbersTableAdapter.Update(array35);
				allAddedRows.AddRange(array35);
			}
		}
		if (_eventTypesTableAdapter != null)
		{
			DataRow[] array36 = ((DataTable)(object)dataSet.EventTypes).Select((string)null, (string)null, DataViewRowState.Added);
			if (array36 != null && array36.Length != 0)
			{
				num += _eventTypesTableAdapter.Update(array36);
				allAddedRows.AddRange(array36);
			}
		}
		if (_externalPluginsTableAdapter != null)
		{
			DataRow[] array37 = ((DataTable)(object)dataSet.ExternalPlugins).Select((string)null, (string)null, DataViewRowState.Added);
			if (array37 != null && array37.Length != 0)
			{
				num += _externalPluginsTableAdapter.Update(array37);
				allAddedRows.AddRange(array37);
			}
		}
		if (_hardwareBlockTableAdapter != null)
		{
			DataRow[] array38 = ((DataTable)(object)dataSet.HardwareBlock).Select((string)null, (string)null, DataViewRowState.Added);
			if (array38 != null && array38.Length != 0)
			{
				num += _hardwareBlockTableAdapter.Update(array38);
				allAddedRows.AddRange(array38);
			}
		}
		if (_hardwareDriverRecorderTableAdapter != null)
		{
			DataRow[] array39 = ((DataTable)(object)dataSet.HardwareDriverRecorder).Select((string)null, (string)null, DataViewRowState.Added);
			if (array39 != null && array39.Length != 0)
			{
				num += _hardwareDriverRecorderTableAdapter.Update(array39);
				allAddedRows.AddRange(array39);
			}
		}
		if (_systemSettingsTableAdapter != null)
		{
			DataRow[] array40 = ((DataTable)(object)dataSet.SystemSettings).Select((string)null, (string)null, DataViewRowState.Added);
			if (array40 != null && array40.Length != 0)
			{
				num += _systemSettingsTableAdapter.Update(array40);
				allAddedRows.AddRange(array40);
			}
		}
		if (_licenseTableAdapter != null)
		{
			DataRow[] array41 = ((DataTable)(object)dataSet.License).Select((string)null, (string)null, DataViewRowState.Added);
			if (array41 != null && array41.Length != 0)
			{
				num += _licenseTableAdapter.Update(array41);
				allAddedRows.AddRange(array41);
			}
		}
		if (_mIPObjectTableAdapter != null)
		{
			DataRow[] array42 = ((DataTable)(object)dataSet.MIPObject).Select((string)null, (string)null, DataViewRowState.Added);
			if (array42 != null && array42.Length != 0)
			{
				num += _mIPObjectTableAdapter.Update(array42);
				allAddedRows.AddRange(array42);
			}
		}
		if (_timeProfileTableAdapter != null)
		{
			DataRow[] array43 = ((DataTable)(object)dataSet.TimeProfile).Select((string)null, (string)null, DataViewRowState.Added);
			if (array43 != null && array43.Length != 0)
			{
				num += _timeProfileTableAdapter.Update(array43);
				allAddedRows.AddRange(array43);
			}
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private int UpdateDeletedRows(ConfigurationDataSet dataSet, List<DataRow> allChangedRows)
	{
		int num = 0;
		if (_timeProfileTableAdapter != null)
		{
			DataRow[] array = ((DataTable)(object)dataSet.TimeProfile).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array != null && array.Length != 0)
			{
				num += _timeProfileTableAdapter.Update(array);
				allChangedRows.AddRange(array);
			}
		}
		if (_mIPObjectTableAdapter != null)
		{
			DataRow[] array2 = ((DataTable)(object)dataSet.MIPObject).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array2 != null && array2.Length != 0)
			{
				num += _mIPObjectTableAdapter.Update(array2);
				allChangedRows.AddRange(array2);
			}
		}
		if (_licenseTableAdapter != null)
		{
			DataRow[] array3 = ((DataTable)(object)dataSet.License).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array3 != null && array3.Length != 0)
			{
				num += _licenseTableAdapter.Update(array3);
				allChangedRows.AddRange(array3);
			}
		}
		if (_systemSettingsTableAdapter != null)
		{
			DataRow[] array4 = ((DataTable)(object)dataSet.SystemSettings).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array4 != null && array4.Length != 0)
			{
				num += _systemSettingsTableAdapter.Update(array4);
				allChangedRows.AddRange(array4);
			}
		}
		if (_hardwareDriverRecorderTableAdapter != null)
		{
			DataRow[] array5 = ((DataTable)(object)dataSet.HardwareDriverRecorder).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array5 != null && array5.Length != 0)
			{
				num += _hardwareDriverRecorderTableAdapter.Update(array5);
				allChangedRows.AddRange(array5);
			}
		}
		if (_hardwareBlockTableAdapter != null)
		{
			DataRow[] array6 = ((DataTable)(object)dataSet.HardwareBlock).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array6 != null && array6.Length != 0)
			{
				num += _hardwareBlockTableAdapter.Update(array6);
				allChangedRows.AddRange(array6);
			}
		}
		if (_externalPluginsTableAdapter != null)
		{
			DataRow[] array7 = ((DataTable)(object)dataSet.ExternalPlugins).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array7 != null && array7.Length != 0)
			{
				num += _externalPluginsTableAdapter.Update(array7);
				allChangedRows.AddRange(array7);
			}
		}
		if (_eventTypesTableAdapter != null)
		{
			DataRow[] array8 = ((DataTable)(object)dataSet.EventTypes).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array8 != null && array8.Length != 0)
			{
				num += _eventTypesTableAdapter.Update(array8);
				allChangedRows.AddRange(array8);
			}
		}
		if (_driverNumbersTableAdapter != null)
		{
			DataRow[] array9 = ((DataTable)(object)dataSet.DriverNumbers).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array9 != null && array9.Length != 0)
			{
				num += _driverNumbersTableAdapter.Update(array9);
				allChangedRows.AddRange(array9);
			}
		}
		if (_devicePrivacyMaskTableAdapter != null)
		{
			DataRow[] array10 = ((DataTable)(object)dataSet.DevicePrivacyMask).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array10 != null && array10.Length != 0)
			{
				num += _devicePrivacyMaskTableAdapter.Update(array10);
				allChangedRows.AddRange(array10);
			}
		}
		if (_deviceLinksTableAdapter != null)
		{
			DataRow[] array11 = ((DataTable)(object)dataSet.DeviceLinks).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array11 != null && array11.Length != 0)
			{
				num += _deviceLinksTableAdapter.Update(array11);
				allChangedRows.AddRange(array11);
			}
		}
		if (_deviceGroupMembersTableAdapter != null)
		{
			DataRow[] array12 = ((DataTable)(object)dataSet.DeviceGroupMembers).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array12 != null && array12.Length != 0)
			{
				num += _deviceGroupMembersTableAdapter.Update(array12);
				allChangedRows.AddRange(array12);
			}
		}
		if (_deviceDataDistributionTableAdapter != null)
		{
			DataRow[] array13 = ((DataTable)(object)dataSet.DeviceDataDistribution).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array13 != null && array13.Length != 0)
			{
				num += _deviceDataDistributionTableAdapter.Update(array13);
				allChangedRows.AddRange(array13);
			}
		}
		if (_compressorsTableAdapter != null)
		{
			DataRow[] array14 = ((DataTable)(object)dataSet.Compressors).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array14 != null && array14.Length != 0)
			{
				num += _compressorsTableAdapter.Update(array14);
				allChangedRows.AddRange(array14);
			}
		}
		if (_audioMessageTableAdapter != null)
		{
			DataRow[] array15 = ((DataTable)(object)dataSet.AudioMessage).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array15 != null && array15.Length != 0)
			{
				num += _audioMessageTableAdapter.Update(array15);
				allChangedRows.AddRange(array15);
			}
		}
		if (_matrixTableAdapter != null)
		{
			DataRow[] array16 = ((DataTable)(object)dataSet.Matrix).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array16 != null && array16.Length != 0)
			{
				num += _matrixTableAdapter.Update(array16);
				allChangedRows.AddRange(array16);
			}
		}
		if (_archiveStorageTableAdapter != null)
		{
			DataRow[] array17 = ((DataTable)(object)dataSet.ArchiveStorage).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array17 != null && array17.Length != 0)
			{
				num += _archiveStorageTableAdapter.Update(array17);
				allChangedRows.AddRange(array17);
			}
		}
		if (_notificationProfilesTableAdapter != null)
		{
			DataRow[] array18 = ((DataTable)(object)dataSet.NotificationProfiles).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array18 != null && array18.Length != 0)
			{
				num += _notificationProfilesTableAdapter.Update(array18);
				allChangedRows.AddRange(array18);
			}
		}
		if (_recordingServerServiceConfigurableTableAdapter != null)
		{
			DataRow[] array19 = ((DataTable)(object)dataSet.RecordingServerServiceConfigurable).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array19 != null && array19.Length != 0)
			{
				num += _recordingServerServiceConfigurableTableAdapter.Update(array19);
				allChangedRows.AddRange(array19);
			}
		}
		if (_serverGroupMembershipTableAdapter != null)
		{
			DataRow[] array20 = ((DataTable)(object)dataSet.ServerGroupMembership).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array20 != null && array20.Length != 0)
			{
				num += _serverGroupMembershipTableAdapter.Update(array20);
				allChangedRows.AddRange(array20);
			}
		}
		if (_ruleVariablesTableAdapter != null)
		{
			DataRow[] array21 = ((DataTable)(object)dataSet.RuleVariables).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array21 != null && array21.Length != 0)
			{
				num += _ruleVariablesTableAdapter.Update(array21);
				allChangedRows.AddRange(array21);
			}
		}
		if (_rulesTableAdapter != null)
		{
			DataRow[] array22 = ((DataTable)(object)dataSet.Rules).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array22 != null && array22.Length != 0)
			{
				num += _rulesTableAdapter.Update(array22);
				allChangedRows.AddRange(array22);
			}
		}
		if (_registeredServersTableAdapter != null)
		{
			DataRow[] array23 = ((DataTable)(object)dataSet.RegisteredServers).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array23 != null && array23.Length != 0)
			{
				num += _registeredServersTableAdapter.Update(array23);
				allChangedRows.AddRange(array23);
			}
		}
		if (_recordingStorageDeviceTableAdapter != null)
		{
			DataRow[] array24 = ((DataTable)(object)dataSet.RecordingStorageDevice).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array24 != null && array24.Length != 0)
			{
				num += _recordingStorageDeviceTableAdapter.Update(array24);
				allChangedRows.AddRange(array24);
			}
		}
		if (_notificationSettingsTableAdapter != null)
		{
			DataRow[] array25 = ((DataTable)(object)dataSet.NotificationSettings).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array25 != null && array25.Length != 0)
			{
				num += _notificationSettingsTableAdapter.Update(array25);
				allChangedRows.AddRange(array25);
			}
		}
		if (_recordingServerServiceRecorderTableAdapter != null)
		{
			DataRow[] array26 = ((DataTable)(object)dataSet.RecordingServerServiceRecorder).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array26 != null && array26.Length != 0)
			{
				num += _recordingServerServiceRecorderTableAdapter.Update(array26);
				allChangedRows.AddRange(array26);
			}
		}
		if (_patrollingProfilesTableAdapter != null)
		{
			DataRow[] array27 = ((DataTable)(object)dataSet.PatrollingProfiles).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array27 != null && array27.Length != 0)
			{
				num += _patrollingProfilesTableAdapter.Update(array27);
				allChangedRows.AddRange(array27);
			}
		}
		if (_recorderPluginDeviceTableAdapter != null)
		{
			DataRow[] array28 = ((DataTable)(object)dataSet.RecorderPluginDevice).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array28 != null && array28.Length != 0)
			{
				num += _recorderPluginDeviceTableAdapter.Update(array28);
				allChangedRows.AddRange(array28);
			}
		}
		if (_notificationTypesTableAdapter != null)
		{
			DataRow[] array29 = ((DataTable)(object)dataSet.NotificationTypes).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array29 != null && array29.Length != 0)
			{
				num += _notificationTypesTableAdapter.Update(array29);
				allChangedRows.AddRange(array29);
			}
		}
		if (_recorderPluginsTableAdapter != null)
		{
			DataRow[] array30 = ((DataTable)(object)dataSet.RecorderPlugins).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array30 != null && array30.Length != 0)
			{
				num += _recorderPluginsTableAdapter.Update(array30);
				allChangedRows.AddRange(array30);
			}
		}
		if (_recordingServerServiceTableAdapter != null)
		{
			DataRow[] array31 = ((DataTable)(object)dataSet.RecordingServerService).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array31 != null && array31.Length != 0)
			{
				num += _recordingServerServiceTableAdapter.Update(array31);
				allChangedRows.AddRange(array31);
			}
		}
		if (_recordingStorageTableAdapter != null)
		{
			DataRow[] array32 = ((DataTable)(object)dataSet.RecordingStorage).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array32 != null && array32.Length != 0)
			{
				num += _recordingStorageTableAdapter.Update(array32);
				allChangedRows.AddRange(array32);
			}
		}
		if (_deviceTrackTableAdapter != null)
		{
			DataRow[] array33 = ((DataTable)(object)dataSet.DeviceTrack).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array33 != null && array33.Length != 0)
			{
				num += _deviceTrackTableAdapter.Update(array33);
				allChangedRows.AddRange(array33);
			}
		}
		if (_deviceGroupsTableAdapter != null)
		{
			DataRow[] array34 = ((DataTable)(object)dataSet.DeviceGroups).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array34 != null && array34.Length != 0)
			{
				num += _deviceGroupsTableAdapter.Update(array34);
				allChangedRows.AddRange(array34);
			}
		}
		if (_serverGroupTableAdapter != null)
		{
			DataRow[] array35 = ((DataTable)(object)dataSet.ServerGroup).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array35 != null && array35.Length != 0)
			{
				num += _serverGroupTableAdapter.Update(array35);
				allChangedRows.AddRange(array35);
			}
		}
		if (_deviceTrackDefinitionTableAdapter != null)
		{
			DataRow[] array36 = ((DataTable)(object)dataSet.DeviceTrackDefinition).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array36 != null && array36.Length != 0)
			{
				num += _deviceTrackDefinitionTableAdapter.Update(array36);
				allChangedRows.AddRange(array36);
			}
		}
		if (_devicesTableAdapter != null)
		{
			DataRow[] array37 = ((DataTable)(object)dataSet.Devices).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array37 != null && array37.Length != 0)
			{
				num += _devicesTableAdapter.Update(array37);
				allChangedRows.AddRange(array37);
			}
		}
		if (_hardwareTableAdapter != null)
		{
			DataRow[] array38 = ((DataTable)(object)dataSet.Hardware).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array38 != null && array38.Length != 0)
			{
				num += _hardwareTableAdapter.Update(array38);
				allChangedRows.AddRange(array38);
			}
		}
		if (_registeredDeviceTypesTableAdapter != null)
		{
			DataRow[] array39 = ((DataTable)(object)dataSet.RegisteredDeviceTypes).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array39 != null && array39.Length != 0)
			{
				num += _registeredDeviceTypesTableAdapter.Update(array39);
				allChangedRows.AddRange(array39);
			}
		}
		if (_hardwareDefinitionTableAdapter != null)
		{
			DataRow[] array40 = ((DataTable)(object)dataSet.HardwareDefinition).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array40 != null && array40.Length != 0)
			{
				num += _hardwareDefinitionTableAdapter.Update(array40);
				allChangedRows.AddRange(array40);
			}
		}
		if (_hostedHardwareTableAdapter != null)
		{
			DataRow[] array41 = ((DataTable)(object)dataSet.HostedHardware).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array41 != null && array41.Length != 0)
			{
				num += _hostedHardwareTableAdapter.Update(array41);
				allChangedRows.AddRange(array41);
			}
		}
		if (_recordersTableAdapter != null)
		{
			DataRow[] array42 = ((DataTable)(object)dataSet.Recorders).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array42 != null && array42.Length != 0)
			{
				num += _recordersTableAdapter.Update(array42);
				allChangedRows.AddRange(array42);
			}
		}
		if (_hostingServerTableAdapter != null)
		{
			DataRow[] array43 = ((DataTable)(object)dataSet.HostingServer).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array43 != null && array43.Length != 0)
			{
				num += _hostingServerTableAdapter.Update(array43);
				allChangedRows.AddRange(array43);
			}
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private DataRow[] GetRealUpdatedRows(DataRow[] updatedRows, List<DataRow> allAddedRows)
	{
		if (updatedRows == null || updatedRows.Length < 1)
		{
			return updatedRows;
		}
		if (allAddedRows == null || allAddedRows.Count < 1)
		{
			return updatedRows;
		}
		List<DataRow> list = new List<DataRow>();
		foreach (DataRow item in updatedRows)
		{
			if (!allAddedRows.Contains(item))
			{
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public virtual int UpdateAll(ConfigurationDataSet dataSet)
	{
		if (dataSet == null)
		{
			throw new ArgumentNullException("dataSet");
		}
		if (!dataSet.HasChanges())
		{
			return 0;
		}
		if (_archiveStorageTableAdapter != null && !MatchTableAdapterConnection(_archiveStorageTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_audioMessageTableAdapter != null && !MatchTableAdapterConnection(_audioMessageTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_compressorsTableAdapter != null && !MatchTableAdapterConnection(_compressorsTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_deviceDataDistributionTableAdapter != null && !MatchTableAdapterConnection(_deviceDataDistributionTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_deviceGroupMembersTableAdapter != null && !MatchTableAdapterConnection(_deviceGroupMembersTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_deviceGroupsTableAdapter != null && !MatchTableAdapterConnection(_deviceGroupsTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_deviceLinksTableAdapter != null && !MatchTableAdapterConnection(_deviceLinksTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_devicePrivacyMaskTableAdapter != null && !MatchTableAdapterConnection(_devicePrivacyMaskTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_devicesTableAdapter != null && !MatchTableAdapterConnection(_devicesTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_deviceTrackDefinitionTableAdapter != null && !MatchTableAdapterConnection(_deviceTrackDefinitionTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_deviceTrackTableAdapter != null && !MatchTableAdapterConnection(_deviceTrackTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_driverNumbersTableAdapter != null && !MatchTableAdapterConnection(_driverNumbersTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_eventTypesTableAdapter != null && !MatchTableAdapterConnection(_eventTypesTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_externalPluginsTableAdapter != null && !MatchTableAdapterConnection(_externalPluginsTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_hardwareBlockTableAdapter != null && !MatchTableAdapterConnection(_hardwareBlockTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_hardwareDefinitionTableAdapter != null && !MatchTableAdapterConnection(_hardwareDefinitionTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_hardwareDriverRecorderTableAdapter != null && !MatchTableAdapterConnection(_hardwareDriverRecorderTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_hardwareTableAdapter != null && !MatchTableAdapterConnection(_hardwareTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_hostedHardwareTableAdapter != null && !MatchTableAdapterConnection(_hostedHardwareTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_hostingServerTableAdapter != null && !MatchTableAdapterConnection(_hostingServerTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_licenseTableAdapter != null && !MatchTableAdapterConnection(_licenseTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_matrixTableAdapter != null && !MatchTableAdapterConnection(_matrixTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_mIPObjectTableAdapter != null && !MatchTableAdapterConnection(_mIPObjectTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_notificationProfilesTableAdapter != null && !MatchTableAdapterConnection(_notificationProfilesTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_notificationSettingsTableAdapter != null && !MatchTableAdapterConnection(_notificationSettingsTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_notificationTypesTableAdapter != null && !MatchTableAdapterConnection(_notificationTypesTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_patrollingProfilesTableAdapter != null && !MatchTableAdapterConnection(_patrollingProfilesTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_recorderPluginDeviceTableAdapter != null && !MatchTableAdapterConnection(_recorderPluginDeviceTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_recorderPluginsTableAdapter != null && !MatchTableAdapterConnection(_recorderPluginsTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_recordersTableAdapter != null && !MatchTableAdapterConnection(_recordersTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_recordingServerServiceConfigurableTableAdapter != null && !MatchTableAdapterConnection(_recordingServerServiceConfigurableTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_recordingServerServiceRecorderTableAdapter != null && !MatchTableAdapterConnection(_recordingServerServiceRecorderTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_recordingServerServiceTableAdapter != null && !MatchTableAdapterConnection(_recordingServerServiceTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_recordingStorageDeviceTableAdapter != null && !MatchTableAdapterConnection(_recordingStorageDeviceTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_recordingStorageTableAdapter != null && !MatchTableAdapterConnection(_recordingStorageTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_registeredDeviceTypesTableAdapter != null && !MatchTableAdapterConnection(_registeredDeviceTypesTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_registeredServersTableAdapter != null && !MatchTableAdapterConnection(_registeredServersTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_rulesTableAdapter != null && !MatchTableAdapterConnection(_rulesTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_ruleVariablesTableAdapter != null && !MatchTableAdapterConnection(_ruleVariablesTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_serverGroupMembershipTableAdapter != null && !MatchTableAdapterConnection(_serverGroupMembershipTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_serverGroupTableAdapter != null && !MatchTableAdapterConnection(_serverGroupTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_systemSettingsTableAdapter != null && !MatchTableAdapterConnection(_systemSettingsTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_timeProfileTableAdapter != null && !MatchTableAdapterConnection(_timeProfileTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		IDbConnection connection = Connection;
		if (connection == null)
		{
			throw new ApplicationException("TableAdapterManager contains no connection information. Set each TableAdapterManager TableAdapter property to a valid TableAdapter instance.");
		}
		bool flag = false;
		if ((connection.State & ConnectionState.Broken) == ConnectionState.Broken)
		{
			connection.Close();
		}
		if (connection.State == ConnectionState.Closed)
		{
			connection.Open();
			flag = true;
		}
		IDbTransaction dbTransaction = connection.BeginTransaction();
		if (dbTransaction == null)
		{
			throw new ApplicationException("The transaction cannot begin. The current data connection does not support transactions or the current state is not allowing the transaction to begin.");
		}
		List<DataRow> list = new List<DataRow>();
		List<DataRow> list2 = new List<DataRow>();
		List<DataAdapter> list3 = new List<DataAdapter>();
		Dictionary<object, IDbConnection> dictionary = new Dictionary<object, IDbConnection>();
		int num = 0;
		DataSet dataSet2 = null;
		if (BackupDataSetBeforeUpdate)
		{
			dataSet2 = new DataSet();
			dataSet2.Merge(dataSet);
		}
		try
		{
			if (_archiveStorageTableAdapter != null)
			{
				dictionary.Add(_archiveStorageTableAdapter, _archiveStorageTableAdapter.Connection);
				_archiveStorageTableAdapter.Connection = (SqlConnection)connection;
				_archiveStorageTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_archiveStorageTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_archiveStorageTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_archiveStorageTableAdapter.Adapter);
				}
			}
			if (_audioMessageTableAdapter != null)
			{
				dictionary.Add(_audioMessageTableAdapter, _audioMessageTableAdapter.Connection);
				_audioMessageTableAdapter.Connection = (SqlConnection)connection;
				_audioMessageTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_audioMessageTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_audioMessageTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_audioMessageTableAdapter.Adapter);
				}
			}
			if (_compressorsTableAdapter != null)
			{
				dictionary.Add(_compressorsTableAdapter, _compressorsTableAdapter.Connection);
				_compressorsTableAdapter.Connection = (SqlConnection)connection;
				_compressorsTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_compressorsTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_compressorsTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_compressorsTableAdapter.Adapter);
				}
			}
			if (_deviceDataDistributionTableAdapter != null)
			{
				dictionary.Add(_deviceDataDistributionTableAdapter, _deviceDataDistributionTableAdapter.Connection);
				_deviceDataDistributionTableAdapter.Connection = (SqlConnection)connection;
				_deviceDataDistributionTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_deviceDataDistributionTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_deviceDataDistributionTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_deviceDataDistributionTableAdapter.Adapter);
				}
			}
			if (_deviceGroupMembersTableAdapter != null)
			{
				dictionary.Add(_deviceGroupMembersTableAdapter, _deviceGroupMembersTableAdapter.Connection);
				_deviceGroupMembersTableAdapter.Connection = (SqlConnection)connection;
				_deviceGroupMembersTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_deviceGroupMembersTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_deviceGroupMembersTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_deviceGroupMembersTableAdapter.Adapter);
				}
			}
			if (_deviceGroupsTableAdapter != null)
			{
				dictionary.Add(_deviceGroupsTableAdapter, _deviceGroupsTableAdapter.Connection);
				_deviceGroupsTableAdapter.Connection = (SqlConnection)connection;
				_deviceGroupsTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_deviceGroupsTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_deviceGroupsTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_deviceGroupsTableAdapter.Adapter);
				}
			}
			if (_deviceLinksTableAdapter != null)
			{
				dictionary.Add(_deviceLinksTableAdapter, _deviceLinksTableAdapter.Connection);
				_deviceLinksTableAdapter.Connection = (SqlConnection)connection;
				_deviceLinksTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_deviceLinksTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_deviceLinksTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_deviceLinksTableAdapter.Adapter);
				}
			}
			if (_devicePrivacyMaskTableAdapter != null)
			{
				dictionary.Add(_devicePrivacyMaskTableAdapter, _devicePrivacyMaskTableAdapter.Connection);
				_devicePrivacyMaskTableAdapter.Connection = (SqlConnection)connection;
				_devicePrivacyMaskTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_devicePrivacyMaskTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_devicePrivacyMaskTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_devicePrivacyMaskTableAdapter.Adapter);
				}
			}
			if (_devicesTableAdapter != null)
			{
				dictionary.Add(_devicesTableAdapter, _devicesTableAdapter.Connection);
				_devicesTableAdapter.Connection = (SqlConnection)connection;
				_devicesTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_devicesTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_devicesTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_devicesTableAdapter.Adapter);
				}
			}
			if (_deviceTrackDefinitionTableAdapter != null)
			{
				dictionary.Add(_deviceTrackDefinitionTableAdapter, _deviceTrackDefinitionTableAdapter.Connection);
				_deviceTrackDefinitionTableAdapter.Connection = (SqlConnection)connection;
				_deviceTrackDefinitionTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_deviceTrackDefinitionTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_deviceTrackDefinitionTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_deviceTrackDefinitionTableAdapter.Adapter);
				}
			}
			if (_deviceTrackTableAdapter != null)
			{
				dictionary.Add(_deviceTrackTableAdapter, _deviceTrackTableAdapter.Connection);
				_deviceTrackTableAdapter.Connection = (SqlConnection)connection;
				_deviceTrackTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_deviceTrackTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_deviceTrackTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_deviceTrackTableAdapter.Adapter);
				}
			}
			if (_driverNumbersTableAdapter != null)
			{
				dictionary.Add(_driverNumbersTableAdapter, _driverNumbersTableAdapter.Connection);
				_driverNumbersTableAdapter.Connection = (SqlConnection)connection;
				_driverNumbersTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_driverNumbersTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_driverNumbersTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_driverNumbersTableAdapter.Adapter);
				}
			}
			if (_eventTypesTableAdapter != null)
			{
				dictionary.Add(_eventTypesTableAdapter, _eventTypesTableAdapter.Connection);
				_eventTypesTableAdapter.Connection = (SqlConnection)connection;
				_eventTypesTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_eventTypesTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_eventTypesTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_eventTypesTableAdapter.Adapter);
				}
			}
			if (_externalPluginsTableAdapter != null)
			{
				dictionary.Add(_externalPluginsTableAdapter, _externalPluginsTableAdapter.Connection);
				_externalPluginsTableAdapter.Connection = (SqlConnection)connection;
				_externalPluginsTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_externalPluginsTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_externalPluginsTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_externalPluginsTableAdapter.Adapter);
				}
			}
			if (_hardwareBlockTableAdapter != null)
			{
				dictionary.Add(_hardwareBlockTableAdapter, _hardwareBlockTableAdapter.Connection);
				_hardwareBlockTableAdapter.Connection = (SqlConnection)connection;
				_hardwareBlockTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_hardwareBlockTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_hardwareBlockTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_hardwareBlockTableAdapter.Adapter);
				}
			}
			if (_hardwareDefinitionTableAdapter != null)
			{
				dictionary.Add(_hardwareDefinitionTableAdapter, _hardwareDefinitionTableAdapter.Connection);
				_hardwareDefinitionTableAdapter.Connection = (SqlConnection)connection;
				_hardwareDefinitionTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_hardwareDefinitionTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_hardwareDefinitionTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_hardwareDefinitionTableAdapter.Adapter);
				}
			}
			if (_hardwareDriverRecorderTableAdapter != null)
			{
				dictionary.Add(_hardwareDriverRecorderTableAdapter, _hardwareDriverRecorderTableAdapter.Connection);
				_hardwareDriverRecorderTableAdapter.Connection = (SqlConnection)connection;
				_hardwareDriverRecorderTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_hardwareDriverRecorderTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_hardwareDriverRecorderTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_hardwareDriverRecorderTableAdapter.Adapter);
				}
			}
			if (_hardwareTableAdapter != null)
			{
				dictionary.Add(_hardwareTableAdapter, _hardwareTableAdapter.Connection);
				_hardwareTableAdapter.Connection = (SqlConnection)connection;
				_hardwareTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_hardwareTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_hardwareTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_hardwareTableAdapter.Adapter);
				}
			}
			if (_hostedHardwareTableAdapter != null)
			{
				dictionary.Add(_hostedHardwareTableAdapter, _hostedHardwareTableAdapter.Connection);
				_hostedHardwareTableAdapter.Connection = (SqlConnection)connection;
				_hostedHardwareTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_hostedHardwareTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_hostedHardwareTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_hostedHardwareTableAdapter.Adapter);
				}
			}
			if (_hostingServerTableAdapter != null)
			{
				dictionary.Add(_hostingServerTableAdapter, _hostingServerTableAdapter.Connection);
				_hostingServerTableAdapter.Connection = (SqlConnection)connection;
				_hostingServerTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_hostingServerTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_hostingServerTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_hostingServerTableAdapter.Adapter);
				}
			}
			if (_licenseTableAdapter != null)
			{
				dictionary.Add(_licenseTableAdapter, _licenseTableAdapter.Connection);
				_licenseTableAdapter.Connection = (SqlConnection)connection;
				_licenseTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_licenseTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_licenseTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_licenseTableAdapter.Adapter);
				}
			}
			if (_matrixTableAdapter != null)
			{
				dictionary.Add(_matrixTableAdapter, _matrixTableAdapter.Connection);
				_matrixTableAdapter.Connection = (SqlConnection)connection;
				_matrixTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_matrixTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_matrixTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_matrixTableAdapter.Adapter);
				}
			}
			if (_mIPObjectTableAdapter != null)
			{
				dictionary.Add(_mIPObjectTableAdapter, _mIPObjectTableAdapter.Connection);
				_mIPObjectTableAdapter.Connection = (SqlConnection)connection;
				_mIPObjectTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_mIPObjectTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_mIPObjectTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_mIPObjectTableAdapter.Adapter);
				}
			}
			if (_notificationProfilesTableAdapter != null)
			{
				dictionary.Add(_notificationProfilesTableAdapter, _notificationProfilesTableAdapter.Connection);
				_notificationProfilesTableAdapter.Connection = (SqlConnection)connection;
				_notificationProfilesTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_notificationProfilesTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_notificationProfilesTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_notificationProfilesTableAdapter.Adapter);
				}
			}
			if (_notificationSettingsTableAdapter != null)
			{
				dictionary.Add(_notificationSettingsTableAdapter, _notificationSettingsTableAdapter.Connection);
				_notificationSettingsTableAdapter.Connection = (SqlConnection)connection;
				_notificationSettingsTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_notificationSettingsTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_notificationSettingsTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_notificationSettingsTableAdapter.Adapter);
				}
			}
			if (_notificationTypesTableAdapter != null)
			{
				dictionary.Add(_notificationTypesTableAdapter, _notificationTypesTableAdapter.Connection);
				_notificationTypesTableAdapter.Connection = (SqlConnection)connection;
				_notificationTypesTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_notificationTypesTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_notificationTypesTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_notificationTypesTableAdapter.Adapter);
				}
			}
			if (_patrollingProfilesTableAdapter != null)
			{
				dictionary.Add(_patrollingProfilesTableAdapter, _patrollingProfilesTableAdapter.Connection);
				_patrollingProfilesTableAdapter.Connection = (SqlConnection)connection;
				_patrollingProfilesTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_patrollingProfilesTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_patrollingProfilesTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_patrollingProfilesTableAdapter.Adapter);
				}
			}
			if (_recorderPluginDeviceTableAdapter != null)
			{
				dictionary.Add(_recorderPluginDeviceTableAdapter, _recorderPluginDeviceTableAdapter.Connection);
				_recorderPluginDeviceTableAdapter.Connection = (SqlConnection)connection;
				_recorderPluginDeviceTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_recorderPluginDeviceTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_recorderPluginDeviceTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_recorderPluginDeviceTableAdapter.Adapter);
				}
			}
			if (_recorderPluginsTableAdapter != null)
			{
				dictionary.Add(_recorderPluginsTableAdapter, _recorderPluginsTableAdapter.Connection);
				_recorderPluginsTableAdapter.Connection = (SqlConnection)connection;
				_recorderPluginsTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_recorderPluginsTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_recorderPluginsTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_recorderPluginsTableAdapter.Adapter);
				}
			}
			if (_recordersTableAdapter != null)
			{
				dictionary.Add(_recordersTableAdapter, _recordersTableAdapter.Connection);
				_recordersTableAdapter.Connection = (SqlConnection)connection;
				_recordersTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_recordersTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_recordersTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_recordersTableAdapter.Adapter);
				}
			}
			if (_recordingServerServiceConfigurableTableAdapter != null)
			{
				dictionary.Add(_recordingServerServiceConfigurableTableAdapter, _recordingServerServiceConfigurableTableAdapter.Connection);
				_recordingServerServiceConfigurableTableAdapter.Connection = (SqlConnection)connection;
				_recordingServerServiceConfigurableTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_recordingServerServiceConfigurableTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_recordingServerServiceConfigurableTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_recordingServerServiceConfigurableTableAdapter.Adapter);
				}
			}
			if (_recordingServerServiceRecorderTableAdapter != null)
			{
				dictionary.Add(_recordingServerServiceRecorderTableAdapter, _recordingServerServiceRecorderTableAdapter.Connection);
				_recordingServerServiceRecorderTableAdapter.Connection = (SqlConnection)connection;
				_recordingServerServiceRecorderTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_recordingServerServiceRecorderTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_recordingServerServiceRecorderTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_recordingServerServiceRecorderTableAdapter.Adapter);
				}
			}
			if (_recordingServerServiceTableAdapter != null)
			{
				dictionary.Add(_recordingServerServiceTableAdapter, _recordingServerServiceTableAdapter.Connection);
				_recordingServerServiceTableAdapter.Connection = (SqlConnection)connection;
				_recordingServerServiceTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_recordingServerServiceTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_recordingServerServiceTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_recordingServerServiceTableAdapter.Adapter);
				}
			}
			if (_recordingStorageDeviceTableAdapter != null)
			{
				dictionary.Add(_recordingStorageDeviceTableAdapter, _recordingStorageDeviceTableAdapter.Connection);
				_recordingStorageDeviceTableAdapter.Connection = (SqlConnection)connection;
				_recordingStorageDeviceTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_recordingStorageDeviceTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_recordingStorageDeviceTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_recordingStorageDeviceTableAdapter.Adapter);
				}
			}
			if (_recordingStorageTableAdapter != null)
			{
				dictionary.Add(_recordingStorageTableAdapter, _recordingStorageTableAdapter.Connection);
				_recordingStorageTableAdapter.Connection = (SqlConnection)connection;
				_recordingStorageTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_recordingStorageTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_recordingStorageTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_recordingStorageTableAdapter.Adapter);
				}
			}
			if (_registeredDeviceTypesTableAdapter != null)
			{
				dictionary.Add(_registeredDeviceTypesTableAdapter, _registeredDeviceTypesTableAdapter.Connection);
				_registeredDeviceTypesTableAdapter.Connection = (SqlConnection)connection;
				_registeredDeviceTypesTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_registeredDeviceTypesTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_registeredDeviceTypesTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_registeredDeviceTypesTableAdapter.Adapter);
				}
			}
			if (_registeredServersTableAdapter != null)
			{
				dictionary.Add(_registeredServersTableAdapter, _registeredServersTableAdapter.Connection);
				_registeredServersTableAdapter.Connection = (SqlConnection)connection;
				_registeredServersTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_registeredServersTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_registeredServersTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_registeredServersTableAdapter.Adapter);
				}
			}
			if (_rulesTableAdapter != null)
			{
				dictionary.Add(_rulesTableAdapter, _rulesTableAdapter.Connection);
				_rulesTableAdapter.Connection = (SqlConnection)connection;
				_rulesTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_rulesTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_rulesTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_rulesTableAdapter.Adapter);
				}
			}
			if (_ruleVariablesTableAdapter != null)
			{
				dictionary.Add(_ruleVariablesTableAdapter, _ruleVariablesTableAdapter.Connection);
				_ruleVariablesTableAdapter.Connection = (SqlConnection)connection;
				_ruleVariablesTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_ruleVariablesTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_ruleVariablesTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_ruleVariablesTableAdapter.Adapter);
				}
			}
			if (_serverGroupMembershipTableAdapter != null)
			{
				dictionary.Add(_serverGroupMembershipTableAdapter, _serverGroupMembershipTableAdapter.Connection);
				_serverGroupMembershipTableAdapter.Connection = (SqlConnection)connection;
				_serverGroupMembershipTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_serverGroupMembershipTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_serverGroupMembershipTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_serverGroupMembershipTableAdapter.Adapter);
				}
			}
			if (_serverGroupTableAdapter != null)
			{
				dictionary.Add(_serverGroupTableAdapter, _serverGroupTableAdapter.Connection);
				_serverGroupTableAdapter.Connection = (SqlConnection)connection;
				_serverGroupTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_serverGroupTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_serverGroupTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_serverGroupTableAdapter.Adapter);
				}
			}
			if (_systemSettingsTableAdapter != null)
			{
				dictionary.Add(_systemSettingsTableAdapter, _systemSettingsTableAdapter.Connection);
				_systemSettingsTableAdapter.Connection = (SqlConnection)connection;
				_systemSettingsTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_systemSettingsTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_systemSettingsTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_systemSettingsTableAdapter.Adapter);
				}
			}
			if (_timeProfileTableAdapter != null)
			{
				dictionary.Add(_timeProfileTableAdapter, _timeProfileTableAdapter.Connection);
				_timeProfileTableAdapter.Connection = (SqlConnection)connection;
				_timeProfileTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_timeProfileTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_timeProfileTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_timeProfileTableAdapter.Adapter);
				}
			}
			if (UpdateOrder == UpdateOrderOption.UpdateInsertDelete)
			{
				num += UpdateUpdatedRows(dataSet, list, list2);
				num += UpdateInsertedRows(dataSet, list2);
			}
			else
			{
				num += UpdateInsertedRows(dataSet, list2);
				num += UpdateUpdatedRows(dataSet, list, list2);
			}
			num += UpdateDeletedRows(dataSet, list);
			dbTransaction.Commit();
			if (0 < list2.Count)
			{
				DataRow[] array = new DataRow[list2.Count];
				list2.CopyTo(array);
				for (int i = 0; i < array.Length; i++)
				{
					array[i].AcceptChanges();
				}
			}
			if (0 < list.Count)
			{
				DataRow[] array2 = new DataRow[list.Count];
				list.CopyTo(array2);
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].AcceptChanges();
				}
			}
		}
		catch (Exception ex)
		{
			dbTransaction.Rollback();
			if (BackupDataSetBeforeUpdate)
			{
				dataSet.Clear();
				dataSet.Merge(dataSet2);
			}
			else if (0 < list2.Count)
			{
				DataRow[] array3 = new DataRow[list2.Count];
				list2.CopyTo(array3);
				foreach (DataRow obj in array3)
				{
					obj.AcceptChanges();
					obj.SetAdded();
				}
			}
			throw ex;
		}
		finally
		{
			if (flag)
			{
				connection.Close();
			}
			if (_archiveStorageTableAdapter != null)
			{
				_archiveStorageTableAdapter.Connection = (SqlConnection)dictionary[_archiveStorageTableAdapter];
				_archiveStorageTableAdapter.Transaction = null;
			}
			if (_audioMessageTableAdapter != null)
			{
				_audioMessageTableAdapter.Connection = (SqlConnection)dictionary[_audioMessageTableAdapter];
				_audioMessageTableAdapter.Transaction = null;
			}
			if (_compressorsTableAdapter != null)
			{
				_compressorsTableAdapter.Connection = (SqlConnection)dictionary[_compressorsTableAdapter];
				_compressorsTableAdapter.Transaction = null;
			}
			if (_deviceDataDistributionTableAdapter != null)
			{
				_deviceDataDistributionTableAdapter.Connection = (SqlConnection)dictionary[_deviceDataDistributionTableAdapter];
				_deviceDataDistributionTableAdapter.Transaction = null;
			}
			if (_deviceGroupMembersTableAdapter != null)
			{
				_deviceGroupMembersTableAdapter.Connection = (SqlConnection)dictionary[_deviceGroupMembersTableAdapter];
				_deviceGroupMembersTableAdapter.Transaction = null;
			}
			if (_deviceGroupsTableAdapter != null)
			{
				_deviceGroupsTableAdapter.Connection = (SqlConnection)dictionary[_deviceGroupsTableAdapter];
				_deviceGroupsTableAdapter.Transaction = null;
			}
			if (_deviceLinksTableAdapter != null)
			{
				_deviceLinksTableAdapter.Connection = (SqlConnection)dictionary[_deviceLinksTableAdapter];
				_deviceLinksTableAdapter.Transaction = null;
			}
			if (_devicePrivacyMaskTableAdapter != null)
			{
				_devicePrivacyMaskTableAdapter.Connection = (SqlConnection)dictionary[_devicePrivacyMaskTableAdapter];
				_devicePrivacyMaskTableAdapter.Transaction = null;
			}
			if (_devicesTableAdapter != null)
			{
				_devicesTableAdapter.Connection = (SqlConnection)dictionary[_devicesTableAdapter];
				_devicesTableAdapter.Transaction = null;
			}
			if (_deviceTrackDefinitionTableAdapter != null)
			{
				_deviceTrackDefinitionTableAdapter.Connection = (SqlConnection)dictionary[_deviceTrackDefinitionTableAdapter];
				_deviceTrackDefinitionTableAdapter.Transaction = null;
			}
			if (_deviceTrackTableAdapter != null)
			{
				_deviceTrackTableAdapter.Connection = (SqlConnection)dictionary[_deviceTrackTableAdapter];
				_deviceTrackTableAdapter.Transaction = null;
			}
			if (_driverNumbersTableAdapter != null)
			{
				_driverNumbersTableAdapter.Connection = (SqlConnection)dictionary[_driverNumbersTableAdapter];
				_driverNumbersTableAdapter.Transaction = null;
			}
			if (_eventTypesTableAdapter != null)
			{
				_eventTypesTableAdapter.Connection = (SqlConnection)dictionary[_eventTypesTableAdapter];
				_eventTypesTableAdapter.Transaction = null;
			}
			if (_externalPluginsTableAdapter != null)
			{
				_externalPluginsTableAdapter.Connection = (SqlConnection)dictionary[_externalPluginsTableAdapter];
				_externalPluginsTableAdapter.Transaction = null;
			}
			if (_hardwareBlockTableAdapter != null)
			{
				_hardwareBlockTableAdapter.Connection = (SqlConnection)dictionary[_hardwareBlockTableAdapter];
				_hardwareBlockTableAdapter.Transaction = null;
			}
			if (_hardwareDefinitionTableAdapter != null)
			{
				_hardwareDefinitionTableAdapter.Connection = (SqlConnection)dictionary[_hardwareDefinitionTableAdapter];
				_hardwareDefinitionTableAdapter.Transaction = null;
			}
			if (_hardwareDriverRecorderTableAdapter != null)
			{
				_hardwareDriverRecorderTableAdapter.Connection = (SqlConnection)dictionary[_hardwareDriverRecorderTableAdapter];
				_hardwareDriverRecorderTableAdapter.Transaction = null;
			}
			if (_hardwareTableAdapter != null)
			{
				_hardwareTableAdapter.Connection = (SqlConnection)dictionary[_hardwareTableAdapter];
				_hardwareTableAdapter.Transaction = null;
			}
			if (_hostedHardwareTableAdapter != null)
			{
				_hostedHardwareTableAdapter.Connection = (SqlConnection)dictionary[_hostedHardwareTableAdapter];
				_hostedHardwareTableAdapter.Transaction = null;
			}
			if (_hostingServerTableAdapter != null)
			{
				_hostingServerTableAdapter.Connection = (SqlConnection)dictionary[_hostingServerTableAdapter];
				_hostingServerTableAdapter.Transaction = null;
			}
			if (_licenseTableAdapter != null)
			{
				_licenseTableAdapter.Connection = (SqlConnection)dictionary[_licenseTableAdapter];
				_licenseTableAdapter.Transaction = null;
			}
			if (_matrixTableAdapter != null)
			{
				_matrixTableAdapter.Connection = (SqlConnection)dictionary[_matrixTableAdapter];
				_matrixTableAdapter.Transaction = null;
			}
			if (_mIPObjectTableAdapter != null)
			{
				_mIPObjectTableAdapter.Connection = (SqlConnection)dictionary[_mIPObjectTableAdapter];
				_mIPObjectTableAdapter.Transaction = null;
			}
			if (_notificationProfilesTableAdapter != null)
			{
				_notificationProfilesTableAdapter.Connection = (SqlConnection)dictionary[_notificationProfilesTableAdapter];
				_notificationProfilesTableAdapter.Transaction = null;
			}
			if (_notificationSettingsTableAdapter != null)
			{
				_notificationSettingsTableAdapter.Connection = (SqlConnection)dictionary[_notificationSettingsTableAdapter];
				_notificationSettingsTableAdapter.Transaction = null;
			}
			if (_notificationTypesTableAdapter != null)
			{
				_notificationTypesTableAdapter.Connection = (SqlConnection)dictionary[_notificationTypesTableAdapter];
				_notificationTypesTableAdapter.Transaction = null;
			}
			if (_patrollingProfilesTableAdapter != null)
			{
				_patrollingProfilesTableAdapter.Connection = (SqlConnection)dictionary[_patrollingProfilesTableAdapter];
				_patrollingProfilesTableAdapter.Transaction = null;
			}
			if (_recorderPluginDeviceTableAdapter != null)
			{
				_recorderPluginDeviceTableAdapter.Connection = (SqlConnection)dictionary[_recorderPluginDeviceTableAdapter];
				_recorderPluginDeviceTableAdapter.Transaction = null;
			}
			if (_recorderPluginsTableAdapter != null)
			{
				_recorderPluginsTableAdapter.Connection = (SqlConnection)dictionary[_recorderPluginsTableAdapter];
				_recorderPluginsTableAdapter.Transaction = null;
			}
			if (_recordersTableAdapter != null)
			{
				_recordersTableAdapter.Connection = (SqlConnection)dictionary[_recordersTableAdapter];
				_recordersTableAdapter.Transaction = null;
			}
			if (_recordingServerServiceConfigurableTableAdapter != null)
			{
				_recordingServerServiceConfigurableTableAdapter.Connection = (SqlConnection)dictionary[_recordingServerServiceConfigurableTableAdapter];
				_recordingServerServiceConfigurableTableAdapter.Transaction = null;
			}
			if (_recordingServerServiceRecorderTableAdapter != null)
			{
				_recordingServerServiceRecorderTableAdapter.Connection = (SqlConnection)dictionary[_recordingServerServiceRecorderTableAdapter];
				_recordingServerServiceRecorderTableAdapter.Transaction = null;
			}
			if (_recordingServerServiceTableAdapter != null)
			{
				_recordingServerServiceTableAdapter.Connection = (SqlConnection)dictionary[_recordingServerServiceTableAdapter];
				_recordingServerServiceTableAdapter.Transaction = null;
			}
			if (_recordingStorageDeviceTableAdapter != null)
			{
				_recordingStorageDeviceTableAdapter.Connection = (SqlConnection)dictionary[_recordingStorageDeviceTableAdapter];
				_recordingStorageDeviceTableAdapter.Transaction = null;
			}
			if (_recordingStorageTableAdapter != null)
			{
				_recordingStorageTableAdapter.Connection = (SqlConnection)dictionary[_recordingStorageTableAdapter];
				_recordingStorageTableAdapter.Transaction = null;
			}
			if (_registeredDeviceTypesTableAdapter != null)
			{
				_registeredDeviceTypesTableAdapter.Connection = (SqlConnection)dictionary[_registeredDeviceTypesTableAdapter];
				_registeredDeviceTypesTableAdapter.Transaction = null;
			}
			if (_registeredServersTableAdapter != null)
			{
				_registeredServersTableAdapter.Connection = (SqlConnection)dictionary[_registeredServersTableAdapter];
				_registeredServersTableAdapter.Transaction = null;
			}
			if (_rulesTableAdapter != null)
			{
				_rulesTableAdapter.Connection = (SqlConnection)dictionary[_rulesTableAdapter];
				_rulesTableAdapter.Transaction = null;
			}
			if (_ruleVariablesTableAdapter != null)
			{
				_ruleVariablesTableAdapter.Connection = (SqlConnection)dictionary[_ruleVariablesTableAdapter];
				_ruleVariablesTableAdapter.Transaction = null;
			}
			if (_serverGroupMembershipTableAdapter != null)
			{
				_serverGroupMembershipTableAdapter.Connection = (SqlConnection)dictionary[_serverGroupMembershipTableAdapter];
				_serverGroupMembershipTableAdapter.Transaction = null;
			}
			if (_serverGroupTableAdapter != null)
			{
				_serverGroupTableAdapter.Connection = (SqlConnection)dictionary[_serverGroupTableAdapter];
				_serverGroupTableAdapter.Transaction = null;
			}
			if (_systemSettingsTableAdapter != null)
			{
				_systemSettingsTableAdapter.Connection = (SqlConnection)dictionary[_systemSettingsTableAdapter];
				_systemSettingsTableAdapter.Transaction = null;
			}
			if (_timeProfileTableAdapter != null)
			{
				_timeProfileTableAdapter.Connection = (SqlConnection)dictionary[_timeProfileTableAdapter];
				_timeProfileTableAdapter.Transaction = null;
			}
			if (0 < list3.Count)
			{
				DataAdapter[] array4 = new DataAdapter[list3.Count];
				list3.CopyTo(array4);
				for (int l = 0; l < array4.Length; l++)
				{
					array4[l].AcceptChangesDuringUpdate = true;
				}
			}
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected virtual void SortSelfReferenceRows(DataRow[] rows, DataRelation relation, bool childFirst)
	{
		Array.Sort(rows, new SelfReferenceComparer(relation, childFirst));
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected virtual bool MatchTableAdapterConnection(IDbConnection inputConnection)
	{
		if (_connection != null)
		{
			return true;
		}
		if (Connection == null || inputConnection == null)
		{
			return true;
		}
		if (string.Equals(Connection.ConnectionString, inputConnection.ConnectionString, StringComparison.Ordinal))
		{
			return true;
		}
		return false;
	}
}
