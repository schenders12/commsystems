using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace systems.Models
{
    public class ManageFilesMetaData
    {
        [Display(Name="FileName", Prompt="Select a file to upload...")]
        public object fileName { get; set; }

        public HttpPostedFileBase File { get; set; }

    }
    public class FileUploadModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
    [MetadataType(typeof(ManageFilesMetaData))]
    public partial class ManageFiles
    {
    }
}