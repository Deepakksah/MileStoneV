using System.Runtime.Serialization;
using VideoOS.Management.Client.Proxy;

namespace VideoOS.Management.Client;

public class TaskObjectKey : SimpleObjectKey
{
	[DataMember]
	public string TaskType { get; set; }

	public TaskObjectKey(string id, string taskType)
		: base(id)
	{
		TaskType = taskType;
	}

	public override ObjectKeyBase Clone()
	{
		return new TaskObjectKey(base.Id, TaskType);
	}

	public override GenericObjectKey ToGenericObjectKey()
	{
		GenericObjectKey genericObjectKey = base.ToGenericObjectKey();
		genericObjectKey.Type = TaskType;
		return genericObjectKey;
	}
}
