using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Log.LPRService_en_US;

[GeneratedCode("MilestoneSystems.LogService.InterfaceGenerator", "1.0.0.0")]
public class LPRService
{
	[GeneratedCode("MilestoneSystems.LogService.InterfaceGenerator", "1.0.0.0")]
	public class Service
	{
		private LogClient m_LogClient;

		public Service()
		{
			m_LogClient = new LogClient("en-US");
		}

		public Service(LogClient logClient)
		{
			m_LogClient = logClient;
		}

		public void SetServer(string server)
		{
			m_LogClient.SetServer(server);
		}

		public void SetServer(string server, string cacheID)
		{
			m_LogClient.SetServer(server, cacheID);
		}

		public void SetAuthentication(Authentication authentication)
		{
			m_LogClient.SetAuthentication(authentication);
		}

		public void SetCredentials(string username, string password)
		{
			m_LogClient.SetCredentials(username, password);
		}

		public void SetUserId(string userId)
		{
			m_LogClient.SetUserId(userId);
		}

		public void SetUserLocation(string userLocation)
		{
			m_LogClient.SetUserLocation(userLocation);
		}

		public void SetFileQueuePath(string path)
		{
			m_LogClient.SetFileQueuePath(path);
		}

		public void SetFileQueueFlushSize(int flushSize)
		{
			m_LogClient.SetFileQueueFlushSize(flushSize);
		}

		public void SetFileQueueFlushTimeout(int flushTimeoutInSeconds)
		{
			m_LogClient.SetFileQueueFlushTimeout(flushTimeoutInSeconds);
		}

		public bool UploadCultureFiles(string directory, bool recursive, bool forceupload)
		{
			return m_LogClient.UploadCultureFiles(directory, recursive, forceupload);
		}

		public bool UploadCultureFiles(string directory, bool recursive, bool forceupload, bool asynchronous)
		{
			return m_LogClient.UploadCultureFiles(directory, recursive, forceupload, asynchronous);
		}

		public bool UploadCultureFile(string file, bool forceupload)
		{
			return m_LogClient.UploadCultureFile(file, forceupload);
		}

		public bool UploadCultureFile(string file, bool forceupload, bool asynchronous)
		{
			return m_LogClient.UploadCultureFile(file, forceupload, asynchronous);
		}

		public void TrustCultureFiles()
		{
			m_LogClient.TrustCultureFiles();
		}

		public bool GetServerVersion(out int major, out int minor, out int revision)
		{
			return m_LogClient.GetServerVersion(out major, out minor, out revision);
		}

		public void ServiceStarted(string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, ueSourceType SourceType, string EventType)
		{
			Hashtable variables = new Hashtable();
			List<LogMessageUserField> list = new List<LogMessageUserField>();
			LogMessageUserField item = default(LogMessageUserField);
			item.FieldName = "SourceType";
			item.Type = "enum";
			item.Value = Enum.GetName(typeof(ueSourceType), SourceType);
			list.Add(item);
			LogMessageUserField item2 = default(LogMessageUserField);
			item2.FieldName = "EventType";
			item2.Type = "string";
			item2.Value = EventType;
			list.Add(item2);
			m_LogClient.LogWrite(token, "Service", objId, objName, objLocationId, objLocationName, timestamp, "4.0.14", "LPRService", "Service", "", "System", "", Status.Success, Severity.Info, "ServiceStarted", "The LPR Server service has started.", "en-US", variables, list);
		}

		public void ServiceStopped(string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, ueSourceType SourceType, string EventType)
		{
			Hashtable variables = new Hashtable();
			List<LogMessageUserField> list = new List<LogMessageUserField>();
			LogMessageUserField item = default(LogMessageUserField);
			item.FieldName = "SourceType";
			item.Type = "enum";
			item.Value = Enum.GetName(typeof(ueSourceType), SourceType);
			list.Add(item);
			LogMessageUserField item2 = default(LogMessageUserField);
			item2.FieldName = "EventType";
			item2.Type = "string";
			item2.Value = EventType;
			list.Add(item2);
			m_LogClient.LogWrite(token, "Service", objId, objName, objLocationId, objLocationName, timestamp, "4.0.14", "LPRService", "Service", "", "System", "", Status.Success, Severity.Info, "ServiceStopped", "The LPR Server service has stopped.", "en-US", variables, list);
		}

