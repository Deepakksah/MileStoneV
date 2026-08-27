using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.Utilities;

public interface IVirtualDirectoryWrapper
{
	AuthorizationTypes Authorization { get; set; }

	string VirtualLocation { get; }

	void CommitChanges();
}
