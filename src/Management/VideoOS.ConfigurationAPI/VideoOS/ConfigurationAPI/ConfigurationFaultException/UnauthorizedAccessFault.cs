using System;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI.ConfigurationFaultException;

[Serializable]
[DataContract]
public class UnauthorizedAccessFault
{
	[DataMember]
	public string TranslationId;

	[DataMember]
	public string Message;

	public UnauthorizedAccessFault()
	{
	}

	public UnauthorizedAccessFault(string message, string translationId)
	{
		TranslationId = translationId;
		Message = message;
	}

	public UnauthorizedAccessFault(UnauthorizedAccessException ex, string translationId)
	{
		TranslationId = translationId;
		Message = ex.Message;
	}
}
