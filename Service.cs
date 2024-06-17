using Grpc.Core;
using Grpc.Core.Interceptors; 

class Program{
    const int Port = 50051;
    const string ServerAddress = "localhost";

    public static void Main(string[] args){ 
        const string Audience = "Your Audience";
        const string SecretKey = "Your JWT Secret Key";
        const string Issuer = "Your Issuer";

        var serviceBinder = FCService.BindService(new Service());
        var interceptor = new JwtTokenInterceptor(Issuer,Audience,SecretKey);
        serviceBinder.Intercept(interceptor);

        Server server = new Server{
            Services = { serviceBinder }, 
            Ports = { new ServerPort(ServerAddress, Port, ServerCredentials.Insecure) }
        };
        
        server.Start();

        Console.WriteLine("PlayerData server listening on port " + Port);
        Console.WriteLine("Press any key to stop the server...");
        Console.ReadKey();
        server.ShutdownAsync().Wait();
    }
}
