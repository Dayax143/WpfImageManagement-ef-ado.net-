using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEFProfile.EF
{
    public class Sessions
    {
        [Key]
        public int ses_id { get; set; }

        public string? ses_user { get; set; }

        public DateTime ses_time {  get; set; }

        public string? ses_status { get; set; }
            
    }
}
