using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Models;
using System;
using System.Diagnostics;

namespace ModernRecrut.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CodeStatus(int code)
        {
            CodeStatusViewModel codeStatusViewModel = new CodeStatusViewModel();

            codeStatusViewModel.IdRequete = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            codeStatusViewModel.CodeStatus = code;

            if (code == 404)
            {
                codeStatusViewModel.MessageErreur = "Oh, non. C'est très gênant. Nous n'avons pas trouvé la page que vous cherchiez... \r\nCependant, voici quelques chatons pour vous motiver dans votre nouvelle recherche !";
            }
            else if (code == 500)
            {
                codeStatusViewModel.MessageErreur = "Oopsies ! Il semblerait que nous soyons en cours de maintenance. Revenez plus tard !";
            }
            else
            {
                codeStatusViewModel.MessageErreur = "Nous ne savons pas exactement ce qui s'est passé... Mais une erreur s'est produite lors de votre demande. Voici quelques chatons pour compenser ! !";
            }

            return View(codeStatusViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

   
    }
}