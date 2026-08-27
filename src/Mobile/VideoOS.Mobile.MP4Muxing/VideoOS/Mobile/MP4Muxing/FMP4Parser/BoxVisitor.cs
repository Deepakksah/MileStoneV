using System;
using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class BoxVisitor
{
	private readonly IDictionary<string, Action<IBoxParse>> _onBoxMapping;

	public BoxVisitor(IDictionary<string, Action<IBoxParse>> onBoxMapping)
	{
		_onBoxMapping = onBoxMapping;
	}

	public void OnBox(IBoxParse box)
	{
		if (_onBoxMapping.ContainsKey(box.BoxType))
		{
			_onBoxMapping[box.BoxType](box);
		}
	}
}
