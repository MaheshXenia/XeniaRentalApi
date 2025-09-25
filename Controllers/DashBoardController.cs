using Microsoft.AspNetCore.Mvc;
using XeniaRentalApi.Repositories.Dashboard;

namespace XeniaRentalApi.Controllers
{
    public class DashBoardController : Controller
    {
        private readonly IDashboardRepsitory _dashboardRepsitory;


        public DashBoardController(IDashboardRepsitory dashboardRepsitory)
        {
            _dashboardRepsitory = dashboardRepsitory;
        }


        [HttpGet("dashboard/rent")]
        public async Task<IActionResult> GetRentDashboard(DateTime fromDate, DateTime toDate)
        {
            var data = await _dashboardRepsitory.GetRentDashboardAsync(fromDate, toDate);
            return Ok(data);
        }

        [HttpGet("rent/monthly")]
        public async Task<IActionResult> GetMonthlyRevenue([FromQuery] int year)
        {
            var revenue = await _dashboardRepsitory.GetMonthlyRentRevenueAsync(year);
            return Ok(revenue);
        }

    }
}
