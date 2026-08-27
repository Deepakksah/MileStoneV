using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Platform.JsonHandling;

public class JsonObject
{
	private IElement _topElement;

	private static JsonObject _empty = new JsonObject("{ }");

	public static JsonObject Empty => _empty;

	public int OriginalJsonSize { get; }

	public string Value
	{
		get
		{
			if (_topElement is StringElement)
			{
				return ((StringElement)_topElement).Value;
			}
			if (_topElement is IntElement)
			{
				return ((IntElement)_topElement).Value;
			}
			if (_topElement is BooleanElement)
			{
				return ((BooleanElement)_topElement).Value.ToString();
			}
			if (_topElement is NullElement)
			{
				return null;
			}
			if (_topElement is DoubleElement)
			{
				return ((DoubleElement)_topElement).Value;
			}
			return "";
		}
	}

	public bool IsArray
	{
		get
		{
			if (_topElement is ElementList)
			{
				return ((ElementList)_topElement).TokenType == TokenType.ArrayStart;
			}
			return false;
		}
	}

	public bool IsString => _topElement is StringElement;

	public bool IsInteger => _topElement is IntElement;

	public bool IsBoolean => _topElement is BooleanElement;

	public bool IsObject
	{
		get
		{
			if (_topElement is ElementList)
			{
				return ((ElementList)_topElement).TokenType == TokenType.ElementStart;
			}
			return false;
		}
	}

	public bool IsArrayOfObjects
	{
		get
		{
			if (!(_topElement is ElementList))
			{
				return false;
			}
			if (((ElementList)_topElement).TokenType != TokenType.ArrayStart)
			{
				return false;
			}
			if (((ElementList)_topElement).Elements.Count == 0)
			{
				return false;
			}
			IElement element = ((ElementList)_topElement).Elements[0];
			if (element is ElementList)
			{
				return ((ElementList)element).TokenType == TokenType.ElementStart;
			}
			return false;
		}
	}

	public bool IsEmptyArray
	{
		get
		{
			if (!(_topElement is ElementList))
			{
				return false;
			}
			if (((ElementList)_topElement).TokenType != TokenType.ArrayStart)
			{
				return false;
			}
			return ((ElementList)_topElement).Elements.Count == 0;
		}
	}

	public static JsonObject Parse(string json)
	{
		if (string.IsNullOrEmpty(json))
		{
			return null;
		}
		return new JsonObject(json);
	}

	[DebuggerStepThrough]
	public JsonObject(string json)
	{
		JsonReader jsonReader = new JsonReader(json);
		OriginalJsonSize = jsonReader.Length;
		_topElement = jsonReader.Parse();
	}

	public JsonObject(IElement top)
	{
		_topElement = top;
	}

	public Collection<string> GetPropertyNames()
	{
		Collection<string> collection = new Collection<string>();
		if (_topElement is ElementList)
		{
			foreach (IElement element in ((ElementList)_topElement).Elements)
			{
				collection.Add(element.Key);
			}
		}
		return collection;
	}

	[DebuggerStepThrough]
	public string GetType(string key)
	{
		if (_topElement is ElementList)
		{
			IElement element = ((ElementList)_topElement).Element(key);
			if (element is IntElement)
			{
				return "Integer";
			}
			if (element is StringElement)
			{
				return "String";
			}
			if (element is BooleanElement)
			{
				return "Bool";
			}
			if (element is DoubleElement)
			{
				return "Double";
			}
			if (element is NullElement)
			{
				return "String";
			}
			if (element is ElementList)
			{
				if (((ElementList)element).TokenType == TokenType.ArrayStart)
				{
					return "Array";
				}
				return "Object";
			}
		}
		throw new MIPException("Unknown json token");
	}

	public string GetString(string key)
	{
		IElement element = _topElement.Element(key);
		if (element == null)
		{
			return string.Empty;
		}
		if (element is StringElement stringElement)
		{
			return stringElement.Value;
		}
		if (element is IntElement intElement)
		{
			return intElement.Value.ToString(CultureInfo.InvariantCulture);
		}
		BooleanElement booleanElement = element as BooleanElement;
		if (booleanElement != null)
		{
			return booleanElement.Value.ToString();
		}
		if (booleanElement != null)
		{
			return booleanElement.Value.ToString(CultureInfo.InvariantCulture);
		}
		if (element is NullElement)
		{
			return null;
		}
		return string.Empty;
	}

	public void SetString(string key, string value)
	{
		(((_topElement.Element(key) ?? throw new ArgumentException("field not found")) as StringElement) ?? throw new ArgumentException("field must be a Stringelement")).Value = value;
	}

