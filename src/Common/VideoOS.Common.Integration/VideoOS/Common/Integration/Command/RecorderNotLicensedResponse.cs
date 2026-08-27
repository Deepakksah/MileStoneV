using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class RecorderNotLicensedResponse : CommonResponseBase
{
	[DataMember]
	public Guid RecorderId { get; private set; }

	[DataMember]
	public string Message { get; private set; }

	public RecorderNotLicensedResponse(Guid recorderId, string message)
	{
		RecorderId = recorderId;
		Message = message;
	}

	public RecorderNotLicensedResponse(Guid recorderId)
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
