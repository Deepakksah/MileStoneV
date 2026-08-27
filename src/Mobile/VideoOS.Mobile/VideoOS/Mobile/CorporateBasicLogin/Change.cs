using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Change", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class Change : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private ChangeType ChangeTypeField;

	[OptionalField]
	private string IdField;

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
	public ChangeType ChangeType
	{
		get
		{
			return ChangeTypeField;
		}
		set
		{
			if (ChangeTypeField != value)
			{
				ChangeTypeField = value;
				RaisePropertyChanged("ChangeType");
			}
		}
	}

	[DataMember]
	public string Id
	{
		get
		{
			return IdField;
		}
		set
		{
			if ((object)IdField != value)
			{
				IdField = value;
				RaisePropertyChanged("Id");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
