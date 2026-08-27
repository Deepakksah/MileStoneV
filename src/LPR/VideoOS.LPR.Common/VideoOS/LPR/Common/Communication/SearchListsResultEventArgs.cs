using System;
using System.Threading.Tasks;

namespace VideoOS.LPR.Common.Communication;

public class SearchListsResultEventArgs<T> : EventArgs where T : class
{
	public T Result { get; }

	public bool IsFaulted { get; }

	public Guid SiteId { get; }

	public SearchListsResultEventArgs(Task<T> resultTask, Guid siteId)
	{
		IsFaulted = resultTask.IsFaulted;
		Result = ((!resultTask.IsFaulted) ? resultTask.Result : null);
		SiteId = siteId;
	}
}
