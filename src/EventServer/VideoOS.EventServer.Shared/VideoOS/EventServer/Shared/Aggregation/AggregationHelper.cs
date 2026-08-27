using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoOS.EventServer.Shared.Aggregation;

public static class AggregationHelper
{
	public static Task<AggregationResult<S, T>> AggregateAsync<S, T>(IEnumerable<S> sources, Func<S, Task<T>> func)
	{
		if (sources == null)
		{
			throw new ArgumentNullException("sources");
		}
		List<Task<Result<S, T>>> list = new List<Task<Result<S, T>>>();
		foreach (S source in sources)
		{
			list.Add(ApplyAsync(source, func));
		}
		Task<Result<S, T>>[] tasks = Enumerable.ToArray(list);
		return Task<AggregationResult<S, T>>.Factory.ContinueWhenAll(tasks, delegate(Task<Result<S, T>>[] antecedents)
		{
			AggregationResult<S, T> aggregationResult = new AggregationResult<S, T>();
			foreach (Task<Result<S, T>> task in antecedents)
			{
				aggregationResult.Add(task.Result);
			}
			return aggregationResult;
		});
	}

	private static Task<Result<S, T>> ApplyAsync<S, T>(S source, Func<S, Task<T>> func)
	{
		return func(source).ContinueWith(delegate(Task<T> task)
		{
			Result<S, T> result = new Result<S, T>
			{
				Source = source
			};
			if (task.IsFaulted)
			{
				result.Exception = task.Exception.InnerException;
			}
			else
			{
				result.Value = task.Result;
			}
			return result;
		});
	}
}
