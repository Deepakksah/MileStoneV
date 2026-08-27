using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

[EntityName("SystemMonitorStateSource")]
public class SystemMonitorStateSource : VmoObjectGuidBased
{
	public string Name
	{
		get
		{
			return base.ObjectProperties.GetPropertyValue<string>("Name");
		}
		set
		{
			base.ObjectProperties.SetPropertyValue("Name", value);
		}
	}

	public SystemMonitorStateSource(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
	}

	public SystemMonitorStateSource(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public SystemMonitorStateSource(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.QuerySystemMonitorStateSourcesByIds(new string[1] { base.Id.ToString() });
		if (array == null || array.Length == 0)
		{
			throw new VmoException(VmoResources.VMO65004);
		}
		base.ObjectProperties = array[0].Properties;
	}
}
