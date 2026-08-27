using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Installer.Interfaces.Content;

namespace VideoOS.Installer.Manager;

public class SubscriptionPlugins
{
	private IEnumerable<IWizardCommonPageSubscription<CertificatePageContent>> _certificateSubscriptions { get; }

	private IEnumerable<IWizardCommonPageSubscription<ServiceUserPageContent>> _serviceUserSubscriptions { get; }

	private IEnumerable<IWizardCommonPageSubscription<WarningPageContent>> _warningSubscriptions { get; }

	private IEnumerable<IWizardCommonPageSubscription<SamplePageContent>> _samplePageSubscriptions { get; }

	public SubscriptionBypassFilter BypassFilter { get; set; }

	public ReadOnlyCollection<IWizardCommonPageSubscription<CertificatePageContent>> CertificateSubscriptions => GetSubscriptions(_certificateSubscriptions);

	public ReadOnlyCollection<IWizardCommonPageSubscription<ServiceUserPageContent>> ServiceUserSubscriptions => GetSubscriptions(_serviceUserSubscriptions);

	public ReadOnlyCollection<IWizardCommonPageSubscription<WarningPageContent>> WarningSubscriptions => GetSubscriptions(_warningSubscriptions);

	public ReadOnlyCollection<IWizardCommonPageSubscription<SamplePageContent>> SamplePageSubscriptions => GetSubscriptions(_samplePageSubscriptions);

	public SubscriptionPlugins()
		: this(Enumerable.Empty<IWizardCommonPageSubscription<CertificatePageContent>>(), Enumerable.Empty<IWizardCommonPageSubscription<ServiceUserPageContent>>(), Enumerable.Empty<IWizardCommonPageSubscription<WarningPageContent>>(), Enumerable.Empty<IWizardCommonPageSubscription<SamplePageContent>>())
	{
	}

	public SubscriptionPlugins(IEnumerable<IWizardCommonPageSubscription<CertificatePageContent>> certificateSubscriptions, IEnumerable<IWizardCommonPageSubscription<ServiceUserPageContent>> serviceUserSubscriptions, IEnumerable<IWizardCommonPageSubscription<WarningPageContent>> warningSubscriptions, IEnumerable<IWizardCommonPageSubscription<SamplePageContent>> samplePageSubscriptions)
	{
		_certificateSubscriptions = certificateSubscriptions;
		_serviceUserSubscriptions = serviceUserSubscriptions;
		_warningSubscriptions = warningSubscriptions;
		_samplePageSubscriptions = samplePageSubscriptions;
	}

	private ReadOnlyCollection<IWizardCommonPageSubscription<TContent>> GetSubscriptions<TContent>(IEnumerable<IWizardCommonPageSubscription<TContent>> subscriptions)
	{
		if (BypassFilter != null)
		{
			return BypassFilter.GetFilteredSubscriptions(subscriptions);
		}
		return subscriptions.ToList().AsReadOnly();
	}
}
