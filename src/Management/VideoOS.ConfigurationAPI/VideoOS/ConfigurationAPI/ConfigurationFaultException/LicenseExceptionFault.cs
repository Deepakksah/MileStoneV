using System;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI.ConfigurationFaultException;

[Serializable]
[DataContract]
public class LicenseExceptionFault
{
	[DataMember]
	public string TranslationId;

	[DataMember]
	public string Message;

	public LicenseExceptionFault()
	{
	}

	public LicenseExceptionFault(string message, string translationId)
	{
		TranslationId = translationId;
		Message = message;
	}

	public LicenseExceptionFault(LicenseException ex, string translationId)
	{
		TranslationId = translationId;
		Message = ex.Message;
	}
}
