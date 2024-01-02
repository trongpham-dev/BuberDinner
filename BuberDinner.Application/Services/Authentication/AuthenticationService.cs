using BuberDinner.Application.common.Errors;
using BuberDinner.Application.common.interfaces.Authentication;
using BuberDinner.Application.common.interfaces.Persistance;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuberDinner.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public ErrorOr<AuthenticationResult> Login(string email, string password)
        {
            //. Validate the user exists
            if (_userRepository.GetUserByEmail(email) is not User user)
            {
                // throw new Exception("User with given email does not exist");
                return Errors.Authentication.InvalidCredentials;

            }

            // 2.Validate the password is correct
            if(user.Password != password)
            {
                // throw new Exception("Invalid password");
                return Errors.Authentication.InvalidCredentials;
            }

            // 3. Create the token
            var token = _jwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult(
                user,
                token
            );
        }

        public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
        {
            // 1.Validate the user doesn't exiust
            if (_userRepository.GetUserByEmail(email) is not null)
            {
                // throw new DuplicateEmailException();
                return Errors.User.DulicateEmail;
            }

            // 2. create user(generate unique id) $ Persist to DB
            var user = new User { FirstName = firstName, LastName = lastName, Email = email, Password = password };
            _userRepository.Add(user);

            // create jwt token
            Guid userId = Guid.NewGuid();
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
               user, 
                token
            );
        }
    }
}