using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VideoOS.Platform.JsonHandling;

public class JsonReader
{
	private string _json;

	private int _pos;

	private readonly HashSet<char> _blank = new HashSet<char> { ' ', '\n', '\r', '\t' };

	public int Length { get; }

	public JsonReader(string json)
	{
		_json = json ?? throw new ArgumentNullException("json");
		_pos = 0;
		Length = json.Length;
	}

	public ElementList Parse()
	{
		if (Length == 0)
		{
			return new ElementList("", TokenType.ElementStart);
		}
		if (NextToken().TokenType != TokenType.ElementStart)
		{
			throw new TokenException();
		}
		return BuildList("");
	}

	private Token NextToken()
	{
		while (_pos < Length && _blank.Contains(_json[_pos]))
		{
			_pos++;
		}
		if (_pos >= Length)
		{
			throw new ArgumentException("Unexpected end of JSON");
		}
		Token token = new Token();
		switch (_json[_pos])
		{
		case '{':
			token.TokenType = TokenType.ElementStart;
			break;
		case '}':
			token.TokenType = TokenType.ElementEnd;
			break;
		case '[':
			token.TokenType = TokenType.ArrayStart;
			break;
		case ']':
			token.TokenType = TokenType.ArrayEnd;
			break;
		case ':':
			token.TokenType = TokenType.Colon;
			break;
		case ',':
			token.TokenType = TokenType.Comma;
			break;
		case '"':
			_pos++;
			token.Text = ReadString();
			if (token.Text == "true" || token.Text == "false" || token.Text == "True" || token.Text == "False")
			{
				token.TokenType = TokenType.Boolean;
				token.BoolValue = bool.Parse(token.Text);
			}
			else
			{
				token.TokenType = TokenType.String;
			}
			break;
		default:
			if (_json[_pos] == 'f' || _json[_pos] == 't')
			{
				token.BoolValue = ReadBoolean();
				token.TokenType = TokenType.Boolean;
				_pos--;
			}
			else if (_json[_pos] == 'n' && ReadNull())
			{
				token.TokenType = TokenType.Null;
				_pos--;
			}
			else
			{
				token.Text = ReadNumber(out token.TokenType);
				_pos--;
			}
			break;
		}
		_pos++;
		return token;
	}

	private string ReadNumber(out TokenType tokenType)
	{
		char c = _json[_pos];
		switch (c)
		{
		case '+':
		case '-':
		case '0':
		case '1':
		case '2':
		case '3':
		case '4':
		case '5':
		case '6':
		case '7':
		case '8':
		case '9':
		{
			bool flag = false;
			string text = "";
			while (c == '+' || c == '-' || (c >= '0' && c <= '9') || c == '.')
			{
				if (c == '.')
				{
					if (flag)
					{
						throw new TokenException();
					}
					flag = true;
				}
				text += c;
				_pos++;
				c = ((_pos >= Length) ? ' ' : _json[_pos]);
			}
			if (flag && double.TryParse(text, out var _))
			{
				tokenType = TokenType.Double;
				return text;
			}
			if (int.TryParse(text, out var _))
			{
				tokenType = TokenType.Int;
				return text;
			}
			if (ulong.TryParse(text, out var _))
			{
				tokenType = TokenType.Ulong;
				return text;
			}
			throw new TokenException();
		}
		default:
			throw new TokenException();
		}
	}

	private bool ReadBoolean()
	{
		if (_pos + 4 < Length && _json[_pos] == 't' && _json[_pos + 1] == 'r' && _json[_pos + 2] == 'u' && _json[_pos + 3] == 'e')
		{
			_pos += 4;
			return true;
		}
		if (_pos + 5 < Length && _json[_pos] == 'f' && _json[_pos + 1] == 'a' && _json[_pos + 2] == 'l' && _json[_pos + 3] == 's' && _json[_pos + 4] == 'e')
		{
			_pos += 5;
			return false;
		}
		throw new TokenException();
	}

