using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LoginInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class LoginInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private DateTime RegistrationTimeField;

	[OptionalField]
	private TimeDuration TimeToLiveField;

	[OptionalField]
	private bool TimeToLiveLimitedField;

	[OptionalField]
	private string TokenField;

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
	public DateTime RegistrationTime
	{
		get
		{
			return RegistrationTimeField;
		}
		set
		{
			if (!RegistrationTimeField.Equals(value))
			{
				RegistrationTimeField = value;
				RaisePropertyChanged("RegistrationTime");
			}
		}
	}

	[DataMember]
	public TimeDuration TimeToLive
	{
		get
		{
			return TimeToLiveField;
		}
		set
		{
			if (TimeToLiveField != value)
			{
				TimeToLiveField = value;
				RaisePropertyChanged("TimeToLive");
			}
		}
	}

	[DataMember]
	public bool TimeToLiveLimited
	{
		get
		{
			return TimeToLiveLimitedField;
		}
		set
		{
			if (!TimeToLiveLimitedField.Equals(value))
			{
				TimeToLiveLimitedField = value;
				RaisePropertyChanged("TimeToLiveLimited");
			}
		}
	}

	[DataMember]
	public string Token
	{
		get
		{
			return TokenField;
		}
		set
		{
			if ((object)TokenField != value)
			{
				TokenField = value;
				RaisePropertyChanged("Token");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
