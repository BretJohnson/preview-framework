namespace SandboxApp;

public partial class TestPage1 : ContentPage
{
    public TestPage1()
    {
        InitializeComponent();
    }
    
#if EXAMPLES
    [UIExample("Example With Custom Params")]
    public static TestPage1 Example1() => new TestPage1();
#endif
}
