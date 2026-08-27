using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Common.Integration.Command;

public class CommandProcessor
{
	private class CommandEntry
	{
		public ICommandHandler CommandHandler;

		public ExecutorType CommandExecutorType;

		public CommandEntry(ICommandHandler commandHandler, ExecutorType commandExecutorType)
		{
			CommandHandler = commandHandler;
			CommandExecutorType = commandExecutorType;
		}
	}

	private static Hashtable _commandExecutors;

	private static Hashtable _commandEntries;

	private static object _tablesLock;

	private static ILogger _logger;

	private static NullResponseSink _nullResponseSink;

	public static NullResponseSink NullResponseSink => _nullResponseSink;

	public static ILogger Logger
	{
		set
		{
			_logger = value;
		}
	}

	static CommandProcessor()
	{
		_commandExecutors = new Hashtable();
		_commandEntries = new Hashtable();
		_tablesLock = new object();
		_logger = null;
		_nullResponseSink = new NullResponseSink();
		_commandExecutors.Clear();
		_commandExecutors.Add(ExecutorType.Direct, new DirectCommandExecutor());
		_commandExecutors.Add(ExecutorType.ThreadPool, new ThreadPoolCommandExecutor());
		_commandExecutors.Add(ExecutorType.AdvancedThreadPool, new AdvancedThreadPoolCommandExecutor());
		_commandExecutors.Add(ExecutorType.Queued, new QueuedCommandExecutor());
		_commandExecutors.Add(ExecutorType.Thread, new ThreadCommandExecutor());
	}

	public static void Close()
	{
		Parallel.ForEach(_commandExecutors.Values.Cast<ICommandExecutor>(), delegate(ICommandExecutor commandExecutor)
		{
			commandExecutor.Close(TimeSpan.FromSeconds(60.0));
		});
	}

	public static void LogUnhandledCommandHandlerException(CommandBase command, Exception ex)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		try
		{
			if (_logger != null)
			{
				LoggerExtensions.LogWarning(_logger, "CommandHandler unhandled exception, token={Token}, type={Type}, exception={Exception}", new object[3]
				{
					command.Token,
					command.GetType(),
					ex
				});
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	public static void Register(Type commandType, ICommandHandler commandHandler, ExecutorType commandExecutorType)
	{
		lock (_tablesLock)
		{
			if (!_commandEntries.Contains(commandType))
			{
				CommandEntry value = new CommandEntry(commandHandler, commandExecutorType);
				_commandEntries.Add(commandType, value);
			}
		}
	}

	public static void ProcessAsync(CommandBase command, object state, ResponseDelegate responseCallback)
	{
		ProcessAsync(command, state, new DefaultResponseSink(command, responseCallback));
	}

	public static void ProcessAsync(CommandBase command, object state, IResponseHandler responseHandler)
	{
		ProcessAsync(command, state, new DefaultResponseSink(command, responseHandler.HandleResponse));
	}

	public static void ProcessAsync(CommandBase command, object state, IResponseSink responseSink)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		CommandEntry commandEntry = null;
		ICommandExecutor commandExecutor = null;
		lock (_tablesLock)
		{
			commandEntry = _commandEntries[command.GetType()] as CommandEntry;
			if (commandEntry == null)
			{
				responseSink.AddResponse(new ErrorResponse(-1, "Command not registered"));
				return;
			}
			commandExecutor = (ICommandExecutor)_commandExecutors[commandEntry.CommandExecutorType];
		}
		commandExecutor.Process(command, state, commandEntry.CommandHandler.Clone(), responseSink);
	}

	public static void ProcessAsync(CommandBase command, object state, ResponseDelegate responseCallback, ExecutorType executorType)
	{
		ProcessAsync(command, state, new DefaultResponseSink(command, responseCallback), executorType);
	}

	public static void ProcessAsync(CommandBase command, object state, IResponseHandler responseHandler, ExecutorType executorType)
	{
		ProcessAsync(command, state, new DefaultResponseSink(command, responseHandler.HandleResponse), executorType);
	}

	public static void ProcessAsync(CommandBase command, object state, IResponseSink responseSink, ExecutorType executorType)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		CommandEntry commandEntry = null;
		ICommandExecutor commandExecutor = null;
		lock (_tablesLock)
		{
			commandEntry = _commandEntries[command.GetType()] as CommandEntry;
			if (commandEntry == null)
			{
				responseSink.AddResponse(new ErrorResponse(-1, "Command not registered"));
				return;
			}
			commandExecutor = (ICommandExecutor)_commandExecutors[executorType];
		}
		commandExecutor.Process(command, state, commandEntry.CommandHandler.Clone(), responseSink);
	}

	public static void ProcessSync(CommandBase command, object state, ResponseDelegate responseCallback)
	{
		ProcessAsync(command, state, responseCallback, ExecutorType.Direct);
	}

	public static void ProcessSync(CommandBase command, object state, IResponseHandler responseHandler)
	{
		ProcessAsync(command, state, responseHandler, ExecutorType.Direct);
	}

	public static void ProcessSync(CommandBase command, object state, IResponseSink responseSink)
	{
		ProcessAsync(command, state, responseSink, ExecutorType.Direct);
	}
}
