#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using C5;

namespace VideoOS.UI.Common.WPF.Timeline.InterfaceData;

public static class TimeIntervalExtension
{
	private static bool _warningGiven;

	public static ISortedEnumerable<T> GetAllIntersecting<T>(this IEnumerable<T> collection, DateTime now, Func<T, T> cloneFunc) where T : TimeInterval, new()
	{
		return collection.GetAllIntersectingImpl(now, cloneFunc).ToSortedEnumerable();
	}

	private static IEnumerable<T> GetAllIntersectingImpl<T>(this IEnumerable<T> collection, DateTime now, Func<T, T> cloneFunc) where T : TimeInterval, new()
	{
		foreach (T item in collection)
		{
			if (!(item.BeginTime >= now))
			{
				if (item.EndTime <= now)
				{
					yield return item;
					continue;
				}
				T val = cloneFunc(item);
				val.TimeSpan = TimeSpan.MaxValue;
				yield return val;
				continue;
			}
			yield break;
		}
	}

	public static ISortedEnumerable<T> GetAllIntersecting<T>(this IEnumerable<T> collection, TimeInterval interval) where T : TimeInterval, new()
	{
		return collection.GetAllIntersecting(interval.BeginTime, interval.EndTime);
	}

	public static ISortedEnumerable<T> GetAllIntersecting<T>(this IEnumerable<T> collection, TimeInterval interval, bool beginInclusive, bool endInclusive) where T : TimeInterval, new()
	{
		return collection.GetAllIntersecting(interval.BeginTime, interval.EndTime, beginInclusive, endInclusive);
	}

	public static ISortedEnumerable<T> GetAllIntersecting<T>(this IEnumerable<T> collection, DateTime beginTime, TimeSpan timeSpan) where T : TimeInterval, new()
	{
		return collection.GetAllIntersecting(beginTime, beginTime.Add(timeSpan));
	}

	public static ISortedEnumerable<T> GetAllIntersecting<T>(this IEnumerable<T> collection, DateTime beginTime, DateTime endTime) where T : TimeInterval, new()
	{
		return collection.GetAllIntersecting(beginTime, endTime, beginInclusive: false, endInclusive: false);
	}

	public static ISortedEnumerable<T> GetAllIntersecting<T>(this IEnumerable<T> collection, DateTime beginTime, DateTime endTime, bool beginInclusive, bool endInclusive) where T : TimeInterval, new()
	{
		ISorted<T> val = collection as ISorted<T>;
		if (val == null && collection is SortedEnumerable<T> sortedEnumerable)
		{
			val = sortedEnumerable.WrappedEnumerable as ISorted<T>;
		}
		if (val != null)
		{
			return DoGetAllIntersectingFast<T>(val, beginTime, endTime, beginInclusive, endInclusive).ToSortedEnumerable();
		}
		return DoGetAllIntersectingSlow(collection, beginTime, endTime, beginInclusive, endInclusive).ToSortedEnumerable();
	}

	private static IEnumerable<T> DoGetAllIntersectingFast<T>(ISorted<T> array, DateTime beginTime, DateTime endTime, bool beginInclusive, bool endInclusive) where T : TimeInterval, new()
	{
		T item = default(T);
		if (!array.TryWeakPredecessor(new T
		{
			BeginTime = beginTime
		}, ref item))
		{
			item = ((IEnumerable<T>)array).FirstOrDefault();
			if (item == null)
			{
				yield break;
			}
		}
		T val = default(T);
		while (item.BeginTime < endTime)
		{
			if (item.IntersectsWith(beginTime, endTime, beginInclusive, endInclusive))
			{
				yield return item;
			}
			if (!array.TrySuccessor(item, ref val))
			{
				break;
			}
			item = val;
		}
	}

	private static IEnumerable<T> DoGetAllIntersectingSlow<T>(IEnumerable<T> array, DateTime beginTime, DateTime endTime, bool beginInclusive, bool endInclusive) where T : TimeInterval, new()
	{
		if (array == null)
		{
			yield break;
		}
		if (!_warningGiven)
		{
			Trace.WriteLine("Warning: GetAllIntersecting is executing using IEnumerable. Investigate upgrading the collection to C5.ISorted.");
			_warningGiven = true;
		}
		foreach (T item in array)
		{
			if (item.IntersectsWith(beginTime, endTime, beginInclusive, endInclusive))
			{
				yield return item;
			}
		}
	}

	public static IEnumerable<T2> Convert<T1, T2>(this IEnumerable<T1> collection, Func<T1, T2> createNewItemFunction)
	{
		foreach (T1 item in collection)
		{
			yield return createNewItemFunction(item);
		}
	}

	public static ISortedEnumerable<T2> Convert<T1, T2>(this ISortedEnumerable<T1> collection, Func<T1, T2> createNewItemFunction)
	{
		return collection.Convert(createNewItemFunction).ToSortedEnumerable();
	}

	public static T IntersectWith<T>(this T item, TimeInterval timeInterval, Func<T, T> cloneFunc) where T : TimeInterval
	{
		T val = cloneFunc(item);
		val.BeginTime = new DateTime(Math.Max(item.BeginTime.Ticks, timeInterval.BeginTime.Ticks), DateTimeKind.Utc);
		val.EndTime = new DateTime(Math.Max(Math.Max(item.BeginTime.Ticks, timeInterval.BeginTime.Ticks), Math.Min(item.EndTime.Ticks, timeInterval.EndTime.Ticks)), DateTimeKind.Utc);
		return val;
	}
}
