using System;
using System.Collections.Specialized;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Configuration.Section;

public class IdentificationConfig : IConfigSection
{
	public const string NameCertificatePassword = "kluch";

	public const string NameServiceId = "ServiceId";

	public const string NameReportedServiceId = "ReportedServiceId";

	public string CertificatePassword { get; private set; }

	public Guid ServiceId { get; private set; }

	public Guid ReportedServiceId { get; private set; }

	public string SectionName => "Identification";

	public IdentificationConfig(Guid value)
	{
		ReportedServiceId = (ServiceId = value);
	}

	public bool InitFromCollection(NameValueCollection section)
	{
		bool result = true;
		try
		{
			CertificatePassword = section["kluch"];
		}
		catch
		{
			result = false;
		}
		try
		{
			ServiceId = new Guid(section["ServiceId"].Sanitized());
		}
		catch
		{
			result = false;
		}
		try
		{
			if (Guid.TryParse(section["ReportedServiceId"].Sanitized(), out var result2))
			{
				ReportedServiceId = result2;
			}
			else
			{
				ReportedServiceId = ServiceId;
			}
		}
		catch
		{
			ReportedServiceId = ServiceId;
		}
		return result;
	}

	public bool ConvToCollection(NameValueCollection serverSection)
	{
		bool result = true;
		try
		{
			serverSection["kluch"] = CertificatePassword;
		}
		catch
		{
			result = false;
		}
		try
		{
			serverSection["ReportedServiceId"] = ReportedServiceId.ToString();
		}
		catch
		{
			result = false;
		}
		return result;
	}
}
