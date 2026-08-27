using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DuplicateNameException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61034;

	[SqlError(61034)]
	public DuplicateNameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
	}

	public DuplicateNameException()
		: base(VmoResources.VMO61034)
	{
	}

	public DuplicateNameException(string message)
		: base(message)
	{
	}

	public DuplicateNameException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DuplicateNameException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
