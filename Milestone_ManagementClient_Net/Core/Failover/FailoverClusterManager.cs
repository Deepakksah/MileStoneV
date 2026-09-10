using System;
using System.Collections.Generic;
using System.Linq;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Core.Failover
{
    public class FailoverClusterManager
    {
        private readonly List<FailoverServerModel> _failoverNodes = new List<FailoverServerModel>();

        public FailoverClusterManager()
        {
            InitializeFailoverNodes();
        }

        private void InitializeFailoverNodes()
        {
            _failoverNodes.Add(new FailoverServerModel
            {
                ServerName = "LYCHEE-FAILOVER-HOT-01",
                HostIp = "192.168.1.150",
                Mode = FailoverMode.HotStandby,
                AssignedPrimaryServer = "JSPANGICCCREC01.JSPLICCC.LOCAL",
                IsActiveTakingOver = false
            });

            _failoverNodes.Add(new FailoverServerModel
            {
                ServerName = "LYCHEE-FAILOVER-HOT-02",
                HostIp = "192.168.1.151",
                Mode = FailoverMode.HotStandby,
                AssignedPrimaryServer = "JSPANGICCCREC02.JSPLICCC.LOCAL",
                IsActiveTakingOver = false
            });
        }

        public List<FailoverServerModel> GetNodes() { return _failoverNodes; }

        public void TriggerFailover(Guid nodeId)
        {
            var node = _failoverNodes.FirstOrDefault(n => n.Id == nodeId);
            if (node != null)
            {
                node.IsActiveTakingOver = !node.IsActiveTakingOver;
            }
        }
    }
}
