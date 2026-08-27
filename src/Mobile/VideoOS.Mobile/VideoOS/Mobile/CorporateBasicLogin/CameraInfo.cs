using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CameraInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CameraInfo : DeviceInfo
{
	[OptionalField]
	private bool BrowsableStreamField;

	[OptionalField]
	private CameraSecurityInfo CameraSecurityField;

	[OptionalField]
	private bool EdgeStorageSupportedField;

	[OptionalField]
	private IpixInfo IpixSettingsField;

	[OptionalField]
	private double MaxFPSField;

	[OptionalField]
	private bool MulticastEnabledField;

	[OptionalField]
	private PanoramicLensInfo PanoramicLensSettingsField;

	[OptionalField]
	private PtzInfo PtzSettingsField;

	[OptionalField]
	private StreamInfo[] StreamsField;

	[DataMember]
	public bool BrowsableStream
	{
		get
		{
			return BrowsableStreamField;
		}
		set
		{
			if (!BrowsableStreamField.Equals(value))
			{
				BrowsableStreamField = value;
				RaisePropertyChanged("BrowsableStream");
			}
		}
	}

	[DataMember]
	public CameraSecurityInfo CameraSecurity
	{
		get
		{
			return CameraSecurityField;
		}
		set
		{
			if (CameraSecurityField != value)
			{
				CameraSecurityField = value;
				RaisePropertyChanged("CameraSecurity");
			}
		}
	}

	[DataMember]
	public bool EdgeStorageSupported
	{
		get
		{
			return EdgeStorageSupportedField;
		}
		set
		{
			if (!EdgeStorageSupportedField.Equals(value))
			{
				EdgeStorageSupportedField = value;
				RaisePropertyChanged("EdgeStorageSupported");
			}
		}
	}

	[DataMember]
	public IpixInfo IpixSettings
	{
		get
		{
			return IpixSettingsField;
		}
		set
		{
			if (IpixSettingsField != value)
			{
				IpixSettingsField = value;
				RaisePropertyChanged("IpixSettings");
			}
		}
	}

	[DataMember]
	public double MaxFPS
	{
		get
		{
			return MaxFPSField;
		}
		set
		{
			if (!MaxFPSField.Equals(value))
			{
				MaxFPSField = value;
				RaisePropertyChanged("MaxFPS");
			}
		}
	}

	[DataMember]
	public bool MulticastEnabled
	{
		get
		{
			return MulticastEnabledField;
		}
		set
		{
			if (!MulticastEnabledField.Equals(value))
			{
				MulticastEnabledField = value;
				RaisePropertyChanged("MulticastEnabled");
			}
		}
	}

	[DataMember]
	public PanoramicLensInfo PanoramicLensSettings
	{
		get
		{
			return PanoramicLensSettingsField;
		}
		set
		{
			if (PanoramicLensSettingsField != value)
			{
				PanoramicLensSettingsField = value;
				RaisePropertyChanged("PanoramicLensSettings");
			}
		}
	}

	[DataMember]
	public PtzInfo PtzSettings
	{
		get
		{
			return PtzSettingsField;
		}
		set
		{
			if (PtzSettingsField != value)
			{
				PtzSettingsField = value;
				RaisePropertyChanged("PtzSettings");
			}
		}
	}

	[DataMember]
	public StreamInfo[] Streams
	{
		get
		{
			return StreamsField;
		}
		set
		{
			if (StreamsField != value)
			{
				StreamsField = value;
				RaisePropertyChanged("Streams");
			}
		}
	}
}
