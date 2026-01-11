using GiftRide.Core.Contracts;
using GiftRide.Models.Statistic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GiftRide.Controllers
{
    public class StatisticController : Controller
    {
        private readonly IStatisticService statisticService;
        public StatisticController(IStatisticService statisticService)
        {
            this.statisticService = statisticService;
        }


        // GET: StatisticController
        public IActionResult Index()
        {
            StatisticVM statistics = new StatisticVM();

            statistics.CountClients = statisticService.CountClients();
            statistics.CountProducts = statisticService.CountProducts();
            statistics.CountOrders = statisticService.CountOrders();
            statistics.SumOrders = statisticService.SumOrders();
            return View(statistics);
        }

        // GET: StatisticController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StatisticController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StatisticController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatisticController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatisticController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatisticController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatisticController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}