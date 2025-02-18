using System.Diagnostics;

namespace ExampleFramework.App.Maui;

public class ExamplesWindowOverlay : WindowOverlay
{
    private ExamplesWindowOverlayElement _overlayElement;

    private Color? _badgeColor;
    
    public ExamplesWindowOverlay(IWindow window, Color? badgeColor = null) : base(window)
    {
        _badgeColor = badgeColor;
        _overlayElement = new ExamplesWindowOverlayElement(this, badgeColor: badgeColor);
        AddWindowElement(_overlayElement);
        Tapped += ExamplesOverlay_Tapped;
    }

    private void ExamplesOverlay_Tapped(object? sender, WindowOverlayTappedEventArgs e)
    {
        if (_overlayElement.Contains(e.Point))
        {
            _ = MauiExamplesApplication.Instance.ShowExamplesAsync();

            /*
            // The tap is on the overlayElement
            this.RemoveWindowElement(overlayElement);
            overlayElement = new ExamplesWindowOverlayElement(this, badgeColor);
            this.AddWindowElement(overlayElement);
            */
        }
        else
        {
            // The tap is not on the overlayElement
        }
    }

    public override void HandleUIChange()
    {
        base.HandleUIChange();
    }
}
