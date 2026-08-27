using System;
using VideoOS.Platform.Util;
using VideoOS.Toolkit;

namespace VideoOS.Platform.SDK.Export;

public static class MediaDataInspector
{
	public class DataInformation
	{
		public bool ContainsData { get; set; }

		public bool RecorderOnline { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }
	}

	public static DataInformation TrimStartAndEndTime(FQID deviceFQID, DateTime exportStart, DateTime exportEnd)
	{
		DataInformation dataInformation = new DataInformation();
		dataInformation.ContainsData = false;
		dataInformation.RecorderOnline = true;
		dataInformation.StartTime = exportStart.ToUniversalTime();
		dataInformation.EndTime = exportEnd.ToUniversalTime();
		IData data = null;
		IData data2 = null;
		bool flag = false;
		IPlaybackSourceToolkit playbackSourceToolkit = null;
		InternalCommandService instance = InternalCommandService.Instance;
		try
		{
			playbackSourceToolkit = instance.ImageExporterConnect(deviceFQID) as IPlaybackSourceToolkit;
			if (playbackSourceToolkit != null && playbackSourceToolkit.MoveTo(exportStart, IPlaybackSourceToolkit.SearchCriteria.AT_OR_AFTER) == IPlaybackSourceToolkit.MoveStatus.MOVED_OK && playbackSourceToolkit.GetData(out data) == IPlaybackSourceToolkit.GetDataStatus.DATA_RETRIEVED && playbackSourceToolkit.MoveTo(exportEnd, IPlaybackSourceToolkit.SearchCriteria.AT_OR_BEFORE) == IPlaybackSourceToolkit.MoveStatus.MOVED_OK && playbackSourceToolkit.GetData(out data2) == IPlaybackSourceToolkit.GetDataStatus.DATA_RETRIEVED)
			{
				flag = true;
			}
			if ((data == null || (data.BeginTimeStamp.Year != 1970 && data.EndTimeStamp.Year != 1970)) && data2 != null && data2.BeginTimeStamp.Year != 1970)
			{
				_ = data2.EndTimeStamp.Year;
				_ = 1970;
			}
			if (flag && data != null && data2 != null && data.BeginTimeStamp < data2.EndTimeStamp)
			{
				dataInformation.ContainsData = true;
				if (data.EndTimeStamp > exportStart.ToUniversalTime())
				{
					dataInformation.StartTime = data.BeginTimeStamp;
				}
				if (data2.BeginTimeStamp < exportEnd.ToUniversalTime())
				{
					dataInformation.EndTime = data2.EndTimeStamp;
				}
			}
			playbackSourceToolkit.Disconnect();
			return dataInformation;
		}
		catch (Exception ex)
		{
			EnvironmentManager instance2 = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance2.Log(error: true, "VideoOS.Platform.SDK.Export.MediaDataInspector.TrimStartAndEndTime", message, args);
			dataInformation.RecorderOnline = false;
			return dataInformation;
		}
		finally
		{
			if (data != null)
			{
				data.Dispose();
				data = null;
			}
			if (data2 != null)
			{
				data2.Dispose();
				data2 = null;
			}
			if (playbackSourceToolkit != null)
			{
				instance?.ImageExporterDisconnect(playbackSourceToolkit);
			}
		}
	}
}
