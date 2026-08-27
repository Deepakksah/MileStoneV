namespace VideoOS.Platform.JsonHandling;

public interface IElement
{
	string Key { get; set; }

	TokenType TokenType { get; set; }

	bool IsBasicElement { get; }

	void ToJson(JsonBuilder jsonBuilder);

	IElement Element(string key);
}
