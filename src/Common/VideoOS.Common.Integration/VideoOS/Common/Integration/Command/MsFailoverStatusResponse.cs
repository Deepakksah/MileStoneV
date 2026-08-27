using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class MsFailoverStatusResponse : CommonResponseBase
{
	[DataMember]
	public bool InvokeFailoverStartedEvent { get; set; }

	[DataMember]
	public IEnumerable<MsFailoverStatusData> StatusResponseMessage { get; set; }

	public MsFailoverStatusResponse(bool invokeFailoverStartedEvent, IEnumerable<MsFailoverStatusData> statusResponseMessage)
	{
		InvokeFailoverStartedEvent = invokeFailoverStartedEvent;
		StatusResponseMessage = statusResponseMessage;
	}

	public override string ToString()
	{
		return GetType().Name;
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
