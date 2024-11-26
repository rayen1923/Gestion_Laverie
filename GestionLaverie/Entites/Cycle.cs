using System;
using System.Collections.Generic;

namespace GestionLaverie.Entites
{
    public class Cycle
    {
        public int Id { get; set; }
        public float Cout { get; set; }
        public TimeSpan Duration { get; set; }

        public Cycle() { }

        public Cycle(int id, float cout, TimeSpan duration)
        {
            Id = id;
            Cout = cout;
            Duration = duration;
        }
    }
}
