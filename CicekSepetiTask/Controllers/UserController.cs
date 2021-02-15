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

        [AllowAnonymous]
        [HttpPost("auth")]
        public ActionResult Authenticate([FromBody] AuthDto authModel)
        {
            var user = _userService.Authenticate(authModel.UserName, authModel.Password);
            if (user == null) return BadRequest(new { message = "Kullanıcı adı yada şifre yanlış" });
            return Ok(user);
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
