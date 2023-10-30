namespace cmclean.MinimalApi.Abstractions
{
    public interface IEndpoint
    {
        IEndpointRouteBuilder RegisterRoute(IEndpointRouteBuilder endpoints);
    }
}
