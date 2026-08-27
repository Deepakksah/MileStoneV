using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.EnvironmentServices;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Platform;
using VideoOS.Platform.EventsAndState;
using VideoOS.Platform.OAuth;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public class EventsAndStateSession : IEventsAndStateSession, IDisposable
{
	private readonly IGatewayProvider _gatewayProvider;

	private readonly IMipTokenCache _tokenCache;

	private readonly IEventsAndStateClient _client;

	private IEventReceiver _eventReceiver;

	private readonly ILogger _logger;

	private readonly TimeSpan _reconnectDelay;

	private readonly ITimer _delayedTimer;

	private SemaphoreSlim _subscriptionLock = new SemaphoreSlim(1);

	private CancellationTokenSource _stoppingToken;

	private Dictionary<Guid, (Guid currentId, IEnumerable<SubscriptionRule> subscriptionRules)> _subscriptions = new Dictionary<Guid, (Guid, IEnumerable<SubscriptionRule>)>();

	private EventSniffer _eventSniffer;

	private int _inactiveTimeoutSeconds = 30;

	private ConnectionState _connectionState = (ConnectionState)(-1);

	public Task SessionTask { get; private set; }

	public bool SendStatefulEventsOnReconnect { get; set; } = true;

	public ConnectionState ConnectionState
	{
		get
		{
			return _connectionState;
		}
		private set
		{
			if (value == _connectionState)
			{
				return;
			}
			if (value != ConnectionState.Delayed)
			{
				_delayedTimer.StopTimer();
			}
			switch (value)
			{
			case ConnectionState.Connecting:
				if (_connectionState == ConnectionState.Online || _connectionState == ConnectionState.Delayed)
				{
					return;
				}
				break;
			case ConnectionState.Delayed:
				if (_connectionState != ConnectionState.Online)
				{
					return;
				}
				_delayedTimer.StartTimer(TimeSpan.FromSeconds(_inactiveTimeoutSeconds), Timeout.InfiniteTimeSpan);
				break;
			}
			_connectionState = value;
			ReportConnectionState(value);
		}
	}

	public EventsAndStateSession(IGatewayProvider gatewayProvider, IMipTokenCache tokenCache, IEventReceiver eventReceiver, ILogger logger)
		: this(gatewayProvider, tokenCache, new EventsAndStateClient(), eventReceiver, logger, TimeSpan.FromSeconds(1.0), new ThreadingTimerService())
	{
	}

	public EventsAndStateSession(IGatewayProvider gatewayProvider, IMipTokenCache tokenCache, IEventsAndStateClient client, IEventReceiver eventReceiver, ILogger logger, TimeSpan reconnectDelay, ITimer timerService)
	{
		_gatewayProvider = gatewayProvider;
		_tokenCache = tokenCache;
		_client = client;
		_eventReceiver = eventReceiver;
		_logger = logger;
		_reconnectDelay = reconnectDelay;
		_delayedTimer = timerService;
		_delayedTimer.Elapsed += DelayTimeout;
		AddEventReceiver();
	}

	public void Connect()
	{
		SessionTask = RunSessionAsync(_stoppingToken.Token);
	}

	private void AddEventReceiver()
	{
		_client.AddEventReceiver(_eventReceiver);
		_eventSniffer = new EventSniffer();
		_client.AddEventReceiver(_eventSniffer);
		_stoppingToken?.Dispose();
		_stoppingToken = new CancellationTokenSource();
	}

	private void RemoveEventReceiver()
	{
		if (_client.RemoveEventReceiver(_eventReceiver))
		{
			_client.RemoveEventReceiver(_eventSniffer);
			_eventSniffer = null;
			ConnectionState = ConnectionState.Offline;
			_eventReceiver = null;
			_stoppingToken.Cancel();
		}
	}

	public async Task<IEnumerable<State>> GetCurrentStateAsync(CancellationToken cancellationToken)
	{
		return await _client.GetCurrentStateAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task SendAllStatefulEventsAsync(CancellationToken cancellationToken)
	{
		await _client.SendAllStatefulEventsAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<Guid> AddSubscriptionAsync(IEnumerable<SubscriptionRule> subscriptionRules, CancellationToken cancellationToken)
	{
		await _subscriptionLock.WaitAsync(cancellationToken);
		try
		{
			Guid guid = await _client.AddSubscriptionAsync(subscriptionRules, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			_subscriptions[guid] = (guid, subscriptionRules);
			return guid;
		}
		finally
		{
			_subscriptionLock.Release();
		}
	}

	public async Task RemoveSubscriptionAsync(Guid subscriptionId, CancellationToken cancellationToken)
	{
		await _subscriptionLock.WaitAsync(cancellationToken);
		try
		{
			if (_subscriptions.TryGetValue(subscriptionId, out (Guid, IEnumerable<SubscriptionRule>) value))
			{
				await _client.RemoveSubscriptionAsync(value.Item1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				_subscriptions.Remove(subscriptionId);
			}
		}
		finally
		{
			_subscriptionLock.Release();
		}
	}

	private async Task RunSessionAsync(CancellationToken cancellationToken)
	{
		using (cancellationToken.Register(async delegate
		{
			await _client.DisconnectAsync();
		}))
		{
			_ = 14;
			try
			{
				ConnectionState = ConnectionState.Connecting;
				Guid sessionId = Guid.Empty;
				bool communicationErrorLogged = false;
				while (!cancellationToken.IsCancellationRequested)
				{
					try
					{
						await _subscriptionLock.WaitAsync(cancellationToken);
						try
						{
							await _client.ConnectAsync(_gatewayProvider.Gateway, _tokenCache.Token, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
							LoggerExtensions.LogInformation(_logger, "EventsAndState: Connected to server.", Array.Empty<object>());
							communicationErrorLogged = false;
							if (sessionId == Guid.Empty)
							{
								(Guid, int) tuple = await _client.CreateSession(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
								ConnectionState = ConnectionState.Online;
								(sessionId, _inactiveTimeoutSeconds) = tuple;
								LoggerExtensions.LogInformation(_logger, "EventsAndState: Session created ({SessionId}).", new object[1] { sessionId });
							}
							else
							{
								_ = (false, Guid.Empty, 30);
								(bool, Guid, int) tuple3;
								Guid newSessionId;
								if (_eventSniffer?.LastKnownGoodEventId.HasValue ?? false)
								{
									tuple3 = await _client.ResumeSession(sessionId, _eventSniffer.LastKnownGoodEventId.Value, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
									newSessionId = tuple3.Item2;
									_inactiveTimeoutSeconds = tuple3.Item3;
								}
								else
								{
									tuple3 = await _client.ResumeSession(sessionId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
									newSessionId = tuple3.Item2;
									_inactiveTimeoutSeconds = tuple3.Item3;
								}
								if (tuple3.Item1)
								{
									LoggerExtensions.LogInformation(_logger, "EventsAndState: Successfully resumed session ({SessionId}).", new object[1] { sessionId });
									ConnectionState = ConnectionState.Online;
								}
								else
								{
									LoggerExtensions.LogWarning(_logger, "EventsAndState: Session could not be resumed. New session created ({SessionId}).", new object[1] { sessionId });
									ConnectionState = ConnectionState.Offline;
									ConnectionState = ConnectionState.Online;
									Dictionary<Guid, (Guid, IEnumerable<SubscriptionRule>)> newSubscriptions = new Dictionary<Guid, (Guid, IEnumerable<SubscriptionRule>)>();
									foreach (KeyValuePair<Guid, (Guid, IEnumerable<SubscriptionRule>)> subscription in _subscriptions)
									{
										Guid item = await _client.AddSubscriptionAsync(subscription.Value.Item2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
										newSubscriptions[subscription.Key] = (item, subscription.Value.Item2);
									}
									if (SendStatefulEventsOnReconnect)
									{
										await _client.SendAllStatefulEventsAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
									}
									_subscriptions = newSubscriptions;
									sessionId = newSessionId;
								}
							}
						}
						finally
						{
							_subscriptionLock?.Release();
						}
						await _client.ConnectionTask.ConfigureAwait(continueOnCapturedContext: false);
						ConnectionState = ConnectionState.Delayed;
					}
					catch (TimeoutCommunicationMIPException ex)
					{
						ConnectionState = ConnectionState.Delayed;
						LoggerExtensions.LogError(_logger, "EventsAndState: Timeout communicating with the server: {ExceptionMessage}. Reconnecting.", new object[1] { ex.Message });
					}
					catch (CommunicationContentMIPException ex2)
					{
						communicationErrorLogged = await LogAndDelayReconnect("Error communicating with the server.", $"({ex2.Status}) ", communicationErrorLogged, ex2, cancellationToken);
					}
					catch (CommunicationMIPException ex3)
					{
						communicationErrorLogged = await LogAndDelayReconnect("Error communicating with the server:", string.Empty, communicationErrorLogged, ex3, cancellationToken);
					}
					catch (IdentityServerTokenException ex4)
					{
						communicationErrorLogged = await LogAndDelayReconnect("Failed to get a token from the authorization server. The token is unreadable:", string.Empty, communicationErrorLogged, ex4, cancellationToken);
					}
					catch (HttpRequestException)
					{
					}
					catch (UnauthorizedException ex6)
					{
						communicationErrorLogged = await LogAndDelayReconnect("Failed to get a token from the authorization server. Not authorized to get a token from the authorization server:", string.Empty, communicationErrorLogged, ex6, cancellationToken);
					}
					catch (IdpException ex7)
					{
						communicationErrorLogged = await LogAndDelayReconnect("Failed to get a token from the authorization server. The server returned an invalid HTTP status code. Server returned:", $"({ex7.StatusCode}) ", communicationErrorLogged, ex7, cancellationToken);
					}
					catch (OperationCanceledException)
					{
					}
					catch (Exception ex9)
					{
						LoggerExtensions.LogError(_logger, "EventsAndState: Fatal error communicating with the server: {Exception}", new object[1] { ex9 });
						ConnectionState = ConnectionState.Halted;
						throw;
					}
					if (cancellationToken.IsCancellationRequested)
					{
						LoggerExtensions.LogInformation(_logger, "EventsAndState: Disconnected from server.", Array.Empty<object>());
					}
					else
					{
						await _client.DisconnectAsync().ConfigureAwait(continueOnCapturedContext: false);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			ConnectionState = ConnectionState.Offline;
		}
	}

	private async Task<bool> LogAndDelayReconnect(string logText, string statusText, bool communicationErrorLogged, Exception ex, CancellationToken cancellationToken)
	{
		if (!communicationErrorLogged)
		{
			LoggerExtensions.LogError(_logger, "EventsAndState: {LogText} Server returned: {StatusText}{ExceptionMessage}. Reconnecting every {ReconnectDelay} second(s).", new object[4]
			{
				logText,
				statusText,
				ex.Message,
				$"{_reconnectDelay.TotalSeconds:0}"
			});
			communicationErrorLogged = true;
		}
		ConnectionState = ConnectionState.Delayed;
		await Task.Delay(_reconnectDelay, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		return communicationErrorLogged;
	}

	public void Dispose()
	{
		RemoveEventReceiver();
		_subscriptionLock?.Dispose();
		_subscriptionLock = null;
		_stoppingToken?.Cancel();
		_stoppingToken?.Dispose();
		_stoppingToken = null;
		GC.SuppressFinalize(this);
	}

	private void DelayTimeout(object sender, EventArgs e)
	{
		if (ConnectionState == ConnectionState.Delayed)
		{
			ConnectionState = ConnectionState.Offline;
		}
	}

	private void ReportConnectionState(ConnectionState value)
	{
		_eventReceiver?.OnConnectionStateChangedAsync(value);
	}
}
