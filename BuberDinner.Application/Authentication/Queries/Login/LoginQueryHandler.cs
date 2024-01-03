using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Register;
using BuberDinner.Application.common.interfaces.Authentication;
using BuberDinner.Application.common.interfaces.Persistance;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            //. Validate the user exists
            if (_userRepository.GetUserByEmail(query.Email) is not User user)
            {
                // throw new Exception("User with given email does not exist");
                return Domain.Common.Errors.Errors.Authentication.InvalidCredentials;

            }

            // 2.Validate the password is correct
            if (user.Password != query.Password)
            {
                // throw new Exception("Invalid password");
                return Domain.Common.Errors.Errors.Authentication.InvalidCredentials;
            }

            // 3. Create the token
            var token = _jwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult(
                user,
                token
            );
        }
    }
}
