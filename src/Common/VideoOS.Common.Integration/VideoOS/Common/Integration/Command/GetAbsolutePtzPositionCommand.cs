using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class GetAbsolutePtzPositionCommand : CommonCommandBase
{
	[DataMember]
	public Guid DeviceId { get; private set; }

	public static GetAbsolutePtzPositionCommand Create(Guid deviceId)
	{
		return new GetAbsolutePtzPositionCommand
		{
			DeviceId = deviceId
		};
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
