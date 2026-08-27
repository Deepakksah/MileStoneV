using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IMIPKindComponent : IDisposable
{
	void CreateKind(MIPKind kind);

	void DeleteKind(Guid id);

	MIPKind QueryKind(Guid id);

	void UpdateKind(MIPKind kind);

	void SaveKind(MIPKind kind);

	MIPKind[] QueryKinds();

	MIPKind[] QueryKindsByPluginId(Guid pluginId);

	MIPKind[] QueryKindsByParentKindId(Guid parentKindId);

	Dictionary<Guid, string> QueryPlugins();
}
