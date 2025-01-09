using RabbitMQ.Client;

namespace Application.Interfaces;

public interface IRabbitMqConnectionManager
{
    Task<IConnection> GetConnectionAsync();

}
