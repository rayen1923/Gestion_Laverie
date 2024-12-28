using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionLaverie.Entites
{
    public class Propriétaire
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Laverie> Laveries { get; set; }

        public Propriétaire()
        {
            Laveries = new List<Laverie>();
        }

        public Propriétaire(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Laveries = new List<Laverie>();
            Password = password;
        }
    }
}
