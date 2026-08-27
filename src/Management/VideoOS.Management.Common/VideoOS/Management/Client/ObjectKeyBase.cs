using System;
using VideoOS.Management.Client.Proxy;

namespace VideoOS.Management.Client;

public class ObjectKeyBase
{
	public virtual bool IsNull => false;

	public virtual ObjectKeyBase Clone()
	{
		return new ObjectKeyBase();
	}

	public virtual string GetExceptionName()
	{
		throw new NotImplementedException();
	}

	public virtual GenericObjectKey ToGenericObjectKey()
	{
		return new GenericObjectKey();
	}
}
