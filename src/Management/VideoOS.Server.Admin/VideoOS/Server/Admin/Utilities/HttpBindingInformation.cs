namespace VideoOS.Server.Admin.Utilities;

public class HttpBindingInformation : BindingInformation
{
	public HttpBindingInformation(BindingInformation binding)
	{
		base.Port = binding.Port;
		base.HasBinding = binding.HasBinding;
	}

	public HttpBindingInformation()
	{
		base.Port = 80;
	}
}
