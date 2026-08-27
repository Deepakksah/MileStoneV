using System;
using System.Data;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

public class RecordingStorageDeviceTableAdapterForce : RecordingStorageDeviceTableAdapter
{
	public override int Update(DataRow[] dataRows)
	{
		int num = 0;
		if (dataRows == null || dataRows.Length == 0)
		{
			return num;
		}
		for (int i = 0; i < dataRows.Length; i++)
		{
			ConfigurationDataSet.RecordingStorageDeviceRow recordingStorageDeviceRow = (ConfigurationDataSet.RecordingStorageDeviceRow)dataRows[i];
			switch (recordingStorageDeviceRow.RowState)
			{
			case DataRowState.Added:
				num += Update(recordingStorageDeviceRow);
				break;
			case DataRowState.Deleted:
			{
				string recordingStorageId = recordingStorageDeviceRow.GetRecordingStorageId(DataRowVersion.Original);
				int num2;
				if ((num2 = ForceDelete(recordingStorageId)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the DeleteCommand affected 0 of the expected 1 records.", null, new DataRow[1] { recordingStorageDeviceRow });
				}
				num += num2;
				break;
			}
			case DataRowState.Modified:
			{
				string deviceId = recordingStorageDeviceRow.GetDeviceId(DataRowVersion.Original);
				int num2;
				if ((num2 = ForceUpdate(recordingStorageDeviceRow.recordingstorage_id, recordingStorageDeviceRow.device_id, recordingStorageDeviceRow.recording_enabled, recordingStorageDeviceRow.recording_framerate, recordingStorageDeviceRow.prebuffer_enabled, recordingStorageDeviceRow.prebuffer_seconds, recordingStorageDeviceRow.prebuffer_in_memory, recordingStorageDeviceRow.edgestorage_enabled, recordingStorageDeviceRow.edgestorage_playback, recordingStorageDeviceRow.record_keyframes, recordingStorageDeviceRow.manual_timeout_enabled, recordingStorageDeviceRow.manual_timeout, recordingStorageDeviceRow.record_on_related_devices, DateTime.UtcNow, deviceId)) == 0)
				{
					throw new DBConcurrencyException("Concurrency violation: the UpdateCommand affected 0 of the expected 1 records.", null, new DataRow[1] { recordingStorageDeviceRow });
				}
				num += num2;
				break;
			}
			}
		}
		return num;
	}
}
