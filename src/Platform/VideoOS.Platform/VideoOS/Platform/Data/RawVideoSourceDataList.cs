using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.Data;

public class RawVideoSourceDataList : IDisposable
{
	public List<RawVideoSourceData> List;

	public virtual byte[] Signature { get; set; }

	public IDisposable ObjectToDispose { get; set; }

	public void Dispose(bool native)
	{
		if (ObjectToDispose != null)
		{
			ObjectToDispose.Dispose();
			ObjectToDispose = null;
		}
	}

	public void Dispose()
	{
		Dispose(native: true);
	}

	public static implicit operator RawVideoSourceData(RawVideoSourceDataList dataList)
	{
		if (dataList != null && dataList.List != null && dataList.List.Any())
		{
			return dataList.List.Last();
		}
		return null;
	}

	public RawVideoSourceData ToRawVideoSourceData()
	{
		return this;
	}
}
