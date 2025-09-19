using Microsoft.AspNetCore.SignalR;
using XeniaRentalApi.Service.Socket;

namespace XeniaRentalApi.Hubs
{
    public class CatalogueHub : Hub
    {
        //private readonly ICatalogueUpdateService _catalogueUpdateService;

        //public CatalogueHub(ICatalogueUpdateService catalogueUpdateService)
        //{
        //    _catalogueUpdateService = catalogueUpdateService;
        //}

        //public async Task JoinCompanyGroup(int companyId)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, $"company-{companyId}");
        //}

        //public async Task LeaveCompanyGroup(int companyId)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"company-{companyId}");
        //}

        //public async Task SendCatalogueUpdate(string type, int companyId, int branchId, int? customerId = null, int? id = null, int? pageNumber = null, int? pageSize = null, string filter = "all", DateTime? startDate = null, DateTime? endDate = null, string? search = null)
        //{
        //    try
        //    {
        //        await _catalogueUpdateService.SendCatalogueUpdate(type, companyId, branchId, customerId, id, pageNumber, pageSize, filter, startDate, endDate,search, connectionId: Context.ConnectionId);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("=== SignalR SendCatalogueUpdate Error ===");
        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.StackTrace);
        //        throw; 
        //    }
        //}

    }
}
