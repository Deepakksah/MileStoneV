using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.WebService.Common.Framework;

public abstract class ManagerBase<TCommunicationObject, TChannel> : ITimeoutSettings, IRetryChannel<TChannel> where TCommunicationObject : ICommunicationObject where TChannel : class
{
	private class AdditionalHeaderBehaviour : IEndpointBehavior
	{
		private IDictionary<string, MessageHeader> _extraHeaders;

		public AdditionalHeaderBehaviour(IDictionary<string, MessageHeader> extraHeaders)
		{
			_extraHeaders = extraHeaders;
		}

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			clientRuntime.ClientMessageInspectors.Add(new AdditionalHeaderInspector(_extraHeaders));
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
		}

		public void Validate(ServiceEndpoint endpoint)
		{
		}
	}

	private class AdditionalHeaderInspector : IClientMessageInspector
	{
		private IDictionary<string, MessageHeader> _extraHeaders;

		public AdditionalHeaderInspector(IDictionary<string, MessageHeader> extraHeaders)
		{
			_extraHeaders = extraHeaders;
		}

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			foreach (MessageHeader value in _extraHeaders.Values)
			{
				int num = request.Headers.FindHeader(value.Name, value.Namespace);
				if (num > -1)
				{
					request.Headers.RemoveAt(num);
				}
				request.Headers.Add(value);
			}
			return null;
		}

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
		}
	}

	private readonly string _namespaceName;

	private readonly Dictionary<string, MessageHeader> _extraHeaders = new Dictionary<string, MessageHeader>(StringComparer.OrdinalIgnoreCase);

	protected IWebServiceManagerExceptionHandler ExceptionHandler { get; set; }

	protected ServerAuthentication AuthenticationType { get; set; }

	protected TCommunicationObject CommunicationObject { get; set; }

	protected TChannel Channel { get; set; }

	protected ITokenCache TokenCache { get; private set; }

	protected string UserName { get; set; }

	protected string Password { get; set; }

	protected Uri ServerUrl { get; set; }

	public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1.0);

	protected ManagerBase(Uri serverUrl, string namespaceName, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: this(serverUrl, namespaceName, exceptionHandler)
	{
		TokenCache = tokenCache ?? throw new ArgumentNullException("tokenCache");
		AuthenticationType = ((serverUrl.Scheme == Uri.UriSchemeHttps) ? ServerAuthentication.OAuthSecure : ServerAuthentication.OAuthUnsecure);
	}

	protected ManagerBase(Uri serverUrl, string namespaceName, IWebServiceManagerExceptionHandler exceptionHandler)
	{
		ServerUrl = serverUrl ?? throw new ArgumentNullException("serverUrl");
		_namespaceName = namespaceName;
		ExceptionHandler = exceptionHandler;
	}

	public void CallWithRetry(Action<TChannel> call)
	{
		if (ExceptionHandler != null)
		{
			CallWithRetry(call, ExceptionHandler.HandleException);
		}
		else
		{
			CallWithRetry(call, null);
		}
	}

	public TResult CallWithRetry<TResult>(Func<TChannel, TResult> call)
	{
		if (ExceptionHandler != null)
		{
			return CallWithRetry(call, ExceptionHandler.HandleException);
		}
		return CallWithRetry(call, null);
	}

	public void CallWithRetry(Action<TChannel> call, Func<Exception, bool> manualErrorHandler)
	{
		if (call == null)
		{
			throw new ArgumentNullException("call");
		}
		CallWithRetry(delegate(TChannel s)
		{
			call(s);
			return (object)null;
		}, manualErrorHandler);
	}

	public TResult CallWithRetry<TResult>(Func<TChannel, TResult> call, Func<Exception, bool> manualErrorHandler)
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
				TChannel arg = (CommunicationObject as TChannel) ?? Channel;
				return call(arg);
			}
			catch (FaultException)
			{
				throw;
			}
			catch (SecurityNegotiationException)
			{
				throw;
			}
			catch (CommunicationException ex3)
			{
				if (ex3.InnerException != null && (ex3.InnerException is IdpException || ex3.InnerException is IdentityServerTokenException))
				{
					throw;
				}
				CloseConnection();
				Connect();
				TChannel arg = (CommunicationObject as TChannel) ?? Channel;
				return call(arg);
			}
			catch (ObjectDisposedException)
			{
				CloseConnection();
				Connect();
				TChannel arg = (CommunicationObject as TChannel) ?? Channel;
				return call(arg);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception arg2)
		{
			if (manualErrorHandler != null)
			{
				if (manualErrorHandler(arg2))
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
		if (CommunicationObject == null)
		{
			Connect();
			return;
		}
		switch (CommunicationObject.State)
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

	public abstract void Initialize();

	public virtual void Connect()
	{
		Initialize();
	}

	public virtual void CloseConnection()
	{
		if (CommunicationObject != null)
		{
			try
			{
				CommunicationObject.Close();
			}
			catch (CommunicationException)
			{
				CommunicationObject.Abort();
			}
			catch (TimeoutException)
			{
				CommunicationObject.Abort();
			}
			catch (Exception)
			{
				CommunicationObject.Abort();
				throw;
			}
		}
	}

	public void ConfigureEndpoint(ServiceEndpoint serviceEndpoint)
	{
		ManagementServerWcfHelper.ConfigureEndpoint(AuthenticationType, serviceEndpoint, TokenCache);
		serviceEndpoint.EndpointBehaviors.Add(new AdditionalHeaderBehaviour(_extraHeaders));
	}

	public EndpointAddress GetEndpointAddress()
	{
		return ManagementServerWcfHelper.GetEndpointAddress(AuthenticationType, ServerUrl);
	}

	public Binding GetBinding()
	{
		return ManagementServerWcfHelper.GetBinding(AuthenticationType, _namespaceName, Timeout);
	}

	protected void AddMessageHeader(string ns, string name, string content)
	{
		if (_extraHeaders.ContainsKey(name))
		{
			_extraHeaders[name] = BuildMessageHeader(ns, name, content);
		}
		else
		{
			_extraHeaders.Add(name, BuildMessageHeader(ns, name, content));
		}
	}

	private static MessageHeader BuildMessageHeader(string ns, string name, string content)
	{
		return new MessageHeader<string>(content).GetUntypedHeader(name, ns);
	}
}
