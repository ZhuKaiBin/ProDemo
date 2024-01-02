using EndpointsFastWebApi.Models.RequestModel;
using FastEndpoints;
using FastEndpoints.Security;

namespace EndpointsFastWebApi.Endpoints.Authorize
{
    public class UserLoginEndpoint : Endpoint<LoginRequest>
    {
        public override void Configure()
        {
            Post("/api/login");
            AllowAnonymous();
        }


        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {

            //if (await authService.CredentialsAreValid(req.UserName, req.Password, ct))
            //{
            //    var jwtToken = JWTBearer.CreateToken
            //        (
            //            signingKey: "TokenSigningKey",
            //            expireAt: DateTime.UtcNow.AddDays(1),
            //            priviledges: u =>
            //            {
            //                u.Roles.Add("Manager");
            //                u.Permissions.AddRange(new[] { "ManageUsers", "ManageInventory" });
            //                u.Claims.Add(new("UserName", req.Username));
            //                u["UserID"] = "001"; //indexer based claim setting
            //            }
            //        );

            //    await SendAsync(new
            //    {
            //        Username = req.UserName,
            //        Token = jwtToken
            //    });

            //}
        }

    }
}
