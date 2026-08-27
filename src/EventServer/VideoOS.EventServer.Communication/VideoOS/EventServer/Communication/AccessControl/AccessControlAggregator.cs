using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using VideoOS.EventServer.Communication.AccessControl.Client;
using VideoOS.EventServer.Shared.Aggregation;
using VideoOS.Platform;
using VideoOS.Platform.AccessControl.Internal.Data;

namespace VideoOS.EventServer.Communication.AccessControl;

public class AccessControlAggregator
{
	private AccessControlClientManager _manager;

	public AccessControlAggregator(AccessControlClientManager manager)
	{
		_manager = manager;
	}

	protected AccessControlClient GetClient(ServerId serverId)
	{
		return _manager.GetClient(serverId) ?? throw new ArgumentException("No AccessControlClient for serverId " + serverId?.ToString() + " found");
	}

	public async Task<SyntheticResponse<GetAccessControlEventsResp>> AggregateSyntheticGetAccessControlEventsAsync(IEnumerable<ServerId> serverIds, GetAccessControlEventsParams parameters)
	{
		AggregationResult<ServerId, GetAccessControlEventsResp> aggregationResult = await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlEventsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
		List<AccessControlEventLine> list = new List<AccessControlEventLine>();
		foreach (KeyValuePair<ServerId, GetAccessControlEventsResp> properResult in aggregationResult.ProperResults)
		{
			list.AddRange(properResult.Value.Result);
		}
		IEnumerable<AccessControlEventLine> source = AggregateOrderAccessControlEvents(list, parameters.SearchOrders).Skip(parameters.Page * parameters.PageSize).Take(parameters.PageSize);
		return new SyntheticResponse<GetAccessControlEventsResp>
		{
			AggregationResult = aggregationResult,
			Response = new GetAccessControlEventsResp
			{
				SearchId = parameters.SearchId,
				Result = source.ToArray()
			}
		};
	}

	public async Task<SyntheticResponse<GetAccessControlEventsWithDowntimesResp>> AggregateSyntheticGetAccessControlEventsWithDowntimesAsync(IEnumerable<ServerId> serverIds, GetAccessControlEventsWithDowntimesParams parameters)
	{
		AggregationResult<ServerId, GetAccessControlEventsWithDowntimesResp> aggregationResult = await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlEventsWithDowntimesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
		List<AccessControlEventLine> list = new List<AccessControlEventLine>();
		List<AccessControlDowntimeData> list2 = new List<AccessControlDowntimeData>();
		foreach (KeyValuePair<ServerId, GetAccessControlEventsWithDowntimesResp> properResult in aggregationResult.ProperResults)
		{
			list.AddRange(properResult.Value.Result);
			list2.AddRange(properResult.Value.DowntimeData);
		}
		IEnumerable<AccessControlEventLine> source = AggregateOrderAccessControlEvents(list, parameters.SearchOrders).Skip(parameters.Page * parameters.PageSize).Take(parameters.PageSize);
		SyntheticResponse<GetAccessControlEventsWithDowntimesResp> obj = new SyntheticResponse<GetAccessControlEventsWithDowntimesResp>
		{
			AggregationResult = aggregationResult
		};
		GetAccessControlEventsWithDowntimesResp getAccessControlEventsWithDowntimesResp = (obj.Response = new GetAccessControlEventsWithDowntimesResp
		{
			SearchId = parameters.SearchId,
			Result = source.ToArray()
		});
		getAccessControlEventsWithDowntimesResp.DowntimeData = Enumerable.ToArray(list2);
		return obj;
	}

