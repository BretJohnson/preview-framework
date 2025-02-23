namespace Microsoft.PreviewFramework.App;

public abstract class AppPreview : Preview
{
    private readonly Type? uiComponentType;
    //private Dictionary<string, ImageSnapshot?>? _snapshotsByEnvironment;

    public AppPreview(PreviewAttribute previewAttribute) : base(previewAttribute.DisplayName)
    {
        this.uiComponentType = previewAttribute.UIComponentType;
    }

    public AppPreview(Type uiComponentType) : base(null)
    {
        this.uiComponentType = uiComponentType;
    }

    /// <summary>
    /// Create an instance of the preview. Normally this returns an instance of a UI framework control/page, suitable
    /// for display.
    /// </summary>
    /// <returns>instantiated preview</returns>
    public abstract object Create();

    public Type UIComponentType
    {
        get
        {
            if (this.uiComponentType != null)
            {
                return this.uiComponentType;
            }

            Type? defaultUIComponentType = this.DefaultUIComponentType;
            if (defaultUIComponentType == null)
                throw new InvalidOperationException($"No DefaultUIComponentType specified for example: {this.Name}");
            else return defaultUIComponentType;
        }
    }

    /// <summary>
    /// Default component type (when there is one), e.g. based on the method return type. If there's no default
    /// type, this will be null.
    /// </summary>
    public abstract Type? DefaultUIComponentType { get; }
}
