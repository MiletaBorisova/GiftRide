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

        
    }
}