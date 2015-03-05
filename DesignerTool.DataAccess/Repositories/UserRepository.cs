using DesignerTool.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesignerTool.DataAccess.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IDesignerToolContext ctx)
            : base(ctx)
        {

        }

        #region CRUD

        public void AddNew(User user)
        {
            base.Context.Users.Add(user);
        }

        public void Delete(User user)
        {
            base.Context.Users.Remove(user);
        }

        #endregion

        #region Get

        public User GetById(long userID)
        {
            return base.Context.Users
                .FirstOrDefault(u => u.UserID == userID && u.IsActive == true);
        }

        #endregion

        #region List

        public IQueryable<User> ListAll()
        {
            return base.Context.Users.Where(u => u.IsActive == true);
        }

        public IQueryable<User> Search_Paged(string searchText, int pageStartIndex, int pageSize)
        {
            return base.Context.Users.Where(u => u.Username.Contains(searchText))
                   .OrderBy(u => u.Username)
                   .Skip(pageStartIndex)
                   .Take(pageSize);
        }

        #endregion

        #region Login & Permissions

        public User LoginUser(string username, string password)
        {
            var user = this.Context.Users
                .FirstOrDefault(u => u.Username == username && u.IsActive == true);

            if (user != null)
            {
                if (user.ValidatePassword(password))
                {
                    // Valid Login.
                    return user;
                }
            }

            return null;
        }

        #endregion
    }
}
