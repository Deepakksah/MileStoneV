using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public abstract class DataSource
{
	public virtual Item Item { get; set; }

	protected DataSource(Item item)
	{
		Item = item;
	}

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public virtual List<DataType> GetTypes()
	{
		return new List<DataType>();
	}

	public virtual List<object> GetData(DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter)
	{
		return new List<object>();
	}

	public virtual List<object> GetData(DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, Guid typeOfData)
	{
		return GetData(dateTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
	}

	public virtual List<object> GetData(DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, Guid typeOfData, bool throwExceptions)
	{
		return GetData(dateTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
	}

	public virtual void GetDataAsync(object asyncState, DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, AsyncCallbackHandler resultCallbackHandler)
	{
		resultCallbackHandler(new List<object>(), asyncState);
	}

	public virtual void GetDataAsync(object asyncState, DateTime dateTime, TimeSpan maxTimeBefore, int maxCountBefore, TimeSpan maxTimeAfter, int maxCountAfter, Guid typeOfData, AsyncCallbackHandler resultCallbackHandler)
	{
		GetDataAsync(asyncState, dateTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter, resultCallbackHandler);
	}

	public virtual string Command(object objectData, string command, string[] args)
	{
		return "";
	}
}
