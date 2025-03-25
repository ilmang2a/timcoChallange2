using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timcoChallange2.Models
{
    public class Person
    {
        [Index(0)]

        public string Name { get; set; }

        [Index(1)]

        public string Surename { get; set; }

        [Index(2)]
        public string Email { get; set; }

    }
}
