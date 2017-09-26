using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
public class ResultMaker
{
    private const string TEMP_DIR = "temp";
    private static int COUNTER = 0;
    private string GET_COUNTER_PATH => $"{TEMP_DIR}/wd_{COUNTER++}/";
    public static void Init()
    {
        Console.WriteLine("ResultMaker.Init");

        if (Directory.Exists(TEMP_DIR))
        {
            foreach (var filename in Directory.GetFiles(TEMP_DIR))
                File.Delete(filename);
            foreach (var dirname in Directory.GetDirectories(TEMP_DIR))
                Directory.Delete(dirname, true);
        }
        else Directory.CreateDirectory(TEMP_DIR);

        if (!File.Exists("wrap.o"))
        {
            Process gcc = new Process();
            gcc.StartInfo.FileName = "gcc";
            gcc.StartInfo.Arguments = "-c wrap.c";
            gcc.Start();
        }
    }

    public readonly string InitFailLog = string.Empty;
    public readonly bool isInitSuccess = false;
    private readonly string workDir;
    public ResultMaker(string source, string testcase)
    {
        workDir = GET_COUNTER_PATH;
        Directory.CreateDirectory(workDir);
        File.WriteAllText(workDir + "main.c", source);
        File.WriteAllText(workDir + "test.txt", testcase);
        File.WriteAllText(workDir + "log.txt", DateTime.Now.ToString() + "\n");

        if (!mainCompile())
        {
            InitFailLog = "main.c 컴파일 실패";
            return;
        }

        if (!linkCompile())
        {
            InitFailLog = "object 링크 실패";
            return;
        }
        
        isInitSuccess = true;
        return;
        bool mainCompile()
        {
            Process gcc = new Process();
            gcc.StartInfo.FileName = "gcc";
            gcc.StartInfo.Arguments = $"-c {workDir + "main.c"} -o {workDir + "main.o"}";
            gcc.Start();
            gcc.WaitForExit();
            return gcc.ExitCode == 0;
        }
        bool linkCompile()
        {
            Process gcc = new Process();
            gcc.StartInfo.FileName = "gcc";
            gcc.StartInfo.Arguments = $"-Wl,-wrap=malloc -Wl,-wrap=free wrap.o {workDir + "main.o"} -o {workDir + "program"}";
            gcc.Start();
            gcc.WaitForExit();
            return gcc.ExitCode == 0;
        }
    }

    public void runProgram()
    {
        Process prog = new Process();
        prog.StartInfo.FileName = "bash";
        prog.StartInfo.Arguments = $"-c \"{workDir}program < {workDir}test.txt\"";
        prog.Start();
        const int SECOND = 1000;
        prog.WaitForExit(10*SECOND);
    }
    public string getMemoryLog() =>
        File.ReadAllText(workDir + "log.txt");
}