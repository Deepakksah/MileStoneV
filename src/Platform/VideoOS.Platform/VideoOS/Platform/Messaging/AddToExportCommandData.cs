using System.Collections.Generic;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Messaging;

public class AddToExportCommandData
{
	public IEnumerable<(FQID device, TimeInterval interval)> ItemsToExport { get; set; }

	public bool ShowConfirmationToasts { get; set; }
}
