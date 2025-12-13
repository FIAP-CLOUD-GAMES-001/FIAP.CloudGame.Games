using FIAP.CloudGames.Games.Domain.Enums;
using FIAP.CloudGames.Games.Domain.Exceptions;
using FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Games.Domain.Interfaces.Services;
using FIAP.CloudGames.Games.Domain.Requests.Payment;
using Microsoft.Extensions.Logging;

namespace FIAP.CloudGames.Games.Service.Payment;

public class PaymentNotificationService : IPaymentNotificationService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<PaymentNotificationService> _logger;

    public PaymentNotificationService(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        ILogger<PaymentNotificationService> logger)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task ProcessNotificationAsync(OrderNotificationRequest request)
    {
        try
        {
            
            var payment = await _paymentRepository.GetByPaymentIdAsync(request.PaymentId)
                ?? await _paymentRepository.GetByOrderIdForUpdateAsync(request.OrderId);

            if (payment == null)
            {
                _logger.LogWarning("Payment not found for PaymentId: {PaymentId}, OrderId: {OrderId}", 
                    request.PaymentId, request.OrderId);
                throw new NotFoundException($"Payment not found for PaymentId: {request.PaymentId} or OrderId: {request.OrderId}");
            }

          
            if (string.IsNullOrEmpty(payment.PaymentId))
            {
                payment.SetPaymentId(request.PaymentId);
            }

            // O status já vem como enum no request
            payment.UpdateStatus(request.Status);

            await _paymentRepository.UpdateAsync(payment);

            // Buscar o pedido pelo OrderId
            if (!int.TryParse(request.OrderId, out var orderId))
            {
                _logger.LogError("Invalid OrderId format: {OrderId}", request.OrderId);
                throw new DomainException($"Invalid OrderId format: {request.OrderId}");
            }

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning("Order not found for OrderId: {OrderId}", request.OrderId);
                throw new NotFoundException($"Order with ID {orderId} not found.");
            }

            // Atualizar o status do pedido baseado no status do pagamento
            var orderStatus = MapPaymentStatusToOrderStatus(request.Status);
            order.UpdateStatus(orderStatus);

            await _orderRepository.UpdateAsync(order);

            _logger.LogInformation(
                "Payment notification processed successfully. PaymentId: {PaymentId}, OrderId: {OrderId}, Status: {Status}",
                request.PaymentId, request.OrderId, request.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment notification. PaymentId: {PaymentId}, OrderId: {OrderId}",
                request.PaymentId, request.OrderId);
            throw;
        }
    }

    private static EOrderStatus MapPaymentStatusToOrderStatus(EPaymentStatus paymentStatus)
    {
        return paymentStatus switch
        {
            EPaymentStatus.Approved => EOrderStatus.Authored,
            EPaymentStatus.Rejected => EOrderStatus.Unauthorized,
            EPaymentStatus.Processing => EOrderStatus.Progress,
            _ => EOrderStatus.Progress
        };
    }
}

