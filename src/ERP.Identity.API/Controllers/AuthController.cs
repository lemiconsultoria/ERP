using ERP.Core.Controllers;
using ERP.Identity.Domain.DTOs;
using ERP.Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ERP.Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]    
    public class AuthController : MasterController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Autentica o usuário com base no email e na senha
        /// ADMIN -> admin@erp.com.br:admin
        /// REPORT -> report@erp.com.br:report
        /// </summary>
        /// <returns>Usuário autenticado</returns>
        /// <response code="200">Usuário autenticado</response>
        /// <response code="400">Autenticação falhou</response>
        /// <response code="500">Exceção</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserAuthenticatedDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ReturnExceptionModel), (int)HttpStatusCode.BadRequest)]        
        public IActionResult Auth([FromBody] UserToAuthenticateDTO user)
        {
            try
            {
                var userAuthenticated = _userService.Authenticate(user);

                if (userAuthenticated != null)
                    return Ok(userAuthenticated);

                return ParseBadRequest("Authentication Failed");
            }
            catch (Exception ex)
            {
                return ParseException(ex, "Authentication Failed");
            }
        }
    }
}