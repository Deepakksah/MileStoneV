using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ViewGroupInternalData", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class ViewGroupInternalData : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private int DataVersionField;

	[OptionalField]
	private string ViewGroupDataXmlField;

	[OptionalField]
	private int XmlVersionField;

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
	public int DataVersion
	{
		get
		{
			return DataVersionField;
		}
		set
		{
			if (!DataVersionField.Equals(value))
			{
				DataVersionField = value;
				RaisePropertyChanged("DataVersion");
			}
		}
	}

	[DataMember]
	public string ViewGroupDataXml
	{
		get
		{
			return ViewGroupDataXmlField;
		}
		set
		{
			if ((object)ViewGroupDataXmlField != value)
			{
				ViewGroupDataXmlField = value;
				RaisePropertyChanged("ViewGroupDataXml");
			}
		}
	}

	[DataMember]
	public int XmlVersion
	{
		get
		{
			return XmlVersionField;
		}
		set
		{
			if (!XmlVersionField.Equals(value))
			{
				XmlVersionField = value;
				RaisePropertyChanged("XmlVersion");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
