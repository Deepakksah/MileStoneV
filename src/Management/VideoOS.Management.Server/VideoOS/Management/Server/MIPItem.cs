#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Common.Services;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Platform;
using VideoOS.Platform.Util;

namespace VideoOS.Management.Server;

public class MIPItem : Item
{
	private delegate string FormatSecurityObject(MIPKind mipKind);

	private static Guid LicenseKindId = new Guid("E145597A-ECAF-40A1-99E6-14BB9D45A09E");

	private static Guid LicensesObjectId = new Guid("B9AFA95A-652B-40A4-981B-F3C7D39BFFDC");

	private const int ChangeTypeAdd = 0;

	private const int ChangeTypeUpdate = 1;

	private const int ChangeTypeDelete = 2;

	private static readonly Guid AnalyticsOptionsId = new Guid("4DE695A0-B30A-4F0F-B25E-EE353528DC02");

	public static Item GetItem(IGeminiRequestContext requestContext, Guid objectId)
	{
		if (objectId == LicensesObjectId)
		{
			using (ILicenseComponent licenseComponent = requestContext.CreateLicenseComponent())
			{
				return ConstructLicenseInformationItem(requestContext, licenseComponent);
			}
		}
		using IMIPItemComponent iMIPItemComponent = requestContext.CreateMIPItemComponent();
		Item item = iMIPItemComponent.QueryItem(objectId);
		if (item != null)
		{
			MIPKind kind = MIPKind.GetKind(requestContext, item.FQID.Kind);
			if (kind == null || kind.KindType != "ITEM")
			{
				return item;
			}
			CheckAuthorization(requestContext, (MIPKind k) => FormatSecurityObjectFullPath(item, k.ParentKindId != Guid.Empty), item.FQID.Kind, "GENERIC_READ");
			if (objectId == MIPItemConfigurationApi.RegistrationsObjectId)
			{
				UpdateLicenseRegistrationItemSystemProperties(requestContext, item);
			}
			return item;
		}
		if (objectId == MIPItemConfigurationApi.RegistrationsObjectId)
		{
			return ConstructLicenseRegistrationItem(requestContext, iMIPItemComponent);
		}
		return null;
	}

	public static List<Item> GetItems(IGeminiRequestContext requestContext, Guid kind)
	{
		if (kind == Kind.LprServer)
		{
			return GetLprServerItems(requestContext);
		}
		using IMIPItemComponent iMIPItemComponent = requestContext.CreateMIPItemComponent();
		Item[] array = iMIPItemComponent.QueryItemsByKind(kind);
		if (kind == LicenseKindId)
		{
			if (array.Length == 0)
			{
				array = new Item[1] { ConstructLicenseRegistrationItem(requestContext, iMIPItemComponent) };
			}
			else
			{
				Item item = array.FirstOrDefault((Item x) => x.FQID.ObjectId == MIPItemConfigurationApi.RegistrationsObjectId);
				if (item != null)
				{
					UpdateLicenseRegistrationItemSystemProperties(requestContext, item);
				}
			}
			using ILicenseComponent licenseComponent = requestContext.CreateLicenseComponent();
			List<Item> list = array.ToList();
			Item item2 = list.FirstOrDefault((Item x) => x.FQID.ObjectId == LicensesObjectId || x.FQID.ObjectId == MIPItemConfigurationApi.LegacyLicensesObjectId);
			if (item2 != null)
			{
				list.Remove(item2);
			}
			list.Add(ConstructLicenseInformationItem(requestContext, licenseComponent));
			array = list.ToArray();
		}
		return FilterPermittedReadItems(requestContext, array);
	}

	private static List<Item> GetLprServerItems(IGeminiRequestContext requestContext)
	{
		using IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent();
		List<Item> list = new List<Item>();
		RegisteredServer[] array = registeredServerComponent.QueryRegisteredServersByType(ServiceTypes.ServiceType_LicensePlateRecognitionService);
		foreach (RegisteredServer registeredServer in array)
		{
			Uri uri = new Uri(registeredServer.Addresses[0].URL);
			string name = ((!string.IsNullOrEmpty(registeredServer.Name)) ? registeredServer.Name : uri.DnsSafeHost.ToUpperInvariant());
			Item item = new Item(new FQID(new ServerId("LPR Server", uri.DnsSafeHost, uri.Port, registeredServer.IDServer), Guid.Empty, registeredServer.IDServer, FolderType.No, Kind.LprServer), name);
			item.Properties["Description"] = registeredServer.Description;
			item.Properties["ServiceData"] = registeredServer.CustomData;
			list.Add(item);
		}
		return FilterPermittedReadItems(requestContext, list.ToArray());
	}

	public static List<Item> GetItemsByParent(IGeminiRequestContext requestContext, Guid parentObjectId)
	{
		using IMIPItemComponent iMIPItemComponent = requestContext.CreateMIPItemComponent();
		Item[] itemArray = iMIPItemComponent.QueryItemsByParent(parentObjectId);
		return FilterPermittedReadItems(requestContext, itemArray);
	}

