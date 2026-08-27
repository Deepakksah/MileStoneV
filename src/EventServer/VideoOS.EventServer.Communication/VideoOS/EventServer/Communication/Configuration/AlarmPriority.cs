using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmPriority", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class AlarmPriority : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool AllowDesktopNotificationsField;

	private Guid IdField;

	private bool IsRepeatedSoundField;

	private string NameField;

	private int NumberField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public bool AllowDesktopNotifications
	{
		get
		{
			return AllowDesktopNotificationsField;
		}
		set
		{
			AllowDesktopNotificationsField = value;
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public bool IsRepeatedSound
	{
		get
		{
			return IsRepeatedSoundField;
		}
		set
		{
			IsRepeatedSoundField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public int Number
	{
		get
		{
			return NumberField;
		}
		set
		{
			NumberField = value;
		}
	}
}
