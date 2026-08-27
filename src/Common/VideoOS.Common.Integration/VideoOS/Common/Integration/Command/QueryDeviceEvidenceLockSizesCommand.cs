using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class QueryDeviceEvidenceLockSizesCommand : CommonCommandBase
{
	[DataMember]
	public Guid EvidenceLockId { get; set; }

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
