using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class PtzReserveSessionCommand : CommonCommandBase
{
	[DataMember]
	public Guid DeviceId { get; private set; }

	[DataMember]
	public string UserToken { get; private set; }

	public static PtzReserveSessionCommand Create(Guid deviceId, string userToken)
	{
		return new PtzReserveSessionCommand
		{
			DeviceId = deviceId,
			UserToken = userToken
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
