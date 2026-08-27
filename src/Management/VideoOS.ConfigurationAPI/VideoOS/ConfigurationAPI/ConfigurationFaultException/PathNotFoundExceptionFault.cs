using System;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI.ConfigurationFaultException;

[Serializable]
[DataContract]
public class PathNotFoundExceptionFault
{
	[DataMember]
	public string TranslationId;

	[DataMember]
	public string Message;

	public PathNotFoundExceptionFault()
	{
	}

	public PathNotFoundExceptionFault(string message, string translationId)
	{
		TranslationId = translationId;
		Message = message;
	}

	public PathNotFoundExceptionFault(PathNotFoundException ex, string translationId)
	{
		TranslationId = translationId;
		Message = ex.Message;
	}
}
