using System;
using System.Runtime.Serialization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class VmoItemAlreadyExistException : VmoServerServiceException
{
	public VmoItemAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: base(ex, databaseError)
	{
	}

	public VmoItemAlreadyExistException()
	{
	}

	public VmoItemAlreadyExistException(string message)
		: base(message)
	{
	}

	public VmoItemAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected VmoItemAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
