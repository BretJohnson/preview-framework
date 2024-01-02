// <copyright file="MainViewModel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Drastic.Tempest;
using Drastic.Tools;
using Drastic.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VisualTestUtils.AppConnector;

namespace SandboxServer.ViewModels;

/// <summary>
/// Main View Model.
/// </summary>
public class MainViewModel : BaseViewModel
{
    private NetworkInterface? _selectedInterface;
    private SandboxController? _controller;
    private int? port = 8888;
    private ILogger? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    /// <param name="services">Services.</param>
    public MainViewModel(IServiceProvider services)
        : base(services)
    {
        ILoggerProvider? loggerFactory = services.GetService<ILoggerProvider>();
        if (loggerFactory is not null)
        {
            this._logger = loggerFactory.CreateLogger("Server");
        }

        IEnumerable<NetworkInterface> test = NetworkUtils.GoodInterfaces();
        this.NetworkInterfaces = new ObservableCollection<NetworkInterface>();
    }

    public SandboxController? Controller => _controller;

    public ObservableCollection<NetworkInterface> NetworkInterfaces { get; }

    public IPAddress? IPAddress
    {
        get
        {
            if (_selectedInterface is not null)
            {
                return _selectedInterface.GetIPProperties().UnicastAddresses.Where(y => y.Address.AddressFamily == AddressFamily.InterNetwork)
                            .Select(y => y.Address).First();
            }

            return null;
        }
    }

    /// <summary>
    /// Gets or sets the selected port.
    /// </summary>
    public int? Port
    {
        get => port;

        set => SetProperty(ref this.port, value);
    }

    /// <summary>
    /// Gets or sets the selected interface.
    /// </summary>
    public NetworkInterface? SelectedInterface
    {
        get => _selectedInterface;

        set => SetProperty(ref this._selectedInterface, value);
    }

    /// <inheritdoc/>
    public override async Task OnLoad()
    {
        await base.OnLoad();
        this.Reload();
    }

    public void Reload()
    {
        this.NetworkInterfaces.Clear();

        foreach (NetworkInterface item in NetworkUtils.GoodInterfaces())
        {
            this.NetworkInterfaces.Add(item);
        }

        this.SelectedInterface = this.NetworkInterfaces.LastOrDefault();
    }

    public Task StartServerAsync(NetworkInterface netInterface)
    {
        if (_controller is not null)
        {
            throw new InvalidOperationException("Server is already running.");
        }

        _controller = new SandboxController(ipAddress: this.IPAddress!, logger: this._logger);
        _logger?.LogInformation($"Server Started");
        return _controller.InitializeAsync(TimeSpan.FromSeconds(120));
    }

    public async Task StartServerAsync(IPAddress ipAddress, int port)
    {
        if (_controller is not null)
        {
            throw new InvalidOperationException("Server is already running.");
        }

        _controller = new SandboxController(ipAddress: ipAddress, port: port, logger: _logger);
        _logger?.LogInformation($"Starting server and waiting for connection");
        await _controller.InitializeAsync(TimeSpan.FromSeconds(120));

        _logger?.LogInformation($"Connected");
    }

    public void StopServer()
    {
        if (this._controller is null)
        {
            return;
        }

        _logger?.LogInformation($"Server Stopped");
        _controller.Dispose();
        _controller = null;
    }
}
