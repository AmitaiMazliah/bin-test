using System.Diagnostics;

namespace ClassLibrary;

public static class FileUtils
{
    private static bool FileOrDirectoryExists(string name)
    {
        return Directory.Exists(name) || File.Exists(name);
    }

    public static void ChangeFilePermissions(string path, bool read, bool write)
    {
        if (!FileOrDirectoryExists(path))
        {
            throw new FileNotFoundException();
        }

        string command;
        if (read && write)
        {
            command = $"-c \" chmod o+rw  {path}\" ";
        }
        else if (read)
        {
            command = $"-c \" chmod o+r-w  {path}\" ";
        }
        else if (write)
        {
            command = $"-c \" chmod o+w-r  {path}\" ";
        }
        else
        {
            command = $"-c \" chmod o-rw  {path}\" ";
        }

        var startInfo = new ProcessStartInfo()
        {
            FileName = "/bin/bash",
            Arguments = command,
            CreateNoWindow = true
        };

        var proc = new Process { StartInfo = startInfo };
        proc.ErrorDataReceived += (sender, e) => { Console.WriteLine("error"); };
        proc.OutputDataReceived += (sender, e) => { Console.WriteLine("output"); };
        proc.Exited += (sender, e) => { Console.WriteLine("exit"); };
        proc.Start();
    }

    public static string GetFilePermissions(string path)
    {
        try
        {
            if (!FileOrDirectoryExists(path))
            {
                return null;
            }

            var startInfo = new ProcessStartInfo()
            {
                FileName = "/bin/bash",
                Arguments = $"-c \" ls -l  {path}\" ",
                CreateNoWindow = true,
                RedirectStandardOutput = true,
            };

            var proc = new Process { StartInfo = startInfo };
            proc.ErrorDataReceived += (sender, e) => { Console.WriteLine("error"); };
            proc.OutputDataReceived += (sender, e) => { Console.WriteLine("output"); };
            proc.Exited += (sender, e) => { Console.WriteLine("exit"); };
            proc.Start();

            while (!proc.StandardOutput.EndOfStream)
            {
                var line = proc.StandardOutput.ReadLine();
                return line?.Split(" ")[0];
            }

            return "";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}