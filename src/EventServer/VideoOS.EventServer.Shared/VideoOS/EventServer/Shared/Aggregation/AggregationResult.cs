using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.EventServer.Shared.Aggregation;

public class AggregationResult<S, T>
{
	private Dictionary<S, T> properResults = new Dictionary<S, T>();

	private Dictionary<S, Exception> exceptionResults = new Dictionary<S, Exception>();

	public IEnumerable<S> ProperResultSources => properResults.Keys;

	public IEnumerable<KeyValuePair<S, T>> ProperResults => properResults.AsEnumerable();

	public IEnumerable<S> ExceptionResultSources => exceptionResults.Keys;

	public IEnumerable<KeyValuePair<S, Exception>> ExceptionResults => exceptionResults.AsEnumerable();

	public void Add(Result<S, T> result)
	{
		if (result == null)
		{
			throw new ArgumentNullException("result");
		}
		if (result.HasValue)
		{
			SetResult(result.Source, result.Value);
		}
		else
		{
			SetException(result.Source, result.Exception);
		}
	}

	public void SetResult(S source, T result)
	{
		if (exceptionResults.ContainsKey(source))
		{
			throw new ArgumentException("Cannot register result and exception for the same source. Check if the same source has been queried more than once.");
		}
		if (properResults.ContainsKey(source))
		{
			throw new ArgumentException("Cannot register multiple results for the same source. Check if the same source has been queried more than once.");
		}
		properResults[source] = result;
	}

	public T GetResult(S source)
	{
		properResults.TryGetValue(source, out var value);
		return value;
	}

	public bool HasResult(S source)
	{
		return properResults.ContainsKey(source);
	}

	public void SetException(S source, Exception error)
	{
		if (properResults.ContainsKey(source))
		{
			throw new ArgumentException("Cannot register result and exception for the same source. Check if the same source has been queried more than once.");
		}
		if (exceptionResults.ContainsKey(source))
		{
			throw new ArgumentException("Cannot register multiple exceptions for the same source. Check if the same source has been queried more than once.");
		}
		exceptionResults[source] = error;
	}

	public Exception GetException(S source)
	{
		exceptionResults.TryGetValue(source, out var value);
		return value;
	}

	public bool HasException(S source)
	{
		return exceptionResults.ContainsKey(source);
	}
}
