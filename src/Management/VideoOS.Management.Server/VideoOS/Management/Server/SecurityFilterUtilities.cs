using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public class SecurityFilterUtilities
{
	public static IEnumerable<T> FilterItems<T>(IGeminiRequestContext requestContext, Guid securityNamespaceId, string securityAction, IEnumerable<(T instance, string id)> items)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (securityAction == null)
		{
			throw new ArgumentNullException("securityAction");
		}
		if (items == null)
		{
			throw new ArgumentNullException("items");
		}
		(T, string)[] array = items.ToArray();
		HashSet<string> invalidIds = new HashSet<string>(array.Length, StringComparer.Ordinal);
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		Parallel.ForEach(items, delegate((T instance, string id) item)
		{
			try
			{
				if (!securityManagerService.HasObjectPermission(requestContext, securityNamespaceId, securityAction, item.id))
				{
					lock (invalidIds)
					{
						invalidIds.Add(item.id);
						return;
					}
				}
			}
			catch (SecurityObjectDoesNotExistException)
			{
				lock (invalidIds)
				{
					invalidIds.Add(item.id);
				}
			}
		});
		return (from d in array
			where !invalidIds.Contains(d.id)
			select d.instance).ToArray();
	}

	public static IEnumerable<T> FilterItems<T>(IGeminiRequestContext requestContext, Guid securityNamespaceId, string securityAction, IEnumerable<(T instance, string id, string securityObjectId)> items)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (securityAction == null)
		{
			throw new ArgumentNullException("securityAction");
		}
		if (items == null)
		{
			throw new ArgumentNullException("items");
		}
		(T, string, string)[] array = items.ToArray();
		HashSet<string> invalidIds = new HashSet<string>(array.Length, StringComparer.Ordinal);
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		Parallel.ForEach(items, delegate((T instance, string id, string securityObjectId) item)
		{
			try
			{
				if (!securityManagerService.HasObjectPermission(requestContext, securityNamespaceId, securityAction, item.securityObjectId))
				{
					lock (invalidIds)
					{
						invalidIds.Add(item.id);
						return;
					}
				}
			}
			catch (SecurityObjectDoesNotExistException)
			{
				lock (invalidIds)
				{
					invalidIds.Add(item.id);
				}
			}
		});
		return (from d in array
			where !invalidIds.Contains(d.id)
			select d.instance).ToArray();
	}
}
