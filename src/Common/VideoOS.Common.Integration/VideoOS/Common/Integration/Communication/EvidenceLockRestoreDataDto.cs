using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[Obsolete("We need to keep class only because it is used in legacy interfaces.")]
public class EvidenceLockRestoreDataDto
{
	public string RetentionId { get; set; }

	public string Header { get; set; }

	public string UserName { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime EndDate { get; set; }

	public DateTime ExpiryDate { get; set; }

	public long Size { get; set; }

	public ICollection<string> Devices { get; set; }

	public StickerSizeCollection StickerSizes { get; set; }
}
