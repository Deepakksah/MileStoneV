using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageServerSelection : WizardPageBase
{
	private delegate bool BeginTryManServersDelegate(Uri managementServerAddress);

	private delegate void ShowNextDelegate(bool res);

	private bool _serverChecked;

	private bool _serverUrlLocked;

	private ManagementServerInfo _managementServerInfo;

	private Uri _managementServerAddress;

	private IContainer components;

	private TextBox textBoxServerUrl;

	private Label labelHeadline;

	private Label labelSubHeading;

	private Label labelMessage;

	private Label labelExample;

	private PictureBox errorIcon;

	public WizardPageServerSelection()
	{
		InitializeComponent();
		LoadResources();
	}

	private void LoadResources()
	{
		labelHeadline.Text = Resources.ServerSelection_LabelHeader;
		labelSubHeading.Text = Resources.ServerSelection_LabelSubHeader;
		labelExample.Text = Resources.ServerSelection_LabelExample;
	}

	private void textBoxServerUrl_TextChanged(object sender, EventArgs e)
	{
		_serverChecked = false;
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.NextButton.Show();
		Wizard.CancelButton.Text = Resources.btn_Cancel;
		if (Wizard.BackButton.Location == Wizard.NextButton.Location)
		{
			Wizard.BackButton.Location = new Point(Wizard.BackButton.Location.X - Wizard.BackButton.Width - 2, Wizard.BackButton.Location.Y);
		}
		UpdateMessage(string.Empty, isError: false);
	}

	public override void SetParameters(Dictionary<string, string> parameters)
	{
		string text = null;
		if (parameters.ContainsKey("MANAGEMENTSERVERURI") && HostHelper.ValidateAddress(parameters["MANAGEMENTSERVERURI"], out var _))
		{
			text = parameters["MANAGEMENTSERVERURI"];
		}
		textBoxServerUrl.Text = text ?? parameters.GetValueOrNull("MANAGEMENTAPPNAME") ?? string.Empty;
		_serverUrlLocked = parameters.TryGetValue("LOCKMANAGEMENTSERVER", out var value) && value == "true";
		textBoxServerUrl.Enabled = !_serverUrlLocked;
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (HostHelper.ValidateAddress(textBoxServerUrl.Text, out var _))
		{
			dictionary["MANAGEMENTAPPNAME"] = _managementServerInfo?.HttpAddress.AbsoluteUri;
			dictionary["MANAGEMENTAPPNAMESECURE"] = _managementServerInfo?.HttpsAddress.AbsoluteUri;
			dictionary["MANAGEMENTSERVERURI"] = _managementServerAddress.AbsoluteUri;
			dictionary["IDPADDRESS"] = _managementServerInfo?.IDPAddress.AbsoluteUri;
		}
		return dictionary;
	}

	public override bool OnBeforeContinue()
	{
		if (_serverChecked)
		{
			return true;
		}
		using (new ShowWaitCursor())
		{
			bool flag = true;
			try
			{
				if (HostHelper.ValidateAddress(textBoxServerUrl.Text, out var validatedAddress))
				{
					Wizard.BackButton.Enabled = false;
					Wizard.NextButton.Enabled = false;
					textBoxServerUrl.Enabled = false;
					UpdateMessage(Resources.ServerSelection_TryingToConnect, isError: false);
					BeginTryManServer(EndTryManServer, validatedAddress);
					return false;
				}
				flag = false;
			}
			catch (Exception)
			{
				flag = false;
			}
			if (!flag)
			{
				UpdateMessage(Resources.ErrorMessage_InvalidUrlText, isError: true);
			}
			return false;
		}
	}

	public IAsyncResult BeginTryManServer(AsyncCallback callback, Uri managementServerAddress)
	{
		BeginTryManServersDelegate beginTryManServersDelegate = TryLoginToServer;
		return beginTryManServersDelegate.BeginInvoke(managementServerAddress, callback, beginTryManServersDelegate);
	}

	public void EndTryManServer(IAsyncResult asyncResult)
	{
		bool res = ((BeginTryManServersDelegate)asyncResult.AsyncState).EndInvoke(asyncResult);
		Next(res);
	}

	private void UpdateMessage(string message, bool isError)
	{
		if (labelHeadline.InvokeRequired)
		{
			labelHeadline.Invoke((Action)delegate
			{
				UpdateMessage(message, isError);
			});
			return;
		}
		if (errorIcon.Visible != isError)
		{
			errorIcon.Visible = isError;
			labelMessage.Left += (isError ? 20 : (-20));
			labelMessage.Width += (isError ? (-20) : 20);
		}
		labelMessage.Text = message;
		if (isError)
		{
			textBoxServerUrl.Focus();
		}
	}

	private void ShowErrorPage(UIFrameworkErrorException ex)
	{
		if (labelHeadline.InvokeRequired)
		{
			labelHeadline.Invoke((Action)delegate
			{
				ShowErrorPage(ex);
			});
		}
		else
		{
			ErrorHandler.ShowError(ex);
		}
	}

	private void Next(bool res)
	{
		if (labelHeadline.InvokeRequired)
		{
			ShowNextDelegate method = Next;
			labelHeadline.Invoke(method, res);
			return;
		}
		Wizard.BackButton.Enabled = true;
		Wizard.NextButton.Enabled = true;
		textBoxServerUrl.Enabled = !_serverUrlLocked;
		_serverChecked = res;
		if (res && Wizard.OnButtonNext != null)
		{
			Wizard.OnButtonNext(Wizard);
		}
	}

	private bool TryLoginToServer(Uri managementServerAddress)
	{
		try
		{
			using (new ShowWaitCursor())
			{
				if (new ServerHelper().TryGetManagementServerInfo(managementServerAddress, out _managementServerInfo))
				{
					if (managementServerAddress.Scheme != _managementServerInfo.IDPAddress.Scheme)
					{
						if (_managementServerInfo.IDPAddress.Scheme == Uri.UriSchemeHttp)
						{
							throw new ServerHelper.ServerException(Resources.ErrorMessage_ServerIsRunningUnsecure);
						}
						throw new ServerHelper.ServerException(Resources.ErrorMessage_ServerIsRunningSecure);
					}
					_managementServerAddress = managementServerAddress;
					if (ServerHelper.TryLoginToCorporate(managementServerAddress, _managementServerInfo.IDPAddress))
					{
						return true;
					}
				}
				throw new ServerHelper.ServerException(Resources.ServerSelection_ServerNotFound);
			}
		}
		catch (ServerHelper.ServerException ex)
		{
			UpdateMessage(ex.Message, isError: true);
		}
		catch (Exception ex2)
		{
			ShowErrorPage(new UIFrameworkErrorException(ex2, enablePreviousButtong: true));
		}
		return false;
	}

	private Uri CreateUri(string uriString)
	{
		if (Uri.TryCreate(uriString, UriKind.Absolute, out var result) && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps))
		{
			return result;
		}
		int port = 80;
		int num = uriString.IndexOf(":");
		if (num != -1 && int.TryParse(uriString.Split(':')[1], out var result2))
		{
			port = result2;
			uriString = uriString.Remove(num, uriString.Length - num);
		}
		UriBuilder uriBuilder = new UriBuilder();
		uriBuilder.Host = uriString;
		uriBuilder.Port = port;
		uriBuilder.Scheme = "http";
		try
		{
			return uriBuilder.Uri;
		}
		catch (UriFormatException)
		{
		}
		return null;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
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
		this.textBoxServerUrl = new System.Windows.Forms.TextBox();
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelSubHeading = new System.Windows.Forms.Label();
		this.labelMessage = new System.Windows.Forms.Label();
		this.labelExample = new System.Windows.Forms.Label();
		this.errorIcon = new System.Windows.Forms.PictureBox();
		base.WizardPage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.errorIcon).BeginInit();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.errorIcon);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.labelSubHeading);
		base.WizardPage.Controls.Add(this.textBoxServerUrl);
		base.WizardPage.Controls.Add(this.labelMessage);
		base.WizardPage.Controls.Add(this.labelExample);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("dec26ad0-4da2-4ca3-96ae-33461a9aebd9");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "ServerSelectionPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.textBoxServerUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f);
		this.textBoxServerUrl.Location = new System.Drawing.Point(49, 176);
		this.textBoxServerUrl.Name = "textBoxServerUrl";
		this.textBoxServerUrl.Size = new System.Drawing.Size(429, 22);
		this.textBoxServerUrl.TabIndex = 8;
		this.textBoxServerUrl.Text = "http://localhost";
		this.textBoxServerUrl.TextChanged += new System.EventHandler(textBoxServerUrl_TextChanged);
		this.labelHeadline.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelHeadline.Location = new System.Drawing.Point(44, 40);
		this.labelHeadline.Name = "labelHeadline";
		this.labelHeadline.Size = new System.Drawing.Size(539, 50);
		this.labelHeadline.TabIndex = 6;
		this.labelHeadline.Text = "[Specify Management Server]";
		this.labelSubHeading.Font = new System.Drawing.Font("Arial", 9f);
		this.labelSubHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelSubHeading.Location = new System.Drawing.Point(46, 131);
		this.labelSubHeading.Name = "labelSubHeading";
		this.labelSubHeading.Size = new System.Drawing.Size(540, 42);
		this.labelSubHeading.TabIndex = 7;
		this.labelSubHeading.Text = "[Specify URL URL to the management server:";
		this.labelMessage.Font = new System.Drawing.Font("Arial", 9f);
		this.labelMessage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelMessage.Location = new System.Drawing.Point(66, 243);
		this.labelMessage.Name = "labelMessage";
		this.labelMessage.Size = new System.Drawing.Size(520, 63);
		this.labelMessage.TabIndex = 8;
		this.labelMessage.Text = "[Message]";
		this.labelExample.Font = new System.Drawing.Font("Arial", 9f);
		this.labelExample.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelExample.Location = new System.Drawing.Point(46, 205);
		this.labelExample.Name = "labelExample";
		this.labelExample.Size = new System.Drawing.Size(540, 42);
		this.labelExample.TabIndex = 9;
		this.labelExample.Text = "Example...";
		this.errorIcon.BackColor = System.Drawing.Color.Transparent;
		this.errorIcon.Image = VideoOS.Installer.UI.Properties.Resources.error;
		this.errorIcon.Location = new System.Drawing.Point(46, 243);
		this.errorIcon.Name = "errorIcon";
		this.errorIcon.Size = new System.Drawing.Size(16, 16);
		this.errorIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
		this.errorIcon.TabIndex = 1;
		this.errorIcon.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageServerSelection";
		this.PageName = "ServerSelectionPage";
		((System.ComponentModel.ISupportInitialize)this.errorIcon).EndInit();
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}
