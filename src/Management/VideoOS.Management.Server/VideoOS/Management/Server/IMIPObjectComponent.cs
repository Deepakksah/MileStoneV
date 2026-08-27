using System;

namespace VideoOS.Management.Server;

public interface IMIPObjectComponent : IDisposable
{
	string CreateMIPObject(string id, string parentId, string name, string kind, string subKind, string pluginId, string customData);

	MIPObject QueryMIPObject(string id);

	MIPObject[] QueryMIPObjects(string parentId);

	MIPObject[] QueryMIPObjectsByIds(string[] mipObjectIds);

	MIPObject[] QueryMIPObjectsByKind(string kind, string subKind);

	void UpdateMIPObject(string id, string name, string customData);

	void DeleteMIPObject(string mipObjectId);
}
