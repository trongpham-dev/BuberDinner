using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Register;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    [Route("auth")]
    public class AuthenticationController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        public AuthenticationController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            //var command = new RegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);
            var command = _mapper.Map<RegisterCommand>(request);
            ErrorOr<AuthenticationResult> authResult =  await _mediator.Send(command);

            //return authResult.Match(authResult => Ok(MapResponse(authResult)), errors => Problem(errors));
            return authResult.Match(authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)), errors => Problem(errors));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request){
            //var query = new LoginQuery(request.Email, request.Password);
            var query = _mapper.Map<LoginQuery>(request);
            var authResult =  await _mediator.Send(query);
            //return authResult.Match(authResult => Ok(MapResponse(authResult)), errors => Problem(errors));
            return authResult.Match(authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)), errors => Problem(errors));
        }

        //private static AuthenticationResponse MapResponse(AuthenticationResult authResult)
        //{
        //    return new AuthenticationResponse(authResult.user.Id, authResult.user.FirstName, authResult.user.LastName, authResult.user.Email, authResult.Token);
        //}
    }
}