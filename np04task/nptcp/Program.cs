using NP04TCP;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

var ip = IPAddress.Loopback;
var port = 27001;

var listener = new TcpListener(ip, port);
listener.Start();

while (true)
{
    var client = listener.AcceptTcpClient();
    var stream = client.GetStream();
    var br = new BinaryReader(stream);
    var cw = new BinaryWriter(stream);
    while (true)
    {
        var input = br.ReadString();
        var command = JsonSerializer.Deserialize<Command>(input);
        if (command == null) 
            continue;
        Console.WriteLine(command.Text);
        Console.WriteLine(command.Parametr);

        switch (command.Text)
        {
            case Command.ProcessList:
                var process = Process.GetProcesses();
                var processNames = JsonSerializer.
                    Serialize(process.Select(p => p.ProcessName));
                cw.Write(processNames);
                break;
            case Command.RUN:

                if (command.Parametr != null)
                {
                    var processToRun = command.Parametr;
                    cw.Write(processToRun);
                }

                break;
            case Command.Kill:
                if (command.Parametr != null)
                {
                    var processToRun = command.Parametr;
                    cw.Write(processToRun);
                }
                break;
            default: 
                break;
        }

    }
}
