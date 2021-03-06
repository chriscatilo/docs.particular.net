﻿using NServiceBus;
using NServiceBus.AzureServiceBus.Addressing;

class MultipleNamespaces
{
    public void SingleNamespaceStrategyWithAddNamespace(EndpointConfiguration endpointConfiguration)
    {
        #region single_namespace_partitioning_strategy_with_add_namespace

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var namespacePartitioning = transport.NamespacePartitioning();
        namespacePartitioning.UseStrategy<SingleNamespacePartitioning>();
        namespacePartitioning.AddNamespace("namespace", "Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }

    public void SingleNamespaceStrategyWithDefaultConnectionString(EndpointConfiguration endpointConfiguration)
    {
        #region single_namespace_partitioning_strategy_with_default_connection_string

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString("Endpoint=sb://namespace.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }

    public void RoundRobinNamespacePartitioning(EndpointConfiguration endpointConfiguration)
    {
        #region round_robin_partitioning_strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var namespacePartitioning = transport.NamespacePartitioning();
        namespacePartitioning.AddNamespace("namespace1", "Endpoint=sb://namespace1.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");
        namespacePartitioning.AddNamespace("namespace2", "Endpoint=sb://namespace2.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");
        namespacePartitioning.AddNamespace("namespace3", "Endpoint=sb://namespace3.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }

    public void FailOverNamespacePartitioning(EndpointConfiguration endpointConfiguration)
    {
        #region fail_over_partitioning_strategy

        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        var namespacePartitioning = transport.NamespacePartitioning();
        namespacePartitioning.UseStrategy<FailOverNamespacePartitioning>();
        namespacePartitioning.AddNamespace("primary", "Endpoint=sb://primary.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");
        namespacePartitioning.AddNamespace("secondary", "Endpoint=sb://secondary.servicebus.windows.net;SharedAccessKeyName=[shared access key name];SharedAccessKey=[shared access key]");

        #endregion
    }
}