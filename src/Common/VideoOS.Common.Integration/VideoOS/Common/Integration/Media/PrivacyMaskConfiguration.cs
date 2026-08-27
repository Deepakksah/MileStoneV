using System;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Media;

public class PrivacyMaskConfiguration
{
	private object _lock = new object();

	private volatile PrivacyMask _privacyMask;

	private static PrivacyMaskConfiguration _emptyMask = new PrivacyMaskConfiguration(DateTime.MinValue, enabled: false, string.Empty);

	public DateTime Time { get; private set; }

	public bool Enabled { get; private set; }

	public string MaskXml { get; private set; }

	public PrivacyMask Mask
	{
		get
		{
			if (_privacyMask != null)
			{
				return _privacyMask;
			}
			lock (_lock)
			{
				if (_privacyMask == null)
				{
					_privacyMask = PrivacyMask.FromXml(MaskXml);
				}
				return _privacyMask;
			}
		}
	}

	public bool IsMaskDefined
	{
		get
		{
			if (Enabled)
			{
				return Mask.Cells.Any();
			}
			return false;
		}
	}

	public static PrivacyMaskConfiguration EmptyMask => _emptyMask;

	public PrivacyMaskConfiguration(DateTime time, bool enabled, string privacyMaskXml)
	{
		Time = time;
		Enabled = enabled;
		MaskXml = (enabled ? privacyMaskXml : string.Empty);
	}
}
