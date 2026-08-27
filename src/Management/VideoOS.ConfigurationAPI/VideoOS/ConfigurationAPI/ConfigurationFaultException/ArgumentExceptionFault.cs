using System;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI.ConfigurationFaultException;

[Serializable]
[DataContract]
public class ArgumentExceptionFault
{
	[DataMember]
	public string ArgumentName;

	[DataMember]
	public string Message;

	[DataMember]
	public string TranslationId;

	public ArgumentExceptionFault()
	{
	}

	public ArgumentExceptionFault(string argumentName, string message, string translationId)
	{
		ArgumentName = argumentName;
		Message = message;
		TranslationId = translationId;
	}

	public ArgumentExceptionFault(ArgumentException ex, string translationId)
	{
		ArgumentName = ex.ParamName;
		Message = ex.Message;
		TranslationId = translationId;
	}
}
