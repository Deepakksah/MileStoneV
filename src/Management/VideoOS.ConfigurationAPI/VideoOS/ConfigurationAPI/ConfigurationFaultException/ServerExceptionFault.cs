using System;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI.ConfigurationFaultException;

[Serializable]
[DataContract]
public class ServerExceptionFault
{
	[DataMember]
	public string TranslationId;

	[DataMember]
	public string Message;

	[DataMember]
	public string Type;

	public ServerExceptionFault()
	{
	}

	public ServerExceptionFault(string message, string typeName, string translationId)
	{
		TranslationId = translationId;
		Message = message;
		Type = typeName;
	}

	public ServerExceptionFault(Exception ex, string translationId)
	{
		TranslationId = translationId;
		Message = ex.Message;
		Type = ex.GetType().FullName;
	}
}
