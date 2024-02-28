using mandarinProject1.Data;
using mandarinProject1.Data.Entities;
using mandarinProject1.Models;
using mandarinProject1.Services;
using mandarinProject1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace mandarinProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMandarinRepository _rps;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMailService _mail;

        public HomeController(ILogger<HomeController> logger, IMandarinRepository rps, UserManager<IdentityUser> userManager, IMailService mail)
        {
            _logger = logger;
            _rps = rps;
            _userManager = userManager;
            _mail = mail;
        }
        //стартовая страница
        public IActionResult Index()
        {
            //получение всех мандаринов
            IEnumerable<Mandarin> mandarins = _rps.GetAllMandarins();
            return View(mandarins);
        }

        [HttpGet("bid")]
        [Authorize]
        //ставка
        public IActionResult Bid(int id)
        {

            BidViewModel model = new BidViewModel
            {//передача идентификатора
                Id = id
            };


            return View(model);
        }



        [HttpPost("bid")]
        public async Task<IActionResult> BidAsync(BidViewModel model)
        {
            //получение мандарина
            var man = _rps.GetmandarinById(model.Id);
            if (ModelState.IsValid)
            {
                //если ставка меньше возвращаем ошибку
                if (model.CurrentPrize <= man.CurrentPrize)
                {

                    ModelState.AddModelError(string.Empty, "Ставка должна быть выше");

                }
                else
                {
                    //если ставка больше 1000000, лот выкупается
                    if (model.CurrentPrize >= 1000000)
                    {
                        man.CurrentPrize = model.CurrentPrize;
                        man.Bought = true;

                        man.User = await _userManager.GetUserAsync(User);
                        //отправка сообщения
                        _mail.SendPurchaseMessage(man.User.Email, man.Id, man.CurrentPrize);



                    }
                    else
                    {
                        // повышение ставки
                        man.CurrentPrize = model.CurrentPrize;

                        man.User = await _userManager.GetUserAsync(User);
                        _rps.Update(man);
                        _rps.SaveAll();
                        //отправка уведомления пользователю в случае повышения ставки
                        _mail.SendNotificationMessage(man.User.Email, man.Id, man.CurrentPrize);








                    }


                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpGet("buy")]
        [Authorize]
        public async Task<IActionResult> BuyAsync(int id)
        {//выкуп лота
            var man = _rps.GetmandarinById(id);

            man.CurrentPrize = 1000000;
            man.Bought = true;
            man.User = await _userManager.GetUserAsync(User);
            _mail.SendPurchaseMessage(man.User.Email, man.Id, man.CurrentPrize);
            _rps.Update(man);
            _rps.SaveAll();
            var man1 = _rps.GetmandarinById(id);
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}