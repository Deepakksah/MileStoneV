using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PresetInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class PresetInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private Guid IdField;

	[OptionalField]
	private string NameField;

	[OptionalField]
	private PositionInfo PositionField;

	[OptionalField]
	private string ShortcutField;

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
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			if (!IdField.Equals(value))
			{
				IdField = value;
				RaisePropertyChanged("Id");
			}
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
			if ((object)NameField != value)
			{
				NameField = value;
				RaisePropertyChanged("Name");
			}
		}
	}

	[DataMember]
	public PositionInfo Position
	{
		get
		{
			return PositionField;
		}
		set
		{
			if (PositionField != value)
			{
				PositionField = value;
				RaisePropertyChanged("Position");
			}
		}
	}

	[DataMember]
	public string Shortcut
	{
		get
		{
			return ShortcutField;
		}
		set
		{
			if ((object)ShortcutField != value)
			{
				ShortcutField = value;
				RaisePropertyChanged("Shortcut");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
