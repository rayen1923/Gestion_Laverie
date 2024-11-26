using GestionLaverie.Entites;
using LaverieController.Modele.Domaine;

namespace LaverieController.Modele.Business
{
    public class ConfigurationBusiness
    {
        private readonly IProprietaireDAO _proprietaireDao;

        public ConfigurationBusiness(IProprietaireDAO proprietaireDao)
        {
            _proprietaireDao = proprietaireDao;
        }

        public List<Propriétaire> GetAllPropriétairesWithDetails()
        {
            var proprietaires = _proprietaireDao.GetAllPropriétairesWithDetails();
            return proprietaires ?? new List<Propriétaire>();
        }
    }
}
