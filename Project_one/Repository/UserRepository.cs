using Microsoft.EntityFrameworkCore;
using Project_one.DbContexts;
using Project_one.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_one.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _dbContext;

        public UserRepository(UserContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void DeleteUser(int userId)
        {
            var user = _dbContext.Users.Find(userId);
            _dbContext.Users.Remove(user);
        }

        public User GetUserByID(int UserId)
        {
            return _dbContext.Users.Find(UserId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _dbContext.Users.ToList();
        }

        public string InsertUser(User user)
        {
            var a = UserExist(user.EmailId);

            if (a.Any())
            { 
                
                return ("User exist try to login");
            }
            _dbContext.Add(user);
            Save();

            return ("user registerd!!!");
                
        }

        public IQueryable<User> UserExist(string EmailId)
        {
            var result = _dbContext.Users.AsQueryable();
            if (EmailId != null)
            {
                if (!string.IsNullOrEmpty(EmailId))
                {
                    result = result.Where(x => x.EmailId == EmailId);

                }
                else
                {
                    result = null;
                }

            }
            return result;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            Save();
        }
    }
}
