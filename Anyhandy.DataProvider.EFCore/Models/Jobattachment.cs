using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class JobAttachment
    {
        public int AttachmentId { get; set; }
        public int? JobId { get; set; }
        public string FilePath { get; set; }
        public string FileShortDescription { get; set; }

        public virtual Job Job { get; set; }
    }
}
