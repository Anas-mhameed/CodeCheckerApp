using IronPython.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace CheckingFiles.MyClass
{
    public class Checker
    {

        static public List<string> Option1()
        {
            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\HP\Envs\app1\Scripts\python.exe";

            var script = @"C:\Users\HP\Desktop\p.py";
            psi.Arguments = $"\"{script}\"";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var errors = "";
            var result = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                result = process.StandardOutput.ReadToEnd();
            }

            //Console.WriteLine("ERRORS:");
            //Console.WriteLine(errors);
            //Console.WriteLine();
            //Console.WriteLine("RESULT");
            //Console.WriteLine(result);
            
            return new List<string>() { result, errors };
        }

        static public List<string> Option2(string script)
        {
            var engine = Python.CreateEngine();

            var source = engine.CreateScriptSourceFromFile(script);

            var argv = new List<string>();
            argv.Add("");

            engine.GetSysModule().SetVariable("argv", argv);

            var eIO = engine.Runtime.IO;

            var errors = new MemoryStream();
            eIO.SetErrorOutput(errors, Encoding.Default);

            var result = new MemoryStream();
            eIO.SetOutput(result, Encoding.Default);

            var scope = engine.CreateScope();

            
            source.Execute(scope);

            string str(byte[] x) => Encoding.Default.GetString(x);

            return new List<string>() { str(result.ToArray()), str(errors.ToArray()) };
}


    }
}
