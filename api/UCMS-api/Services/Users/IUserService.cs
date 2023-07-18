﻿using User_Contact_Management_System.Dtos.Users;
using User_Contact_Management_System.Models;

namespace User_Contact_Management_System.Services.Users
{
    public interface IUserService
    {
        Task<AuthResult?> Register(UserCreateDto user);
        Task<AuthResult?> Login(UserLoginDto user);
        Task<bool> UpdateUserDetails(string id, UserUpdateDetailsDto user);
        Task<bool> UpdateUserPassword(string id, UserUpdatePasswordDto user);
        Task<bool> DeleteUser(string username);
    }
}