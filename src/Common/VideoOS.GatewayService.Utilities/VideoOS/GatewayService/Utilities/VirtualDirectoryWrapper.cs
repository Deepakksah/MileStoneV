using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.Utilities;

public class VirtualDirectoryWrapper : IVirtualDirectoryWrapper
{
	private VirtualDirectory _internal;

	public AuthorizationTypes Authorization
	{
		get
		{
			return _internal.Authorization;
		}
		set
		{
			_internal.Authorization = value;
		}
	}

	public string VirtualLocation => _internal.VirtualLocation;

	public VirtualDirectoryWrapper(VirtualDirectory virtualDirectory)
	{
		_internal = virtualDirectory;
	}

	public void CommitChanges()
	{
		_internal.CommitChanges();
	}
}
