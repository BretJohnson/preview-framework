using System.Diagnostics;

namespace ExampleFramework.Maui;

public class ExamplesWindowOverlay : WindowOverlay
{
    private ExamplesWindowOverlayElement overlayElement;

    private Color? badgeColor;
    
    public ExamplesWindowOverlay(IWindow window, Color? badgeColor = null) : base(window)
    {
        this.badgeColor = badgeColor;
        overlayElement = new ExamplesWindowOverlayElement(this, badgeColor: badgeColor);
        this.AddWindowElement(overlayElement);
        this.Tapped += this.ExamplesOverlay_Tapped;
    }

    private void ExamplesOverlay_Tapped(object? sender, WindowOverlayTappedEventArgs e)
    {
        if (overlayElement.Contains(e.Point))
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
