using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public class MetadataDeviceInFile : IMetadataDevice, IStreamDevice, IDevice, IDeviceInFile
{
	private readonly ISystemInfo _systemInfo;

	private readonly MetadataSecurity _metadataSecurity;

	public string DatabasePath { get; set; }

	public string ArchivePath { get; set; }

	public string CameraPath { get; set; }

	public PositioningInformation PositioningInformation => null;

	public ISystemInfo SystemInfo => _systemInfo;

	public string Id => Guid.ToString();

	public string Name { get; set; }

	public IRecorder Recorder { get; set; }

	public Guid Guid { get; set; }

	public bool Disabled { get; set; }

	public string DBExportName { get; private set; }

	public string HostName => _systemInfo.ServerId.ServerHostname;

	public int Channel => 0;

	public IDictionary<Guid, MetadataType> MetadataTypes => new Dictionary<Guid, MetadataType>();

	public MetadataSecurity MetadataAccess => _metadataSecurity;

	public ITrackInfo[] Tracks => null;

	public MetadataDeviceInFile(ISystemInfo systemInfo, IRecorder recorder, Guid guid, string name, string dbExportName)
	{
		_systemInfo = systemInfo;
		Recorder = recorder;
		Guid = guid;
		Name = name;
		DBExportName = dbExportName;
		_metadataSecurity = new MetadataSecurity();
	}

	public SequenceType[] GetSequenceTypes()
	{
		return null;
	}

	public SequenceCollection GetSequenceAtTimeMinutes(DateTime time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType)
	{
		return new SequenceCollection();
	}

	public XmlDocument GetToolkitConfigurationXml(Guid streamId)
	{
		throw new NotImplementedException();
	}
}
