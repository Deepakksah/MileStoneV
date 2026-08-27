using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public static class FilterProvider
{
	public static IEnumerable<Func<T, bool>> GetFilters<T>(CertificateGroup certificateGroup) where T : ICertificate
	{
		if (certificateGroup.FilterStrings.Any())
		{
			List<Func<T, bool>> list = new List<Func<T, bool>>();
			{
				foreach (string filterString in certificateGroup.FilterStrings)
				{
					if (filterString.Contains("ValidFrom"))
					{
						list.Add((T x) => x.ValidFrom.ToUniversalTime() <= DateTime.UtcNow);
					}
					if (filterString.Contains("ValidUntil"))
					{
						list.Add((T x) => x.ValidUntil.ToUniversalTime() > DateTime.UtcNow);
					}
					if (filterString.Contains("HasPrivateKey"))
					{
						list.Add((T x) => x.HasPrivateKey);
					}
					if (filterString.Contains("ServerAuthentication"))
					{
						list.Add((T x) => x.ServerAuthentication);
					}
				}
				return list;
			}
		}
		return new List<Func<T, bool>>
		{
			(T x) => x.ValidFrom.ToUniversalTime() <= DateTime.UtcNow,
			(T x) => x.ValidUntil.ToUniversalTime() > DateTime.UtcNow,
			(T x) => x.HasPrivateKey,
			(T x) => x.ServerAuthentication
		};
	}
}
