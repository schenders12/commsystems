using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace systems.Models
{

    public class LocalBankRepository
    {

        #region Variables

        private CommonEntities entities = new CommonEntities();

        private const string MissingRole = "Role does not exist";
        private const string MissingUser = "User does not exist";
        private const string TooManyUser = "User already exists";
        private const string TooManyRole = "Role already exists";
        private const string AssignedRole = "Cannot delete a role with assigned users";

        #endregion

        #region Properties

        public int NumberOfUsers
        {
            get
            {
                return this.entities.Users.Count();
            }
        }

        public int NumberOfRoles
        {
            get
            {
                return this.entities.Roles.Count();
            }
        }

        #endregion

        #region Constructors

        public LocalBankRepository()
        {
            this.entities = new CommonEntities();
        }

        #endregion

        #region Query Methods

        public IQueryable<User> GetAllUsers()
        {
            return from user in entities.Users
                   orderby user.UserName
                   select user;
        }

        public User GetUser(int id)
        {
            return entities.Users.SingleOrDefault(user => user.ID == id);
        }

        public User GetUser(string userName)
        {
            return entities.Users.SingleOrDefault(user => user.UserName == userName);
        }

        public IQueryable<User> GetUsersForRole(string roleName)
        {
            return GetUsersForRole(GetRole(roleName));
        }

        public IQueryable<User> GetUsersForRole(int id)
        {
            return GetUsersForRole(GetRole(id));
        }

        public IQueryable<User> GetUsersForRole(Role role)
        {
            if (!RoleExists(role))
                throw new ArgumentException(MissingRole);

            return from user in entities.Users
                   where user.RoleID == role.ID
                   orderby user.UserName
                   select user;
        }

        public IQueryable<Role> GetAllUserRoles()
        {
            return from role in entities.Roles
                   orderby role.Name
                   select role;
        }

        public Role GetRole(int id)
        {
            return entities.Roles.SingleOrDefault(role => role.ID == id);
        }

        public Role GetRole(string name)
        {
            return entities.Roles.SingleOrDefault(role => role.Name == name);
        }

        public Role GetRoleForUser(string userName)
        {
            return GetRoleForUser(GetUser(userName));
        }

        public Role GetRoleForUser(int id)
        {
            return GetRoleForUser(GetUser(id));
        }

        public Role GetRoleForUser(User user)
        {
            if (!UserExists(user))
                throw new ArgumentException(MissingUser);

            return user.Role;
        }

        #endregion

        #region Insert/Delete

        private void AddUser(User user)
        {
            if (UserExists(user))
                throw new ArgumentException(TooManyUser);

         //   entities.Users.AddObject(user);
        }

        public void CreateUser(string username, string name, string password, string email, string roleName)
        {
            Role role = GetRole(roleName);

            if (string.IsNullOrEmpty(username.Trim()))
                throw new ArgumentException("The user name provided is invalid. Please check the value and try again.");
            if (string.IsNullOrEmpty(name.Trim()))
                throw new ArgumentException("The name provided is invalid. Please check the value and try again.");
            if (string.IsNullOrEmpty(password.Trim()))
                throw new ArgumentException("The password provided is invalid. Please enter a valid password value.");
            if (string.IsNullOrEmpty(email.Trim()))
                throw new ArgumentException("The e-mail address provided is invalid. Please check the value and try again.");
            if (!RoleExists(role))
                throw new ArgumentException("The role selected for this user does not exist! Contact an administrator!");
            if (this.entities.Users.Any(user => user.UserName == username))
                throw new ArgumentException("Username already exists. Please enter a different user name.");

            User newUser = new User()
            {
                UserName = username,
                Name = name,
                Password = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5"),
                Email = email,
                RoleID = role.ID
            };

            try
            {
                AddUser(newUser);
            }
            catch (ArgumentException ae)
            {
                throw ae;
            }
            catch (Exception e)
            {
                throw new ArgumentException("The authentication provider returned an error. Please verify your entry and try again. " +
                    "If the problem persists, please contact your system administrator.");
            }

            // Immediately persist the user data
            Save();
        }

        public void DeleteUser(User user)
        {
            //if (!UserExists(user))
            //    throw new ArgumentException(MissingUser);

            //entities.Users.DeleteObject(user);
        }

        public void DeleteUser(string userName)
        {
            DeleteUser(GetUser(userName));
        }

        public void AddRole(Role role)
        {
            if (RoleExists(role))
                throw new ArgumentException(TooManyRole);

            //entities.Roles.AddObject(role);
        }

        public void AddRole(string roleName)
        {
            Role role = new Role()
            {
                Name = roleName
            };

            AddRole(role);
        }

        public void DeleteRole(Role role)
        {
            //if (!RoleExists(role))
            //    throw new ArgumentException(MissingRole);

            //if (GetUsersForRole(role).Count() > 0)
            //    throw new ArgumentException(AssignedRole);

            //entities.Roles.DeleteObject(role);
        }

        public void DeleteRole(string roleName)
        {
            DeleteRole(GetRole(roleName));
        }

        #endregion

        #region Persistence

        public void Save()
        {
            entities.SaveChanges();
        }

        #endregion

        #region Helper Methods

        public bool UserExists(User user)
        {
            if (user == null)
                return false;

            return (entities.Users.SingleOrDefault(u => u.ID == user.ID || u.UserName == user.UserName) != null);
        }

        public bool RoleExists(Role role)
        {
            if (role == null)
                return false;

            return (entities.Roles.SingleOrDefault(r => r.ID == role.ID || r.Name == role.Name) != null);
        }

        #endregion

    }

}