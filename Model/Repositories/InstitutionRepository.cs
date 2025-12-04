using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using Soul_and_talk.Model;
using System.Collections.Generic;

namespace Soul_and_talk.Model.Repositories
{
    public class InstitutionRepository
    {
        private List<Institution> _institutions = new List<Institution>();

        public List<Institution> GetAllInstitutions()
        {
            return _institutions;
        }

        public void AddInstitution(Institution institution)
        {
            _institutions.Add(institution);
        }
    }
}
