using System;
using System.Collections.Generic;
using System.Text;

namespace Soul_and_talk.Model.Repositories
{
    public class InstitutionRepository
    {
        public List<Institution> Institutions { get; } = new List<Institution.Institution>();
        public List<Institution> GetAllInstitutions()
        {
            return Institutions;
        }
        public void AddInstitution(Institution institution)
        {
            Institutions.Add(institution);
        }

    }
}
