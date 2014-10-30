using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace systems.Models
{
    public class FacultyPageMetaData
    {
           [Required(ErrorMessage="Page Title is required")]
           [StringLength(100, ErrorMessage = "Page Title length Should be less than 100 characters")]
           public string PageTitle { get; set; }

           [Required(ErrorMessage = "Link URL is required.  This should be a path/URL to an outside page that you have already created.")]
           [DataType(DataType.Url)]
           public string LinkURL { get; set; }
    }
    [MetadataType(typeof(FacultyPageMetaData))]
    public partial class FacultyPage
    {
    }
}