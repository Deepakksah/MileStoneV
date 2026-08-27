using System;

namespace VideoOS.Management.Server;

public interface IViewLayoutComponent : IDisposable
{
	ViewLayout QueryViewLayout(string id);

	string[] QueryViewLayoutIds(string viewLayoutGroupId);

	ViewLayout[] QueryViewLayoutsByIds(string[] ids);

	void CreateViewLayout(string id, string viewLayoutGroupId, string name, string description, string definitionXml);

	void UpdateViewLayout(string id, string name, string description, string definitionXml);

	void DeleteViewLayout(string id);
}
