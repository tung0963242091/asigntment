using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T1808A_Asigntment.Entity
{
    class Student
    {
        public string rollNumber { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return rollNumber + " - " + name;
        }
    }
}
