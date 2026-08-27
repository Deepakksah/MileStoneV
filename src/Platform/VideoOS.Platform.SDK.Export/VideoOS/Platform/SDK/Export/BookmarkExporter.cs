using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using VideoOS.Platform.Data;
using VideoOS.Platform.SDK.Export.Resources;

namespace VideoOS.Platform.SDK.Export;

public class BookmarkExporter
{
	public enum ExceptionCode
	{
		ServerCommunicationFailed,
		FileWriteFailed
	}

	[Serializable]
	public class FailedException : Exception
	{
		public ExceptionCode ErrorCode { get; public set; }

		public FailedException(ExceptionCode errorCode)
		{
			ErrorCode = errorCode;
		}

		public FailedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	private readonly Guid[] _cameraDeviceType = new Guid[3]
	{
		Kind.Camera,
		Kind.Microphone,
		Kind.Speaker
	};

	public const string BookmarksDataFile = "Bookmarks.xml";

	private const int _maxChunkSize = 100;

	private IEnumerable<FQID> _cameraIds;

	private readonly DateTime _beginTime;

	private readonly DateTime _endTime;

	private readonly string _destinationDirectory;

	public BookmarkExporter(IEnumerable<FQID> cameraIds, DateTime beginTimeUtc, DateTime endTimeUtc, string destinationDirectory)
	{
		_cameraIds = cameraIds;
		_beginTime = beginTimeUtc;
		_endTime = endTimeUtc;
		_destinationDirectory = destinationDirectory;
	}

	private static string GetBookmarkFilePath(string basePath, Guid deviceId)
	{
		return Path.Combine(Path.Combine(basePath, deviceId.ToString()), "Bookmarks.xml");
	}

	public void DoExport(DBExporterXpco.BookmarkJob job)
	{
		job.ExportInProgress = true;
		job.Progress = 0;
		try
		{
			if (job.IsCancelled)
			{
				job.ExportInProgress = false;
				return;
			}
			Dictionary<ServerId, List<FQID>> dictionary = new Dictionary<ServerId, List<FQID>>();
			foreach (FQID cameraId in _cameraIds)
			{
				if (!dictionary.ContainsKey(cameraId.ServerId))
				{
					dictionary.Add(cameraId.ServerId, new List<FQID>());
				}
				dictionary[cameraId.ServerId].Add(cameraId);
			}
			int num = ((dictionary.Count != 0) ? (100 / dictionary.Count) : 0);
			foreach (KeyValuePair<ServerId, List<FQID>> item in dictionary)
			{
				if (job.IsCancelled)
				{
					job.ExportInProgress = false;
					job.Progress = 0;
					return;
				}
				IEnumerable<Bookmark> bookmarks = RetrieveBookmarks(item.Key, item.Value, job);
				foreach (FQID item2 in item.Value)
				{
					if (job.IsCancelled)
					{
						job.ExportInProgress = false;
						job.Progress = 0;
						return;
					}
					try
					{
						WriteDeviceBookmarkFile(bookmarks, item2);
					}
					catch (Exception)
					{
						throw new FailedException(ExceptionCode.FileWriteFailed);
					}
				}
				job.Progress += num;
			}
			job.Progress = 100;
		}
		catch (FailedException ex2)
		{
			switch (ex2.ErrorCode)
			{
			case ExceptionCode.ServerCommunicationFailed:
				job.LastErrorMessage = Strings.BookmarkExporterError_ServerCommunicationErrorDuringBookmarkExport;
				break;
			case ExceptionCode.FileWriteFailed:
				job.LastErrorMessage = Strings.BookmarkExporterError_WriteErrorDuringBookmarkExport;
				break;
			default:
				job.LastErrorMessage = Strings.BookmarkExporterError_UnknownErrorDuringBookmarkExport;
				break;
			}
			job.Progress = -1;
		}
		catch (Exception)
		{
			job.Progress = -1;
			job.LastErrorMessage = Strings.BookmarkExporterError_UnknownErrorDuringBookmarkExport;
		}
		job.ExportInProgress = false;
	}

