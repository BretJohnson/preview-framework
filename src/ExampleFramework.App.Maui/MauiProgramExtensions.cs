using Microsoft.Maui.Handlers;
using System.Diagnostics;

namespace ExampleFramework.App.Maui;

public static class MauiProgramExtensions
{
	public static MauiAppBuilder UseExamplesOverlay(this MauiAppBuilder builder, Color? ribbonColor = null)
	{
		builder.ConfigureMauiHandlers(handlers =>
		{
			WindowHandler.Mapper.AppendToMapping("AddDebugOverlay", (handler, view) =>
            {
                Debug.WriteLine("Adding DebugOverlay");
                var overlay = new ExamplesWindowOverlay(handler.VirtualView, ribbonColor);
                handler.VirtualView.AddOverlay(overlay);
            });
		});

		return builder;
	}
}
