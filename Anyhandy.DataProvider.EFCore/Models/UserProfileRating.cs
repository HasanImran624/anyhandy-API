using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.DataProvider.EFCore.Models
{
    public partial class UserProfileRating
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public float Rating { get; set; }
        public int RatedByUserId { get; set; }

        public virtual User RatedByUser { get; set; }
        public virtual User User { get; set; }
    }
}