	public static bool SaveItem(IGeminiRequestContext requestContext, Item item)
	{
		try
		{
			bool result = CheckAuthorization(requestContext, (MIPKind mipKind) => FormatSecurityObjectFullPath(item, mipKind.ParentKindId != Guid.Empty), item.FQID.Kind, "GENERIC_WRITE");
			using (IMIPItemComponent iMIPItemComponent = requestContext.CreateMIPItemComponent())
			{
				Item item2 = iMIPItemComponent.QueryItem(item.FQID.ObjectId);
				iMIPItemComponent.SaveItem(item, requestContext.UserDomainAccountName);
				NotifyItemChanged(item.FQID, (item2 != null) ? 1 : 0);
				if ((item2 == null && item.PositioningInformation != null && item.PositioningInformation.IsPointDefined) || (item2 != null && item2.PositioningInformation != item.PositioningInformation))
				{
					NotifyPositionChanged(item.FQID, item.PositioningInformation);
				}
			}
			return result;
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new UnauthorizedAccessException();
		}
	}

	public static bool Delete(IGeminiRequestContext requestContext, Item item)
	{
		try
		{
			bool result = CheckAuthorization(requestContext, (MIPKind mipKind) => FormatSecurityObjectFullPath(item, mipKind.ParentKindId != Guid.Empty), item.FQID.Kind, "GENERIC_WRITE");
			using (IMIPItemComponent iMIPItemComponent = requestContext.CreateMIPItemComponent())
			{
				iMIPItemComponent.DeleteItem(item.FQID.ObjectId);
				if (item != null)
				{
					NotifyItemChanged(item.FQID, 2);
				}
				if (item != null && item.PositioningInformation != null && item.PositioningInformation.IsPointDefined)
				{
					NotifyPositionChanged(item.FQID, new PositioningInformation());
				}
			}
			return result;
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new UnauthorizedAccessException();
		}
	}

	private static void NotifyPositionChanged(FQID fqid, PositioningInformation newPosition)
	{
		Integration.TryNotifyServerMipItemPositionChangedAsync(fqid, newPosition);
	}

	private static void NotifyItemChanged(FQID fqid, int changeType)
	{
		Integration.TryNotifyServerMipItemChangedAsync(fqid, changeType);
	}

	public static void RegisterSecurityObject(IGeminiRequestContext requestContext, Guid objectClass, string objectId, string parentObjectId)
	{
		requestContext.GetSecurityManagerService().RegisterSecurityObject(requestContext, objectClass, objectId?.ToUpperInvariant(), parentObjectId?.ToUpperInvariant());
	}

	public static void UnRegisterSecurityObject(IGeminiRequestContext requestContext, Guid objectClass, string objectId)
	{
		requestContext.GetSecurityManagerService().UnregisterSecurityObject(requestContext, objectClass, objectId?.ToUpperInvariant());
	}

	public static void AddPermissions(IGeminiRequestContext requestContext, Guid objectClass, string objectId, string objectName, AccessControlEntry[] permissions)
	{
		requestContext.GetSecurityManagerService().AddPermissions(requestContext, objectClass, objectId?.ToUpperInvariant(), objectName, permissions);
	}

	private static bool CheckAuthorization(IGeminiRequestContext requestContext, FormatSecurityObject formatSecurityObject, Guid kind, string securityAction)
	{
		MIPKind kind2 = MIPKind.GetKind(requestContext, kind);
		if (kind2 == null)
		{
			throw new MIPKindNotFoundException();
		}
		return CheckAuthorization(requestContext, formatSecurityObject, kind2, securityAction);
	}

