using System.Diagnostics;
using FluentValidation;
using MediatR;
using Ordering.Application.Commands;

namespace Ordering.Application.Behaviour;

//This will collect all fluent validators and run before handler
/// <summary>
/// Todo: 6.18.1 Add validator for validate automatically with MediatR Pipeline
/// IPipelineBehavior là một tính năng của MediatR, giúp bạn thực hiện logic trước và sau khi Command hoặc Query Handler của bạn được thực thi1.
/// Điều này giúp cho Handler của bạn chỉ phải xử lý các yêu cầu hợp lệ trong triển khai CQRS của bạn, và bạn không phải làm rối rắm các phương thức Handler với logic kiểm tra hoặc ghi log lặp đi lặp lại
///
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehaviour<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    //IValidator, will return all the classes which implement this under _validators
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Todo: 6.18.5 Explain _validators
    /// _validators là danh sách các Validator đã khai báo theo TRequest
    /// FluentValidation là một thư viện phổ biến để thực hiện việc kiểm tra dữ liệu đầu vào trong .NET.
    /// Khi bạn đăng ký các Validator của mình với DI Container, thì FluentValidation sẽ tự động tạo ra một IEnumerable<IValidator<TRequest>> cho mỗi loại yêu cầu mà bạn có Validator
    /// </summary>
    /// <param name="validators"></param>
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            //This runs all the validation rules one by one returns the validation result
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            //Now, need to check for any failures
            var failures = validationResults.SelectMany(e => e.Errors).Where(f => f != null).ToList();
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

        }
        //On success, continue the mediator pipeline for the next step
        return await next();
    }
    
    /// <summary>
    /// Todo: 6.18.2 Order of processing before or after Handler
    /// Thứ tự xử lý trước hoặc sau
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle (TRequest request, RequestHandlerDelegate<TResponse> next)  
    {  
        // Logic trước khi handler được thực thi
        Trace.WriteLine ("Before");  

        // Gọi handler
        var response = await next ();  

        // Logic sau khi handler được thực thi
        Trace.WriteLine ("After");   

        return response;  
    }

    /// <summary>
    /// Todo: 6.18.3 Multi Pipeline Behaviour in Handler and order
    /// Có, một handler có thể có nhiều IPipelineBehavior.
    /// Khi một yêu cầu được gửi, MediatR sẽ tự động tìm và sử dụng tất cả các phiên bản của IPipelineBehavior đã được đăng ký.
    ///
    /// Các IPipelineBehavior sẽ được thực thi theo thứ tự mà chúng được đăng ký trong DI container.
    /// Điều này cho phép bạn kiểm soát thứ tự mà các hành động của bạn được thực hiện trong quá trình xử lý yêu cầu.
    /// </summary>
    public void MultiPipelineBehaviour()
    {
        //First
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FirstPipelineBehavior<,>));
        //
        // //Second
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(SecondPipelineBehavior<,>));
    }

    /// <summary>
    /// Todo: 6.18.4 Multi Pipeline Behaviour in Handler
    /// Chỉ định rõ xem hành vi nào mới được phép chạy qua Pipeline này
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task<TResponse> SpecifyBehaviorHandle (TRequest request, RequestHandlerDelegate<TResponse> next)  
    {  
        if (request is CheckoutOrderCommand)
        {
            // Logic của LoggingBehavior
        }

        return await next();  
    } 
}

