using System.Linq;
using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using VideoOS.IdentityServer.Model.Responses;

namespace VideoOS.IdentityServer.Profiles;

public class ClientProfile : Profile
{
	public ClientProfile()
	{
		CreateMap<Client, ClientDto>().ForMember((ClientDto dm) => dm.ClientScopes, delegate(IMemberConfigurationExpression<Client, ClientDto, string[]> sm)
		{
			sm.MapFrom((Client model) => model.AllowedScopes.Select((ClientScope scope) => scope.Scope).ToArray());
		}).ForMember((ClientDto dm) => dm.ClientRedirectUris, delegate(IMemberConfigurationExpression<Client, ClientDto, string[]> sm)
		{
			sm.MapFrom((Client model) => model.RedirectUris.Select((ClientRedirectUri uri) => uri.RedirectUri).ToArray());
		});
	}
}
