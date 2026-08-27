using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Messaging.Internal;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Platform.Common.MessageClient;

public class MessageClient : IManagedMessageClient, IMessageClient, IDisposable
{
	private readonly IUnmanagedMessageClient _messagesClient;

	private readonly LoginSettings _loginSettings;

	private readonly Func<LoginSettings, Uri> _apiGwUriProvider;

	private Guid? _sessionId;

	private SubscriptionStore _subscriptionStore;

	private bool _disposed;

	private ClientState _state = ClientState.Closed;

	private readonly object _stateLock = new object();

	public ClientState State
	{
		get
		{
			return _state;
		}
		private set
		{
			if (_state != value)
			{
				this.StateChanged?.Invoke(this, value);
				_state = value;
			}
		}
	}

	public event EventHandler<ClientState> StateChanged;

	public MessageClient(LoginSettings loginSettings)
		: this(loginSettings, new UnmanagedMessageClient(), WellKnownInformation.GetApiGatewayUri)
	{
	}

	public MessageClient(LoginSettings loginSettings, IUnmanagedMessageClient messagesClient, Func<LoginSettings, Uri> apiGwUriProvider)
	{
		_loginSettings = loginSettings ?? throw new ArgumentNullMIPException("loginSettings");
		_messagesClient = messagesClient ?? throw new ArgumentNullMIPException("messagesClient");
		_apiGwUriProvider = apiGwUriProvider ?? throw new ArgumentNullMIPException("apiGwUriProvider");
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		lock (_stateLock)
		{
			if (State != ClientState.Closed)
			{
				throw new InvalidOperationMIPException($"Cannot start when client is in state {State}.");
			}
			State = ClientState.Connecting;
		}
		_subscriptionStore = new SubscriptionStore();
		await DoConnectWithRetry(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task CloseAsync()
	{
		lock (_stateLock)
		{
			if (State != ClientState.Connected && State != ClientState.Connecting)
			{
				throw new InvalidOperationMIPException($"Cannot stop when client is in state {State}.");
			}
			State = ClientState.Closing;
		}
		UnRegisterMessageClientHandlers();
		try
		{
			await _messagesClient.DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception arg)
		{
			EnvironmentManager.Instance?.Log(error: true, "MessageClient", $"An exception was raised when disconnecting: {arg}");
		}
		finally
		{
			lock (_stateLock)
			{
				State = ClientState.Closed;
			}
		}
	}

	public async Task<Guid> SubscribeAsync<T>(string topic, MessageReceiverAsync receiver, CancellationToken cancellationToken)
	{
		if (receiver == null)
		{
			throw new ArgumentNullMIPException("receiver");
		}
		Subscription subscription = new Subscription
		{
			Id = Guid.NewGuid(),
			Topic = topic,
			ReceiverAsync = receiver,
			Type = typeof(T)
		};
		if (_subscriptionStore.GetSubscriptions(subscription.Topic).Count == 0)
		{
			await DoSubscribe(subscription, skipConnectedCheck: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		_subscriptionStore.StoreSubscription(subscription);
		return subscription.Id;
	}

	public async Task UnsubscribeAsync(Guid subscriptionId, CancellationToken cancellationToken)
	{
		if (State != ClientState.Connected)
		{
			throw new InvalidOperationMIPException($"Cannot unsubscribe when client is in state {State}.");
		}
		Subscription subscription = _subscriptionStore.GetSubscription(subscriptionId);
		if (subscription == null || _subscriptionStore.RemoveSubscription(subscription.Id))
		{
			return;
		}
		try
		{
			await _messagesClient.UnsubscribeAsync(subscription.Topic, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception)
		{
			_subscriptionStore.StoreSubscription(subscription);
			throw;
		}
	}

	public async Task PublishAsync(string topic, object data, CancellationToken cancellationToken)
	{
		if (State != ClientState.Connected)
		{
			throw new InvalidOperationMIPException($"Cannot publish when client is in state {State}.");
		}
		await _messagesClient.PublishAsync(topic, data, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void UnRegisterMessageClientHandlers()
	{
		_messagesClient.UnexpectedlyDisconnected -= OnUnexpectedDisconnectAsync;
		_messagesClient.SessionIdReceived -= OnSessionId;
		_messagesClient.MessageReceived -= OnNewMessageAsync;
	}

	private void RegisterMessageClientHandlers()
	{
		_messagesClient.UnexpectedlyDisconnected += OnUnexpectedDisconnectAsync;
		_messagesClient.SessionIdReceived += OnSessionId;
		_messagesClient.MessageReceived += OnNewMessageAsync;
	}

	private async void OnUnexpectedDisconnectAsync(object _, WebSocketCloseStatus status)
	{
		lock (_stateLock)
		{
			State = ClientState.ReconnectingWithSession;
		}
		await DoConnectWithRetry(CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
	}

	private void OnSessionId(object _, Guid sessionId)
	{
		if (sessionId != Guid.Empty)
		{
			_sessionId = sessionId;
		}
	}

	private async void OnNewMessageAsync(object _, NewMessageArgs newMessageArgs)
	{
		List<Subscription> subscriptions = _subscriptionStore.GetSubscriptions(newMessageArgs.Topic);
		List<Task> list = new List<Task>();
		foreach (Subscription item in subscriptions)
		{
			if (TryDeserialize(item, newMessageArgs.Data, out var deserializedData))
			{
				list.Add(TryExecute(item, deserializedData));
			}
		}
		await Task.WhenAll(list).ConfigureAwait(continueOnCapturedContext: false);
	}

	private bool TryDeserialize(Subscription subscription, JObject data, out object deserializedData)
	{
		//IL_0031: Expected O, but got Unknown
		if (data == null || subscription.Type == typeof(JObject))
		{
			deserializedData = data;
			return true;
		}
		try
		{
			deserializedData = ((JToken)data).ToObject(subscription.Type);
		}
		catch (JsonException ex)
		{
			JsonException ex2 = ex;
			EnvironmentManager.Instance?.Log(error: true, "MessageClient", $"Unable to desserialize message data from topic '{subscription.Topic}' to type '{subscription.Type}': {data} ||| {ex2}");
			deserializedData = null;
			return false;
		}
		return true;
	}

	private async Task TryExecute(Subscription subscription, object data)
	{
		try
		{
			MessageReceiverArgs message = new MessageReceiverArgs
			{
				Topic = subscription.Topic,
				Data = data
			};
			if (subscription.ReceiverAsync != null)
			{
				await subscription.ReceiverAsync(message).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch (Exception arg)
		{
			EnvironmentManager.Instance?.Log(error: true, "MessageClient", $"Error while executing {subscription.ReceiverAsync} for subscription {subscription.Id}: {arg}");
		}
	}

	private async Task DoConnectWithRetry(CancellationToken cancellationToken)
	{
		try
		{
			UnRegisterMessageClientHandlers();
			RegisterMessageClientHandlers();
			await WithRetryAsync(async delegate
			{
				await DoConnect(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			lock (_stateLock)
			{
				State = ClientState.Connected;
			}
		}
		catch (Exception)
		{
			lock (_stateLock)
			{
				State = ClientState.Closed;
			}
		}
	}

	private async Task DoConnect(CancellationToken cancellationToken)
	{
		Uri uri = _apiGwUriProvider(_loginSettings);
		if (uri == null)
		{
			throw new InvalidOperationMIPException("Cannot attempt connection if GetApiGatewayUri is null.");
		}
		Uri uri2 = new UriBuilder(uri)
		{
			Scheme = ((uri.Scheme == Uri.UriSchemeHttp) ? "ws" : "wss"),
			Path = "/api/ws/messages/v1"
		}.Uri;
		EnvironmentManager.Instance?.Log(error: false, "MessageClient", $"Connecting to messaging service at: {uri2}");
		if (_loginSettings.IdentityTokenCache == null)
		{
			EnvironmentManager.Instance?.Log(error: true, "MessageClient", "IdentityTokenCache is null");
			throw new InvalidOperationMIPException("IdentityTokenCache is null");
		}
		if (_loginSettings.IdentityTokenCache.Token == null)
		{
			EnvironmentManager.Instance?.Log(error: true, "MessageClient", "IdentityTokenCache.Token is null");
			throw new InvalidOperationMIPException("IdentityTokenCache.Token is null");
		}
		try
		{
			await _messagesClient.ConnectAsync(uri2, _loginSettings.IdentityTokenCache.Token, _sessionId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (CommunicationMIPException)
		{
			_sessionId = null;
			lock (_stateLock)
			{
				State = ClientState.ReconnectingWithoutSession;
			}
			throw;
		}
		catch (Exception)
		{
			throw;
		}
		if (_sessionId.HasValue)
		{
			return;
		}
		try
		{
			await Task.WhenAll(from subscription in _subscriptionStore.GetAllSubscriptions()
				select DoSubscribe(subscription, skipConnectedCheck: true, cancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private async Task DoSubscribe(Subscription subscription, bool skipConnectedCheck, CancellationToken cancellationToken)
	{
		if (!skipConnectedCheck && State != ClientState.Connected)
		{
			throw new InvalidOperationMIPException("Call Start before attempting to subscribe.");
		}
		await _messagesClient.SubscribeAsync(subscription.Topic, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private static async Task WithRetryAsync(Func<CancellationToken, Task> call, CancellationToken cancellationToken, int? maxRetries = null)
	{
		int currentRetry = 0;
		double delayMs = 500.0;
		while (true)
		{
			try
			{
				await call(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				break;
			}
			catch (Exception arg)
			{
				EnvironmentManager.Instance?.Log(error: true, "MessageClient", $"An exception was raised when connecting (retry: {currentRetry}: {arg}");
				currentRetry++;
			}
			if (currentRetry >= maxRetries)
			{
				EnvironmentManager.Instance?.Log(error: true, "MessageClient", $"Stopped trying after {currentRetry} retries");
			}
			await Task.Delay((int)delayMs, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			delayMs = Math.Min(delayMs * 1.5, 10000.0);
		}
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			_subscriptionStore?.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
