using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PanoramicLensInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class PanoramicLensInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private string CameraMountField;

	[OptionalField]
	private ImmerVisionInfo ImmerVisionField;

	[OptionalField]
	private bool PanoramicLensEnabledField;

	[OptionalField]
	private string PanoramicLensTypeField;

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
	public string CameraMount
	{
		get
		{
			return CameraMountField;
		}
		set
		{
			if ((object)CameraMountField != value)
			{
				CameraMountField = value;
				RaisePropertyChanged("CameraMount");
			}
		}
	}

	[DataMember]
	public ImmerVisionInfo ImmerVision
	{
		get
		{
			return ImmerVisionField;
		}
		set
		{
			if (ImmerVisionField != value)
			{
				ImmerVisionField = value;
				RaisePropertyChanged("ImmerVision");
			}
		}
	}

	[DataMember]
	public bool PanoramicLensEnabled
	{
		get
		{
			return PanoramicLensEnabledField;
		}
		set
		{
			if (!PanoramicLensEnabledField.Equals(value))
			{
				PanoramicLensEnabledField = value;
				RaisePropertyChanged("PanoramicLensEnabled");
			}
		}
	}

	[DataMember]
	public string PanoramicLensType
	{
		get
		{
			return PanoramicLensTypeField;
		}
		set
		{
			if ((object)PanoramicLensTypeField != value)
			{
				PanoramicLensTypeField = value;
				RaisePropertyChanged("PanoramicLensType");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
