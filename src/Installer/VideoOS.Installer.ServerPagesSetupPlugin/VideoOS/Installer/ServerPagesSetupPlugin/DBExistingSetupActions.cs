using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.DatabasePlugin;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.ServerPagesSetupPlugin.Properties;
using VideoOS.Installer.UI.Framework;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Network;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class DBExistingSetupActions : DbExistingPageActions
{
	public enum DatabaseType
	{
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "XPCO", Justification = "Must be like this")]
		XPCO = 100,
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "XPET", Justification = "Must be like this")]
		XPET = 50,
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "XPP", Justification = "Must be like this")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "XPP", Justification = "This is how the SKUs are named")]
		XPPPlus = 45,
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "XPLA", Justification = "Must be like this")]
		XPLA = 40,
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "XPLL", Justification = "Must be like this")]
		XPLL = 30,
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "XPEX", Justification = "Must be like this")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "XPEX", Justification = "This is how the SKUs are named")]
		XPEXPlus = 10,
		[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "XPES", Justification = "Must be like this")]
		[SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "XPES", Justification = "This is how the SKUs are named")]
		XPESPlus = 5
	}

	private const string LocationName = "DbExistingSetupActions.BeforeActivatePage()";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		bool flag = true;
		IDatabaseConnectionInfo databaseConnectionInfo = null;
		ISqlConnectionManager sqlConnectionManager = null;
		if (!GlobalData.TryGetValue("INSTALLATION_MODE", out var value))
		{
			return true;
		}
		if (value == "Typical")
		{
			SetDefaultParameters();
		}
		ValidateParameters(parameters);
		if (base.BeforeActivatePage(parameters))
		{
			string connectionString = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.ManagementServer, new RegistrySqlConnectionStorage());
			if (!string.IsNullOrEmpty(connectionString))
			{
				databaseConnectionInfo = new DatabaseConnectionInfo(connectionString);
				sqlConnectionManager = new SqlConnectionManager(databaseConnectionInfo, null);
				using (new ShowWaitCursor())
				{
					try
					{
						if (DatabaseErrorCheckHelper.CheckSelectedDB(sqlConnectionManager) != DataBaseErrorString.Empty)
						{
							LogManager.Write("Connection using string from the registry fails. " + connectionString);
							sqlConnectionManager = null;
						}
					}
					catch (UIFrameworkErrorException)
					{
						sqlConnectionManager = null;
					}
				}
			}
		}
		else
		{
			SetDefaultParameters();
			flag = false;
		}
		if (value == "Typical")
		{
			flag = false;
		}
		if (sqlConnectionManager != null)
		{
			if (value == "Typical")
			{
				bool flag2 = IsAddressThisMachine(sqlConnectionManager.ServerName);
				if (flag2 || MessageHelper.ShowInstallerMessageBox(Resources.Database_ConnectionStringFoundCaption, string.Format(CultureInfo.CurrentCulture, Resources.Database_ConnectionStringFoundText, sqlConnectionManager.ServerName, sqlConnectionManager.DatabaseName), InstallerMessageBoxIcon.Question, InstallerMessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					if (!flag2)
					{
						DatabaseComplianceCheck.DoDatabaseComplianceCheck(sqlConnectionManager);
					}
					LogManager.Write("Connection string from previous installation was found and is going to be reused.");
					LogManager.Write(FormattableString.Invariant($"Database connection information server='{databaseConnectionInfo.ServerName}' database={databaseConnectionInfo.DatabaseName}"));
					SaveParameters(sqlConnectionManager, databaseConnectionInfo, considerSqlInstance: false);
				}
				else
				{
					LogManager.Write("Connection string from previous installation was found but denied by user.");
				}
			}
			else
			{
				SaveParameters(sqlConnectionManager, databaseConnectionInfo, considerSqlInstance: true);
			}
		}
		Dictionary<string, string> dictionary = GlobalData.ToDict();
		if (!flag && dictionary["SQL-KEEP-DATA"] == "yes" && dictionary["SQL-CREATE-DATABASE"] == "no")
		{
			HandleDuplicateDevices(parameters.GetValueAsNullableBoolean("QUIET-MODE") == true);
		}
		return flag;
	}

	private static void ValidateParameters(Dictionary<string, string> parameters)
	{
		if (!GlobalData.TryGetValue("SQL_CONNECTION_STRING_MS", out var _))
		{
			if (!parameters.ContainsKey("SQL_CONNECTION_STRING_MS"))
			{
				throw new ArgumentException("Parameters does not contain Params.SQL_CONNECTION_STRING_MS", "parameters");
			}
			GlobalData.SetValue("SQL_CONNECTION_STRING_MS", parameters["SQL_CONNECTION_STRING_MS"], "DbExistingSetupActions.BeforeActivatePage()");
		}
	}

	private static void SetDefaultParameters()
	{
		GlobalData.SetValue("SQL-CREATE-DATABASE", "yes", "DbExistingSetupActions.BeforeActivatePage()");
		GlobalData.SetValue("SQL-KEEP-DATA", "no", "DbExistingSetupActions.BeforeActivatePage()");
	}

	private static void SaveParameters(ISqlConnectionManager sqlConnectionManager, IDatabaseConnectionInfo connectionInfo, bool considerSqlInstance)
	{
		if (!considerSqlInstance || !GlobalData.TryGetValue("SQL_CONNECTION_STRING_MS", out var value) || string.Equals(new DatabaseConnectionInfo(value).ServerName, sqlConnectionManager.ServerName, StringComparison.OrdinalIgnoreCase))
		{
			GlobalData.SetValue("SQL_CONNECTION_STRING_MS", connectionInfo.ConnectionString, "DbExistingSetupActions.BeforeActivatePage()");
			string connectionString = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.IDP, new IdpRegistrySqlConnectionStorage());
			if (!string.IsNullOrEmpty(connectionString))
			{
				GlobalData.SetValue("SQL_CONNECTION_STRING_IDP", connectionString, "DbExistingSetupActions.BeforeActivatePage()");
			}
			string connectionString2 = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.IncidentManager, null);
			if (!string.IsNullOrEmpty(connectionString2))
			{
				GlobalData.SetValue("SQL_CONNECTION_STRING_IM", connectionString2, "DbExistingSetupActions.BeforeActivatePage()");
			}
			else
			{
				IDatabaseConnectionInfo databaseConnectionInfo = connectionInfo.CloneWithNewDatabaseName(connectionInfo.DatabaseName + "_IM");
				GlobalData.SetValue("SQL_CONNECTION_STRING_IM", databaseConnectionInfo.ConnectionString, "DbExistingSetupActions.BeforeActivatePage()");
			}
			string connectionString3 = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.EventServer, new EventServerSqlConnectionStorage());
			if (!string.IsNullOrEmpty(connectionString3))
			{
				GlobalData.SetValue("SQL_CONNECTION_STRING_ES", connectionString3, "DbExistingSetupActions.BeforeActivatePage()");
			}
			else
			{
				GlobalData.SetValue("SQL_CONNECTION_STRING_ES", connectionInfo.ConnectionString, "DbExistingSetupActions.BeforeActivatePage()");
			}
			string connectionString4 = InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.LogServer, null);
			if (!string.IsNullOrEmpty(connectionString4))
			{
				GlobalData.SetValue("SQL_CONNECTION_STRING_LOG", connectionString4, "DbExistingSetupActions.BeforeActivatePage()");
			}
			GlobalData.SetValue("SQL-CREATE-DATABASE", "no", "DbExistingSetupActions.BeforeActivatePage()");
			GlobalData.SetValue("SQL-KEEP-DATA", "yes", "DbExistingSetupActions.BeforeActivatePage()");
		}
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		if (!base.BeforeLeavePage(parameters))
		{
			return false;
		}
		Dictionary<string, string> dictionary = GlobalData.ToDict();
		using (new ShowWaitCursor())
		{
			string serverName = new DatabaseConnectionInfo(dictionary["SQL_CONNECTION_STRING_MS"]).ServerName;
			if (dictionary["SQL-KEEP-DATA"] == "yes" && dictionary["SQL-CREATE-DATABASE"] == "no" && !IsAddressThisMachine(serverName) && DatabaseComplianceCheck.NeedsDatabaseComplianceCheck)
			{
				DatabaseComplianceCheck.DoDatabaseComplianceCheck(new SqlConnectionManager(new DatabaseConnectionInfo(InstallerConnectionStringHelper.GetConnectionString(SqlConnectionServerType.ManagementServer, new RegistrySqlConnectionStorage())), null));
			}
			if (dictionary["SQL-KEEP-DATA"] == "yes" && dictionary["SQL-CREATE-DATABASE"] == "no")
			{
				HandleDuplicateDevices(parameters.GetValueAsNullableBoolean("QUIET-MODE") == true);
			}
			string databaseName = new DatabaseConnectionInfo(dictionary["SQL_CONNECTION_STRING_MS"]).DatabaseName;
			GlobalData.SetValue("LOG-SQL-DATABASE-NAME", databaseName + "LogServerV2", "DbExistingSetupActions.BeforeLeavePage()");
		}
		return true;
	}

	private static void HandleDuplicateDevices(bool isQuietInstall)
	{
		(bool, bool, bool) tuple = QueryDuplicateDevicesInfo();
		if (!tuple.Item1 || (!tuple.Item3 && !tuple.Item2))
		{
			return;
		}
		LogManager.Write("Duplicate devices have been found in the devices table", TraceEventType.Warning);
		Func<string> func = () => Resources.Database_DuplicateDevices_Found;
		string supportUrl = OemResources.Instance.Manager.SupportPortalUrl;
		Func<ContentLinkText> func2 = null;
		throw new RunActionException(innerException: new DetailedErrorException(func, (!isQuietInstall) ? ((Func<ContentLinkText>)(() => (!string.IsNullOrEmpty(supportUrl)) ? new ContentLinkText(Resources.Database_UpdateFailed_DuplicateDevicesFound, (Resources.Database_UpdateFailed_SupportLink_Here, new Uri(supportUrl))) : new ContentLinkText(Resources.Database_UpdateFailed_DuplicateDevicesFound_NoSupportUrl))) : ((Func<ContentLinkText>)(() => (!string.IsNullOrEmpty(supportUrl)) ? new ContentLinkText(string.Format(CultureInfo.InvariantCulture, Resources.Database_UpdateFailed_DuplicateDevicesFound_QuietInstall, new Uri(supportUrl).AbsoluteUri)) : new ContentLinkText(Resources.Database_UpdateFailed_DuplicateDevicesFound_QuietInstall_NoSupportUrl))), enablePreviousButton: false), message: func());
	}

	private static (bool HasAnyDuplicateDevices, bool HasImportantDuplicateDevices, bool HasDuplicateRecordingDevices) QueryDuplicateDevicesInfo()
	{
		if (!GlobalData.TryGetValue("SQL_CONNECTION_STRING_MS", out var value))
		{
			LogManager.Write("Failed to look up connection string when checking if database has duplicate devices.", TraceEventType.Error);
			return (HasAnyDuplicateDevices: false, HasImportantDuplicateDevices: false, HasDuplicateRecordingDevices: false);
		}
		try
		{
			using IConfigurationDataComponent configurationDataComponent = new ComponentFactory().CreateConfigurationComponent(value, null);
			return configurationDataComponent.QueryDuplicateDevicesInfo();
		}
		catch (ConfigurationDataException exception)
		{
			LogManager.Write("Error occurred while checking duplicate devices.");
			LogManager.Write(exception);
			return (HasAnyDuplicateDevices: false, HasImportantDuplicateDevices: false, HasDuplicateRecordingDevices: false);
		}
	}

	public static int GetDatabaseTypeFromString(string productSku)
	{
		try
		{
			return (int)(DatabaseType)Enum.Parse(typeof(DatabaseType), productSku, ignoreCase: true);
		}
		catch (ArgumentException)
		{
			return 100;
		}
		catch (OverflowException)
		{
			return 100;
		}
	}

	private static bool IsAddressThisMachine(string address)
	{
		address = address.Split(new string[2] { "\\", "," }, StringSplitOptions.RemoveEmptyEntries)[0];
		return IPHelper.DnsGetHostEntry(address).HostName.Equals(Dns.GetHostEntry("").HostName, StringComparison.OrdinalIgnoreCase);
	}
}