	private static bool CheckAuthorization(IGeminiRequestContext requestContext, FormatSecurityObject formatSecurityObject, MIPKind mipKind, string securityAction)
	{
		string objectId = (string.IsNullOrEmpty(mipKind.SecurityAction) ? "/" : formatSecurityObject(mipKind));
		bool result = mipKind.KindType == "ITEM" && !string.IsNullOrEmpty(mipKind.SecurityAction);
		try
		{
			if (mipKind.Id != LicenseKindId)
			{
				requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, mipKind.Id, securityAction, objectId);
			}
		}
		catch (VmoServerServiceException ex)
		{
			if (ex.ErrorCode != 60325)
			{
				throw;
			}
			if (mipKind.KindType == "OPTION" || mipKind.Id == AnalyticsOptionsId)
			{
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, securityAction);
			}
		}
		return result;
	}

	private static List<Item> FilterPermittedReadItems(IGeminiRequestContext requestContext, Item[] itemArray)
	{
		List<Item> list = new List<Item>();
		MIPKind mIPKind = null;
		foreach (Item item in itemArray)
		{
			if (mIPKind == null || mIPKind.Id != item.FQID.Kind)
			{
				mIPKind = MIPKind.GetKind(requestContext, item.FQID.Kind);
			}
			try
			{
				CheckAuthorization(requestContext, (MIPKind k) => FormatSecurityObjectFullPath(item, k.ParentKindId != Guid.Empty), mIPKind, "GENERIC_READ");
				list.Add(item);
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (SecurityObjectDoesNotExistException)
			{
			}
		}
		return list;
	}

	private static string FormatSecurityObjectFullPath(Item item, bool hasParent)
	{
		if (item.FQID.ParentId != Guid.Empty && hasParent)
		{
			return FormatSecurityObjectFullPath(item.FQID.ParentId, item.FQID.ObjectId);
		}
		return FormatSecurityObjectFullPath(item.FQID.Kind, item.FQID.ObjectId);
	}

	private static string FormatSecurityObjectFullPath(Guid overallId, Guid objectId)
	{
		return string.Format(CultureInfo.InvariantCulture, "/{0}/{1}", overallId, objectId).ToUpper(CultureInfo.InvariantCulture);
	}

	private static Item ConstructLicenseInformationItem(IGeminiRequestContext requestContext, ILicenseComponent licenseComponent)
	{
		ILicenseDocument licenseDocument = licenseComponent.QueryLicenseDocument();
		Item item = ConstructEmptyLicenseInformationItem(requestContext);
		if (licenseDocument != null)
		{
			item.Properties[MIPItemConfigurationProperties.Licenses] = licenseDocument.MipPluginLicenseXml;
		}
		else
		{
			item.Properties[MIPItemConfigurationProperties.Licenses] = "<MIPPlugins />";
		}
		return item;
	}

	private static Item ConstructEmptyLicenseInformationItem(IGeminiRequestContext requestContext)
	{
		return new Item(new FQID(ConstructServerId(requestContext), Guid.Empty, MIPItemConfigurationApi.LicensesObjectId, FolderType.No, MIPItemConfigurationApi.LicenseKindId), "License")
		{
			Encrypt = true
		};
	}

	private static void UpdateLicenseRegistrationItemSystemProperties(IGeminiRequestContext requestContext, Item registrationItem)
	{
		registrationItem.Properties["SLC"] = GeminiApplicationCore.FeatureCacheManager.GetSLC(requestContext);
		string sLCEnvironment = GeminiApplicationCore.FeatureCacheManager.GetSLCEnvironment(requestContext);
		if (!string.IsNullOrEmpty(sLCEnvironment))
		{
			registrationItem.Properties["ProductionEnvironment"] = sLCEnvironment;
		}
		string text = "";
		foreach (string item in GeminiApplicationCore.FeatureCacheManager.QueryAllFeatureNames(requestContext))
		{
			text = text + item + ",";
		}
		registrationItem.Properties["Features"] = text;
		registrationItem.Properties["Expire"] = GeminiApplicationCore.FeatureCacheManager.GetLicenseExpiryDate(requestContext).ToString(CultureInfo.InvariantCulture);
		registrationItem.Properties["ProductCode"] = GeminiApplicationCore.FeatureCacheManager.GetProductCode(requestContext).ToString(CultureInfo.InvariantCulture);
		registrationItem.Properties["ExpirationDates"] = GetExpirationDateProperties(requestContext);
	}

	private static string GetExpirationDateProperties(IGeminiRequestContext requestContext)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ILicenseProperty item in from p in GeminiApplicationCore.FeatureCacheManager.QueryAllLicenseProperties(requestContext)
			where string.Equals(p.PropertyType, "ExpirationDate", StringComparison.Ordinal)
			select p)
		{
			if (DateTime.TryParse(item.Value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var result))
			{
				stringBuilder.Append(item.Name + "=" + TimeConvert.ToGmtString(result, includeMilliSeconds: false) + ",");
			}
		}
		return stringBuilder.ToString();
	}

	private static Item ConstructLicenseRegistrationItem(IGeminiRequestContext requestContext, IMIPItemComponent mipItemComponent)
	{
		Item item = new Item(new FQID(ConstructServerId(requestContext), Guid.Empty, MIPItemConfigurationApi.RegistrationsObjectId, FolderType.No, MIPItemConfigurationApi.LicenseKindId), "Registrations");
		UpdateLicenseRegistrationItemSystemProperties(requestContext, item);
		item.Encrypt = true;
		mipItemComponent.SaveItem(item, requestContext.UserDomainAccountName ?? "MIPItem");
		return item;
	}

	private static ServerId ConstructServerId(IGeminiRequestContext requestContext)
	{
		Guid result = Guid.Empty;
		if (!Guid.TryParse(ServerInformation.GetServerInformation(requestContext).Id, out result))
		{
			Trace.WriteLine("MIPItem: No Mgt Server Id ?");
		}
		return new ServerId("XPCO", "localhost", 80, result);
	}
}
