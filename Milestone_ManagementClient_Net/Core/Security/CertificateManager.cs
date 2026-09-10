using System;
using System.Collections.Generic;
using System.Linq;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Core.Security
{
    public class CertificateManager
    {
        private readonly List<CertificateModel> _certificates = new List<CertificateModel>();

        public CertificateManager()
        {
            InitializeCertificates();
        }

        private void InitializeCertificates()
        {
            _certificates.Add(new CertificateModel
            {
                Subject = "CN=lychee-cluster.local, OU=Milestone VMS, O=i-Comply",
                Issuer = "CN=Milestone VMS Root CA 2025",
                Thumbprint = "4F 8A 99 21 BB 02 AA 14 55 9C EE 30 12 FA 09 88 C1 22 DF 77",
                ServiceBinding = "Management Server HTTPS (Port 443 / 7563)",
                IsBound = true
            });

            _certificates.Add(new CertificateModel
            {
                Subject = "CN=JSPANGICCCREC01.JSPLICCC.LOCAL",
                Issuer = "CN=Milestone VMS Intermediate Device CA",
                Thumbprint = "7A 19 E0 44 88 F1 23 99 10 BA CC 45 90 EF 33 21 00 14 78 92",
                ServiceBinding = "Recording Server Secure gRPC / TLS",
                IsBound = true
            });

            _certificates.Add(new CertificateModel
            {
                Subject = "CN=Milestone Mobile & WebRTC Gateway Portal",
                Issuer = "CN=Let's Encrypt Authority X3",
                Thumbprint = "9B C2 11 00 44 55 88 77 22 13 90 FE AC 88 12 34 56 78 90 AB",
                ServiceBinding = "Mobile Server Gateway (Port 8082)",
                IsBound = true
            });
        }

        public List<CertificateModel> GetCertificates() { return _certificates; }

        public void BindCertificate(Guid certId, string serviceName)
        {
            var cert = _certificates.FirstOrDefault(c => c.Id == certId);
            if (cert != null)
            {
                cert.ServiceBinding = serviceName;
                cert.IsBound = true;
            }
        }
    }
}
