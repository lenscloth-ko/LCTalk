using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCTalk.Web.Models
{
    public class User
    {
        public User()
        {
        }

        public int id { get; set; }
        public string name { get; set; }
        public DateTime created_at { get; set; }
    }
}