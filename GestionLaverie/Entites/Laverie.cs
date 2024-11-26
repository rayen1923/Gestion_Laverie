using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionLaverie.Entites
{
    public class Laverie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public int PropId { get; set; }
        public List<Machine> Machines { get; set; } 

        public Laverie()
        {
            Machines = new List<Machine>();
        }

        public Laverie(int id, string name, string adress, int propid)
        {
            Id = id;
            Name = name;
            Adress = adress;
            PropId = propid;
            Machines = new List<Machine>();
        }
    }
}

