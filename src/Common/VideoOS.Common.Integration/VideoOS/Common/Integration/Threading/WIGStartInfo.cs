using System;

namespace VideoOS.Common.Integration.Threading;

public class WIGStartInfo
{
	private bool _useCallerCallContext;

	private bool _useCallerHttpContext;

	private bool _disposeOfStateObjects;

	private CallToPostExecute _callToPostExecute;

	private PostExecuteWorkItemCallback _postExecuteWorkItemCallback;

	private bool _startSuspended;

	private WorkItemPriority _workItemPriority;

	private bool _fillStateWithArgs;

	protected bool _readOnly;

	public virtual bool UseCallerCallContext
	{
		get
		{
			return _useCallerCallContext;
		}
		set
		{
			ThrowIfReadOnly();
			_useCallerCallContext = value;
		}
	}

	public virtual bool UseCallerHttpContext
	{
		get
		{
			return _useCallerHttpContext;
		}
		set
		{
			ThrowIfReadOnly();
			_useCallerHttpContext = value;
		}
	}

	public virtual bool DisposeOfStateObjects
	{
		get
		{
			return _disposeOfStateObjects;
		}
		set
		{
			ThrowIfReadOnly();
			_disposeOfStateObjects = value;
		}
	}

	public virtual CallToPostExecute CallToPostExecute
	{
		get
		{
			return _callToPostExecute;
		}
		set
		{
			ThrowIfReadOnly();
			_callToPostExecute = value;
		}
	}

	public virtual PostExecuteWorkItemCallback PostExecuteWorkItemCallback
	{
		get
		{
			return _postExecuteWorkItemCallback;
		}
		set
		{
			ThrowIfReadOnly();
			_postExecuteWorkItemCallback = value;
		}
	}

	public virtual bool StartSuspended
	{
		get
		{
			return _startSuspended;
		}
		set
		{
			ThrowIfReadOnly();
			_startSuspended = value;
		}
	}

	public virtual WorkItemPriority WorkItemPriority
	{
		get
		{
			return _workItemPriority;
		}
		set
		{
			_workItemPriority = value;
		}
	}

	public virtual bool FillStateWithArgs
	{
		get
		{
			return _fillStateWithArgs;
		}
		set
		{
			ThrowIfReadOnly();
			_fillStateWithArgs = value;
		}
	}

	public WIGStartInfo()
	{
		_fillStateWithArgs = false;
		_workItemPriority = WorkItemPriority.Normal;
		_startSuspended = false;
		_postExecuteWorkItemCallback = SmartThreadPool.DefaultPostExecuteWorkItemCallback;
		_callToPostExecute = CallToPostExecute.Always;
		_disposeOfStateObjects = false;
		_useCallerHttpContext = false;
		_useCallerCallContext = false;
	}

	public WIGStartInfo(WIGStartInfo wigStartInfo)
	{
		if (wigStartInfo == null)
		{
			throw new ArgumentNullException("wigStartInfo");
		}
		_useCallerCallContext = wigStartInfo.UseCallerCallContext;
		_useCallerHttpContext = wigStartInfo.UseCallerHttpContext;
		_disposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
		_callToPostExecute = wigStartInfo.CallToPostExecute;
		_postExecuteWorkItemCallback = wigStartInfo.PostExecuteWorkItemCallback;
		_workItemPriority = wigStartInfo.WorkItemPriority;
		_startSuspended = wigStartInfo.StartSuspended;
		_fillStateWithArgs = wigStartInfo.FillStateWithArgs;
	}

	protected void ThrowIfReadOnly()
	{
		if (_readOnly)
		{
			throw new NotSupportedException("This is a readonly instance and set is not supported");
		}
	}

	public WIGStartInfo AsReadOnly()
	{
		return new WIGStartInfo(this)
		{
			_readOnly = true
		};
	}
}
