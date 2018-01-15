using Group11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class Posts
    {
        public int Id { get; set; }
        public virtual ApplicationUser FromUser { get; set; }
        public virtual ApplicationUser ToUser { get; set; }
        public string Text { get; set; }

    }

    public class CreateViewModel
    {
        public Posts Post { get; set; }
        public ApplicationUser ToUser { get; set; }
    }
}