	[DebuggerStepThrough]
	public int? GetInteger(string key)
	{
		IElement element = _topElement.Element(key);
		if (element is NullElement)
		{
			return null;
		}
		IntElement intElement = element as IntElement;
		if (intElement != null && intElement.Value == null)
		{
			return null;
		}
		if (intElement != null && int.TryParse(intElement.Value, out var result))
		{
			return result;
		}
		if (element is StringElement)
		{
			return int.Parse(((StringElement)element).Value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
		}
		return 0;
	}

	[DebuggerStepThrough]
	public ulong GetUlong(string key)
	{
		if (_topElement.Element(key) is IntElement intElement && ulong.TryParse(intElement.Value, out var result))
		{
			return result;
		}
		return 0uL;
	}

	[DebuggerStepThrough]
	public void SetInteger(string key, int value)
	{
		IElement element = _topElement.Element(key);
		if (!(element is IntElement intElement))
		{
			((element as DoubleElement) ?? throw new ArgumentException("Field not found, or not an integer field")).Value = value.ToString(CultureInfo.InvariantCulture);
		}
		else
		{
			intElement.Value = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	[DebuggerStepThrough]
	public double GetDouble(string key)
	{
		IElement element = _topElement.Element(key);
		if (element is IntElement)
		{
			return double.Parse(((IntElement)element).Value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
		}
		if (element is StringElement)
		{
			return double.Parse(((StringElement)element).Value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
		}
		if (element is DoubleElement && double.TryParse(((DoubleElement)_topElement.Element(key)).Value, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var result))
		{
			return result;
		}
		return 0.0;
	}

	[DebuggerStepThrough]
	public void SetDouble(string key, double value)
	{
		IElement element = _topElement.Element(key);
		if (!(element is DoubleElement doubleElement))
		{
			if (element is IntElement intElement)
			{
				intElement.Value = value.ToString(CultureInfo.InvariantCulture);
				return;
			}
			if (!(element is StringElement stringElement))
			{
				throw new ArgumentException("Field not found, or not a double / int");
			}
			stringElement.Value = value.ToString(CultureInfo.InvariantCulture);
		}
		else
		{
			doubleElement.Value = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	[DebuggerStepThrough]
	public bool? GetBool(string key)
	{
		IElement element = _topElement.Element(key);
		if (element is BooleanElement)
		{
			return ((BooleanElement)element).Value;
		}
		if (element is StringElement && bool.TryParse(((StringElement)element).Value, out var result))
		{
			return result;
		}
		return null;
	}

	[DebuggerStepThrough]
	public void SetBool(string key, bool value)
	{
		IElement element = _topElement.Element(key);
		if (element is BooleanElement)
		{
			((BooleanElement)element).Value = value;
			return;
		}
		throw new ArgumentException("Field not found, or not a bool");
	}

	[DebuggerStepThrough]
	public string GetDateTime(string key)
	{
		return GetString(key);
	}

	[DebuggerStepThrough]
	public string GetTime(string key)
	{
		return GetString(key);
	}

	[DebuggerStepThrough]
	public JsonObject GetChild(string key)
	{
		try
		{
			if (_topElement is ElementList)
			{
				IElement element = ((ElementList)_topElement).Element(key);
				if (element != null)
				{
					return new JsonObject(element);
				}
			}
		}
		catch (FormatException)
		{
		}
		return null;
	}

	[DebuggerStepThrough]
	public JsonObject GetChildAt(int index)
	{
		if (_topElement is ElementList && index >= 0 && index < ((ElementList)_topElement).Elements.Count)
		{
			IElement element = ((ElementList)_topElement).Elements[index];
			if (element != null)
			{
				return new JsonObject(element);
			}
		}
		return null;
	}

	public Collection<JsonObject> GetChildren()
	{
		if (_topElement is ElementList && ((ElementList)_topElement).TokenType == TokenType.ArrayStart)
		{
			Collection<JsonObject> collection = new Collection<JsonObject>();
			{
				foreach (IElement element in ((ElementList)_topElement).Elements)
				{
					collection.Add(new JsonObject(element));
				}
				return collection;
			}
		}
		return null;
	}

	public string ToJson()
	{
		JsonBuilder jsonBuilder = new JsonBuilder(IsArray);
		foreach (IElement element in ((ElementList)_topElement).Elements)
		{
			element.ToJson(jsonBuilder);
		}
		return jsonBuilder.ToJson();
	}
}
