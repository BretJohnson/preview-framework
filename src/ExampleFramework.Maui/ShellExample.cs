namespace ExampleFramework.Maui;

public class ShellExample
{
    public string Route { get; }
    public IDictionary<string, object> Parameters{ get; }

    public ShellExample(string route, IDictionary<string, object> parameters)
    {
        this.Route = route;
        this.Parameters = parameters;
    }

    public ShellExample(string route)
    {
        this.Route = route;
        this.Parameters = new Dictionary<string, object>
        {
        };
    }

    public ShellExample(string route, string parameter1, object value1)
    {
        this.Route = route;
        this.Parameters = new Dictionary<string, object>
        {
            { parameter1, value1 }
        };
    }

    public ShellExample(string route, string parameter1, object value1, string parameter2, object value2)
    {
        this.Route = route;
        this.Parameters = new Dictionary<string, object>
        {
            { parameter1, value1 },
            { parameter2, value2 }
        };
    }
}
