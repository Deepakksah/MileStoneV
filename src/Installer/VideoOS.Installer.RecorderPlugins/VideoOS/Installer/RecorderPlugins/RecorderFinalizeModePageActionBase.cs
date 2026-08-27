using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using VideoOS.Installer.Common;
using VideoOS.Installer.RecorderPlugins.Properties;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Framework.PageActions;

namespace VideoOS.Installer.RecorderPlugins;

public abstract class RecorderFinalizeModePageActionBase : ServerSelectionPageActionsBase
{
	private IGlobalData globalData;

	private IPluginUtilities pluginUtilities;

	private ILogManager logManager;

	protected RecorderFinalizeModePageActionBase()
		: this(new GlobalDataWrapper(), PluginUtilities.Instance, new LogManagerWrapper())
	{
	}

	public RecorderFinalizeModePageActionBase(IGlobalData globalData, IPluginUtilities pluginUtilities, ILogManager logManager)
	{
		this.globalData = globalData;
		this.pluginUtilities = pluginUtilities;
		this.logManager = logManager;
	}

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (globalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			return false;
		}
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		return CheckServerTypeValidity(parameters);
	}

	private bool CheckServerTypeValidity(Dictionary<string, string> parameters)
	{
		PluginUtilities.MilestoneServerTypes milestoneServerTypes = pluginUtilities.InstalledMilestoneServices();
		bool flag = milestoneServerTypes.HasFlag(PluginUtilities.MilestoneServerTypes.Failover);
		bool flag2 = milestoneServerTypes.HasFlag(PluginUtilities.MilestoneServerTypes.RecordingServer);
		if (!flag && !flag2)
		{
			logManager.Write("Blank system - neither failover or recording server installed: installation permitted");
			return base.BeforeActivatePage(parameters);
		}
		if (flag)
		{
			logManager.Write("Failover service is installed - checking if user selected failover installation, else installation will be aborted");
			if (pluginUtilities.FailoverInstallation)
			{
				return base.BeforeActivatePage(parameters);
			}
		}
		else
		{
			logManager.Write("Recording server is installed - checking if user selected recording server installation. If not, installation will be aborted");
			if (!pluginUtilities.FailoverInstallation)
			{
				return base.BeforeActivatePage(parameters);
			}
		}
		StringBuilder stringBuilder = new StringBuilder("Install error :  Could not install selected type of server.\n\nParameters :\n ");
		foreach (KeyValuePair<string, string> parameter in parameters)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "({0},{1})\n", parameter.Key, parameter.Value);
		}
		logManager.Write(stringBuilder.ToString());
		throw new UIFrameworkErrorException(Resources.ErrorMessage_MustInstallBeforeChangingTracks);
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (GlobalData.HasParameter("CONNECTEDTOSERVER") && !GlobalData.GetValueAsBoolean("CONNECTEDTOSERVER"))
		{
			return true;
		}
		return base.BeforeLeavePage(parameters);
	}
}
