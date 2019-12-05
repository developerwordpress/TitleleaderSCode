using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TitleLeader.Models.Dataupload
{
    public class ExcelUpload
    {
        public string FileName { get; set; }
        public HttpPostedFileBase UploadFile { get; set; }
    }
}