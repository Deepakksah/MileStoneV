using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Log;

namespace VideoOS.Platform.Common.Logging;

public interface ILogClientHandler
{
	void Dispose();

	int GetColumnValues(string column, string culture, string table, string filter, int page, int pagesize, out ArrayList result);

	long GetCount(string group, DateTime beginTime, DateTime endTime);

	void GetLogMessages(string query, string culture, int page, int pagesize, out ArrayList result, out ArrayList names);

	void LogWrite(string token, string objectType, string objectId, string objectName, string objectLocationId, string objectLocationName, DateTime timestamp, string version, string application, string component, string module, string group, string category, string status, string severity, string messageId, string messageText, string messageCulture, Hashtable variables, List<LogMessageUserField> fields);

	ArrayList ReadGroups();

	void ReadLogs(int page, out ArrayList result, out ArrayList names, string group);

	void ReadLogs(int page, out ArrayList result, out ArrayList names, string group, DateTime beginTime, DateTime endTime);

	void SetUserId(string userName);

	bool UploadCultureFile(string fileName, bool forceupload, bool asynchronous);
}
