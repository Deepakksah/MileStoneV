using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using VideoOS.Common.Crypto;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Platform;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIMIPItem")]
[EntityName("ConfigAPIMIPItemFolder")]
[EntityName("ConfigAPIMIPKind")]
[EntityName("ConfigAPIMIPKindFolder")]
public class MIPItemConfigurationApi : SecurityConfigurationApi, IConfigurationApiSupport
{
	public static class Keys
	{
		public static string SymmetricAlgorithmConfigurationXml => "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n                            <SymmetricAlgorithmConfiguration>\r\n                                <Algorithm>RC2</Algorithm>\r\n                                <Key>BtV/KTKU8n487Lrr3/MbnQ==</Key>\r\n                                <IV>I+5bufSLOtk=</IV>\r\n                                <Compression>Deflate</Compression>\r\n                            </SymmetricAlgorithmConfiguration>";
	}

	public static Guid LicenseKindId = new Guid("E145597A-ECAF-40A1-99E6-14BB9D45A09E");

	public static Guid LicensesObjectId = new Guid("B9AFA95A-652B-40A4-981B-F3C7D39BFFDC");

	public static Guid RegistrationsObjectId = new Guid("AD5EFDCE-3B21-47D5-A6A8-657E43A3E638");

	public static Guid LegacyLicensesObjectId = new Guid("E145597A-ECAF-40A1-99E6-14BB9D45A09E");

	public static Guid LegacyRegistrationsObjectId = new Guid("E125597A-ECAF-40A1-99E6-24BB9D45A09E");

	private HashSet<Guid> _productPluginOrKinds = new HashSet<Guid>
	{
		LicenseKindId,
		Kind.GisMapLocation,
		new Guid("A4BB0877-1640-403F-B6E1-AA0B48383C92"),
		new Guid("EBA9E28B-143C-42EA-8D5A-BB72781F5472"),
		new Guid("F8C3FC22-3622-4C43-AC02-D06BC87638EA"),
		new Guid("32DC3EC5-FD0F-4C12-899B-EC62E077D101"),
		new Guid("8C65A718-9A1E-4615-AE0B-9BDB0B5D6B75"),
		new Guid("F3B0A519-CE2F-4292-B637-97F93D441859"),
		new Guid("B71FD628-E593-42FC-B528-5A927EA9BDA4"),
		new Guid("5D220973-46DF-4EAA-9A53-11BD59C61EA5"),
		new Guid("B301A435-1A25-41E3-86B4-4786D9D876CD"),
		new Guid("C20AB189-8568-4885-9CE0-41EFB153FEB0"),
		new Guid("6D3BDB33-D913-470F-8784-3DD4EE94DB8C"),
		new Guid("D0BC0D25-1F1E-41D0-8606-5AA321D360A3"),
		new Guid("9807CA8A-46C2-45ED-9446-6423DAE2CD88"),
		new Guid("2EB4A521-063A-4C9F-B415-9FC78411C1F8"),
		new Guid("C5CF7015-549C-492D-AD66-8D6C59B8D03D")
	};

	private static readonly HashSet<string> PropertiesToKeep = new HashSet<string>
	{
		"Enabled",
		"LastModified",
		MIPItemConfigurationProperties.GisPointKey,
		"Name",
		"Id",
		"EnabledPropertySpecialFormat"
	};

