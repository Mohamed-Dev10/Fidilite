using System;
using System.Collections.Generic;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Helpers
{
    public class FTPSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string BasePath { get; set; }

        public bool UseSSL { get; set; } = false;
        public bool UsePassive { get; set; } = true;
        public int Timeout { get; set; } = 30000;
    }
}

