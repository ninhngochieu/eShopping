using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    /// <summary>
    /// Todo: 6.12.1 Setup logger
    /// </summary>
    /// <param name="orderRepository"></param>
    /// <param name="mapper"></param>
    /// <param name="logger"></param>
    public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }
    
    /// <summary>
    /// Todo: 6.12.2 Log Information
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(request);
        var generatedOrder = await _orderRepository.AddAsync(orderEntity);
        _logger.LogInformation(($"Order {generatedOrder} successfully created."));
        return generatedOrder.Id;
    }
}