using Microsoft.AspNetCore.Routing;

namespace VideoOS.IdentityServer.Endpoints;

public interface IEndpoint
{
	void MapEndpoint(IEndpointRouteBuilder app);
}
