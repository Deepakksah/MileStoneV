using System;
using VideoOS.EventServer.Communication.AlarmSecurity;
using VideoOS.Management.Common;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Platform;

namespace VideoOS.Mobile.Management;

public class AlarmSecurityManager
{
	public static readonly AlarmSecurityManager Instance = new AlarmSecurityManager();

	private AlarmSecurityClient _alarmSecurityClient;

	public Identity[] GetUsersPermittedByObject(string objectId)
	{
		return _alarmSecurityClient.GetUsersPermittedByObject(objectId);
	}

	public void Initialize(FQID fqid)
	{
		try
		{
			if ((object)fqid != null && (object)fqid.ServerId != null)
			{
				_alarmSecurityClient = new AlarmSecurityClientManager().GetAlarmSecurityClient(fqid.ServerId);
			}
		}
		catch (Exception ex)
		{
			Log.Error("AlarmSecurityManager.InitializeFromServerFqid", ex.Message);
		}
	}
}
