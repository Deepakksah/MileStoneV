using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;

namespace VideoOS.Management.VmoClient;

[EntityName("Task")]
public abstract class AsyncTask : NonCachedVmoObject, IAsyncTask
{
	private TaskError errorResultInstance;

	public Guid Target => GetPropertyValue<Guid>("Target");

	public int PercentCompleted => GetPropertyValue<int>("PercentCompleted");

	public abstract string Name { get; }

	public abstract string EntityName { get; }

	public DateTime StartTime => GetPropertyValue<DateTime>("StartTime");

	public DateTime EndTime => GetPropertyValue<DateTime>("EndTime");

	public int Id => GetPropertyValue<int>("Id");

	public string StatusDetails => GetPropertyValue<string>("StatusDetails");

	public AsyncTaskStatus Status
	{
		get
		{
			if (Enum.TryParse<AsyncTaskStatus>(GetPropertyValue<string>("Status"), out var result))
			{
				return result;
			}
			return AsyncTaskStatus.Unknown;
		}
	}

	protected TaskError ErrorResultInstance
	{
		get
		{
			if (Status != AsyncTaskStatus.Failed)
			{
				throw new InvalidOperationException("Not possible to get error result if task status is not 'Failed'");
			}
			if (errorResultInstance != null)
			{
				return errorResultInstance;
			}
			TaskError taskError = null;
			try
			{
				taskError = TaskError.FromXml(StatusDetails);
			}
			catch (InvalidOperationException)
			{
				taskError = new TaskError
				{
					ErrorCode = 0,
					ErrorText = StatusDetails
				};
			}
			PrepareLocalizedErrorMessage(taskError);
			errorResultInstance = taskError;
			return taskError;
		}
	}

	public AsyncTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(objectProperties, repositories, internalId)
	{
		SetPropertyValue("Id", internalId);
	}

	public void Refresh()
	{
		string id = Id.ToString(CultureInfo.InvariantCulture);
		AsyncTask objectById = GetObjectById<AsyncTask>(id);
		if (objectById == null)
		{
			SetPropertyValue("Status", AsyncTaskStatus.Removed.ToString());
		}
		else
		{
			base.ObjectProperties = objectById.ObjectProperties;
		}
	}

	public void Cleanup()
	{
		if (Status != AsyncTaskStatus.Removed)
		{
			base.Repositories.ServerCommunication.CleanupTask(base.InternalId);
			SetPropertyValue("Status", AsyncTaskStatus.Removed.ToString());
		}
	}

	protected T GetNestedInputPropertyValue<T>(string propertyName)
	{
		return GetNestedPropertyValue<T>("InputParameters", propertyName);
	}

	protected static string ResolveLocalizedErrorMessage(string message, int code, ArgumentCollection arguments)
	{
		if (message == null)
		{
			throw new ArgumentNullException("message");
		}
		if (arguments == null)
		{
			throw new ArgumentNullException("arguments");
		}
		CultureInfo culture = VmoClient.Environment.ResolveUICulture();
		string errorMessage = VmoResourcesLookup.GetErrorMessage(string.Format(CultureInfo.InvariantCulture, "VMO{0}", code), culture);
		if (!string.IsNullOrEmpty(errorMessage))
		{
			List<string> list = new List<string>();
			for (int i = 0; i < arguments.Count; i++)
			{
				Argument argument = arguments[i];
				if (string.IsNullOrEmpty(argument.ResourceId))
				{
					list.Add(argument.Value);
					continue;
				}
				string messageParameterString = VmoResourcesLookup.GetMessageParameterString(argument.ResourceId, culture);
				if (messageParameterString != null)
				{
					list.Add(messageParameterString);
				}
				else
				{
					list.Add(argument.Value);
				}
			}
			string[] array = list.ToArray();
			try
			{
				CultureInfo currentCulture = CultureInfo.CurrentCulture;
				object[] args = array;
				return string.Format(currentCulture, errorMessage, args);
			}
			catch (FormatException)
			{
			}
		}
		return TryRemoveVmoErrorMessagePrefix(message);
	}

	private static void PrepareLocalizedErrorMessage(TaskError taskError)
	{
		if (taskError != null)
		{
			taskError.ErrorText = ResolveLocalizedErrorMessage(taskError.ErrorText, taskError.ErrorCode, taskError.Arguments);
		}
	}

	private static string TryRemoveVmoErrorMessagePrefix(string input)
	{
		if (input != null && Regex.IsMatch(input, "^VMO\\d{5}:\\s"))
		{
			input = input.Substring(10);
		}
		return input;
	}
}
