using System;
using System.Collections;
using System.Threading;

namespace VideoOS.Common.Integration.Command;

public class ThreadCommandExecutor : ICommandExecutor
{
	public class ThreadObject
	{
		private ThreadCommandExecutor _parent;

		private CommandBase _command;

		private object _state;

		private ICommandHandler _commandHandler;

		private IResponseSink _responseSink;

		private Thread _thread;

		public ThreadObject(ThreadCommandExecutor parent, CommandBase command, object state, ICommandHandler commandHandler, IResponseSink responseSink)
		{
			_parent = parent;
			_command = command;
			_state = state;
			_commandHandler = commandHandler;
			_responseSink = responseSink;
		}

		public void Start()
		{
			_thread = new Thread(ThreadFunction);
			_thread.Name = "ThreadObject-" + _command.ToString();
			_thread.IsBackground = true;
			_thread.Start();
		}

		public void ThreadFunction()
		{
			try
			{
				_commandHandler.Execute(_command, _state, _responseSink);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				try
				{
					CommandProcessor.LogUnhandledCommandHandlerException(_command, ex2);
					_responseSink.AddResponse(new ExceptionResponse(ex2));
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
			}
			finally
			{
				_parent.ThreadDone(this);
			}
		}
	}

	private volatile bool _isClosing;

	private ArrayList _threadObjects = new ArrayList();

	private ManualResetEvent _allThreadsDoneEvent = new ManualResetEvent(initialState: true);

	public void Close(TimeSpan timeout)
	{
		_isClosing = true;
		_allThreadsDoneEvent.WaitOne(timeout, exitContext: false);
	}

	public void Process(CommandBase command, object state, ICommandHandler commandHandler, IResponseSink responseSink)
	{
		if (_isClosing)
		{
			return;
		}
		lock (this)
		{
			_allThreadsDoneEvent.Reset();
			ThreadObject threadObject = new ThreadObject(this, command, state, commandHandler, responseSink);
			_threadObjects.Add(threadObject);
			threadObject.Start();
		}
	}

	private void ThreadDone(ThreadObject threadObject)
	{
		lock (this)
		{
			_threadObjects.Remove(threadObject);
			if (_threadObjects.Count == 0)
			{
				_allThreadsDoneEvent.Set();
			}
		}
	}
}
