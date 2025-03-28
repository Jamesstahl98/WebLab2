﻿using Microsoft.AspNetCore.Identity;
using WebLab2.Entities;
using WebLab2.Models;

namespace WebLab2.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return users.Select(u => new UserDto
        {
            Username = u.Username,
            Role = u.Role,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Country = u.Country,
            City = u.City,
            Address = u.Address
        });
    }

    public async Task<UserDto> GetUserProfileAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Country = user.Country,
            City = user.City,
            Address = user.Address
        };
    }

    public async Task<UserDto> UpdateUserProfileAsync(Guid userId, UpdateUserDto updatedUser)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return null;

        var passwordHasher = new PasswordHasher<User>();
        var passwordVerification = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, updatedUser.CurrentPassword);

        if (passwordVerification != PasswordVerificationResult.Success)
        {
            throw new ArgumentException("Incorrect password.");
        }

        if (!string.Equals(user.Username, updatedUser.Username, StringComparison.OrdinalIgnoreCase))
        {
            var usernameExists = await _unitOfWork.Users.UsernameExistsAsync(updatedUser.Username);
            if (usernameExists)
            {
                throw new ArgumentException("Username already exists.");
            }
        }

        if (!string.Equals(user.Email, updatedUser.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailExists = await _unitOfWork.Users.EmailExistsAsync(updatedUser.Email);
            if (emailExists)
            {
                throw new ArgumentException("Email already exists.");
            }
        }

        user.Username = updatedUser.Username;
        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.Email = updatedUser.Email;
        user.PhoneNumber = updatedUser.PhoneNumber;
        user.Country = updatedUser.Country;
        user.City = updatedUser.City;
        user.Address = updatedUser.Address;

        if (!string.IsNullOrEmpty(updatedUser.NewPassword))
        {
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, updatedUser.NewPassword);
        }

        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Country = user.Country,
            City = user.City,
            Address = user.Address
        };
    }
}