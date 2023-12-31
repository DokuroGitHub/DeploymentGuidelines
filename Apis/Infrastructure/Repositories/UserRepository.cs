﻿using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext,
        IMapper mapper)
        : base(dbContext,
              mapper)
    {
        _dbContext = dbContext;
    }

    public Task<bool> CheckUserNameExited(string username) => _dbContext.Users.AnyAsync(u => u.Username == username);

    public async Task<User> GetUserByUserNameAndPasswordHash(string username, string passwordHash)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(record => record.Username == username
                                    && record.PasswordHash == passwordHash);
        if (user is null)
        {
            throw new Exception("UserName & password is not correct");
        }


        return user;
    }
}
