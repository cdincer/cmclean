﻿namespace cmclean.MinimalApi.Abstractions
{
    public interface IModule
    {
        WebApplicationBuilder RegisterModule(WebApplicationBuilder builder);
        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }
}
