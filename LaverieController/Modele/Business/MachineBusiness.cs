using LaverieController.Modele.Domaine;

namespace LaverieController.Modele.Business
{
    public class MachineBusiness
    {
        private readonly IMachineDAO _machineDAO;

        public MachineBusiness(IMachineDAO machineDAO)
        {
            _machineDAO = machineDAO;
        }

        public bool ToggleMachineEtat(int machineId)
        {
            return _machineDAO.UpdateMachineEtat(machineId);
        }
    }
}
