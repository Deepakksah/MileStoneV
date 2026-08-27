using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using VideoOS.Platform.Client.Hid;
using VideoOS.Platform.License;
using VideoOS.Platform.RuleAction;
using VideoOS.Platform.Search;

namespace VideoOS.Platform;

public abstract class PluginDefinition
{
	private AdminPlacementHint _adminPlacementHint = AdminPlacementHint.Default;

	private bool _generateAlarmPreviewCalledInternally;

	public abstract Guid Id { get; }

	public abstract string Name { get; }

	public virtual string SharedNodeName => Name;

	public virtual string RolesTabName => Name;

	public virtual string VersionString => "1.0.0.0";

	public virtual string Manufacturer => "Your Company name goes here";

	public virtual Guid SharedNodeId => Guid.Empty;

	public abstract System.Drawing.Image Icon { get; }

	public virtual AdminPlacementHint AdminPlacementHint
	{
		get
		{
			return _adminPlacementHint;
		}
		set
		{
			_adminPlacementHint = value;
		}
	}

	public virtual bool ExcludeFromAboutBox { get; set; }

	public virtual bool UserControlFillEntirePanel => false;

	public virtual List<LoginPlugin> LoginPlugins => null;

	public virtual List<ViewItemPlugin> ViewItemPlugins => null;

	[Obsolete("This property is deprecated. Windows Forms controls are no longer supported. Use SettingsPanelPlugins instead.", false)]
	public virtual List<OptionsDialogPlugin> OptionsDialogPlugins => null;

	public virtual Collection<SettingsPanelPlugin> SettingsPanelPlugins => null;

	public virtual List<BackgroundPlugin> BackgroundPlugins => null;

	public virtual List<HidPlugin> HidPlugins => null;

	public virtual List<SidePanelPlugin> SidePanelPlugins => null;

	public virtual List<ViewItemToolbarPlugin> ViewItemToolbarPlugins => null;

	public virtual List<WorkSpacePlugin> WorkSpacePlugins => null;

	public virtual List<WorkSpaceToolbarPlugin> WorkSpaceToolbarPlugins => null;

	public virtual List<WorkSpaceToolbarPluginGroup> WorkSpaceToolbarPluginGroups => null;

	public virtual List<ToolsOptionsDialogPlugin> ToolsOptionsDialogPlugins => null;

	public virtual ICollection<TabPlugin> TabPlugins => null;

	public virtual IEnumerable<SearchAgentPlugin> SearchAgentPlugins => null;

	public virtual IEnumerable<SearchUserControlsPlugin> SearchUserControlsPlugins => null;

	public virtual IEnumerable<SearchToolbarPlugin> SearchToolbarPlugins => null;

	public virtual List<ViewLayout> ViewLayouts => null;

	public virtual List<ClientActionGroup> ClientActionGroups => null;

	public virtual List<ItemNode> ItemNodes => new List<ItemNode>();

	public virtual List<string> PluginDefinedMessageIds => null;

	public virtual List<SecurityAction> SecurityActions
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual bool IncludeInExport => false;

	public virtual bool ManageDatabaseExport => IncludeInExport;

	public virtual bool ManageMovieClipExport => false;

	public virtual bool ManageStillImageExport => false;

	public virtual Collection<LicenseInformation> PluginLicenseRequest => new Collection<LicenseInformation>();

	public virtual Collection<Guid> CustomSettingsGuids => new Collection<Guid>();

	public virtual Collection<Guid> OptionDialogGuids => new Collection<Guid>();

	public virtual ActionManager ActionManager => null;

	public virtual string CustomLicenseType => string.Empty;

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public virtual System.Windows.Forms.UserControl GenerateUserControl()
	{
		return new System.Windows.Forms.UserControl();
	}

	public virtual void ShowHelp()
	{
	}

	public virtual void ShowHelp(ItemNode itemNode)
	{
	}

	public virtual ExportManager GenerateExportManager(ExportParameters exportParameters)
	{
		return null;
	}

	public virtual System.Windows.Forms.UserControl GenerateAlarmPreviewUserControl(object alarmOrBaseEvent)
	{
		if (_generateAlarmPreviewCalledInternally)
		{
			return null;
		}
		_generateAlarmPreviewCalledInternally = true;
		System.Windows.Controls.UserControl userControl = GenerateAlarmPreviewWpfUserControl(alarmOrBaseEvent);
		_generateAlarmPreviewCalledInternally = false;
		if (userControl == null)
		{
			return null;
		}
		ElementHost elementHost = new ElementHost();
		elementHost.Child = userControl;
		elementHost.Dock = DockStyle.Fill;
		return new System.Windows.Forms.UserControl
		{
			Dock = DockStyle.Fill,
			Controls = { (System.Windows.Forms.Control)elementHost }
		};
	}

	public virtual System.Windows.Controls.UserControl GenerateAlarmPreviewWpfUserControl(object alarmOrBaseEvent)
	{
		if (_generateAlarmPreviewCalledInternally)
		{
			return null;
		}
		_generateAlarmPreviewCalledInternally = true;
		System.Windows.Forms.UserControl userControl = GenerateAlarmPreviewUserControl(alarmOrBaseEvent);
		_generateAlarmPreviewCalledInternally = false;
		if (userControl == null)
		{
			return null;
		}
		WindowsFormsHost windowsFormsHost = new WindowsFormsHost();
		windowsFormsHost.Child = userControl;
		return new System.Windows.Controls.UserControl
		{
			Content = windowsFormsHost
		};
	}
}
