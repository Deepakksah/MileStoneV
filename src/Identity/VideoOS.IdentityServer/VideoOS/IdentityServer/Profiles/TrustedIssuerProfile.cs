using AutoMapper;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Model.Requests;
using VideoOS.IdentityServer.Model.Responses;

namespace VideoOS.IdentityServer.Profiles;

public class TrustedIssuerProfile : Profile
{
	public TrustedIssuerProfile()
	{
		CreateMap<TrustedIssuerForCreationModel, TrustedIssuer>();
		CreateMap<TrustedIssuerForUpdateModel, TrustedIssuer>();
		CreateMap<TrustedIssuer, TrustedIssuerDto>();
	}
}
