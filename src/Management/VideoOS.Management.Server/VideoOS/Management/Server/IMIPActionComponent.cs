using System;

namespace VideoOS.Management.Server;

public interface IMIPActionComponent : IDisposable
{
	string CreateMIPAction(string id, string name, string definingPluginId, string definingPluginName, string selectionText, string descriptionText, string pickerText, string mipKinds, string pickerText2, string mipKinds2);

	MIPAction QueryMIPAction(string id);

	MIPAction[] QueryMIPActions();

	MIPAction[] QueryMIPActionsByIds(string[] mipActionIds);

	void UpdateMIPAction(string id, string name, string definingPluginId, string definingPluginName, string selectionText, string descriptionText, string pickerText, string mipKinds, string pickerText2, string mipKinds2);

	void DeleteMIPAction(string mipActionId);
}
