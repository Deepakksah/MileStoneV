using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SystemSettingDoesNotExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 65008;

	protected override int ErrorCodeDefault => 65008;

	public SystemSettingDoesNotExistException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	public SystemSettingDoesNotExistException(string name, Exception inner)
		: base(FormatMessage(name), inner)
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(65008)]
	public SystemSettingDoesNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"), ex)
	{
	}

	protected SystemSettingDoesNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65008, name);
	}
}
