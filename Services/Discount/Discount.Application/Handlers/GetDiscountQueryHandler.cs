using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Application.Handlers;

public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
{
    private readonly IDiscountRepository _discountRepository;

    public GetDiscountQueryHandler(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }
    
    /// <summary>
    /// Todo: 4.8.2 Create GRPC Model (Coupon Model) to return for endpoint
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="RpcException"></exception>
    public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _discountRepository.GetDiscount(request.ProductName);
        if (coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound,
                $"Discount with the product name = {request.ProductName} not found"));
        }
        
        var couponModel = new CouponModel
        {
            Id = coupon.Id,
            Amount = coupon.Amount,
            Description = coupon.Description,
            ProductName = coupon.ProductName
        };
        return couponModel;
    }
}