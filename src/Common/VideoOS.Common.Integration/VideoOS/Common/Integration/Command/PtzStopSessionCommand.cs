using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class PtzStopSessionCommand : CommonCommandBase
{
	[DataMember]
	public Guid DeviceId { get; private set; }

	[DataMember]
	public string UserToken { get; private set; }

	public static PtzStopSessionCommand Create(Guid deviceId, string userToken)
	{
		return new PtzStopSessionCommand
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
