using System;
using System.Collections.Generic;
using UserService.Core.Interfaces;
using UserService.Core.Models;

namespace UserService.Core.Services
{
    public class UserDomainService
    {
        private readonly IUserRepository _userRepository;

        public UserDomainService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public AppUser CreateUser(string email, string name)
        {
            if (_userRepository.EmailExists(email))
                throw new InvalidOperationException("User with this email already exists");

            var user = new AppUser(email, name);

            _userRepository.Add(user);

            return user;
        }
    }
}
