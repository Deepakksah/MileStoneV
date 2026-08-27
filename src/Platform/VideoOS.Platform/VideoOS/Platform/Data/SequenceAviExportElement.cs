using System;

namespace VideoOS.Platform.Data;

public class SequenceAviExportElement
{
	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }

	public Item Camera { get; set; }

	public string OverlayText { get; set; }
}
