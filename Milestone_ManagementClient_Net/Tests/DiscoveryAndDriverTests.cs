using System;
using MilestoneManagementClient.Core;

namespace MilestoneManagementClient.Tests
{
    public class DiscoveryAndDriverTests
    {
        public static bool Run()
        {
            Console.WriteLine("--- [RUNNING DiscoveryAndDriverTests] ---");

            // Test MAC OUI resolution
            string brand1 = OnvifDeviceProber.ResolveBrandFromMac("00:1A:8C:11:22:33");
            if (brand1 != "Hikvision")
            {
                Console.WriteLine("FAIL: MAC resolution failed for Hikvision! Got: " + brand1);
                return false;
            }
            Console.WriteLine("PASS: Hikvision MAC OUI resolved.");

            string brand2 = OnvifDeviceProber.ResolveBrandFromMac("38:AF:29:AA:BB:CC");
            if (brand2 != "Dahua Technology")
            {
                Console.WriteLine("FAIL: MAC resolution failed for Dahua! Got: " + brand2);
                return false;
            }
            Console.WriteLine("PASS: Dahua MAC OUI resolved.");

            string brand3 = OnvifDeviceProber.ResolveBrandFromMac("00:40:8C:99:88:77");
            if (brand3 != "Axis Communications")
            {
                Console.WriteLine("FAIL: MAC resolution failed for Axis! Got: " + brand3);
                return false;
            }
            Console.WriteLine("PASS: Axis MAC OUI resolved.");

            // Test Driver registry mapping
            var driverMeta = HardwareDriverRegistry.GetDriverByBrand("Hikvision");
            if (driverMeta.Brand != "Hikvision" || !driverMeta.DriverName.Contains("HikVision"))
            {
                Console.WriteLine("FAIL: Driver registry mismatch for Hikvision!");
                return false;
            }
            Console.WriteLine("PASS: Driver registry lookup verified -> " + driverMeta.DriverName);

            Console.WriteLine("SUCCESS: DiscoveryAndDriverTests Passed!\n");
            return true;
        }
    }
}
