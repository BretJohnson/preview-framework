using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using StreamJsonRpc;

namespace VisualTestUtils.AppConnector.App;

public class AppConnectorApp
{
    private string _ip;
    private int _port;
    private AppService _appService;
    private ILogger? _logger;
    private TcpClient? _client;

    public AppConnectorApp(AppService appService, ILogger? logger = null)
    {
        _logger = logger;
        _appService = appService;

        _ip = "127.0.0.1";
        _port = 8888;
    }

    public async Task StartClientAsync()
    {
        _client = new TcpClient();

        await _client.ConnectAsync(_ip, _port);
        NetworkStream networkStream = _client.GetStream();

        JsonRpc.Attach(networkStream, _appService);
    }
}
