using System;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Installer.Interfaces;

[CLSCompliant(true)]
public interface IWizardPage
{
	IFlowManager FlowManager { get; set; }

	string PageName { get; set; }

	void SetParameters(Dictionary<string, string> parameters);

	List<string> GetEventParameter();

	Dictionary<string, string> CollectParameters();

	void ReloadResources(CultureInfo culture);

	bool OnBeforeContinue();

	string OnTriggerEvent();

	void OnBeforeCancel();

	bool OnBeforeActivate(Dictionary<string, string> parameters);

	bool OnBeforeActivateOnPrevious();
}
