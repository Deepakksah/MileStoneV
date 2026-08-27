using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MatrixMonitorInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MatrixMonitorInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string DisplayNameField;

	[OptionalField]
	private Guid MatrixMonitorIdField;

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
	public Guid MatrixMonitorId
	{
		get
		{
			return MatrixMonitorIdField;
		}
		set
		{
			if (!MatrixMonitorIdField.Equals(value))
			{
				MatrixMonitorIdField = value;
				RaisePropertyChanged("MatrixMonitorId");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
