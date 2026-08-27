using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VideoOS.Toolkit;

public interface IPrivacyMaskProperty : IProperty
{
	PrivacyMaskGridSize GridSize { get; }

	long Id { get; }

	IEnumerable<PrivacyMaskCellInformation> GetCellInformation();

	PrivacyMaskCellData GetCellData(byte row, byte column);

	[return: MarshalAs(UnmanagedType.U1)]
	bool IsEmpty([MarshalAs(UnmanagedType.U1)] bool ignoreRemovableMasks);
}
