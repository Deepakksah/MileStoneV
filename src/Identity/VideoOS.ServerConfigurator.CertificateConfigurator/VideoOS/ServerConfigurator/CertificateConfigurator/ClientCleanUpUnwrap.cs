using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class ClientCleanUpUnwrap : CleanUpUnwrapperBase, IClientCleanup
{
	public override void SetInnerItem(IWrapperCleanup innerItem)
	{
		base.SetInnerItem(innerItem);
		base.InnerItem.InterfaceName = "IClientCleanup";
		base.InnerItem.InitializeName = "Initialize";
		base.InnerItem.CleanupName = "CleanUp";
	}
}
