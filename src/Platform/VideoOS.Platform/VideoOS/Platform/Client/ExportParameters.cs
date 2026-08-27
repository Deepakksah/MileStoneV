using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Platform.Client;

public class ExportParameters
{
	public DateTime BeginTime { get; set; }

	public DateTime EndTime { get; set; }

	public List<Item> ViewItems { get; set; }

	public string DestinationDirectory { get; set; }

	public IDictionary<string, object> Parameters { get; set; }

	public List<Item> CameraItems { get; set; }

	public List<Item> AudioItems { get; set; }

	public Collection<Item> MetadataItems { get; set; }

	public bool ExportingDatabase { get; set; }

	public bool ExportingVideoClip { get; set; }

	public bool ExportingStillImages { get; set; }
}
