using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionLaverie.Entites
{
    class CycleHistory
    {
        public int MachineId { get; set; }
        public int CycleId { get; set; }
        public DateTime Timestamp { get; set; }

        public CycleHistory() { }

        public CycleHistory(int machineId, int cycleId, DateTime timestamp)
        {
            MachineId = machineId;
            CycleId = cycleId;
            Timestamp = timestamp;
        }
    }
}
