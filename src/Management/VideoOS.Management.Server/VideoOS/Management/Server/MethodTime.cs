using System;

namespace VideoOS.Management.Server;

public class MethodTime
{
	public string Name { get; set; }

	public TimeSpan Time { get; set; }

	public int LogicalReads { get; set; }

	public int PhysicalReads { get; set; }

	public int CPUTime { get; set; }

	public int ElapsedTime { get; set; }

	public MethodTime()
	{
	}

	public MethodTime(string name, TimeSpan time, int logicalReads, int physicalReads, int cpuTime, int elapsedTime)
	{
		Name = name;
		Time = time;
		LogicalReads = logicalReads;
		PhysicalReads = physicalReads;
		CPUTime = cpuTime;
		ElapsedTime = elapsedTime;
	}
}
