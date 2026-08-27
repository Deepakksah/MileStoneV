using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces;

public interface IFlowManager
{
	Dictionary<string, IPageSetupActions> PageSetupActions { get; set; }

	void Next();

	void Back();

	void TriggerEvent(string eventName);

	void TriggerEventWithParameters(string flowEvent, Dictionary<string, string> parameters);
}
