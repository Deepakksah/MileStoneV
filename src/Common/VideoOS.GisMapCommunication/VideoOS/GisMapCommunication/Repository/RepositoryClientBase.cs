using System;
using System.Threading.Tasks;
using VideoOS.GisMapCommunication.Repository.Client;
using VideoOS.GisMapSharedData.DataContracts;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Repository;

public abstract class RepositoryClientBase : AbstractClient<GisRepositoryCommandClient, IGisRepositoryCommand>, IRepositoryClient, IClientBase, IDisposable
{
	protected override string Name => "GisRepositoryService";

	protected RepositoryClientBase(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	public async Task<AddRepositoryResp> AddRepositoryAsync(AddRepositoryParams parameters)
	{
		return await CallWithRetryAsync<AddRepositoryResp>((GisRepositoryCommandClient client) => async () => await client.AddRepositoryAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<UpdateRepositoryResp> UpdateRepositoryAsync(UpdateRepositoryParams parameters)
	{
		return await CallWithRetryAsync<UpdateRepositoryResp>((GisRepositoryCommandClient client) => async () => await client.UpdateRepositoryAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public async Task<GetRepositoryResp> GetRepositoryAsync(GetRepositoryParams parameters)
	{
		return await CallWithRetryAsync<GetRepositoryResp>((GisRepositoryCommandClient client) => async () => await client.GetRepositoryAsync(parameters).ConfigureAwait(continueOnCapturedContext: false));
	}

	public AddRepositoryResp AddRepository(AddRepositoryParams parameters)
	{
		return CallWithRetry<AddRepositoryResp>((GisRepositoryCommandClient client) => () => client.AddRepository(parameters));
	}

	public UpdateRepositoryResp UpdateRepository(UpdateRepositoryParams parameters)
	{
		return CallWithRetry<UpdateRepositoryResp>((GisRepositoryCommandClient client) => () => client.UpdateRepository(parameters));
	}

	public GetRepositoryResp GetRepository(GetRepositoryParams parameters)
	{
		return CallWithRetry<GetRepositoryResp>((GisRepositoryCommandClient client) => () => client.GetRepository(parameters));
	}
}
