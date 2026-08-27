using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace VideoOS.Management.Server;

public sealed class MethodInformation
{
	private string _webMethodName;

	private MethodType _methodType;

	private EstimatedMethodCost _estimatedCost;

	private TimeSpan _timeout;

	private NameValueCollection _parameters;

	public bool InProgress { get; set; }

	public string Name => _webMethodName;

	public MethodType MethodType => _methodType;

	public EstimatedMethodCost EstimatedCost => _estimatedCost;

	public TimeSpan Timeout => _timeout;

	public NameValueCollection Parameters => _parameters;

	public MethodInformation([CallerMemberName] string webMethodName = null)
		: this(webMethodName, MethodType.Normal, EstimatedMethodCost.Low, default(TimeSpan))
	{
	}

	public MethodInformation(string webMethodName, MethodType methodType, EstimatedMethodCost estimatedCost, TimeSpan timeout)
	{
		_webMethodName = webMethodName;
		_methodType = methodType;
		_estimatedCost = estimatedCost;
		_timeout = timeout;
		_parameters = new NameValueCollection();
	}

	public void AddParameter(string parameterName, object parameterValue)
	{
		string value = "null";
		if (parameterValue != null)
		{
			try
			{
				value = parameterValue.ToString();
			}
			catch (Exception ex)
			{
				GeminiTrace.Error("MethodInformation.AddParameter can't ToString on value passed", ex);
				value = ex.Message;
			}
		}
		_parameters[parameterName] = value;
	}

	public void AddArrayParameter(string parameterName, IEnumerable parameterArray)
	{
		if (parameterArray == null)
		{
			return;
		}
		int num = 0;
		foreach (object item in parameterArray)
		{
			if (num < 10)
			{
				AddParameter(string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", parameterName, num), item);
				num++;
				continue;
			}
			break;
		}
		AddParameter(parameterName, string.Format(CultureInfo.InvariantCulture, "Count={0}", num));
	}

	public void AddArrayParameter<T>(string parameterName, IList<T> parameterArray)
	{
		AddArrayParameter(parameterName, (IList)parameterArray);
	}

	public void AddArrayArrayParameter(string parameterName, IList<IList> parameterArray)
	{
		if (parameterArray == null)
		{
			return;
		}
		foreach (IList item in parameterArray)
		{
			AddArrayParameter(parameterName, item);
		}
	}
}
