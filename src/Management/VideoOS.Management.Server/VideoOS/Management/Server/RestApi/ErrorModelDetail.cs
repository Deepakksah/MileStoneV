using System;
using System.Globalization;

namespace VideoOS.Management.Server.RestApi;

public class ErrorModelDetail
{
	private string _text = string.Empty;

	public string Key { get; set; } = string.Empty;

	public string Text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = value ?? "";
			_text = _text.TrimEnd(' ').TrimEnd(',');
			try
			{
				int num = _text.IndexOf('[');
				if (num > 0)
				{
					_text = JsonUtilities.CheckConvertPath(_text);
				}
				num = _text.IndexOf("VMO", StringComparison.Ordinal);
				if (num >= 0 && char.IsDigit(_text[num + 3]) && char.IsDigit(_text[num + 4]) && char.IsDigit(_text[num + 5]) && char.IsDigit(_text[num + 6]) && char.IsDigit(_text[num + 7]))
				{
					string text = _text.Substring(0, num);
					string textId = _text.Substring(num, 8);
					string text2 = _text.Substring(num + 8).Trim(':', ' ');
					_text = text + text2;
					TextId = textId;
				}
			}
			catch (Exception ex)
			{
				_text = string.Format(CultureInfo.InvariantCulture, "Unable to process error message due to: {0}", ex.Message);
			}
		}
	}

	public string TextId { get; set; } = string.Empty;
}
