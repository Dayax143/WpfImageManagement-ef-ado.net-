using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEFProfile.EF
{
    public class tblMedia
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public long FileSize { get; set; }
        public byte[] FileData { get; set; }
        public DateTime UploadedAt { get; set; }
        public string Description { get; set; }
    }
}
