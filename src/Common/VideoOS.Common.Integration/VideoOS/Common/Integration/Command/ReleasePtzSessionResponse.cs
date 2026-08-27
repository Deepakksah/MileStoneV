using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class ReleasePtzSessionResponse : CommonResponseBase
{
	[DataMember]
	public PtzSessionInfo PtzSessionInfo { get; set; }

	public static ReleasePtzSessionResponse Create(PtzSessionInfo ptzSessionInfo)
	{
		return new ReleasePtzSessionResponse
		{
			PtzSessionInfo = ptzSessionInfo
		};
	}

	private ReleasePtzSessionResponse()
	{
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
