// See https://aka.ms/new-console-template for more information

using ClassLibrary;

Console.WriteLine("Hello");
Console.WriteLine("Welcome to Bindecy app");
Console.WriteLine("This app can execute 2 commands. register and unregister via test, examples below");
Console.WriteLine("");
Console.WriteLine("register secret.text true false");
Console.WriteLine("unregister 1");

while (true)
{
    var command = Console.ReadLine();
    if (command == null)
    {
        continue;
    }

    var parts = command.Split(" ");

    if (parts[0] == "register")
    {
        if (parts.Length != 4)
        {
            Console.WriteLine("Bad command format");
            continue;
        }
        var path = parts[1];
        var read = bool.Parse(parts[2]);
        var write = bool.Parse(parts[3]);

        var handle = Engine.Register(path, read, write);
        if (handle != -1)
        {
            Console.WriteLine($"Register completed, id = {handle}");
        }
        else
        {
            Console.WriteLine("Register failed");
        }
    }
    else if (parts[0] == "unregister")
    {
        if (parts.Length != 2)
        {
            Console.WriteLine("Bad command format");
            continue;
        }

        var handle = int.Parse(parts[1]);
        Engine.Unregister(handle);
        Console.WriteLine("Unregister completed");
    }
    else
    {
        Console.WriteLine("Bad command");
    }
}
