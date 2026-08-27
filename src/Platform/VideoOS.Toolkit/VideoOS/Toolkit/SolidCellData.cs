using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmToolkit;

namespace VideoOS.Toolkit;

public class SolidCellData : PrivacyMaskCellData
{
	private Color m_Color;

	public Color Color => m_Color;

	public unsafe SolidCellData(ImPrivacyMaskProperty.SmSolidCellParameters parameters, [MarshalAs(UnmanagedType.U1)] bool isRemovable)
		: base(PrivacyMaskCellMethod.Solid, isRemovable)
	{
		Color color = Color.FromArgb(*(byte*)(&parameters), System.Runtime.CompilerServices.Unsafe.As<ImPrivacyMaskProperty.SmSolidCellParameters, byte>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref parameters, 1)), System.Runtime.CompilerServices.Unsafe.As<ImPrivacyMaskProperty.SmSolidCellParameters, byte>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref parameters, 2)));
		m_Color = color;
		GC.KeepAlive(this);
	}
}
