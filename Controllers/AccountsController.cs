using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SMTS.DTOs;
using SMTS.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
 
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
 
using System;
using System.Collections.Generic;
 using System.Linq;
 using System.Threading.Tasks;
using SMTS.Helpers;

namespace SMTS.Controllers
{
    [ApiController]
    [Route("api/accounts")]

    public class AccountsController : ControllerBase
    {
        private readonly UserManager<MyUser> userManager;
        private readonly SignInManager<MyUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly MESDbContext context;
        private readonly IMapper mapper;
         public AccountsController(UserManager<MyUser> userManager,
            SignInManager<MyUser> signInManager,
            IConfiguration configuration,
            MESDbContext context,
            IMapper mapper) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet("checkExistingEmployeeId")]
        public async Task<ActionResult<bool>> CheckExistingEmployeeId(string employeeId)
        {
            var user = await userManager.FindByNameAsync(employeeId);
            return user != null;
        }
        [HttpPost("create")]
        public async Task<ActionResult<AuthenticationResponse>> Create(
            [FromBody] UserLoginCredentials userLoginCredentials)
        {
            var user = new MyUser { UserName = userLoginCredentials.EmployeeID, Name = userLoginCredentials.Name };
            var result = await userManager.CreateAsync(user, userLoginCredentials.Password);
             if (result.Succeeded)
            {
                return await BuildTokenForRegister(userLoginCredentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
        {

            var result = await signInManager.PasswordSignInAsync(userCredentials.EmployeeID,
                        userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await BuildToken(userCredentials);
            }
            else
            {
                return BadRequest("Incorrect Login");
            }
        }

        [HttpGet("listUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult<List<UserDTO>>> GetListUsers([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Users.AsQueryable();
            await HttpContext.InsertParametersPaginationInHeader(queryable);
            var users = await queryable.OrderBy(x => x.Email).Paginate(paginationDTO).ToListAsync();
            return mapper.Map<List<UserDTO>>(users);
        }

        [HttpPost("makeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult> MakeAdmin([FromBody] string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddClaimAsync(user, new Claim("role", "admin"));
            return NoContent();
        }

        [HttpPost("removeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult> RemoveAdmin([FromBody] string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.RemoveClaimAsync(user, new Claim("role", "admin"));
            return NoContent();
        }



        private async Task<AuthenticationResponse> BuildTokenForRegister(UserLoginCredentials userLoginCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("EmployeeID", userLoginCredentials.EmployeeID)
            };

            var user = await userManager.FindByNameAsync(userLoginCredentials.EmployeeID);
            var claimsDB = await userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: creds);

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        private async Task<AuthenticationResponse> BuildToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("EmployeeID", userCredentials.EmployeeID)
            };

            var user = await userManager.FindByNameAsync(userCredentials.EmployeeID);
            var claimsDB = await userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: creds);

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }


    }

}
