using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Soul_and_talk.Model
{
    public class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public InstitutionType Type { get; set; }
    }
}
