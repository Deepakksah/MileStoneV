using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Core.Cab;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Framework.Globalization;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework;

public class WizardPageLanguage : WizardPageBase
{
	private delegate void BeginLanguageUncabDelegate(string cabFile, string destDir);

	private delegate void UncabEndedDelegate();

	public class CultureComparer : IComparer<string>
	{
		public int Compare(string x, string y)
		{
			try
			{
				return new CultureInfo(x).EnglishName.CompareTo(new CultureInfo(y).EnglishName);
			}
			catch (Exception)
			{
				return 0;
			}
		}
	}

	private string[] _installerLanguages;

	private string _defaultLanguage = "";

	private Wizard _wizard;

	private bool _isInitializing;

	private bool _disableContinue;

	private BeginLanguageUncabDelegate _uncabDelegate;

	private IContainer components;

	private Label labelHeadline;

	private ComboBox comboBoxInstallerLanguage;

	private Label labelInstallerLanguage;

	public event CultureChangedEventHandler CultureChanged;

	public event ErrorOccuredEventHandler ErrorOccurred;

	public WizardPageLanguage(string[] languages, string defaultLanguage, string singleLanguage, Wizard wizard)
	{
		_defaultLanguage = defaultLanguage;
		_installerLanguages = languages;
		_wizard = wizard;
		_uncabDelegate = null;
		InitializeComponent();
		_isInitializing = true;
		comboBoxInstallerLanguage.Items.Add(Resources.Language_Preparing);
		comboBoxInstallerLanguage.SelectedIndex = 0;
		_isInitializing = false;
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		string text = Path.Combine(directoryName, "lang.cab");
		_disableContinue = true;
		if (File.Exists(text))
		{
			if (!string.IsNullOrEmpty(singleLanguage))
			{
				DoLanguageUncab(text, directoryName, singleLanguage);
				return;
			}
			SynchronizationContext current = SynchronizationContext.Current;
			BeginLanguageUncab(text, directoryName, LanguageUncabEnded, current);
		}
		else
		{
			_disableContinue = false;
			LanguageUncabEndedMainThread(null);
		}
	}

