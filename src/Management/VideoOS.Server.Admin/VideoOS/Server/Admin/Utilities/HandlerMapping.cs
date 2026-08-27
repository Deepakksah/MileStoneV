namespace VideoOS.Server.Admin.Utilities;

public class HandlerMapping
{
	public string Name { get; }

	public string Path { get; }

	public string Verb { get; }

	public string Module { get; }

	public string ScriptProcessor { get; }

	public string RequireAccess { get; }

	public int ResponseBufferLimit { get; }

	public HandlerMapping(string name, string path, string verbs, string module, string scriptProcessor, string requireAccess)
	{
		Name = name;
		Path = path;
		Verb = verbs;
		Module = module;
		ScriptProcessor = scriptProcessor;
		RequireAccess = requireAccess;
		ResponseBufferLimit = 0;
	}
}
