using System;
using System.Collections.Generic;
using VideoOS.Management.Server.BusinessObjects.EventTypes;

namespace VideoOS.Management.Server;

public interface IStateGroupComponent : IDisposable
{
	StateGroupInfo QueryStateGroup(Guid id);

	IEnumerable<StateGroupInfo> QueryStateGroups();

	void CreateStateGroup(StateGroupInfo stateGroupInfo);

	void UpdateStateGroup(StateGroupInfo stateGroupInfo);

	void DeleteStateGroup(Guid id);
}
