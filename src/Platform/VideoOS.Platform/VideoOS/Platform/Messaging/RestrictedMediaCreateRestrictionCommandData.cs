using System;
using System.Collections.Generic;
using System.Windows;

namespace VideoOS.Platform.Messaging;

public class RestrictedMediaCreateRestrictionCommandData
{
	public IEnumerable<Item> Devices { get; set; }

	public string Header { get; set; }

	public string Description { get; set; }

	public DateTime BeginTime { get; set; }

	public DateTime EndTime { get; set; }

	public Window ParentWindow { get; set; }
}
