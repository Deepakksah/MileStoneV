using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("AxisTunnelServer")]
public class AxisTunnelServer : HostingServer, ICreatable, IUpdatable, IDeletable
{
	public AxisOneClickCameraCollection AxisOneClickCameras { get; private set; }

	public AxisTunnelServer(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
	}

	public AxisTunnelServer(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		base.Name = string.Empty;
		base.Description = string.Empty;
		base.UserName = string.Empty;
		base.Password = string.Empty;
		base.InternalAddress = null;
		base.ExternalAddress = null;
	}

	public AxisTunnelServer(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		AxisOneClickCameras = new AxisOneClickCameraCollection(LoadAxisOneClickCameras, this, isInitialized);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}

	private AxisOneClickCamera[] LoadAxisOneClickCameras(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<AxisTunnelServer, AxisOneClickCamera>(base.Id, refreshCachedObjects);
	}
}
