using Azure.Core;
using FIAP.CloudGames.Games.Domain.Entities;
using FIAP.CloudGames.Games.Domain.Enums;
using FIAP.CloudGames.Games.Domain.Exceptions;
using FIAP.CloudGames.Games.Domain.Interfaces.Repositories;
using FIAP.CloudGames.Games.Domain.Interfaces.Services;
using FIAP.CloudGames.Games.Domain.Requests.Order;
using FIAP.CloudGames.Games.Domain.Requests.Payment;
using FIAP.CloudGames.Games.Domain.Responses.Order;

namespace FIAP.CloudGames.Games.Service.Order;
public class OrderService(
    IOrderRepository orderRepository,
    IGameRepository gameRepository,
    IPaymentService paymentService,
    IPaymentRepository paymentRepository) : IOrderService
{
    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
    {
        var games = new List<GameEntity>();
        decimal totalAmount = 0;
        if (request.Games == null || request.Games.Length == 0)
            throw new DomainException("At least one game must be selected.");

       
       
        foreach (var gameId in request.Games)
        {
            var game = await gameRepository.GetByIdAsync(gameId);
            if (game == null)
                throw new NotFoundException($"Game with ID {gameId} not found.");

            games.Add(game);
            totalAmount += game.Price;
        }

        var order = new OrderEntity(request.UserId, request.Games);
        await orderRepository.AddAsync(order);

        // Atualizar status do pedido para Progress (em processamento) após criar o pedido
        order.UpdateStatus(EOrderStatus.Progress);
        await orderRepository.UpdateAsync(order);

        var paymentRequest = new PaymentRequest
        {
            OrderId = order.Id.ToString(),
            OrderAmount = totalAmount,
            PaymentMethod = ((int)request.PaymentMethod).ToString(),
            OrderDate = order.CreatedAt
        };

        await paymentService.SendPaymentRequestAsync(paymentRequest);

      
        var paymentEntity = new PaymentEntity(
            paymentRequest.OrderId,
            paymentRequest.OrderAmount,
            paymentRequest.PaymentMethod.ToString(),
            paymentRequest.OrderDate);
        
        await paymentRepository.AddAsync(paymentEntity);
        
       
        var gamesResponse = games.Select(g => new GameOrderResponse(
            g.Id,
            g.Title,
            g.Description,
            g.Price,
            g.Genre,
            g.ReleaseDate
        )).ToList();

        return new OrderResponse(
            order.Id, 
            gamesResponse,
            totalAmount,
            order.Status, 
            order.UserId, 
            order.CreatedAt);
    }

  

    public async Task<IEnumerable<OrderResponse>> ListAsync()
    {
        var orders = await orderRepository.ListAllAsync();
        return orders.Select(o => MapToOrderResponse(o));
    }

    public async Task<OrderResponse?> GetByIdAsync(int id)
    {
        var order = await orderRepository.GetByIdAsync(id);
        if (order == null) return null;
        
        return MapToOrderResponse(order);
    }

    public async Task<IEnumerable<OrderResponse>> GetByUserIdAsync(int userId)
    {
        var orders = await orderRepository.GetByUserIdAsync(userId);
        return orders.Select(o => MapToOrderResponse(o));
    }

    private OrderResponse MapToOrderResponse(OrderEntity order)
    {
        var totalAmount = order.OrderGames?.Sum(og => og.Game?.Price ?? 0) ?? 0;
        
        var gamesResponse = order.OrderGames?
            .Where(og => og.Game != null)
            .Select(og => new GameOrderResponse(
                og.Game!.Id,
                og.Game.Title,
                og.Game.Description,
                og.Game.Price,
                og.Game.Genre,
                og.Game.ReleaseDate
            ))
            .ToList() ?? new List<GameOrderResponse>();

        return new OrderResponse(
            order.Id, 
            gamesResponse,
            totalAmount,
            order.Status, 
            order.UserId, 
            order.CreatedAt);
    }

    public async Task UpdateAsync(UpdateOrderRequest request)
    {
        var order = await orderRepository.GetByIdAsync(request.Id);
        if (order == null)
            throw new NotFoundException($"Order with ID {request.Id} not found.");

        if (request.Status.HasValue)
            order.UpdateStatus(request.Status.Value);

        await orderRepository.UpdateAsync(order);
    }
}

