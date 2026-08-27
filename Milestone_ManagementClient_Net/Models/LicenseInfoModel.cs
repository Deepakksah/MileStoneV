using System;

namespace MilestoneManagementClient.Models
{
    public class LicenseInfoModel
    {
        public string SoftwareLicenseCode { get; set; }
        public string ProductName { get; set; }
        public string Version { get; set; }
        public string LicenseStatus { get; set; }
        public int HardwareDeviceLicensesUsed { get; set; }
        public int HardwareDeviceLicensesPurchased { get; set; }
        public int CarePlusStatus { get; set; } // 1 = Active, 0 = Expired
        public DateTime ExpirationDate { get; set; }
        public int GracePeriodDaysRemaining { get; set; }

        public LicenseInfoModel()
        {
            SoftwareLicenseCode = "M01-DEMO-100CAM-2025-XPROTECT";
            ProductName = "Milestone XProtect Corporate 2025 R3 (100 Camera Demo Pack)";
            Version = "25.3a (Build 100)";
            LicenseStatus = "Activated (100 Camera Demo Enterprise License)";
            HardwareDeviceLicensesUsed = 26;
            HardwareDeviceLicensesPurchased = 100;
            CarePlusStatus = 1;
            ExpirationDate = DateTime.Now.AddYears(1);
            GracePeriodDaysRemaining = 30;
        }
    }
}