	private void DoLanguageUncab(string cabFile, string destDir, string singleLanguage)
	{
		try
		{
			new CabHandler().ExtractFolder(cabFile, destDir, singleLanguage);
		}
		catch (CabException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), ex.StackTrace, TraceEventType.Error);
		}
	}

	private void DoLanguageUncab(string cabFile, string destDir)
	{
		try
		{
			new CabHandler().ExtractAll(cabFile, destDir);
		}
		catch (CabException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), ex.StackTrace, TraceEventType.Error);
		}
	}

	public IAsyncResult BeginLanguageUncab(string cabFile, string destDir, AsyncCallback callback, object asyncState)
	{
		_uncabDelegate = DoLanguageUncab;
		return _uncabDelegate.BeginInvoke(cabFile, destDir, callback, asyncState);
	}

	private void LanguageUncabEndedMainThread(object state)
	{
		_wizard.NextEnabled = true;
		_disableContinue = false;
		InitializePage();
	}

	private void LanguageUncabEnded(IAsyncResult ar)
	{
		try
		{
			if (_uncabDelegate != null)
			{
				_uncabDelegate.EndInvoke(ar);
			}
			((SynchronizationContext)ar.AsyncState).Send(LanguageUncabEndedMainThread, null);
		}
		catch (CabException)
		{
			throw;
		}
	}

	private void SortLanguages()
	{
		List<string> list = new List<string>();
		bool flag = false;
		string[] installerLanguages = _installerLanguages;
		foreach (string text in installerLanguages)
		{
			if (text == "en-US")
			{
				flag = true;
				list.Add(text);
			}
		}
		installerLanguages = _installerLanguages;
		foreach (string text2 in installerLanguages)
		{
			if (text2 != "en-US")
			{
				list.Add(text2);
			}
		}
		int num = (flag ? 1 : 0);
		list.Sort(num, list.Count - num, new CultureComparer());
		_installerLanguages = list.ToArray();
	}

	private void InitializePage()
	{
		LoadResources();
		ICultureProvider cultureProvider = new CultureProvider(_installerLanguages, throwOnUnsupported: false);
		foreach (Exception exception in cultureProvider.Exceptions)
		{
			LogManager.Write(exception.TypeAndMessage(), TraceEventType.Warning);
		}
		IOrderedEnumerable<CultureInfo> source = ((ICultureProvider)new RegionFreeCultures(cultureProvider)).SupportedCultures.OrderBy((CultureInfo x) => x.NativeName);
		comboBoxInstallerLanguage.Items.Clear();
		IEnumerable<string> source2 = source.Select((CultureInfo x) => x.NativeName);
		ComboBox.ObjectCollection items = comboBoxInstallerLanguage.Items;
		object[] items2 = source2.ToArray();
		items.AddRange(items2);
		_installerLanguages = source.Select((CultureInfo x) => x.Name).ToArray();
		comboBoxInstallerLanguage.SelectedItem = source.FirstOrDefault((CultureInfo x) => x.Name == _defaultLanguage)?.NativeName ?? source.FirstOrDefault((CultureInfo x) => x.Name == "en-US")?.NativeName ?? source2.FirstOrDefault();
	}

	private void LoadResources()
	{
		labelHeadline.Text = Resources.Language_LabelHeadLine;
		labelInstallerLanguage.Text = Resources.Language_LabelInstallerLanguage;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		Resources.Culture = culture;
		base.ReloadResources(culture);
		LoadResources();
	}

	private void SetCulture()
	{
		int selectedIndex = comboBoxInstallerLanguage.SelectedIndex;
		try
		{
			CultureInfo cultureInfo = ((_installerLanguages == null) ? new CultureInfo(_defaultLanguage) : new CultureInfo(_installerLanguages[selectedIndex]));
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			if (this.CultureChanged != null)
			{
				this.CultureChanged(this, new CultureChangedEventArgs(cultureInfo));
			}
			else
			{
				ReloadResources(cultureInfo);
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(ex.TypeAndMessage(), TraceEventType.Error);
			if (this.ErrorOccurred != null)
			{
				this.ErrorOccurred(this, new ErrorOccuredEventArgs(ex));
			}
		}
	}

	public override bool OnBeforeContinue()
	{
		SetCulture();
		return true;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		int selectedIndex = comboBoxInstallerLanguage.SelectedIndex;
		if (_installerLanguages != null)
		{
			dictionary.Add("LANGUAGE", _installerLanguages[selectedIndex]);
		}
		else
		{
			dictionary.Add("LANGUAGE", _defaultLanguage);
		}
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE") || GlobalData.GetValueAsBoolean("FINALIZE-MODE"))
		{
			dictionary.Add("TRECK_ID", "Typical");
			GlobalData.SetValue("INSTALLATION_MODE", "Typical", "WizardPageLanguage.CollectParameters()");
		}
		return dictionary;
	}

	private void WizardPage_Enter(object sender, EventArgs e)
	{
		Focus();
	}

	private void comboBoxInstallerLanguage_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!_isInitializing)
		{
			SetCulture();
		}
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		if (_disableContinue)
		{
			_wizard.NextEnabled = false;
			_disableContinue = false;
		}
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.Framework.WizardPageLanguage));
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelInstallerLanguage = new System.Windows.Forms.Label();
		this.comboBoxInstallerLanguage = new System.Windows.Forms.ComboBox();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.Controls.Add(this.comboBoxInstallerLanguage);
		base.WizardPage.Controls.Add(this.labelInstallerLanguage);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.FlowId = new System.Guid("33f60f0b-4946-4b7d-b2b7-79b028ee5cc8");
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.WizardPage.Enter += new System.EventHandler(WizardPage_Enter);
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		componentResourceManager.ApplyResources(this.labelInstallerLanguage, "labelInstallerLanguage");
		this.labelInstallerLanguage.Name = "labelInstallerLanguage";
		this.comboBoxInstallerLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		componentResourceManager.ApplyResources(this.comboBoxInstallerLanguage, "comboBoxInstallerLanguage");
		this.comboBoxInstallerLanguage.Name = "comboBoxInstallerLanguage";
		this.comboBoxInstallerLanguage.SelectedIndexChanged += new System.EventHandler(comboBoxInstallerLanguage_SelectedIndexChanged);
		base.Name = "WizardPageLanguage";
		this.PageName = "LanguagePage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}
