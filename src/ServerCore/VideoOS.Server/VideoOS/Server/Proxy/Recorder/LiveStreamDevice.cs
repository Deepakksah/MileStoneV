using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Server.Proxy.Recorder;

[Serializable]
[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class LiveStreamDevice
{
	private Guid deviceIDField;

	private LiveStreamSetting[] liveStreamSettingsField;

	public Guid DeviceID
	{
		get
		{
			return deviceIDField;
		}
		set
		{
			deviceIDField = value;
		}
	}

	public LiveStreamSetting[] LiveStreamSettings
	{
		get
		{
			return liveStreamSettingsField;
		}
		set
		{
			liveStreamSettingsField = value;
		}
	}
}
