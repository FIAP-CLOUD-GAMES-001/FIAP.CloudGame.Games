using FIAP.CloudGames.Games.Domain.Configurations;
using FIAP.CloudGames.Games.Domain.Interfaces.Services;
using FIAP.CloudGames.Games.Domain.Requests.Payment;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FIAP.CloudGames.Games.Api.Consumers;

public class PaymentNotificationConsumer : BackgroundService
{
    private readonly ILogger<PaymentNotificationConsumer> _logger;
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _connection;
    private IModel? _channel;

    public PaymentNotificationConsumer(
        ILogger<PaymentNotificationConsumer> logger,
        IOptions<RabbitMqSettings> rabbitMqSettings,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _rabbitMqSettings = rabbitMqSettings.Value;
        _serviceProvider = serviceProvider;

        _logger.LogInformation("PaymentNotificationConsumer constructor called");
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PaymentNotificationConsumer StartAsync called");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PaymentNotificationConsumer StopAsync called");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PaymentNotificationConsumer iniciado");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                InitializeRabbitMq();

                var consumer = new AsyncEventingBasicConsumer(_channel!);
                consumer.Received += async (_, ea) => await ProcessMessageAsync(ea, stoppingToken);

                _channel!.BasicConsume(
                    queue: _rabbitMqSettings.QueueName,
                    autoAck: false,
                    consumer: consumer);

                _logger.LogInformation("Consumidor ativo na fila: {QueueName}", _rabbitMqSettings.QueueName);

                while (!stoppingToken.IsCancellationRequested &&
                       _connection != null && _connection.IsOpen)
                {
                    await Task.Delay(5000, stoppingToken);
                }

                _logger.LogWarning("Conexão perdida. Tentando reconectar...");
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na conexão ou consumo RabbitMQ. Tentando novamente...");
            }
            finally
            {
                try { _channel?.Close(); } catch { }
                try { _connection?.Close(); } catch { }
                _channel = null;
                _connection = null;

                if (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(5000, stoppingToken);
                }
            }
        }

        _logger.LogInformation("PaymentNotificationConsumer encerrado");
    }

    private void InitializeRabbitMq()
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.Host,
            Port = _rabbitMqSettings.Port,
            UserName = _rabbitMqSettings.Username,
            Password = _rabbitMqSettings.Password,
            DispatchConsumersAsync = true,
            VirtualHost = _rabbitMqSettings.Username,
            Ssl = new SslOption
            {
                Enabled = true,
                ServerName = _rabbitMqSettings.Host,
                CertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            }
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: _rabbitMqSettings.ExchangeName,
            type: ExchangeType.Direct,
            durable: true);

        _channel.QueueDeclare(
            queue: _rabbitMqSettings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _channel.QueueBind(
            queue: _rabbitMqSettings.QueueName,
            exchange: _rabbitMqSettings.ExchangeName,
            routingKey: _rabbitMqSettings.RoutingKey);

        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        _logger.LogInformation("RabbitMQ initialized. Exchange: {Exchange}, Queue: {Queue}, RoutingKey: {RoutingKey}",
            _rabbitMqSettings.ExchangeName, _rabbitMqSettings.QueueName, _rabbitMqSettings.RoutingKey);
    }

    private async Task ProcessMessageAsync(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        try
        {
            _logger.LogInformation("Received payment notification message: {Message}", message);

            var request = JsonSerializer.Deserialize<OrderNotificationRequest>(message, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (request == null)
            {
                _logger.LogError("Failed to deserialize payment notification message");
                _channel!.BasicNack(ea.DeliveryTag, false, false);
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var paymentNotificationService = scope.ServiceProvider.GetRequiredService<IPaymentNotificationService>();

            await paymentNotificationService.ProcessNotificationAsync(request);

            _channel!.BasicAck(ea.DeliveryTag, false);
            _logger.LogInformation("Payment notification processed successfully. OrderId: {OrderId}, PaymentId: {PaymentId}",
                request.OrderId, request.PaymentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment notification message: {Message}", message);

            _channel!.BasicNack(ea.DeliveryTag, false, true);
        }
    }

    public override void Dispose()
    {
        try
        {
            _channel?.Close();
            _connection?.Close();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao fechar conexão RabbitMQ durante dispose.");
        }
        base.Dispose();
    }
}