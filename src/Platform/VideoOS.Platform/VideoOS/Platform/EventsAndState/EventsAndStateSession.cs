using System;
using VideoOS.Platform.Login;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Platform.EventsAndState;

public static class EventsAndStateSession
{
	private static Uri _alternativeUri;

	private static Func<Uri, IMipTokenCache, IEventReceiver, (IEventsAndStateSession, Action)> _sessionFactory;

	public static void SetEventsAndSessionFactory(Func<Uri, IMipTokenCache, IEventReceiver, (IEventsAndStateSession, Action)> sessionFactory)
	{
		SetEventsAndSessionFactory(sessionFactory, null);
	}

	public static void SetEventsAndSessionFactory(Func<Uri, IMipTokenCache, IEventReceiver, (IEventsAndStateSession, Action)> sessionFactory, Uri alternativeUri)
	{
		_alternativeUri = alternativeUri;
		_sessionFactory = sessionFactory;
	}

	public static IEventsAndStateSession Create(LoginSettings loginSettings, IEventReceiver eventReceiver)
	{
		if (!Version.TryParse(WellKnownInformation.GetProductVersion(loginSettings), out var result) || result < new Version("23.3"))
		{
			throw new InvalidOperationMIPException("Events and States are not supported for this product version");
		}
		Uri arg = _alternativeUri ?? GetApiGw(loginSettings);
		(IEventsAndStateSession, Action) tuple = _sessionFactory(arg, loginSettings.IdentityTokenCache, eventReceiver);
		var (result2, _) = tuple;
		tuple.Item2();
		return result2;
	}

	private static Uri GetApiGw(LoginSettings loginSettings)
	{
		Uri apiGatewayUri = WellKnownInformation.GetApiGatewayUri(loginSettings);
		if (apiGatewayUri == null)
		{
			throw new InvalidOperationMIPException("API Gateway not found");
		}
		return apiGatewayUri;
	}
}
