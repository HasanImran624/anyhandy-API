using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Jobproposalattachment
    {
        public int AttachmentId { get; set; }
        public int? JobProposalId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public virtual Jobproposal JobProposal { get; set; }
    }
}
