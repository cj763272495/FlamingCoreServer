using Grpc.Core;

public class Service:FCService.FCServiceBase {
    public override Task<LoginResponse> Login(LoginRequest request,ServerCallContext context) {
        Console.WriteLine("Get PlayerData ");

        var response = new LoginResponse {
            Token = new TokenGenerator().GenerateJwtToken(request.Name),
            PlayerData = new PlayerData {
                Player = request.Name,
                Coin = 0,
                Skin = { 0 },
                Trail = { 0 },
                Energy = 99,
                MaxUnLockWave = 1,
                CurSkin = 0,
                CurTrail = 0
            }
        };

        return Task.FromResult(response);
    }
}