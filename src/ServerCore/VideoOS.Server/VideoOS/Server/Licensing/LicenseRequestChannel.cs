using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;
using VideoOS.Server.DataComponents;

namespace VideoOS.Server.Licensing;

public class LicenseRequestChannel : LicenseRequestBase, ILicenseRequest
{
	public ILicenseRequest SnapshotOfCurrentHardwareToLicense
	{
		get
		{
			ILicenseRequest licenseRequest = new LicenseRequestChannel(base.LicenseManagerComponentsProvider, base.LicenseFeatureProvider, base.LicenseDocument, base.DataComponentFactory, base.ServerLogManager, base.LicenseDataSetComponent, includeHardwareIds: true, base.OemResourcesWrapper);
			ILicenseSnapshot licenseSnapshot = null;
			base.LicenseDataSetComponent.LicenseCacheLock.EnterWriteLock();
			try
			{
				licenseSnapshot = base.LicenseDataSetComponent.LoadLicenseDataSet(out var _);
			}
			finally
			{
				base.LicenseDataSetComponent.LicenseCacheLock.ExitWriteLock();
			}
			foreach (IRecorderInfo recorder in licenseSnapshot.Recorders)
			{
				UpdateLicenseRequestFromRecorderConfiguration(licenseRequest, licenseSnapshot, recorder.Id, includeEmptyMacAddresses: false);
			}
			return licenseRequest;
		}
	}

	public LicenseRequestChannel(ILicenseManagerComponentsProvider licenseManagerComponentsProvider, ILicenseFeatureProvider licenseFeatureProvider, ILicenseDocument licenseDocument, IDataComponentFactory dataComponentFactory, IServerLogManager serverLogManager, ILicenseDataSetComponent licenseDataSetComponent, bool includeHardwareIds, IOemResources oemResources)
		: base(licenseManagerComponentsProvider, licenseFeatureProvider, licenseDocument, dataComponentFactory, serverLogManager, licenseDataSetComponent, includeHardwareIds, oemResources)
	{
	}

	public string GenerateLicenseRequestAsXml()
	{
		ClearExistingHardwareDetails();
		UpdateLicenseRequestServerInfo();
		UpdateLicenseRequestFeatureInfo();
		UpdateLicenseRequestSLCInfo();
		base.LicenseDataSetComponent.LicenseCacheLock.EnterWriteLock();
		try
		{
			bool isObtainedFromCache;
			ILicenseSnapshot licenseSnapshot = base.LicenseDataSetComponent.LoadLicenseDataSet(out isObtainedFromCache);
			foreach (IRecorderInfo recorder in licenseSnapshot.Recorders)
			{
				UpdateLicenseRequestFromRecorderConfiguration(this, licenseSnapshot, recorder.Id, includeEmptyMacAddresses: false);
			}
		}
		finally
		{
			base.LicenseDataSetComponent.LicenseCacheLock.ExitWriteLock();
		}
		base.HardwareList.UpdateHardwareAggregates();
		base.HardwareList.UpdateTotalFeedCountsAcrossAllHardware(base.Feeds);
		base.HardwareList.UpdateTotalICPFeedCountsAcrossAllHardware(base.ICPFeeds);
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
		return BuildXmlRequest();
	}

	public bool IsActivationRequired()
	{
		ILicenseRequest snapshotOfCurrentHardwareToLicense = SnapshotOfCurrentHardwareToLicense;
		bool flag = base.LicenseFeatureProvider.IsFeatureLicensed("ChannelLicensing");
		int nonInterconnectedCount = snapshotOfCurrentHardwareToLicense.HardwareList.NonInterconnectedCount;
		int hardwareCount = base.LicenseDocument.GetHardwareCount(isInterconnected: false);
		if (nonInterconnectedCount != hardwareCount)
		{
			return true;
		}
		int interconnectedCount = snapshotOfCurrentHardwareToLicense.HardwareList.InterconnectedCount;
		int hardwareCount2 = base.LicenseDocument.GetHardwareCount(isInterconnected: true);
		if (interconnectedCount != hardwareCount2)
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
					int count = item.Feeds["Camera"].Count;
					int hardwareCamerasCount = base.LicenseDocument.GetHardwareCamerasCount(item.MacAddress);
					if (count != hardwareCamerasCount)
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
		base.HardwareList.AddHardware(hardware);
	}

	public string GenerateSerializedIntegrationRegistrations()
	{
		return WriteIntegrationInformation();
	}

