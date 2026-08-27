using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces.Content;

public interface IWizardCommonPageSubscription<TContent>
{
	Guid Id { get; }

	bool BeforeActivate(Dictionary<string, string> parameters);

	TContent GetContent();

	Dictionary<string, string> CollectParameters(TContent content);
}
