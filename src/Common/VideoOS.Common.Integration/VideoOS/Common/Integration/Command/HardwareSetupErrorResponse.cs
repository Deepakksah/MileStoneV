using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class HardwareSetupErrorResponse : CommonResponseBase
{
	[DataMember]
	public int ErrorCode { get; private set; }

	[DataMember]
	public string ErrorMessageInternal { get; private set; }

	public HardwareSetupErrorResponse(int errorCode, string errorMessage)
	{
		ErrorCode = errorCode;
		ErrorMessageInternal = errorMessage;
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
