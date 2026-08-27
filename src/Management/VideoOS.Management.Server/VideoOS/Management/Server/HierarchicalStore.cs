using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class HierarchicalStore<T> : ITokenStore<T>
{
	public delegate bool EnumNodeCallback(T referencedObject, object param);

	public class HierarchicalTreeNode<X>
	{
		public HierarchicalTreeNode<X> _parent;

		public List<HierarchicalTreeNode<X>> _children;

		public X _referencedObject;

		public string _token;

		public string[] _tokenElements;

		public int ChildCount
		{
			get
			{
				if (_children != null)
				{
					return _children.Count;
				}
				return 0;
			}
		}

		public HierarchicalTreeNode(string token, string[] tokenElements, X referencedObject)
		{
			_token = token;
			_tokenElements = tokenElements;
			_referencedObject = referencedObject;
			_children = null;
		}
	}

	private class EnumParentsEnumerable : IEnumerable<EnumeratedHierarchicalNode<T>>, IEnumerable
	{
		private HierarchicalStore<T> _hierarchicalStore;

		private string _token;

		public EnumParentsEnumerable(HierarchicalStore<T> hierarchicalStore, string token)
		{
			_hierarchicalStore = hierarchicalStore;
			_token = token;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<EnumeratedHierarchicalNode<T>>)this).GetEnumerator();
		}

		IEnumerator<EnumeratedHierarchicalNode<T>> IEnumerable<EnumeratedHierarchicalNode<T>>.GetEnumerator()
		{
			return new EnumParentsEnumerator(_hierarchicalStore, _token);
		}
	}

	private class EnumParentsEnumerator : IEnumerator<EnumeratedHierarchicalNode<T>>, IDisposable, IEnumerator
	{
		private HierarchicalStore<T> _hierarchicalStore;

		private string _token;

		private string[] _tokenElements;

		private EnumeratedHierarchicalNode<T> _current;

		private HierarchicalTreeNode<T> _currentParent;

		EnumeratedHierarchicalNode<T> IEnumerator<EnumeratedHierarchicalNode<T>>.Current => _current;

		object IEnumerator.Current => ((IEnumerator<EnumeratedHierarchicalNode<T>>)this).Current;

		public EnumParentsEnumerator(HierarchicalStore<T> hierarchicalStore, string token)
		{
			_hierarchicalStore = hierarchicalStore;
			_token = token;
			Reset();
		}

		public bool MoveNext()
		{
			if (_currentParent == null)
			{
				_current = null;
				return false;
			}
			if (_currentParent._parent != null)
			{
				_current = new EnumeratedHierarchicalNode<T>(_currentParent._token, _currentParent._referencedObject, hasChildren: false, null);
				_currentParent = _currentParent._parent;
				return true;
			}
			_current = null;
			return false;
		}

		public void Reset()
		{
			_current = null;
			_tokenElements = _hierarchicalStore.SplitToken(_token);
			_hierarchicalStore.FindClosestNode(_token, _tokenElements, _hierarchicalStore._rootNode, out _currentParent);
		}

		void IDisposable.Dispose()
		{
		}
	}

	private char _tokenSeparator;

	private StringComparison _tokenComparison;

	private HierarchicalTreeNode<T> _rootNode;

	private int _count;

	[ThreadStatic]
	private WeakReference _closestNodeCache;

	private static readonly string[] _emptyStringArray = new string[1] { string.Empty };

	public T this[string token]
	{
		get
		{
			if (!TryGetValue(token, out var referencedObject))
			{
				throw new KeyNotFoundException();
			}
			return referencedObject;
		}
		set
		{
			Add(token, value, overwrite: true);
		}
	}

	public int Count => _count;

	private HierarchicalTreeNode<T> ClosestNodeCache
	{
		get
		{
			if (_closestNodeCache == null)
			{
				return null;
			}
			return _closestNodeCache.Target as HierarchicalTreeNode<T>;
		}
		set
		{
			if (_closestNodeCache != null)
			{
				_closestNodeCache.Target = value;
			}
			else
			{
				_closestNodeCache = new WeakReference(value, trackResurrection: false);
			}
		}
	}

	public HierarchicalStore(char tokenSeparator)
		: this(tokenSeparator, StringComparison.OrdinalIgnoreCase)
	{
	}

	public HierarchicalStore(char tokenSeparator, StringComparison tokenComparison)
	{
		Clear();
		_tokenSeparator = tokenSeparator;
		_tokenComparison = tokenComparison;
	}

	public bool TryGetValue(string token, out T referencedObject)
	{
		return TryGetValue(token, exactMatch: true, out referencedObject);
	}

	public bool TryGetValue(string token, bool exactMatch, out T referencedObject)
	{
		if (token == null)
		{
			throw new ArgumentNullException("token");
		}
		string[] tokenElements = SplitToken(token);
		if (!FindClosestNode(token, tokenElements, _rootNode, out var closestNode))
		{
			if (exactMatch || closestNode == _rootNode)
			{
				referencedObject = default(T);
				return false;
			}
			referencedObject = closestNode._referencedObject;
			return true;
		}
		referencedObject = closestNode._referencedObject;
		return true;
	}

	public void Clear()
	{
		_rootNode = new HierarchicalTreeNode<T>(string.Empty, new string[0], default(T));
		_count = 0;
	}

	private void Add(string token, T referencedObject, bool overwrite)
	{
		if (token == null)
		{
			throw new ArgumentNullException("token");
		}
		token = FormatToken(token);
		string[] tokenElements = SplitToken(token);
		if (!FindClosestNode(token, tokenElements, _rootNode, out var closestNode))
		{
			HierarchicalTreeNode<T> childNode = new HierarchicalTreeNode<T>(token, tokenElements, referencedObject);
			if (closestNode == null)
			{
				closestNode = _rootNode;
			}
			AddNode(closestNode, childNode);
		}
		else
		{
			if (!overwrite)
			{
				throw new ArgumentException("A node with the same token already exists");
			}
			closestNode._referencedObject = referencedObject;
		}
	}

	private void AddNode(HierarchicalTreeNode<T> parentNode, HierarchicalTreeNode<T> childNode)
	{
		int childCount = parentNode.ChildCount;
		ReparentNode(parentNode, childNode);
		if (childCount == 0)
		{
			parentNode._children = new List<HierarchicalTreeNode<T>>();
			parentNode._children.Add(childNode);
		}
		else
		{
			string[] tokenElements = childNode._tokenElements;
			string[] tokenElements2 = parentNode._children[childCount - 1]._tokenElements;
			int startIndex = parentNode._tokenElements.Length;
			int endIndex = parentNode._children[childCount - 1]._tokenElements.Length - 1;
			int index = ((CompareByElements(tokenElements, tokenElements2, startIndex, endIndex) <= 0) ? FindLocation(childNode._tokenElements, parentNode) : childCount);
			parentNode._children.Insert(index, childNode);
		}
		_count++;
	}

	public bool Remove(string token, bool removeChildren)
	{
		if (token == null)
		{
			throw new ArgumentNullException("token");
		}
		return false;
	}

	private static void ReparentNode(HierarchicalTreeNode<T> parentNode, HierarchicalTreeNode<T> childNode)
	{
		childNode._parent = parentNode;
		for (int i = 0; i < parentNode._tokenElements.Length; i++)
		{
			childNode._tokenElements[i] = parentNode._tokenElements[i];
		}
	}

	private string FormatToken(string token)
	{
		if (token == null)
		{
			return null;
		}
		if (_tokenSeparator != 0)
		{
			return token.TrimEnd(_tokenSeparator);
		}
		return token;
	}

	private string[] SplitToken(string token)
	{
		if (_tokenSeparator != 0)
		{
			if (token.Length == 0)
			{
				return _emptyStringArray;
			}
			return token.Split(_tokenSeparator);
		}
		return null;
	}

	public bool FindClosestNode(string token, string[] tokenElements, HierarchicalTreeNode<T> nodes, out HierarchicalTreeNode<T> closestNode)
	{
		HierarchicalTreeNode<T> closestNodeCache = ClosestNodeCache;
		if (closestNodeCache != null && closestNodeCache._parent != null && IsSubItem(token, closestNodeCache._token) && IsRooted(closestNodeCache))
		{
			nodes = closestNodeCache;
			if (closestNodeCache._tokenElements.Length == tokenElements.Length)
			{
				closestNode = closestNodeCache;
				return true;
			}
		}
		bool result = FindClosestNodeHelper(tokenElements, 0, nodes, out closestNode);
		ClosestNodeCache = closestNode;
		return result;
	}

	private bool FindClosestNodeHelper(string[] tokenElements, int elementIndex, HierarchicalTreeNode<T> nodeList, out HierarchicalTreeNode<T> closestNode)
	{
		closestNode = nodeList;
		int num = 0;
		int num2 = nodeList.ChildCount - 1;
		while (num <= num2)
		{
			HierarchicalTreeNode<T> hierarchicalTreeNode = nodeList._children[num];
			int num3 = elementIndex;
			int num4;
			while ((num4 = string.Compare(tokenElements[num3], hierarchicalTreeNode._tokenElements[num3], _tokenComparison)) == 0)
			{
				if (tokenElements.Length - 1 == num3 && hierarchicalTreeNode._tokenElements.Length == tokenElements.Length)
				{
					closestNode = hierarchicalTreeNode;
					return true;
				}
				if (hierarchicalTreeNode._tokenElements.Length - 1 == num3 && hierarchicalTreeNode._tokenElements.Length < tokenElements.Length)
				{
					closestNode = hierarchicalTreeNode;
					return FindClosestNodeHelper(tokenElements, elementIndex + 1, hierarchicalTreeNode, out closestNode);
				}
				num3++;
				if (num3 == tokenElements.Length)
				{
					break;
				}
			}
			if (num4 == 0)
			{
				return false;
			}
			if (num4 < 0)
			{
				num2 = num - 1;
			}
			else
			{
				num++;
			}
		}
		return false;
	}

	private int FindLocation(string[] tokenElements, HierarchicalTreeNode<T> nodeToSearch)
	{
		if (nodeToSearch.ChildCount == 0)
		{
			return -1;
		}
		int num = 0;
		int num2 = nodeToSearch.ChildCount - 1;
		while (num <= num2)
		{
			int num3 = CompareByElements(tokenElements, nodeToSearch._children[num]._tokenElements, nodeToSearch._tokenElements.Length, nodeToSearch._children[num]._tokenElements.Length - 1);
			if (num3 == 0)
			{
				return num;
			}
			if (num3 > 0)
			{
				num++;
			}
			else
			{
				num2 = num - 1;
			}
		}
		return num;
	}

	private int CompareByElements(string[] tokenElements1, string[] tokenElements2, int startIndex, int endIndex)
	{
		endIndex++;
		int i = startIndex;
		for (int num = Math.Min(endIndex, Math.Min(tokenElements1.Length, tokenElements2.Length)); i < num; i++)
		{
			int num2 = string.Compare(tokenElements1[i], tokenElements2[i], _tokenComparison);
			if (num2 != 0)
			{
				return num2;
			}
		}
		if (i.Equals(endIndex))
		{
			return 0;
		}
		return tokenElements1.Length - tokenElements2.Length;
	}

	public IEnumerable<EnumeratedHierarchicalNode<T>> EnumParents(string token)
	{
		return new EnumParentsEnumerable(this, token);
	}

	public bool EnumParents(string token, EnumNodeCallback callback, object param)
	{
		foreach (EnumeratedHierarchicalNode<T> item in EnumParents(token))
		{
			if (callback(item.ReferencedObject, param))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsSubItem(string item, string parent)
	{
		if (_tokenSeparator != 0 && item.StartsWith(parent, _tokenComparison))
		{
			if (item.Length != parent.Length && (parent.Length <= 0 || parent[parent.Length - 1] != _tokenSeparator))
			{
				return item[parent.Length] == _tokenSeparator;
			}
			return true;
		}
		return false;
	}

	private bool IsRooted(HierarchicalTreeNode<T> node)
	{
		while (node._parent != null)
		{
			node = node._parent;
		}
		return node == _rootNode;
	}
}
