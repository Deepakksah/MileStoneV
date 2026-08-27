using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class ErrorResponse : CommonResponseBase
{
	[DataMember]
	public int ErrorCode { get; private set; }

	[DataMember]
	public int SubErrorCode { get; private set; }

	[DataMember]
	public string Message { get; private set; }

	[Localizable(false)]
	public ErrorResponse(int errorCode, int subErrorCode, string message)
	{
		ErrorCode = errorCode;
		SubErrorCode = subErrorCode;
		Message = message;
	}

	[Localizable(false)]
	public ErrorResponse(int errorCode, string message)
	{
		ErrorCode = errorCode;
		SubErrorCode = 0;
		Message = message;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", GetType().Name, Message);
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
