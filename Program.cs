using System.Diagnostics;

namespace LimitNinjaWorkers
{
    internal class Program
    {
        static string kConfigName = "NinjaArg.cfg";

        static void Main(string[] args)
        {
            string szArgs = string.Join(' ', args).Trim();
            string szArgsExtra = "";

            string? exeDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName);
            string szConfigPath = $"{exeDir}\\{kConfigName}";

            if (!File.Exists(szConfigPath))
            {
                Console.WriteLine($"Can not find file '{szConfigPath}'");
            }
            else
            {
                // 给 ninja.exe 附加额外的启动参数
                szArgsExtra = File.ReadAllText(szConfigPath);
                szArgs += " ";
                szArgs += szArgsExtra;
            }

            Console.WriteLine($"Run Ninja with args:{szArgs}, extra:{szArgsExtra}");

            string szLog = $"{DateTime.Now.ToString()}\t args:{szArgs}, extra:{szArgsExtra}\r\n";
            File.WriteAllText($"{exeDir}\\ninja.log", szLog);

            Process process = new Process();
            process.StartInfo.FileName = $"{exeDir}\\ninja_ori.exe";
            process.StartInfo.Arguments = szArgs;
            process.Start();
            
            // android studio 结束 ninja 进程时只会结束其直接子进程，这会导致 clang++.exe 无法被结束，我们使用Job来解决
            var job = new JobManagement.Job();
            job.AddProcess(process.Handle);

            process.WaitForExit();
        }
    }
}
