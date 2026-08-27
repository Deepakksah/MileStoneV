using System.Runtime.InteropServices;

namespace VideoOS.Toolkit;

public abstract class PrivacyMaskCellData
{
	private PrivacyMaskCellMethod m_Method;

	private bool m_isRemovable;

	public bool IsRemovable
	{
		[return: MarshalAs(UnmanagedType.U1)]
		get
		{
			return m_isRemovable;
		}
	}

	public PrivacyMaskCellMethod Method => m_Method;

	protected PrivacyMaskCellData(PrivacyMaskCellMethod method, [MarshalAs(UnmanagedType.U1)] bool isRemovable)
	{
		m_Method = method;
		m_isRemovable = isRemovable;
	}
}
