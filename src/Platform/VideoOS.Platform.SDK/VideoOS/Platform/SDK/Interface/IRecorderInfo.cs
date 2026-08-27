using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IRecorderInfo
{
	Guid RecorderId { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	string HostName { get; set; }

	string WebServerUri { get; set; }

	string TimeZoneName { get; set; }

	int DefaultRecordingTimeSeconds { get; set; }

	string XmlEncoding { get; set; }

	DateTime LastModified { get; set; }

	ICameraInfo[] Cameras { get; set; }

	IInputInfo[] Inputs { get; set; }

	IOutputInfo[] Outputs { get; set; }

	IMicrophoneInfo[] Microphones { get; set; }

	ISpeakerInfo[] Speakers { get; set; }

	IMetadataDeviceInfo[] MetadataDevices { get; set; }

	IHardwareInfo[] Hardware { get; set; }

	void RemoveCamera(Guid deviceId);

	void RemoveMicrophone(Guid deviceId);

	void RemoveSpeaker(Guid deviceId);
}
