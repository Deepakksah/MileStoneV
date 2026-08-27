using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class RecorderNotSupportedResponse : CommonResponseBase
{
	[DataMember]
	public Guid RecorderId { get; private set; }

	[DataMember]
	public string Message { get; private set; }

	public RecorderNotSupportedResponse(Guid recorderId, string message)
	{
		RecorderId = recorderId;
		Message = message;
	}

	public RecorderNotSupportedResponse(Guid recorderId)
		: this(recorderId, string.Empty)
	{
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
