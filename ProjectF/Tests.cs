using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleApplication1
{
    internal class Tests
    {
        public static void Main(string[] args)
        {
            int total = 0;
            int passed = 0;

            DirectoryInfo d =
                new DirectoryInfo(
                    @"D:/Homeworks/Compilers/CompilersProject/ConsoleApplication1/ConsoleApplication1/Tests");
            FileInfo[] inputFiles = d.GetFiles("in_*.txt");
            FileInfo[] outputFiles = d.GetFiles("out_*.txt");

            for (int i = 0; i < inputFiles.Length; i++)
            {
                string testName = inputFiles[i].FullName.Replace("in_"+i+"_", "").Replace(".txt", "");

                // Read the input and pass to program
                string input = File.ReadAllText(inputFiles[i].FullName);
                System.IO.File.WriteAllText(
                    @"D:/Homeworks/Compilers/CompilersProject/ConsoleApplication1/ConsoleApplication1/input_code.f",
                    input);

                // Launch program and wait until termination
                using (Process proc =
                    Process.Start(
                        @"D:/Homeworks/Compilers/CompilersProject/ConsoleApplication1/ConsoleApplication1/ProjectF.exe")
                )
                {
                    proc.WaitForExit();
                }

                // Read the actual output and compare with expected
                FileInfo realOutput =
                    new FileInfo(
                        "D:/Homeworks/Compilers/CompilersProject/ConsoleApplication1/ConsoleApplication1/output.c");
                if (FilesAreEqual(outputFiles[i], realOutput))
                {
                    passed++;
                    Console.WriteLine("[PASSED] Test " + testName + " is passed.");
                }
                else
                {
                    Console.WriteLine("[ERROR] Test " + testName + " is NOT passed.");
                }

                total++;
            }

            Console.WriteLine("Tests total: " + total);
            Console.WriteLine("Tests passed: " + passed);
        }

        const int BYTES_TO_READ = sizeof(Int64);

        public static bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
                return false;

//            if (string.Equals(first.FullName, second.FullName, StringComparison.OrdinalIgnoreCase))
//                                            return true;

            int iterations = (int) Math.Ceiling((double) first.Length / BYTES_TO_READ);

            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                byte[] one = new byte[BYTES_TO_READ];
                byte[] two = new byte[BYTES_TO_READ];

                for (int i = 0; i < iterations; i++)
                {
                    fs1.Read(one, 0, BYTES_TO_READ);
                    fs2.Read(two, 0, BYTES_TO_READ);

                    if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                        return false;
                }
            }

            return true;
        }
    }
}
