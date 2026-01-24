namespace FIAP.CloudGames.Games.Domain.Configurations;

public class RabbitMqSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ExchangeName { get; set; } = null!;
    public string QueueName { get; set; } = null!;
    public string RetryQueueName { get; set; } = null!;
    public string FailQueueName { get; set; } = null!;
    public string RoutingKey { get; set; } = null!;
}