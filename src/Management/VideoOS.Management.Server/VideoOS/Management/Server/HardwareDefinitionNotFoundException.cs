using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HardwareDefinitionNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60189;

	[SqlError(60189)]
	public HardwareDefinitionNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public HardwareDefinitionNotFoundException()
		: base(VmoResources.VMO60189)
	{
	}

	public HardwareDefinitionNotFoundException(string message)
		: base(message)
	{
	}

	public HardwareDefinitionNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HardwareDefinitionNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
