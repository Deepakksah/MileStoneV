using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class FailoverStatusResponse : CommonResponseBase
{
	[DataMember]
	public Guid FailoverServerId { get; private set; }

	[DataMember]
	public FailoverStatusDto Status { get; private set; }

	public FailoverStatusResponse(Guid id, FailoverStatusDto status)
	{
		FailoverServerId = id;
		Status = status;
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
