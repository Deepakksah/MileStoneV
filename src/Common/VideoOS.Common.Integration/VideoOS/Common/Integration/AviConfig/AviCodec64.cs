using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.AviConfig;

[Serializable]
[DataContract]
public class AviCodec64
{
	[DataMember]
	public int Handler { get; private set; }

	[DataMember]
	public string Description { get; private set; }

	[DataMember]
	public bool QualityOption { get; private set; }

	[DataMember]
	public bool QualityEnabled { get; set; }

	[DataMember]
	public int QualityValue { get; set; }

	[DataMember]
	public bool KeyFrameOption { get; private set; }

	[DataMember]
	public bool KeyFrameEnabled { get; set; }

	[DataMember]
	public int KeyFrameValue { get; set; }

	[DataMember]
	public bool DataRateOption { get; private set; }

	[DataMember]
	public bool DataRateEnabled { get; set; }

	[DataMember]
	public int DataRateValue { get; set; }

	[DataMember]
	public byte[] ConfigData { get; set; }

	public AviCodec64(int handler, string description, bool qualityOption, bool qualityEnabled, int qualityValue, bool keyFrameOption, bool keyFrameEnabled, int keyFrameValue, bool dataRateOption, bool dataRateEnabled, int dataRateValue, byte[] configData)
	{
		Handler = handler;
		Description = description;
		QualityOption = qualityOption;
		if (qualityOption)
		{
			QualityEnabled = qualityEnabled;
			QualityValue = qualityValue;
		}
		KeyFrameOption = keyFrameOption;
		if (keyFrameOption)
		{
			KeyFrameEnabled = keyFrameEnabled;
			KeyFrameValue = keyFrameValue;
		}
		DataRateOption = dataRateOption;
		if (dataRateOption)
		{
			DataRateEnabled = dataRateEnabled;
			DataRateValue = dataRateValue;
		}
		ConfigData = configData;
	}

	public override string ToString()
	{
		return Description;
	}
}
