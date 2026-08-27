using System;
using System.Xml;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public interface IStreamDevice : IDevice
{
	ISystemInfo SystemInfo { get; }

	ITrackInfo[] Tracks { get; }

	XmlDocument GetToolkitConfigurationXml(Guid streamId);
}
