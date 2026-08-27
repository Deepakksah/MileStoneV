using System;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI.ConfigurationFaultException;

[Serializable]
[DataContract]
public class ArgumentNullExceptionFault
{
	[DataMember]
	public string ArgumentName;

	[DataMember]
	public string Message;

	[DataMember]
	public string TranslationId;

	public ArgumentNullExceptionFault()
	{
	}

	public ArgumentNullExceptionFault(string argumentName, string message, string translationId)
	{
		ArgumentName = argumentName;
		Message = message;
		TranslationId = translationId;
	}

	public ArgumentNullExceptionFault(ArgumentNullException ex, string translationId)
	{
		ArgumentName = ex.ParamName;
		Message = ex.Message;
		TranslationId = translationId;
	}
}
