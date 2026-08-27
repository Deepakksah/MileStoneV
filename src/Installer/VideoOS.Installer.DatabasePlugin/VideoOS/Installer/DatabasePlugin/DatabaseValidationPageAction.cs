using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Framework;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.SqlUpdate.Core.Configuration;

namespace VideoOS.Installer.DatabasePlugin;

public abstract class DatabaseValidationPageAction : PageSetupActionsBase
{
	protected ILogger Logger { get; }

	public override string PageName => "DatabaseValidationPage";

	protected DatabaseValidationPageAction()
	{
		Logger = LogManager.InnerLog;
	}

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		string result = ValidateDatabasesAsync().ConfigureAwait(continueOnCapturedContext: false).GetAwaiter().GetResult();
		if (!string.IsNullOrEmpty(result))
		{
			throw new DetailedErrorException(result, enablePreviousButton: true);
		}
		return false;
	}

	public async Task<string> ValidateDatabasesAsync()
	{
		List<Task<DatabaseValidationResult>> databaseValidationResults = new List<Task<DatabaseValidationResult>>();
		string[] registeredComponentsPackageIds = GetRegisteredComponentsPackageIds();
		foreach (string componentPackageId in registeredComponentsPackageIds)
		{
			if (IsComponentPackageInstallRequested(componentPackageId))
			{
				databaseValidationResults.Add(Task.Run(() => ValidateDatabase(componentPackageId) ?? throw new InvalidOperationException("Component id not found.")));
			}
		}
		await Task.WhenAll(databaseValidationResults).ConfigureAwait(continueOnCapturedContext: false);
		IEnumerable<DatabaseValidationResult> enumerable = from t in databaseValidationResults
			select t.Result into r
			where !r.Status.HasFlag(DatabaseValidationStatus.Success)
			select r;
		if (enumerable.Any())
		{
			return GetErrorMessage(enumerable);
		}
		return null;
	}

	public abstract DatabaseValidationResult ValidateDatabase(string componentPackageId);

	public abstract string[] GetRegisteredComponentsPackageIds();

	protected static SqlUpdateAction ParseSqlUpdateActionFromUserParameters()
	{
		bool flag = !IsNewDatabase() && KeepData();
		if (IsExternallyManaged())
		{
			return SqlUpdateAction.CreateOrUpdateDatabaseObjectsOnly;
		}
		if (flag)
		{
			return SqlUpdateAction.CreateOrUpgrade;
		}
		return SqlUpdateAction.ReCreate;
	}

	private static string GetErrorMessage(IEnumerable<DatabaseValidationResult> failedValidations)
	{
		return string.Join(Environment.NewLine + Environment.NewLine, failedValidations.Select((DatabaseValidationResult v) => v.ErrorMessage));
	}

	private static bool IsComponentPackageInstallRequested(string componentPackageId)
	{
		PackageRefType packageRefType = InstallerManager.Instance.InstallerConfig.InstallExecuteSequence.FirstOrDefault((PackageRefType p) => p.ID.ToUpperInvariant() == componentPackageId.ToUpperInvariant());
		if (packageRefType != null)
		{
			if (packageRefType.Condition != ConditionType.Optional)
			{
				return packageRefType.Condition == ConditionType.Required;
			}
			return true;
		}
		return false;
	}

	private static bool IsNewDatabase()
	{
		if (GlobalData.TryGetValue("SQL-CREATE-DATABASE", out var value) && string.Equals("yes", value, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	private static bool KeepData()
	{
		if (GlobalData.TryGetValue("SQL-KEEP-DATA", out var value) && string.Equals("yes", value, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	private static bool IsExternallyManaged()
	{
		if (GlobalData.TryGetValue("IS_EXTERNALLY_MANAGED", out var value) && string.Equals("true", value, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}
}
