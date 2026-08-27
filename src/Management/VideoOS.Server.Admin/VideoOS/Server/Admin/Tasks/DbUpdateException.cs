using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoOS.Server.Admin.Tasks;

[Serializable]
public class DbUpdateException : Exception
{
	public DbUpdateException()
	{
	}

	public DbUpdateException(string message)
		: base(message)
	{
	}

	public DbUpdateException(int exitCode, string output)
		: base(string.Format(CultureInfo.InvariantCulture, "DBUpdate failed. Exit code: {0}. Output: {1}", exitCode, output))
	{
	}

	public DbUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected DbUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
