using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupUpdateNameAlreadyExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60111;

	protected override int ErrorCodeDefault => 60111;

	public DeviceGroupUpdateNameAlreadyExistException(string newName, string existingParentGroupName)
		: base(FormatMessage(newName, existingParentGroupName))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "NewName", newName));
		base.MessageParameters.Add(1, new MessageParameter(1, "ExistingParentGroupName", existingParentGroupName));
	}

	[SqlError(60111)]
	public DeviceGroupUpdateNameAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("Name"), databaseError.ExtractString("GroupName"))
	{
	}

	private static string FormatMessage(string newName, string existingParentGroupName)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60111, newName, existingParentGroupName);
	}
}
