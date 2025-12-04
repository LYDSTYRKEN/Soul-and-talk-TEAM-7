using System.IO;

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

        // id;name;type
        public void SaveToFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                foreach (Institution inst in _institutions)
                {
                    string line = inst.Id + ";" + inst.Name + ";" + inst.Type;
                    writer.WriteLine(line);
                }
            }
        }
    }
}
