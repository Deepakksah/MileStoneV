using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "BookmarkSettingInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class BookmarkSettingInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int DefaultPostTimeSecField;

	[OptionalField]
	private int DefaultPreTimeSecField;

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
	public int DefaultPostTimeSec
	{
		get
		{
			return DefaultPostTimeSecField;
		}
		set
		{
			if (!DefaultPostTimeSecField.Equals(value))
			{
				DefaultPostTimeSecField = value;
				RaisePropertyChanged("DefaultPostTimeSec");
			}
		}
	}

	[DataMember]
	public int DefaultPreTimeSec
	{
		get
		{
			return DefaultPreTimeSecField;
		}
		set
		{
			if (!DefaultPreTimeSecField.Equals(value))
			{
				DefaultPreTimeSecField = value;
				RaisePropertyChanged("DefaultPreTimeSec");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
