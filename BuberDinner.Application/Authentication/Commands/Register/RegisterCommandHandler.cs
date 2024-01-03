using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.common.interfaces.Authentication;
using BuberDinner.Application.common.interfaces.Persistance;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            // 1.Validate the user doesn't exiust
            if (_userRepository.GetUserByEmail(command.Email) is not null)
            {
                // throw new DuplicateEmailException();
                return Domain.Common.Errors.Errors.User.DulicateEmail;
            }

            // 2. create user(generate unique id) $ Persist to DB
            var user = new User { FirstName = command.FirstName, LastName = command.LastName, Email = command.Email, Password = command.Passowrd };
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
