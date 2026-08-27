using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceStreamConfigurationException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60229;

	public DeviceStreamConfigurationException()
		: base(VmoResources.VMO60229)
	{
	}

	public DeviceStreamConfigurationException(string message)
		: base(message)
	{
	}

	public DeviceStreamConfigurationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	[SqlError(60229)]
	[SqlError(60230)]
	[SqlError(60231)]
	[SqlError(60232)]
	[SqlError(60233)]
	[SqlError(60234)]
	[SqlError(60235)]
	[SqlError(60236)]
	[SqlError(60240)]
	[SqlError(60241)]
	[SqlError(60386)]
	[SqlError(60387)]
	[SqlError(60388)]
	[SqlError(60389)]
	[SqlError(60390)]
	[SqlError(60391)]
	[SqlError(60392)]
	[SqlError(60393)]
	[SqlError(60394)]
	[SqlError(60395)]
	public DeviceStreamConfigurationException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected DeviceStreamConfigurationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
