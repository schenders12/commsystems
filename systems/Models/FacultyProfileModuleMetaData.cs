using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace systems.Models
{
    public class FacultyProfileModuleMetaData
    {
           [Required(ErrorMessage="Module Title is required")]
           [StringLength(100, ErrorMessage = "Module Title length Should be less than 100 characters")]
           public string ModuleTitle { get; set; }

           [Required(ErrorMessage = "Please add content to this module, or cancel using the button at the lower left.")]
           public string ModuleData { get; set; }
    }
    [MetadataType(typeof(FacultyProfileModuleMetaData))]
    public partial class FacultyProfileModule
    {
    }
}