using GestionLaverie.Entites;

namespace LaverieController.Modele.Domaine
{
    public interface IProprietaireDAO
    {
        List<Propriétaire> GetAllPropriétairesWithDetails();
    }
}
