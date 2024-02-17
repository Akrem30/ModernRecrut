using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Controllers;
using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Models;
using ModernRecrut.MVC.Models.DTO;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ModernRecrut.MVC.UnitTests.Controllers
{
	public class PostulationsControllerTests
	{
        #region Create Http Post Tests
        [Fact]
		public async Task CreatePost_CandidatSansCV_Retourne_ViewResultAvecModelStateError()
		{
			//Étant donné
			Fixture fixture = new Fixture();
			var requetePostulation = fixture.Create<RequetePostulation>();
			requetePostulation.DateDisponibilite = DateTime.Now.AddDays(1);
			requetePostulation.PretentionSalariale = 80000;
           
            fixture.RepeatCount = 3;
			List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();
			
			Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
			Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
			Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
									new Mock<IUserStore<Utilisateur>>().Object,
									new Mock<IOptions<IdentityOptions>>().Object,
									new Mock<IPasswordHasher<Utilisateur>>().Object,
									new IUserValidator<Utilisateur>[0],
									new IPasswordValidator<Utilisateur>[0],
									new Mock<ILookupNormalizer>().Object,
									new Mock<IdentityErrorDescriber>().Object,
									new Mock<IServiceProvider>().Object,
									new Mock<ILogger<UserManager<Utilisateur>>>().Object);

			mockDocumentsService.Setup(s => s.ObtenirDocumentsSelonId(It.IsAny<Guid>())).ReturnsAsync(new List<string> { "Diplome", "LETTREDEMOTIVATION" });
			mockPostulationsService.Setup(s=>s.AjouterPostulation(It.IsAny<RequetePostulation>()));
			mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
           

            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

			//Lorsque
			var actionResult = await postulationsController.Create(requetePostulation) as ViewResult;

			//Alors
			actionResult.Should().NotBeNull();
			var requetePostulationResult = actionResult.Model as RequetePostulation;
			requetePostulationResult.Should().Be(requetePostulation);
			mockPostulationsService.Verify(p => p.AjouterPostulation(It.IsAny<RequetePostulation>()), Times.Never);

			//Validation des erreurs dans le ModelState
			var modelState = postulationsController.ModelState;
			modelState.IsValid.Should().BeFalse();
			modelState.ErrorCount.Should().Be(1);
			modelState.Should().ContainKey(string.Empty);
			modelState[string.Empty].Errors.FirstOrDefault().ErrorMessage.Should().Be("Un CV est obligatoire pour postuler. Veuillez déposer au préalable un CV dans votre espace Documents");
		}

		[Fact]
		public async Task CreatePost_CandidatSansLettreDeMotivation_Retourne_ViewResultAvecModelStateError()
		{
			//Étant donné
			Fixture fixture = new Fixture();
			var requetePostulation = fixture.Create<RequetePostulation>();
			
			requetePostulation.DateDisponibilite = DateTime.Now.AddDays(1);
			requetePostulation.PretentionSalariale = 80000;
            
			fixture.RepeatCount = 3;
			List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();

			Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
			Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
			Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
								new Mock<IUserStore<Utilisateur>>().Object,
								new Mock<IOptions<IdentityOptions>>().Object,
								new Mock<IPasswordHasher<Utilisateur>>().Object,
								new IUserValidator<Utilisateur>[0],
								new IPasswordValidator<Utilisateur>[0],
								new Mock<ILookupNormalizer>().Object,
								new Mock<IdentityErrorDescriber>().Object,
								new Mock<IServiceProvider>().Object,
								new Mock<ILogger<UserManager<Utilisateur>>>().Object);

			mockDocumentsService.Setup(s => s.ObtenirDocumentsSelonId(It.IsAny<Guid>())).ReturnsAsync(new List<string> { "Diplome", "CV" });
			mockPostulationsService.Setup(s => s.AjouterPostulation(It.IsAny<RequetePostulation>()));
			mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
			

			var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

			//Lorsque
			var actionResult = await postulationsController.Create(requetePostulation) as ViewResult;

			//Alors
			actionResult.Should().NotBeNull();
			var requetePostulationResult = actionResult.Model as RequetePostulation;
			requetePostulationResult.Should().Be(requetePostulation);
			mockPostulationsService.Verify(p => p.AjouterPostulation(It.IsAny<RequetePostulation>()), Times.Never);

			//Validation des erreurs dans le ModelState
			var modelState = postulationsController.ModelState;
			modelState.IsValid.Should().BeFalse();
			modelState.ErrorCount.Should().Be(1);
			modelState.Should().ContainKey(string.Empty);
			modelState[string.Empty].Errors.FirstOrDefault().ErrorMessage.Should().Be("Une lettre de motivation est obligatoire pour postuler. Veuillez déposer au préalable une lettre de motivation dans votre espace Documents");
		}

		[Fact]
		public async Task CreatePost_AvecCandidatDejaPostuleALaMemeOffre_Retourne_ViewResultAvecModelStateError()
		{
			//Étant donné
			Fixture fixture = new Fixture();
			var requetePostulation = fixture.Create<RequetePostulation>();
			requetePostulation.DateDisponibilite = DateTime.Now.AddDays(1);
			requetePostulation.PretentionSalariale = 80000;
           
            fixture.RepeatCount = 3;
			List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();
			requetePostulation.CandidatID = postulations[0].CandidatID;
			requetePostulation.OffreEmploiID = postulations[0].OffreEmploiID;

			Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
			Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
			Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
			Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
								new Mock<IUserStore<Utilisateur>>().Object,
								new Mock<IOptions<IdentityOptions>>().Object,
								new Mock<IPasswordHasher<Utilisateur>>().Object,
								new IUserValidator<Utilisateur>[0],
								new IPasswordValidator<Utilisateur>[0],
								new Mock<ILookupNormalizer>().Object,
								new Mock<IdentityErrorDescriber>().Object,
								new Mock<IServiceProvider>().Object,
								new Mock<ILogger<UserManager<Utilisateur>>>().Object);

			mockDocumentsService.Setup(s => s.ObtenirDocumentsSelonId(It.IsAny<Guid>())).ReturnsAsync(new List<string> { "LETTREDEMOTIVATION", "CV" });
			mockPostulationsService.Setup(s => s.AjouterPostulation(It.IsAny<RequetePostulation>()));
			mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
   
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

			//Lorsque
			var actionResult = await postulationsController.Create(requetePostulation) as ViewResult;

			//Alors
			actionResult.Should().NotBeNull();
			var requetePostulationResult = actionResult.Model as RequetePostulation;
			requetePostulationResult.Should().Be(requetePostulation);
			mockPostulationsService.Verify(p => p.AjouterPostulation(It.IsAny<RequetePostulation>()), Times.Never);

			//Validation des erreurs dans le ModelState
			var modelState = postulationsController.ModelState;
			modelState.IsValid.Should().BeFalse();
			modelState.ErrorCount.Should().Be(1);
			modelState.Should().ContainKey(string.Empty);
			modelState[string.Empty].Errors.FirstOrDefault().ErrorMessage.Should().Be("Vous avez déjà postulé à cette offre d'emploi");
		}
		[Fact]
		public async Task CreatePost_AvecPretentionSalarialesSupA150000_Retourne_ViewResultAvecModelStateError()
		{
			//Étant donné
			Fixture fixture = new Fixture();
			var requetePostulation = fixture.Create<RequetePostulation>();
			requetePostulation.DateDisponibilite = DateTime.Now.AddDays(1);
			requetePostulation.PretentionSalariale = 180000;
            
            fixture.RepeatCount = 3;
			List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();
			

			Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
			Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
			Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
									new Mock<IUserStore<Utilisateur>>().Object,
									new Mock<IOptions<IdentityOptions>>().Object,
									new Mock<IPasswordHasher<Utilisateur>>().Object,
									new IUserValidator<Utilisateur>[0],
									new IPasswordValidator<Utilisateur>[0],
									new Mock<ILookupNormalizer>().Object,
									new Mock<IdentityErrorDescriber>().Object,
									new Mock<IServiceProvider>().Object,
									new Mock<ILogger<UserManager<Utilisateur>>>().Object);

			mockDocumentsService.Setup(s => s.ObtenirDocumentsSelonId(It.IsAny<Guid>())).ReturnsAsync(new List<string> { "LETTREDEMOTIVATION", "CV" });
			mockPostulationsService.Setup(s => s.AjouterPostulation(It.IsAny<RequetePostulation>()));
			mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
          
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

			//Lorsque
			var actionResult = await postulationsController.Create(requetePostulation) as ViewResult;

			//Alors
			actionResult.Should().NotBeNull();
			var requetePostulationResult = actionResult.Model as RequetePostulation;
			requetePostulationResult.Should().Be(requetePostulation);
			mockPostulationsService.Verify(p => p.AjouterPostulation(It.IsAny<RequetePostulation>()), Times.Never);

			//Validation des erreurs dans le ModelState
			var modelState = postulationsController.ModelState;
			modelState.IsValid.Should().BeFalse();
			modelState.ErrorCount.Should().Be(1);
			modelState.Should().ContainKey("PretentionSalariale");
			modelState["PretentionSalariale"].Errors.FirstOrDefault().ErrorMessage.Should().Be("Votre prétention salariale est au-delà de nos limites");
		}
        [Fact]
        public async Task CreatePost_AvecPretentionSalarialesNegative_Retourne_ViewResultAvecModelStateError()
        {
            //Étant donné
            Fixture fixture = new Fixture();
            var requetePostulation = fixture.Create<RequetePostulation>();
            requetePostulation.DateDisponibilite = DateTime.Now.AddDays(1);
            requetePostulation.PretentionSalariale = -180000;
           
            fixture.RepeatCount = 3;
            List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();


            Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
            Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
            Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
                                    new Mock<IUserStore<Utilisateur>>().Object,
                                    new Mock<IOptions<IdentityOptions>>().Object,
                                    new Mock<IPasswordHasher<Utilisateur>>().Object,
                                    new IUserValidator<Utilisateur>[0],
                                    new IPasswordValidator<Utilisateur>[0],
                                    new Mock<ILookupNormalizer>().Object,
                                    new Mock<IdentityErrorDescriber>().Object,
                                    new Mock<IServiceProvider>().Object,
                                    new Mock<ILogger<UserManager<Utilisateur>>>().Object);

            mockDocumentsService.Setup(s => s.ObtenirDocumentsSelonId(It.IsAny<Guid>())).ReturnsAsync(new List<string> { "LETTREDEMOTIVATION", "CV" });
            mockPostulationsService.Setup(s => s.AjouterPostulation(It.IsAny<RequetePostulation>()));
            mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
         
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

            //Lorsque
            var actionResult = await postulationsController.Create(requetePostulation) as ViewResult;

            //Alors
            actionResult.Should().NotBeNull();
            var requetePostulationResult = actionResult.Model as RequetePostulation;
            requetePostulationResult.Should().Be(requetePostulation);
            mockPostulationsService.Verify(p => p.AjouterPostulation(It.IsAny<RequetePostulation>()), Times.Never);

            //Validation des erreurs dans le ModelState
            var modelState = postulationsController.ModelState;
            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(1);
            modelState.Should().ContainKey("PretentionSalariale");
            modelState["PretentionSalariale"].Errors.FirstOrDefault().ErrorMessage.Should().Be("Veuillez saisir une valeur valide pour le salaire");
        }
        [Fact]
		public async Task CreatePost_AvecDateDisponibiliteSupA45Jours_Retourne_ViewResultAvecModelStateError()
		{
			//Étant donné
			Fixture fixture = new Fixture();
			var requetePostulation = fixture.Create<RequetePostulation>();
			requetePostulation.DateDisponibilite = DateTime.Now.AddDays(60);
			requetePostulation.PretentionSalariale = 80000;
            
            fixture.RepeatCount = 3;
			List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();

            DateTime dateLimite = DateTime.Now.AddDays(45);


            Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
			Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
			Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
									new Mock<IUserStore<Utilisateur>>().Object,
									new Mock<IOptions<IdentityOptions>>().Object,
									new Mock<IPasswordHasher<Utilisateur>>().Object,
									new IUserValidator<Utilisateur>[0],
									new IPasswordValidator<Utilisateur>[0],
									new Mock<ILookupNormalizer>().Object,
									new Mock<IdentityErrorDescriber>().Object,
									new Mock<IServiceProvider>().Object,
									new Mock<ILogger<UserManager<Utilisateur>>>().Object);

			mockDocumentsService.Setup(s => s.ObtenirDocumentsSelonId(It.IsAny<Guid>())).ReturnsAsync(new List<string> { "LETTREDEMOTIVATION", "CV" });
			mockPostulationsService.Setup(s => s.AjouterPostulation(It.IsAny<RequetePostulation>()));
			mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
         
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

			//Lorsque
			var actionResult = await postulationsController.Create(requetePostulation) as ViewResult;

			//Alors
			actionResult.Should().NotBeNull();
			var requetePostulationResult = actionResult.Model as RequetePostulation;
			requetePostulationResult.Should().Be(requetePostulation);
			mockPostulationsService.Verify(p => p.AjouterPostulation(It.IsAny<RequetePostulation>()), Times.Never);

			//Validation des erreurs dans le ModelState
			var modelState = postulationsController.ModelState;
			modelState.IsValid.Should().BeFalse();
			modelState.ErrorCount.Should().Be(1);
			modelState.Should().ContainKey("DateDisponibilite");
			modelState["DateDisponibilite"].Errors.FirstOrDefault().ErrorMessage.Should().Be($"La date de disponibilité doit être supérieure à la date du jour et inférieure à {dateLimite.ToString("yyyy-MM-dd")}");
		}
		[Fact]
		public async Task CreatePost_AvecDateDisponibiliteInfADateDuJour_Retourne_ViewResultAvecModelStateError()
		{
			//Étant donné
			Fixture fixture = new Fixture();
			var requetePostulation = fixture.Create<RequetePostulation>();
			requetePostulation.DateDisponibilite = DateTime.Now.AddDays(-1);
			requetePostulation.PretentionSalariale = 80000;
           
            fixture.RepeatCount = 3;
			List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();

            DateTime dateLimite = DateTime.Now.AddDays(45);


            Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
			Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
			Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
									new Mock<IUserStore<Utilisateur>>().Object,
									new Mock<IOptions<IdentityOptions>>().Object,
									new Mock<IPasswordHasher<Utilisateur>>().Object,
									new IUserValidator<Utilisateur>[0],
									new IPasswordValidator<Utilisateur>[0],
									new Mock<ILookupNormalizer>().Object,
									new Mock<IdentityErrorDescriber>().Object,
									new Mock<IServiceProvider>().Object,
									new Mock<ILogger<UserManager<Utilisateur>>>().Object);

			mockDocumentsService.Setup(s => s.ObtenirDocumentsSelonId(It.IsAny<Guid>())).ReturnsAsync(new List<string> { "LETTREDEMOTIVATION", "CV" });
			mockPostulationsService.Setup(s => s.AjouterPostulation(It.IsAny<RequetePostulation>()));
			mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
          
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

			//Lorsque
			var actionResult = await postulationsController.Create(requetePostulation) as ViewResult;

			//Alors
			actionResult.Should().NotBeNull();
			var requetePostulationResult = actionResult.Model as RequetePostulation;
			requetePostulationResult.Should().Be(requetePostulation);
			mockPostulationsService.Verify(p => p.AjouterPostulation(It.IsAny<RequetePostulation>()), Times.Never);

			//Validation des erreurs dans le ModelState
			var modelState = postulationsController.ModelState;
			modelState.IsValid.Should().BeFalse();
			modelState.ErrorCount.Should().Be(1);
			modelState.Should().ContainKey("DateDisponibilite");
			modelState["DateDisponibilite"].Errors.FirstOrDefault().ErrorMessage.Should().Be($"La date de disponibilité doit être supérieure à la date du jour et inférieure à {dateLimite.ToString("yyyy-MM-dd")}");
		}
       
        [Fact]
		public async Task CreatePost_AvecRequeteValide_Retourne_RedirectToAction()
		{
			//Étant donné
			Fixture fixture = new Fixture();
			var requetePostulation = fixture.Create<RequetePostulation>();
			requetePostulation.DateDisponibilite = DateTime.Now.AddDays(1);
			requetePostulation.PretentionSalariale = 80000;
          
            fixture.RepeatCount = 3;
			List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();


			Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
			Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
			Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
									new Mock<IUserStore<Utilisateur>>().Object,
									new Mock<IOptions<IdentityOptions>>().Object,
									new Mock<IPasswordHasher<Utilisateur>>().Object,
									new IUserValidator<Utilisateur>[0],
									new IPasswordValidator<Utilisateur>[0],
									new Mock<ILookupNormalizer>().Object,
									new Mock<IdentityErrorDescriber>().Object,
									new Mock<IServiceProvider>().Object,
									new Mock<ILogger<UserManager<Utilisateur>>>().Object);

			mockDocumentsService.Setup(s => s.ObtenirDocumentsSelonId(It.IsAny<Guid>())).ReturnsAsync(new List<string> { "LETTREDEMOTIVATION", "CV" });
			mockPostulationsService.Setup(s => s.AjouterPostulation(It.IsAny<RequetePostulation>()));
			mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
         
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

			//Lorsque
			var redirectToActionResult = await postulationsController.Create(requetePostulation) as RedirectToActionResult;

			//Alors
			redirectToActionResult.Should().NotBeNull();
			redirectToActionResult.ActionName.Should().Be("Index");
			mockPostulationsService.Verify(n => n.AjouterPostulation(It.IsAny<RequetePostulation>()), Times.Once);
		}

        #endregion

        #region Edit Http Get

        [Fact]
		public async Task EditGet_PostulationNull_Retourne_NotFound()
		{
			//Étant donné
			Fixture fixture = new Fixture();
			Guid id = fixture.Create<Guid>();

			Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
			Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
			Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
								new Mock<IUserStore<Utilisateur>>().Object,
								new Mock<IOptions<IdentityOptions>>().Object,
								new Mock<IPasswordHasher<Utilisateur>>().Object,
								new IUserValidator<Utilisateur>[0],
								new IPasswordValidator<Utilisateur>[0],
								new Mock<ILookupNormalizer>().Object,
								new Mock<IdentityErrorDescriber>().Object,
								new Mock<IServiceProvider>().Object,
								new Mock<ILogger<UserManager<Utilisateur>>>().Object);

			mockPostulationsService.Setup(s => s.ObtenirPostulationSelonId(It.IsAny<Guid>())).Returns(Task.FromResult<Postulation>(null));

			var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

			//Lorsque
			var actionResult = await postulationsController.Edit(id) ;

			//Alors
			actionResult.Should().BeOfType(typeof(NotFoundResult));


		}
		[Fact]
		public async Task EditGet_PostulationValide_Retourne_ViewResult()
		{
			//Étant donné
			Fixture fixture = new Fixture();
			Guid id = fixture.Create<Guid>();
			Postulation postulation = fixture.Create<Postulation>();

			Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
			Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
			Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
								new Mock<IUserStore<Utilisateur>>().Object,
								new Mock<IOptions<IdentityOptions>>().Object,
								new Mock<IPasswordHasher<Utilisateur>>().Object,
								new IUserValidator<Utilisateur>[0],
								new IPasswordValidator<Utilisateur>[0],
								new Mock<ILookupNormalizer>().Object,
								new Mock<IdentityErrorDescriber>().Object,
								new Mock<IServiceProvider>().Object,
								new Mock<ILogger<UserManager<Utilisateur>>>().Object);

			mockPostulationsService.Setup(s => s.ObtenirPostulationSelonId(It.IsAny<Guid>())).Returns(Task.FromResult<Postulation>(postulation));

			var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

			//Lorsque
			var actionResult = await postulationsController.Edit(id) as ViewResult;

			//Alors
			actionResult.Should().NotBeNull();
			var postulationResult = actionResult.Model as Postulation;
			postulationResult.Should().Be(postulation);


		}

        #endregion

        #region Edit Http Post Tests

        [Fact]
        public async Task EditPost_AvecDateDisponibiliteSupA45Jours_Retourne_ViewResultAvecModelStateError()
        {
            //Étant donné
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            var postulationAmodifier = fixture.Create<Postulation>();
            postulationAmodifier.DateDisponibilite = DateTime.Now.AddDays(1);
            postulation.DateDisponibilite = DateTime.Now.AddDays(60);
            postulation.PretentionSalariale = 80000;
          
            fixture.RepeatCount = 3;
            List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();

            DateTime dateLimite = DateTime.Now.AddDays(45);

            Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
            Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
            Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
                                    new Mock<IUserStore<Utilisateur>>().Object,
                                    new Mock<IOptions<IdentityOptions>>().Object,
                                    new Mock<IPasswordHasher<Utilisateur>>().Object,
                                    new IUserValidator<Utilisateur>[0],
                                    new IPasswordValidator<Utilisateur>[0],
                                    new Mock<ILookupNormalizer>().Object,
                                    new Mock<IdentityErrorDescriber>().Object,
                                    new Mock<IServiceProvider>().Object,
                                    new Mock<ILogger<UserManager<Utilisateur>>>().Object);

            mockPostulationsService.Setup(s => s.ObtenirPostulationSelonId(It.IsAny<Guid>())).ReturnsAsync(postulationAmodifier);
            mockPostulationsService.Setup(s => s.ModifierPostulation(It.IsAny<Postulation>()));
            mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
         
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

            //Lorsque
            var actionResult = await postulationsController.Edit(postulation) as ViewResult;

            //Alors
            actionResult.Should().NotBeNull();
            var postulationResult = actionResult.Model as Postulation;
            postulationResult.Should().Be(postulation);
            mockPostulationsService.Verify(p => p.ModifierPostulation(It.IsAny<Postulation>()), Times.Never);

            //Validation des erreurs dans le ModelState
            var modelState = postulationsController.ModelState;
            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(1);
            modelState.Should().ContainKey("DateDisponibilite");
            modelState["DateDisponibilite"].Errors.FirstOrDefault().ErrorMessage.Should().Be($"La date de disponibilité doit être supérieure à la date du jour et inférieure à {dateLimite.ToString("yyyy-MM-dd")}");
        }
        [Fact]
        public async Task EditPost_AvecDateDisponibiliteInfADateDuJour_Retourne_ViewResultAvecModelStateError()
        {
            //Étant donné
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            var postulationAmodifier = fixture.Create<Postulation>();
            postulationAmodifier.DateDisponibilite = DateTime.Now.AddDays(1);
            postulation.DateDisponibilite = DateTime.Now.AddDays(-1);
            postulation.PretentionSalariale = 80000;
           
            fixture.RepeatCount = 3;
            List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();

            DateTime dateLimite = DateTime.Now.AddDays(45);


            Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
            Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
            Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
                                    new Mock<IUserStore<Utilisateur>>().Object,
                                    new Mock<IOptions<IdentityOptions>>().Object,
                                    new Mock<IPasswordHasher<Utilisateur>>().Object,
                                    new IUserValidator<Utilisateur>[0],
                                    new IPasswordValidator<Utilisateur>[0],
                                    new Mock<ILookupNormalizer>().Object,
                                    new Mock<IdentityErrorDescriber>().Object,
                                    new Mock<IServiceProvider>().Object,
                                    new Mock<ILogger<UserManager<Utilisateur>>>().Object);

            mockPostulationsService.Setup(s => s.ObtenirPostulationSelonId(It.IsAny<Guid>())).ReturnsAsync(postulationAmodifier);
            mockPostulationsService.Setup(s => s.ModifierPostulation(It.IsAny<Postulation>()));
            mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
          
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

            //Lorsque
            var actionResult = await postulationsController.Edit(postulation) as ViewResult;

            //Alors
            actionResult.Should().NotBeNull();
            var postulationResult = actionResult.Model as Postulation;
            postulationResult.Should().Be(postulation);
            mockPostulationsService.Verify(p => p.ModifierPostulation(It.IsAny<Postulation>()), Times.Never);

            //Validation des erreurs dans le ModelState
            var modelState = postulationsController.ModelState;
            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(1);
            modelState.Should().ContainKey("DateDisponibilite");
            modelState["DateDisponibilite"].Errors.FirstOrDefault().ErrorMessage.Should().Be($"La date de disponibilité doit être supérieure à la date du jour et inférieure à {dateLimite.ToString("yyyy-MM-dd")}");
        }


        [Fact]
        public async Task EditPost_AvecDateDisponibiliteSupADateDuJourPlus5_Retourne_ViewResultAvecModelStateError()
        {
            //Étant donné
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            var postulationAmodifier = fixture.Create<Postulation>();
            postulationAmodifier.DateDisponibilite = DateTime.Now.AddDays(8);
            postulation.DateDisponibilite = DateTime.Now.AddDays(1);
            postulation.PretentionSalariale = 80000;
           
            fixture.RepeatCount = 3;
            List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();


            Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
            Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
            Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
                                    new Mock<IUserStore<Utilisateur>>().Object,
                                    new Mock<IOptions<IdentityOptions>>().Object,
                                    new Mock<IPasswordHasher<Utilisateur>>().Object,
                                    new IUserValidator<Utilisateur>[0],
                                    new IPasswordValidator<Utilisateur>[0],
                                    new Mock<ILookupNormalizer>().Object,
                                    new Mock<IdentityErrorDescriber>().Object,
                                    new Mock<IServiceProvider>().Object,
                                    new Mock<ILogger<UserManager<Utilisateur>>>().Object);

            mockPostulationsService.Setup(s => s.ObtenirPostulationSelonId(It.IsAny<Guid>())).ReturnsAsync(postulationAmodifier);
            mockPostulationsService.Setup(s => s.ModifierPostulation(It.IsAny<Postulation>()));
            mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
          
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

            //Lorsque
            var actionResult = await postulationsController.Edit(postulation) as ViewResult;

            //Alors
            actionResult.Should().NotBeNull();
            var postulationResult = actionResult.Model as Postulation;
            postulationResult.Should().Be(postulation);
            mockPostulationsService.Verify(p => p.ModifierPostulation(It.IsAny<Postulation>()), Times.Never);

            //Validation des erreurs dans le ModelState
            var modelState = postulationsController.ModelState;
            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(1);
            modelState.Should().ContainKey("DateDisponibilite");
            modelState["DateDisponibilite"].Errors.FirstOrDefault().ErrorMessage.Should().Be("Vous ne pouvez pas modifier la date de disponibilité pour cette postulation");
        }


        [Fact]
        public async Task EditPost_AvecDateDisponibiliteInfADateDuJourMoins5_Retourne_ViewResultAvecModelStateError()
        {
            //Étant donné
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            var postulationAmodifier = fixture.Create<Postulation>();
            postulationAmodifier.DateDisponibilite = DateTime.Now.AddDays(-8);
            postulation.DateDisponibilite = DateTime.Now.AddDays(1);
            postulation.PretentionSalariale = 80000;
           
            fixture.RepeatCount = 3;
            List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();


            Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
            Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
            Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
                                    new Mock<IUserStore<Utilisateur>>().Object,
                                    new Mock<IOptions<IdentityOptions>>().Object,
                                    new Mock<IPasswordHasher<Utilisateur>>().Object,
                                    new IUserValidator<Utilisateur>[0],
                                    new IPasswordValidator<Utilisateur>[0],
                                    new Mock<ILookupNormalizer>().Object,
                                    new Mock<IdentityErrorDescriber>().Object,
                                    new Mock<IServiceProvider>().Object,
                                    new Mock<ILogger<UserManager<Utilisateur>>>().Object);

            mockPostulationsService.Setup(s => s.ObtenirPostulationSelonId(It.IsAny<Guid>())).ReturnsAsync(postulationAmodifier);
            mockPostulationsService.Setup(s => s.ModifierPostulation(It.IsAny<Postulation>()));
            mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
          
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

            //Lorsque
            var actionResult = await postulationsController.Edit(postulation) as ViewResult;

            //Alors
            actionResult.Should().NotBeNull();
            var postulationResult = actionResult.Model as Postulation;
            postulationResult.Should().Be(postulation);
            mockPostulationsService.Verify(p => p.ModifierPostulation(It.IsAny<Postulation>()), Times.Never);

            //Validation des erreurs dans le ModelState
            var modelState = postulationsController.ModelState;
            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(1);
            modelState.Should().ContainKey("DateDisponibilite");
            modelState["DateDisponibilite"].Errors.FirstOrDefault().ErrorMessage.Should().Be("Vous ne pouvez pas modifier la date de disponibilité pour cette postulation");
        }

        [Fact]
        public async Task EditPost_AvecPretentionSalarialesSupA150000_Retourne_ViewResultAvecModelStateError()
        {
            //Étant donné
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            var postulationAmodifier = fixture.Create<Postulation>();
            postulationAmodifier.DateDisponibilite = DateTime.Now.AddDays(1);
            postulation.DateDisponibilite = DateTime.Now.AddDays(1);
            postulation.PretentionSalariale = 180000;
           
            fixture.RepeatCount = 3;
            List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();


            Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
            Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
            Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
                                    new Mock<IUserStore<Utilisateur>>().Object,
                                    new Mock<IOptions<IdentityOptions>>().Object,
                                    new Mock<IPasswordHasher<Utilisateur>>().Object,
                                    new IUserValidator<Utilisateur>[0],
                                    new IPasswordValidator<Utilisateur>[0],
                                    new Mock<ILookupNormalizer>().Object,
                                    new Mock<IdentityErrorDescriber>().Object,
                                    new Mock<IServiceProvider>().Object,
                                    new Mock<ILogger<UserManager<Utilisateur>>>().Object);

            mockPostulationsService.Setup(s => s.ObtenirPostulationSelonId(It.IsAny<Guid>())).ReturnsAsync(postulationAmodifier);
            mockPostulationsService.Setup(s => s.ModifierPostulation(It.IsAny<Postulation>()));
            mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
       
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

            //Lorsque
            var actionResult = await postulationsController.Edit(postulation) as ViewResult;

            //Alors
            actionResult.Should().NotBeNull();
            var postulationResult = actionResult.Model as Postulation;
            postulationResult.Should().Be(postulation);
            mockPostulationsService.Verify(p => p.ModifierPostulation(It.IsAny<Postulation>()), Times.Never);

            //Validation des erreurs dans le ModelState
            var modelState = postulationsController.ModelState;
            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(1);
            modelState.Should().ContainKey("PretentionSalariale");
            modelState["PretentionSalariale"].Errors.FirstOrDefault().ErrorMessage.Should().Be("Votre prétention salariale est au-delà de nos limites");
        }

        [Fact]
        public async Task EditPost_AvecPretentionSalarialesNegative_Retourne_ViewResultAvecModelStateError()
        {
            //Étant donné
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            var postulationAmodifier = fixture.Create<Postulation>();
            postulationAmodifier.DateDisponibilite = DateTime.Now.AddDays(1);
            postulation.DateDisponibilite = DateTime.Now.AddDays(1);
            postulation.PretentionSalariale = -180000;
            
            fixture.RepeatCount = 3;
            List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();


            Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
            Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
            Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
                                    new Mock<IUserStore<Utilisateur>>().Object,
                                    new Mock<IOptions<IdentityOptions>>().Object,
                                    new Mock<IPasswordHasher<Utilisateur>>().Object,
                                    new IUserValidator<Utilisateur>[0],
                                    new IPasswordValidator<Utilisateur>[0],
                                    new Mock<ILookupNormalizer>().Object,
                                    new Mock<IdentityErrorDescriber>().Object,
                                    new Mock<IServiceProvider>().Object,
                                    new Mock<ILogger<UserManager<Utilisateur>>>().Object);

            mockPostulationsService.Setup(s => s.ObtenirPostulationSelonId(It.IsAny<Guid>())).ReturnsAsync(postulationAmodifier);
            mockPostulationsService.Setup(s => s.ModifierPostulation(It.IsAny<Postulation>()));
            mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
           
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

            //Lorsque
            var actionResult = await postulationsController.Edit(postulation) as ViewResult;

            //Alors
            actionResult.Should().NotBeNull();
            var postulationResult = actionResult.Model as Postulation;
            postulationResult.Should().Be(postulation);
            mockPostulationsService.Verify(p => p.ModifierPostulation(It.IsAny<Postulation>()), Times.Never);

            //Validation des erreurs dans le ModelState
            var modelState = postulationsController.ModelState;
            modelState.IsValid.Should().BeFalse();
            modelState.ErrorCount.Should().Be(1);
            modelState.Should().ContainKey("PretentionSalariale");
            modelState["PretentionSalariale"].Errors.FirstOrDefault().ErrorMessage.Should().Be("Veuillez saisir une valeur valide pour le salaire");
        }
        [Fact]
        public async Task EditPost_AvecPostulationValide_Retourne_RedirectToAction()
        {
            //Étant donné
            Fixture fixture = new Fixture();
            var postulation = fixture.Create<Postulation>();
            var postulationAmodifier = fixture.Create<Postulation>();
            postulationAmodifier.DateDisponibilite = DateTime.Now.AddDays(1);
            postulation.DateDisponibilite = DateTime.Now.AddDays(1);
            postulation.PretentionSalariale = 80000;
           
            fixture.RepeatCount = 3;
            List<Postulation> postulations = fixture.CreateMany<Postulation>().ToList();


            Mock<IPostulationsService> mockPostulationsService = new Mock<IPostulationsService>();
            Mock<IDocumentsService> mockDocumentsService = new Mock<IDocumentsService>();
            Mock<IOffresEmploiService> mockEmploisService = new Mock<IOffresEmploiService>();
            Mock<INotesService> mockNotesService = new Mock<INotesService>();
            Mock<UserManager<Utilisateur>> mockUserManager = new Mock<UserManager<Utilisateur>>(
                                    new Mock<IUserStore<Utilisateur>>().Object,
                                    new Mock<IOptions<IdentityOptions>>().Object,
                                    new Mock<IPasswordHasher<Utilisateur>>().Object,
                                    new IUserValidator<Utilisateur>[0],
                                    new IPasswordValidator<Utilisateur>[0],
                                    new Mock<ILookupNormalizer>().Object,
                                    new Mock<IdentityErrorDescriber>().Object,
                                    new Mock<IServiceProvider>().Object,
                                    new Mock<ILogger<UserManager<Utilisateur>>>().Object);

            mockPostulationsService.Setup(s => s.ObtenirPostulationSelonId(It.IsAny<Guid>())).ReturnsAsync(postulationAmodifier);
            mockPostulationsService.Setup(s => s.ModifierPostulation(It.IsAny<Postulation>()));
            mockPostulationsService.Setup(s => s.ObtenirPostulations()).Returns(Task.FromResult(postulations));
           
            var postulationsController = new PostulationsController(mockPostulationsService.Object, mockDocumentsService.Object, mockEmploisService.Object, mockUserManager.Object, mockNotesService.Object);

            //Lorsque
            var redirectToActionResult = await postulationsController.Edit(postulation) as RedirectToActionResult;

            //Alors
            redirectToActionResult.Should().NotBeNull();
            redirectToActionResult.ActionName.Should().Be("Index");
            mockPostulationsService.Verify(n => n.ModifierPostulation(It.IsAny<Postulation>()), Times.Once);
        }

        #endregion
    }
}
