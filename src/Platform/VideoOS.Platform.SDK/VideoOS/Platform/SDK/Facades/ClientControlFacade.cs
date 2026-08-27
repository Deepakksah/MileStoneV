namespace VideoOS.Platform.SDK.Facades;

public class ClientControlFacade : IClientControlFacade
{
	public void Close()
	{
		if (ClientControl.Instance != null)
		{
			ClientControl.Instance.Close();
		}
	}
}
