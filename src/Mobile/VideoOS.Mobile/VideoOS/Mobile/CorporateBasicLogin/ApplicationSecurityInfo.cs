using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ApplicationSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ApplicationSecurityInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private SmartClientSecurityInfo SmartClientSecurityField;

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
	public SmartClientSecurityInfo SmartClientSecurity
	{
		get
		{
			return SmartClientSecurityField;
		}
		set
		{
			if (SmartClientSecurityField != value)
			{
				SmartClientSecurityField = value;
				RaisePropertyChanged("SmartClientSecurity");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
