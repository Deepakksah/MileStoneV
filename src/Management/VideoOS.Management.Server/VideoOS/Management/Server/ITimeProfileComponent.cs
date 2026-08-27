using System;

namespace VideoOS.Management.Server;

public interface ITimeProfileComponent : IDisposable
{
	void CreateTimeProfile(TimeProfile timeProfile);

	string[] QueryTimeProfileIds();

	string[] QueryTimeProfileIds(TimeProfileType type);

	TimeProfile QueryTimeProfile(string timeProfileId);

	TimeProfile[] QueryTimeProfiles();

	TimeProfile[] QueryTimeProfilesByIds(string[] timeProfileIds);

	void UpdateTimeProfile(TimeProfile timeProfile, Guid sessionId);

	void DeleteTimeProfile(string timeProfileId);
}
