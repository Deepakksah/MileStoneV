using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Util;

public static class AlarmBuilder
{
	public static Alarm CreateAlarmFromEvent(BaseEvent baseEvent)
	{
		Alarm alarm = null;
		if (baseEvent != null)
		{
			if (baseEvent is EventData)
			{
				if (baseEvent is EventData eventData)
				{
					alarm = new Alarm();
					alarm.EventHeader = eventData.EventHeader;
					if (eventData.EventSequence != null)
					{
						alarm.StartTime = eventData.EventSequence.StartDateTime;
						alarm.EndTime = eventData.EventSequence.EndDateTime;
						if (eventData.EventSequence.SnapshotImages != null)
						{
							alarm.SnapshotList = new SnapshotList();
							Image[] snapshotImages = eventData.EventSequence.SnapshotImages;
							foreach (Image image in snapshotImages)
							{
								Snapshot snapshot = new Snapshot();
								snapshot.Height = (uint)image.Height;
								snapshot.Width = (uint)image.Width;
								snapshot.Image = ImageToBytes(image);
								snapshot.SizeInBytes = (uint)snapshot.Image.Length;
								alarm.SnapshotList.Add(snapshot);
							}
						}
						if (eventData.EventSequence.SnapshotJPEGs != null)
						{
							if (alarm.SnapshotList == null)
							{
								alarm.SnapshotList = new SnapshotList();
							}
							byte[][] snapshotJPEGs = eventData.EventSequence.SnapshotJPEGs;
							foreach (byte[] array in snapshotJPEGs)
							{
								Snapshot snapshot2 = new Snapshot();
								snapshot2.Image = array;
								snapshot2.SizeInBytes = (uint)array.Length;
								alarm.SnapshotList.Add(snapshot2);
							}
						}
					}
					alarm.State = 1;
				}
			}
			else if (baseEvent is AnalyticsEvent)
			{
				if (baseEvent is AnalyticsEvent analyticsEvent)
				{
					alarm = new Alarm
					{
						EventHeader = analyticsEvent.EventHeader,
						Count = analyticsEvent.Count,
						Description = analyticsEvent.Description,
						EndTime = analyticsEvent.EndTime,
						Location = analyticsEvent.Location,
						ObjectList = analyticsEvent.ObjectList,
						ReferenceList = analyticsEvent.ReferenceList,
						RuleList = analyticsEvent.RuleList,
						SnapshotList = analyticsEvent.SnapshotList,
						StartTime = analyticsEvent.StartTime,
						Vendor = analyticsEvent.Vendor
					};
				}
			}
			else
			{
				alarm = new Alarm();
				alarm.EventHeader = baseEvent.EventHeader;
			}
		}
		return alarm;
	}

	private static byte[] ImageToBytes(Image image)
	{
		if (image == null)
		{
			return null;
		}
		MemoryStream memoryStream = new MemoryStream();
		image.Save(memoryStream, ImageFormat.Jpeg);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		byte[] result = memoryStream.ToArray();
		memoryStream.Close();
		return result;
	}
}
