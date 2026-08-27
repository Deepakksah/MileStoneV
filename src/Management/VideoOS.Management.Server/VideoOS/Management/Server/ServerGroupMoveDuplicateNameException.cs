using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupMoveDuplicateNameException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60087;

	protected override int ErrorCodeDefault => 60087;

	public ServerGroupMoveDuplicateNameException(string name, string parentGroupName)
		: base(FormatMessage(name, parentGroupName))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "Name", name));
		base.MessageParameters.Add(1, new MessageParameter(1, "ParentGroupName", parentGroupName));
	}

	[SqlError(60087)]
	public ServerGroupMoveDuplicateNameException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("Name"), databaseError.ExtractString("parentGroupName"))
	{
	}

	private static string FormatMessage(string name, string parentGroupName)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60087, name, parentGroupName);
	}
}
