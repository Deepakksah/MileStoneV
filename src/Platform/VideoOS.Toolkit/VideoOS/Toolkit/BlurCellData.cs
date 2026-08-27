using System.Runtime.InteropServices;
using NmToolkit;

namespace VideoOS.Toolkit;

public class BlurCellData : PrivacyMaskCellData
{
	private byte m_Level;

	public byte Level => m_Level;

	public unsafe BlurCellData(ImPrivacyMaskProperty.SmBlurCellParameters parameters, [MarshalAs(UnmanagedType.U1)] bool isRemovable)
		: base(PrivacyMaskCellMethod.Blur, isRemovable)
	{
		m_Level = *(byte*)(&parameters);
	}
}
