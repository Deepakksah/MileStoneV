using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;
using VideoOS.Server.DataComponents;

namespace VideoOS.Server.Licensing;

public class LicenseRequestHardware : LicenseRequestBase, ILicenseRequest
{
	public ILicenseRequest SnapshotOfCurrentHardwareToLicense
	{
		get
		{
			LicenseRequestHardware licenseRequestHardware = new LicenseRequestHardware(base.LicenseManagerComponentsProvider, base.LicenseFeatureProvider, base.LicenseDocument, base.DataComponentFactory, base.ServerLogManager, base.LicenseDataSetComponent, includeHardwareIds: true, base.OemResourcesWrapper);
			ILicenseSnapshot licenseSnapshot = null;
			base.LicenseDataSetComponent.LicenseCacheLock.EnterWriteLock();
			try
			{
				licenseSnapshot = base.LicenseDataSetComponent.LoadLicenseDataSetByEnabled(enabled: true);
			}
			finally
			{
				base.LicenseDataSetComponent.LicenseCacheLock.ExitWriteLock();
			}
			foreach (IRecorderInfo recorder in licenseSnapshot.Recorders)
			{
				if (recorder.Enabled)
				{
					licenseRequestHardware.Recorders++;
					UpdateLicenseRequestFromRecorderConfiguration(licenseRequestHardware, recorder, includeEmptyMacAddresses: false);
				}
			}
			return licenseRequestHardware;
		}
	}

	public LicenseRequestHardware(ILicenseManagerComponentsProvider licenseManagerComponentsProvider, ILicenseFeatureProvider licenseFeatureProvider, ILicenseDocument licenseDocument, IDataComponentFactory dataComponentFactory, IServerLogManager serverLogManager, ILicenseDataSetComponent licenseDataSetComponent, bool includeHardwareIds, IOemResources oemResources)
		: base(licenseManagerComponentsProvider, licenseFeatureProvider, licenseDocument, dataComponentFactory, serverLogManager, licenseDataSetComponent, includeHardwareIds, oemResources)
	{
	}

	public string GenerateLicenseRequestAsXml()
	{
		UpdateLicenseRequestServerInfo();
		UpdateLicenseRequestFeatureInfo();
		UpdateLicenseRequestSLCInfo();
		base.LicenseDataSetComponent.LicenseCacheLock.EnterWriteLock();
		try
		{
			ILicenseSnapshot licenseSnapshot = base.LicenseDataSetComponent.LoadLicenseDataSetByEnabled(enabled: true);
			foreach (IRecorderInfo recorder in licenseSnapshot.Recorders)
			{
				UpdateLicenseRequestFromRecorderConfiguration(recorder, includeEmptyMacAddresses: false);
			}
		}
		finally
		{
			base.LicenseDataSetComponent.LicenseCacheLock.ExitWriteLock();
		}
		if (HasLicense())
		{
			base.HardwareGracePeriod = base.LicenseDocument.HardwareGracePeriod;
		}
		using (IMIPLicenseRequestComponent iMIPLicenseRequestComponent = base.DataComponentFactory.CreateMIPLicenseRequestComponent())
		{
			base.MIPLicenseRequest = iMIPLicenseRequestComponent.QueryMIPLicenseRequest();
		}
		using (ISystemBuilderLicenseRequestComponent systemBuilderLicenseRequestComponent = base.DataComponentFactory.CreateSystemBuilderLicenseRequestComponent())
		{
			base.SystemBuilderRequest = systemBuilderLicenseRequestComponent.GetSystemBuilderLicenseRequest();
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = null;
		try
		{
			stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument();
			xmlTextWriter.WriteStartElement("LicenseRequest");
			xmlTextWriter.WriteElementString("RequestTime", DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture));
			WriteExpiresDate(xmlTextWriter);
			WriteSLC(xmlTextWriter);
			WriteSiteInformation(xmlTextWriter);
			WriteHardwareGracePeriod(xmlTextWriter);
			WriteSoftwareInformation(xmlTextWriter);
			WriteComputerRequestInformation(xmlTextWriter);
			WriteProperties(xmlTextWriter);
			WriteFeedRequestInformation(xmlTextWriter);
			WriteChannelRequestInformation(xmlTextWriter);
			if (base.Features.ContainsKey("InterconnectedProducts"))
			{
				WriteICPFeedRequestInformation(xmlTextWriter);
				WriteICPChannelRequestInformation(xmlTextWriter);
			}
			WriteRecorderRequestInformation(xmlTextWriter);
			WriteHardwareRequestInformation(xmlTextWriter);
			if (base.Features.ContainsKey("InterconnectedProducts"))
			{
				WriteICPRequestInformation(xmlTextWriter);
			}
			WriteMIPPluginsInformation(xmlTextWriter);
			WriteSystemBuilderInformation(xmlTextWriter);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
		}
		finally
		{
			stringWriter?.Dispose();
		}
		return stringBuilder.ToString();
	}

