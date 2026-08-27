using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class SetMatrixViewCommand : CommonCommandBase
{
	[DataMember]
	public Guid MatrixId { get; private set; }

	[DataMember]
	public Guid[] DeviceIds { get; private set; }

	public SetMatrixViewCommand(Guid matrixId, Guid[] deviceIds)
	{
		MatrixId = matrixId;
		DeviceIds = deviceIds;
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
