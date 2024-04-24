namespace ClassLibrary;

public static class Engine
{
    private static Dictionary<string, List<RegisterOperation>> registerOperationsByPath = new();

    private static Dictionary<int, RegisterOperation> registerOperationsById = new();

    private static int registerOperationCount;
    
    /// <summary>
    /// Register the file or directory in the requested path and changes it's permissions
    /// Returns a request id if success, if failed returns -1
    /// </summary>
    /// <param name="path"></param>
    /// <param name="read"></param>
    /// <param name="write"></param>
    /// <returns></returns>
    public static int Register(string path, bool read, bool write)
    {
        try
        {
            if (!registerOperationsByPath.ContainsKey(path))
            {
                registerOperationsByPath.Add(path, new List<RegisterOperation>());
            }
            
            var allFileOperations = registerOperationsByPath.GetValueOrDefault(path)!;
            
            var id = ++registerOperationCount;
            var operation = new RegisterOperation()
            {
                Id = id,
                Path = path,
                Read = read,
                Write = write
            };
            allFileOperations.Add(operation);
            registerOperationsById.Add(id, operation);

            ExecuteFilePermissionsChange(path);
                
            return id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
    }

    public static void Unregister(int handle)
    {
        var operation = registerOperationsById.GetValueOrDefault(handle);

        if (operation == null)
        {
            Console.WriteLine("Unregister on handle that doesn't exists");
            return;
        }

        var path = operation.Path;

        registerOperationsByPath.GetValueOrDefault(path)!.Remove(operation);

        registerOperationsById.Remove(handle);
        
        ExecuteFilePermissionsChange(path);
    }

    private static void ExecuteFilePermissionsChange(string path)
    {
        var allFileOperations = registerOperationsByPath.GetValueOrDefault(path)!;

        var shouldHaveRead = allFileOperations.Any(o => o.Read);
        var shouldHaveWrite = allFileOperations.Any(o => o.Write);
            
        FileUtils.ChangeFilePermissions(path, shouldHaveRead, shouldHaveWrite);
    }
}
