using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.Configuration;

public class ConfigurationManager : IDisposable
{
	private class WeakReferenceDataSetEventHandler : WeakReference
	{
		private ConfigurationDataSet configurationDataSet;

		public WeakReferenceDataSetEventHandler(ConfigurationManager weakTarget, ConfigurationDataSet configurationDataSet)
			: base(weakTarget)
		{
			this.configurationDataSet = configurationDataSet;
		}

		public void CallConfigurationManagerOnDataChanged(object sender, DataRowChangeEventArgs e)
		{
			ConfigurationManager configurationManager = (ConfigurationManager)Target;
			if (configurationManager != null)
			{
				configurationManager.OnDataChanged(sender, e);
			}
			else
			{
				UnregisterEventHandler();
			}
		}

		public void UnregisterEventHandler()
		{
			if (configurationDataSet != null)
			{
				configurationDataSet.DataChanged -= CallConfigurationManagerOnDataChanged;
			}
		}
	}

	private List<string> loadedTables = new List<string>();

	public IConfigurationDataSetLoader dataLoader;

	protected ConfigurationDataSet _configurationDataSet;

	private RecordingServerServiceRecorders _recordingServerServiceRecorders;

	private RecordingServerServices _recordingServerServices;

	private RecordingServerServiceConfigurableCollection _recordingServerServiceConfigurable;

	private Recorders _recorders;

	private IPHardwareBlockCollection _hardwareBlocks;

	private DeviceDataDistributionCollection _deviceDataDistributionCollection;

	private DeviceGroups _deviceGroups;

	private DeviceLinks _deviceLinks;

	private RecorderPlugins _recorderPlugins;

	private Rules _Rules;

	private RuleVariables _RuleVariables;

	private EventTypes _eventTypes;

	private NotificationProfiles _notificationProfiles;

	private NotificationSettings _notificationSettings;

	private NotificationTypes _notificationTypes;

	private SystemSettings _systemSettings;

	private RegisteredDeviceTypes _registeredDeviceTypes;

	private PatrollingProfiles _patrollingProfiles;

	private Compressors _compressors;

	private Licenses _licenses;

	private ExternalPlugins _externalPlugins;

	private RegisteredServers _registeredServers;

	private MatrixViews _matrixViews;

	private WeakReferenceDataSetEventHandler weakReferenceDataSetEventHandler;

	private object _dataChangedEventLock = new object();

	public bool DisableEventHandling;

	private bool isDirty;

	private bool useDynamicLoad;

	private bool inBeginLoad;

	public static readonly Type HostingServerDataTable = typeof(ConfigurationDataSet.HostingServerDataTable);

	private HostingServerItems hostingServers;

	public static readonly Type HostedHardwareDataTable = typeof(ConfigurationDataSet.HostedHardwareDataTable);

	private HostedHardwareItems hostedHardwareItems;

	public static readonly Type RecordingServerServiceRecordersDataTable = typeof(ConfigurationDataSet.RecordingServerServiceRecorderDataTable);

	public static readonly Type RecordingServerServiceDataTable = typeof(ConfigurationDataSet.RecordingServerServiceDataTable);

	public static readonly Type RecordingServerServiceConfigurableDataTable = typeof(ConfigurationDataSet.RecordingServerServiceConfigurableDataTable);

	public static readonly Type RecordersDataTable = typeof(ConfigurationDataSet.RecordersDataTable);

	public static readonly Type DeviceDataDistributionDataTable = typeof(ConfigurationDataSet.DeviceDataDistributionDataTable);

	public static readonly Type HardwareBlockDataTable = typeof(ConfigurationDataSet.HardwareBlockDataTable);

	public static readonly Type HardwareDefinitionDataTable = typeof(ConfigurationDataSet.HardwareDefinitionDataTable);

	private IPHardwareDefinitionItems hardwareDefinitionItems;

	public static readonly Type HardwareDataTable = typeof(ConfigurationDataSet.HardwareDataTable);

	private IPHardwareItems hardwareItems;

