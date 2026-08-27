using System;
using System.Collections.Generic;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.UI;

public class FlowManager : IFlowManager
{
	private Dictionary<string, Dictionary<string, FlowPage>> _flowTreck = new Dictionary<string, Dictionary<string, FlowPage>>();

	private Dictionary<string, IWizardPage> _pages = new Dictionary<string, IWizardPage>();

	private Stack<IWizardPage> _stack = new Stack<IWizardPage>();

	private Wizard _wizard;

	private Dictionary<string, string> _parameters;

	public IWizardPage ActivePage { get; set; }

	public IPageSetupActions ActivePageSetupActions
	{
		get
		{
			IWizardPage activePage = ActivePage;
			if (activePage != null && activePage.FlowManager?.PageSetupActions?.ContainsKey(ActivePage.PageName) == true)
			{
				return PageSetupActions[ActivePage.PageName];
			}
			return null;
		}
	}

	public string ActiveTreckKey { get; set; }

	public PageSequencerManager PageSequencer { get; set; }

	public Dictionary<string, IPageSetupActions> PageSetupActions { get; set; }

	public FlowManager(Wizard wizard, Dictionary<string, IPageSetupActions> pageSetupActions)
	{
		_wizard = wizard;
		PageSetupActions = pageSetupActions;
	}

	public void Back()
	{
		IWizardPage wizardPage = _stack.Pop();
		if (wizardPage != null)
		{
			if (wizardPage.PageName == "InitializePage" || !wizardPage.OnBeforeActivateOnPrevious())
			{
				wizardPage = _stack.Pop();
			}
			WizardPageControl wizardPageControl = wizardPage as WizardPageControl;
			_wizard.BackTo(wizardPageControl.Page);
			ActivePage = wizardPage;
		}
	}

	public void Next()
	{
		TriggerEvent(null);
	}

	public bool IsPagePostInstallation(string pageName)
	{
		if (_flowTreck[ActiveTreckKey].TryGetValue($"{pageName}.NextPage", out var value))
		{
			return value.PostInstall;
		}
		return false;
	}

	public void TriggerEvent(string flowEvent)
	{
		if (flowEvent == null)
		{
			flowEvent = "NextPage";
		}
		string key = ActivePage.PageName + "." + flowEvent;
		Dictionary<string, FlowPage> dictionary = _flowTreck[ActiveTreckKey];
		if (dictionary.ContainsKey(key))
		{
			FlowPage flowPage = dictionary[key];
			if (flowEvent == "NextPage")
			{
				if (!(ActivePage as WizardPageControl).Page.FinishPage)
				{
					if (!ActivateNewPage(flowPage))
					{
						TriggerEvent(null);
					}
				}
				else
				{
					_wizard.Next();
				}
			}
			else if (!ActivateNewPage(flowPage))
			{
				TriggerEvent(null);
			}
		}
		else
		{
			TriggerEvent(null);
		}
	}

	public void TriggerEventWithParameters(string flowEvent, Dictionary<string, string> parameters)
	{
		TriggerEvent(flowEvent);
		ActivePage.SetParameters(parameters);
	}

	public bool ActivateNewPage(FlowPage flowPage)
	{
		if (!_pages.ContainsKey(flowPage.NextStep))
		{
			return false;
		}
		if (PageSequencer.IsPageShown(ActiveTreckKey, ActivePage.PageName))
		{
			_stack.Push(ActivePage);
		}
		IWizardPage wizardPage = _pages[flowPage.NextStep];
		WizardPageControl wizardPageControl = wizardPage as WizardPageControl;
		ActivePage = wizardPage;
		if (!PageSequencer.CheckBeforeActivate(ActiveTreckKey, ActivePage.PageName))
		{
			return false;
		}
		bool errorOccured = false;
		bool flag = RunBeforeActivate(out errorOccured);
		if (errorOccured)
		{
			return true;
		}
		if (!flag)
		{
			if (PageSequencer.IsPageShown(ActiveTreckKey, ActivePage.PageName))
			{
				PageSequencer.AddSkipPage(ActiveTreckKey, ActivePage.PageName);
			}
			return false;
		}
		_wizard.NextTo(wizardPageControl.Page);
		return true;
	}

	public void AddFlowTreckPages(KeyValuePair<string, List<FlowPage>> treck)
	{
		Dictionary<string, FlowPage> dictionary = new Dictionary<string, FlowPage>();
		foreach (FlowPage item in treck.Value)
		{
			item.Page.FlowManager = this;
			if (item.Event == null)
			{
				item.Event = "NextPage";
			}
			string key = item.Name + "." + item.Event;
			if (!_pages.ContainsKey(item.Name))
			{
				_pages.Add(item.Name, item.Page);
			}
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, item);
			}
		}
		_flowTreck.Add(treck.Key, dictionary);
	}

	public void SetContinueFromPage(FlowPage page)
	{
		WizardPageControl wizardPageControl = _pages[page.Name] as WizardPageControl;
		_wizard.SetContinueFromPage(wizardPageControl.Page);
	}

	public void SetParametersProp(Dictionary<string, string> paramters)
	{
		_parameters = paramters;
	}

	private bool RunBeforeActivate(out bool errorOccured)
	{
		try
		{
			errorOccured = false;
			bool flag = ActivePage.OnBeforeActivate(_parameters);
			if (ActivePage.FlowManager.PageSetupActions == null || !ActivePage.FlowManager.PageSetupActions.ContainsKey(ActivePage.PageName))
			{
				return flag;
			}
			return ActivePage.FlowManager.PageSetupActions[ActivePage.PageName].BeforeActivatePage(_parameters) && flag;
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			ErrorHandler.ShowError(exception);
			errorOccured = true;
		}
		return false;
	}

	public bool RunBeforeLeave(out bool errorOccured)
	{
		errorOccured = false;
		try
		{
			bool flag = ActivePage.OnBeforeContinue();
			IPageSetupActions activePageSetupActions = ActivePageSetupActions;
			if (activePageSetupActions == null)
			{
				return flag;
			}
			return flag && activePageSetupActions.BeforeLeavePage(_parameters);
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			ErrorHandler.ShowError(exception);
			errorOccured = true;
		}
		return false;
	}
}