	private bool ReadNull()
	{
		if (_pos + 4 < Length && _json[_pos] == 'n' && _json[_pos + 1] == 'u' && _json[_pos + 2] == 'l' && _json[_pos + 3] == 'l')
		{
			_pos += 4;
			return true;
		}
		throw new TokenException();
	}

	private string ReadString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		while (_pos < Length && _json[_pos] != '"')
		{
			char c = _json[_pos++];
			if (c == '\\')
			{
				c = _json[_pos++];
				switch (c)
				{
				case 'u':
				{
					c = ((!int.TryParse(_json[_pos++].ToString() + _json[_pos++] + _json[_pos++] + _json[_pos++], NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var result)) ? '?' : ((char)result));
					break;
				}
				case 'n':
					c = '\n';
					break;
				case 'r':
					c = '\r';
					break;
				case 't':
					c = '\t';
					break;
				}
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	private ElementList BuildList(string parentKey)
	{
		ElementList elementList = new ElementList(parentKey, TokenType.ElementStart);
		Token token = NextToken();
		while (token.TokenType == TokenType.String)
		{
			string text = token.Text;
			token = NextToken();
			if (token.TokenType != TokenType.Colon && token.TokenType != TokenType.Comma)
			{
				throw new TokenException();
			}
			token = NextToken();
			switch (token.TokenType)
			{
			case TokenType.ElementStart:
			{
				ElementList element = BuildList(text);
				elementList.AddElement(element);
				break;
			}
			case TokenType.ArrayStart:
				elementList.AddElement(BuildArray(text));
				break;
			case TokenType.String:
				elementList.AddElement(new StringElement(text, token.Text));
				break;
			case TokenType.Int:
				elementList.AddElement(new IntElement(text, token.Text));
				break;
			case TokenType.Ulong:
				elementList.AddElement(new IntElement(text, token.Text));
				break;
			case TokenType.Null:
				elementList.AddElement(new NullElement(text));
				break;
			case TokenType.Boolean:
				elementList.AddElement(new BooleanElement(text, token.BoolValue));
				break;
			case TokenType.Double:
				elementList.AddElement(new DoubleElement(text, token.Text));
				break;
			}
			token = NextToken();
			if (token.TokenType == TokenType.Comma)
			{
				token = NextToken();
			}
		}
		if (token.TokenType != TokenType.ElementEnd)
		{
			throw new TokenException();
		}
		return elementList;
	}

	private ElementList BuildArray(string key)
	{
		ElementList elementList = new ElementList(key, TokenType.ArrayStart);
		Token token = NextToken();
		while (token.TokenType == TokenType.ElementStart || token.TokenType == TokenType.ArrayStart || token.TokenType == TokenType.String || token.TokenType == TokenType.Int || token.TokenType == TokenType.Ulong || token.TokenType == TokenType.Boolean || token.TokenType == TokenType.Double || token.TokenType == TokenType.Null)
		{
			switch (token.TokenType)
			{
			case TokenType.ElementStart:
			{
				ElementList element2 = BuildList("");
				elementList.AddElement(element2);
				break;
			}
			case TokenType.ArrayStart:
			{
				ElementList element = BuildArray("");
				elementList.AddElement(element);
				break;
			}
			case TokenType.String:
				elementList.AddElement(new StringElement("", token.Text));
				break;
			case TokenType.Int:
			case TokenType.Ulong:
				elementList.AddElement(new IntElement("", token.Text));
				break;
			case TokenType.Boolean:
				elementList.AddElement(new BooleanElement("", token.BoolValue));
				break;
			case TokenType.Double:
				elementList.AddElement(new DoubleElement("", token.Text));
				break;
			case TokenType.Null:
				elementList.AddElement(new NullElement(""));
				break;
			}
			token = NextToken();
			if (token.TokenType == TokenType.Comma)
			{
				token = NextToken();
			}
		}
		return elementList;
	}
}