	public static IEnumerable<AccessControlEventLine> AggregateOrderAccessControlEvents(IEnumerable<AccessControlEventLine> events, SearchOrders searchOrders)
	{
		IQueryable<AccessControlEventLine> queryable = events.AsQueryable();
		Expression expression = queryable.Expression;
		bool flag = true;
		if (searchOrders != null)
		{
			SearchOrder[] orders = searchOrders.Orders;
			foreach (SearchOrder searchOrder in orders)
			{
				string methodName = ((searchOrder.Direction == Direction.Ascending) ? (flag ? "OrderBy" : "ThenBy") : (flag ? "OrderByDescending" : "ThenByDescending"));
				Type typeFromHandle = typeof(AccessControlEventLine);
				PropertyInfo property = typeFromHandle.GetProperty(searchOrder.Target);
				ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "p");
				LambdaExpression expression2 = Expression.Lambda(Expression.MakeMemberAccess(parameterExpression, property), parameterExpression);
				expression = Expression.Call(typeof(Queryable), methodName, new Type[2] { typeFromHandle, property.PropertyType }, expression, Expression.Quote(expression2));
				flag = false;
			}
		}
		return queryable.Provider.CreateQuery<AccessControlEventLine>(expression);
	}

	public async Task<AggregationResult<ServerId, GetAccessControlPluginsResp>> AggregateGetAccessControlPluginsAsync(IEnumerable<ServerId> serverIds, GetAccessControlPluginsParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlPluginsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetGlobalSettingsResp>> AggregateGetGlobalSettingsAsync(IEnumerable<ServerId> serverIds, GetGlobalSettingsParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetGlobalSettingsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, SetGlobalSettingsResp>> AggregateSetGlobalSettingsAsync(IEnumerable<ServerId> serverIds, SetGlobalSettingsParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).SetGlobalSettingsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetUserDefinedCategoriesResp>> AggregateGetUserDefinedCategoriesAsync(IEnumerable<ServerId> serverIds, GetUserDefinedCategoriesParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetUserDefinedCategoriesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, SetUserDefinedCategoriesResp>> AggregateSetUserDefinedCategoriesAsync(IEnumerable<ServerId> serverIds, SetUserDefinedCategoriesParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).SetUserDefinedCategoriesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetAccessControlEventsResp>> AggregateGetAccessControlEventsAsync(IEnumerable<ServerId> serverIds, GetAccessControlEventsParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlEventsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetAccessControlEventsWithDowntimesResp>> AggregateGetAccessControlEventsWithDowntimesAsync(IEnumerable<ServerId> serverIds, GetAccessControlEventsWithDowntimesParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlEventsWithDowntimesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetAccessControlEventPageResp>> AggregateGetAccessControlEventPageAsync(IEnumerable<ServerId> serverIds, GetAccessControlEventPageParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlEventPageAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetAccessControlEventPageWithDowntimesResp>> AggregateGetAccessControlEventPageWithDowntimesAsync(IEnumerable<ServerId> serverIds, GetAccessControlEventPageWithDowntimesParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlEventPageWithDowntimesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetAccessControlEventCountResp>> AggregateGetAccessControlEventCountAsync(IEnumerable<ServerId> serverIds, GetAccessControlEventCountParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlEventCountAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, SearchCredentialHoldersResp>> AggregateSearchCredentialHoldersAsync(IEnumerable<ServerId> serverIds, SearchCredentialHoldersParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).SearchCredentialHoldersAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetDowntimesResp>> AggregateGetDowntimesAsync(IEnumerable<ServerId> serverIds, GetDowntimesParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetDowntimesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetAccessControlSystemsResp>> AggregateGetAccessControlSystemsAsync(IEnumerable<ServerId> serverIds, GetAccessControlSystemsParams parameters)
	{
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlSystemsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected static void CheckAtMostOneServer(IEnumerable<ServerId> serverIds)
	{
		using IEnumerator<ServerId> enumerator = serverIds.GetEnumerator();
		enumerator.MoveNext();
		if (enumerator.MoveNext())
		{
			throw new ArgumentException("Method does not support aggregation over multiple servers");
		}
	}

	public async Task<AggregationResult<ServerId, ExecuteCommandResp>> AggregateExecuteCommandAsync(IEnumerable<ServerId> serverIds, ExecuteCommandParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).ExecuteCommandAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetStatesResp>> AggregateGetStatesAsync(IEnumerable<ServerId> serverIds, GetStatesParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetStatesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetCredentialHoldersResp>> AggregateGetCredentialHoldersAsync(IEnumerable<ServerId> serverIds, GetCredentialHoldersParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetCredentialHoldersAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, StartFetchAccessControlSystemConfigurationResp>> AggregateStartFetchAccessControlSystemConfigurationAsync(IEnumerable<ServerId> serverIds, StartFetchAccessControlSystemConfigurationParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).StartFetchAccessControlSystemConfigurationAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, CancelFetchAccessControlSystemConfigurationResp>> AggregateCancelFetchAccessControlSystemConfigurationAsync(IEnumerable<ServerId> serverIds, CancelFetchAccessControlSystemConfigurationParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).CancelFetchAccessControlSystemConfigurationAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetAccessControlSystemConfigurationResp>> AggregateGetAccessControlSystemConfigurationAsync(IEnumerable<ServerId> serverIds, GetAccessControlSystemConfigurationParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlSystemConfigurationAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, SetAccessControlSystemConfigurationResp>> AggregateSetAccessControlSystemConfigurationAsync(IEnumerable<ServerId> serverIds, SetAccessControlSystemConfigurationParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).SetAccessControlSystemConfigurationAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, DeleteAccessControlSystemResp>> AggregateDeleteAccessControlSystemAsync(IEnumerable<ServerId> serverIds, DeleteAccessControlSystemParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).DeleteAccessControlSystemAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, CreateAccessControlSystemResp>> AggregateCreateAccessControlSystemAsync(IEnumerable<ServerId> serverIds, CreateAccessControlSystemParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).CreateAccessControlSystemAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetAccessControlPluginIconsResp>> AggregateGetAccessControlPluginIconsAsync(IEnumerable<ServerId> serverIds, GetAccessControlPluginIconsParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlPluginIconsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, GetAccessControlPluginCategoriesResp>> AggregateGetAccessControlPluginCategoriesAsync(IEnumerable<ServerId> serverIds, GetAccessControlPluginCategoriesParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).GetAccessControlPluginCategoriesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, ValidateAccessControlSystemPropertiesResp>> AggregateValidateAccessControlSystemPropertiesAsync(IEnumerable<ServerId> serverIds, ValidateAccessControlSystemPropertiesParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).ValidateAccessControlSystemPropertiesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, SetCredentialHolderImageResp>> AggregateSetCredentialHolderImageAsync(IEnumerable<ServerId> serverIds, SetCredentialHolderImageParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).SetCredentialHolderImageAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, ValidateUserCredentialsResp>> AggregateValidateUserCredentialsAsync(IEnumerable<ServerId> serverIds, ValidateUserCredentialsParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).ValidateUserCredentialsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public async Task<AggregationResult<ServerId, SetAccessControlUnitPositionResp>> AggregateSetAccessControlUnitPositionAsync(IEnumerable<ServerId> serverIds, SetAccessControlUnitPositionParams parameters)
	{
		CheckAtMostOneServer(serverIds);
		return await AggregationHelper.AggregateAsync(serverIds, (ServerId serverId) => GetClient(serverId).SetAccessControlUnitPositionAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}
}
