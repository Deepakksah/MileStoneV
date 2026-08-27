using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IRestrictedMediaComponent : IDisposable
{
	Guid[] CreatePlayback(Guid id, string[] deviceIds, string header, string description, string userName, DateTime startTime, DateTime endTime, DateTime lastModified, Guid sessionId);

	Guid[] UpdatePlayback(Guid Id, string[] deviceIds, string header, string description, DateTime startTime, DateTime endTime, DateTime lastModified, Guid sessionId);

	Guid[] DeletePlayback(Guid Id, Guid sessionId);

	RestrictedMediaDataContract QueryPlayback(Guid id);

	RestrictedMediaDataContract[] QueryAllPlayback();

	Guid[] EnterLive(string[] deviceIds, string userName, DateTime startTime, DateTime lastModified, Guid sessionId);

	Guid[] ExitLive(Guid id, string[] deviceIds, string header, string description, string userName, DateTime startTime, DateTime endTime, DateTime lastModified, Guid sessionId);

	Guid[] UpdateLive(string[] deviceIds, DateTime startTime, DateTime lastModified, Guid sessionId);

	Guid[] DeleteLive(string[] deviceIds, Guid sessionId);

	RestrictedMediaLiveDataContract[] QueryLiveByIds(IEnumerable<string> deviceIds);

	RestrictedMediaLiveDataContract[] QueryAllLive();
}
