using Microsoft.Extensions.Caching.Memory;
using ModernRecrut.Favoris.API.Interfaces;
using ModernRecrut.Favoris.API.Models;
using ModernRecrut.Favoris.API.Models.DTO;
using System.Net;

namespace ModernRecrut.Favoris.API.Services
{
    public class FavorisService : IFavorisService
    {
        private readonly IMemoryCache _memoryCache;
        private string? _cleUtilisateur;
        private List<string>? _erreurs;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        private readonly IFavorisServiceProxy _favorisServiceProxy;
        private readonly IConfiguration _config;


        public FavorisService(IMemoryCache memoryCache, IFavorisServiceProxy favorisServiceProxy, IConfiguration config)
        {
            _memoryCache = memoryCache;
            _erreurs = new List<string>();
            _favorisServiceProxy = favorisServiceProxy;
            _cacheOptions = new MemoryCacheEntryOptions();
            _config = config;

        }



        public async Task<Models.Favoris> ObtenirTout(string ipAddress)
        {

            if (VerifierAdresseIp(ipAddress))
                _cleUtilisateur = ipAddress;
            else
                return null;


            List<OffreEmploi> offresEmploisValides = await _favorisServiceProxy.ObtenirOffresEmploisValides();
            List<OffreEmploi> ListeFavoris = new List<OffreEmploi>();
            foreach (var offre in offresEmploisValides)
            {
                if (_memoryCache.TryGetValue(_cleUtilisateur + offre.Id, out OffreEmploi offreEmploi))
                {
                    ListeFavoris.Add(offreEmploi);
                }
            }

            return new Models.Favoris { Cle = _cleUtilisateur, Contenu = ListeFavoris };


        }
        public async Task<List<string>> Ajouter(RequeteFavoris requeteFavoris)
        {
            Guid offreID = requeteFavoris.OffreEmploiID;
            OffreEmploi offreEmploi = await ObtenirOffreEmploi(offreID);
            _cleUtilisateur = requeteFavoris.Cle;

            if (offreEmploi == null || string.IsNullOrWhiteSpace(_cleUtilisateur))
            {
                _erreurs.Add("Erreur lors de l'ajout de l'offre dans les favoris");
                return _erreurs;
            }

            if (!_memoryCache.TryGetValue(_cleUtilisateur + offreID, out OffreEmploi offre))
            {
                //si l'offre va expirer bientot, il faut vérifier que le favori n'expire pas après l'offre en question
                if (offreEmploi.DateFin < DateTime.Now.AddHours(_config.GetValue<int>("EXPIRATION_GLOBALE")) && offreEmploi.DateFin > DateTime.Now.AddHours(_config.GetValue<int>("EXPIRATION_NON_ACCES")))
                {
                    _cacheOptions.SlidingExpiration = TimeSpan.FromHours(_config.GetValue<int>("EXPIRATION_NON_ACCES"));
                    _cacheOptions.AbsoluteExpirationRelativeToNow = offreEmploi.DateFin - DateTime.Now;
                }
                else if (offreEmploi.DateFin < DateTime.Now.AddHours(_config.GetValue<int>("EXPIRATION_NON_ACCES")))
                {
                    _cacheOptions.AbsoluteExpirationRelativeToNow = offreEmploi.DateFin - DateTime.Now;
                }
                else
                {

                    _cacheOptions.SlidingExpiration = TimeSpan.FromHours(_config.GetValue<int>("EXPIRATION_NON_ACCES"));
                    _cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_config.GetValue<int>("EXPIRATION_GLOBALE"));
                }

                _cacheOptions.Size = CountCaracteresOffreEmploi(offreEmploi);
                _memoryCache.Set(_cleUtilisateur + offreID, offreEmploi, _cacheOptions);
                return _erreurs;
            }
            else
            {
                _erreurs.Add("L'offre existe déjà dans la liste des favoris de l'utilisateur");
                return _erreurs;
            }


        }
        public async Task<List<string>> Supprimer(string ipAddress, Guid offreId)
        {
            if (VerifierAdresseIp(ipAddress))
                _cleUtilisateur = ipAddress;
            else
            {
                _erreurs.Add("L'addresse ip est invalide");
                return _erreurs;
            }
            if (offreId == null)
            {
                _erreurs.Add("L'offre d'emploi est invalide");
                return _erreurs;
            }
            if (_memoryCache.TryGetValue(_cleUtilisateur + offreId, out OffreEmploi offreEmploi))
            {

                _memoryCache.Remove(_cleUtilisateur + offreId);
                return _erreurs;
            }
            _erreurs.Add("Erreur lors de la suppression de l'offre d'emploi des favoris");
            return _erreurs;
        }

        private bool VerifierAdresseIp(string ipAddress)
        {

            bool ipEstValid = IPAddress.TryParse(ipAddress, out IPAddress adresseIp);
            return ipEstValid;

        }
        private async Task<OffreEmploi> ObtenirOffreEmploi(Guid offreID)
        {

            List<OffreEmploi> offresEmplois = await _favorisServiceProxy.ObtenirOffresEmploisValides();
            return offresEmplois.FirstOrDefault(o => o.Id == offreID);
        }

        private int CountCaracteresOffreEmploi(OffreEmploi emploi)
        {
            int id = emploi.Id.ToString().Length;
            int dateAffichage = emploi.DateAffichage.ToString().Length;
            int dateFin = emploi.DateFin.ToString().Length;
            int description = emploi.Description.Length;
            int poste = emploi.Poste.Length;

            return id + dateAffichage + dateFin + description + poste;
        }
    }
}

