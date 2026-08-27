using System;
using System.Collections;
using System.Text;

namespace NSpring.Logging;

public sealed class Level : IComparable
{
	private const string ACCEPTABLE_NAME_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";

	private static Hashtable levelPrioritiesMap = new Hashtable(50);

	private static Hashtable levelNamesMap = new Hashtable(50);

	private static Hashtable levelIDsMap = new Hashtable(50);

	private static ArrayList levelsList = new ArrayList(50);

	private const string BLANK_STRING = "";

	private static readonly object staticMonitor = new object();

	public static readonly Level Debug = new Level(1, 1, "Debug", "debug", "DEBUG", _isPredefined: true);

	public static readonly Level Verbose = new Level(2, 8, "Verbose", "verbose", "VERBOSE", _isPredefined: true);

	public static readonly Level Config = new Level(3, 64, "Config", "config", "CONFIG", _isPredefined: true);

	public static readonly Level Info = new Level(4, 512, "Info", "info", "INFO", _isPredefined: true);

	public static readonly Level Warning = new Level(5, 4096, "Warning", "warning", "WARNING", _isPredefined: true);

	public static readonly Level Error = new Level(6, 32768, "Error", "error", "ERROR", _isPredefined: true);

	public static readonly Level All = new Level(7, int.MinValue, "All", "all", "ALL", _isPredefined: false);

	public static readonly Level None = new Level(8, int.MaxValue, "None", "none", "NONE", _isPredefined: false);

	private static int nextID = 9;

	public readonly int Priority;

	public readonly string PriorityString;

	public readonly string Name;

	public readonly string NameLowerCase;

	public readonly string NameUpperCase;

	public readonly bool IsPredefined;

	private int id;

	private string idString;

	public int ID
	{
		get
		{
			return id;
		}
		set
		{
			if (IsPredefined)
			{
				throw new ApplicationException("Cannot set the ID of a predefined level");
			}
			lock (staticMonitor)
			{
				if (levelIDsMap.ContainsKey(value))
				{
					throw new ApplicationException("ID " + Priority + " is already in use");
				}
				levelIDsMap.Remove(id);
				id = value;
				idString = id.ToString();
				levelIDsMap[id] = this;
				if (value >= nextID)
				{
					nextID = value + 1;
				}
			}
		}
	}

	public string IDString => idString;

	public static Level GetLevel(string name)
	{
		return (Level)levelNamesMap[name.ToLower()];
	}

	public static Level GetLevel(int priority)
	{
		return (Level)levelPrioritiesMap[priority];
	}

	private Level()
	{
	}

	public static Level CreateLevel(int _priority, string _name)
	{
		if (_name == null)
		{
			throw new ArgumentNullException();
		}
		lock (staticMonitor)
		{
			Level level = new Level(_priority, _name);
			AddLevel(level);
			return level;
		}
	}

	public static Level CreateLevel(int _priority, string _name, int _id)
	{
		if (_name == null)
		{
			throw new ArgumentNullException();
		}
		lock (staticMonitor)
		{
			Level level = new Level(_priority, _name, _id);
			AddLevel(level);
			return level;
		}
	}

	private static Hashtable CopyHashtable(Hashtable h)
	{
		Hashtable hashtable = new Hashtable(h.Count * 2);
		IEnumerator enumerator = h.Keys.GetEnumerator();
		while (enumerator.MoveNext())
		{
			object current = enumerator.Current;
			hashtable[current] = h[current];
		}
		return hashtable;
	}

	private static ArrayList CopyArrayList(ArrayList al)
	{
		ArrayList arrayList = new ArrayList(al.Count + 1);
		IEnumerator enumerator = al.GetEnumerator();
		while (enumerator.MoveNext())
		{
			arrayList.Add(enumerator.Current);
		}
		return arrayList;
	}

