using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using VideoOS.Common.Integration;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.RecorderPlugins;

public class RecorderMultiInstanceValidationPageAction : PageSetupActionsBase
{
	public override string PageName => "RecorderMultiInstanceValidationPage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		ValidateMultiInstanceRecorders();
		return false;
	}

	private void ValidateMultiInstanceRecorders()
	{
		LogManager.Write("Validating multi instance recorders");
		if (!IsUpgrade())
		{
			return;
		}
		if (!IsStandAlone())
		{
			if (IsServiceControllerHasAnyMultiInstanceRecorders())
			{
				throw CreateMultiInstancesRecorderNotSupportedException();
			}
		}
		else if (IsXpcoHasAnyMultiInstanceRecorders())
		{
			throw CreateMultiInstancesRecorderNotSupportedException();
		}
		static DetailedErrorException CreateMultiInstancesRecorderNotSupportedException()
		{
			return new DetailedErrorException(() => Resources.MultiInstancesRecorderNotSupportedHeader, () => new ContentLinkText(Resources.MultiInstancesRecorderNotSupportedMessage, (Resources.MultiInstancesRecorderNotSupportedMessageLinkText, new Uri("https://supportcommunity.milestonesys.com/KBRedir?art=000065620&lang=en_US"))), enablePreviousButton: false);
		}
	}

	private bool IsXpcoHasAnyMultiInstanceRecorders()
	{
		LogManager.Write("Verifying if system has any multi instance recording servers.");
		bool flag = false;
		Dictionary<Guid, string> recordingServersInfoFromSql = GetRecordingServersInfoFromSql();
		flag = (from hostName in recordingServersInfoFromSql.Values
			group hostName by hostName).Any((IGrouping<string, string> group) => group.Count() > 1);
		if (!flag)
		{
			flag = AreRecorderIdsInSequence(recordingServersInfoFromSql.Select((KeyValuePair<Guid, string> rs) => rs.Key).ToList());
		}
		return flag;
	}

	private static bool AreRecorderIdsInSequence(List<Guid> objects)
	{
		var list = (from o in objects
			select new
			{
				OriginalGuid = o,
				SequenceNumber = Convert.ToInt64(o.ToString().Split('-').Last(), 16)
			} into x
			orderby x.SequenceNumber
			select x).ToList();
		List<Guid> list2 = new List<Guid>();
		for (int num = 1; num < list.Count; num++)
		{
			if (list[num].SequenceNumber == list[num - 1].SequenceNumber + 1)
			{
				if (!list2.Contains(list[num - 1].OriginalGuid))
				{
					list2.Add(list[num - 1].OriginalGuid);
				}
				list2.Add(list[num].OriginalGuid);
			}
		}
		return list2.Any();
	}

	private static bool IsServiceControllerHasAnyMultiInstanceRecorders()
	{
		int result = 0;
		try
		{
			ServiceController[] services = ServiceController.GetServices();
			for (int i = 0; i < services.Length; i++)
			{
				if (services[i].DisplayName.StartsWith(OemResources.Instance.Recorder.ServiceName, StringComparison.Ordinal))
				{
					result++;
				}
			}
		}
		catch (Win32Exception)
		{
		}
		if (result == 0 && !int.TryParse(ConfigurationManager.AppSettings.Get("traycontrollerinstances"), out result))
		{
			result = 0;
		}
		LogManager.Write($"Instances count: {result}", TraceEventType.Error);
		return result > 1;
	}

	private static bool IsStandAlone()
	{
		if (InstallerManager.Instance.Parameters.ContainsKey("ISCORPORATEINSTALLER"))
		{
			return InstallerManager.Instance.Parameters["ISCORPORATEINSTALLER"] == "true";
		}
		return false;
	}

	private static bool IsUpgrade()
	{
		if (InstallerManager.Instance.Parameters.ContainsKey("IS_UPGRADE") && InstallerManager.Instance.Parameters["IS_UPGRADE"] == "true" && InstallerManager.Instance.Parameters.ContainsKey("FINALIZE-MODE"))
		{
			return InstallerManager.Instance.Parameters["FINALIZE-MODE"] == "true";
		}
		return false;
	}

	private Dictionary<Guid, string> GetRecordingServersInfoFromSql()
	{
		Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
		string commandText = "\r\n        IF OBJECT_ID('RecordingServerService', 'U') IS NOT NULL\r\n        BEGIN\r\n            SELECT pkid, host_name FROM RecordingServerService\r\n        END\r\n        ELSE\r\n        BEGIN\r\n            SELECT CAST(NULL AS UNIQUEIDENTIFIER) AS pkid, NULL AS host_name\r\n        END";
		try
		{
			using DatabaseConnection databaseConnection = new DatabaseConnection(InstallerManager.Instance.Parameters["SQL_CONNECTION_STRING_MS"], LogManager.InnerLog);
			databaseConnection.Open();
			using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(commandText);
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			if (databaseDataReader.HasRows)
			{
				while (databaseDataReader.Read())
				{
					while (databaseDataReader.Read())
					{
						if (!databaseDataReader.IsDBNull(0) && !databaseDataReader.IsDBNull(1))
						{
							dictionary.Add(new Guid(databaseDataReader.GetString(0)), databaseDataReader.GetString(1));
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new RecorderInstallerException(string.Format(Resources.MultiInstanceRecorderVerificationErrorMessage, ex.Message));
		}
		return dictionary;
	}
}
