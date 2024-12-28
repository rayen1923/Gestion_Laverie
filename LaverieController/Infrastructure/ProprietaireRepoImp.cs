using MySql.Data.MySqlClient;
using GestionLaverie.Entites;
using LaverieController.Modele.Domaine;

namespace LaverieController.Infrastructure
{
    public class ProprietaireRepoImp : IProprietaireDAO
    {
        private readonly string _connectionString = "Server=localhost;Database=laverie;Uid=root;Pwd=;";

        public List<Propriétaire> GetAllPropriétairesWithDetails()
        {
            var proprietaires = new List<Propriétaire>();

            string query = @"SELECT p.Id AS PropId, p.Name AS PropName, 
                                    l.Id AS LaverieId, l.Name AS LaverieName, l.Adress, 
                                    m.Id AS MachineId, m.Marque, m.Modele, m.Etat, 
                                    c.Id AS CycleId, c.Cout, c.Duration 
                             FROM Propriétaires p
                             LEFT JOIN Laveries l ON p.Id = l.PropId
                             LEFT JOIN Machines m ON l.Id = m.LaverieId
                             LEFT JOIN Cycles c ON m.Id = c.MachineId";

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int propId = reader.GetInt32("PropId");
                        var propriétaire = proprietaires.Find(p => p.Id == propId);

                        if (propriétaire == null)
                        {
                            propriétaire = new Propriétaire
                            {
                                Id = propId,
                                Name = reader.GetString("PropName"),
                                Laveries = new List<Laverie>()
                            };
                            proprietaires.Add(propriétaire);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("LaverieId")))
                        {
                            int laverieId = reader.GetInt32("LaverieId");
                            var laverie = propriétaire.Laveries.Find(l => l.Id == laverieId);

                            if (laverie == null)
                            {
                                laverie = new Laverie
                                {
                                    Id = laverieId,
                                    Name = reader.GetString("LaverieName"),
                                    Adress = reader.GetString("Adress"),
                                    Machines = new List<Machine>()
                                };
                                propriétaire.Laveries.Add(laverie);
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("MachineId")))
                            {
                                int machineId = reader.GetInt32("MachineId");
                                var machine = laverie.Machines.Find(m => m.Id == machineId);

                                if (machine == null)
                                {
                                    machine = new Machine
                                    {
                                        Id = machineId,
                                        Marque = reader.GetString("Marque"),
                                        Modele = reader.GetString("Modele"),
                                        Etat = reader.GetInt32("Etat"),
                                        Cycles = new List<Cycle>()
                                    };
                                    laverie.Machines.Add(machine);
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("CycleId")))
                                {
                                    var cycle = new Cycle
                                    {
                                        Id = reader.GetInt32("CycleId"),
                                        Cout = reader.GetFloat("Cout"),
                                        Duration = reader.IsDBNull(reader.GetOrdinal("Duration"))
                                                    ? TimeSpan.Zero
                                                    : reader.GetTimeSpan("Duration")
                                    };
                                    machine.Cycles.Add(cycle);
                                }
                            }
                        }
                    }
                }
            }

            return proprietaires;
        }

        public int Login(string username, string password)
        {
            string query = "SELECT Id FROM Propriétaires WHERE Name = @username AND Password = @password";
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
    }
}
