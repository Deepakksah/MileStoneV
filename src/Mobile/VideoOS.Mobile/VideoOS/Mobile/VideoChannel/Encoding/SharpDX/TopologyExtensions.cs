using System;
using SharpDX;
using SharpDX.MediaFoundation;

namespace VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

public static class TopologyExtensions
{
	public static void CreateAddNode(this Topology topology, out TopologyNode topologyNode, TopologyType topologyType, Action<TopologyNode> initilizer = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		MediaFactory.CreateTopologyNode(topologyType, ref topologyNode);
		initilizer?.Invoke(topologyNode);
		topology.AddNode(topologyNode);
	}

	public static void CreateAddNode(this Topology topology, out TopologyNode topologyNode, TopologyType topologyType, ComObject comObject)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		topology.CreateAddNode(out topologyNode, topologyType, (Action<TopologyNode>)delegate(TopologyNode node)
		{
			node.Object = comObject;
		});
	}

	public static void AddTransformNode(this Topology topology, out TopologyNode topologyNode, ComObject transform)
	{
		topology.CreateAddNode(out topologyNode, (TopologyType)2, transform);
	}

	public static void AddSinkNode(this Topology topology, out TopologyNode topologyNode, ComObject sink)
	{
		topology.CreateAddNode(out topologyNode, (TopologyType)0, sink);
		((MediaAttributes)topologyNode).Set<int>(((MediaAttributeKey)TopologyNodeAttributeKeys.NoshutdownOnRemove).Guid, 1);
	}
}
