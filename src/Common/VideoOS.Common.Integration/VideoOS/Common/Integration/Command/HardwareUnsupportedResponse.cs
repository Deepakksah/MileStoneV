using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class HardwareUnsupportedResponse : CommonResponseBase
{
	[DataMember]
	public string ProductName { get; private set; }

	[DataMember]
	public string ProductVersion { get; private set; }

	[DataMember]
	public int ErrorCode { get; private set; }

	public HardwareUnsupportedResponse(string productName, string productVersion, int errorCode)
	{
		ProductName = productName;
		ProductVersion = productVersion;
		ErrorCode = errorCode;
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