	private static void ValidateLevel(Level level)
	{
		if (levelPrioritiesMap.ContainsKey(level.Priority))
		{
			throw new ApplicationException("A level already exists with the specified priority (" + level.Priority + ")");
		}
		if (levelNamesMap.ContainsKey(level.NameLowerCase))
		{
			throw new ApplicationException("A level already exists with the specified name (" + level.NameLowerCase + ")");
		}
		if (levelIDsMap.ContainsKey(level.ID))
		{
			throw new ApplicationException("A level already exists with the specified ID (" + level.ID + ")");
		}
	}

	private static void AddLevel(Level level)
	{
		lock (staticMonitor)
		{
			ValidateLevel(level);
			Hashtable hashtable = CopyHashtable(levelPrioritiesMap);
			Hashtable hashtable2 = CopyHashtable(levelNamesMap);
			Hashtable hashtable3 = CopyHashtable(levelIDsMap);
			ArrayList arrayList = CopyArrayList(levelsList);
			hashtable.Add(level.Priority, level);
			hashtable2.Add(level.NameLowerCase, level);
			hashtable3.Add(level.ID, level);
			arrayList.Add(level);
			arrayList.Sort();
			levelPrioritiesMap = hashtable;
			levelNamesMap = hashtable2;
			levelIDsMap = hashtable3;
			levelsList = arrayList;
		}
	}

	private Level(int _priority, string _name)
	{
		_name = _name.Trim();
		StringBuilder stringBuilder = new StringBuilder(_name.Length);
		foreach (char value in _name)
		{
			if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-".IndexOf(value) >= 0)
			{
				stringBuilder.Append(value);
			}
		}
		_name = stringBuilder.ToString();
		if (_name.Equals(""))
		{
			throw new ApplicationException("Invalid name");
		}
		Priority = _priority;
		PriorityString = _priority.ToString();
		Name = _name;
		NameLowerCase = Name.ToLower();
		NameUpperCase = Name.ToUpper();
		IsPredefined = false;
		while (levelIDsMap.ContainsKey(nextID))
		{
			nextID++;
		}
		id = nextID++;
		idString = id.ToString();
	}

	private Level(int _priority, string _name, int _id)
	{
		_name = _name.Trim();
		StringBuilder stringBuilder = new StringBuilder(_name.Length);
		foreach (char value in _name)
		{
			if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-".IndexOf(value) >= 0)
			{
				stringBuilder.Append(value);
			}
		}
		_name = stringBuilder.ToString();
		if (_name.Equals(""))
		{
			throw new ApplicationException("Invalid name");
		}
		Priority = _priority;
		PriorityString = _priority.ToString();
		Name = _name;
		NameLowerCase = Name.ToLower();
		NameUpperCase = Name.ToUpper();
		IsPredefined = false;
		id = _id;
		idString = id.ToString();
	}

	private Level(int _id, int _priority, string _name, string _nameLowerCase, string _nameUpperCase, bool _isPredefined)
	{
		if (_name == null)
		{
			throw new ArgumentNullException();
		}
		_name = _name.Trim();
		if (_name.Equals(""))
		{
			throw new ApplicationException("Invalid name");
		}
		id = _id;
		idString = ID.ToString();
		Priority = _priority;
		PriorityString = _priority.ToString();
		Name = _name;
		NameLowerCase = _nameLowerCase;
		NameUpperCase = _nameUpperCase;
		IsPredefined = _isPredefined;
		AddLevel(this);
	}

	public override string ToString()
	{
		return Name;
	}

	public int CompareTo(object o)
	{
		return Priority - ((Level)o).Priority;
	}

	public static bool operator <(Level _level1, Level _level2)
	{
		return _level1.Priority < _level2.Priority;
	}

	public static bool operator <=(Level _level1, Level _level2)
	{
		return _level1.Priority <= _level2.Priority;
	}

	public static bool operator >(Level _level1, Level _level2)
	{
		return _level1.Priority > _level2.Priority;
	}

	public static bool operator >=(Level _level1, Level _level2)
	{
		return _level1.Priority >= _level2.Priority;
	}
}
