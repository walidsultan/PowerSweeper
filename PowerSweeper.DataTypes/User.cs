using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerSweeper.DataTypes
{
   public class User
    {
        public string FBUserId { get; set; }

        public DateTime Birthday { get; set; }

        public string Gender { get; set; }

        public string Name { get; set; }

        public string SmallPhoto { get; set; }

       public int LoginCounter { get; set; }
    }
}
