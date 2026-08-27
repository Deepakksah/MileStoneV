using System;

namespace VideoOS.Platform.SDK.Multicast;

public class AsyncObjectState
{
	private MulticastManager.ResultCallbackEventHandler _resultCallbackEventHandler;

	private object _callerTag;

	private bool _handleCallbackException;

	private ulong _index;

	private Guid _id = Guid.Empty;

	public MulticastManager.ResultCallbackEventHandler ResultCallbackEventHandler => _resultCallbackEventHandler;

	public object CallerTag => _callerTag;

	public bool HandleCallbackException => _handleCallbackException;

	public ulong Index => _index;

	public Guid Id => _id;

	public AsyncObjectState(MulticastManager.ResultCallbackEventHandler resultCallbackEventHandler, object callerTag)
	{
		_resultCallbackEventHandler = resultCallbackEventHandler;
		_callerTag = callerTag;
	}

	public AsyncObjectState(MulticastManager.ResultCallbackEventHandler resultCallbackEventHandler, object callerTag, bool handleCallbackException)
	{
		_resultCallbackEventHandler = resultCallbackEventHandler;
		_callerTag = callerTag;
		_handleCallbackException = handleCallbackException;
	}

	public AsyncObjectState(MulticastManager.ResultCallbackEventHandler resultCallbackEventHandler, object callerTag, bool handleCallbackException, ulong index, Guid id)
	{
		_resultCallbackEventHandler = resultCallbackEventHandler;
		_callerTag = callerTag;
		_handleCallbackException = handleCallbackException;
		_index = index;
		_id = id;
	}
}
