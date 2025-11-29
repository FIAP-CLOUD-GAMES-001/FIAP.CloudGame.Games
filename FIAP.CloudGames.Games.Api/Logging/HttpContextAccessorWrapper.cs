namespace FIAP.CloudGames.Usuarios.Api.Logging;

public static class HttpContextAccessorWrapper
{
    public static IHttpContextAccessor Accessor { get; set; } = default!;
    public static HttpContext? HttpContext => Accessor.HttpContext;
}


