using CicekSepetiTask.Dtos;
using CicekSepetiTask.Entities;
using CicekSepetiTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Used to authenticate the user.
        /// Get one of the UserName and Password from the already hardcoded users in the UserService 
        /// usinj JWT Authentication to login and get a JWT token back.
        /// </summary>
        /// <param name="authModel"></param>
        /// <returns> Returns the logged in user with its JWT token </returns>
        [AllowAnonymous]
        [HttpPost("auth")]
        public ActionResult Authenticate([FromBody] AuthDto authModel)
        {
            var user = _userService.Authenticate(authModel.UserName, authModel.Password);
            if (user == null) return BadRequest(new { message = "Wrong Username or Password!" });
            return Ok(user);
        }

        /// <summary>
        /// This method is written to check if you are logged in or not.
        /// </summary>
        /// <returns> Returns all users </returns>
        [HttpGet]
        public ActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
