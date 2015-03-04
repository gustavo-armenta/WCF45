﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName = "ILabUsersPasswords")]
public interface ILabUsersPasswords
{

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ILabUsersPasswords/GetPassword", ReplyAction = "http://tempuri.org/ILabUsersPasswords/GetPasswordResponse")]
    string GetPassword(string userFullName);

    [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/ILabUsersPasswords/GetPassword", ReplyAction = "http://tempuri.org/ILabUsersPasswords/GetPasswordResponse")]
    System.Threading.Tasks.Task<string> GetPasswordAsync(string userFullName);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public interface ILabUsersPasswordsChannel : ILabUsersPasswords, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
public partial class LabUsersPasswordsClient : System.ServiceModel.ClientBase<ILabUsersPasswords>, ILabUsersPasswords
{

    public LabUsersPasswordsClient()
    {
    }

    public LabUsersPasswordsClient(string endpointConfigurationName) :
        base(endpointConfigurationName)
    {
    }

    public LabUsersPasswordsClient(string endpointConfigurationName, string remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public LabUsersPasswordsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public LabUsersPasswordsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
        base(binding, remoteAddress)
    {
    }

    public string GetPassword(string userFullName)
    {
        return base.Channel.GetPassword(userFullName);
    }

    public System.Threading.Tasks.Task<string> GetPasswordAsync(string userFullName)
    {
        return base.Channel.GetPasswordAsync(userFullName);
    }
}
