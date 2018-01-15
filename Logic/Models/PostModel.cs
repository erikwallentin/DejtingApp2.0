using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
    }
}
