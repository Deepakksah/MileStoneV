using System;

namespace MilestoneManagementClient.Tests
{
    public class TestRunnerMain
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("  MILESTONE XPROTECT MANAGEMENT CLIENT - AUTOMATED TEST SUITE");
            Console.WriteLine("============================================================\n");

            bool p1 = LiveStreamingTests.Run();
            bool p2 = RecordingStorageTests.Run();
            bool p3 = DiscoveryAndDriverTests.Run();

            Console.WriteLine("============================================================");
            if (p1 && p2 && p3)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  >>> [ALL 3 TEST SUITES PASSED SUCCESSFULLY - 100% OK] <<<");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  >>> [SOME TESTS FAILED] <<<");
                Console.ResetColor();
            }
            Console.WriteLine("============================================================");
        }
    }
}
