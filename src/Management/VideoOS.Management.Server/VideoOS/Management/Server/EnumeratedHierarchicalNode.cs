namespace VideoOS.Management.Server;

public class EnumeratedHierarchicalNode<T>
{
	public bool HasChildren { get; set; }

	public string NoChildrenBelow { get; set; }

	public T ReferencedObject { get; set; }

	public string Token { get; set; }

	public EnumeratedHierarchicalNode(string token, T referencedObject, bool hasChildren, string noChildrenBelow)
	{
		Token = token;
		ReferencedObject = referencedObject;
		HasChildren = hasChildren;
		NoChildrenBelow = noChildrenBelow;
	}
}
