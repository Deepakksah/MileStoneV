using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HardwareNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60045;

	[SqlError(60045)]
	[SqlError(60247)]
	public HardwareNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public HardwareNotFoundException()
		: base(VmoResources.VMO60045)
	{
	}

	public HardwareNotFoundException(string message)
		: base(message)
	{
	}

	public HardwareNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected HardwareNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
