using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace VideoOS.Installer.Common.Dism;

public class NativeDismWrapper : INativeDismWrapper
{
	private static class NativeMethods
	{
		[DllImport("DismApi.dll", CharSet = CharSet.Unicode)]
		public static extern int DismGetLastErrorMessage(out IntPtr ErrorMessage);

		[DllImport("DismApi.dll", CharSet = CharSet.Unicode)]
		public static extern int DismInitialize(DismSession.LogLevel LogLevel, [MarshalAs(UnmanagedType.LPWStr)] string LogFilePath, [MarshalAs(UnmanagedType.LPWStr)] string ScratchDirectory);

		[DllImport("DismApi.dll", CharSet = CharSet.Unicode)]
		public static extern int DismShutdown();

		[DllImport("DismApi.dll", CharSet = CharSet.Unicode)]
		public static extern int DismOpenSession([MarshalAs(UnmanagedType.LPWStr)] string ImagePath, [MarshalAs(UnmanagedType.LPWStr)] string WindowsDirectory, [MarshalAs(UnmanagedType.LPWStr)] string SystemDrive, out uint Session);

		[DllImport("DismApi.dll", CharSet = CharSet.Unicode)]
		public static extern int DismCloseSession(uint Session);

		[DllImport("DismApi.dll", CharSet = CharSet.Unicode)]
		public static extern int DismDelete(IntPtr DismStructure);

		[DllImport("DismApi.dll", CharSet = CharSet.Unicode)]
		public static extern int DismGetFeatures(uint Session, [MarshalAs(UnmanagedType.LPWStr)] string Identifier, DismSession.PackageIdentifier PackageIdentifier, out IntPtr Feature, out uint Count);

		[DllImport("DismApi.dll", CharSet = CharSet.Unicode)]
		public static extern int DismEnableFeature(uint Session, [MarshalAs(UnmanagedType.LPWStr)] string FeatureName, [MarshalAs(UnmanagedType.LPWStr)] string Identifier, DismSession.PackageIdentifier PackageIdentifier, [MarshalAs(UnmanagedType.Bool)] bool LimitAccess, [MarshalAs(UnmanagedType.LPWStr)] string SourcePaths, uint SourcePathCount, [MarshalAs(UnmanagedType.Bool)] bool EnableAll, SafeWaitHandle CancelEvent, [MarshalAs(UnmanagedType.FunctionPtr)] DismProgressCallback Progress, IntPtr UserData);

		[DllImport("DismApi.dll", CharSet = CharSet.Unicode)]
		public static extern int DismDisableFeature(uint Session, [MarshalAs(UnmanagedType.LPWStr)] string FeatureName, [MarshalAs(UnmanagedType.LPWStr)] string PackageName, [MarshalAs(UnmanagedType.Bool)] bool RemovePayload, SafeWaitHandle CancelEvent, [MarshalAs(UnmanagedType.FunctionPtr)] DismProgressCallback Progress, IntPtr UserData);

		[DllImport("DismApi.dll", CharSet = CharSet.Unicode)]
		public static extern int DismGetFeatureInfo(uint Session, [MarshalAs(UnmanagedType.LPWStr)] string FeatureName, [MarshalAs(UnmanagedType.LPWStr)] string Identifier, DismSession.PackageIdentifier PackageIdentifier, out IntPtr featureInfo);
	}

	public int DismGetLastErrorMessage(out IntPtr errorMessage)
	{
		return NativeMethods.DismGetLastErrorMessage(out errorMessage);
	}

	public int DismInitialize(DismSession.LogLevel logLevel, string logFilePath, string scratchDirectory)
	{
		return NativeMethods.DismInitialize(logLevel, logFilePath, scratchDirectory);
	}

	public int DismShutdown()
	{
		return NativeMethods.DismShutdown();
	}

	public int DismOpenSession(string imagePath, string windowsDirectory, string systemDrive, out uint session)
	{
		return NativeMethods.DismOpenSession(imagePath, windowsDirectory, systemDrive, out session);
	}

	public int DismCloseSession(uint session)
	{
		return NativeMethods.DismCloseSession(session);
	}

	public int DismDelete(IntPtr dismStructure)
	{
		return NativeMethods.DismDelete(dismStructure);
	}

	public int DismGetFeatures(uint session, string identifier, DismSession.PackageIdentifier packageIdentifier, out IntPtr feature, out uint count)
	{
		return NativeMethods.DismGetFeatures(session, identifier, packageIdentifier, out feature, out count);
	}

	public int DismEnableFeature(uint session, string featureName, string identifier, DismSession.PackageIdentifier packageIdentifier, bool limitAccess, string sourcePaths, uint sourcePathCount, bool enableAll, SafeWaitHandle cancelEvent, DismProgressCallback progress, IntPtr userData)
	{
		return NativeMethods.DismEnableFeature(session, featureName, identifier, packageIdentifier, limitAccess, sourcePaths, sourcePathCount, enableAll, cancelEvent, progress, userData);
	}

	public int DismDisableFeature(uint session, string featureName, string packageName, bool removePayload, SafeWaitHandle cancelEvent, DismProgressCallback progress, IntPtr userData)
	{
		return NativeMethods.DismDisableFeature(session, featureName, packageName, removePayload, cancelEvent, progress, userData);
	}

	public int DismGetFeatureInfo(uint session, string featureName, string identifier, DismSession.PackageIdentifier packageIdentifier, out IntPtr featureInfo)
	{
		return NativeMethods.DismGetFeatureInfo(session, featureName, identifier, packageIdentifier, out featureInfo);
	}
}