	public static readonly Type DevicesDataTable = typeof(ConfigurationDataSet.DevicesDataTable);

	private DeviceItems deviceItems;

	public static readonly Type DeviceGroupsDataTable = typeof(ConfigurationDataSet.DeviceGroupsDataTable);

	public static readonly Type DeviceGroupMembersDataTable = typeof(ConfigurationDataSet.DeviceGroupMembersDataTable);

	private DeviceGroupItems deviceGroupItems;

	private DeviceGroupMemberItems deviceGroupMemberItems;

	public static readonly Type DeviceLinksDataTable = typeof(ConfigurationDataSet.DeviceLinksDataTable);

	public static readonly Type EventTypesDataTable = typeof(ConfigurationDataSet.EventTypesDataTable);

	public static readonly Type RulesDataTable = typeof(ConfigurationDataSet.RulesDataTable);

	public static readonly Type RuleVariablesDataTable = typeof(ConfigurationDataSet.RuleVariablesDataTable);

	public static readonly Type TimeProfileDataTable = typeof(ConfigurationDataSet.TimeProfileDataTable);

	public static readonly Type NotificationProfilesDataTable = typeof(ConfigurationDataSet.NotificationProfilesDataTable);

	public static readonly Type PatrollingProfilesDataTable = typeof(ConfigurationDataSet.PatrollingProfilesDataTable);

	public static readonly Type RecorderPluginDeviceDataTable = typeof(ConfigurationDataSet.RecorderPluginDeviceDataTable);

	private DeviceRecorderPluginItems deviceRecorderPluginItems;

	private DevicePrivacyMaskItems devicePrivacyMaskItems;

	private DeviceTrackItems deviceTrackItems;

	public static readonly Type SystemSettingsDataTable = typeof(ConfigurationDataSet.SystemSettingsDataTable);

	private LicenseItems licenseItems;

	public static readonly Type RegisteredServersDataTable = typeof(ConfigurationDataSet.RegisteredServersDataTable);

	public static readonly Type MatrixDataTable = typeof(ConfigurationDataSet.MatrixDataTable);

	private bool disposed;

	public bool IsDirty
	{
		[DebuggerStepThrough]
		get
		{
			return isDirty;
		}
	}

	public bool UseDynamicLoad
	{
		get
		{
			return useDynamicLoad;
		}
		set
		{
			useDynamicLoad = value;
		}
	}

	public ConfigurationDataSet Dataset
	{
		[DebuggerStepThrough]
		get
		{
			return _configurationDataSet;
		}
	}

	public bool HasChanges => _configurationDataSet.HasChanges();

	public HostingServerItems HostingServers{
		get
		{
			if (hostingServers == null)
			{
				hostingServers = new HostingServerItems(this);
			}
			return hostingServers;
		}
	}

	public HostedHardwareItems HostedHardwareItems{
		get
		{
			if (hostedHardwareItems == null)
			{
				hostedHardwareItems = new HostedHardwareItems(this);
			}
			return hostedHardwareItems;
		}
	}

	public RecordingServerServiceRecorders RecordingServerServiceRecorders{
		get
		{
			if (_recordingServerServiceRecorders == null)
			{
				_recordingServerServiceRecorders = new RecordingServerServiceRecorders(this);
			}
			return _recordingServerServiceRecorders;
		}
	}

	public RecordingServerServices RecordingServerServices{
		get
		{
			if (_recordingServerServices == null)
			{
				_recordingServerServices = new RecordingServerServices(this);
			}
			return _recordingServerServices;
		}
	}

	public RecordingServerServiceConfigurableCollection RecordingServerServiceConfigurable
	{
		get
		{
			if (_recordingServerServiceConfigurable == null)
			{
				_recordingServerServiceConfigurable = new RecordingServerServiceConfigurableCollection(this);
			}
			return _recordingServerServiceConfigurable;
		}
	}

	public Recorders Recorders
	{
		get
		{
			if (_recorders == null)
			{
				_recorders = new Recorders(this);
			}
			return _recorders;
		}
	}

