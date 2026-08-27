using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class SystemConfigurationCleanUpUnwrap : CleanUpUnwrapperBase, ISystemConfigurationCleanUp
{
	public override void SetInnerItem(IWrapperCleanup innerItem)
	{
		base.SetInnerItem(innerItem);
		base.InnerItem.InterfaceName = "ISystemConfigurationCleanUp";
		base.InnerItem.InitializeName = "Initialize";
		base.InnerItem.CleanupName = "CleanUp";
	}
}
