using DTOs;
using ChatBot.Methods;
using ChatBot.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Endpoints
{
    public class UserEndpoints : InterfaceEndpoints
    {
        public void MapEndpoints(WebApplication app)
        {
            app.MapPost("/users/create_account", async ([FromBody] CreateAccountRequest request, UserServices user) =>
            {
                CreateAccountResult result = await user.CreateAccount(request);

                return result switch
                {
                    CreateAccountResult.AcountAlreadyExists => Results.BadRequest("Account Already exists"),
                    CreateAccountResult.AccountCreatedSuccessfully => Results.Created(),
                    _ => Results.BadRequest("Unknown error")
                };
            });

            app.MapPost("/user/account_login", async ([FromBody] LoginAccountRequest request, UserServices user, AuthService authService) =>
            {
                var login = await user.LoginAccount(request);

                switch (login.Item1)
                {
                    case LoginResult.AccountNotExists:
                        return Results.NotFound("Account not exists");

                    case LoginResult.WrongData:
                        return Results.Unauthorized();

                    case LoginResult.LoginAccountSuccessfully:
                        string token = authService.GenerateToken(login.User!);
                        return Results.Ok(token);

                    default:
                        return Results.BadRequest("Unknown error");
                }
            });

            app.MapPatch("/user/change_password", async ([FromBody] ChangePasswordRequest request, UserServices user) =>
            {
                ChangePasswordResult result = await user.ChangePassword(request);

                return result switch
                {
                    ChangePasswordResult.AccountNotFound => Results.NotFound("Account not exists"),

                    ChangePasswordResult.WrongData => Results.Unauthorized(),

                    ChangePasswordResult.PasswordChagedSuccessfuly => Results.Accepted("Password changed successfully"),

                    _ => Results.BadRequest("Unknown error")
                };
            });

            app.MapDelete("/user/change_password", async ([FromBody] DeleteAccountRequest request, UserServices user) =>
            {
                DeleteAccountResult result = await user.DeleteAccount(request);

                return result switch
                {
                    DeleteAccountResult.AccountNotFound => Results.NotFound("AccountNotFound"),

                    DeleteAccountResult.WrongData => Results.Unauthorized(),

                    DeleteAccountResult.DeleteAccountSuccessfuly => Results.Accepted("Your account has deleted successfully"),

                    _ => Results.BadRequest("Unknown error")
                };
            });
        }
    }
}
