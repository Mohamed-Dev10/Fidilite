using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Results
{
    public class AppFileResult
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public DateTime? DateCreation { get; set; }
        public string? NomPrenom { get; set; }
        public Stream Stream { get; set; }
        public byte[] Bytes { get; set; }
        public string FileName { get; set; }
    }
}

