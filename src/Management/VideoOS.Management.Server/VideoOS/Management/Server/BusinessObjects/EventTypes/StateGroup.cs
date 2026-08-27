using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server.BusinessObjects.EventTypes;

public sealed class StateGroup
{
	private StateGroup()
	{
	}

	public static IEnumerable<StateGroupInfo> GetAllStateGroups(IGeminiRequestContext requestContext)
	{
		using IStateGroupComponent stateGroupComponent = requestContext.CreateStateGroupComponent();
		return stateGroupComponent.QueryStateGroups();
	}

	public static StateGroupInfo GetStateGroupInfo(IGeminiRequestContext requestContext, Guid stateGroupId)
	{
		using IStateGroupComponent stateGroupComponent = requestContext.CreateStateGroupComponent();
		return stateGroupComponent.QueryStateGroup(stateGroupId);
	}

	public static void CreateStateGroupInfo(IGeminiRequestContext requestContext, StateGroupInfo stateGroupInfo)
	{
		if (!requestContext.GetSecurityManagerService().IsAdministrator(requestContext))
		{
			throw new UnauthorizedAccessException();
		}
		using IStateGroupComponent stateGroupComponent = requestContext.CreateStateGroupComponent();
		stateGroupComponent.CreateStateGroup(stateGroupInfo);
	}

	public static void UpdateStateGroupInfo(IGeminiRequestContext requestContext, StateGroupInfo stateGroupInfo)
	{
		if (!requestContext.GetSecurityManagerService().IsAdministrator(requestContext))
		{
			throw new UnauthorizedAccessException();
		}
		using IStateGroupComponent stateGroupComponent = requestContext.CreateStateGroupComponent();
		stateGroupComponent.UpdateStateGroup(stateGroupInfo);
	}

	public static void DeleteStateGroup(IGeminiRequestContext requestContext, Guid stateGroupId)
	{
		if (!requestContext.GetSecurityManagerService().IsAdministrator(requestContext))
		{
			throw new UnauthorizedAccessException();
		}
		using IStateGroupComponent stateGroupComponent = requestContext.CreateStateGroupComponent();
		stateGroupComponent.DeleteStateGroup(stateGroupId);
	}
}