	private IEnumerable<Bookmark> RetrieveBookmarks(ServerId serverId, IEnumerable<FQID> cameraIds, DBExporterXpco.BookmarkJob job)
	{
		List<Bookmark> list = new List<Bookmark>();
		try
		{
			BookmarkService instance = BookmarkService.Instance;
			TimeSpan timeSpan = _endTime - _beginTime;
			Bookmark[] array = instance.BookmarkSearchTime(serverId, _beginTime, (long)timeSpan.TotalMilliseconds * 1000, 100, _cameraDeviceType, cameraIds.ToArray(), null, null);
			if (array.Length == 0)
			{
				return list;
			}
			list.AddRange(array.Where((Bookmark b) => b.TimeBegin < _endTime));
			Bookmark bookmark = array.Last();
			while (array.Length == 100 && bookmark.TimeBegin < _endTime)
			{
				if (job.IsCancelled)
				{
					job.ExportInProgress = false;
					job.Progress = 0;
					return null;
				}
				timeSpan = _endTime - bookmark.TimeEnd;
				array = instance.BookmarkSearchFromBookmark(bookmark.BookmarkFQID, (long)timeSpan.TotalMilliseconds * 1000, 100, _cameraDeviceType, cameraIds.ToArray(), null, null);
				if (array.Length != 0)
				{
					list.AddRange(array);
					bookmark = array.Last();
					continue;
				}
				break;
			}
		}
		catch (Exception)
		{
			throw new FailedException(ExceptionCode.ServerCommunicationFailed);
		}
		return list;
	}

	private void WriteDeviceBookmarkFile(IEnumerable<Bookmark> bookmarks, FQID deviceId)
	{
		string bookmarkFilePath = GetBookmarkFilePath(_destinationDirectory, deviceId.ObjectId);
		Directory.CreateDirectory(Path.GetDirectoryName(bookmarkFilePath));
		XmlWriterSettings settings = new XmlWriterSettings
		{
			Indent = true
		};
		XmlWriter xmlWriter = XmlWriter.Create(bookmarkFilePath, settings);
		try
		{
			xmlWriter.WriteStartDocument(standalone: true);
			xmlWriter.WriteStartElement("BookmarkExport");
			xmlWriter.WriteAttributeString("version", "1.0");
			xmlWriter.WriteAttributeString("deviceid", deviceId.ToString());
			xmlWriter.WriteAttributeString("begintime", XmlConvert.ToString(_beginTime, XmlDateTimeSerializationMode.Utc));
			xmlWriter.WriteAttributeString("endtime", XmlConvert.ToString(_endTime, XmlDateTimeSerializationMode.Utc));
			WriteBookmarks(xmlWriter, bookmarks, deviceId);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
		}
		finally
		{
			if (xmlWriter != null)
			{
				xmlWriter.Flush();
				xmlWriter.Close();
			}
		}
	}

	private static void WriteBookmarks(XmlWriter writer, IEnumerable<Bookmark> bookmarks, FQID deviceId)
	{
		writer.WriteStartElement("Bookmarks");
		foreach (Bookmark bookmark in bookmarks)
		{
			if (bookmark.GetDeviceItem().FQID.ObjectId == deviceId.ObjectId)
			{
				writer.WriteStartElement("Bookmark");
				writer.WriteAttributeString("id", XmlConvert.ToString(bookmark.BookmarkFQID.ObjectId));
				writer.WriteAttributeString("deviceid", XmlConvert.ToString(deviceId.ObjectId));
				writer.WriteAttributeString("timebegin", XmlConvert.ToString(bookmark.TimeBegin, XmlDateTimeSerializationMode.Utc));
				writer.WriteAttributeString("timetrigged", XmlConvert.ToString(bookmark.TimeTrigged, XmlDateTimeSerializationMode.Utc));
				writer.WriteAttributeString("timeend", XmlConvert.ToString(bookmark.TimeEnd, XmlDateTimeSerializationMode.Utc));
				writer.WriteElementString("Reference", bookmark.Reference ?? "");
				writer.WriteElementString("Header", bookmark.Header ?? "");
				writer.WriteElementString("Description", bookmark.Description ?? "");
				writer.WriteElementString("User", bookmark.User ?? "");
				writer.WriteEndElement();
			}
		}
		writer.WriteEndElement();
	}
}
