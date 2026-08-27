using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace VideoOS.Installer.Interfaces.Content;

public abstract class CertificateSubscriberBase : IWizardCommonPageSubscription<CertificatePageContent>
{
	protected ILogger Logger { get; private set; }

	protected Dictionary<string, string> Parameters { get; private set; }

	public abstract Guid Id { get; }

	protected CertificateSubscriberBase(ILogger logger)
	{
		Logger = logger;
	}

	public virtual bool BeforeActivate(Dictionary<string, string> parameters)
	{
		Parameters = parameters ?? throw new ArgumentNullException("parameters");
		return true;
	}

	public virtual Dictionary<string, string> CollectParameters(CertificatePageContent content)
	{
		return new Dictionary<string, string>();
	}

	public abstract CertificatePageContent GetContent();
}
