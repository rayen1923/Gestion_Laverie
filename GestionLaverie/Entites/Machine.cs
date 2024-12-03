using System;
using System.Collections.Generic;

namespace GestionLaverie.Entites
{
    public class Machine
    {
        public int Id { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public int Etat { get; set; }
        public List<Cycle> Cycles { get; set; }

        public Machine()
        {
            Cycles = new List<Cycle>();
        }

        public Machine(int id, string marque, string modele, int etat, List<Cycle> cycles)
        {
            Id = id;
            Marque = marque;
            Modele = modele;
            Etat = etat;
            Cycles = cycles;
        }
    }
}
