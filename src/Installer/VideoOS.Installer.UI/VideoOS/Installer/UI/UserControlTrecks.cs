using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Configuration.TreckDescriptions;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class UserControlTrecks : UserControl
{
	private const int _widthPerc1 = 36;

	private const int _widthPerc2 = 64;

	private const int _numbRowsOptimal = 5;

	private Descriptions _descriptions;

	private bool _isPopulated;

	private IContainer components;

	private TableLayoutPanel tableLayoutPanelTrecks;

	public bool IsPopulated => _isPopulated;

	public event EventHandlers.TreckChangedEventHandler ButtonClicked;

	public UserControlTrecks()
	{
		InitializeComponent();
	}

	public void DrawLayout()
	{
		InstallerSettingsTypeFlowSettings20AdvancePageSettings advancePageSettings = InstallerManager.Instance.InstallerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings;
		LoadDescription();
		if (_descriptions == null && advancePageSettings.FlowTrecks.Length == 2)
		{
			string arg = VersionFormat.GenerateProductNameWithVersion();
			_descriptions = new Descriptions();
			_descriptions.Language = InstallerManager.Instance.Language;
			TreckDescriptionType[] array = new TreckDescriptionType[2]
			{
				new TreckDescriptionType(),
				null
			};
			array[0].ButtonName = Resources.InstallationType_ButtonInstallTypical;
			array[0].SideText = (InstallerManager.Instance.InstallerConfig.InstallerInfo.GroupInstaller ? string.Format(Resources.InstallationType_LabelInstallTypicalGroupDescription, arg) : string.Format(Resources.InstallationType_LabelInstallTypicalCompDescription, arg));
			array[0].Id = advancePageSettings.FlowTrecks[0].DescriptionId;
			array[1] = new TreckDescriptionType();
			array[1].ButtonName = Resources.InstallationType_ButtonInstallCustom;
			array[1].SideText = (InstallerManager.Instance.InstallerConfig.InstallerInfo.GroupInstaller ? Resources.InstallationType_LabelInstallCustomGroupDescription : Resources.InstallationType_LabelInstallCustomCompDescription);
			array[1].Id = advancePageSettings.FlowTrecks[1].DescriptionId;
			_descriptions.TreckDescriptions = array;
		}
		SuspendLayout();
		tableLayoutPanelTrecks.Controls.Clear();
		int num = 0;
		int numb = ((advancePageSettings.FlowTrecks.Length < 5) ? 5 : advancePageSettings.FlowTrecks.Length);
		if (advancePageSettings.FlowTrecks != null)
		{
			TreckType[] flowTrecks = advancePageSettings.FlowTrecks;
			foreach (TreckType treck in flowTrecks)
			{
				Label label = CreateLabel((_descriptions != null) ? _descriptions.TreckDescriptions.Where((TreckDescriptionType treckDescriptionType) => treckDescriptionType.Id == treck.DescriptionId) : null, num, advancePageSettings.FlowTrecks.Length);
				bool flag = (label.Enabled = IsControlEnabled(treck));
				Button button = CreateButton((_descriptions != null) ? _descriptions.TreckDescriptions.Where((TreckDescriptionType treckDescriptionType) => treckDescriptionType.Id == treck.DescriptionId) : null, num, numb, treck.Id);
				button.UseVisualStyleBackColor = flag;
				button.Enabled = flag;
				tableLayoutPanelTrecks.Controls.Add(button, 0, num);
				tableLayoutPanelTrecks.Controls.Add(label, 1, num);
				num++;
			}
			foreach (RowStyle item in (IEnumerable)tableLayoutPanelTrecks.RowStyles)
			{
				item.SizeType = SizeType.Absolute;
				item.Height = base.Height / advancePageSettings.FlowTrecks.Length;
			}
		}
		_isPopulated = true;
		ResumeLayout();
	}

	private static bool IsControlEnabled(TreckType treck)
	{
		if (InstallerManager.Instance.Parameters.ContainsKey("IS_UPGRADE") && InstallerManager.Instance.Parameters["IS_UPGRADE"] == "true")
		{
			if (!InstallerManager.Instance.Parameters.TryGetValue("TRECK_ID", out var value) && InstallerManager.Instance.Parameters.ContainsKey("QUIET-MODE") && InstallerManager.Instance.Parameters["QUIET-MODE"] == "true")
			{
				value = "Typical";
			}
			else if (!InstallerManager.Instance.Parameters.ContainsKey("TRECK_ID"))
			{
				return true;
			}
			return treck.DoesAllowUpgradeFrom(value);
		}
		return true;
	}

	private void LoadDescription()
	{
		try
		{
			_descriptions = InstallerConfigManager.LoadTreckDescriptions(Path.Combine(InstallerManager.Instance.CurrentDirectory, InstallerManager.Instance.Language));
			return;
		}
		catch (Exception)
		{
			_descriptions = null;
		}
		if (InstallerManager.Instance.Language != "en-US")
		{
			try
			{
				_descriptions = InstallerConfigManager.LoadTreckDescriptions(Path.Combine(InstallerManager.Instance.CurrentDirectory, "en-US"));
				return;
			}
			catch (Exception)
			{
				_descriptions = null;
			}
		}
		try
		{
			_descriptions = InstallerConfigManager.LoadTreckDescriptions(InstallerManager.Instance.CurrentDirectory);
		}
		catch (Exception)
		{
			_descriptions = null;
		}
	}

	private Button CreateButton(IEnumerable<TreckDescriptionType> descrs, int index, int numb, string id)
	{
		int num = base.Width * 36 / 100 - 30;
		int num2 = base.Height / numb - 25;
		Button button = new Button();
		button.Anchor = AnchorStyles.Top | AnchorStyles.Left;
		button.Click += _buttonTemplate_Click;
		button.TextAlign = ContentAlignment.MiddleCenter;
		button.Size = new Size(num, num2);
		button.Font = new Font("Tahoma", 11f);
		button.FlatStyle = FlatStyle.System;
		button.UseVisualStyleBackColor = true;
		button.Name = string.Format(CultureInfo.InvariantCulture, "{0}{1}", "buttonTemplate", index);
		if (descrs != null && descrs.Count() != 0)
		{
			TreckDescriptionType treckDescriptionType = descrs.Single();
			button.Text = treckDescriptionType.ButtonName;
			button.Tag = id;
		}
		return button;
	}

	private Label CreateLabel(IEnumerable<TreckDescriptionType> descrs, int index, int numb)
	{
		int num = base.Width * 64 / 100 - 20;
		int num2 = base.Height / numb;
		Label label = new Label();
		label.Anchor = AnchorStyles.Top | AnchorStyles.Left;
		label.TextAlign = ContentAlignment.TopLeft;
		label.Size = new Size(num, num2);
		label.Font = new Font("Arial", 9f);
		label.Name = string.Format(CultureInfo.InvariantCulture, "{0}{1}", "LabelTemplate", index);
		label.UseMnemonic = false;
		if (descrs != null && descrs.Count() != 0)
		{
			TreckDescriptionType treckDescriptionType = descrs.Single();
			label.Text = treckDescriptionType.SideText;
		}
		return label;
	}

	private void _buttonTemplate_Click(object sender, EventArgs e)
	{
		Button button = sender as Button;
		if (this.ButtonClicked != null)
		{
			this.ButtonClicked(this, new TreckChangedEventArgs(button.Tag as string));
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
		this.tableLayoutPanelTrecks = new System.Windows.Forms.TableLayoutPanel();
		base.SuspendLayout();
		this.tableLayoutPanelTrecks.ColumnCount = 2;
		this.tableLayoutPanelTrecks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36f));
		this.tableLayoutPanelTrecks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 64f));
		this.tableLayoutPanelTrecks.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanelTrecks.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanelTrecks.Name = "tableLayoutPanelTrecks";
		this.tableLayoutPanelTrecks.RowCount = 1;
		this.tableLayoutPanelTrecks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanelTrecks.Size = new System.Drawing.Size(540, 334);
		this.tableLayoutPanelTrecks.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.tableLayoutPanelTrecks);
		base.Name = "UserControlTrecks";
		base.Size = new System.Drawing.Size(540, 334);
		base.ResumeLayout(false);
	}
}
