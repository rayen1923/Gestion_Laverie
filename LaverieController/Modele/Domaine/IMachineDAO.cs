namespace LaverieController.Modele.Domaine
{
    public interface IMachineDAO
    {
        bool UpdateMachineEtat(int machineId, int cycleId);
    }
}
