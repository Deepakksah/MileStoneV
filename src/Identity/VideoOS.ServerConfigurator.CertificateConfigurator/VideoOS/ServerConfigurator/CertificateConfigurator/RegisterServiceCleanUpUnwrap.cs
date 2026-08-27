using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class RegisterServiceCleanUpUnwrap : CleanUpUnwrapperBase, IServerCleanup
{
	public override void SetInnerItem(IWrapperCleanup innerItem)
	{
		base.SetInnerItem(innerItem);
		base.InnerItem.InterfaceName = "IServerCleanup";
		base.InnerItem.InitializeName = "Initialize";
		base.InnerItem.CleanupName = "CleanUp";
	}
}
