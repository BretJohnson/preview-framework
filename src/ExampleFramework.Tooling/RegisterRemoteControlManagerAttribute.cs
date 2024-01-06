﻿namespace ExampleFramework.Tooling;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public class RegisterRemoteControlManagerAttribute : Attribute
{
    public Type RemoteControlManagerType { get; }

    public RegisterRemoteControlManagerAttribute(Type remoteControlManagerType)
    {
        this.RemoteControlManagerType = remoteControlManagerType;
    }
}
