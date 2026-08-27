using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class TransientExceptionDetector : ITransientExceptionDetector
{
	private static readonly HashSet<int> TransientSqlErrorNumbers = new HashSet<int>
	{
		49920, 49919, 49918, 41839, 41325, 41305, 41302, 41301, 40613, 40501,
		40197, 10936, 10929, 10928, 10060, 10054, 10053, 1205, 233, 121,
		64, 20
	};

	private readonly HashSet<Type> _transientExceptionTypes = new HashSet<Type>
	{
		typeof(TransientDomainException),
		typeof(TimeoutException)
	};

	public bool IsTransient(Exception ex)
	{
		if (_transientExceptionTypes.Contains(ex.GetType()))
		{
			return true;
		}
		if (ex is SqlException ex2)
		{
			return ex2.Errors.Cast<SqlError>().Any((SqlError err) => TransientSqlErrorNumbers.Contains(err.Number));
		}
		return false;
	}
}
