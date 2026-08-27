using System;
using System.Globalization;
using System.Resources;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using VideoOS.Common.Integration;
using VideoOS.Licensing;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server;

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class ManagementServerService : GeminiWebServiceBase, IManagementServerService
{
	public ManagementServerService()
		: base(connect: false)
	{
	}

	public GenericObject2[] Execute(string functionName, GenericObject2[] parameters)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("Execute");
			methodInformation.AddParameter("functionName", functionName);
			methodInformation.AddArrayParameter("parameters", parameters);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(ObjectDispatcher2.GetObjectByFunctionName(functionName).Execute(clientVersion: GetClientVersionFromHeader(), requestContext: base.RequestContext, functionName: functionName, parameters: parameters));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 QueryObject2(string entityName, string id, string[] propertiesToLoad)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryObject2");
			methodInformation.AddParameter("entityName", entityName);
			methodInformation.AddParameter("id", id);
			methodInformation.AddArrayParameter("propertiesToLoad", propertiesToLoad);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(ObjectDispatcher2.GetObjectByEntityName(entityName).QueryObject2(clientVersion: GetClientVersionFromHeader(), requestContext: base.RequestContext, entityName: entityName, id: id, propertiesToLoad: propertiesToLoad));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QueryObjects(string entityName, string[] ids, string[] propertiesToLoad)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryObjects");
			methodInformation.AddParameter("entityName", entityName);
			methodInformation.AddArrayParameter("ids", ids);
			methodInformation.AddArrayParameter("propertiesToLoad", propertiesToLoad);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(ObjectDispatcher2.GetObjectByEntityName(entityName).QueryObjects(clientVersion: GetClientVersionFromHeader(), requestContext: base.RequestContext, ids: ids, propertiesToLoad: propertiesToLoad));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string[] QueryObjectCollectionIds(string entityName, string parentEntityName, string parentId, string[] propertiesToLoad)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryObjectCollectionIds");
			methodInformation.AddParameter("entityName", entityName);
			methodInformation.AddParameter("parentEntityName", parentEntityName);
			methodInformation.AddParameter("parentId", parentId);
			methodInformation.AddArrayParameter("propertiesToLoad", propertiesToLoad);
			EnterWebMethod(methodInformation);
			return ObjectDispatcher2.GetObjectByEntityName(entityName).QueryObjectCollectionIds(clientVersion: GetClientVersionFromHeader(), requestContext: base.RequestContext, entityName: entityName, parentEntityName: parentEntityName, parentId: parentId, propertiesToLoad: propertiesToLoad);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 CreateObject2(string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("CreateObject2");
			methodInformation.AddParameter("entityName", entityName);
			methodInformation.AddArrayParameter("properties", properties);
			methodInformation.AddArrayParameter("customProperties", customProperties);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(ObjectDispatcher2.GetObjectByEntityName(entityName).CreateObject2(properties: GenericObjectHelper2.DeserializeNestedProperties(properties), clientVersion: GetClientVersionFromHeader(), requestContext: base.RequestContext, entityName: entityName, customProperties: customProperties));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 UpdateObject2(GenericObject2 genericObject, CustomProperty[] customProperties)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdateObject2");
			methodInformation.AddParameter("genericObject", genericObject);
			methodInformation.AddArrayParameter("customProperties", customProperties);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(ObjectDispatcher2.GetObjectByEntityName(genericObject.EntityName).UpdateObject2(genericObject: GenericObjectHelper2.DeserializeNestedProperties(genericObject), clientVersion: GetClientVersionFromHeader(), requestContext: base.RequestContext, customProperties: customProperties));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeleteObject2(string entityName, string id)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteObject2");
			methodInformation.AddParameter("entityName", entityName);
			methodInformation.AddParameter("id", id);
			EnterWebMethod(methodInformation);
			ObjectDispatcher2.GetObjectByEntityName(entityName).DeleteObject2(clientVersion: GetClientVersionFromHeader(), requestContext: base.RequestContext, id: id);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ServerGroupOld[] QueryServerGroups(string groupStructureType, string parentGroupId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryServerGroups");
			methodInformation.AddParameter("groupStructureType", groupStructureType);
			methodInformation.AddParameter("parentGroupId", parentGroupId);
			EnterWebMethod(methodInformation);
			return ServerGroupOld.QueryServerGroups(base.RequestContext, groupStructureType, parentGroupId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ServerGroupOld CreateServerGroup(string name, string description, string structureType, string parentGroupId, ServerGroupOptions serverGroupOptions)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("CreateServerGroup");
			methodInformation.AddParameter("name", name);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("structureType", structureType);
			methodInformation.AddParameter("parentGroupId", parentGroupId);
			methodInformation.AddParameter("serverGroupOptions", serverGroupOptions);
			EnterWebMethod(methodInformation);
			ServerGroupOld serverGroupOld = new ServerGroupOld
			{
				Name = name,
				Description = description,
				StructureType = structureType,
				ParentId = parentGroupId,
				ServerGroupOptions = serverGroupOptions
			};
			return ServerGroupOld.CreateServerGroup(base.RequestContext, serverGroupOld);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public CustomProperty[] GetCustomProperties(string objectId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetCustomProperties");
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			Validation.CheckObjectId(ref objectId, "objectId", allowNullOrEmpty: false);
			return CustomProperty.GetCustomProperties(base.RequestContext, objectId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public CustomProperty GetCustomProperty(string objectId, string name)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetCustomProperty");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddParameter("name", name);
			EnterWebMethod(methodInformation);
			Validation.CheckObjectId(ref objectId, "objectId", allowNullOrEmpty: false);
			Validation.CheckName(name, "name");
			return CustomProperty.GetCustomProperty(base.RequestContext, objectId, name);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void UpdateCustomProperties(string objectId, CustomProperty[] customPropertyArray)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdateCustomProperties");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddArrayParameter("customPropertyArray", customPropertyArray);
			EnterWebMethod(methodInformation);
			Validation.CheckObjectId(ref objectId, "objectId", allowNullOrEmpty: false);
			CustomProperty.UpdateCustomProperties(base.RequestContext, objectId, customPropertyArray);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeleteCustomProperty(string objectId, string name)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteCustomProperty");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddParameter("name", name);
			EnterWebMethod(methodInformation);
			Validation.CheckObjectId(ref objectId, "objectId", allowNullOrEmpty: false);
			Validation.CheckName(name, "name");
			CustomProperty.DeleteCustomProperty(base.RequestContext, objectId, name);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void TriggerUserDefinedEvent2(string userDefinedEventId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("TriggerUserDefinedEvent2");
			methodInformation.AddParameter("userDefinedEventId", userDefinedEventId);
			EnterWebMethod(methodInformation);
			UserDefinedEvent.TriggerUserDefinedEvent(base.RequestContext, userDefinedEventId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void TriggerPluginEvent(Guid pluginEventId, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("TriggerPluginEvent");
			methodInformation.AddParameter("pluginEventId", pluginEventId);
			methodInformation.AddParameter("sourceId", sourceId);
			methodInformation.AddArrayParameter("metadataDeviceIds", metadataDeviceIds);
			methodInformation.AddArrayParameter("metadataDeviceGroupIds", metadataDeviceGroupIds);
			methodInformation.AddParameter("customData", customData);
			EnterWebMethod(methodInformation);
			PluginEvent.TriggerPluginEvent(base.RequestContext, pluginEventId, sourceId, metadataDeviceIds, metadataDeviceGroupIds, customData);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void TriggerPluginEventBulk(PluginEventInfo[] pluginEvents)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("TriggerPluginEventBulk");
			methodInformation.AddArrayParameter("pluginEvents", pluginEvents);
			EnterWebMethod(methodInformation);
			PluginEvent.TriggerPluginEventBulk(base.RequestContext, pluginEvents);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QuerySystemMonitorStateSourcesByIds(string[] ids)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QuerySystemMonitorStateSourcesByIds");
			methodInformation.AddArrayParameter("ids", ids);
			EnterWebMethod(methodInformation);
			return SystemMonitorStateSourceGeneric.QuerySystemMonitorStateSourcesByIds(base.RequestContext, ids);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QuerySystemMonitorStateSourcesByTypes(string[] types)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QuerySystemMonitorStateSourcesByTypes");
			methodInformation.AddArrayParameter("types", types);
			EnterWebMethod(methodInformation);
			return SystemMonitorStateSourceGeneric.QuerySystemMonitorStateSourcesByTypes(base.RequestContext, types);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QueryAllRuleSources()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryAllRuleSources");
			EnterWebMethod(methodInformation);
			return RuleSourceGeneric.QueryAllRuleSources(base.RequestContext);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void ChangePasswordBasicUser(string id, string newPassword)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ChangePasswordBasicUser");
			methodInformation.AddParameter("id", id);
			methodInformation.AddParameter("newPassword", newPassword);
			EnterWebMethod(methodInformation);
			BasicUserGeneric.ChangePasswordBasicUser(base.RequestContext, id, newPassword);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeleteSite(string siteId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteSite");
			methodInformation.AddParameter("siteId", siteId);
			EnterWebMethod(methodInformation);
			Site.DeleteSite(base.RequestContext, siteId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void SyncronizeSite(string siteId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SyncronizeSite");
			methodInformation.AddParameter("siteId", siteId);
			EnterWebMethod(methodInformation);
			Site.SyncronizeSite(base.RequestContext, siteId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void MarkInconsistent(string siteId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkInconsistent");
			methodInformation.AddParameter("siteId", siteId);
			EnterWebMethod(methodInformation);
			Site.MarkInconsistent(base.RequestContext, siteId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void TriggerFullSiteSynchronization()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("TriggerFullSiteSynchronization");
			EnterWebMethod(methodInformation);
			Site.TriggerFullSiteSynchronization(base.RequestContext);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public TaskOld[] EnableRecordingStorageEncryption(string recordingStorageId, EncryptionMethod encryptionMethod, string password)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("EnableRecordingStorageEncryption");
			methodInformation.AddParameter("recordingStorageId", recordingStorageId);
			methodInformation.AddParameter("encryptionMethod", encryptionMethod);
			methodInformation.AddParameter("password", password);
			EnterWebMethod(methodInformation);
			return TaskOld.EnableRecordingStorageEncryption(base.RequestContext, recordingStorageId, encryptionMethod, password);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public TaskOld[] DisableRecordingStorageEncryption(string recordingStorageId, string password)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("DisableRecordingStorageEncryption");
			methodInformation.AddParameter("recordingStorageId", recordingStorageId);
			methodInformation.AddParameter("password", password);
			EnterWebMethod(methodInformation);
			return TaskOld.DisableRecordingStorageEncryption(base.RequestContext, recordingStorageId, password);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void StopEdgeStorageRetrieval(string taskId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("StopEdgeStorageRetrieval");
			methodInformation.AddParameter("taskId", taskId);
			EnterWebMethod(methodInformation);
			Task.RequestStopTaskStopEdgeStorageRetrieval(base.RequestContext, taskId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void CleanupTask(string taskId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("CleanupTask");
			methodInformation.AddParameter("taskId", taskId);
			EnterWebMethod(methodInformation);
			Task.CleanupTask(base.RequestContext, taskId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool IsFeatureLicensed(string featureName)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("IsFeatureLicensed");
			methodInformation.AddParameter("featureName", featureName);
			EnterWebMethod(methodInformation);
			return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(base.RequestContext, featureName);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string GetFeatureParameterValue(string featureName, string parameterName)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetFeatureParameterValue");
			methodInformation.AddParameter("featureName", featureName);
			methodInformation.AddParameter("parameterName", parameterName);
			EnterWebMethod(methodInformation);
			return GeminiApplicationCore.FeatureCacheManager.GetFeatureParameterValue(base.RequestContext, featureName, parameterName);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VMOExceptionDetail GetLastAutomaticActivationError()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetLastAutomaticActivationError");
			EnterWebMethod(methodInformation);
			base.RequestContext.GetService<SecurityManagerService>().CheckApplicationReadPermission(base.RequestContext);
			using (ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture)))
			{
				int num = serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.GetLastAutomaticActivationError(), null, delegate(Exception ex2, Action ac)
				{
					new WcfWebServiceManagerExceptionHandler().HandleException(ex2, null);
					throw ex2;
				});
				try
				{
					if (num == 0)
					{
						return null;
					}
					HandleLicenseException((LicenseErrorNumber)num);
				}
				catch (FaultException<VMOExceptionDetail> ex)
				{
					return ex.Detail;
				}
			}
			return null;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void UpdateLicense(byte[] license, string ownerXml, string sitesInformation)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdateLicense");
			methodInformation.AddArrayParameter("license", license);
			methodInformation.AddParameter("ownerXml", ownerXml);
			methodInformation.AddParameter("sitesInformation", sitesInformation);
			EnterWebMethod(methodInformation);
			base.RequestContext.GetService<SecurityManagerService>().CheckApplicationWritePermission(base.RequestContext);
			using (ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture)))
			{
				serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
				{
					client.UpdateLicense(license, ownerXml, sitesInformation);
				}, null, delegate(Exception ex, Action ac)
				{
					if (ex is FaultException<LicenseExceptionFault> ex2)
					{
						switch ((LicenseErrorNumber)ex2.Detail.ErrorNumber)
						{
						case LicenseErrorNumber.Expired:
							throw HandleException(new LicenseExpiredException());
						case LicenseErrorNumber.LicenseDecryptFailure:
						case LicenseErrorNumber.InvalidLicenseFile:
							throw HandleException(new LicenseFileInvalidException());
						default:
							HandleFeatureError((FeatureError)ex2.Detail.FeatureErrorNumber);
							throw HandleException(new GeneralLicenseException(ex2.Detail.Message));
						}
					}
					new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
					throw ex;
				});
			}
			GeminiApplicationCore.FeatureCacheManager.Update(base.RequestContext);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void UpdateMipPluginsLicenseRequestPart(string license)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdateMipPluginsLicenseRequestPart");
			methodInformation.AddParameter("license", license);
			EnterWebMethod(methodInformation);
			License.UpdateMipPluginsLicenseRequestPart(base.RequestContext, license);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void QueueAutomaticActivation()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueueAutomaticActivation");
			EnterWebMethod(methodInformation);
			base.RequestContext.GetService<SecurityManagerService>().CheckApplicationWritePermission(base.RequestContext);
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
			{
				client.QueueAutomaticActivation();
			}, null, delegate(Exception ex, Action ac)
			{
				new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
				throw ex;
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void RefreshLicenseCache()
	{
		CheckConnect();
		try
		{
			base.RequestContext.GetService<SecurityManagerService>().CheckApplicationWritePermission(base.RequestContext);
			GeminiApplicationCore.FeatureCacheManager.Update(base.RequestContext);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public void SetSystemSetting(string name, string value)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SetSystemSetting");
			methodInformation.AddParameter("name", name);
			methodInformation.AddParameter("value", value);
			EnterWebMethod(methodInformation);
			SystemSetting.SetSystemSetting(base.RequestContext, name, value);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public string GetSystemSetting(string name)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetSystemSetting");
			methodInformation.AddParameter("name", name);
			EnterWebMethod(methodInformation);
			return SystemSetting.GetSystemSetting(base.RequestContext, name);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	private void HandleFeatureError(FeatureError featureError)
	{
		switch (featureError)
		{
		case FeatureError.NoError:
			return;
		case FeatureError.UnlicensedArchiveReducedFramerateFeature:
			throw HandleException(new UnlicensedArchiveReducedFramerateFeatureException());
		case FeatureError.UnlicensedTimeControlledAccessRightsFeature:
			throw HandleException(new UnlicensedTimeControlledAccessRightsFeatureException());
		case FeatureError.UnlicensedAxisOneClickFeature:
			throw HandleException(new UnlicensedAxisOneClickFeatureException());
		case FeatureError.UnlicensedFederatedSitesFeature:
			throw HandleException(new UnlicensedFederatedSitesFeatureException());
		case FeatureError.UnlicensedInterconnectedProductsFeature:
			throw HandleException(new UnlicensedInterconnectedProductsFeatureException());
		case FeatureError.UnlicensedLiveMultistreamingFeature:
			throw HandleException(new UnlicensedLiveMultistreamingFeatureException());
		case FeatureError.UnlicensedMulticastFeature:
			throw HandleException(new UnlicensedMulticastFeatureException());
		case FeatureError.UnlicensedMultistageVideoStorageFeature:
			throw HandleException(new UnlicensedMultistageVideoStorageFeatureException());
		case FeatureError.UnlicensedRecordingServerFailoverFeature:
			throw HandleException(new UnlicensedRecordingServerFailoverFeatureException());
		case FeatureError.UnlicensedRecordingStorageEncryptionFeature:
			throw HandleException(new UnlicensedRecordingStorageEncryptionFeatureException());
		case FeatureError.UnlicensedRecordingStorageSigningFeature:
			throw HandleException(new UnlicensedRecordingStorageSigningFeatureException());
		case FeatureError.UnlicensedRemoteConnectedServicesFeature:
			throw HandleException(new UnlicensedRemoteConnectedServicesFeatureException());
		case FeatureError.UnlicensedRuleBasedBookmarkingFeature:
			throw HandleException(new UnlicensedRuleBasedBookmarkingFeatureException());
		case FeatureError.UnlicensedBookmarkingFeature:
			throw HandleException(new UnlicensedBookmarkingFeatureException());
		case FeatureError.UnlicensedSmartClientProfilesFeature:
			throw HandleException(new UnlicensedSmartClientProfilesFeatureException());
		case FeatureError.UnlicensedSmartClientProfiles:
			throw HandleException(new UnlicensedSmartClientProfilesException());
		case FeatureError.UnlicensedSmartWallFeature:
			throw HandleException(new UnlicensedSmartWallFeatureException());
		case FeatureError.UnlicensedSnmpFeature:
			throw HandleException(new UnlicensedSnmpFeatureException());
		case FeatureError.UnlicensedEvidenceLockFeature:
			throw HandleException(new UnlicensedEvidenceLockFeatureException());
		case FeatureError.UnlicensedDiffAdminFeature:
			throw HandleException(new UnlicensedDiffAdminFeatureException());
		case FeatureError.UnlicensedEdgeStorageFeature:
			throw HandleException(new UnlicensedEdgeStorageFeatureException());
		case FeatureError.UnlicensedRuleBasedEdgeStorageFeature:
			throw HandleException(new UnlicensedRuleBasedEdgeStorageFeatureException());
		case FeatureError.UnlicensedRuleBasedPtzPriorityFeature:
			throw HandleException(new UnlicensedRuleBasedPtzPriorityFeatureException());
		case FeatureError.UnlicensedSecurityPtzPriorityFeature:
			throw HandleException(new UnlicensedSecurityPtzPriorityFeatureException());
		case FeatureError.UnlicensedAddToFederatedHierarchyFeature:
			throw HandleException(new UnlicensedAddToFederatedHierarchyFeatureException());
		case FeatureError.UnlicensedRecorders:
			throw HandleException(new UnlicensedRecordersException());
		case FeatureError.UnlicensedExtendedPtzAnonymousPtz:
			throw HandleException(new UnlicensedExtendedPtzAnonymousPtzFeatureException());
		case FeatureError.UnlicensedExtendedPtzLockedPreset:
			throw HandleException(new UnlicensedExtendedPtzLockedPresetFeatureException());
		}
		ResourceManager resourceManager = VmoResources.ResourceManager;
		int num = (int)featureError;
		VmoServerServiceException ex = VmoServerServiceException.CreateFromVmoErrorMessage(resourceManager.GetString("VMO" + num.ToString(CultureInfo.InvariantCulture)));
		throw HandleException(ex);
	}

	public void ResolveDataCollectorIdOnRegisteredServices(string[] serviceIds)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("ResolveDataCollectorIdOnRegisteredServices");
			methodInformation.AddArrayParameter("serviceIds", serviceIds);
			EnterWebMethod(methodInformation);
			if (!base.RequestContext.GetService<SecurityManagerService>().IsAdministrator(base.RequestContext))
			{
				throw new UnauthorizedAccessException(VmoResources.VMO61008);
			}
			Validation.CheckObjectIdArray(serviceIds, "serviceIds", allowNullOrEmptyIds: true);
			if (serviceIds != null)
			{
				foreach (string g in serviceIds)
				{
					RegisteredServer.UpdateDataCollectorsForRegisteredServers(base.RequestContext, new Guid(g), 0);
				}
			}
			DataCollectorOnRegisteredServicesManager.Instance.QueueResolve();
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 AttachSlaveSite2(string slaveSiteId, string slaveSiteAddressesXml, string slaveSiteVersion)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("AttachSlaveSite2");
			methodInformation.AddParameter("slaveSiteId", slaveSiteId);
			methodInformation.AddParameter("slaveSiteAddressesXml", slaveSiteAddressesXml);
			methodInformation.AddParameter("slaveSiteVersion", slaveSiteVersion);
			EnterWebMethod(methodInformation);
			return SitesGeneric.AttachSlaveSite(base.RequestContext, slaveSiteId, slaveSiteAddressesXml, slaveSiteVersion);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 AttachMasterSite2(string masterSiteId, string masterSiteAddressesXml, string masterSiteVersion)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("AttachMasterSite2");
			methodInformation.AddParameter("masterSiteId", masterSiteId);
			methodInformation.AddParameter("masterSiteAddressesXml", masterSiteAddressesXml);
			methodInformation.AddParameter("masterSiteVersion", masterSiteVersion);
			EnterWebMethod(methodInformation);
			return SitesGeneric.AttachMasterSite(base.RequestContext, masterSiteId, masterSiteAddressesXml, masterSiteVersion);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 AuthorizeMaster2()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("AuthorizeMaster2");
			EnterWebMethod(methodInformation);
			return SitesGeneric.AuthroizeMaster(base.RequestContext);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public int GetServerStatus()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetServerStatus");
			EnterWebMethod(methodInformation);
			return ServerInformationGeneric.GetServerStatus();
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 ApplyLayoutGeneric(string monitorId, string layoutId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ApplyLayoutGeneric");
			methodInformation.AddParameter("monitorId", monitorId);
			methodInformation.AddParameter("layoutId", layoutId);
			EnterWebMethod(methodInformation);
			return MonitorGeneric.ApplyLayout(base.RequestContext, monitorId, layoutId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 ApplyCamerasToMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ApplyCamerasToMonitorViewGeneric");
			methodInformation.AddParameter("monitorId", monitorId);
			methodInformation.AddArrayParameter("cameras", cameras);
			methodInformation.AddArrayParameter("cameraGroups", cameraGroups);
			methodInformation.AddParameter("fromPosition", fromPosition);
			EnterWebMethod(methodInformation);
			return MonitorGeneric.ApplyCamerasToMonitorView(base.RequestContext, monitorId, cameras, cameraGroups, fromPosition);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 ApplyCamerasAndLayoutToMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ApplyCamerasAndLayoutToMonitorViewGeneric");
			methodInformation.AddParameter("monitorId", monitorId);
			methodInformation.AddArrayParameter("cameras", cameras);
			methodInformation.AddArrayParameter("cameraGroups", cameraGroups);
			methodInformation.AddParameter("fromPosition", fromPosition);
			methodInformation.AddParameter("layoutId", layoutId);
			EnterWebMethod(methodInformation);
			return MonitorGeneric.ApplyCamerasAndLayoutToMonitorView(base.RequestContext, monitorId, cameras, cameraGroups, fromPosition, layoutId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 ApplyViewItemXmlsToMonitorViewGeneric(string monitorId, string[] viewItemXmls, int fromPosition)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ApplyViewItemXmlsToMonitorViewGeneric");
			methodInformation.AddParameter("monitorId", monitorId);
			methodInformation.AddArrayParameter("viewItemXmls", viewItemXmls);
			methodInformation.AddParameter("fromPosition", fromPosition);
			EnterWebMethod(methodInformation);
			return MonitorGeneric.ApplyViewItemXmlsToMonitorView(base.RequestContext, monitorId, viewItemXmls, fromPosition);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void ApplyMonitorStateToMonitorGeneric(string monitorId, string xmlState)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ApplyMonitorStateToMonitorGeneric");
			methodInformation.AddParameter("monitorId", monitorId);
			methodInformation.AddParameter("xmlState", xmlState);
			EnterWebMethod(methodInformation);
			MonitorGeneric.ApplyMonitorStateToMonitor(base.RequestContext, monitorId, xmlState);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 ApplyTextAndLayoutToMonitorViewGeneric(string monitorId, string messageToDisplay, int fromPosition, string layoutId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ApplyTextAndLayoutToMonitorViewGeneric");
			methodInformation.AddParameter("monitorId", monitorId);
			methodInformation.AddParameter("messageToDisplay", messageToDisplay);
			methodInformation.AddParameter("fromPosition", fromPosition);
			methodInformation.AddParameter("layoutId", layoutId);
			EnterWebMethod(methodInformation);
			return MonitorGeneric.ApplyTextAndLayoutToMonitorView(base.RequestContext, monitorId, messageToDisplay, fromPosition, layoutId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void RemoveCamerasFromMonitorViewGeneric(string monitorId, string[] cameras, string[] cameraGroups)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RemoveCamerasFromMonitorViewGeneric");
			methodInformation.AddParameter("monitorId", monitorId);
			methodInformation.AddArrayParameter("cameras", cameras);
			methodInformation.AddArrayParameter("cameraGroups", cameraGroups);
			EnterWebMethod(methodInformation);
			MonitorGeneric.RemoveCamerasFromMonitorView(base.RequestContext, monitorId, cameras, cameraGroups);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void ActivateVideoWallPreset(string videoWallPresetId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ActivateVideoWallPreset");
			methodInformation.AddParameter("videoWallPresetId", videoWallPresetId);
			EnterWebMethod(methodInformation);
			VideoWallPreset.ActivateVideoWallPreset(base.RequestContext, videoWallPresetId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 CreateViewGeneric(string parentId, ObjectProperty2[] viewProperties, CustomProperty[] viewCustomProperties, ObjectProperty2[][] viewItemsProperties, CustomProperty[][] viewItemsCustomProperties)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("CreateViewGeneric");
			methodInformation.AddParameter("parentId", parentId);
			methodInformation.AddArrayParameter("viewProperties", viewProperties);
			methodInformation.AddArrayParameter("CustomProperty", viewCustomProperties);
			methodInformation.AddArrayArrayParameter("viewItemsProperties", viewItemsProperties);
			methodInformation.AddArrayArrayParameter("viewItemsCustomProperties", viewItemsCustomProperties);
			EnterWebMethod(methodInformation);
			return ViewGeneric.CreateView(base.RequestContext, parentId, viewProperties, viewCustomProperties, viewItemsProperties, viewItemsCustomProperties);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 ClearViewGeneric(string viewId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ClearViewGeneric");
			methodInformation.AddParameter("viewId", viewId);
			EnterWebMethod(methodInformation);
			return ViewGeneric.ClearView(base.RequestContext, viewId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 ApplyViewToViewGeneric(string sourceViewId, string destinationViewId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ApplyViewToViewGeneric");
			methodInformation.AddParameter("sourceViewId", sourceViewId);
			methodInformation.AddParameter("destinationViewId", destinationViewId);
			EnterWebMethod(methodInformation);
			return ViewGeneric.ApplyViewToView(base.RequestContext, sourceViewId, destinationViewId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 ClearViewItemGeneric(string viewItemId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ClearViewItemGeneric");
			methodInformation.AddParameter("viewItemId", viewItemId);
			EnterWebMethod(methodInformation);
			return ViewItemGeneric.ClearViewItem(base.RequestContext, viewItemId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void ChangeViewItemId(string existingId, string newId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ChangeViewItemId");
			methodInformation.AddParameter("existingId", existingId);
			methodInformation.AddParameter("newId", newId);
			EnterWebMethod(methodInformation);
			Validation.CheckObjectId(ref existingId, "existingId", allowNullOrEmpty: false);
			Validation.CheckObjectId(ref newId, "newId", allowNullOrEmpty: false);
			View.GetIdFromItem(base.RequestContext, existingId);
			ViewItem.ChangeViewItemId(base.RequestContext, existingId, newId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 GetRecordingServerInformation(string recordingServerId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetRecordingServerInformation");
			methodInformation.AddParameter("recordingServerId", recordingServerId);
			EnterWebMethod(methodInformation);
			return RecordingServerInformationGeneric.GetRecordingServerInformation(base.RequestContext, recordingServerId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 GetRecordingStorageInformation(string recordingStorageId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetRecordingStorageInformation");
			methodInformation.AddParameter("recordingStorageId", recordingStorageId);
			EnterWebMethod(methodInformation);
			return StorageInformationGeneric.GetRecordingStorageInformation(base.RequestContext, recordingStorageId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 GetArchiveStorageInformation(string archiveStorageId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetArchiveStorageInformation");
			methodInformation.AddParameter("archiveStorageId", archiveStorageId);
			EnterWebMethod(methodInformation);
			return StorageInformationGeneric.GetArchiveStorageInformation(base.RequestContext, archiveStorageId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void SetRecordingStorageAsDefault(string recordingStorageId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SetRecordingStorageAsDefault");
			methodInformation.AddParameter("recordingStorageId", recordingStorageId);
			EnterWebMethod(methodInformation);
			RecordingStorageGeneric.SetRecordingStorageAsDefault(base.RequestContext, recordingStorageId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool ValidateEncryptionPassword(string recordingStorageId, string password)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ValidateEncryptionPassword");
			methodInformation.AddParameter("recordingStorageId", recordingStorageId);
			methodInformation.AddParameter("password", password);
			EnterWebMethod(methodInformation);
			return RecordingStorageGeneric.ValidateEncryptionPassword(base.RequestContext, recordingStorageId, password);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void EnableFramerateReduction(string archiveStorageId, bool enable)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("EnableFramerateReduction");
			methodInformation.AddParameter("archiveStorageId", archiveStorageId);
			methodInformation.AddParameter("enable", enable);
			EnterWebMethod(methodInformation);
			ArchiveStorageGeneric.EnableFramerateReduction(base.RequestContext, archiveStorageId, enable);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public MarkedDataReference2 QueryMarkedDataReference(string[] deviceIds, bool realTime, MarkedDataDefinitions.MarkedDataTypes markedDataTypes)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryMarkedDataReference");
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("realTime", realTime);
			methodInformation.AddParameter("markedDataTypes", markedDataTypes);
			EnterWebMethod(methodInformation);
			return MarkedDataGeneric.QueryMarkedDataReference(base.RequestContext, deviceIds, realTime, markedDataTypes);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 QueryMarkedData(string markedDataId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryMarkedData");
			methodInformation.AddParameter("markedDataId", markedDataId);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(MarkedDataGeneric.QueryMarkedDataById(base.RequestContext, markedDataId));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QueryBookmarksSearch(DateTime centerTime, DateTime timeLimit, int countLimit, string[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, string optFromBookmarkId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryBookmarksSearch");
			methodInformation.AddParameter("centerTime", centerTime);
			methodInformation.AddParameter("timeLimit", timeLimit);
			methodInformation.AddParameter("countLimit", countLimit);
			methodInformation.AddArrayParameter("deviceTypes", deviceTypes);
			methodInformation.AddArrayParameter("optDeviceIds", optDeviceIds);
			methodInformation.AddArrayParameter("optUsers", optUsers);
			methodInformation.AddParameter("optSearchStr", optSearchStr);
			methodInformation.AddParameter("optFromBookmarkId", optFromBookmarkId);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(MarkedDataGeneric.QueryBookmarksSearch(base.RequestContext, centerTime, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, optFromBookmarkId));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QueryBookmarksSearch2(DateTime centerTime, DateTime timeLimit, int countLimit, string[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string optFromBookmarkId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryBookmarksSearch2");
			methodInformation.AddParameter("centerTime", centerTime);
			methodInformation.AddParameter("timeLimit", timeLimit);
			methodInformation.AddParameter("countLimit", countLimit);
			methodInformation.AddArrayParameter("deviceTypes", deviceTypes);
			methodInformation.AddArrayParameter("optDeviceIds", optDeviceIds);
			methodInformation.AddArrayParameter("optUsers", optUsers);
			methodInformation.AddParameter("optSearchStr", optSearchStr);
			methodInformation.AddParameter("searchHeader", searchHeader);
			methodInformation.AddParameter("searchDescription", searchDescription);
			methodInformation.AddParameter("searchReference", searchReference);
			methodInformation.AddParameter("searchUser", searchUser);
			methodInformation.AddParameter("headerFilter", headerFilter);
			methodInformation.AddParameter("descriptionFilter", descriptionFilter);
			methodInformation.AddParameter("optFromBookmarkId", optFromBookmarkId);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(MarkedDataGeneric.QueryBookmarksSearch2(base.RequestContext, centerTime, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, optFromBookmarkId));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QueryMarkedDataSearch(string[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryMarkedDataSearch");
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("searchText", searchText);
			methodInformation.AddArrayParameter("users", users);
			methodInformation.AddParameter("createdFromTime", createdFromTime);
			methodInformation.AddParameter("createdToTime", createdToTime);
			methodInformation.AddParameter("markedDataFromTime", markedDataFromTime);
			methodInformation.AddParameter("markedDataToTime", markedDataToTime);
			methodInformation.AddParameter("tagTimeFromTime", tagTimeFromTime);
			methodInformation.AddParameter("tagTimeToTime", tagTimeToTime);
			methodInformation.AddParameter("retentionExpireFromTime", retentionExpireFromTime);
			methodInformation.AddParameter("retentionExpireToTime", retentionExpireToTime);
			methodInformation.AddParameter("pageIndex", pageIndex);
			methodInformation.AddParameter("pageSize", pageSize);
			methodInformation.AddParameter("sortOrder", sortOrder);
			methodInformation.AddParameter("sortAscending", sortAscending);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(MarkedDataGeneric.QueryMarkedDataSearch(base.RequestContext, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFromTime, retentionExpireToTime, pageIndex, pageSize, sortOrder, sortAscending));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QueryMarkedDataSearch2(string[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryMarkedDataSearch2");
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("searchText", searchText);
			methodInformation.AddParameter("searchHeader", searchHeader);
			methodInformation.AddParameter("searchDescription", searchDescription);
			methodInformation.AddParameter("searchReference", searchReference);
			methodInformation.AddParameter("searchUser", searchUser);
			methodInformation.AddParameter("headerFilter", headerFilter);
			methodInformation.AddParameter("descriptionFilter", descriptionFilter);
			methodInformation.AddArrayParameter("users", users);
			methodInformation.AddParameter("createdFromTime", createdFromTime);
			methodInformation.AddParameter("createdToTime", createdToTime);
			methodInformation.AddParameter("markedDataFromTime", markedDataFromTime);
			methodInformation.AddParameter("markedDataToTime", markedDataToTime);
			methodInformation.AddParameter("tagTimeFromTime", tagTimeFromTime);
			methodInformation.AddParameter("tagTimeToTime", tagTimeToTime);
			methodInformation.AddParameter("retentionExpireFromTime", retentionExpireFromTime);
			methodInformation.AddParameter("retentionExpireToTime", retentionExpireToTime);
			methodInformation.AddParameter("pageIndex", pageIndex);
			methodInformation.AddParameter("pageSize", pageSize);
			methodInformation.AddParameter("sortOrder", sortOrder);
			methodInformation.AddParameter("sortAscending", sortAscending);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(MarkedDataGeneric.QueryMarkedDataSearch2(base.RequestContext, deviceIds, searchText, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFromTime, retentionExpireToTime, pageIndex, pageSize, sortOrder, sortAscending));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 CreateMarkedData(ObjectProperty2[] properties)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("CreateMarkedData");
			methodInformation.AddArrayParameter("properties", properties);
			EnterWebMethod(methodInformation);
			return GenericObjectHelper2.SerializeNestedProperties(MarkedDataGeneric.CreateMarkedData(base.RequestContext, properties));
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 UpdateMarkedData(ObjectProperty2[] properties)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdateMarkedData");
			methodInformation.AddArrayParameter("properties", properties);
			EnterWebMethod(methodInformation);
			GenericObject2 obj = MarkedDataGeneric.UpdateMarkedData(base.RequestContext, properties);
			return GenericObjectHelper2.SerializeNestedProperties(obj);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeleteMarkedData(string markedDataId, MarkedDataDefinitions.MarkedDataTypes markedDataType)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteMarkedData");
			methodInformation.AddParameter("markedDataId", markedDataId);
			methodInformation.AddParameter("markedDataType", markedDataType);
			EnterWebMethod(methodInformation);
			MarkedDataGeneric.DeleteMarkedData(base.RequestContext, markedDataId, markedDataType);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteBookmarksByDeviceIdBeforeTime");
			methodInformation.AddParameter("mediaStatus", mediaStatus);
			EnterWebMethod(methodInformation);
			MarkedDataGeneric.DeleteBookmarksByDeviceIdBeforeTime(base.RequestContext, mediaStatus);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void EnableFailoverServer(string failoverServerId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("EnableFailoverServer");
			methodInformation.AddParameter("failoverServerId", failoverServerId);
			EnterWebMethod(methodInformation);
			FailoverServerGeneric.SetEnabled(base.RequestContext, failoverServerId, enable: true);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DisableFailoverServer(string failoverServerId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("DisableFailoverServer");
			methodInformation.AddParameter("failoverServerId", failoverServerId);
			EnterWebMethod(methodInformation);
			FailoverServerGeneric.SetEnabled(base.RequestContext, failoverServerId, enable: false);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void StopFailoverService(string failoverServerId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("StopFailoverService");
			methodInformation.AddParameter("failoverServerId", failoverServerId);
			EnterWebMethod(methodInformation);
			throw new NotSupportedException();
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void MoveFailoverServer(string failoverServerId, string newParentId, int insertAtPosition)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MoveFailoverServer");
			methodInformation.AddParameter("failoverServerId", failoverServerId);
			methodInformation.AddParameter("newParentId", newParentId);
			methodInformation.AddParameter("insertAtPosition", insertAtPosition);
			EnterWebMethod(methodInformation);
			FailoverServerGeneric.Move(base.RequestContext, failoverServerId, newParentId, insertAtPosition);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QueryRecordingServerHardwareDrivers(string recordingServerId, bool used)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryRecordingServerHardwareDrivers");
			methodInformation.AddParameter("recordingServerId", recordingServerId);
			methodInformation.AddParameter("used", used);
			EnterWebMethod(methodInformation);
			return HardwareDriverGeneric.QueryRecordingServerHardwareDrivers(base.RequestContext, recordingServerId, used);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QueryFailoverServerHardwareDrivers(string failoverServerId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryFailoverServerHardwareDrivers");
			methodInformation.AddParameter("failoverServerId", failoverServerId);
			EnterWebMethod(methodInformation);
			return HardwareDriverGeneric.QueryFailoverServerHardwareDrivers(base.RequestContext, failoverServerId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2[] QueryActiveRecordingServerHardwareDrivers(string recordingServerId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryActiveRecordingServerHardwareDrivers");
			methodInformation.AddParameter("recordingServerId", recordingServerId);
			EnterWebMethod(methodInformation);
			return HardwareDriverGeneric.QueryActiveRecordingServerHardwareDrivers(base.RequestContext, recordingServerId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GenericObject2 QueryActiveHardwareDriver(string recordingServerId, string driverId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryActiveHardwareDriver");
			methodInformation.AddParameter("recordingServerId", recordingServerId);
			methodInformation.AddParameter("driverId", driverId);
			EnterWebMethod(methodInformation);
			return HardwareDriverGeneric.QueryActiveHardwareDriver(base.RequestContext, recordingServerId, driverId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeleteServerTasksByDeviceIdBeforeTime(string deviceId, DateTime time)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteServerTasksByDeviceIdBeforeTime");
			methodInformation.AddParameter("deviceId", deviceId);
			methodInformation.AddParameter("time", time);
			EnterWebMethod(methodInformation);
			TaskGeneric.DeleteTasksByDeviceBeforeDt(base.RequestContext, deviceId, time);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void ClientProfileMovePriorityUp(string id)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ClientProfileMovePriorityUp");
			methodInformation.AddParameter("id", id);
			EnterWebMethod(methodInformation);
			ClientProfileGeneric.MovePriorityUp(base.RequestContext, id);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void ClientProfileMovePriorityDown(string id)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ClientProfileMovePriorityDown");
			methodInformation.AddParameter("id", id);
			EnterWebMethod(methodInformation);
			ClientProfileGeneric.MovePriorityDown(base.RequestContext, id);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string QueryActiveManagementClientProfileId()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryActiveManagementClientProfileId");
			EnterWebMethod(methodInformation);
			return ClientProfileGeneric.QueryActiveManagementClientProfileId(base.RequestContext);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string QueryActiveSmartClientProfileId()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryActiveSmartClientProfileId");
			EnterWebMethod(methodInformation);
			return ClientProfileGeneric.QueryActiveSmartClientProfileId(base.RequestContext);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string QueryActiveSmartClientProfileIdBySid(string sid)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryActiveSmartClientProfileIdBySid");
			methodInformation.AddParameter("sid", sid);
			EnterWebMethod(methodInformation);
			return ClientProfileGeneric.QueryActiveSmartClientProfileId(base.RequestContext, sid);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void AttachClientProfileToRole(string id, string roleId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("AttachClientProfileToRole");
			methodInformation.AddParameter("id", id);
			methodInformation.AddParameter("roleId", roleId);
			EnterWebMethod(methodInformation);
			ClientProfileGeneric.AttachClientProfileToRole(base.RequestContext, id, roleId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	private void HandleLicenseException(LicenseErrorNumber errorNumber)
	{
		switch (errorNumber)
		{
		case LicenseErrorNumber.Expired:
			throw HandleException(new LicenseExpiredException());
		case LicenseErrorNumber.LicenseDecryptFailure:
		case LicenseErrorNumber.InvalidLicenseFile:
			throw HandleException(new LicenseFileInvalidException());
		case LicenseErrorNumber.GeneralLicenseError:
			throw HandleException(new GeneralLicenseException());
		case LicenseErrorNumber.LicenseActivationServerCommunicationFailed:
			throw HandleException(VmoExceptionFactory.VMO64058_LicenseActivationException());
		case LicenseErrorNumber.UserNameAndPasswordNotFound:
			throw HandleException(new LicenseUserNameAndPasswordNotFoundException());
		case LicenseErrorNumber.SlcNotFound:
			throw HandleException(VmoExceptionFactory.VMO64007_LicenseActivationException());
		case LicenseErrorNumber.SlcRegisteredOnOtherLogOn:
			throw HandleException(VmoExceptionFactory.VMO64008_LicenseActivationException());
		case LicenseErrorNumber.MismatchOnComputerSid:
			throw HandleException(VmoExceptionFactory.VMO64009_LicenseActivationException());
		case LicenseErrorNumber.AccessDenied:
			throw HandleException(VmoExceptionFactory.VMO64050_LicenseActivationException());
		case LicenseErrorNumber.TooManyCamerasRequested:
			throw HandleException(VmoExceptionFactory.VMO64051_LicenseActivationException());
		case LicenseErrorNumber.TooManyHardwareDevicesRequested:
			throw HandleException(VmoExceptionFactory.VMO64052_LicenseActivationException());
		case LicenseErrorNumber.TooManyInterconnectedCamerasRequested:
			throw HandleException(VmoExceptionFactory.VMO64053_LicenseActivationException(OemResources.Instance.Server.ICPApplicationName));
		case LicenseErrorNumber.TooManyRecordersRequested:
			throw HandleException(VmoExceptionFactory.VMO64054_LicenseActivationException());
		case LicenseErrorNumber.SlcNotRegistered:
			throw HandleException(VmoExceptionFactory.VMO64055_LicenseActivationException());
		case LicenseErrorNumber.SlcAlreadyInUse:
			throw HandleException(VmoExceptionFactory.VMO64056_LicenseActivationException());
		case LicenseErrorNumber.ActivationLimitExceeded:
			throw HandleException(VmoExceptionFactory.VMO64057_LicenseActivationException());
		case LicenseErrorNumber.CustomerIsNotCdsValidated:
			throw HandleException(VmoExceptionFactory.VMO64059_LicenseActivationException());
		}
	}

	private static int GetClientVersionFromHeader()
	{
		MessageHeaders incomingMessageHeaders = OperationContext.Current.IncomingMessageHeaders;
		int num = incomingMessageHeaders.FindHeader("VMO_Client_Version", "http://schemas.videoos/ManagementServerService/2008/06/Vmo/01");
		if (num != -1)
		{
			return incomingMessageHeaders.GetHeader<int>(num);
		}
		return 0;
	}
}