	public bool IsActivationRequired()
	{
		ILicenseRequest snapshotOfCurrentHardwareToLicense = SnapshotOfCurrentHardwareToLicense;
		bool flag = base.LicenseFeatureProvider.IsFeatureLicensed("HardwareLicensing");
		int count = snapshotOfCurrentHardwareToLicense.HardwareList.NonInterconnectHardware.Count;
		int hardwareCount = base.LicenseDocument.GetHardwareCount(isInterconnected: false);
		if (count != hardwareCount)
		{
			return true;
		}
		int count2 = snapshotOfCurrentHardwareToLicense.HardwareList.InterconnectHardware.Count;
		int hardwareCount2 = base.LicenseDocument.GetHardwareCount(isInterconnected: true);
		if (count2 != hardwareCount2)
		{
			return true;
		}
		foreach (KeyValuePair<string, List<IHardwareInfo>> hardware in snapshotOfCurrentHardwareToLicense.HardwareList)
		{
			foreach (IHardwareInfo item in hardware.Value)
			{
				if (!base.LicenseDocument.IsMacAddressLicensed(item.MacAddress))
				{
					return true;
				}
				if (item.IsInterconnectedProduct || !flag)
				{
					int count3 = item.Feeds["Camera"].Count;
					int hardwareCamerasCount = base.LicenseDocument.GetHardwareCamerasCount(item.MacAddress);
					if (count3 != hardwareCamerasCount)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void AddHardwareToRequest(IHardwareInfo hardware)
	{
		if (hardware == null)
		{
			throw new ArgumentNullException("hardware");
		}
		if (hardware.IsInterconnectedProduct)
		{
			IHardwareInfo hardwareInfo = base.HardwareList.InterconnectHardware.FirstOrDefault((IHardwareInfo h) => h.MacAddress == hardware.MacAddress);
			if (hardwareInfo != null)
			{
				hardwareInfo.LicenseCount += hardware.LicenseCount;
				foreach (KeyValuePair<string, ILicenseFeed> feed in hardware.Feeds)
				{
					hardwareInfo.Feeds.IncrementCount(feed.Key, feed.Value.Count);
				}
				base.HardwareList.AddHardware(hardwareInfo);
			}
			else
			{
				base.HardwareList.AddHardware(hardware);
			}
		}
		else
		{
			base.HardwareList.AddHardware(hardware);
		}
	}

	public string GenerateSerializedIntegrationRegistrations()
	{
		return WriteIntegrationInformation();
	}

	protected void UpdateLicenseRequestFromRecorderConfiguration(IRecorderInfo recorderInfo, bool includeEmptyMacAddresses)
	{
		if (recorderInfo == null || !recorderInfo.Enabled)
		{
			return;
		}
		base.Recorders++;
		foreach (IHardwareInfo item in recorderInfo.Hardware)
		{
			if (item.Enabled && (includeEmptyMacAddresses || !MacAddressUtilities.IsEmptyMacAddress(item.MacAddress)) && (!item.IsInterconnectedProduct || base.LicenseFeatureProvider.IsFeatureLicensed("InterconnectedProducts")))
			{
				string macAddress = item.MacAddress;
				if (string.IsNullOrEmpty(macAddress))
				{
					base.ServerLogManager?.AddLogEntry(LogEntryId.LicenseNoMacAddressForHardware, item.Name);
				}
				else
				{
					RegisterFeeds(item);
					AddHardwareToRequest(item);
				}
			}
		}
	}

	protected override void WriteHardwareRequestInformation(XmlTextWriter request)
	{
		request.WriteStartElement("HardwareCollection");
		foreach (IHardwareInfo item in base.HardwareList.NonInterconnectHardware)
		{
			request.WriteStartElement("Hardware");
			WriteFeedRequestInformation(request, item.Feeds);
			if (base.IncludeHardwareIds)
			{
				request.WriteElementString("MacAddress", item.MacAddress);
				request.WriteElementString("SerialNumber", item.SerialNumber);
			}
			else
			{
				request.WriteElementString("MacAddress", string.Empty);
				request.WriteElementString("SerialNumber", string.Empty);
			}
			request.WriteElementString("Key", item.MacAddressHashed);
			request.WriteStartElement("Driver");
			request.WriteElementString("Name", item.DriverName);
			request.WriteElementString("Version", item.DriverVersion);
			request.WriteElementString("Number", item.DriverNumber.ToString(CultureInfo.InvariantCulture));
			request.WriteEndElement();
			WritePortsToRequestInformation(request, item.Ports);
			request.WriteElementString("Model", item.DetectedModel);
			WriteServerHardwareRequestInformation(request, item.MacAddress);
			request.WriteEndElement();
		}
		request.WriteEndElement();
	}

	protected override void WriteICPRequestInformation(XmlTextWriter request)
	{
		request.WriteStartElement("ICPCollection");
		foreach (IHardwareInfo item in base.HardwareList.InterconnectHardware)
		{
			request.WriteStartElement("ICP");
			WriteFeedRequestInformation(request, item.Feeds);
			if (base.IncludeHardwareIds)
			{
				request.WriteElementString("SystemID", item.MacAddress);
				request.WriteElementString("SerialNumber", item.SerialNumber);
			}
			else
			{
				request.WriteElementString("SystemID", string.Empty);
				request.WriteElementString("SerialNumber", string.Empty);
			}
			string value = MacAddressUtilities.CreateMacAddressHashString(item.MacAddress);
			request.WriteElementString("Key", value);
			request.WriteStartElement("Driver");
			request.WriteElementString("Name", item.DriverName);
			request.WriteElementString("Version", item.DriverVersion);
			request.WriteEndElement();
			request.WriteEndElement();
		}
		request.WriteEndElement();
	}

	protected override void WriteChannelRequestInformation(XmlTextWriter request)
	{
		int num = 0;
		foreach (IHardwareInfo item in base.HardwareList.NonInterconnectHardware)
		{
			num = ((item.ChannelLicenseCount != 1) ? ((!item.Feeds.TryGetValue("Camera", out var value)) ? (num + 1) : (num + Math.Max(1, value.Count))) : (num + 1));
		}
		request.WriteStartElement("DeviceChannels");
		request.WriteAttributeString("Count", num.ToString(CultureInfo.InvariantCulture));
		request.WriteEndElement();
	}

	protected override void WriteICPChannelRequestInformation(XmlTextWriter request)
	{
		int num = 0;
		foreach (IHardwareInfo item in base.HardwareList.InterconnectHardware)
		{
			num = ((!item.Feeds.TryGetValue("Camera", out var value)) ? (num + 1) : (num + Math.Max(1, value.Count)));
		}
		request.WriteStartElement("ICPChannels");
		request.WriteAttributeString("Count", num.ToString(CultureInfo.InvariantCulture));
		request.WriteEndElement();
	}

	private void RegisterFeeds(IHardwareInfo hardware)
	{
		IFeedDictionary feeds = (hardware.IsInterconnectedProduct ? base.ICPFeeds : base.Feeds);
		DefaultLicenseMethod(feeds, hardware);
	}

	private void DefaultLicenseMethod(IFeedDictionary feeds, IHardwareInfo hardware)
	{
		foreach (IDeviceInfo device in hardware.Devices)
		{
			if (ValidateDeviceForLicensing(device, hardware))
			{
				hardware.Ports.IncrementCount(device.DeviceType);
				if (device.Enabled)
				{
					hardware.Feeds.IncrementCount(device.DeviceType);
					feeds.IncrementCount(device.DeviceType);
				}
			}
		}
	}

	private bool ValidateDeviceForLicensing(IDeviceInfo device, IHardwareInfo hardware)
	{
		if (hardware.IsInterconnectedProduct)
		{
			IHardwareInfo hardwareInfo = base.HardwareList.InterconnectHardware.FirstOrDefault((IHardwareInfo h) => h.MacAddress == hardware.MacAddress);
			if (hardwareInfo != null && hardwareInfo.Devices.Any((IDeviceInfo d) => d.OriginalDeviceId == device.OriginalDeviceId))
			{
				return false;
			}
		}
		return true;
	}

	private void UpdateLicenseRequestFromRecorderConfiguration(LicenseRequestHardware licenseRequest, IRecorderInfo recorderInfo, bool includeEmptyMacAddresses)
	{
		foreach (IHardwareInfo item in recorderInfo.Hardware)
		{
			if (item.Enabled && (includeEmptyMacAddresses || !MacAddressUtilities.IsEmptyMacAddress(item.MacAddress)) && (!item.IsInterconnectedProduct || base.LicenseFeatureProvider.IsFeatureLicensed("InterconnectedProducts")))
			{
				if (string.IsNullOrEmpty(item.MacAddress))
				{
					base.ServerLogManager?.AddLogEntry(LogEntryId.LicenseNoMacAddressForHardware, item.Name);
				}
				else
				{
					licenseRequest.RegisterFeeds(item);
					licenseRequest.AddHardwareToRequest(item);
				}
			}
		}
	}
}
