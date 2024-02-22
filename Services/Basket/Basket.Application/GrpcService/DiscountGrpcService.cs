using Discount.Grpc.Protos;

namespace Basket.Application.GrpcService;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    /// <summary>
    /// Todo: 5.2.2 Setup Grpc Client Service
    /// </summary>
    /// <param name="discountProtoServiceClient"></param>
    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    /// <summary>
    /// Todo: 5.2.3 Call Grpc Server and processing logic
    /// </summary>
    /// <param name="productName"></param>
    /// <returns></returns>
    public async Task<CouponModel> GetDiscount(string productName)
    {
        var discountRequest = new GetDiscountRequest {ProductName = productName};
        return await _discountProtoServiceClient.GetDiscountAsync(discountRequest);
    }
}