	public DeviceDataDistributionCollection DeviceDataDistributionEntries{
		get
		{
			if (_deviceDataDistributionCollection == null)
			{
				_deviceDataDistributionCollection = new DeviceDataDistributionCollection(this);
			}
			return _deviceDataDistributionCollection;
		}
	}

	public IPHardwareBlockCollection HardwareBlocks{
		get
		{
			if (_hardwareBlocks == null)
			{
				_hardwareBlocks = new IPHardwareBlockCollection(this);
			}
			return _hardwareBlocks;
		}
	}

	public IPHardwareDefinitionItems HardwareDefinitionItems{
		get
		{
			if (hardwareDefinitionItems == null)
			{
				hardwareDefinitionItems = new IPHardwareDefinitionItems(this);
			}
			return hardwareDefinitionItems;
		}
	}

	public IPHardwareItems HardwareItems{
		get
		{
			if (hardwareItems == null)
			{
				hardwareItems = new IPHardwareItems(this);
			}
			return hardwareItems;
		}
	}

	public DeviceItems DeviceItems{
		get
		{
			if (deviceItems == null)
			{
				deviceItems = new DeviceItems(this);
			}
			return deviceItems;
		}
	}

	public DeviceGroupItems DeviceGroupItems{
		get
		{
			if (deviceGroupItems == null)
			{
				deviceGroupItems = new DeviceGroupItems(this);
			}
			return deviceGroupItems;
		}
	}

	public DeviceGroupMemberItems DeviceGroupMemberItems{
		get
		{
			if (deviceGroupMemberItems == null)
			{
				deviceGroupMemberItems = new DeviceGroupMemberItems(this);
			}
			return deviceGroupMemberItems;
		}
	}

	public DeviceGroups DeviceGroups
	{
		get
		{
			if (_deviceGroups == null)
			{
				_deviceGroups = new DeviceGroups(this, null);
			}
			return _deviceGroups;
		}
	}

	public DeviceLinks DeviceLinks
	{
		get
		{
			if (_deviceLinks == null)
			{
				_deviceLinks = new DeviceLinks(this);
			}
			return _deviceLinks;
		}
	}

	public EventTypes EventTypes
	{
		get
		{
			if (_eventTypes == null)
			{
				_eventTypes = new EventTypes(this);
			}
			return _eventTypes;
		}
	}

	public Rules Rules
	{
		get
		{
			if (_Rules == null)
			{
				_Rules = new Rules(this);
			}
			return _Rules;
		}
	}

	public RuleVariables RuleVariables
	{
		get
		{
			if (_RuleVariables == null)
			{
				_RuleVariables = new RuleVariables(this);
			}
			return _RuleVariables;
		}
	}

	public NotificationProfiles NotificationProfiles
	{
		get
		{
			if (_notificationProfiles == null)
			{
				_notificationProfiles = new NotificationProfiles(this);
			}
			return _notificationProfiles;
		}
	}

	public NotificationSettings NotificationSettings
	{
		get
		{
			if (_notificationSettings == null)
			{
				_notificationSettings = new NotificationSettings(this);
			}
			return _notificationSettings;
		}
	}

	public NotificationTypes NotificationTypes
	{
		get
		{
			if (_notificationTypes == null)
			{
				_notificationTypes = new NotificationTypes(this);
			}
			return _notificationTypes;
		}
	}

	public PatrollingProfiles PatrollingProfiles
	{
		get
		{
			if (_patrollingProfiles == null)
			{
				_patrollingProfiles = new PatrollingProfiles(this);
			}
			return _patrollingProfiles;
		}
	}

	public RecorderPlugins RecorderPlugins
	{
		get
		{
			if (_recorderPlugins == null)
			{
				_recorderPlugins = new RecorderPlugins(this);
			}
			return _recorderPlugins;
		}
	}

	public DeviceRecorderPluginItems DeviceRecorderPluginItems{
		get
		{
			if (deviceRecorderPluginItems == null)
			{
				deviceRecorderPluginItems = new DeviceRecorderPluginItems(this);
			}
			return deviceRecorderPluginItems;
		}
	}

