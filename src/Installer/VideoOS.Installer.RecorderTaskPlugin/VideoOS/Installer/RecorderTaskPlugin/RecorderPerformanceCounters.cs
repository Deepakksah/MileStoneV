using System;
using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.Installer.Logging;
using VideoOS.Vms.Utilities.Diagnostics;

namespace VideoOS.Installer.RecorderTaskPlugin;

public class RecorderPerformanceCounters
{
	public static void CreatePerformanceCounterCategory(bool isFailover)
	{
		List<PerformanceCounterCategoryBase> list = new List<PerformanceCounterCategoryBase>();
		try
		{
			LogManager.Write("Creating PerformanceCounters");
			list.Add(new WebServerPerformance());
			list.Add(new DriverPerformance());
			list.Add(new RecorderDatabasePerformance());
			list.Add(new RecorderDevicePerformance());
			list.Add(new PipelinePerformance());
			list.Add(new MulticastPerformance());
			list.Add(new ImageServerPerformance());
			list.Add(new DeviceStoragePerformance());
			list.Add(new RecorderDatabaseDiskPerformance());
			list.Add(new StoragePerformance());
			list.Add(new ArchiveStoragePerformance());
			list.Add(new DatabaseStoragePerformance());
			if (isFailover)
			{
				list.Add(new FailoverStatePerformance());
			}
			foreach (PerformanceCounterCategoryBase item in list)
			{
				item.DeleteCategory();
				item.CreateCategory();
			}
			LogManager.Write("Creating PerformanceCounters, Done!");
		}
		catch (Exception ex)
		{
			LogManager.Write("An error has occured while creating performance counter categories", ex.Message, TraceEventType.Error);
			throw new PerformanceCounterCategoryException("Creation of performance counter category failed", ex);
		}
	}

	public static void RemovePerformanceCounterCategory(bool isFailover)
	{
		try
		{
			LogManager.Write("Remove PerformanceCounters");
			List<PerformanceCounterCategoryBase> list = new List<PerformanceCounterCategoryBase>();
			list.Add(new WebServerPerformance());
			list.Add(new DriverPerformance());
			list.Add(new RecorderDatabasePerformance());
			list.Add(new RecorderDevicePerformance());
			list.Add(new PipelinePerformance());
			list.Add(new MulticastPerformance());
			list.Add(new ImageServerPerformance());
			list.Add(new DeviceStoragePerformance());
			list.Add(new RecorderDatabaseDiskPerformance());
			list.Add(new StoragePerformance());
			list.Add(new ArchiveStoragePerformance());
			list.Add(new DatabaseStoragePerformance());
			if (isFailover)
			{
				list.Add(new FailoverStatePerformance());
			}
			foreach (PerformanceCounterCategoryBase item in list)
			{
				item.DeleteCategory();
			}
			LogManager.Write("Remove PerformanceCounters, Done!");
		}
		catch (Exception ex)
		{
			LogManager.Write("An error has occured while removing performance counter categories", ex.Message, TraceEventType.Error);
			throw new PerformanceCounterCategoryException("Removal of performance counter category failed", ex);
		}
	}
}
