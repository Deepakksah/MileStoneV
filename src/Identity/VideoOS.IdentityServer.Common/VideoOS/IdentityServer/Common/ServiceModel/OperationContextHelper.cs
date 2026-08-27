using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace VideoOS.IdentityServer.Common.ServiceModel;

public class OperationContextHelper : IOperationContextHelper
{
	private static readonly ConcurrentDictionary<string, Claim[]> ClaimCache = new ConcurrentDictionary<string, Claim[]>();

	public string AuthorizationHeaderValue
	{
		get
		{
			ThrowIfOperationContextUnavailable();
			return ((HttpRequestMessageProperty)OperationContext.Current.IncomingMessageProperties[HttpRequestMessageProperty.Name]).Headers[HttpRequestHeader.Authorization];
		}
	}

	public Claim[] CollectCurrentActionClaims(string action, string resource)
	{
		ThrowIfOperationContextUnavailable();
		if (ClaimCache.TryGetValue(action, out var value))
		{
			return value;
		}
		DispatchOperation dispatchOperation = OperationContext.Current.EndpointDispatcher.DispatchRuntime.Operations.FirstOrDefault((DispatchOperation o) => o.Action == action);
		if (dispatchOperation == null)
		{
			throw new InvalidOperationException(FormattableString.Invariant(FormattableStringFactory.Create("Expected to find a {0} corresponding to the action {1} on {2}.", "DispatchOperation", action, resource)));
		}
		Type serviceType = OperationContext.Current.Host.Description.ServiceType;
		List<ClaimRequirementAttribute> list = (from o in serviceType.GetCustomAttributes(typeof(ClaimRequirementAttribute), inherit: true)
			select o as ClaimRequirementAttribute).ToList();
		IEnumerable<ClaimRequirementAttribute> collection = from o in serviceType.GetMethod(dispatchOperation.Name).GetCustomAttributes(typeof(ClaimRequirementAttribute), inherit: true)
			select o as ClaimRequirementAttribute;
		list.AddRange(collection);
		Claim[] array = list.Select((ClaimRequirementAttribute requiredScopesAttribute) => requiredScopesAttribute.RequiredClaim).ToArray();
		ClaimCache.TryAdd(action, array);
		return array;
	}

	private static void ThrowIfOperationContextUnavailable()
	{
		if (OperationContext.Current == null)
		{
			throw new InvalidOperationException(FormattableString.Invariant(FormattableStringFactory.Create("{0} is not available in the current context.", "OperationContext")));
		}
	}
}
