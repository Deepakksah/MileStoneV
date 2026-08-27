using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Log;

public class LogMessageDictionary
{
	private readonly Guid _instanceId = Guid.NewGuid();

	private string _applicationName;

	public Guid InstanceId => _instanceId;

	public string Culture { get; set; }

	public string Version { get; set; }

	public string Application { get; set; }

	public string ApplicationName
	{
		get
		{
			return _applicationName ?? Application;
		}
		set
		{
			_applicationName = value;
		}
	}

	public string Component { get; set; }

	public string ResourceType { get; set; }

	public virtual Dictionary<string, LogMessage> LogMessages { get; private set; }

	public LogMessageDictionary(string culture, string version, string application, string applicationName, string component, Dictionary<string, LogMessage> logMessages, string resourceType)
	{
		Culture = culture;
		Version = version;
		Application = application;
		ApplicationName = applicationName;
		Component = component;
		LogMessages = logMessages;
		ResourceType = resourceType;
	}

	[Obsolete("This constructor does not allow for localized application name")]
	public LogMessageDictionary(string culture, string version, string application, string component, Dictionary<string, LogMessage> logMessages, string resourceType)
	{
		Culture = culture;
		Version = version;
		Application = application;
		Component = component;
		LogMessages = logMessages;
		ResourceType = resourceType;
	}

	[Obsolete("This constructor does not allow for localized application name")]
	public LogMessageDictionary(string culture, string version, string application, string component, string resourceType)
	{
		Culture = culture;
		Version = version;
		Application = application;
		Component = component;
		ResourceType = resourceType;
	}
}
