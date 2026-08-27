using System;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Components;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services.Business.Workflows;

public class BatchJob
{
	private const int MaxExceptionMsgLengthToLog = 270;

	public static string SessionCleanupJobName => Application.GetServerName() + "_SessionCleanup";

	public static string DeleteNonSessionMessagesJobName => Application.GetServerName() + "_MessageCleanup";

	public static string SessionMsgIndicatorUpdateJobName => Application.GetServerName() + "_SessionMsgIndicatorUpdate";

	public bool SessionCleanup()
	{
		string text = null;
		string text2 = null;
		if (!VideoOS.SCH.Services.Business.Components.BatchJob.JobStatusExist(SessionCleanupJobName))
		{
			VideoOS.SCH.Services.Business.Components.BatchJob.CreateJobStatus(new BatchJobStatus(SessionCleanupJobName, 1, 0, new DateTime(1900, 1, 1, 0, 0, 0), null));
		}
		if (!VideoOS.SCH.Services.Business.Components.BatchJob.UpdateJobStatus(SessionCleanupJobName, 1, 2))
		{
			VideoOS.SCH.Services.Business.Components.BatchJob.IncreaseNoofFailedStarts(SessionCleanupJobName);
		}
		try
		{
			bool flag = new VideoOS.SCH.Services.Business.Components.Session().DeleteExpiredSessions();
			text = ((!flag) ? "Delete expired sessions failed." : "Delete expired sessions OK.");
			int count = ServiceContext.Current.Errors.Count;
			text = text + " Number of errors: " + count + ".";
			bool flag2 = new VideoOS.SCH.Services.Business.Components.MessageWatch().DeleteMessageWatchOrphans();
			text = ((!flag2) ? (text + " Delete msg watch orphans failed.") : (text + " Delete msg watch orphans OK."));
			text = text + " Number of errors: " + (ServiceContext.Current.Errors.Count - count) + ".";
			foreach (ErrorItem error in ServiceContext.Current.Errors)
			{
				text2 = ((text2 != null) ? (text2 + ", " + error.ErrorNumber) : (text2 + error.ErrorNumber));
			}
			if (text2 != null)
			{
				text = text + " {" + text2 + "}";
				ServiceContext.Current.LogToServerLog(0, text, LogType.Error);
			}
			if (flag && flag2)
			{
				VideoOS.SCH.Services.Business.Components.BatchJob.SetLastSuccessInfo(SessionCleanupJobName, DateTime.UtcNow, text);
			}
			else
			{
				VideoOS.SCH.Services.Business.Components.BatchJob.SetLastJobMessage(SessionCleanupJobName, text, createJobIfNotExist: false);
			}
			return flag && flag2;
		}
		catch (Exception ex)
		{
			string text3 = "<null message>";
			if (ex.Message != null)
			{
				text3 = ((ex.Message.Length <= 270) ? ex.Message : (ex.Message.Substring(0, 270) + "..."));
			}
			VideoOS.SCH.Services.Business.Components.BatchJob.SetLastJobMessage(SessionCleanupJobName, "Exception occurred: " + text3, createJobIfNotExist: false);
			throw;
		}
		finally
		{
			VideoOS.SCH.Services.Business.Components.BatchJob.UpdateJobStatus(SessionCleanupJobName, 1);
		}
	}

	public bool MessageCleanup()
	{
		string text = null;
		string text2 = null;
		if (!VideoOS.SCH.Services.Business.Components.BatchJob.JobStatusExist(DeleteNonSessionMessagesJobName))
		{
			VideoOS.SCH.Services.Business.Components.BatchJob.CreateJobStatus(new BatchJobStatus(DeleteNonSessionMessagesJobName, 1, 0, new DateTime(1900, 1, 1, 0, 0, 0), null));
		}
		if (!VideoOS.SCH.Services.Business.Components.BatchJob.UpdateJobStatus(DeleteNonSessionMessagesJobName, 1, 2))
		{
			VideoOS.SCH.Services.Business.Components.BatchJob.IncreaseNoofFailedStarts(DeleteNonSessionMessagesJobName);
		}
		try
		{
			bool flag = new VideoOS.SCH.Services.Business.Components.Message().DeleteNonSessionMessages();
			text = ((!flag) ? "Delete non-session messages failed." : "Delete non-session messages OK.");
			text = text + " Number of errors: " + ServiceContext.Current.Errors.Count + ".";
			foreach (ErrorItem error in ServiceContext.Current.Errors)
			{
				text2 = ((text2 != null) ? (text2 + ", " + error.ToString()) : (text2 + error.ToString()));
			}
			if (text2 != null)
			{
				text = text + " {" + text2 + "}";
				ServiceContext.Current.LogToServerLog(0, text, LogType.Error);
			}
			if (flag)
			{
				VideoOS.SCH.Services.Business.Components.BatchJob.SetLastSuccessInfo(DeleteNonSessionMessagesJobName, DateTime.UtcNow, text);
			}
			else
			{
				VideoOS.SCH.Services.Business.Components.BatchJob.SetLastJobMessage(DeleteNonSessionMessagesJobName, text, createJobIfNotExist: false);
			}
			return flag;
		}
		catch (Exception ex)
		{
			string text3 = "<null message>";
			if (ex.Message != null)
			{
				text3 = ((ex.Message.Length <= 270) ? ex.Message : (ex.Message.Substring(0, 270) + "..."));
			}
			VideoOS.SCH.Services.Business.Components.BatchJob.SetLastJobMessage(DeleteNonSessionMessagesJobName, "Exception occurred: " + text3, createJobIfNotExist: false);
			throw;
		}
		finally
		{
			bool flag = VideoOS.SCH.Services.Business.Components.BatchJob.UpdateJobStatus(DeleteNonSessionMessagesJobName, 1);
		}
	}

	public bool SessionMsgIndicatorUpdate()
	{
		string text = null;
		string text2 = null;
		try
		{
			bool flag = SessionMsgIndicatorUpdater.UpdateSessionMsgIndicatorData();
			if (!flag)
			{
				text = "Update of session message indicator failed. Number of errors: " + ServiceContext.Current.Errors.Count + ".";
				foreach (ErrorItem error in ServiceContext.Current.Errors)
				{
					text2 = ((text2 != null) ? (text2 + ", " + error.ToString()) : (text2 + error.ToString()));
				}
				if (text2 != null)
				{
					text = text + " {" + text2 + "}";
				}
				ServiceContext.Current.LogToServerLog(0, text, LogType.Error);
				VideoOS.SCH.Services.Business.Components.BatchJob.SetLastJobMessage(SessionMsgIndicatorUpdateJobName, text, createJobIfNotExist: true);
			}
			return flag;
		}
		catch (Exception ex)
		{
			string text3 = "<null message>";
			if (ex.Message != null)
			{
				text3 = ((ex.Message.Length <= 270) ? ex.Message : (ex.Message.Substring(0, 270) + "..."));
			}
			VideoOS.SCH.Services.Business.Components.BatchJob.SetLastJobMessage(SessionMsgIndicatorUpdateJobName, "Exception occurred: " + text3, createJobIfNotExist: true);
			throw;
		}
	}
}
