using LaverieController.Modele.Domaine;
using MySql.Data.MySqlClient;

namespace LaverieController.Infrastructure
{
    public class MachineRepoImp : IMachineDAO
    {
        private readonly string _connectionString = "Server=localhost;Database=laverie;Uid=root;Pwd=;";

        public bool UpdateMachineEtat(int machineId)
        {
            string selectQuery = "SELECT Etat FROM Machines WHERE Id = @MachineId";
            string updateQuery = "UPDATE Machines SET Etat = @NewEtat WHERE Id = @MachineId";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    int currentEtat;
                    using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@MachineId", machineId);

                        object result = selectCmd.ExecuteScalar();
                        if (result == null)
                        {
                            return false;
                        }

                        currentEtat = Convert.ToInt32(result);
                    }

                    int newEtat = currentEtat == 1 ? 0 : 1;

                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@NewEtat", newEtat);
                        updateCmd.Parameters.AddWithValue("@MachineId", machineId);

                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the machine state: {ex.Message}");
            }
        }
    }
}
