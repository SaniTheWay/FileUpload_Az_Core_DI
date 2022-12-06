using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FileUplaodAz_Core.Models
{
    public partial class BlobRecord
    {
        public int Id { get; set; }
        public string BlobName { get; set; }
        public DateTime Date { get; set; }
        public string Url { get; set; }
    }
}