	public DevicePrivacyMaskItems DevicePrivacyMaskItems{
		get
		{
			if (devicePrivacyMaskItems == null)
			{
				devicePrivacyMaskItems = new DevicePrivacyMaskItems(this);
			}
			return devicePrivacyMaskItems;
		}
	}

	public DeviceTrackItems DeviceTrackItems{
		get
		{
			if (deviceTrackItems == null)
			{
				deviceTrackItems = new DeviceTrackItems(this);
			}
			return deviceTrackItems;
		}
	}

	public SystemSettings SystemSettings
	{
		get
		{
			if (_systemSettings == null)
			{
				_systemSettings = new SystemSettings(this);
			}
			return _systemSettings;
		}
	}

	public RegisteredDeviceTypes RegisteredDeviceTypes
	{
		get
		{
			if (_registeredDeviceTypes == null)
			{
				_registeredDeviceTypes = new RegisteredDeviceTypes(this);
			}
			return _registeredDeviceTypes;
		}
	}

	public Compressors Compressors
	{
		get
		{
			if (_compressors == null)
			{
				_compressors = new Compressors(this);
			}
			return _compressors;
		}
	}

	public LicenseItems LicenseItems{
		get
		{
			if (licenseItems == null)
			{
				licenseItems = new LicenseItems(this);
			}
			return licenseItems;
		}
	}

	public Licenses Licenses
	{
		get
		{
			if (_licenses == null)
			{
				_licenses = new Licenses(this);
			}
			return _licenses;
		}
	}

	public ILicenseDocument LicenseDocument
	{
		get
		{
			try
			{
				License serverLicense = Licenses.ServerLicense;
				return LicenseFactory.CreateLicenseLoader().CreateLicenseDocument(serverLicense.DataAsBytes);
			}
			catch (ConfigurationItemNotFoundException)
			{
				return null;
			}
		}
	}

	public ExternalPlugins ExternalPlugins
	{
		get
		{
			if (_externalPlugins == null)
			{
				_externalPlugins = new ExternalPlugins(this);
			}
			return _externalPlugins;
		}
	}

	public RegisteredServers RegisteredServers
	{
		get
		{
			if (_registeredServers == null)
			{
				_registeredServers = new RegisteredServers(this);
			}
			return _registeredServers;
		}
	}

	public MatrixViews MatrixViews
	{
		get
		{
			if (_matrixViews == null)
			{
				_matrixViews = new MatrixViews(this);
			}
			return _matrixViews;
		}
	}

	private event EventHandler<DataRowChangeEventArgs> _dataChanged;

	public event EventHandler<DataRowChangeEventArgs> DataChanged
	{
		add
		{
			lock (_dataChangedEventLock)
			{
				if (this._dataChanged == null)
				{
					_configurationDataSet.DataChanged += weakReferenceDataSetEventHandler.CallConfigurationManagerOnDataChanged;
				}
				_dataChanged += value;
			}
		}
		remove
		{
			lock (_dataChangedEventLock)
			{
				if (this._dataChanged != null)
				{
					_dataChanged -= value;
					if (this._dataChanged == null)
					{
						_configurationDataSet.DataChanged -= weakReferenceDataSetEventHandler.CallConfigurationManagerOnDataChanged;
					}
				}
			}
		}
	}

	public event EventHandler<EventArgs> DataSaved;

	public event EventHandler<EventArgs> ChangesAccepted;

	public event EventHandler<EventArgs> ChangesRejected;

	public void EnsureTableDataLoaded(DataTable table)
	{
		if (!dataLoader.IsDynamicLoader || !UseDynamicLoad || loadedTables.Contains(table.TableName))
		{
			return;
		}
		using ConfigurationDataSet configurationDataSet = dataLoader.GetConfigurationDataSetTables(new string[1] { table.TableName });
		table.BeginLoadData();
		DisableEventHandling = true;
		foreach (DataRow row in configurationDataSet.Tables[table.TableName].Rows)
		{
			table.ImportRow(row);
		}
		DisableEventHandling = false;
		table.EndLoadData();
		loadedTables.Add(table.TableName);
	}

