using Application.Interfaces;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;

public class RabbitMqConnectionManager: IRabbitMqConnectionManager
{
    private readonly ConnectionFactory _factory;

    public RabbitMqConnectionManager(string hostName, string userName, string password)
    {
        _factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };
    }

    public async Task<IConnection> GetConnectionAsync()
    {
        return await _factory.CreateConnectionAsync();
    }
    
}