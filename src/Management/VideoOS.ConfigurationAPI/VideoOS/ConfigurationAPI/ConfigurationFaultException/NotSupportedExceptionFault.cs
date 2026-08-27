using System;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI.ConfigurationFaultException;

[Serializable]
[DataContract]
public class NotSupportedExceptionFault
{
	[DataMember]
	public string TranslationId;

	[DataMember]
	public string Message;

	public NotSupportedExceptionFault()
	{
	}

	public NotSupportedExceptionFault(string message, string translationId)
	{
		TranslationId = translationId;
		Message = message;
	}

	public NotSupportedExceptionFault(NotSupportedException ex, string translationId)
	{
		TranslationId = translationId;
		Message = ex.Message;
	}
}
