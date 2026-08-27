using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class RecordingStorageDeviceException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60335;

	protected override int ErrorCodeDefault => 60335;

	public RecordingStorageDeviceException()
	{
	}

	public RecordingStorageDeviceException(string name)
		: base(FormatMessage(name))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
	}

	[SqlError(60335)]
	public RecordingStorageDeviceException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("Name"))
	{
	}

	public RecordingStorageDeviceException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RecordingStorageDeviceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private static string FormatMessage(string name)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60335, name);
	}
}
