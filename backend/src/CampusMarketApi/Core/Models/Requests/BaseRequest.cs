namespace CampusMarketApi.Core.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

public abstract class BaseRequest { 
}

public abstract class RequestExample<T> : IExamplesProvider<T> where T : BaseRequest, new()
{

    T IExamplesProvider<T>.GetExamples() => GetExamples();

    public abstract T GetExamples();
}