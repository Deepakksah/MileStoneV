using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Xml;

namespace VideoOS.Management.Common;

public abstract class WebServiceManagerBase<TClient, TChannel> : IDisposable where TClient : ICommunicationObject where TChannel : class
{
	private IWebServiceManagerExceptionHandler _exceptionHandler;

	public TClient Instance { get; set; }

	protected WebServiceManagerBase()
	{
	}

	protected WebServiceManagerBase(IWebServiceManagerExceptionHandler exceptionHandler)
	{
		if (exceptionHandler == null)
		{
			throw new ArgumentNullException("exceptionHandler");
		}
		_exceptionHandler = exceptionHandler;
	}

	private void Connect()
	{
		InitializeConnection();
	}

	private void Disconnect()
	{
		CloseConnection();
		Instance = default(TClient);
	}

	public virtual void CloseConnection()
	{
		if (Instance != null)
		{
			try
			{
				Instance.Close();
			}
			catch (CommunicationException)
			{
				Instance.Abort();
			}
			catch (TimeoutException)
			{
				Instance.Abort();
			}
			catch (Exception)
			{
				Instance.Abort();
				throw;
			}
		}
	}

	protected abstract TClient CreateClientObject();

	public virtual void InitializeConnection()
	{
		Instance = CreateClientObject();
		Instance.Open();
	}

	protected static Binding CreateDefaultBinding()
	{
		return new WSHttpBinding
		{
			Security = 
			{
				Message = 
				{
					ClientCredentialType = MessageCredentialType.Windows
				}
			},
			MaxBufferPoolSize = 2147483647L,
			MaxReceivedMessageSize = 2147483647L,
			CloseTimeout = TimeSpan.FromMinutes(10.0),
			SendTimeout = TimeSpan.FromMinutes(10.0),
			BypassProxyOnLocal = false,
			TransactionFlow = false,
			HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
			MessageEncoding = WSMessageEncoding.Text,
			TextEncoding = Encoding.UTF8,
			UseDefaultWebProxy = true,
			AllowCookies = false,
			ReaderQuotas = XmlDictionaryReaderQuotas.Max
		};
	}

	public void CallWithRetry(Action<TClient> call, Action exceptionTraceCall)
	{
		if (_exceptionHandler != null)
		{
			CallWithRetry(call, exceptionTraceCall, _exceptionHandler.HandleException);
		}
		else
		{
			CallWithRetry(call, exceptionTraceCall, null);
		}
	}

	public TResult CallWithRetry<TResult>(Func<TClient, TResult> call, Action exceptionTraceCall)
	{
		if (_exceptionHandler != null)
		{
			return CallWithRetry(call, exceptionTraceCall, _exceptionHandler.HandleException);
		}
		return CallWithRetry(call, exceptionTraceCall, null);
	}

	public void CallWithRetry(Action<TClient> call, Action exceptionTraceCall, Func<Exception, Action, bool> manualErrorHandler)
	{
		CallWithRetry(delegate(TClient s)
		{
			call(s);
			return (object)null;
		}, exceptionTraceCall, manualErrorHandler);
	}

	public TResult CallWithRetry<TResult>(Func<TClient, TResult> call, Action exceptionTraceCall, Func<Exception, Action, bool> manualErrorHandler)
	{
		if (call == null)
		{
			throw new ArgumentNullException("call");
		}
		try
		{
			try
			{
				EnsureConnection();
				return call(Instance);
			}
			catch (FaultException)
			{
				throw;
			}
			catch (SecurityNegotiationException)
			{
				throw;
			}
			catch (CommunicationException)
			{
				CloseConnection();
				Connect();
				return call(Instance);
			}
			catch (ObjectDisposedException)
			{
				CloseConnection();
				Connect();
				return call(Instance);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception arg)
		{
			if (exceptionTraceCall != null && manualErrorHandler == null)
			{
				exceptionTraceCall();
			}
			if (manualErrorHandler != null)
			{
				if (manualErrorHandler(arg, exceptionTraceCall))
				{
					throw;
				}
				return default(TResult);
			}
			throw;
		}
	}

	public void EnsureConnection()
	{
		if (Instance == null)
		{
			Connect();
			return;
		}
		switch (Instance.State)
		{
		case CommunicationState.Closed:
			Connect();
			break;
		case CommunicationState.Faulted:
			CloseConnection();
			Connect();
			break;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			Disconnect();
		}
	}
}
