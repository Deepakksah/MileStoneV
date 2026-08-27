using System;
using Microsoft.Win32.SafeHandles;

namespace VideoOS.Installer.Common.Dism;

public interface INativeDismWrapper
{
	int DismCloseSession(uint session);

	int DismDelete(IntPtr dismStructure);

	int DismDisableFeature(uint session, string featureName, string packageName, bool removePayload, SafeWaitHandle cancelEvent, DismProgressCallback progress, IntPtr userData);

	int DismEnableFeature(uint session, string featureName, string identifier, DismSession.PackageIdentifier packageIdentifier, bool limitAccess, string sourcePaths, uint sourcePathCount, bool enableAll, SafeWaitHandle cancelEvent, DismProgressCallback progress, IntPtr userData);

	int DismGetFeatureInfo(uint session, string featureName, string identifier, DismSession.PackageIdentifier packageIdentifier, out IntPtr featureInfo);

	int DismGetFeatures(uint session, string identifier, DismSession.PackageIdentifier packageIdentifier, out IntPtr feature, out uint count);

	int DismGetLastErrorMessage(out IntPtr errorMessage);

	int DismInitialize(DismSession.LogLevel logLevel, string logFilePath, string scratchDirectory);

	int DismOpenSession(string imagePath, string windowsDirectory, string systemDrive, out uint session);

	int DismShutdown();
}
