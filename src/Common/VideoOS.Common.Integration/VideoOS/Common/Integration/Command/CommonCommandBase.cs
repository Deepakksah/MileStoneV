using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public abstract class CommonCommandBase : CommandBase, IVisitableCommand, IVisitableCommandAsync
{
	public abstract void Accept(ICommandVisitor visitor);

	public abstract Task AcceptAsync(ICommonCommandVisitorAsync visitor);
}
