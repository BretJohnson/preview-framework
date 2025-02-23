using System.Reflection;

namespace Microsoft.PreviewFramework.App;

public class StaticMethodUIPreview : AppPreview
{
    public MethodInfo MethodInfo { get; }

    public StaticMethodUIPreview(PreviewAttribute previewAttribute, MethodInfo methodInfo) : base(previewAttribute)
    {
        this.MethodInfo = methodInfo;
    }

    public override object Create()
    {
        if (this.MethodInfo.GetParameters().Length != 0)
            throw new InvalidOperationException($"Previews that take parameters aren't yet supported: {this.Name}");

        return this.MethodInfo.Invoke(null, null);
    }

    public override Type? DefaultUIComponentType => this.MethodInfo.ReturnType;

    /// <summary>
    /// FullName is intended to be what's used by the code to identify the preview. It's the preview's
    /// full qualified method name.
    /// </summary>
    public override string Name => this.MethodInfo.DeclaringType.FullName + "." + this.MethodInfo.Name;
}
