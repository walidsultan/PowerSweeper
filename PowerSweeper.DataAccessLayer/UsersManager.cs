using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerSweeper.DataTypes;
using Db4objects.Db4o.Linq;
namespace PowerSweeper.DataAccessLayer
{
    public class UsersManager : DataManager
    {
        public void AddUser(User user)
        {
            Client.Store(user);
            Client.Commit();
        }

        public void UpdateUser(User user)
        {
            User existingUser = GetUserByFbUserId(user.FBUserId);
            Client.Delete(existingUser);
            Client.Store(user);
            Client.Commit();
        }

        public User GetUserByFbUserId(string fbUserId )
        {
            return (from User u in Client select u).SingleOrDefault(u => u.FBUserId == fbUserId);
        }

        public bool IsExistingUser(string fbUserId)
        {
           return (from User u in Client select u).Count(u=>u.FBUserId ==fbUserId)>0;
        }

        public List<User> GetAllUsers()
        {
            return (from User u in Client select u).ToList();
        }
    }
}
