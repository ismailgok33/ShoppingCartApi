using CicekSepetiTask.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CicekSepetiTask.Services
{
    public interface IUserService
    {
        User Authenticate(string userName, string password);
        List<User> GetAll();
    }
}
