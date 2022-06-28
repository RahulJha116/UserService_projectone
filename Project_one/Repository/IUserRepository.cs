using Project_one.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_one.Repository
{
   public  interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUserByID(int UserId);

        string InsertUser(User user);
        void DeleteUser(int UserId);
        void UpdateUser(User user);
        void Save();
    }
}
