using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ViewGroupInternal", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class ViewGroupInternal : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private ViewGroupInternalData ViewGroupDataField;

	[OptionalField]
	private ViewGroupInfo ViewGroupInfoField;

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
	public ViewGroupInternalData ViewGroupData
	{
		get
		{
			return ViewGroupDataField;
		}
		set
		{
			if (ViewGroupDataField != value)
			{
				ViewGroupDataField = value;
				RaisePropertyChanged("ViewGroupData");
			}
		}
	}

	[DataMember]
	public ViewGroupInfo ViewGroupInfo
	{
		get
		{
			return ViewGroupInfoField;
		}
		set
		{
			if (ViewGroupInfoField != value)
			{
				ViewGroupInfoField = value;
				RaisePropertyChanged("ViewGroupInfo");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
