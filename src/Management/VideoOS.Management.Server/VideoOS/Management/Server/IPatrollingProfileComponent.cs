using System;

namespace VideoOS.Management.Server;

public interface IPatrollingProfileComponent : IDisposable
{
	PatrollingProfile[] QueryPatrollingProfiles(string[] patrollingProfileIds);

	PatrollingProfile[] QueryPatrollingProfiles(string cameraDeviceId);

	PatrollingProfile QueryPatrollingProfileById(string patrollingProfileId);

	void CreatePatrollingProfile(string cameraDeviceId, PatrollingProfile newPatrollingProfile, Guid sessionId);

	void UpdatePatrollingProfile(string patrollingProfileId, string name, string description, string definitionXml, Guid sessionId);

	void DeletePatrollingProfile(string patrollingProfileId, string name, Guid sessionId);
}
