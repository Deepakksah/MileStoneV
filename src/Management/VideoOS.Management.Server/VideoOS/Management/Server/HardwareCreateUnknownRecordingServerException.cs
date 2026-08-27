using System;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class HardwareCreateUnknownRecordingServerException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60083;

	protected override int ErrorCodeDefault => 60083;

	public HardwareCreateUnknownRecordingServerException()
		: base(VmoResources.VMO60083)
	{
	}

	[SqlError(60083)]
	public HardwareCreateUnknownRecordingServerException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}
}
