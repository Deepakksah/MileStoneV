using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public class MetadataDevice : IMetadataDevice, IStreamDevice, IDevice
{
	private readonly ISystemInfo _systemInfo;

	private readonly IMetadataDeviceInfo _metadataDeviceInfo;

	private readonly IRecorderInfo _recorderInfo;

	private int _channel;

	private readonly IDictionary<Guid, MetadataType> _metadataTypes;

	private PositioningInformation _posInfo;

	private readonly MetadataSecurity _metadataSecurity;

	public ISystemInfo SystemInfo => _systemInfo;

	public string Id => _metadataDeviceInfo.DeviceId.ToString();

	public Guid Guid
	{
		get
		{
			return _metadataDeviceInfo.DeviceId;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			return _metadataDeviceInfo.Name;
		}
		set
		{
		}
	}

	public bool Disabled { get; set; }

	public IRecorder Recorder { get; set; }

	public string DBExportName => null;

	public PositioningInformation PositioningInformation => _posInfo;

	public string HostName => _recorderInfo.HostName;

	public int Channel => _channel;

	public IDictionary<Guid, MetadataType> MetadataTypes => _metadataTypes;

	public int Port => new UriBuilder(_recorderInfo.WebServerUri).Port;

	public MetadataSecurity MetadataAccess => _metadataSecurity;

	public ITrackInfo[] Tracks
	{
		get
		{
			if (_metadataDeviceInfo == null)
			{
				return null;
			}
			return _metadataDeviceInfo.Tracks;
		}
	}

	public MetadataDevice(ISystemInfo systemInfo, IMetadataDeviceInfo metadataDeviceInfo, IRecorder recorder, IRecorderInfo recorderInfo)
	{
		_systemInfo = systemInfo;
		_metadataDeviceInfo = metadataDeviceInfo;
		_recorderInfo = recorderInfo;
		Recorder = recorder;
		_channel = metadataDeviceInfo.DeviceIndex;
		_metadataTypes = metadataDeviceInfo.MetadataTypes.ToDictionary((IMetadataTypeInfo info) => info.Id, (IMetadataTypeInfo info) => new MetadataType(info));
		_posInfo = new PositioningInformation(metadataDeviceInfo.GisPoint, metadataDeviceInfo.CoverageDirection, metadataDeviceInfo.CoverageDepth, metadataDeviceInfo.CoverageFieldOfView);
		_metadataSecurity = new MetadataSecurity(_metadataDeviceInfo.MetadataDeviceSecurity);
	}

	public SequenceType[] GetSequenceTypes()
	{
		return Recorder.GetSequenceTypes(Guid);
	}

	public SequenceCollection GetSequenceAtTimeMinutes(DateTime time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType)
	{
		return Recorder.GetSequenceAtTimeMinutes(Guid, time, minutesBefore, maxCountBefore, minutesAfter, maxCountAfter, seqType);
	}

	public XmlDocument GetToolkitConfigurationXml(Guid streamId)
	{
		throw new NotImplementedException();
	}
}