	public void OnDataChanged(object sender, DataRowChangeEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		if (DisableEventHandling)
		{
			return;
		}
		if (e.Action == DataRowAction.Rollback)
		{
			if (e.Row.RowState != DataRowState.Detached)
			{
				if (e.Row is ConfigurationDataSet.DevicesRow)
				{
					ConfigurationDataSet.DevicesRow devicesRow = (ConfigurationDataSet.DevicesRow)e.Row;
					DeviceItems.GetDeviceByDeviceId(new Guid(devicesRow.IDDevice)).ReloadSettings();
				}
				else if (e.Row is ConfigurationDataSet.HardwareRow)
				{
					ConfigurationDataSet.HardwareRow hardwareRow = (ConfigurationDataSet.HardwareRow)e.Row;
					HardwareItems.GetHardwareByHardwareId(new Guid(hardwareRow.IDHardware)).ReloadSettings();
				}
			}
		}
		else
		{
			isDirty = true;
		}
		EventHandler<DataRowChangeEventArgs> eventHandler;
		lock (_dataChangedEventLock)
		{
			eventHandler = this._dataChanged;
		}
		eventHandler?.Invoke(sender, e);
	}

	public void OnDataSaved(object sender, EventArgs e)
	{
		if (this.DataSaved != null)
		{
			this.DataSaved(sender, e);
		}
	}

	public void BeginLoadData()
	{
		foreach (DataTable table in _configurationDataSet.Tables)
		{
			table.BeginLoadData();
		}
		inBeginLoad = true;
	}

	public void EndLoadData()
	{
		foreach (DataTable table in _configurationDataSet.Tables)
		{
			table.EndLoadData();
		}
		inBeginLoad = false;
	}

	public void BeginLoadData(DataTable table)
	{
		if (!inBeginLoad)
		{
			table.BeginLoadData();
		}
	}

	public void EndLoadData(DataTable table)
	{
		if (!inBeginLoad)
		{
			table.EndLoadData();
		}
	}

	public ConfigurationManager(ConfigurationDataSet configurationDataSet, IConfigurationDataSetLoader configurationDataSetLoader)
	{
		if (configurationDataSetLoader == null)
		{
			throw new ArgumentNullException("configurationDataSetLoader");
		}
		weakReferenceDataSetEventHandler = new WeakReferenceDataSetEventHandler(this, configurationDataSet);
		_configurationDataSet = configurationDataSet;
		dataLoader = configurationDataSetLoader;
		useDynamicLoad = configurationDataSetLoader.IsDynamicLoader;
	}

	public ConfigurationManager(IConfigurationDataSetLoader configurationDataSetLoader)
		: this(new ConfigurationDataSet(), configurationDataSetLoader)
	{
	}

	public void AcceptChanges()
	{
		if (_configurationDataSet.HasChanges())
		{
			_configurationDataSet.AcceptChanges();
			if (this.ChangesAccepted != null)
			{
				this.ChangesAccepted(this, null);
			}
		}
		isDirty = false;
		OnDataSaved(this, null);
	}

	public void RejectChanges()
	{
		if (_configurationDataSet.HasChanges())
		{
			_configurationDataSet.RejectChanges();
			if (this.ChangesRejected != null)
			{
				this.ChangesRejected(this, null);
			}
		}
		isDirty = false;
	}

	public void EnsureHardwareDefinitionLoadedByHardware(Guid[] hardwareIds)
	{
		HardwareItems.EnsureDefinitionLoaded(hardwareIds);
	}

	public void EnsureHardwareDefinitionLoadedByHardware(IPHardware[] hardware)
	{
		if (hardware == null)
		{
			throw new ArgumentNullException("hardware");
		}
		List<Guid> list = new List<Guid>();
		foreach (IPHardware iPHardware in hardware)
		{
			list.Add(iPHardware.ID);
		}
		EnsureHardwareDefinitionLoadedByHardware(list.ToArray());
	}