		public void VideoSourceStateChangedToOK(string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, ueSourceType SourceType, string EventType)
		{
			Hashtable variables = new Hashtable();
			List<LogMessageUserField> list = new List<LogMessageUserField>();
			LogMessageUserField item = default(LogMessageUserField);
			item.FieldName = "SourceType";
			item.Type = "enum";
			item.Value = Enum.GetName(typeof(ueSourceType), SourceType);
			list.Add(item);
			LogMessageUserField item2 = default(LogMessageUserField);
			item2.FieldName = "EventType";
			item2.Type = "string";
			item2.Value = EventType;
			list.Add(item2);
			m_LogClient.LogWrite(token, "Service", objId, objName, objLocationId, objLocationName, timestamp, "4.0.14", "LPRService", "Service", "", "System", "", Status.Success, Severity.Info, "VideoSourceStateChangedToOK", "The LPR camera state has been changed to 'OK'.", "en-US", variables, list);
		}

		public void VideoSourceStateChangedToWarning(string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, ueSourceType SourceType, string EventType)
		{
			Hashtable variables = new Hashtable();
			List<LogMessageUserField> list = new List<LogMessageUserField>();
			LogMessageUserField item = default(LogMessageUserField);
			item.FieldName = "SourceType";
			item.Type = "enum";
			item.Value = Enum.GetName(typeof(ueSourceType), SourceType);
			list.Add(item);
			LogMessageUserField item2 = default(LogMessageUserField);
			item2.FieldName = "EventType";
			item2.Type = "string";
			item2.Value = EventType;
			list.Add(item2);
			m_LogClient.LogWrite(token, "Service", objId, objName, objLocationId, objLocationName, timestamp, "4.0.14", "LPRService", "Service", "", "System", "", Status.Success, Severity.Info, "VideoSourceStateChangedToWarning", "The LPR camera state has been changed to 'Warning'.", "en-US", variables, list);
		}

		public void VideoSourceStateChangedToError(string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, ueSourceType SourceType, string EventType)
		{
			Hashtable variables = new Hashtable();
			List<LogMessageUserField> list = new List<LogMessageUserField>();
			LogMessageUserField item = default(LogMessageUserField);
			item.FieldName = "SourceType";
			item.Type = "enum";
			item.Value = Enum.GetName(typeof(ueSourceType), SourceType);
			list.Add(item);
			LogMessageUserField item2 = default(LogMessageUserField);
			item2.FieldName = "EventType";
			item2.Type = "string";
			item2.Value = EventType;
			list.Add(item2);
			m_LogClient.LogWrite(token, "Service", objId, objName, objLocationId, objLocationName, timestamp, "4.0.14", "LPRService", "Service", "", "System", "", Status.Success, Severity.Info, "VideoSourceStateChangedToError", "The LPR camera state has been changed to 'Error'.", "en-US", variables, list);
		}
	}

	private LogClient m_LogClient;

	public LPRService()
	{
		m_LogClient = new LogClient("en-US");
	}

	public LPRService(LogClient logClient)
	{
		m_LogClient = logClient;
	}

	public void SetServer(string server)
	{
		m_LogClient.SetServer(server);
	}

	public void SetServer(string server, string cacheID)
	{
		m_LogClient.SetServer(server, cacheID);
	}

	public void SetAuthentication(Authentication authentication)
	{
		m_LogClient.SetAuthentication(authentication);
	}

	public void SetCredentials(string username, string password)
	{
		m_LogClient.SetCredentials(username, password);
	}

	public void SetUserId(string userId)
	{
		m_LogClient.SetUserId(userId);
	}

	public void SetUserLocation(string userLocation)
	{
		m_LogClient.SetUserLocation(userLocation);
	}

	public void SetFileQueuePath(string path)
	{
		m_LogClient.SetFileQueuePath(path);
	}

	public void SetFileQueueFlushSize(int flushSize)
	{
		m_LogClient.SetFileQueueFlushSize(flushSize);
	}

	public void SetFileQueueFlushTimeout(int flushTimeoutInSeconds)
	{
		m_LogClient.SetFileQueueFlushTimeout(flushTimeoutInSeconds);
	}

	public bool UploadCultureFiles(string directory, bool recursive, bool forceupload)
	{
		return m_LogClient.UploadCultureFiles(directory, recursive, forceupload);
	}

	public bool UploadCultureFiles(string directory, bool recursive, bool forceupload, bool asynchronous)
	{
		return m_LogClient.UploadCultureFiles(directory, recursive, forceupload, asynchronous);
	}

	public bool UploadCultureFile(string file, bool forceupload)
	{
		return m_LogClient.UploadCultureFile(file, forceupload);
	}

	public bool UploadCultureFile(string file, bool forceupload, bool asynchronous)
	{
		return m_LogClient.UploadCultureFile(file, forceupload, asynchronous);
	}

	public void TrustCultureFiles()
	{
		m_LogClient.TrustCultureFiles();
	}

	public bool GetServerVersion(out int major, out int minor, out int revision)
	{
		return m_LogClient.GetServerVersion(out major, out minor, out revision);
	}
}
