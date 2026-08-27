using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
public class LicenseExceptionFault
{
	[DataMember]
	public string Message { get; private set; }

	[DataMember]
	public int ErrorNumber { get; private set; }

	[DataMember]
	public int FeatureErrorNumber { get; private set; }

	public LicenseExceptionFault(string message, int errorNumber, int featureErrorNumber)
	{
		Message = message;
		ErrorNumber = errorNumber;
		FeatureErrorNumber = featureErrorNumber;
	}
}