	public void EnsureHardwareDefinitionLoadedByDevices(IEnumerable<Device> devices)
	{
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		HashSet<Guid> source = new HashSet<Guid>(devices.Select((Device d) => d.IPHardwareID));
		EnsureHardwareDefinitionLoadedByHardware(source.ToArray());
	}

	public void EnsureRecorderPluginDeviceLoadedByRecorderPluginIdAndDeviceIds(Guid recorderPluginId, Guid[] deviceIds)
	{
		DeviceRecorderPluginItems.EnsureRecorderPluginDeviceLoaded(recorderPluginId, deviceIds);
	}

	public void EnsureDevicePrivacyMaskItemsLoaded(Guid[] deviceIds)
	{
		DevicePrivacyMaskItems.EnsureDevicePrivacyMaskItemsLoaded(deviceIds);
	}

	public void EnsureDeviceTrackItemsLoaded(IEnumerable<Guid> deviceIds)
	{
		DeviceTrackItems.EnsureDeviceTrackItemsLoaded(deviceIds);
	}

	public void EnsureRecorderPluginDeviceLoadedByRecorderPluginIdAndDeviceIds(Guid recorderPluginId, Device[] devices)
	{
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		List<Guid> list = new List<Guid>();
		foreach (Device device in devices)
		{
			list.Add(device.ID);
		}
		EnsureRecorderPluginDeviceLoadedByRecorderPluginIdAndDeviceIds(recorderPluginId, list.ToArray());
	}

	public void EnsureRecordingStorageDeviceLoadedByRecorderId(Guid recorderId)
	{
		using ConfigurationDataSet configurationDataSet = dataLoader.GetRecordingStorageDevicesByRecorderId(recorderId);
		DisableEventHandling = true;
		BeginLoadData(Dataset.RecordingStorageDevice);
		foreach (ConfigurationDataSet.RecordingStorageDeviceRow row in configurationDataSet.RecordingStorageDevice.Rows)
		{
			_configurationDataSet.RecordingStorageDevice.ImportRow(row);
		}
		EndLoadData(Dataset.RecordingStorageDevice);
		DisableEventHandling = false;
	}

	public IPHardware FindHardwareByHardwareId(Guid hardwareId)
	{
		return HardwareItems.GetHardwareByHardwareId(hardwareId);
	}

	public IPHardware FindHardwareByHostedHardwareId(int hostedHardwareId)
	{
		return HardwareItems.GetHardwareByHostedHardwareId(hostedHardwareId);
	}

	public bool TryGetDevice(Guid deviceId, out Device device)
	{
		return DeviceItems.TryGetDevice(deviceId, out device);
	}

	public Device FindDeviceByDeviceId(Guid deviceId)
	{
		return DeviceItems.GetDeviceByDeviceId(deviceId);
	}

	public Device[] FindDevicesByDeviceIds(IEnumerable<Guid> deviceIds, bool suppressItemNotFoundError)
	{
		return DeviceItems.GetDevicesByDeviceIds(deviceIds, suppressItemNotFoundError);
	}

	public DeviceGroup FindDeviceGroupByDeviceGroupId(Guid deviceGroupId)
	{
		return DeviceGroupItems.GetDeviceGroupByDeviceGroupId(deviceGroupId);
	}

	public HostingServer FindHostingServerById(int id)
	{
		return HostingServers.GetHostingServerById(id);
	}

	public HostedHardware FindHostedHardwareById(int id)
	{
		return HostedHardwareItems.GetHostedHardwareById(id);
	}

	public HostedHardware[] FindHostedHardwareByIds(int[] ids)
	{
		return HostedHardwareItems.GetHostedHardwareByIds(ids);
	}

	public IPHardwareDefinition FindDefinitionByDigest(string digest)
	{
		return HardwareDefinitionItems.GetHardwareDefinitionByDigest(digest);
	}

	~ConfigurationManager()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing && weakReferenceDataSetEventHandler != null)
			{
				weakReferenceDataSetEventHandler.UnregisterEventHandler();
			}
			disposed = true;
		}
	}
}
