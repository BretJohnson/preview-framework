using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using StreamJsonRpc;

namespace VisualTestUtils.AppConnector.App;

public class AppConnectorApp
{
    private readonly string ip;
    private readonly int port;
    private readonly AppService appService;
    private readonly ILogger? logger;
    private TcpClient? client;

    public AppConnectorApp(AppService appService, ILogger? logger = null)
    {
        this.logger = logger;
        this.appService = appService;

        this.ip = "127.0.0.1";
        this.port = 8888;
    }

    public async Task StartClientAsync()
    {
        this.client = new TcpClient();

        await this.client.ConnectAsync(this.ip, this.port);
        NetworkStream networkStream = this.client.GetStream();

        JsonRpc.Attach(networkStream, this.appService);
    }
}
