using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class MessageAttachment
    {
        public int AttachmentId { get; set; }
        public int? MessageId { get; set; }
        public string FilePath { get; set; }

        public virtual Message Message { get; set; }
    }
}
