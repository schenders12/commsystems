using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace systems.Models
{

    public class LocalBankMembershipProvider : MembershipProvider
    {

        private LocalBankRepository repository;

        public int MinPasswordLength
        {
            get
            {
                return 5;
            }
        }

		public override int MinRequiredPasswordLength
		{
			get
			{
				return MinPasswordLength;
			}
		}

        public LocalBankMembershipProvider()
        {
            this.repository = new LocalBankRepository();
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(password.Trim()) || string.IsNullOrEmpty(username.Trim()))
                return false;

            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5");

            return this.repository.GetAllUsers().Any(user => (user.UserName == username.Trim()) && (user.Password == hash));
        }

        public void CreateUser(string username, string fullName, string password, string email, string roleName)
        {
            this.repository.CreateUser(username, fullName, password, email, roleName);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword) || string.IsNullOrEmpty(newPassword.Trim()))
                return false;

            User user = repository.GetUser(username);
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword.Trim(), "md5");

            user.Password = hash;
            repository.Save();

            return true;
        }

        #region Not Implemented MembershipProvider Methods

        #region Properties

        /// <summary>
        /// This property is not implemented.
        /// </summary>
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int MaxInvalidPasswordAttempts
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int MinRequiredNonAlphanumericCharacters
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int PasswordAttemptWindow
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override MembershipPasswordFormat PasswordFormat
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override string PasswordStrengthRegularExpression
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool RequiresQuestionAndAnswer
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool RequiresUniqueEmail
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool EnablePasswordReset
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool EnablePasswordRetrieval
		{
			get { throw new NotImplementedException(); }
		}

        #endregion

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}