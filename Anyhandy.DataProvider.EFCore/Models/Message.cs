using System;
using System.Collections.Generic;

#nullable disable

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class Message
    {
        public Message()
        {
            Messageattachments = new HashSet<Messageattachment>();
        }

        public int MessageId { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }
        public string MessageText { get; set; }

        public virtual User Receiver { get; set; }
        public virtual User Sender { get; set; }
        public virtual ICollection<Messageattachment> Messageattachments { get; set; }
    }
}
