using System;
using System.Xml;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public interface IMicrophone : IStreamDevice, IDevice
{
	string HostName { get; }

	string ConnectionXml { get; }

	MicrophoneSecurity MicrophoneAccess { get; }

	SequenceType[] GetSequenceTypes();

	SequenceCollection GetSequenceAtTimeMinutes(DateTime centerTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, Guid sequenceType);

	XmlDocument ConfigurationXml();
}
