using System;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.CertificateRegistration.Common;

public class ServiceRegistrationParameters
{
	public Uri MainUri
	{
		get
		{
			string text = ((Uris.Length != 0) ? Uris[0] : null);
			if (text != null && Uri.TryCreate(text, UriKind.Absolute, out var result))
			{
				return result;
			}
			return null;
		}
		set
		{
			string text = value.ToString();
			if (Uris.Length != 0)
			{
				Uris[0] = text;
				return;
			}
			Uris = new string[1] { text };
		}
	}

	public string[] Uris { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string ServiceData { get; set; }

	public ServiceRegistrationParameters()
	{
	}

	public ServiceRegistrationParameters(ServiceInfo serviceInfo, string serviceData)
	{
		Uris = serviceInfo?.uri;
		Name = serviceInfo?.name;
		Description = serviceInfo?.description;
		ServiceData = (string.IsNullOrWhiteSpace(serviceData) ? null : serviceData);
	}
}
