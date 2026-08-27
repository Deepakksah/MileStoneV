using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VideoOS.Common.Integration;

public static class ImmerVisionLenses
{
	private static IReadOnlyList<ImmerVisionLensDescription> lenses;

	private static readonly object lockObj = new object();

	public static IReadOnlyList<ImmerVisionLensDescription> Lenses
	{
		get
		{
			lock (lockObj)
			{
				if (lenses == null)
				{
					List<ImmerVisionLensDescription> list = new List<ImmerVisionLensDescription>();
					list.Add(new ImmerVisionLensDescription("Panomorph", "Generic"));
					long num = NativeMethods.StaticGetLensDescriptionCount();
					IntPtr pointer = NativeMethods.StaticGetLensDescription();
					int num2 = Marshal.SizeOf<NativeMethods.SLensDescription>();
					for (int i = 0; i < num; i++)
					{
						NativeMethods.SLensDescription obj = (NativeMethods.SLensDescription)Marshal.PtrToStructure(IntPtr.Add(pointer, i * num2), typeof(NativeMethods.SLensDescription));
						string name = Marshal.PtrToStringAnsi(obj.Name);
						string regPanLens = Marshal.PtrToStringAnsi(obj.RPL);
						list.Add(new ImmerVisionLensDescription(name, regPanLens));
					}
					lenses = list;
				}
				return lenses;
			}
		}
	}

	public static IReadOnlyCollection<LensProfile> MergeProfiles(IReadOnlyCollection<ImmerVisionLensDescription> immerVisionLenses, IReadOnlyCollection<LensProfile> pluginDataProfiles)
	{
		if (immerVisionLenses == null)
		{
			throw new ArgumentNullException("immerVisionLenses");
		}
		if (pluginDataProfiles == null)
		{
			throw new ArgumentNullException("pluginDataProfiles");
		}
		List<LensProfile> list = new List<LensProfile>();
		foreach (ImmerVisionLensDescription immerVisionLens in immerVisionLenses)
		{
			ImmerVisionLensDescription immerVisionLensDescription = new ImmerVisionLensDescription(immerVisionLens.Name, immerVisionLens.LensProfile);
			LensProfile item = new LensProfile(immerVisionLens.LensProfile, immerVisionLens.Name, 180.0);
			foreach (LensProfile pluginDataProfile in pluginDataProfiles)
			{
				if (pluginDataProfile.Name == immerVisionLensDescription.Name && pluginDataProfile.RegisteredPanomorphLensNumber == immerVisionLensDescription.LensProfile)
				{
					item = new LensProfile(immerVisionLens.LensProfile, immerVisionLens.Name, pluginDataProfile.FieldOfView);
					break;
				}
			}
			list.Add(item);
		}
		return list;
	}
}