	protected override Dictionary<string, List<string>> ItemTypeActionsToExclude => new Dictionary<string, List<string>> { 
	{
		"MIPItem",
		new List<string>()
	} };

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		MIPFactory.CheckAndInitialize(requestContext);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		if (new Guid(path.Id) == LicensesObjectId)
		{
			throw new NotSupportedException("Cannot save license object item");
		}
		if (path.ItemType == "MIPItem")
		{
			return SetMipItem(requestContext, item, path);
		}
		if (path.ItemType == "MIPKind")
		{
			return SetMipKind(requestContext, item, path, parentPath);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult SetMipItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidateResult validateResult = ValidateMIPItemInternal(item, out var name, out var serverScheme, out var serverHostName, out var serverPort, out var serverType, out var serverId, out var parentId, out var kind, out var objectId, out var enabled, out var gisPoint, out var encrypt);
		if (!validateResult.ValidatedOk)
		{
			if (validateResult.ErrorResults != null && validateResult.ErrorResults.Length != 0)
			{
				throw new PropertyInvalidException(validateResult.ErrorResults[0].ErrorText, validateResult.ErrorResults[0].ErrorProperty);
			}
			throw new PropertyInvalidException();
		}
		Item item2 = MIPItem.GetItem(requestContext, objectId);
		if (item2 != null && kind == LicenseKindId && objectId == RegistrationsObjectId && IsLicenseRegistrationUnchanged(item2, item))
		{
			try
			{
				ConfigurationItem resultItem = ConvertToMIPItemConfigurationItem(path, item2, creating: false, hasPermissions: false);
				validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = resultItem;
				return validateResult;
			}
			catch (Exception ex)
			{
				throw new PropertyInvalidException(ex.Message);
			}
		}
		MIPKind kind2 = MIPKind.GetKind(requestContext, kind);
		if (kind2 == null)
		{
			Guid guid = kind;
			throw new ArgumentException("Kind does not exist: " + guid.ToString());
		}
		if (item2 == null)
		{
			throw new PathNotFoundException("Path not found: " + item.Path);
		}
		string[] array = (from p in item2.Properties
			where !PropertiesToKeep.Contains(p.Key) && !item.Properties.Any((Property ip) => ip.Key == p.Key)
			select p.Key).ToArray();
		foreach (string key in array)
		{
			item2.Properties.Remove(key);
		}
		item2.Enabled = enabled;
		item2.PositioningInformation = new PositioningInformation(gisPoint, 0.0, 0.0, 0.0);
		item2.Name = name;
		item2.Encrypt = encrypt;
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (!(property.Key == "Enabled") && !(property.Key == MIPItemConfigurationProperties.GisPointKey) && !property.Key.StartsWith("FQID.", StringComparison.OrdinalIgnoreCase) && !(property.Key == "Name") && !(property.Key == "Id"))
			{
				item2.Properties[property.Key] = property.Value;
			}
		}
		if (item2.Properties.ContainsKey("Name"))
		{
			item2.Properties.Remove("Name");
		}
		if (item2.Properties.ContainsKey("Id"))
		{
			item2.Properties.Remove("Id");
		}
		item2.FQID.ServerId.Id = serverId;
		item2.FQID.ServerId.ServerScheme = serverScheme;
		item2.FQID.ServerId.ServerHostname = serverHostName;
		item2.FQID.ServerId.Serverport = serverPort;
		item2.FQID.ServerId.ServerType = serverType;
		item2.FQID.ParentId = parentId;
		try
		{
			MIPItem.SaveItem(requestContext, item2);
			ConfigurationItem resultItem2 = ConvertToMIPItemConfigurationItem(path, MIPItem.GetItem(requestContext, item2.FQID.ObjectId), creating: false, kind2.SecurityAction != null && kind2.SecurityAction.Any());
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem2;
			return validateResult;
		}
		catch (UnauthorizedAccessException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw new PropertyInvalidException(ex3.Message);
		}
	}

	private static bool IsLicenseRegistrationUnchanged(Item existingItem, ConfigurationItem newItem)
	{
		string[] array = new string[7]
		{
			"SLC",
			"Features",
			"Expire",
			"ProductCode",
			MIPItemConfigurationProperties.Registrations,
			"CudaPublicKey",
			"ExpirationDates"
		};
		if (existingItem.Encrypt.ToString() != newItem.Properties.FirstOrDefault((Property x) => x.Key == "__Encrypt")?.Value)
		{
			return false;
		}
		string[] array2 = array;
		for (int num = 0; num < array2.Length; num++)
		{
			if (!IsPropertyUnchanged(array2[num], existingItem, newItem))
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsPropertyUnchanged(string propertyKey, Item existingItem, ConfigurationItem newItem)
	{
		if (existingItem.Properties.ContainsKey(propertyKey))
		{
			Property property = newItem.Properties.FirstOrDefault((Property p) => p.Key == propertyKey);
			if (property == null)
			{
				return false;
			}
			if (property.Value == null)
			{
				return existingItem.Properties[propertyKey] == null;
			}
			return property.Value.Equals(existingItem.Properties[propertyKey], StringComparison.Ordinal);
		}
		if (newItem.Properties.Any((Property p) => p.Key == propertyKey))
		{
			return false;
		}
		return true;
	}

	private static ValidateResult SetMipKind(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ValidateResult validateResult = ValidateMIPKindInternal(item, out var name, out var kind, out var _, out var pluginName, out var kindType, out var securityAction, out var securityActionList, out var securityActionNames, out var securityActionNamesList, out var displayOnGisMap, out var parentKindId);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		MIPKind kind2 = MIPKind.GetKind(requestContext, kind);
		if (kind2 == null)
		{
			throw new PathNotFoundException("path: " + path.Text);
		}
		validateResult = ValidateParentKindId(requestContext, parentKindId);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		validateResult = ValidateSecurityModifications(requestContext, kind2, securityAction);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		kind2.PluginName = pluginName;
		kind2.Name = name;
		kind2.KindType = kindType;
		kind2.SecurityAction = string.Join(",", securityActionList);
		kind2.SecurityActionNames = string.Join(",", securityActionNames);
		kind2.DisplayOnGisMap = !displayOnGisMap.HasValue || displayOnGisMap.Value;
		kind2.ParentKindId = parentKindId;
		kind2.ModifiedUser = requestContext.UserDomainAccountName;
		MIPKind.SaveKind(requestContext, kind2);
		if (!string.IsNullOrEmpty(kind2.SecurityAction))
		{
			MIPItemConfigurationApiSecurity.RegisterKindNamespace(requestContext, kind, kind.ToString(), securityActionList, securityActionNamesList);
		}
		validateResult = new ValidateResult();
		validateResult.ValidatedOk = true;
		validateResult.ResultItem = ConvertToMIPKindConfigurationItem(parentPath, MIPKind.GetKind(requestContext, kind), isSettable: true);
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		if (path.ItemType == "MIPItem")
		{
			string name;
			string serverScheme;
			string serverHostName;
			int serverPort;
			string serverType;
			Guid serverId;
			Guid parentId;
			Guid kind;
			Guid objectId;
			bool enabled;
			string gisPoint;
			bool encrypt;
			ValidateResult validateResult = ValidateMIPItemInternal(item, out name, out serverScheme, out serverHostName, out serverPort, out serverType, out serverId, out parentId, out kind, out objectId, out enabled, out gisPoint, out encrypt);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			if (MIPItem.GetItem(requestContext, objectId) != null)
			{
				return new ValidateResult
				{
					ValidatedOk = true
				};
			}
			ValidateResult validateResult2 = new ValidateResult();
			validateResult2.ValidatedOk = false;
			validateResult2.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorProperty = "Path",
					ErrorText = "Path not found: " + item.Path
				}
			};
			return validateResult2;
		}
		if (path.ItemType == "MIPKind")
		{
			ValidateResult validateResult3 = ValidateMIPKindInternal(item, out var _, out var kind2, out var _, out var _, out var _, out var securityAction, out var _, out var _, out var _, out var _, out var parentKindId);
			if (!validateResult3.ValidatedOk)
			{
				return validateResult3;
			}
			MIPKind kind3 = MIPKind.GetKind(requestContext, kind2);
			if (kind3 == null)
			{
				throw new PathNotFoundException("path: " + path.Text);
			}
			validateResult3 = ValidateParentKindId(requestContext, parentKindId);
			if (!validateResult3.ValidatedOk)
			{
				return validateResult3;
			}
			validateResult3 = ValidateSecurityModifications(requestContext, kind3, securityAction);
			if (!validateResult3.ValidatedOk)
			{
				return validateResult3;
			}
			validateResult3 = new ValidateResult();
			validateResult3.ValidatedOk = true;
			validateResult3.ResultItem = null;
			return validateResult3;
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateMIPKindInternal(ConfigurationItem item, out string name, out Guid kind, out Guid pluginId, out string pluginName, out string kindType, out string securityAction, out List<string> securityActionList, out string securityActionNames, out List<string> securityActionNamesList, out bool? displayOnGisMap, out Guid parentKindId)
	{
		name = null;
		kind = Guid.Empty;
		pluginId = Guid.Empty;
		pluginName = null;
		kindType = "";
		securityAction = "";
		securityActionNames = "";
		securityActionList = new List<string>();
		securityActionNamesList = new List<string>();
		displayOnGisMap = true;
		parentKindId = Guid.Empty;
		try
		{
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			if (item.Properties != null)
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue(ConfigurationApiResources.PropertyGeneralId, out string value);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.PluginIdKey, out string value2);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.PluginNameKey, out pluginName);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.KindTypeKey, out kindType);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.SecurityActionKey, out securityAction);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.SecurityActionNamesKey, out securityActionNames);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.DisplayOnGisMapKey, out displayOnGisMap);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.ParentKindKey, out string value3);
				if (!Guid.TryParse(value, out kind))
				{
					validateResult.ValidatedOk = false;
					validateResult.ErrorResults = new ErrorResult[1]
					{
						new ErrorResult
						{
							ErrorProperty = ConfigurationApiResources.PropertyGeneralId,
							ErrorText = "Not a GUID"
						}
					};
					return validateResult;
				}
				if (kind == Guid.Empty)
				{
					validateResult.ValidatedOk = false;
					validateResult.ErrorResults = new ErrorResult[1]
					{
						new ErrorResult
						{
							ErrorProperty = ConfigurationApiResources.PropertyGeneralId,
							ErrorText = "Not a GUID"
						}
					};
					return validateResult;
				}
				if (!Guid.TryParse(value2, out pluginId))
				{
					validateResult.ValidatedOk = false;
					validateResult.ErrorResults = new ErrorResult[1]
					{
						new ErrorResult
						{
							ErrorProperty = MIPItemConfigurationProperties.PluginIdKey,
							ErrorText = "Not a GUID"
						}
					};
					return validateResult;
				}
				if (pluginId == Guid.Empty)
				{
					validateResult.ValidatedOk = false;
					validateResult.ErrorResults = new ErrorResult[1]
					{
						new ErrorResult
						{
							ErrorProperty = MIPItemConfigurationProperties.PluginIdKey,
							ErrorText = "Not a GUID"
						}
					};
					return validateResult;
				}
				if (kindType != "ITEM" && kindType != "OPTION")
				{
					validateResult.ValidatedOk = false;
					validateResult.ErrorResults = new ErrorResult[1]
					{
						new ErrorResult
						{
							ErrorProperty = MIPItemConfigurationProperties.KindTypeKey,
							ErrorText = "Must be ITEM or OPTION"
						}
					};
					return validateResult;
				}
				if (!Guid.TryParse(value3, out parentKindId))
				{
					validateResult.ValidatedOk = false;
					validateResult.ErrorResults = new ErrorResult[1]
					{
						new ErrorResult
						{
							ErrorProperty = MIPItemConfigurationProperties.ParentKindKey,
							ErrorText = "Not a GUID"
						}
					};
					return validateResult;
				}
				if (!string.IsNullOrEmpty(securityAction))
				{
					string[] array = securityAction.Split(',');
					foreach (string text in array)
					{
						if (!string.IsNullOrEmpty(text))
						{
							securityActionList.Add(text);
						}
					}
					array = securityActionNames?.Split(',') ?? Array.Empty<string>();
					foreach (string text2 in array)
					{
						if (!string.IsNullOrEmpty(text2))
						{
							securityActionNamesList.Add(text2);
						}
					}
					if (securityActionList.Count != 0)
					{
						if (!securityActionList.Contains("ADMIN_SECURITY"))
						{
							securityActionList.Add("ADMIN_SECURITY");
							if (securityActionNamesList.Count != 0)
							{
								securityActionNamesList.Add("Administration");
							}
						}
						if (!securityActionList.Contains("GENERIC_READ") || !securityActionList.Contains("GENERIC_WRITE"))
						{
							validateResult.ValidatedOk = false;
							validateResult.ErrorResults = new ErrorResult[1]
							{
								new ErrorResult
								{
									ErrorProperty = MIPItemConfigurationProperties.SecurityActionKey,
									ErrorText = "When used, SecurityAction must contain GENERIC_READ, GENERIC_WRITE and ADMIN_SECURITY"
								}
							};
							return validateResult;
						}
					}
					if (securityActionNamesList.Count == 0)
					{
						securityActionNamesList = securityActionList;
						securityActionNames = securityAction;
					}
					if (securityActionList.Count != securityActionNamesList.Count)
					{
						GeminiTrace.Error("MIPItemConfigurationApi.ValidateMIPKindInternal: SecurityAction and SecurityActionNames must have the same number of entries. SecurityAction: " + securityAction + ", SecurityActionNames: " + securityActionNames);
						validateResult.ValidatedOk = false;
						validateResult.ErrorResults = new ErrorResult[1]
						{
							new ErrorResult
							{
								ErrorProperty = MIPItemConfigurationProperties.SecurityActionKey,
								ErrorText = "SecurityAction and SecurityActionNames must have the same number of entries"
							}
						};
						return validateResult;
					}
				}
			}
			return validateResult;
		}
		catch (ArgumentException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	private static ValidateResult ValidateMIPItemInternal(ConfigurationItem item, out string name, out string serverScheme, out string serverHostName, out int serverPort, out string serverType, out Guid serverId, out Guid parentId, out Guid kind, out Guid objectId, out bool enabled, out string gisPoint, out bool encrypt)
	{
		name = null;
		kind = Guid.Empty;
		name = "";
		serverScheme = "";
		serverHostName = "";
		serverPort = 80;
		serverType = "NA";
		serverId = Guid.Empty;
		parentId = Guid.Empty;
		objectId = Guid.Empty;
		enabled = item.EnableProperty.Enabled;
		gisPoint = "";
		encrypt = false;
		try
		{
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			if (item.Properties == null)
			{
				ValidateResult validateResult2 = new ValidateResult();
				validateResult2.ValidatedOk = false;
				validateResult2.ErrorResults = new ErrorResult[1]
				{
					new ErrorResult
					{
						ErrorText = "No properties provided"
					}
				};
				return validateResult2;
			}
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue(ConfigurationApiResources.PropertyGeneralId, out string value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.FQIDKindKey, out string value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.FQIDParentIdKey, out string value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.FQIDServerIdKey, out string value4);
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.FQIDSchemeKey, out serverScheme))
			{
				serverScheme = Uri.UriSchemeHttp;
			}
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.FQIDHostKey, out serverHostName);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.FQIDPortKey, out string value5);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.FQIDTypeKey, out serverType);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.GisPointKey, out gisPoint);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.EncryptKey, out string value6);
			List<ErrorResult> list = new List<ErrorResult>();
			if (!Guid.TryParse(value, out objectId))
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = ConfigurationApiResources.PropertyGeneralId,
					ErrorText = "Not a GUID"
				});
			}
			if (objectId == Guid.Empty)
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = ConfigurationApiResources.PropertyGeneralId,
					ErrorText = "GUID is Empty"
				});
			}
			if (!Guid.TryParse(value2, out kind))
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = MIPItemConfigurationProperties.FQIDKindKey,
					ErrorText = "Not a GUID"
				});
			}
			if (kind == Guid.Empty)
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = MIPItemConfigurationProperties.FQIDKindKey,
					ErrorText = "GUID is Empty"
				});
			}
			if (!Guid.TryParse(value3, out parentId))
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = MIPItemConfigurationProperties.FQIDParentIdKey,
					ErrorText = "Not a GUID"
				});
			}
			if (!Guid.TryParse(value4, out serverId))
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = MIPItemConfigurationProperties.FQIDServerIdKey,
					ErrorText = "Not a GUID"
				});
			}
			if (!int.TryParse(value5, out serverPort))
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = MIPItemConfigurationProperties.FQIDPortKey,
					ErrorText = "Not a integer"
				});
			}
			if (value6 != null && !bool.TryParse(value6, out encrypt))
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = MIPItemConfigurationProperties.EncryptKey,
					ErrorText = "Not a boolean"
				});
			}
			if (list.Count > 0)
			{
				validateResult.ValidatedOk = false;
				validateResult.ErrorResults = list.ToArray();
			}
			return validateResult;
		}
		catch (ArgumentException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			if (path.ItemType == "MIPKindFolder")
			{
				foreach (MIPKind kind3 in MIPKind.GetKinds(requestContext))
				{
					if (!_productPluginOrKinds.Contains(kind3.Id) && !_productPluginOrKinds.Contains(kind3.PluginId))
					{
						list.Add(ConvertToMIPKindConfigurationItem(path, kind3, isSettable: true));
					}
				}
			}
			else if (path.ItemType == "MIPItemFolder")
			{
				Guid guid = new Guid(path.Id);
				if (guid == Guid.Empty)
				{
					throw new PathInvalidException("Guid is Empty");
				}
				bool hasPermissions = false;
				List<Item> list2;
				if (path.ParentItemType == "MIPKind")
				{
					MIPKind kind = MIPKind.GetKind(requestContext, guid);
					if (kind == null)
					{
						return list.ToArray();
					}
					hasPermissions = kind.SecurityAction != null && kind.SecurityAction.Any();
					list2 = MIPItem.GetItems(requestContext, guid);
				}
				else
				{
					list2 = MIPItem.GetItemsByParent(requestContext, guid);
					if (list2.Any())
					{
						MIPKind kind2 = MIPKind.GetKind(requestContext, list2.First().FQID.Kind);
						hasPermissions = kind2.SecurityAction != null && kind2.SecurityAction.Any();
					}
				}
				foreach (Item item in list2)
				{
					list.Add(ConvertToMIPItemConfigurationItem(path, item, creating: false, hasPermissions));
				}
			}
			return list.ToArray();
		}
		List<ConfigurationItem> list3 = new List<ConfigurationItem>();
		if (path.ItemType == "MIPKind")
		{
			list3.Add(CreateMIPItemFolderFromParent(path));
		}
		else if (path.ItemType == "MIPItem")
		{
			ThrowIfInvalidMIPItem(path);
			list3.Add(CreateMIPItemFolderFromParent(path));
		}
		return list3.ToArray();
	}

	public override ConfigurationItem[] QueryItems(IGeminiRequestContext requestContext, ConfigurationItemPath folderPath, ItemFilter itemFilter, int maxResults, int startIndex, List<ItemFilter> childItemFilters)
	{
		if (folderPath == null)
		{
			throw new ArgumentNullException("folderPath");
		}
		if (itemFilter == null)
		{
			throw new ArgumentNullException("itemFilter");
		}
		List<MIPKind> list = new List<MIPKind>();
		PropertyFilter propertyFilter = itemFilter.PropertyFilters.FirstOrDefault((PropertyFilter f) => f.Key == "FQID.Kind");
		if (propertyFilter != null)
		{
			if (Guid.TryParse(propertyFilter.Value, out var result))
			{
				MIPKind kind = MIPKind.GetKind(requestContext, result);
				if (kind == null)
				{
					throw new ArgumentException("FQID.Kind does not exists");
				}
				list.Add(kind);
			}
		}
		else
		{
			list = MIPKind.GetKinds(requestContext);
		}
		List<ConfigurationItem> list2 = new List<ConfigurationItem>();
		if (itemFilter.ItemType == "MIPItem")
		{
			foreach (MIPKind item3 in list)
			{
				foreach (Item item4 in MIPItem.GetItems(requestContext, item3.Id))
				{
					ConfigurationItem item = ConvertToMIPItemConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateItemPath("MIPKind", item4.FQID.Kind.ToString()), "MIPItemFolder").CreatePath(), item4, creating: false, item3.SecurityAction != null && item3.SecurityAction.Any());
					if (ConfigurationApiBase.Match(item, itemFilter))
					{
						list2.Add(item);
					}
					if (list2.Count >= maxResults)
					{
						break;
					}
				}
				if (list2.Count >= maxResults)
				{
					break;
				}
			}
		}
		if (itemFilter.ItemType == "MIPKind")
		{
			ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "MIPKindFolder").CreatePath();
			foreach (MIPKind item5 in list)
			{
				if (!_productPluginOrKinds.Contains(item5.Id) && !_productPluginOrKinds.Contains(item5.PluginId))
				{
					ConfigurationItem item2 = ConvertToMIPKindConfigurationItem(parentPath, item5, isSettable: false);
					if (ConfigurationApiBase.Match(item2, itemFilter))
					{
						list2.Add(item2);
					}
					if (list2.Count >= maxResults)
					{
						break;
					}
				}
			}
		}
		List<ConfigurationItem> list3 = new List<ConfigurationItem>();
		for (int num = startIndex; num < startIndex + maxResults && num < list2.Count; num++)
		{
			list3.Add(list2[num]);
		}
		return list3.ToArray();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		MIPFactory.CheckAndInitialize(requestContext);
		if (path.IsFolder)
		{
			if (path.ItemType == "MIPKindFolder")
			{
				return GroupConfigurationItemFactory.CreateFolderItem("MIPKindFolder", ConfigurationItemPath.CreateSystemPath());
			}
			ThrowIfInvalidMIPItem(path);
			return CreateMIPItemFolder(path);
		}
		if (path.ItemType == "MIPKind")
		{
			ThrowIfInvalidMIPKind(path);
			MIPKind kind = MIPKind.GetKind(requestContext, new Guid(path.Id));
			if (kind == null)
			{
				throw new PathNotFoundException("MIPKind not found: " + path.Text);
			}
			return ConvertToMIPKindConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "MIPKindFolder").CreatePath(), kind, isSettable: true);
		}
		if (path.ItemType == "MIPItem")
		{
			ThrowIfInvalidMIPItem(path);
			bool hasPermissions = false;
			Guid guid = new Guid(path.Id);
			Item item;
			if (guid == LegacyLicensesObjectId)
			{
				item = EncryptNonFipsProperty(MIPItem.GetItem(requestContext, LicensesObjectId), MIPItemConfigurationProperties.Licenses);
				item.FQID = new FQID(item.FQID.ServerId, item.FQID.ParentId, LegacyLicensesObjectId, item.FQID.FolderType, item.FQID.Kind);
			}
			else if (guid == LegacyRegistrationsObjectId)
			{
				item = EncryptNonFipsProperty(MIPItem.GetItem(requestContext, RegistrationsObjectId), MIPItemConfigurationProperties.Registrations);
				item.FQID = new FQID(item.FQID.ServerId, item.FQID.ParentId, LegacyRegistrationsObjectId, item.FQID.FolderType, item.FQID.Kind);
			}
			else
			{
				item = MIPItem.GetItem(requestContext, guid);
				if (item != null)
				{
					MIPKind kind2 = MIPKind.GetKind(requestContext, item.FQID.Kind);
					hasPermissions = kind2.SecurityAction != null && kind2.SecurityAction.Any();
				}
			}
			if (item == null)
			{
				throw new PathNotFoundException("MIPItem not found: " + path.Text);
			}
			return ConvertToMIPItemConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateItemPath("MIPKind", item.FQID.Kind.ToString()), "MIPItemFolder").CreatePath(), item, creating: false, hasPermissions);
		}
		throw new PathInvalidException("Unknown ItemType:" + path.ItemType);
	}

	public static Item EncryptNonFipsProperty(Item item, string propertyName)
	{
		if (item != null && item.Properties.ContainsKey(propertyName))
		{
			try
			{
				byte[] inArray = new EncryptionHelper(Keys.SymmetricAlgorithmConfigurationXml).EncryptStringData(item.Properties[propertyName]);
				item.Properties[propertyName] = Convert.ToBase64String(inArray);
			}
			catch (Exception ex) when (ex is InvalidOperationException || ex is CryptographicException)
			{
				throw new NotSupportedException("Data protected with non-FIPS compliant algorithms cannot be fetched when FIPS group policy is enabled.", ex.InnerException);
			}
		}
		return item;
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		MIPFactory.CheckAndInitialize(requestContext);
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddMIPKind":
			if (!flag)
			{
				return CreateInvokeInfoAddMIPKind(item, method);
			}
			return AddMIPKind(requestContext, item, path);
		case "RemoveMIPKind":
			if (!flag)
			{
				return CreateInvokeInfoRemoveMIPKind(item, GetChildItems(requestContext, path), method);
			}
			return RemoveMIPKind(requestContext, item, path);
		case "AddMIPItem":
			if (!flag)
			{
				return CreateInvokeInfoAddMIPItem(item, parentPath, method);
			}
			return AddMIPItem(requestContext, item, path);
		case "RemoveMIPItem":
			if (!flag)
			{
				return CreateInvokeInfoRemoveMIPItem(item, GetChildItems(requestContext, path), method);
			}
			return RemoveMIPItem(requestContext, item, path);
		case "GetMIPOptionProperty":
			if (!flag)
			{
				return CreateInvokeInfoGetMIPOption(item, method);
			}
			return GetMIPOptionProperty(requestContext, item, path);
		case "SetMIPOptionProperty":
			if (!flag)
			{
				return CreateInvokeInfoSetMIPOption(item, method);
			}
			return SetMIPOptionProperty(requestContext, item, path);
		case "MIPItemDataVersionSum":
			return MIPItemDataVersionSum(requestContext, item, path);
		case "ChangeSecurityPermissions":
			return InvokeSecurityMethod(requestContext, item, path, method);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
	}

	private static ConfigurationItem CreateInvokeInfoAddMIPKind(ConfigurationItem item, string methodId)
	{
		Property idPropertyGuid = GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.Empty.ToString());
		idPropertyGuid.IsSettable = true;
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddMIPKind;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[9]
		{
			idPropertyGuid,
			GeneralPropertyDefinitions.GetNameProperty("", isSettable: true),
			MIPItemConfigurationProperties.PluginIdProperty(Guid.Empty, isSettable: true),
			MIPItemConfigurationProperties.PluginNameProperty("", isSettable: true),
			MIPItemConfigurationProperties.KindTypeProperty("ITEM", isSettable: true),
			MIPItemConfigurationProperties.SecurityActionProperty("", isSettable: true),
			MIPItemConfigurationProperties.SecurityActionNamesProperty("", isSettable: true),
			MIPItemConfigurationProperties.DisplayOnGisMapProperty(propertyValue: true),
			MIPItemConfigurationProperties.ParentKindProperty(Guid.Empty, isSettable: true)
		};
		return configurationItem;
	}

	public override bool SetSecurityOnRoleOnly()
	{
		return true;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveMIPKind(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveMIPKind, "PropertyRemoveMIPKind", ConfigurationApiResources.PropertyRemoveMIPKind, item, members);
	}

	private static ConfigurationItem AddMIPKind(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		List<string> securityActionList = new List<string>();
		List<string> securityActionNamesList = new List<string>();
		ValidateResult validateResult = ValidateMIPKindInternal(item, out var name, out var kind, out var pluginId, out var pluginName, out var kindType, out var securityAction, out securityActionList, out var _, out securityActionNamesList, out var displayOnGisMap, out var parentKindId);
		if (!validateResult.ValidatedOk)
		{
			if (validateResult.ErrorResults != null && validateResult.ErrorResults.Length != 0)
			{
				throw new PropertyInvalidException(validateResult.ErrorResults[0].ErrorText, validateResult.ErrorResults[0].ErrorProperty);
			}
			throw new PropertyInvalidException();
		}
		if (MIPKind.GetKind(requestContext, kind) != null)
		{
			Guid guid = kind;
			throw new ArgumentException("Duplicate, KindId already exists - " + guid.ToString());
		}
		validateResult = ValidateParentKindId(requestContext, parentKindId);
		if (!validateResult.ValidatedOk)
		{
			throw new ArgumentException(validateResult.ErrorResults[0].ErrorText);
		}
		if (securityActionList.Count != 0)
		{
			if (!securityActionList.Contains("ADMIN_SECURITY"))
			{
				securityActionList.Add("ADMIN_SECURITY");
				if (securityActionNamesList.Count != 0)
				{
					securityActionNamesList.Add("ADMIN_SECURITY");
				}
			}
			if (!securityActionList.Contains("GENERIC_READ"))
			{
				securityActionList.Add("GENERIC_READ");
			}
			if (!securityActionList.Contains("GENERIC_WRITE"))
			{
				securityActionList.Add("GENERIC_WRITE");
			}
		}
		MIPKind mIPKind = new MIPKind
		{
			Id = kind,
			Name = name,
			PluginId = pluginId,
			PluginName = pluginName,
			KindType = kindType,
			SecurityAction = string.Join(",", securityActionList),
			SecurityActionNames = string.Join(",", securityActionNamesList),
			DisplayOnGisMap = (!displayOnGisMap.HasValue || displayOnGisMap.Value),
			ParentKindId = parentKindId,
			LastModified = DateTime.UtcNow,
			DataVersion = 1,
			ModifiedUser = requestContext.UserDomainAccountName
		};
		try
		{
			MIPKind.SaveKind(requestContext, mIPKind);
			if (!string.IsNullOrEmpty(securityAction))
			{
				MIPItemConfigurationApiSecurity.RegisterKindNamespace(requestContext, kind, kind.ToString(), securityActionList, securityActionNamesList);
				foreach (Item item2 in MIPItem.GetItems(requestContext, kind))
				{
					MIPItemConfigurationApiSecurity.RegisterItem(requestContext, item2.FQID.ParentId, item2.FQID.ObjectId, kind);
				}
			}
			ConfigurationItem configurationItem = ConvertToMIPKindConfigurationItem(path, mIPKind, isSettable: false);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (Exception ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
	}

	private static ConfigurationItem RemoveMIPKind(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		Guid guid = new Guid(invokeMemberItemSelectionPath.Id);
		List<Item> items = MIPItem.GetItems(requestContext, guid);
		if (items != null && items.Count > 0)
		{
			throw new ArgumentException("Cannot remove Kind when Items are defined for this Kind");
		}
		try
		{
			MIPKind obj = MIPKind.GetKind(requestContext, guid) ?? throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
			MIPKind.DeleteKind(requestContext, guid);
			if (!string.IsNullOrEmpty(obj.SecurityAction))
			{
				MIPKind.UnRegisterNamespace(requestContext, guid);
			}
		}
		catch (RemoveNonExistingMIPItemException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem CreateInvokeInfoAddMIPItem(ConfigurationItem item, ConfigurationItemPath parentPath, string methodId)
	{
		Property idPropertyGuid = GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.NewGuid().ToString());
		Guid parentId = Guid.Empty;
		Guid kind = Guid.Empty;
		if (parentPath.ItemType == "MIPKind")
		{
			kind = new Guid(parentPath.Id);
		}
		else
		{
			parentId = new Guid(parentPath.Id);
		}
		idPropertyGuid.IsSettable = true;
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddMIPItem;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(enabled: true, uiFollowsEnabled: true);
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[11]
		{
			idPropertyGuid,
			GeneralPropertyDefinitions.GetNameProperty("", isSettable: true),
			MIPItemConfigurationProperties.FQIDKindProperty(kind, isSettable: true),
			MIPItemConfigurationProperties.FQIDParentIdProperty(parentId, isSettable: true),
			MIPItemConfigurationProperties.FQIDServerIdProperty(Guid.Empty, isSettable: true),
			MIPItemConfigurationProperties.FQIDSchemeProperty(Uri.UriSchemeHttp, isSettable: true),
			MIPItemConfigurationProperties.FQIDHostProperty("", isSettable: true),
			MIPItemConfigurationProperties.FQIDPortProperty(80, isSettable: true),
			MIPItemConfigurationProperties.FQIDTypeProperty("", isSettable: true),
			MIPItemConfigurationProperties.GisPointProperty("", isSettable: true),
			MIPItemConfigurationProperties.EncryptProperty(propertyValue: false)
		};
		return configurationItem;
	}

	private static ConfigurationItem MIPItemDataVersionSum(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		int num = 0;
		foreach (MIPKind kind in MIPKind.GetKinds(requestContext))
		{
			num += kind.DataVersion;
		}
		return ConvertToMIPItemDateVersionSumInvokeResult(path, num);
	}

	private static ConfigurationItem CreateInvokeInfoGetMIPOption(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardGetMIPOptionProperties;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			MIPItemConfigurationProperties.OptionIdProperty(Guid.Empty, isSettable: true),
			MIPItemConfigurationProperties.OptionUserProperty(value: false)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoSetMIPOption(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardSetMIPOptionProperties;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[4]
		{
			MIPItemConfigurationProperties.OptionIdProperty(Guid.Empty, isSettable: true),
			MIPItemConfigurationProperties.OptionDataVersionProperty(0, isSettable: true),
			MIPItemConfigurationProperties.OptionXmlProperty("", isSettable: true),
			MIPItemConfigurationProperties.OptionUserProperty(value: false)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveMIPItem(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveMIPItem, "PropertyRemoveMIPItem", ConfigurationApiResources.PropertyRemoveMIPItem, item, members);
	}

	private static ConfigurationItem GetMIPOptionProperty(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		bool userPrivate = false;
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.OptionIdKey, out string value);
		if (!Guid.TryParse(value, out var result))
		{
			throw new ArgumentException("OptionId incorrect");
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.OptionUserKey, out string value2))
		{
			userPrivate = bool.Parse(value2);
		}
		if (MIPKind.GetKind(requestContext, result) == null)
		{
			MIPKind.SaveKind(requestContext, new MIPKind
			{
				Id = result,
				Name = "N/A",
				PluginId = result,
				PluginName = "N/A",
				KindType = "OPTION",
				DataVersion = 1,
				SecurityAction = "",
				LastModified = DateTime.UtcNow,
				ModifiedUser = requestContext.UserDomainAccountName
			});
		}
		return GetMIPOptionFromCustomSetting(requestContext, result, userPrivate);
	}

	private static ConfigurationItem GetMIPOptionFromCustomSetting(IGeminiRequestContext requestContext, Guid optionId, bool userPrivate)
	{
		CustomSetting customSetting = (userPrivate ? CustomSetting.GetCustomSettingDataUser(requestContext, optionId) : CustomSetting.GetCustomSettingDataGlobal(requestContext, optionId));
		if (customSetting != null)
		{
			ConfigurationItemPath path = new ConfigurationItemPathBuilder("MIPOption" + (userPrivate ? "User" : ""), optionId.ToString()).CreatePath();
			return ConvertToMIPOptionConfigurationItem(customSetting, path);
		}
		throw new PathNotFoundException("No data for optionId: " + optionId.ToString());
	}

	private static void ValidateMIPOptionSecurity(IGeminiRequestContext requestContext, Guid optionId)
	{
		string objectId = (string.IsNullOrEmpty((MIPKind.GetKind(requestContext, optionId) ?? throw new MIPKindNotFoundException()).SecurityAction) ? "/" : string.Format(CultureInfo.InvariantCulture, "/{0}", optionId).ToUpper(CultureInfo.InvariantCulture));
		try
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, optionId, "GENERIC_WRITE", objectId);
		}
		catch (VmoServerServiceException ex)
		{
			if (ex.ErrorCode == 60325)
			{
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
				return;
			}
			throw;
		}
	}

	private static ConfigurationItem SetMIPOptionProperty(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		bool flag = false;
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.OptionIdKey, out string value);
		if (!Guid.TryParse(value, out var result))
		{
			throw new ArgumentException("OptionId incorrect");
		}
		ValidateMIPOptionSecurity(requestContext, result);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.OptionDataVersionKey, out string value2);
		if (!int.TryParse(value2, out var result2))
		{
			throw new ArgumentException("DataVersion incorrect");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.OptionXmlKey, out string value3))
		{
			throw new ArgumentException("XML missing");
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue(MIPItemConfigurationProperties.OptionUserKey, out string value4))
		{
			flag = bool.Parse(value4);
		}
		MIPKind kind = MIPKind.GetKind(requestContext, result);
		if (kind == null)
		{
			throw new ArgumentException("No data for OptionId: " + value);
		}
		CustomSettingInternalData customSettingInternalData = new CustomSettingInternalData
		{
			CustomSettingDataXml = value3,
			DataVersion = result2
		};
		if (flag)
		{
			CustomSetting.SetCustomSettingDataUser(requestContext, result, customSettingInternalData, forceOverride: false);
		}
		else
		{
			CustomSetting.SetCustomSettingDataGlobal(requestContext, result, customSettingInternalData, forceOverride: false);
		}
		kind.DataVersion = result2;
		kind.ModifiedUser = requestContext.UserDomainAccountName;
		MIPKind.SaveKind(requestContext, kind);
		return MethodTypesHelper.CreateInvokeResult(item, item.Path, "Result", "Success", true);
	}

	private static ConfigurationItem AddMIPItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		string name;
		string serverScheme;
		string serverHostName;
		int serverPort;
		string serverType;
		Guid serverId;
		Guid parentId;
		Guid kind;
		Guid objectId;
		bool enabled;
		string gisPoint;
		bool encrypt;
		ValidateResult validateResult = ValidateMIPItemInternal(item, out name, out serverScheme, out serverHostName, out serverPort, out serverType, out serverId, out parentId, out kind, out objectId, out enabled, out gisPoint, out encrypt);
		if (!validateResult.ValidatedOk)
		{
			if (validateResult.ErrorResults != null && validateResult.ErrorResults.Length != 0)
			{
				throw new PropertyInvalidException(validateResult.ErrorResults[0].ErrorText, validateResult.ErrorResults[0].ErrorProperty);
			}
			throw new PropertyInvalidException();
		}
		if (objectId == LegacyLicensesObjectId || objectId == LegacyRegistrationsObjectId)
		{
			throw new ArgumentException("This object ID is already reserved.");
		}
		Item item2 = new Item(new ServerId(serverType, serverScheme, serverHostName, serverPort, serverId), parentId, objectId, name, FolderType.No, kind);
		item2.Enabled = enabled;
		item2.PositioningInformation = new PositioningInformation(gisPoint, 0.0, 0.0, 0.0);
		item2.Encrypt = encrypt;
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (!(property.Key == "Enabled") && !(property.Key == MIPItemConfigurationProperties.GisPointKey) && !property.Key.StartsWith("FQID.", StringComparison.OrdinalIgnoreCase) && !(property.Key == "Name") && !(property.Key == "Id"))
			{
				item2.Properties[property.Key] = property.Value;
			}
		}
		try
		{
			if (MIPItem.SaveItem(requestContext, item2))
			{
				MIPItemConfigurationApiSecurity.RegisterItem(requestContext, parentId, objectId, kind);
			}
			MIPKind kind2 = MIPKind.GetKind(requestContext, item2.FQID.Kind);
			bool hasPermissions = kind2.SecurityAction != null && kind2.SecurityAction.Any();
			ConfigurationItem configurationItem = ConvertToMIPItemConfigurationItem(path, item2, creating: false, hasPermissions);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (MIPKindNotFoundException)
		{
			throw new PropertyInvalidException("Kind is not defined: " + kind.ToString());
		}
		catch (DuplicateMIPItemObjectIdException)
		{
			throw new ArgumentException("This object ID is already in use.");
		}
		catch (Exception ex3)
		{
			throw new PropertyInvalidException(ex3.Message);
		}
	}

	private static ConfigurationItem RemoveMIPItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		List<Item> itemsByParent = MIPItem.GetItemsByParent(requestContext, new Guid(invokeMemberItemSelectionPath.Id));
		if (itemsByParent != null && itemsByParent.Count > 0)
		{
			throw new ArgumentException("Cannot remove Item that has children");
		}
		Guid objectId = new Guid(invokeMemberItemSelectionPath.Id);
		Item item2 = MIPItem.GetItem(requestContext, objectId);
		if (item2 == null)
		{
			throw new PathNotFoundException();
		}
		if (item2.FQID.Kind == LicenseKindId)
		{
			throw new NotSupportedException("Invalid kind.");
		}
		if (MIPItem.Delete(requestContext, item2))
		{
			MIPItemConfigurationApiSecurity.UnregisterItem(requestContext, objectId, item2.FQID.Kind);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if ((!(path.ItemType == "MIPKindFolder") || !(path.ParentItemType == "System")) && (!(path.ItemType == "MIPItemFolder") || !(path.ParentItemType == "MIPKind")) && (!(path.ItemType == "MIPItemFolder") || !(path.ParentItemType == "MIPItem")))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static void ThrowIfInvalidMIPKind(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "MIPKindFolder", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
			return;
		}
		if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
		if (!string.Equals(path.ItemType, "MIPKind", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid 'ItemType', expected 'MIPKind'. Path: " + path.Text);
		}
	}

	private static void ThrowIfInvalidMIPItem(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ItemType, "MIPItemFolder", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
			return;
		}
		if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
		if (!string.Equals(path.ItemType, "MIPItem", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid 'ItemType', expected 'MIPItem'. Path: " + path.Text);
		}
	}

	private static ValidateResult ValidateParentKindId(IGeminiRequestContext requestContext, Guid parentKindId)
	{
		if (parentKindId != Guid.Empty && MIPKind.GetKind(requestContext, parentKindId) == null)
		{
			GeminiTrace.Error($"MIPItemConfigurationApi.ValidateParentKindId: ParentKind not found for id {parentKindId}");
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = false;
			validateResult.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorProperty = MIPItemConfigurationProperties.ParentKindKey,
					ErrorText = $"ParentKind not found: {parentKindId}"
				}
			};
			return validateResult;
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ValidateResult ValidateSecurityModifications(IGeminiRequestContext requestContext, MIPKind mipKind, string newSecurityActions)
	{
		if (!string.IsNullOrEmpty(mipKind.SecurityAction) && (mipKind.SecurityAction.Length > newSecurityActions.Length || string.CompareOrdinal(mipKind.SecurityAction, 0, newSecurityActions, 0, mipKind.SecurityAction.Length) != 0))
		{
			GeminiTrace.Error($"MIPItemConfigurationApi.ValidateSecurityModifications: Modifying order of MIP SecurityAction list is not supported for already defined SecurityActions for Kind {mipKind.Id}");
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = false;
			validateResult.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorProperty = MIPItemConfigurationProperties.SecurityActionKey,
					ErrorText = "Modifying order of MIP SecurityAction list is not supported for already defined SecurityActions"
				}
			};
			return validateResult;
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ConfigurationItem ConvertToMIPKindConfigurationItem(ConfigurationItemPath parentPath, MIPKind mipKind, bool isSettable)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "MIPKind",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = mipKind.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, mipKind.Id.ToString()).Text;
		obj.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(mipKind.Id.ToString()),
			GeneralPropertyDefinitions.GetNameProperty(mipKind.Name, isSettable),
			MIPItemConfigurationProperties.PluginIdProperty(mipKind.PluginId, isSettable: false),
			MIPItemConfigurationProperties.PluginNameProperty(mipKind.PluginName, isSettable),
			MIPItemConfigurationProperties.KindTypeProperty(mipKind.KindType, isSettable),
			MIPItemConfigurationProperties.SecurityActionProperty(mipKind.SecurityAction ?? "".ToString(CultureInfo.InvariantCulture), isSettable),
			MIPItemConfigurationProperties.SecurityActionNamesProperty(mipKind.SecurityActionNames, isSettable),
			MIPItemConfigurationProperties.DisplayOnGisMapProperty(mipKind.DisplayOnGisMap),
			MIPItemConfigurationProperties.ParentKindProperty(mipKind.ParentKindId, isSettable),
			MIPItemConfigurationProperties.DataVersionProperty(mipKind.DataVersion),
			MIPItemConfigurationProperties.ModifiedUserProperty(mipKind.ModifiedUser),
			GeneralPropertyDefinitions.GetLastModifiedProperty(mipKind.LastModified)
		}.ToArray();
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		return obj;
	}

	private static ConfigurationItem ConvertToMIPItemConfigurationItem(ConfigurationItemPath parentPath, Item mipItem, bool creating, bool hasPermissions)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "MIPItem";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = mipItem.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, mipItem.FQID.ObjectId.ToString()).Text;
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(mipItem.Enabled, uiFollowsEnabled: true);
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(mipItem.FQID.ObjectId.ToString()));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(mipItem.Name, isSettable: true));
		list.Add(MIPItemConfigurationProperties.FQIDKindProperty(mipItem.FQID.Kind, creating));
		list.Add(MIPItemConfigurationProperties.FQIDParentIdProperty(mipItem.FQID.ParentId, isSettable: true));
		list.Add(MIPItemConfigurationProperties.FQIDServerIdProperty(mipItem.FQID.ServerId.Id, isSettable: true));
		list.Add(MIPItemConfigurationProperties.FQIDSchemeProperty(mipItem.FQID.ServerId.ServerScheme, isSettable: true));
		list.Add(MIPItemConfigurationProperties.FQIDHostProperty(mipItem.FQID.ServerId.ServerHostname, isSettable: true));
		list.Add(MIPItemConfigurationProperties.FQIDPortProperty(mipItem.FQID.ServerId.Serverport, isSettable: true));
		list.Add(MIPItemConfigurationProperties.FQIDTypeProperty(mipItem.FQID.ServerId.ServerType, isSettable: true));
		list.Add(MIPItemConfigurationProperties.GisPointProperty(mipItem.PositioningInformation?.ToString() ?? "", isSettable: true));
		list.Add(MIPItemConfigurationProperties.EncryptProperty(mipItem.Encrypt));
		foreach (string key in mipItem.Properties.Keys)
		{
			if (key != "Enabled" && key != "Disabled" && key != MIPItemConfigurationProperties.GisPointKey && key != MIPItemConfigurationProperties.EncryptKey)
			{
				list.Add(new Property(key, key, mipItem.Properties[key])
				{
					IsSettable = true
				});
			}
		}
		configurationItem.Properties = list.ToArray();
		List<string> list2 = new List<string>();
		if (hasPermissions)
		{
			list2.Add("ChangeSecurityPermissions");
		}
		configurationItem.MethodIds = list2.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateMIPItemFolderFromParent(ConfigurationItemPath parentPath)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "MIPItemFolder";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Group;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeMIPItemFolder;
		configurationItem.ParentPath = parentPath.Text;
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(parentPath.ItemType, parentPath.Id, configurationItem.ItemType);
		configurationItem.Path = configurationItemPath.Text;
		configurationItem.EnableProperty = null;
		configurationItem.Properties = new Property[0];
		configurationItem.ChildrenFilled = false;
		configurationItem.MethodIds = new string[2] { "AddMIPItem", "RemoveMIPItem" };
		return configurationItem;
	}

	private static ConfigurationItem CreateMIPItemFolder(ConfigurationItemPath path)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "MIPItemFolder";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Group;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeMIPItemFolder;
		configurationItem.ParentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id).Text;
		configurationItem.Path = path.Text;
		configurationItem.EnableProperty = null;
		configurationItem.Properties = new Property[0];
		configurationItem.ChildrenFilled = false;
		configurationItem.MethodIds = new string[2] { "AddMIPItem", "RemoveMIPItem" };
		return configurationItem;
	}

	private static ConfigurationItem ConvertToMIPOptionConfigurationItem(CustomSetting customSetting, ConfigurationItemPath path)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "MIPOption";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeMIPOptionProperties;
		configurationItem.Path = path.Text;
		configurationItem.ParentPath = "/MIPKindFolder";
		configurationItem.EnableProperty = null;
		configurationItem.Properties = new Property[3]
		{
			MIPItemConfigurationProperties.OptionIdProperty(customSetting.Id, isSettable: true),
			MIPItemConfigurationProperties.OptionDataVersionProperty(customSetting.DataVersion, isSettable: true),
			MIPItemConfigurationProperties.OptionXmlProperty(customSetting.CustomSettingDataXml, isSettable: true)
		};
		configurationItem.ChildrenFilled = true;
		configurationItem.Children = new ConfigurationItem[0];
		configurationItem.MethodIds = new string[0];
		return configurationItem;
	}

	private static ConfigurationItem ConvertToMIPItemDateVersionSumInvokeResult(ConfigurationItemPath path, int sum)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeResult";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.PropertyMIPItemDataVersionSum;
		configurationItem.Path = path.Text;
		configurationItem.ParentPath = "/MIPKindFolder";
		configurationItem.EnableProperty = null;
		configurationItem.Properties = new Property[1] { MIPItemConfigurationProperties.MIPItemDataVersionSumProperty(sum) };
		configurationItem.ChildrenFilled = true;
		configurationItem.Children = new ConfigurationItem[0];
		configurationItem.MethodIds = new string[0];
		return configurationItem;
	}
}
