using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Log;

public class LogClient : LogClientInternal
{
	public LogClient()
	{
	}

	public LogClient(string culture)
	{
	}

	public void SetServer(string server)
	{
		DoSetServer(server);
	}

	public void SetServer(string server, string cacheID)
	{
		DoSetServer(server);
	}

	public void SetAuthentication(Authentication authentication)
	{
	}

	public void SetCredentials(string username, string password)
	{
		base.UserName = username;
	}

	public void SetFileQueuePath(string path)
	{
	}

	public void SetFileQueueFlushSize(int flushSize)
	{
	}

	public void SetFileQueueFlushTimeout(int flushTimeoutInSeconds)
	{
	}

	public int GetNumberOfCachedChunks()
	{
		return 0;
	}

	public void SetVersion(string version)
	{
		throw new NotImplementedException();
	}

	public void SetCulture(string culture)
	{
	}

	public void SetUserId(string userId)
	{
		base.UserId = userId;
	}

	public void SetUserLocation(string userLocation)
	{
		base.UserLocation = userLocation;
	}

	public void SetApplication(string application)
	{
		throw new NotImplementedException();
	}

	public void SetComponent(string component)
	{
		throw new NotImplementedException();
	}

	public void SetModule(string module)
	{
		throw new NotImplementedException();
	}

	public bool Ping()
	{
		return true;
	}

	public string GetStatus(string infotype)
	{
		return "";
	}

	public void TrustCultureFiles()
	{
	}

	public bool UploadCultureFile(string file, bool forceupload)
	{
		return UploadCultureFile(file, forceupload, asynchronous: false);
	}

	public bool UploadCultureFile(string file, bool forceupload, bool asynchronous)
	{
		return DoUploadCultureFile(file);
	}

	public bool UploadCultureFiles(string directory, bool recursive, bool forceupload)
	{
		return UploadCultureFiles(directory, recursive, forceupload, asynchronous: false);
	}

	public bool UploadCultureFiles(string directory, bool recursive, bool forceupload, bool asynchronous)
	{
		return DoUploadCultureFiles(directory, recursive);
	}

	public int GetLogMessages(string query, string culture, int page, int pagesize, out ArrayList result, out ArrayList names)
	{
		throw new NotImplementedException();
	}

	public int TranslateLogMessage(ArrayList messages, string query, string culture, int page, int pagesize, out ArrayList result)
	{
		throw new NotImplementedException();
	}

	public bool SetRetentionTime(string retentiontime)
	{
		return true;
	}

	public bool GetRetentionTime(out string retentiontime)
	{
		throw new NotImplementedException();
	}

	public bool GetServerVersion(out int major, out int minor, out int revision)
	{
		major = 12;
		minor = 1;
		revision = 1;
		return true;
	}

	public bool Clear(string Table, ClearType type, DateTime timestamp, int count)
	{
		throw new NotImplementedException();
	}

	public int GetColumnValues(string column, string culture, string table, string filter, int page, int pagesize, out ArrayList result)
	{
		throw new NotImplementedException();
	}

	public int GetColumnValuesEx(string column, string culture, string table, string filter, int page, int pagesize, bool getKey, out ArrayList result)
	{
		throw new NotImplementedException();
	}

	public void LogWrite(string token, string objectType, string objectId, string objectName, string objectLocationId, string objectLocationName, DateTime timestamp, string group, string category, Status status, Severity severity, string messageId, string messageText, Hashtable variables)
	{
		DoLogWrite(token, objectType, objectId, objectName, objectLocationId, objectLocationName, timestamp, "", "", "", "", group, category, status, severity, messageId, messageText, "", variables, null);
	}

	public void LogWrite(string token, string objectType, string objectId, string objectName, string objectLocationId, string objectLocationName, DateTime timestamp, string version, string application, string component, string module, string group, string category, Status status, Severity severity, string messageId, string messageText, string messageCulture, Hashtable variables)
	{
		DoLogWrite(token, objectType, objectId, objectName, objectLocationId, objectLocationName, timestamp, version, application, component, module, group, category, status, severity, messageId, messageText, messageCulture, variables, null);
	}

	public void LogWrite(string token, string objectType, string objectId, string objectName, string objectLocationId, string objectLocationName, DateTime timestamp, string group, string category, Status status, Severity severity, string messageId, string messageText, Hashtable variables, List<LogMessageUserField> fields)
	{
		DoLogWrite(token, objectType, objectId, objectName, objectLocationId, objectLocationName, timestamp, "", "", "", "", group, category, status, severity, messageId, messageText, "", variables, fields);
	}

	public void LogWrite(string token, string objectType, string objectId, string objectName, string objectLocationId, string objectLocationName, DateTime timestamp, string version, string application, string component, string module, string group, string category, Status status, Severity severity, string messageId, string messageText, string messageCulture, Hashtable variables, List<LogMessageUserField> fields)
	{
		DoLogWrite(token, objectType, objectId, objectName, objectLocationId, objectLocationName, timestamp, version, application, component, module, group, category, status, severity, messageId, messageText, messageCulture, variables, fields);
	}
}
