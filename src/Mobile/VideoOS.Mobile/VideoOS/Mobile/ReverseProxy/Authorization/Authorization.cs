using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace VideoOS.Mobile.ReverseProxy.Authorization;

public class Authorization
{
	private List<IAuthorizationProvider> _providers = new List<IAuthorizationProvider>();

	public HttpStatusCode GetProvider(HttpListenerRequest request, out IAuthorizationProvider provider)
	{
		if (request == null)
		{
			throw new NullReferenceException("Request is null");
		}
		provider = null;
		if (!_providers.Any())
		{
			return HttpStatusCode.OK;
		}
		List<IAuthorizationProvider> list = _providers.Where((IAuthorizationProvider auth) => auth.IsPresent(request)).ToList();
		if (!list.Any())
		{
			return HttpStatusCode.Unauthorized;
		}
		if (list.Count > 1)
		{
			return HttpStatusCode.BadRequest;
		}
		provider = list.First();
		return HttpStatusCode.OK;
	}

	public void Add(IAuthorizationProvider provider)
	{
		if (provider == null)
		{
			throw new NullReferenceException("Provider is null");
		}
		_providers.Add(provider);
	}
}
