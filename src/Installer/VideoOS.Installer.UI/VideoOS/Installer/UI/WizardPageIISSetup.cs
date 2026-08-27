using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Framework.IISSetup;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageIISSetup : WizardPageBase
{
	private IEnumerable<SiteInfo> _webSites;

	private IContainer components;

	private Label labelDetails;

	private Label labelHeadline;

	private Label labelDetailsDescription;

	private ListView listViewWebSites;

	private ColumnHeader Description;

	private ColumnHeader State;

	private ColumnHeader Address;

	private ColumnHeader HttpPort;

	private ColumnHeader HttpsPort;

	protected SiteInfo WebsiteToUse { get; private set; }

	public WizardPageIISSetup()
	{
		InitializeComponent();
	}

	private void LoadResources()
	{
		labelHeadline.Text = string.Format(Resources.IISSetup_HeadLine, VersionFormat.GenerateProductNameWithVersion());
		labelDetails.Text = Resources.IISSetup_Details;
		labelDetailsDescription.Text = Resources.IISSetup_DetailsDesc;
		Description.Text = Resources.IISSetup_ListDescription;
		State.Text = Resources.IISSetup_ListState;
		Address.Text = Resources.IISSetup_ListAddress;
		HttpPort.Text = Resources.IISSetup_ListHttpPort;
		HttpsPort.Text = Resources.IISSetup_ListHttpsPort;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	public override bool OnBeforeActivateOnPrevious()
	{
		if (base.OnBeforeActivateOnPrevious() && !_webSites.Any())
		{
			return false;
		}
		return true;
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		LoadResources();
		Wizard.BackButton.Show();
		Wizard.BackButton.Enabled = true;
		FillInWebSites();
		Wizard.NextButton.Show();
		Wizard.CancelButton.Text = Resources.btn_Cancel;
		if (Wizard.BackButton.Location == Wizard.NextButton.Location)
		{
			Wizard.BackButton.Location = new Point(Wizard.BackButton.Location.X - Wizard.BackButton.Width - 2, Wizard.BackButton.Location.Y);
		}
	}

	public override bool OnBeforeActivate()
	{
		return true;
	}

	private void FillInWebSites()
	{
		listViewWebSites.Items.Clear();
		string[] array = null;
		if (GlobalData.TryGetValue("ConfiguredSitesIds", out var value))
		{
			string[] separator = new string[1] { ";" };
			array = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		}
		_webSites = WebSiteExplorer.GetWebSites();
		if (_webSites.Any())
		{
			foreach (SiteInfo webSite in _webSites)
			{
				string text = ((webSite.HttpPort > 0) ? webSite.HttpPort.ToString() : "N/A");
				string text2 = ((webSite.HttpsPort > 0) ? webSite.HttpsPort.ToString() : "N/A");
				ListViewItem listViewItem = new ListViewItem(new string[5]
				{
					webSite.Name,
					webSite.State.ToString(),
					webSite.Address,
					text,
					text2
				});
				if (array == null || array.Contains(webSite.Id))
				{
					listViewItem.Tag = webSite;
				}
				else
				{
					listViewItem.ForeColor = Color.DarkGray;
					listViewItem.Tag = null;
				}
				listViewWebSites.Items.Add(listViewItem);
			}
			int num = 0;
			if (GlobalData.TryGetValue("WebSiteId", out var value2))
			{
				foreach (ListViewItem item in listViewWebSites.Items)
				{
					if (item.Tag is SiteInfo siteInfo && siteInfo.Id.Equals(value2, StringComparison.OrdinalIgnoreCase))
					{
						num = item.Index;
						break;
					}
				}
			}
			if (listViewWebSites.Items.Count - 1 >= num)
			{
				listViewWebSites.SelectedItems.Clear();
				listViewWebSites.Items[num].Selected = true;
				listViewWebSites.Items[num].Focused = true;
				listViewWebSites.EnsureVisible(num);
				if (GlobalData.GetValueAsBoolean("IS_UPGRADE") && GlobalData.HasParameter("WebSiteId") && listViewWebSites.Items[num].Tag != null)
				{
					listViewWebSites.Enabled = false;
				}
			}
		}
		else
		{
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_GeneralErrorLabelHeadLine, Resources.IISSetup_NoWebSiteFound, null, null, enablePreviousButton: true);
			ErrorHandler.ShowError();
		}
	}

	public override bool OnBeforeContinue()
	{
		if (listViewWebSites.SelectedItems.Count == 0)
		{
			return false;
		}
		WebsiteToUse = (SiteInfo)listViewWebSites.SelectedItems[0].Tag;
		if (WebsiteToUse != null)
		{
			if (WebsiteToUse.HttpsPort == 0)
			{
				WebsiteToUse = new SiteInfo(WebsiteToUse.Id, WebsiteToUse.Name, WebsiteToUse.Address, WebsiteToUse.HttpPort, 443, (int)WebsiteToUse.State.State);
			}
			if (WebsiteToUse.HttpPort == 0)
			{
				WebsiteToUse = new SiteInfo(WebsiteToUse.Id, WebsiteToUse.Name, WebsiteToUse.Address, 80, WebsiteToUse.HttpsPort, (int)WebsiteToUse.State.State);
			}
			GlobalData.SetValue("WebSiteId", WebsiteToUse.Id, "IISSetup.OnBeforeContinue()");
			GlobalData.SetValue("WebSiteName", WebsiteToUse.Name, "IISSetup.OnBeforeContinue()");
			GlobalData.SetValue("WebSiteAddress", WebsiteToUse.Address, "IISSetup.OnBeforeContinue()");
			GlobalData.SetValue("WebSitePort", WebsiteToUse.HttpPort.ToString(), "IISSetup.OnBeforeContinue()");
			GlobalData.SetValue("WebSiteSecurePort", WebsiteToUse.HttpsPort.ToString(), "IISSetup.OnBeforeContinue()");
			GlobalData.SetValue("ServerState", Convert.ToInt16(WebsiteToUse.State.State).ToString(), "IISSetup.OnBeforeContinue()");
			return true;
		}
		MessageHelper.ShowInstallerMessageBox(Resources.ErrorMessage_GeneralErrorLabelHeadLine, Resources.IISSetup_NotConnfigured, InstallerMessageBoxIcon.Warning);
		GlobalData.RemoveValue("WebSiteId");
		GlobalData.RemoveValue("WebSiteName");
		GlobalData.RemoveValue("WebSiteAddress");
		GlobalData.RemoveValue("WebSitePort");
		GlobalData.RemoveValue("WebSiteSecurePort");
		GlobalData.RemoveValue("ServerState");
		LogManager.Write("Selected website can't be used. It's not configured correctly");
		return false;
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
		this.labelDetails = new System.Windows.Forms.Label();
		this.labelHeadline = new System.Windows.Forms.Label();
		this.labelDetailsDescription = new System.Windows.Forms.Label();
		this.listViewWebSites = new System.Windows.Forms.ListView();
		this.Description = new System.Windows.Forms.ColumnHeader();
		this.State = new System.Windows.Forms.ColumnHeader();
		this.Address = new System.Windows.Forms.ColumnHeader();
		this.HttpPort = new System.Windows.Forms.ColumnHeader();
		this.HttpsPort = new System.Windows.Forms.ColumnHeader();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.listViewWebSites);
		base.WizardPage.Controls.Add(this.labelDetailsDescription);
		base.WizardPage.Controls.Add(this.labelDetails);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.labelDetails.AutoSize = true;
		this.labelDetails.BackColor = System.Drawing.Color.Transparent;
		this.labelDetails.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelDetails.Location = new System.Drawing.Point(47, 296);
		this.labelDetails.Name = "labelDetails";
		this.labelDetails.Size = new System.Drawing.Size(46, 14);
		this.labelDetails.TabIndex = 4;
		this.labelDetails.Text = "Details:";
		this.labelHeadline.Font = new System.Drawing.Font("Arial", 16f);
		this.labelHeadline.ImeMode = System.Windows.Forms.ImeMode.NoControl;
		this.labelHeadline.Location = new System.Drawing.Point(44, 40);
		this.labelHeadline.Name = "labelHeadline";
		this.labelHeadline.Size = new System.Drawing.Size(540, 50);
		this.labelHeadline.TabIndex = 0;
		this.labelHeadline.Text = "Select a website on the IIS to use with the {0}";
		this.labelDetailsDescription.BackColor = System.Drawing.Color.Transparent;
		this.labelDetailsDescription.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.labelDetailsDescription.Location = new System.Drawing.Point(47, 314);
		this.labelDetailsDescription.Name = "labelDetailsDescription";
		this.labelDetailsDescription.Size = new System.Drawing.Size(537, 88);
		this.labelDetailsDescription.TabIndex = 5;
		this.labelDetailsDescription.Text = "The web site is running and can be used for the installation.";
		this.listViewWebSites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[5] { this.Description, this.State, this.Address, this.HttpPort, this.HttpsPort });
		this.listViewWebSites.HideSelection = false;
		this.listViewWebSites.Location = new System.Drawing.Point(50, 150);
		this.listViewWebSites.Name = "listViewWebSites";
		this.listViewWebSites.Size = new System.Drawing.Size(534, 90);
		this.listViewWebSites.TabIndex = 2;
		this.listViewWebSites.UseCompatibleStateImageBehavior = false;
		this.listViewWebSites.View = System.Windows.Forms.View.Details;
		this.listViewWebSites.FullRowSelect = true;
		this.Description.Text = "Description";
		this.Description.Width = 170;
		this.State.Text = "State";
		this.State.Width = 90;
		this.Address.Text = "Address";
		this.Address.Width = 100;
		this.HttpPort.Text = "HTTP port";
		this.HttpPort.Width = 85;
		this.HttpsPort.Text = "HTTPS port";
		this.HttpsPort.Width = 85;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageIISSetup";
		this.PageName = "IISSetupPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}
