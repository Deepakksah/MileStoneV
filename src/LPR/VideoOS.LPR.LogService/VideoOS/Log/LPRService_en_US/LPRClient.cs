using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Log.LPRService_en_US;

[GeneratedCode("MilestoneSystems.LogService.InterfaceGenerator", "1.0.0.0")]
public class LPRClient
{
	[GeneratedCode("MilestoneSystems.LogService.InterfaceGenerator", "1.0.0.0")]
	public class Client
	{
		private LogClient m_LogClient;

		public Client()
		{
			m_LogClient = new LogClient("en-US");
		}

		public Client(LogClient logClient)
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

		public void LPRListsAdd(string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, ueSourceType SourceType, ueListType ListType, string plateName, string listName)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("plateName", plateName);
			hashtable.Add("listName", listName);
			List<LogMessageUserField> list = new List<LogMessageUserField>();
			LogMessageUserField item = default(LogMessageUserField);
			item.FieldName = "SourceType";
			item.Type = "enum";
			item.Value = Enum.GetName(typeof(ueSourceType), SourceType);
			list.Add(item);
			LogMessageUserField item2 = default(LogMessageUserField);
			item2.FieldName = "ListType";
			item2.Type = "enum";
			item2.Value = Enum.GetName(typeof(ueListType), ListType);
			list.Add(item2);
			m_LogClient.LogWrite(token, "LPR license plate list", objId, objName, objLocationId, objLocationName, timestamp, "4.0.14", "LPRClient", "Client", "", "Audit", "Edit_LPR_license_plate_list", Status.Success, Severity.Info, "LPRListsAdd", "License plate: {plateName} added to LPR license plate list: {listName}.", "en-US", hashtable, list);
		}

		public void LPRListsDelete(string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, ueSourceType SourceType, ueListType ListType, string plateName, string listName)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("plateName", plateName);
			hashtable.Add("listName", listName);
			List<LogMessageUserField> list = new List<LogMessageUserField>();
			LogMessageUserField item = default(LogMessageUserField);
			item.FieldName = "SourceType";
			item.Type = "enum";
			item.Value = Enum.GetName(typeof(ueSourceType), SourceType);
			list.Add(item);
			LogMessageUserField item2 = default(LogMessageUserField);
			item2.FieldName = "ListType";
			item2.Type = "enum";
			item2.Value = Enum.GetName(typeof(ueListType), ListType);
			list.Add(item2);
			m_LogClient.LogWrite(token, "LPR license plate list", objId, objName, objLocationId, objLocationName, timestamp, "4.0.14", "LPRClient", "Client", "", "Audit", "Edit_LPR_license_plate_list", Status.Success, Severity.Info, "LPRListsDelete", "License plate: {plateName} deleted from LPR license plate list: {listName}.", "en-US", hashtable, list);
		}

		public void LPRListsSearch(string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, ueSourceType SourceType, ueListType ListType, string plateName, string listName)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("plateName", plateName);
			hashtable.Add("listName", listName);
			List<LogMessageUserField> list = new List<LogMessageUserField>();
			LogMessageUserField item = default(LogMessageUserField);
			item.FieldName = "SourceType";
			item.Type = "enum";
			item.Value = Enum.GetName(typeof(ueSourceType), SourceType);
			list.Add(item);
			LogMessageUserField item2 = default(LogMessageUserField);
			item2.FieldName = "ListType";
			item2.Type = "enum";
			item2.Value = Enum.GetName(typeof(ueListType), ListType);
			list.Add(item2);
			m_LogClient.LogWrite(token, "LPR license plate list", objId, objName, objLocationId, objLocationName, timestamp, "4.0.14", "LPRClient", "Client", "", "Audit", "Edit_LPR_license_plate_list", Status.Success, Severity.Info, "LPRListsSearch", "Searched license plate: {plateName} in LPR license plate list: {listName}.", "en-US", hashtable, list);
		}

		public void LPREventSearch(string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, ueSourceType SourceType, ueListType ListType, string searchFilter)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("searchFilter", searchFilter);
			List<LogMessageUserField> list = new List<LogMessageUserField>();
			LogMessageUserField item = default(LogMessageUserField);
			item.FieldName = "SourceType";
			item.Type = "enum";
			item.Value = Enum.GetName(typeof(ueSourceType), SourceType);
			list.Add(item);
			LogMessageUserField item2 = default(LogMessageUserField);
			item2.FieldName = "ListType";
			item2.Type = "enum";
			item2.Value = Enum.GetName(typeof(ueListType), ListType);
			list.Add(item2);
			LogMessageUserField item3 = default(LogMessageUserField);
			item3.FieldName = "ResourceName";
			item3.Type = "string";
			item3.Value = "LPR event";
			list.Add(item3);
			LogMessageUserField item4 = default(LogMessageUserField);
			item4.FieldName = "ResourceType";
			item4.Type = "enum";
			item4.Value = "LPR";
			list.Add(item4);
			m_LogClient.LogWrite(token, "LPR event search", objId, objName, objLocationId, objLocationName, timestamp, "4.0.14", "LPRClient", "Client", "", "Audit", "LPRSearch", Status.Success, Severity.Info, "LPREventSearch", "Searched license plate event with filter: {searchFilter}.", "en-US", hashtable, list);
		}
	}

	private LogClient m_LogClient;

	public LPRClient()
	{
		m_LogClient = new LogClient("en-US");
	}

	public LPRClient(LogClient logClient)
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
