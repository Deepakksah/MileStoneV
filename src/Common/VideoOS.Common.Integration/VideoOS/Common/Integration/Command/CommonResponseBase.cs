using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public abstract class CommonResponseBase : ResponseBase
{
	public abstract void Accept(ICommonResponseVisitor visitor);

	public abstract Task AcceptAsync(ICommonResponseVisitorAsync visitor);
}
