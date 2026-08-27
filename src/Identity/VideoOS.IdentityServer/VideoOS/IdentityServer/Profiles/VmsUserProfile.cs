using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Model.Responses;

namespace VideoOS.IdentityServer.Profiles;

public class VmsUserProfile : Profile
{
	public VmsUserProfile()
	{
		CreateMap<VmsUser, UserDto>();
		CreateMap<VmsUserClaim, UserClaimDto>().ForMember((UserClaimDto dto) => dto.LoginProviderId, delegate(IMemberConfigurationExpression<VmsUserClaim, UserClaimDto, string> opt)
		{
			opt.MapFrom((VmsUserClaim claim) => claim.LoginProvider);
		});
		CreateMap<IdentityUserLogin<string>, UserLoginDto>().ForMember((UserLoginDto dto) => dto.LoginProviderId, delegate(IMemberConfigurationExpression<IdentityUserLogin<string>, UserLoginDto, string> opt)
		{
			opt.MapFrom((IdentityUserLogin<string> login) => login.LoginProvider);
		});
	}
}
