using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.IdentityServer.Common.Model.Requests;
using VideoOS.IdentityServer.Common.Model.Responses;
using VideoOS.Management.Common;
using VideoOS.Management.Server.DataComponents.Idp;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.DataComponents.IDP;

public class TrustedIssuerIdpComponent : IdpComponentBase, ITrustedIssuerIdpComponent, IDisposable
{
	public TrustedIssuerIdpComponent(IdpClientService idpClientService, ITokenCacheService tokenCacheService)
		: base(idpClientService, tokenCacheService)
	{
	}

	public TrustedIssuerData CreateTrustedIssuer(string issuer, Uri address)
	{
		TrustedIssuerCreateModel model = new TrustedIssuerCreateModel
		{
			Issuer = issuer,
			Address = address
		};
		return ConvertToTrustedIssuerData(CallIdp((IIdpAdministrationClient client, string token) => client.CreateTrustedIssuer(token, model), HandleCreateException));
	}

	public TrustedIssuerData[] QueryTrustedIssuers()
	{
		return CallIdp((IIdpAdministrationClient client, string token) => client.GetTrustedIssuers(token), HandleGetException)?.Select(ConvertToTrustedIssuerData).ToArray() ?? Array.Empty<TrustedIssuerData>();
	}

	public TrustedIssuerData QueryTrustedIssuerById(int id)
	{
		return ConvertToTrustedIssuerData(CallIdp((IIdpAdministrationClient client, string token) => client.GetTrustedIssuer(token, id), HandleGetException));
	}

	public TrustedIssuerData[] QueryTrustedIssuerByIds(IEnumerable<int> ids)
	{
		return CallIdp((IIdpAdministrationClient client, string token) => client.GetTrustedIssuers(token), HandleGetException)?.Where((TrustedIssuerResponseModel m) => ids.Contains(m.Id)).Select(ConvertToTrustedIssuerData).ToArray() ?? Array.Empty<TrustedIssuerData>();
	}

	public TrustedIssuerData UpdateTrustedIssuer(int id, string issuer, Uri address)
	{
		TrustedIssuerUpdateModel updateModel = new TrustedIssuerUpdateModel
		{
			Issuer = issuer,
			Address = address
		};
		return ConvertToTrustedIssuerData(CallIdp((IIdpAdministrationClient client, string token) => client.UpdateTrustedIssuer(token, id, updateModel), HandleUpdateException));
	}

	public void DeleteTrustedIssuer(int id)
	{
		CallIdp((IIdpAdministrationClient client, string token) => client.DeleteTrustedIssuer(token, id), HandleDeleteException);
	}

	private static TrustedIssuerData ConvertToTrustedIssuerData(TrustedIssuerResponseModel model)
	{
		return new TrustedIssuerData
		{
			Id = model.Id,
			Issuer = model.Issuer,
			Address = model.Address.ToString()
		};
	}

	private static Exception HandleDeleteException(IdpException ex)
	{
		if (ex is NotFoundException)
		{
			return VmoExceptionFactory.VMO60411_ObjectNotFoundException();
		}
		return new GenericIdpException(VmoResources.VMO68000, ex);
	}

	private static Exception HandleUpdateException(IdpException ex)
	{
		if (!(ex is NotFoundException))
		{
			if (!(ex is InvalidArgumentsException ex2))
			{
				if (ex is DefaultErrorException ex3)
				{
					string text = ex3.Errors.Keys.FirstOrDefault();
					if (text == "DuplicateIssuer")
					{
						return VmoExceptionFactory.VMO60413_VmoServerServiceException();
					}
					if (text == "DuplicateAddress")
					{
						return VmoExceptionFactory.VMO60416_VmoServerServiceException();
					}
				}
				return new GenericIdpException(VmoResources.VMO68000, ex);
			}
			return new ArgumentException(VmoResources.VMO62001, ex2.Arguments.First());
		}
		return VmoExceptionFactory.VMO60412_ObjectNotFoundException();
	}

	private static Exception HandleGetException(Exception ex)
	{
		if (!(ex is NotFoundException))
		{
			return new GenericIdpException(VmoResources.VMO68000, ex);
		}
		return VmoExceptionFactory.VMO60414_ObjectNotFoundException();
	}

	private static Exception HandleCreateException(IdpException ex)
	{
		if (!(ex is InvalidArgumentsException ex2))
		{
			if (ex is DefaultErrorException ex3)
			{
				string text = ex3.Errors.Keys.FirstOrDefault();
				if (text == "DuplicateIssuer")
				{
					return VmoExceptionFactory.VMO60415_VmoServerServiceException();
				}
				if (text == "DuplicateAddress")
				{
					return VmoExceptionFactory.VMO60417_VmoServerServiceException();
				}
			}
			return new GenericIdpException(VmoResources.VMO68000, ex);
		}
		return new ArgumentException(VmoResources.VMO62001, ex2.Arguments.First());
	}

	public void Dispose()
	{
	}
}
