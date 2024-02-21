using Basket.Application.Queries;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class DeleteBasketByUserNameHandler: IRequestHandler<DeleteBasketByUserNameQuery>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketByUserNameHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }
    /// <summary>
    /// Todo: 3.8.1 Delete action handler
    /// Return Unit.Value nếu không trả về dữ liệu gì
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Unit> Handle(DeleteBasketByUserNameQuery request, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasket(request.UserName);
        return Unit.Value;
    }
}