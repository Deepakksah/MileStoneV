using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SmartClientSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SmartClientSecurityInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool SmartClientBrowseField;

	[OptionalField]
	private bool SmartClientLiveField;

	[OptionalField]
	private bool SmartClientReportField;

	[OptionalField]
	private bool SmartClientSetupField;

	[Browsable(false)]
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
	public bool SmartClientBrowse
	{
		get
		{
			return SmartClientBrowseField;
		}
		set
		{
			if (!SmartClientBrowseField.Equals(value))
			{
				SmartClientBrowseField = value;
				RaisePropertyChanged("SmartClientBrowse");
			}
		}
	}

	[DataMember]
	public bool SmartClientLive
	{
		get
		{
			return SmartClientLiveField;
		}
		set
		{
			if (!SmartClientLiveField.Equals(value))
			{
				SmartClientLiveField = value;
				RaisePropertyChanged("SmartClientLive");
			}
		}
	}

	[DataMember]
	public bool SmartClientReport
	{
		get
		{
			return SmartClientReportField;
		}
		set
		{
			if (!SmartClientReportField.Equals(value))
			{
				SmartClientReportField = value;
				RaisePropertyChanged("SmartClientReport");
			}
		}
	}

	[DataMember]
	public bool SmartClientSetup
	{
		get
		{
			return SmartClientSetupField;
		}
		set
		{
			if (!SmartClientSetupField.Equals(value))
			{
				SmartClientSetupField = value;
				RaisePropertyChanged("SmartClientSetup");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
