using Group11.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Friend
    {
        public int Id { get; set; }

        public ApplicationUser User1 { get; set; }

        public ApplicationUser User2 { get; set; }

    }
}
