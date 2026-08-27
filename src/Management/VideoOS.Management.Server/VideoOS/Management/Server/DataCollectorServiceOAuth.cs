using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using VideoOS.IdentityServer.Common.ServiceModel;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
[ScopeRequirement("managementserver")]
public class DataCollectorServiceOAuth : GeminiWebServiceBase, IDataCollection
{
	public DataCollectorServiceOAuth()
		: base(connect: false)
	{
	}

	public int RegisterDataCollector(string applicationId, string hostName)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterDataCollector");
			methodInformation.AddParameter("applicationId", applicationId);
			methodInformation.AddParameter("hostName", hostName);
			EnterWebMethod(methodInformation);
			Validation.CheckName(applicationId, "applicationId");
			Validation.CheckName(hostName, "hostName");
			int result = DataCollector.RegisterDataCollector(base.RequestContext, applicationId, hostName, 1);
			DataCollectorOnRegisteredServicesManager.Instance.QueueResolve();
			return result;
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

	public int RegisterDataCollectorWithVersion(string applicationId, string hostName, int version)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterDataCollectorWithVersion");
			methodInformation.AddParameter("applicationId", applicationId);
			methodInformation.AddParameter("hostName", hostName);
			methodInformation.AddParameter("version", version);
			EnterWebMethod(methodInformation);
			Validation.CheckName(applicationId, "applicationId");
			Validation.CheckName(hostName, "hostName");
			int result = DataCollector.RegisterDataCollector(base.RequestContext, applicationId, hostName, version);
			DataCollectorOnRegisteredServicesManager.Instance.QueueResolve();
			return result;
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

	public int RegisterDataCollectorNewApplicationId(string applicationId, string oldApplicationId, string hostName, int version)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterDataCollectorNewApplicationId");
			methodInformation.AddParameter("applicationId", applicationId);
			methodInformation.AddParameter("oldApplicationId", oldApplicationId);
			methodInformation.AddParameter("hostName", hostName);
			methodInformation.AddParameter("version", version);
			EnterWebMethod(methodInformation);
			Validation.CheckName(applicationId, "applicationId");
			Validation.CheckName(oldApplicationId, "oldApplicationId");
			Validation.CheckName(hostName, "hostName");
			int result = DataCollector.RegisterDataCollectorNewApplicationId(base.RequestContext, applicationId, oldApplicationId, hostName, version);
			DataCollectorOnRegisteredServicesManager.Instance.QueueResolve();
			return result;
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

	public DataCollectorConfiguration GetDataCollectorConfiguration(int dataCollectorId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDataCollectorConfiguration");
			methodInformation.AddParameter("dataCollectorId", dataCollectorId);
			EnterWebMethod(methodInformation);
			return DataCollector.GetDataCollectorConfiguration(base.RequestContext, dataCollectorId);
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

	public void AddPerformanceCounterData(PerformanceCounterData counterData)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("AddPerformanceCounterData");
			methodInformation.AddParameter("counterData", counterData);
			EnterWebMethod(methodInformation);
			DataCollector.AddPerformanceCounterData(base.RequestContext, counterData);
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

	public void AddPerformanceCounterDataWithConfigRefresh(PerformanceCounterData counterData)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("AddPerformanceCounterDataWithConfigRefresh");
			methodInformation.AddParameter("counterData", counterData);
			EnterWebMethod(methodInformation);
			DataCollector.AddPerformanceCounterDataWithConfigRefresh(base.RequestContext, counterData);
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

	public void MarkPerformanceCounterDataAsObsolete(string[] instances)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkPerformanceCounterDataAsObsolete");
			methodInformation.AddArrayParameter("instances", instances);
			EnterWebMethod(methodInformation);
			DataCollector.MarkPerformanceCounterDataAsObsolete(base.RequestContext, instances);
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

	public ServiceApplication[] GetServiceApplicationList(int dataCollectorId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetServiceApplicationList");
			methodInformation.AddParameter("dataCollectorId", dataCollectorId);
			EnterWebMethod(methodInformation);
			return DataCollector.GetServiceApplicationList(base.RequestContext, dataCollectorId);
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

	public void RegisterServiceApplication(int dataCollectorId, ServiceApplication[] serviceApplications)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterServiceApplication");
			methodInformation.AddParameter("dataCollectorId", dataCollectorId);
			methodInformation.AddArrayParameter("serviceApplications", serviceApplications);
			EnterWebMethod(methodInformation);
			DataCollector.RegisterServiceApplications(base.RequestContext, dataCollectorId, serviceApplications);
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

	public void UpdateMachineProperties(int dataCollectorId, MachineProperty[] machineProperties)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdateMachineProperties");
			methodInformation.AddParameter("dataCollectorId", dataCollectorId);
			methodInformation.AddArrayParameter("serviceApplications", machineProperties);
			EnterWebMethod(methodInformation);
			DataCollector.UpdateMachineProperties(base.RequestContext, dataCollectorId, machineProperties);
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

	public CounterStateRulesConfiguration GetCounterStateRulesConfiguration(int dataCollectorId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetCounterStateRulesConfiguration");
			methodInformation.AddParameter("dataCollectorId", dataCollectorId);
			EnterWebMethod(methodInformation);
			return DataCollector.GetCountersStateRulesConfiguration(base.RequestContext, dataCollectorId);
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

	public void RegisterStateChangeData(CounterStateChangeData data)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterStateChangeData");
			methodInformation.AddParameter("data", data);
			EnterWebMethod(methodInformation);
			DataCollector.RegisterStateChangeData(base.RequestContext, data);
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

	public void DeleteStateData(CounterStateChangeData data)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteStateData");
			methodInformation.AddParameter("data", data);
			EnterWebMethod(methodInformation);
			DataCollector.DeleteStateData(base.RequestContext, data);
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

	public void DeleteStateDataFromDataCollector(int datacollectorId, string[] activePaths)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteStateDataFromDataCollector");
			methodInformation.AddParameter("datacollectorId", datacollectorId);
			methodInformation.AddParameter("activePaths", activePaths);
			EnterWebMethod(methodInformation);
			DataCollector.DeleteStateDataFromDatacollector(base.RequestContext, datacollectorId, activePaths);
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

	public HistoricSample[] GetHistoricSamples(HistoricSampleRequestData data)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetHistoricSamples");
			methodInformation.AddParameter("data", data);
			EnterWebMethod(methodInformation);
			return DataCollector.QueryHistoricSamples(base.RequestContext, data);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}
}
