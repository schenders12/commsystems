using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace systems.Models
{
    public class ProfilePhoto
    {
       // [Required]
        public HttpPostedFileBase Attachment { get; set; }

        public string profileId;
        public string profileFirstName;
        public string profileLastName;
        public string profileEMail;

        public ProfilePhoto(string id, string firstName, string lastName, string email)
        {
            profileId = id;
            profileFirstName = firstName;
            profileLastName = lastName;
            profileEMail = email;
        }
        public ProfilePhoto()
        {
            int i = 0;
        }
    }

}