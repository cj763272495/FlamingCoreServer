using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims;
using System.Text; 
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.IdentityModel.Tokens;

public class JwtTokenInterceptor:Interceptor {
    private const string AuthorizationHeader = "Authorization";
    private const string BearerPrefix = "Bearer ";
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _secretKey;

    public JwtTokenInterceptor(string issuer,string audience,string secretKey) {
        _issuer = issuer;
        _audience = audience;
        _secretKey = secretKey;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,ServerCallContext context,UnaryServerMethod<TRequest,TResponse> continuation) {
        // 从请求头中提取令牌
        var authHeader = context.RequestHeaders.Get(AuthorizationHeader);
        if(string.IsNullOrEmpty(authHeader.Value.ToString()) || !authHeader.Value.ToString().StartsWith(BearerPrefix)) {
             
            throw new RpcException(new Status(StatusCode.Unauthenticated,"Missing or invalid token"));
        }
        var token = authHeader.Value.ToString().Substring(BearerPrefix.Length);
         
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey)),
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30) // Allow small clock variation
        };

        ClaimsPrincipal principal = null;
        SecurityToken validatedToken;
        try {
            principal = tokenHandler.ValidateToken(token,validationParameters,out validatedToken);
        } catch {
            throw new RpcException(new Status(StatusCode.Unauthenticated,"Token validation failed"));
        }

        if(!principal.Identity.IsAuthenticated) {
            throw new RpcException(new Status(StatusCode.Unauthenticated,"Token validation failed"));
        }

        // 从令牌中提取用户ID
        var jwtToken = (JwtSecurityToken)validatedToken;
        var userIdClaim = jwtToken.Claims.First(claim => claim.Type == "user_id");
        var userId = userIdClaim.Value;

        // 将用户ID添加到上下文中，以便在服务方法中使用
        context.UserState["UserId"] = userId;

        // 继续执行服务方法
        return await continuation(request,context);
    }
}