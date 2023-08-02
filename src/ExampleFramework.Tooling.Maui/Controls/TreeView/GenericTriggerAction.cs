namespace ExampleFramework.Tooling.Maui.Controls.TreeView;

public class GenericTriggerAction<T> : TriggerAction<T>
    where T : BindableObject
{
    private readonly Action<T> _action;

    public GenericTriggerAction(Action<T> action)
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        _action = action;
    }

    protected override void Invoke(T sender)
    {
        _action(sender);
    }
}
