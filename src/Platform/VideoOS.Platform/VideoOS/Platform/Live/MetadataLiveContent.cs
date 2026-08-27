using System;
using System.Collections.Generic;
using VideoOS.Platform.Metadata;

namespace VideoOS.Platform.Live;

public class MetadataLiveContent
{
	private readonly DateTime _beginTime;

	private readonly DateTime _endTime;

	private readonly MetadataContent _content;

	private readonly Dictionary<string, string> _properties;

	public DateTime BeginTime => _beginTime;

	public DateTime EndTime => _endTime;

	public MetadataContent Content => _content;

	public Dictionary<string, string> Properties => _properties;

	public MetadataLiveContent(DateTime beginTime, DateTime endTime, MetadataContent content, Dictionary<string, string> properties)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		_beginTime = beginTime;
		_endTime = endTime;
		_content = content;
		_properties = properties;
	}
}
