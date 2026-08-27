using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ProductInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ProductInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string BuildConfigurationField;

	[OptionalField]
	private DateTime BuildDateField;

	[OptionalField]
	private string BuildNumberField;

	[OptionalField]
	private string MajorVersionField;

	[OptionalField]
	private string MinorVersionField;

	[OptionalField]
	private Guid ProductLineField;

	[OptionalField]
	private string ProductNameField;

	[OptionalField]
	private string ServiceVersionField;

	[OptionalField]
	private Guid SubProductField;

	[OptionalField]
	private Guid VendorIdField;

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
	public string BuildConfiguration
	{
		get
		{
			return BuildConfigurationField;
		}
		set
		{
			if ((object)BuildConfigurationField != value)
			{
				BuildConfigurationField = value;
				RaisePropertyChanged("BuildConfiguration");
			}
		}
	}

	[DataMember]
	public DateTime BuildDate
	{
		get
		{
			return BuildDateField;
		}
		set
		{
			if (!BuildDateField.Equals(value))
			{
				BuildDateField = value;
				RaisePropertyChanged("BuildDate");
			}
		}
	}

	[DataMember]
	public string BuildNumber
	{
		get
		{
			return BuildNumberField;
		}
		set
		{
			if ((object)BuildNumberField != value)
			{
				BuildNumberField = value;
				RaisePropertyChanged("BuildNumber");
			}
		}
	}

	[DataMember]
	public string MajorVersion
	{
		get
		{
			return MajorVersionField;
		}
		set
		{
			if ((object)MajorVersionField != value)
			{
				MajorVersionField = value;
				RaisePropertyChanged("MajorVersion");
			}
		}
	}

	[DataMember]
	public string MinorVersion
	{
		get
		{
			return MinorVersionField;
		}
		set
		{
			if ((object)MinorVersionField != value)
			{
				MinorVersionField = value;
				RaisePropertyChanged("MinorVersion");
			}
		}
	}

	[DataMember]
	public Guid ProductLine
	{
		get
		{
			return ProductLineField;
		}
		set
		{
			if (!ProductLineField.Equals(value))
			{
				ProductLineField = value;
				RaisePropertyChanged("ProductLine");
			}
		}
	}

	[DataMember]
	public string ProductName
	{
		get
		{
			return ProductNameField;
		}
		set
		{
			if ((object)ProductNameField != value)
			{
				ProductNameField = value;
				RaisePropertyChanged("ProductName");
			}
		}
	}

	[DataMember]
	public string ServiceVersion
	{
		get
		{
			return ServiceVersionField;
		}
		set
		{
			if ((object)ServiceVersionField != value)
			{
				ServiceVersionField = value;
				RaisePropertyChanged("ServiceVersion");
			}
		}
	}

	[DataMember]
	public Guid SubProduct
	{
		get
		{
			return SubProductField;
		}
		set
		{
			if (!SubProductField.Equals(value))
			{
				SubProductField = value;
				RaisePropertyChanged("SubProduct");
			}
		}
	}

	[DataMember]
	public Guid VendorId
	{
		get
		{
			return VendorIdField;
		}
		set
		{
			if (!VendorIdField.Equals(value))
			{
				VendorIdField = value;
				RaisePropertyChanged("VendorId");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
