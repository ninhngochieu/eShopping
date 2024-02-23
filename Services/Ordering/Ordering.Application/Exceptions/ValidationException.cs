using FluentValidation.Results;

namespace Ordering.Application.Exceptions;

/// <summary>
/// Todo: 6.16.1 Add Validation Exception
/// </summary>
public class ValidationException :  ApplicationException
{
    public IDictionary<string, string[]> Errors { get; }
    public ValidationException(): base("One or more validation error(s) occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Todo: 6.16.2 Group field with messages
    /// 1 field sẽ có nhiều loại lỗi khác nhau 
    /// </summary>
    /// <param name="failures"></param>
    public ValidationException(IEnumerable<ValidationFailure> failures): this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failure => failure.Key, failure => failure.ToArray());
    }
}