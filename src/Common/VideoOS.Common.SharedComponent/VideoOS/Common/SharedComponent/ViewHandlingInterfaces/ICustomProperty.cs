namespace VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

public interface ICustomProperty
{
	string Key { get; set; }

	string Value { get; set; }

	void Update();

	void Delete();

	void Create();
}
