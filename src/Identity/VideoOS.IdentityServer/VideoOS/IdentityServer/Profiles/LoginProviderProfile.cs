using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Model.Requests;
using VideoOS.IdentityServer.Model.Responses;

namespace VideoOS.IdentityServer.Profiles;

public class LoginProviderProfile : Profile
{
	public LoginProviderProfile()
	{
		CreateMap<LoginProviderForCreationModel, LoginProvider>().ForMember((LoginProvider dm) => dm.Scopes, delegate(IMemberConfigurationExpression<LoginProviderForCreationModel, LoginProvider, string> sm)
		{
			sm.MapFrom((LoginProviderForCreationModel model) => JsonConvert.SerializeObject(model.Scopes));
		}).ForMember((LoginProvider dm) => dm.ScimClients, delegate(IMemberConfigurationExpression<LoginProviderForCreationModel, LoginProvider, ICollection<LoginProviderClient>> sm)
		{
			sm.MapFrom((LoginProviderForCreationModel model) => model.ScimClientIds.Select((string f) => new LoginProviderClient
			{
				ClientId = f,
				LoginProviderId = model.Id
			}));
		});
		CreateMap<LoginProvider, LoginProviderForUpdateModel>().ForMember((LoginProviderForUpdateModel dm) => dm.Scopes, delegate(IMemberConfigurationExpression<LoginProvider, LoginProviderForUpdateModel, string[]> sm)
		{
			sm.MapFrom((LoginProvider model) => JsonConvert.DeserializeObject<string[]>(model.Scopes));
		}).ForMember((LoginProviderForUpdateModel dm) => dm.ScimClientIds, delegate(IMemberConfigurationExpression<LoginProvider, LoginProviderForUpdateModel, string[]> sm)
		{
			sm.MapFrom((LoginProvider model) => (model.ScimClients == null) ? Enumerable.Empty<string>().ToArray() : model.ScimClients.Select((LoginProviderClient f) => f.ClientId).ToArray());
		});
		CreateMap<LoginProviderForUpdateModel, LoginProvider>().ForMember((LoginProvider dm) => dm.Scopes, delegate(IMemberConfigurationExpression<LoginProviderForUpdateModel, LoginProvider, string> sm)
		{
			sm.MapFrom((LoginProviderForUpdateModel model) => JsonConvert.SerializeObject(model.Scopes));
		}).ForMember((LoginProvider dm) => dm.ScimClients, delegate(IMemberConfigurationExpression<LoginProviderForUpdateModel, LoginProvider, ICollection<LoginProviderClient>> opt)
		{
			opt.MapFrom((LoginProviderForUpdateModel src, LoginProvider dest, ICollection<LoginProviderClient> destMember, ResolutionContext context) => new List<LoginProviderClient>((src.ScimClientIds == null) ? Enumerable.Empty<LoginProviderClient>().ToArray() : src.ScimClientIds.Select((string f) => new LoginProviderClient
			{
				LoginProviderId = new Guid(context.Items["ProviderId"].ToString()),
				ClientId = f
			}).ToArray()));
		}).ForMember((LoginProvider x) => x.Id, delegate(IMemberConfigurationExpression<LoginProviderForUpdateModel, LoginProvider, Guid> opt)
		{
			opt.Ignore();
		});
		CreateMap<LoginProvider, LoginProviderDto>().ForMember((LoginProviderDto dm) => dm.Scopes, delegate(IMemberConfigurationExpression<LoginProvider, LoginProviderDto, string[]> sm)
		{
			sm.MapFrom((LoginProvider model) => JsonConvert.DeserializeObject<string[]>(model.Scopes));
		}).ForMember((LoginProviderDto dm) => dm.ScimClientIds, delegate(IMemberConfigurationExpression<LoginProvider, LoginProviderDto, string[]> sm)
		{
			sm.MapFrom((LoginProvider model) => model.ScimClients.Select((LoginProviderClient f) => f.ClientId).ToArray());
		});
	}
}
