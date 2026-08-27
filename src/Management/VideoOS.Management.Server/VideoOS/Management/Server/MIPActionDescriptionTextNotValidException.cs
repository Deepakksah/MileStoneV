using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class MIPActionDescriptionTextNotValidException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60472;

	public MIPActionDescriptionTextNotValidException()
		: base(VmoResources.VMO60472)
	{
	}

	public MIPActionDescriptionTextNotValidException(string message)
		: base(message)
	{
	}

	public MIPActionDescriptionTextNotValidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected MIPActionDescriptionTextNotValidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
