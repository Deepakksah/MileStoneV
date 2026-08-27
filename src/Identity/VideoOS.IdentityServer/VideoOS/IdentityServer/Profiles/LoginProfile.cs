using AutoMapper;
using Microsoft.AspNetCore.Identity;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Model.Requests;
using VideoOS.IdentityServer.Model.Responses;

namespace VideoOS.IdentityServer.Profiles;

public class LoginProfile : Profile
{
	public LoginProfile()
	{
		CreateMap<Login, LoginDto>();
		CreateMap<LoginForCreationModel, Login>();
		CreateMap<IdentityUserLogin<string>, LoginDto>();
	}
}
