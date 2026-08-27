using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Log.LPRService_en_US;

[GeneratedCode("MilestoneSystems.LogService.InterfaceGenerator", "1.0.0.0")]
public class Legacy
{
	private LogClient m_LogClient;

	public Legacy()
	{
		m_LogClient = new LogClient("en-US");
	}

	public Legacy(LogClient logClient)
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

	public void ValidateMessage(string Id, string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, Hashtable vars, List<LogMessageUserField> fields)
	{
		if (vars.Count <= 0 && GetMessageVariables(Id).Count <= 0)
		{
			return;
		}
		ArrayList messageVariables = GetMessageVariables(Id);
		if (vars.Count == messageVariables.Count)
		{
			bool flag = true;
			IEnumerator enumerator = messageVariables.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (!vars.ContainsKey(enumerator))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return;
			}
		}
		Hashtable hashtable = new Hashtable();
		string text = "Arguments supplied for message - legacy id = '" + Id + "' - did not match definition";
		if (vars.Count > 0)
		{
			text = text + "supplied variables are : <" + vars.ToString() + ">";
		}
		text = ((messageVariables.Count <= 0) ? (text + " but no variables was expected.") : (text + " expected variables are : <" + messageVariables.ToString() + ">"));
		hashtable.Add("legacy", text);
		m_LogClient.LogWrite(token, "", objId, objName, objLocationId, objLocationName, timestamp, "", "", "", "", "", "", Status.StatusQuo, Severity.Info, "", "{legacy}", "en-US", hashtable, fields);
	}

	public void LogWrite(string Id, string token, string objId, string objName, string objLocationId, string objLocationName, DateTime timestamp, Hashtable vars, List<LogMessageUserField> fields)
	{
		ValidateMessage(Id, token, objId, objName, objLocationId, objLocationName, timestamp, vars, fields);
		Hashtable hashtable = new Hashtable();
		string value = "Unknown message - legacy id = '" + Id + "' values are : " + vars.ToString();
		hashtable.Add("legacy", value);
		m_LogClient.LogWrite(token, "", objId, objName, objLocationId, objLocationName, timestamp, "", "", "", "", "", "", Status.StatusQuo, Severity.Info, "", "{legacy}", "en-US", hashtable, fields);
	}

	public ArrayList GetMessageVariables(string Id)
	{
		return new ArrayList();
	}
}
