using System;
using System.Collections.Generic;
using System.Text;

namespace BRICOMA.ECOMMERCE.Models.Helpers
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
    }
}

