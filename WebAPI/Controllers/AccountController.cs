using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<APIUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<APIUser> userManager, ILogger<AccountController> logger, IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserDTO registerDTO)
        {

            if (await UserNameExists(registerDTO.Username)) return BadRequest("Username is taken");

            if (await UserExists(registerDTO.Email)) return BadRequest("Email is taken");

            var user = _mapper.Map<APIUser>(registerDTO);

            user.UserName = registerDTO.Username;
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded) return BadRequest("Password not valid");

            var rolesResult = await _userManager.AddToRoleAsync(user, "Administrator");

            if (!rolesResult.Succeeded) return BadRequest(result.Errors);

            //return Ok(registerDTO);

            return new UserDTO
            {
                FirstName = user.FirstName,
                Email = user.Email,
                Username = user.UserName,
                Roles = await _userManager.GetRolesAsync(user),
                Token = await _authManager.CreateToken(registerDTO)
            };
        }


        /// <summary>
        /// user@example.com
        /// Password@123
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginUserDTO loginDTO)
        {
            var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.UserName == loginDTO.Username);

            if (user == null) return Unauthorized("Invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!result) return Unauthorized("Invalid password");


            return new UserDTO
            {
                FirstName = user.FirstName,
                Email = user.Email,
                Username = user.UserName,
                Roles = await _userManager.GetRolesAsync(user),
                Token = await _authManager.CreateToken(loginDTO)
            };
        }

        private async Task<bool> UserExists(string email)
        {
            return await _userManager.Users.AnyAsync(x=>x.Email == email);
        }

        private async Task<bool> UserNameExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username);
        }
    }
}
