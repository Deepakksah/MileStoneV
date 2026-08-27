using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Config;

public interface ICamera : IStreamDevice, IDevice
{
	string Id { get; }

	UserDefinedEventCollection Events { get; }

	Ptz PtzSettings { get; }

	string Shortcut { get; }

	string HostName { get; }

	int Channel { get; }

	IMicrophone Microphone { get; set; }

	Speaker Speaker { get; }

	IEnumerable<IMetadataDevice> RelatedMetadataDevices { get; }

	OutputDeviceCollection Outputs { get; }

	PresetCollection Presets { get; }

	CameraSecurity CameraAccess { get; }

	bool MulticastEnabled { get; }

	bool Browsable { get; }

	bool IpixEnabled { get; }

	bool EdgeSupported { get; }

	PanoramicLens PanoramicLens { get; }

	XmlDocument ConfigurationXml(Guid streamId);

	string GetIpixData();

	string GetPanoramicLensData();

	string GetPanoramicLensDataForXpco();

	VideoOS.Platform.SDK.Proxy.RecorderServices.Net.SequenceType[] GetSequenceTypes();

	StreamType[] GetStreamTypes();

	SequenceCollection GetSequenceAtTimeMinutes(DateTime Time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType);

	void MoveToPreset(string presetName);

	void PtzMoveRelative(PtzMoveRelativeTypes moveRelative);

	void PtzMoveRelative(PtzMoveRelativeTypes moveRelative, double speed, double step);

	void PtzMoveAbsolute(double pan, double tilt, double zoom);

	void PtzMoveAbsolute(PTZArgs ptzArgs);

	PTZArgs PtzGetAbsolutePosition();

	void PtzMoveHome();

	void PtzMoveStart(PTZArgs ptzArgs);

	void PtzAuxOn(int auxNumber);

	void PtzAuxOff(int auxNumber);

	void PtzMoveStop();

	void PtzCenter(double refWidth, double refHeight, double centerX, double centerY, double zoom);

	void PtzRectangle(double refWidth, double refHeight, double left, double top, double bottom, double right);
}
