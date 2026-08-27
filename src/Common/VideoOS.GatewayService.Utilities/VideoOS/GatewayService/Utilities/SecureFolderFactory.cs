namespace VideoOS.GatewayService.Utilities;

public class SecureFolderFactory : ISecureFolderFactory
{
	public ISecureFolderWrapper GetSecureFolder()
	{
		return new SecureFolderWrapper();
	}
}
