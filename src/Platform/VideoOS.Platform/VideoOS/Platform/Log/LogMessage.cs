using System;

namespace VideoOS.Platform.Log;

public class LogMessage
{
	private string _categoryName;

	public string Id { get; set; }

	public Guid RelatedObjectKind { get; set; }

	public string Group { get; set; }

	public string Category { get; set; }

	public string CategoryName
	{
		get
		{
			return _categoryName ?? Category;
		}
		set
		{
			_categoryName = value;
		}
	}

	public string Status { get; set; }

	public string Severity { get; set; }

	public string Message { get; set; }
}
