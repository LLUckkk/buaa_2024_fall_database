using Swashbuckle.AspNetCore.Filters;

namespace CampusMarketApi.Core.Models.Responses;

public abstract class BaseResponse { }

public abstract class ResponseExample<T> : IExamplesProvider<T> where T : BaseResponse, new()
{
    T IExamplesProvider<T>.GetExamples() => GetExamples();

    public abstract T GetExamples();
}