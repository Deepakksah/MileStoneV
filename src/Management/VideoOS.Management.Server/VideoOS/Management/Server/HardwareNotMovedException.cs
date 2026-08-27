using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HardwareNotMovedException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60297;

	[SqlError(60297)]
	public HardwareNotMovedException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public HardwareNotMovedException()
		: base(VmoResources.VMO60297)
	{
	}

	public HardwareNotMovedException(string message)
		: base(message)
	{
	}

	public HardwareNotMovedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HardwareNotMovedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
