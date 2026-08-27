using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;

namespace VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;

public class RepositoryResponseBase<TRepository>
{
	public bool Success { get; set; }

	public string Message { get; set; }

	public string MessageDescription { get; set; }

	public RepositoryPaging PagingObject { get; set; }

	public TRepository Value { get; set; }
}
