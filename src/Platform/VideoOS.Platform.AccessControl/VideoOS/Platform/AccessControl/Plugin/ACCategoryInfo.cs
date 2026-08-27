namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACCategoryInfo
{
	public enum ACTypes
	{
		StateType,
		CommandType,
		EventType,
		ServerType,
		UnitType
	}

	public string Id { get; private set; }

	public string Name { get; private set; }

	public ACTypes ACType { get; private set; }

	public ACCategoryInfo(string id, string name, ACTypes acType)
	{
		Id = id;
		Name = name;
		ACType = acType;
	}
}