	private bool IsHardwareExemptFromLicense(IHardwareInfo hardwareInfo, bool includeEmptyMacAddresses, string macAddress)
	{
		if (!hardwareInfo.Enabled)
		{
			return true;
		}
		if (!includeEmptyMacAddresses && MacAddressUtilities.IsEmptyMacAddress(hardwareInfo.MacAddress))
		{
			return true;
		}
		if (hardwareInfo.IsInterconnectedProduct && !base.LicenseFeatureProvider.IsFeatureLicensed("InterconnectedProducts"))
		{
			return true;
		}
		if (string.IsNullOrEmpty(macAddress))
		{
			base.ServerLogManager?.AddLogEntry(LogEntryId.LicenseNoMacAddressForHardware, hardwareInfo.Name);
			return true;
		}
		return false;
	}

	protected void UpdateLicenseRequestFromRecorderConfiguration(ILicenseRequest request, ILicenseSnapshot licenseSnapshot, Guid recorderId, bool includeEmptyMacAddresses)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		if (licenseSnapshot == null)
		{
			throw new ArgumentNullException("licenseSnapshot");
		}
		IRecorderInfo recorder = licenseSnapshot.GetRecorder(recorderId);
		if (recorder == null || !recorder.Enabled)
		{
			return;
		}
		request.Recorders++;
		IHardwareLicenseCountEvaluator hardwareLicenseCountEvaluator = base.LicenseManagerComponentsProvider.CreateHardwareLicenseCountEvaluator(base.LicenseDocument);
		foreach (IHardwareInfo item in recorder.Hardware)
		{
			string macAddress = item.MacAddress;
			if (!IsHardwareExemptFromLicense(item, includeEmptyMacAddresses, macAddress))
			{
				hardwareLicenseCountEvaluator?.UpdateLicenseCount(item);
				request.AddHardwareToRequest(item);
			}
		}
	}

	protected override void WriteHardwareRequestInformation(XmlTextWriter request)
	{
		request.WriteStartElement("HardwareCollection");
		foreach (IHardwareInfo item in base.HardwareList.NonInterconnectHardware)
		{
			request.WriteStartElement("Hardware");
			WriteAggregateFeedsToRequestInformation(request, base.HardwareList.GetAggregateFeeds(item.MacAddress));
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
			WriteAggregateFeedsToRequestInformation(request, base.HardwareList.GetAggregateFeeds(item.MacAddress));
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
			if (item.ChannelLicenseCount == 1)
			{
				num++;
				continue;
			}
			IAggregateFeedDictionary aggregateFeeds = base.HardwareList.GetAggregateFeeds(item.MacAddress);
			num = ((!aggregateFeeds.TryGetValue("Camera", out var value)) ? (num + 1) : (num + Math.Max(1, value.Count)));
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
			IAggregateFeedDictionary aggregateFeeds = base.HardwareList.GetAggregateFeeds(item.MacAddress);
			num = ((!aggregateFeeds.TryGetValue("Camera", out var value)) ? (num + 1) : (num + Math.Max(1, value.Count)));
		}
		request.WriteStartElement("ICPChannels");
		request.WriteAttributeString("Count", num.ToString(CultureInfo.InvariantCulture));
		request.WriteEndElement();
	}

	private string BuildXmlRequest()
	{
		StringBuilder stringBuilder = new StringBuilder(4096);
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
			WriteWhiteList(xmlTextWriter);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
		}
		finally
		{
			stringWriter?.Dispose();
		}
		return stringBuilder.ToString();
	}

	private void WriteWhiteList(XmlTextWriter request)
	{
		IHardwareLicenseCountEvaluator hardwareLicenseCountEvaluator = base.LicenseManagerComponentsProvider.CreateHardwareLicenseCountEvaluator(base.LicenseDocument);
		request.WriteStartElement("Whitelist");
		request.WriteAttributeString("Version", hardwareLicenseCountEvaluator.Version);
		request.WriteEndElement();
	}

	private static void WriteAggregateFeedsToRequestInformation(XmlTextWriter request, IAggregateFeedDictionary feedDictionary)
	{
		request.WriteStartElement("Feeds");
		foreach (ILicenseFeed value in feedDictionary.Values)
		{
			request.WriteStartElement("Feed");
			request.WriteAttributeString("name", value.Name);
			if (feedDictionary.EnabledCameraIndexes.Length != 0 && value.Name.Equals("Camera", StringComparison.OrdinalIgnoreCase))
			{
				request.WriteAttributeString("channels", string.Join(",", feedDictionary.EnabledCameraIndexes));
			}
			request.WriteString(value.Count.ToString(CultureInfo.InvariantCulture));
			request.WriteEndElement();
		}
		request.WriteEndElement();
	}

	private void ClearExistingHardwareDetails()
	{
		base.Recorders = 0;
		base.Properties.Clear();
		base.HardwareList.Clear();
		base.Feeds.Clear();
		base.ICPFeeds.Clear();
		base.Features.Clear();
	}
}
