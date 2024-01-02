using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuberDinner.Api.Filters;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    [Route("auth")]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService){
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            ErrorOr<AuthenticationResult> authResult = _authenticationService.Register(request.FirstName, request.LastName, request.Email, request.Password);

            return authResult.Match(authResult => Ok(MapResponse(authResult)), errors => Problem(errors));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request){
             var authResult = _authenticationService.Login(request.Email, request.Password);
             return authResult.Match(authResult => Ok(MapResponse(authResult)), errors => Problem(errors));
        }

        private static AuthenticationResponse MapResponse(AuthenticationResult authResult)
        {
            return new AuthenticationResponse(authResult.user.Id, authResult.user.FirstName, authResult.user.LastName, authResult.user.Email, authResult.Token);
        }
    }
}