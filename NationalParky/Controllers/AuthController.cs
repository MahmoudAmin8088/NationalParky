using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalParky.Models;
using NationalParky.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NationalParky.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }


        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authRepo.RegisterAsync(model);

            if (!result.IsAuthentication)
            {
                return BadRequest(result.Message);
            }
            //return Ok(result);
            return Ok(new { UserName = result.UserName, Email = result.Email, Role = result.Roles, ExpireOn = result.ExpiresOn, Token = result.Token });
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepo.LoginAsync(model);

            if (!result.IsAuthentication)
                return BadRequest(result.Message);

            //return Ok(result);
            return Ok(new {UserName =result.UserName,Email =result.Email ,Role =result.Roles ,ExpireOn = result.ExpiresOn , Token =result.Token });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("addrole")]
        
        public async Task<IActionResult> AddRole ([FromBody] AddRoleModel model)
        {
            


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authRepo.AddRolesAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);



        }
    }
}
