using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.UI.Framework;

public abstract class WizardPageSubscription<TContent, TViewModel> : WizardPageBase
{
	private System.Windows.Controls.UserControl _wpfControl;

	private TViewModel _viewModel;

	private List<IWizardCommonPageSubscription<TContent>> _activeSubscriptions;

	private Dictionary<IWizardCommonPageSubscription<TContent>, TContent> _subscriptionContent;

	private IContainer components;

	private ElementHost _elementHost;

	protected abstract IEnumerable<IWizardCommonPageSubscription<TContent>> Subscriptions { get; }

	protected WizardPageSubscription(string pageName, System.Windows.Controls.UserControl wpfControl)
	{
		InitializeComponent();
		_wpfControl = wpfControl;
		_elementHost.Child = _wpfControl;
		PageName = pageName;
		WizardPage.ShowFromNext += WizardPage_ShowFromNext;
		WizardPage.ShowFromBack += WizardPage_ShowFromBack;
	}

	public override bool OnBeforeActivate(Dictionary<string, string> parameters)
	{
		_activeSubscriptions = Subscriptions.Where((IWizardCommonPageSubscription<TContent> s) => s.BeforeActivate(parameters)).ToList();
		return _activeSubscriptions.Any();
	}

	public override Dictionary<string, string> CollectParameters()
	{
		if (_subscriptionContent != null)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			{
				foreach (KeyValuePair<IWizardCommonPageSubscription<TContent>, TContent> item in _subscriptionContent)
				{
					try
					{
						dictionary.AddRange(item.Key.CollectParameters(item.Value));
					}
					catch (ArgumentException innerException)
					{
						string text = "Subscription returned a parameter already defined by another subscription. Subscription id: " + item.Key.Id.ToString();
						LogManager.Write(string.Format(CultureInfo.InvariantCulture, "{0}.{1}()", GetType().Name, "CollectParameters"), text, TraceEventType.Critical);
						throw new ArgumentException(text, innerException);
					}
				}
				return dictionary;
			}
		}
		return null;
	}

	protected abstract TViewModel CreateViewModel(IEnumerable<TContent> content);

	protected abstract bool ValidateViewModel(TViewModel viewModel);

	protected void TriggerValidation()
	{
		Wizard.NextEnabled = ValidateViewModel(_viewModel);
	}

	private void WizardPage_ShowFromBack(object sender, EventArgs e)
	{
		PageActivated(reset: false);
	}

	private void WizardPage_ShowFromNext(object sender, EventArgs e)
	{
		PageActivated(reset: true);
	}

	private void PageActivated(bool reset)
	{
		if (reset || _viewModel == null)
		{
			ReloadViewModel();
		}
		TriggerValidation();
	}

	private void ReloadViewModel()
	{
		_subscriptionContent = _activeSubscriptions.ToDictionary((IWizardCommonPageSubscription<TContent> s) => s, (IWizardCommonPageSubscription<TContent> s) => s.GetContent());
		_viewModel = CreateViewModel(_subscriptionContent.Values);
		_wpfControl.DataContext = _viewModel;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this._elementHost = new System.Windows.Forms.Integration.ElementHost();
		base.SuspendLayout();
		this._elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
		this._elementHost.Location = new System.Drawing.Point(0, 0);
		this._elementHost.Name = "elementHost";
		this._elementHost.Size = new System.Drawing.Size(634, 492);
		this._elementHost.TabIndex = 1;
		this._elementHost.Child = null;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageSubscription";
		base.WizardPage.Controls.Add(this._elementHost);
		base.ResumeLayout(false);
	}
}
