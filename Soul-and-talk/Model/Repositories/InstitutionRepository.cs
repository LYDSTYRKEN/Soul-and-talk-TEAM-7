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
        public void LoadFromFile(string path)
        {
            // 1) Institutions
            if (File.Exists("institutions.txt"))
            {
                string[] lines = File.ReadAllLines("institutions.txt");

                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');   // 0=Id, 1=Name, 2=Type

                    Institution inst = new Institution();
                    inst.Id = int.Parse(parts[0]);
                    inst.Name = parts[1];
                    inst.Type = (InstitutionType)Enum.Parse(typeof(InstitutionType), parts[2]);

                    AddInstitution(inst);
                }
            }
        }
    }
}
