using Group11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class Message
    {
        public int Id { get; set; }
        public ApplicationUser Receiver { get; set; }
        public ApplicationUser Sender { get; set; }
        public string Text { get; set; }

    }
}
