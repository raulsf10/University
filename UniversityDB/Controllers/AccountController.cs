using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDB.DataAccess;
using UniversityDB.Helpers;
using UniversityDB.Models;
using UniversityDB.Models.DataModels;

namespace UniversityDB.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTSettings _jwtSettings;
        private readonly UniversityDBContext _context;

        // Example Users
        // TODO: Change by real Users in DB
        public AccountController(UniversityDBContext context, JWTSettings jwtSettings)
        {

            _context = context;
            _jwtSettings = jwtSettings;
        }

        [HttpPost]
        public IActionResult GetToken(UserLogins userLogin)
        {
            try
            {
                var token = new UserTokens();
                var valid = _context.Users.Any(user => user.Name.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase) && user.Password.Equals(userLogin.Password, StringComparison.OrdinalIgnoreCase));

                if (!valid)
                    return BadRequest("Wrong credentials");

                var user = _context.Users.FirstOrDefault(user => user.Name.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase) && user.Password.Equals(userLogin.Password, StringComparison.OrdinalIgnoreCase));

                token = JWTHelpers.GenTokenKey(new UserTokens()
                {
                    UserName = user.Name,
                    EmailId = user.Email,
                    Id = user.Id,
                    GuidId = Guid.NewGuid()
                }, _jwtSettings);

                return Ok(token);

            }
            catch(Exception ex)
            {
                throw new Exception("Get Token Error", ex);
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        public IActionResult GetUsersLists()
        {
            return Ok(_context.Users.ToListAsync());
        }


    }
}
