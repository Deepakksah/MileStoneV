using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace VideoOS.Platform.Util;

public class ItemSearch
{
	private struct SearchParams
	{
		public string searchText;

		public int maxCount;

		public int maxSeconds;

		public AsyncItemsSearchHandler asyncItemsHandler;

		public Control control;

		public List<Item> topItems;

		public object callerReference;
	}

	public static SearchResult Search(string tx, int max, int maxSeconds, out List<Item> found, List<Item> topItems)
	{
		found = new List<Item>();
		DateTime endTime = DateTime.UtcNow + TimeSpan.FromSeconds(maxSeconds);
		foreach (Item topItem in topItems)
		{
			if (topItem.Contains(tx))
			{
				found.Add(topItem);
			}
			SearchResult searchResult = SearchChildren(topItem, tx, max, endTime, ref found);
			if (searchResult != SearchResult.OK)
			{
				return searchResult;
			}
		}
		return SearchResult.OK;
	}

	private static SearchResult SearchChildren(Item item, string tx, int max, DateTime endTime, ref List<Item> found)
	{
		if (item.HasChildren != HasChildren.No)
		{
			List<Item> children = item.GetChildren();
			if (children != null)
			{
				foreach (Item item2 in children)
				{
					if (item2.Contains(tx))
					{
						found.Add(item2);
					}
					if (found.Count > max)
					{
						return SearchResult.CountExceeded;
					}
					if (DateTime.UtcNow > endTime)
					{
						return SearchResult.TimeExceeded;
					}
					try
					{
						SearchResult searchResult = SearchChildren(item2, tx, max, endTime, ref found);
						if (searchResult != SearchResult.OK)
						{
							return searchResult;
						}
					}
					catch (Exception ex)
					{
						EnvironmentManager.Instance.Log(error: false, "SearchChildren", ex.Message);
					}
				}
			}
		}
		return SearchResult.OK;
	}

	public static void SearchAsync(string tx, int max, int maxSeconds, AsyncItemsSearchHandler asyncItemsHandler, Control control, List<Item> topItems, object callerReference)
	{
		Thread thread = new Thread(SearchThread);
		SearchParams searchParams = new SearchParams
		{
			searchText = tx,
			maxCount = max,
			maxSeconds = maxSeconds,
			asyncItemsHandler = asyncItemsHandler,
			control = control,
			topItems = topItems,
			callerReference = callerReference
		};
		thread.Name = "MIP Async Search";
		thread.Start(searchParams);
	}

	private static void SearchThread(object parms)
	{
		SearchParams searchParams = (SearchParams)parms;
		SearchResult searchResult = SearchResult.OK;
		List<Item> found = new List<Item>();
		List<Item> topItems = searchParams.topItems;
		try
		{
			searchResult = Search(searchParams.searchText, searchParams.maxCount, searchParams.maxSeconds, out found, topItems);
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("ItemSearchASync", exception);
			searchResult = SearchResult.Error;
		}
		try
		{
			if (searchParams.control != null)
			{
				searchParams.control.BeginInvoke(searchParams.asyncItemsHandler, found, searchParams.callerReference, searchResult);
			}
			else
			{
				searchParams.asyncItemsHandler(found, searchParams.callerReference, searchResult);
			}
		}
		catch (Exception exception2)
		{
			EnvironmentManager.Instance.ExceptionHandler("SearchThread", exception2);
		}
	}
}
