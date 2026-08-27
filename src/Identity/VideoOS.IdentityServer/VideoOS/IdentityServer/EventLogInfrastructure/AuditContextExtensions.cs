using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.EventLogInfrastructure;

public static class AuditContextExtensions
{
	private const string IsSystemContextKey = "IsSystemContext";

	public static async Task<T> WithSystemAuditContext<T>(this HttpContext context, Func<Task<T>> action)
	{
		try
		{
			context.EnableSystemAuditContext();
			return await action();
		}
		finally
		{
			context.DisableSystemAuditContext();
		}
	}

	public static string GetAuditUserIdentification(this HttpContext context)
	{
		if (context.IsSystemAuditContext())
		{
			return "";
		}
		if (context != null && context.User.Identity?.IsAuthenticated == true)
		{
			if (!string.IsNullOrEmpty(context.User.Identity.Name))
			{
				return context.User.Identity.Name;
			}
			if (!context.User.Claims.Any((Claim c) => c.Type == "client_id"))
			{
				throw new InvalidOperationException("No authenticated user or client");
			}
		}
		return "";
	}

	public static string GetClientId(this HttpContext context)
	{
		if (!context.IsSystemAuditContext())
		{
			return context?.User.Claims.FirstOrDefault((Claim c) => c.Type == "client_id")?.Value;
		}
		return null;
	}

	private static void EnableSystemAuditContext(this HttpContext context)
	{
		ArgumentNullException.ThrowIfNull(context, "context");
		context.Items["IsSystemContext"] = true;
	}

	private static void DisableSystemAuditContext(this HttpContext context)
	{
		ArgumentNullException.ThrowIfNull(context, "context");
		context.Items["IsSystemContext"] = false;
	}

	private static bool IsSystemAuditContext(this HttpContext context)
	{
		object obj = context?.Items["IsSystemContext"];
		if (obj is bool)
		{
			return (bool)obj;
		}
		return false;
	}
}
