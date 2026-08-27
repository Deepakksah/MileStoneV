using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupCreationNameAlreadyExistException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60108;

	[SqlError(60108)]
	[SqlError(60159)]
	public DeviceGroupCreationNameAlreadyExistException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.FormatMessage(), ex)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		base.ErrorCode = databaseError.Number;
		if (databaseError.Number == 60108)
		{
			string value = databaseError.ExtractString("Name");
			string value2 = databaseError.ExtractString("GroupName");
			base.MessageParameters.Add(0, new MessageParameter(0, "NewName", value));
			base.MessageParameters.Add(1, new MessageParameter(1, "ExistingParentGroupName", value2));
		}
	}

	public DeviceGroupCreationNameAlreadyExistException()
	{
	}

	public DeviceGroupCreationNameAlreadyExistException(string message)
		: base(message)
	{
	}

	public DeviceGroupCreationNameAlreadyExistException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceGroupCreationNameAlreadyExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
