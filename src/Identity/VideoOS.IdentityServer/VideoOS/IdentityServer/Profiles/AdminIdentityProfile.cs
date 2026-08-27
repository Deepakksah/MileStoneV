using AutoMapper;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Model.Requests;
using VideoOS.IdentityServer.Model.Responses;

namespace VideoOS.IdentityServer.Profiles;

public class AdminIdentityProfile : Profile
{
	public AdminIdentityProfile()
	{
		CreateMap<AdminIdentityForCreationModel, AdminIdentity>();
		CreateMap<AdminIdentity, AdminIdentityDto>();
	}
}
