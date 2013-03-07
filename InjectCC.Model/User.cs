using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace InjectCC.Model
{
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }
    }
}
