namespace VideoOS.Server.Admin.Utilities;

public class BindingInformation
{
	public bool HasBinding { get; set; }

	public int Port { get; set; }

	public BindingInformation()
	{
		HasBinding = false;
	}
}
