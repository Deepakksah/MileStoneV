using System;

namespace VideoOS.Management.Server;

public interface IBasicOwnerInfoComponent : IDisposable
{
	void CreateBasicOwnerInfo(string id, string name, string property, string order);

	string[] QueryBasicOwnerInfoIds();

	BasicOwnerInfo[] QueryBasicOwnerInfosByIds(string[] ids);

	BasicOwnerInfo[] QueryBasicOwnerInfo();

	BasicOwnerInfo QueryBasicOwnerInfoById(string id);

	void UpdateBasicOwnerInfo(string id, string name, string property, string order, Guid sessionId);

	void DeleteBasicOwnerInfo(string id, Guid sessionId);
}
