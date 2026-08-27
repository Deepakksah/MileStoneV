using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Data;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Model.Responses;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public sealed class ClientRegistrationHandler(SeededConfigurationDbContext dbContext, ClientConfigurationService configurationService) : IRequestHandler<RegisterClientCommand, Result<RegisterClientOperationResponse>>
{
	public async Task<Result<RegisterClientOperationResponse>> Handle(RegisterClientCommand command, CancellationToken cancellationToken)
	{
		Result<RegisterClientOperationResponse> result = ValidateBusinessRules(command);
		if (result.IsError)
		{
			return result;
		}
		Client client = await dbContext.Clients.IncludeMultiple((Client c) => c.ClientSecrets, (Client c) => c.AllowedGrantTypes, (Client c) => c.AllowedScopes, (Client c) => c.RedirectUris, (Client c) => c.PostLogoutRedirectUris).FirstOrDefaultAsync((Client c) => c.ClientId == command.ClientId, cancellationToken);
		ClientRegistrationContext context = configurationService.ConfigureClient(command, client);
		if (context.Operation == OperationType.Created)
		{
			dbContext.Clients.Add(context.Client);
		}
		else if (client != null)
		{
			dbContext.Entry(client).State = EntityState.Modified;
		}
		await dbContext.SaveChangesAsync(cancellationToken);
		RegisterClientResponse response = new RegisterClientResponse
		{
			Name = context.Client.ClientName,
			Scopes = context.Client.AllowedScopes.Select((ClientScope s) => s.Scope).ToArray(),
			Secret = context.GeneratedSecret
		};
		RegisterClientOperationResponse value = new RegisterClientOperationResponse(response, context.Operation);
		return Result<RegisterClientOperationResponse>.Success(value);
	}

	private static Result<RegisterClientOperationResponse> ValidateBusinessRules(RegisterClientCommand command)
	{
		if (command.ClientId == "winauthclient")
		{
			return Result<RegisterClientOperationResponse>.Failure(ClientRegistrationErrors.ClientIdReserved());
		}
		if (command.ClientGrantTypes.Length != command.ClientGrantTypes.Distinct().Count())
		{
			return Result<RegisterClientOperationResponse>.Failure(ClientRegistrationErrors.GrantTypesUnsupported());
		}
		if (!command.ClientGrantTypes.All(SupportedGrantTypes.IsSupported))
		{
			return Result<RegisterClientOperationResponse>.Failure(ClientRegistrationErrors.GrantTypesUnsupported());
		}
		if (Enumerable.Contains(command.ClientGrantTypes, "authorization_code"))
		{
			if (command.ClientRedirectUris == null || command.ClientRedirectUris.Length == 0)
			{
				return Result<RegisterClientOperationResponse>.Failure(ClientRegistrationErrors.RedirectUriRequired());
			}
			string text = command.ClientRedirectUris.FirstOrDefault((string uri) => !Uri.IsWellFormedUriString(uri, UriKind.Absolute));
			if (text != null)
			{
				return Result<RegisterClientOperationResponse>.Failure(ClientRegistrationErrors.RedirectUriMalformed(text));
			}
		}
		if (!Enumerable.Contains(command.ClientGrantTypes, "authorization_code"))
		{
			string[] clientRedirectUris = command.ClientRedirectUris;
			if (clientRedirectUris != null && clientRedirectUris.Length > 0)
			{
				return Result<RegisterClientOperationResponse>.Failure(ClientRegistrationErrors.RedirectUriNotAllowed());
			}
		}
		if (command.PostLogoutRedirectUris != null)
		{
			string text2 = command.PostLogoutRedirectUris.FirstOrDefault((string uri) => !Uri.IsWellFormedUriString(uri, UriKind.Absolute));
			if (text2 != null)
			{
				return Result<RegisterClientOperationResponse>.Failure(ClientRegistrationErrors.PostLogoutUriMalformed(text2));
			}
		}
		return Result<RegisterClientOperationResponse>.Success();
	}
}
