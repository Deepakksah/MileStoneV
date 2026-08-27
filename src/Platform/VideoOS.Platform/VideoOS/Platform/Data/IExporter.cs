using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public interface IExporter
{
	List<Item> CameraList { get; set; }

	List<Item> AudioList { get; set; }

	int Progress { get; }

	int LastError { get; }

	int LastDetailedError { get; }

	string LastErrorString { get; }

	string Path { get; set; }

	void Init();

	bool StartExport(DateTime start, DateTime end);

	void Close();

	void Cancel();

	void EndExport();
}
