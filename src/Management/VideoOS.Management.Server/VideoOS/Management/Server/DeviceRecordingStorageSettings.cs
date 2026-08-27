using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "DeviceRecordingStorageSettingsInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class DeviceRecordingStorageSettings
{
	private bool _prebufferingEnabled;

	private int _prebufferLengthInMs;

	private double _recordingFramerateInFps;

	private int _recordingSizeRetentionInMB;

	private int _recordingTimeRetentionInMinutes;

	[DataMember]
	public bool PrebufferingEnabled
	{
		get
		{
			return _prebufferingEnabled;
		}
		set
		{
			_prebufferingEnabled = value;
		}
	}

	[DataMember]
	public int PrebufferLengthInMs
	{
		get
		{
			return _prebufferLengthInMs;
		}
		set
		{
			_prebufferLengthInMs = value;
		}
	}

	[DataMember]
	public double RecordingFramerateInFps
	{
		get
		{
			return _recordingFramerateInFps;
		}
		set
		{
			_recordingFramerateInFps = value;
		}
	}

	[DataMember]
	public int RecordingSizeRetentionInMB
	{
		get
		{
			return _recordingSizeRetentionInMB;
		}
		set
		{
			_recordingSizeRetentionInMB = value;
		}
	}

	[DataMember]
	public int RecordingTimeRetentionInMinutes
	{
		get
		{
			return _recordingTimeRetentionInMinutes;
		}
		set
		{
			_recordingTimeRetentionInMinutes = value;
		}
	}
}
