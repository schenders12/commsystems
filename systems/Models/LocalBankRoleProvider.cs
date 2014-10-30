using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace systems.Models
{

    public class LocalBankRoleProvider : RoleProvider
    {

        #region Variables

        private LocalBankRepository repository;

        #endregion

        #region Constructors

        public LocalBankRoleProvider()
        {
            this.repository = new LocalBankRepository();
        }

        #endregion

        #region Implemented RoleProvider Methods

        public override bool IsUserInRole(string userName, string roleName)
        {
            User user = repository.GetUser(userName);
            Role role = repository.GetRole(roleName);

            if (!repository.UserExists(user))
                return false;
            if (!repository.RoleExists(role))
                return false;

            return user.Role.Name == role.Name;
        }

        public override string[] GetRolesForUser(string userName)
        {
            Role role = this.repository.GetRoleForUser(userName);
            if (!this.repository.RoleExists(role))
                return new string[] { string.Empty };

            return new string[] { role.Name };
        }

        #endregion

        #region Not Implemented RoleProvider Methods

        #region Properties

        /// <summary>
        /// This property is not implemented.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}