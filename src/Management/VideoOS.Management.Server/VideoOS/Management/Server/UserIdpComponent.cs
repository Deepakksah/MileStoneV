using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Model.Responses;
using VideoOS.Management.Common;
using VideoOS.Management.Server.DataComponents.Idp;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class UserIdpComponent : IdpComponentBase, IUserIdpComponent, IDisposable
{
	public UserIdpComponent(IdpClientService idpClientService, ITokenCacheService tokenCacheService)
		: base(idpClientService, tokenCacheService)
	{
	}

	public BasicUser CreateVmsUser(CreateUserModel model)
	{
		return BasicUser.Map(CallIdp((IIdpAdministrationClient client, string token) => client.CreateUser(token, model), (IdpException IdpException) => HandleCreateException(IdpException, model)));
	}

	public BasicUser UpdateBasicUser(string id, UpdateUserModel model)
	{
		return BasicUser.Map(CallIdp((IIdpAdministrationClient client, string token) => client.UpdateUser(token, id, model), (IdpException ex) => HandleUpdateException(ex)));
	}

	public BasicUser GetVmsUserById(string id)
	{
		return BasicUser.Map(CallIdp((IIdpAdministrationClient client, string token) => client.GetUser(token, id), HandleGetByIdException));
	}

	public BasicUser[] GetAllVmsUsers()
	{
		return (from um in CallIdp((IIdpAdministrationClient client, string token) => client.GetUsers(token), HandleGetAllException)
			select BasicUser.Map(um)).ToArray();
	}

	public BasicUser GetVmsUserByName(string name)
	{
		return BasicUser.Map(CallIdp((IIdpAdministrationClient client, string token) => client.GetUserByName(token, name), HandleGetAllException)) ?? throw new BasicUserNotExistException();
	}

	public BasicUser[] GetVmsUserCollection(ICollection<string> ids)
	{
		return (from um in CallIdp((IIdpAdministrationClient client, string token) => client.GetUserCollection(token, ids), HandleGetAllException)
			select BasicUser.Map(um)).ToArray();
	}

	public BasicUser[] GetVmsUsersModifiedAfter(DateTime afterDateTime)
	{
		return (from um in CallIdp((IIdpAdministrationClient client, string token) => client.GetUsersAfterModifiedDate(token, afterDateTime), HandleGetBySearchException)
			select BasicUser.Map(um)).ToArray();
	}

	public GetLoginModel GetLogin(string provider, string key)
	{
		return CallIdp((IIdpAdministrationClient client, string token) => client.GetLogin(token, provider, key), HandleGetByLoginException);
	}

	public GetLoginModel CreateLogin(CreateLoginModel model)
	{
		return CallIdp((IIdpAdministrationClient client, string token) => client.CreateLogin(token, model), HandleCreateLoginException);
	}

	public void Dispose()
	{
	}

	private static Exception HandleCreateLoginException(IdpException ex)
	{
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}

	private static Exception HandleGetAllException(IdpException ex)
	{
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}

	private static Exception HandleGetBySearchException(IdpException ex)
	{
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}

	private Exception HandleGetByIdException(Exception ex)
	{
		if (ex is NotFoundException)
		{
			return new BasicUserNotExistException();
		}
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}

	private static Exception HandleGetByLoginException(IdpException ex)
	{
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}

	private static Exception HandleCreateException(IdpException ex, CreateUserModel model)
	{
		if (ex is AlreadyExistsException)
		{
			return new BasicUserCreationException(model.UserName);
		}
		if (ex is PasswordNotComplexEnoughException)
		{
			return new BasicUserPasswordException();
		}
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}

	private static Exception HandleUpdateException(IdpException ex)
	{
		if (ex is NotFoundException)
		{
			return new BasicUserUpdateException();
		}
		if (ex is PasswordNotComplexEnoughException)
		{
			return new BasicUserPasswordException();
		}
		if (ex is PasswordSameAsOldException)
		{
			return new BasicUserPasswordSameAsOldException();
		}
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}
}
