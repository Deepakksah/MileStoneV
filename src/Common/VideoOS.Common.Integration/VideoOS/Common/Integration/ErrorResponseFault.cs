using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
public class ErrorResponseFault
{
	[DataMember]
	public string Message { get; private set; }

	[DataMember]
	public int ErrorNumber { get; private set; }

	public ErrorResponseFault(string message, int errorNumber)
	{
		Message = message;
		ErrorNumber = errorNumber;
	}
}
