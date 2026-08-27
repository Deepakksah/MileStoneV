using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SmartClientVersion", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SmartClientVersion : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int BetaField;

	[OptionalField]
	private string DisplayNameField;

	[OptionalField]
	private bool ForceUpgradeField;

	[OptionalField]
	private int MajorField;

	[OptionalField]
	private int MinorField;

	[OptionalField]
	private string RevisionField;

	[OptionalField]
	private UpgradeInfo UpgradeInfoField;

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
	public int Beta
	{
		get
		{
			return BetaField;
		}
		set
		{
			if (!BetaField.Equals(value))
			{
				BetaField = value;
				RaisePropertyChanged("Beta");
			}
		}
	}

	[DataMember]
	public string DisplayName
	{
		get
		{
			return DisplayNameField;
		}
		set
		{
			if ((object)DisplayNameField != value)
			{
				DisplayNameField = value;
				RaisePropertyChanged("DisplayName");
			}
		}
	}

	[DataMember]
	public bool ForceUpgrade
	{
		get
		{
			return ForceUpgradeField;
		}
		set
		{
			if (!ForceUpgradeField.Equals(value))
			{
				ForceUpgradeField = value;
				RaisePropertyChanged("ForceUpgrade");
			}
		}
	}

	[DataMember]
	public int Major
	{
		get
		{
			return MajorField;
		}
		set
		{
			if (!MajorField.Equals(value))
			{
				MajorField = value;
				RaisePropertyChanged("Major");
			}
		}
	}

	[DataMember]
	public int Minor
	{
		get
		{
			return MinorField;
		}
		set
		{
			if (!MinorField.Equals(value))
			{
				MinorField = value;
				RaisePropertyChanged("Minor");
			}
		}
	}

	[DataMember]
	public string Revision
	{
		get
		{
			return RevisionField;
		}
		set
		{
			if ((object)RevisionField != value)
			{
				RevisionField = value;
				RaisePropertyChanged("Revision");
			}
		}
	}

	[DataMember]
	public UpgradeInfo UpgradeInfo
	{
		get
		{
			return UpgradeInfoField;
		}
		set
		{
			if (UpgradeInfoField != value)
			{
				UpgradeInfoField = value;
				RaisePropertyChanged("UpgradeInfo");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
