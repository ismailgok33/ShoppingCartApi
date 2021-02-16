using CicekSepetiTask.Entities;
using CicekSepetiTask.Utility;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CicekSepetiTask.Services
{
    public class UserService : IUserService
    {
        readonly AppSettings _appSettings;

        /// <summary>
        /// Hard-coded user list
        /// </summary>
        private List<User> _users = new List<User>
        {
            new User {Id = 1, FirstName = "test1", LastName = "test1", UserName = "test1", Password = "test123"},
            new User {Id = 2, FirstName = "test2", LastName = "test2", UserName = "test2", Password = "test123"}
        };

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Authentication method
        /// </summary>
        /// <param name="userName"> UserName </param>
        /// <param name="password"> Password </param>
        /// <returns> return the Authenticated User and its token to be passed as Bearer Token for authorization </returns>
        public User Authenticate(string userName, string password)
        {
            var user = _users.FirstOrDefault(x => x.UserName == userName && x.Password == password);
            if (user == null) return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;
            return user;
        }

        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns> Returns all users </returns>
        public List<User> GetAll()
        {
            return _users;
        }
    }
}
