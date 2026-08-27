using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml.XPath;

namespace VideoOS.Common.Integration.Command;

public abstract class TaskCommandBase : CommandBase, ITaskCommandBase
{
	protected const int InfiniteRetryCount = -1;

	private bool _permanent;

	public long TaskId { get; private set; }

	public string TaskXml { get; private set; }

	public int RetryCount { get; set; }

	protected int MaxRetryCount { get; set; }

	public bool StopRetry
	{
		get
		{
			if (MaxRetryCount != -1)
			{
				return RetryCount >= MaxRetryCount;
			}
			return false;
		}
	}

	public bool Permanent
	{
		[DebuggerStepThrough]
		get
		{
			return _permanent;
		}
		[DebuggerStepThrough]
		set
		{
			_permanent = value;
		}
	}

	protected TaskCommandBase(string taskXml)
	{
		TaskXml = taskXml;
		MaxRetryCount = 0;
		RetryCount = 0;
	}

	protected virtual void Initialize(XPathNavigator taskNode)
	{
		if (taskNode == null)
		{
			throw new ArgumentNullException("taskNode");
		}
		if (!long.TryParse(taskNode.GetAttribute("id", string.Empty), out var result))
		{
			throw new InvalidTaskXmlException("The task xml contains no 'id' attribute containing an int node.");
		}
		TaskId = result;
	}

	protected virtual Guid ReadTaskGuidNodeValue(XPathNavigator taskNode, string nodeName)
	{
		if (taskNode == null)
		{
			throw new ArgumentNullException("taskNode");
		}
		if (nodeName == null)
		{
			throw new ArgumentNullException("nodeName");
		}
		XPathNavigator obj = taskNode.SelectSingleNode(nodeName) ?? throw new InvalidTaskXmlException("The task xml contains no '" + nodeName + "' node.");
		if (!obj.Value.IsGuid())
		{
			throw new InvalidTaskXmlException("The task xml contains an '" + nodeName + "' that does not contain a guid.");
		}
		return new Guid(obj.Value);
	}

	protected virtual string ReadTaskStringNodeValue(XPathNavigator taskNode, string nodeName)
	{
		if (taskNode == null)
		{
			throw new ArgumentNullException("taskNode");
		}
		if (nodeName == null)
		{
			throw new ArgumentNullException("nodeName");
		}
		return (taskNode.SelectSingleNode(nodeName) ?? throw new InvalidTaskXmlException("The task xml contains no '" + nodeName + "' node.")).Value;
	}

	protected virtual bool TryReadTaskStringNodeValue(XPathNavigator taskNode, string nodeName, out string nodeValue)
	{
		if (taskNode == null)
		{
			throw new ArgumentNullException("taskNode");
		}
		if (nodeName == null)
		{
			throw new ArgumentNullException("nodeName");
		}
		XPathNavigator xPathNavigator = taskNode.SelectSingleNode(nodeName);
		if (xPathNavigator == null)
		{
			nodeValue = null;
			return false;
		}
		nodeValue = xPathNavigator.Value;
		return true;
	}

	protected virtual bool ReadTaskBoolNodeValue(XPathNavigator taskNode, string nodeName)
	{
		if (taskNode == null)
		{
			throw new ArgumentNullException("taskNode");
		}
		if (nodeName == null)
		{
			throw new ArgumentNullException("nodeName");
		}
		XPathNavigator xPathNavigator = taskNode.SelectSingleNode(nodeName);
		if (xPathNavigator == null)
		{
			throw new InvalidTaskXmlException("The task xml contains no '" + nodeName + "' node.");
		}
		if (xPathNavigator.Value != "1" && xPathNavigator.Value != "0")
		{
			throw new InvalidTaskXmlException("The task xml contains an '" + nodeName + "' that does not contain a bool value.");
		}
		return xPathNavigator.Value == "1";
	}

	protected virtual DateTime ReadTaskDateTimeNodeValue(XPathNavigator taskNode, string nodeName)
	{
		if (taskNode == null)
		{
			throw new ArgumentNullException("taskNode");
		}
		if (nodeName == null)
		{
			throw new ArgumentNullException("nodeName");
		}
		if (!DateTime.TryParse((taskNode.SelectSingleNode(nodeName) ?? throw new InvalidTaskXmlException("The task xml contains no '" + nodeName + "' node.")).Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
		{
			throw new InvalidTaskXmlException("The task xml contains an '" + nodeName + "' that does not contain a valid date time.");
		}
		return result.ToUniversalTime();
	}

	protected virtual int ReadTaskIntegerNodeValue(XPathNavigator taskNode, string nodeName)
	{
		if (taskNode == null)
		{
			throw new ArgumentNullException("taskNode");
		}
		if (nodeName == null)
		{
			throw new ArgumentNullException("nodeName");
		}
		if (!int.TryParse((taskNode.SelectSingleNode(nodeName) ?? throw new InvalidTaskXmlException("The task xml contains no '" + nodeName + "' node.")).Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			throw new InvalidTaskXmlException("The task xml contains an '" + nodeName + "' that does not contain a valid integer.");
		}
		return result;
	}

	protected virtual Uri ReadTaskUriNodeValue(XPathNavigator taskNode, string nodeName)
	{
		if (taskNode == null)
		{
			throw new ArgumentNullException("taskNode");
		}
		if (nodeName == null)
		{
			throw new ArgumentNullException("nodeName");
		}
		if (!Uri.TryCreate((taskNode.SelectSingleNode(nodeName) ?? throw new InvalidTaskXmlException("The task xml contains no '" + nodeName + "' node.")).Value, UriKind.Absolute, out var result))
		{
			throw new InvalidTaskXmlException("The task xml contains an '" + nodeName + "' that does not contain a valid integer.");
		}
		return result;
	}

	public static DateTime CalculateEstimatedEndTime(DateTime taskStartTimeUtc, int percentComplete)
	{
		taskStartTimeUtc = taskStartTimeUtc.ToUniversalTime();
		DateTime result = DateTime.MinValue;
		if (percentComplete > 0)
		{
			double value = DateTime.UtcNow.Subtract(taskStartTimeUtc).TotalMilliseconds / (double)percentComplete * 100.0;
			result = taskStartTimeUtc.AddMilliseconds(value);
		}
		return result;
	}
}
