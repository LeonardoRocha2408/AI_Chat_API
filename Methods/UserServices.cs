using Microsoft.EntityFrameworkCore;
using ChatBot.Entities;
using ChatBot.Enums;
using DTOs;
using Microsoft.AspNetCore.Identity;

namespace ChatBot.Methods
{
    public class UserServices
    {
        private readonly AuthService _authService;
        private readonly DbEntity _context;
        public UserServices(DbEntity context, AuthService authService)
        {
            _authService = authService;
            _context = context;
        }

        public async Task<CreateAccountResult> CreateAccount(CreateAccountRequest dto)
        {
            UserEntity? User = await _context.Users.FirstOrDefaultAsync(user => user.UserName == dto.UserName);

            if (User != null)
            {
                return CreateAccountResult.AcountAlreadyExists;
            }

            User = new UserEntity()
            {
                UserName = dto.UserName
            };
            User.Password = _authService.HashPassword(User, dto.Password);
            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();
            return CreateAccountResult.AccountCreatedSuccessfully;
        }

        public async Task<(LoginResult, UserEntity? User)> LoginAccount(LoginAccountRequest dto)
        {
            UserEntity? User = await _context.Users.FirstOrDefaultAsync(u => 
            u.UserName == dto.UserName);

            if (User == null)
            {
                return (LoginResult.AccountNotExists, User);
            }
            var result = _authService.VerityPassword(User, User.Password, dto.Password);
            
            if (result == PasswordVerificationResult.Failed)
            {
                return (LoginResult.WrongData, User = null);
            }

            return (LoginResult.LoginAccountSuccessfully, User);
        }

        public async Task<ChangePasswordResult> ChangePassword(ChangePasswordRequest dto)
        {
            UserEntity? user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == dto.UserName);

            if (user == null)
            {
                return ChangePasswordResult.AccountNotFound;
            }

            PasswordVerificationResult result = _authService.VerityPassword(user, user.Password, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return ChangePasswordResult.WrongData;
            }
            else if (result == PasswordVerificationResult.Success)
            {
                user.Password = _authService.HashPassword(user, dto.NewPassword);
                await _context.SaveChangesAsync();
            }

            return ChangePasswordResult.PasswordChagedSuccessfuly;
        }

        public async Task<DeleteAccountResult> DeleteAccount(DeleteAccountRequest dto)
        {
            UserEntity? user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == dto.UserName);

            if (user == null)
            {
                return DeleteAccountResult.AccountNotFound;
            }

            PasswordVerificationResult result = _authService.VerityPassword(user, user.Password, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return DeleteAccountResult.WrongData;
            }
            else if (result == PasswordVerificationResult.Success)
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();
            }
            return DeleteAccountResult.DeleteAccountSuccessfuly;
        }
    }
}

