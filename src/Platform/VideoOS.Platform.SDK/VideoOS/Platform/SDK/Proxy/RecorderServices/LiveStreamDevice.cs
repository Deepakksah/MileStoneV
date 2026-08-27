using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
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
