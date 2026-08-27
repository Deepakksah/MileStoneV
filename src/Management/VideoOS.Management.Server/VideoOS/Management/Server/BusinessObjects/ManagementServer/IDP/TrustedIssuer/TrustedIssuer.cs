using System;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Server.DataComponents.IDP;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.BusinessObjects.ManagementServer.IDP.TrustedIssuer;

public static class TrustedIssuer
{
	public static TrustedIssuerData QueryById(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Validation.CheckName(id, "id");
		ConvertToInt(id, "id", out var result);
		using ITrustedIssuerIdpComponent trustedIssuerIdpComponent = requestContext.GetService<IdpClientService>().CreateTrustedIssuerIdpComponent();
		return trustedIssuerIdpComponent.QueryTrustedIssuerById(result);
	}

	public static TrustedIssuerData[] QueryByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Validation.CheckNameArray(ids, "ids");
		int[] array = new int[ids.Length];
		for (int i = 0; i < ids.Length; i++)
		{
			ConvertToInt(ids[i], "ids", out array[i]);
		}
		using ITrustedIssuerIdpComponent trustedIssuerIdpComponent = requestContext.GetService<IdpClientService>().CreateTrustedIssuerIdpComponent();
		return trustedIssuerIdpComponent.QueryTrustedIssuerByIds(array);
	}

	public static TrustedIssuerData[] QueryAll(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using ITrustedIssuerIdpComponent trustedIssuerIdpComponent = requestContext.GetService<IdpClientService>().CreateTrustedIssuerIdpComponent();
		return trustedIssuerIdpComponent.QueryTrustedIssuers();
	}

	public static string[] QueryIds(IGeminiRequestContext requestContext)
	{
		return (from lp in QueryAll(requestContext)
			select lp.Id.ToString(CultureInfo.InvariantCulture)).ToArray();
	}

	public static TrustedIssuerData Create(IGeminiRequestContext requestContext, string issuer, string address)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckStringNotNullOrWhiteSpace(issuer, "issuer");
		Validation.CheckWebAddress(address, allowHttps: true, "address", allowNull: false);
		Uri address2 = new Uri(address);
		TrustedIssuerData trustedIssuerData;
		using (ITrustedIssuerIdpComponent trustedIssuerIdpComponent = requestContext.GetService<IdpClientService>().CreateTrustedIssuerIdpComponent())
		{
			trustedIssuerData = trustedIssuerIdpComponent.CreateTrustedIssuer(issuer, address2);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditTrustedIssuerCreated(trustedIssuerData.Id, issuer, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return trustedIssuerData;
	}

	public static TrustedIssuerData Update(IGeminiRequestContext requestContext, int id, string issuer, string address)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		Validation.CheckStringNotNullOrWhiteSpace(issuer, "issuer");
		Validation.CheckWebAddress(address, allowHttps: true, "address", allowNull: false);
		Uri address2 = new Uri(address);
		TrustedIssuerData trustedIssuerData;
		using (ITrustedIssuerIdpComponent trustedIssuerIdpComponent = requestContext.GetService<IdpClientService>().CreateTrustedIssuerIdpComponent())
		{
			trustedIssuerData = trustedIssuerIdpComponent.UpdateTrustedIssuer(id, issuer, address2);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditTrustedIssuerUpdated(trustedIssuerData.Id, issuer, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return trustedIssuerData;
	}

	public static void Delete(IGeminiRequestContext requestContext, int id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
		TrustedIssuerData trustedIssuerData;
		using (ITrustedIssuerIdpComponent trustedIssuerIdpComponent = requestContext.GetService<IdpClientService>().CreateTrustedIssuerIdpComponent())
		{
			try
			{
				trustedIssuerData = trustedIssuerIdpComponent.QueryTrustedIssuerById(id);
			}
			catch (ObjectNotFoundException)
			{
				throw VmoExceptionFactory.VMO60411_ObjectNotFoundException();
			}
			trustedIssuerIdpComponent.DeleteTrustedIssuer(id);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditTrustedIssuerDeleted(trustedIssuerData.Id, trustedIssuerData.Issuer, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	public static void ConvertToInt(string objectId, string parameterName, out int result)
	{
		if (!int.TryParse(objectId, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
		{
			throw new ArgumentException(VmoResources.VMO62003, parameterName);
		}
	}
}
