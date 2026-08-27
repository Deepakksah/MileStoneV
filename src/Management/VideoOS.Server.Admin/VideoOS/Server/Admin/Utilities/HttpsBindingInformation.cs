namespace VideoOS.Server.Admin.Utilities;

public class HttpsBindingInformation : BindingInformation
{
	public HttpsBindingInformation(BindingInformation binding)
	{
		base.Port = binding.Port;
		base.HasBinding = binding.HasBinding;
	}

	public HttpsBindingInformation()
	{
		base.Port = 443;
	}
}
