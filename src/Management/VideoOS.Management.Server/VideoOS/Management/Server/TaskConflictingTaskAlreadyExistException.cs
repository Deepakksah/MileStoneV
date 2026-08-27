using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class TaskConflictingTaskAlreadyExistException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61106;

	public TaskConflictingTaskAlreadyExistException()
		: base(VmoResources.VMO61106)
	{
	}

	public TaskConflictingTaskAlreadyExistException(string message)
		: base(message)
	{
	}

	public TaskConflictingTaskAlreadyExistException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected TaskConflictingTaskAlreadyExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
