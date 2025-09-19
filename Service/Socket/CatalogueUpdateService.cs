using Microsoft.AspNetCore.SignalR;
using XeniaCatalogueApi.Dictionary;
//using XeniaCatalogueApi.Hubs;
//using XeniaCatalogueApi.Repositories.CartRepository;
//using XeniaCatalogueApi.Repositories.CategoryRepository;
//using XeniaCatalogueApi.Repositories.OrderRepository;
//using XeniaCatalogueApi.Repositories.ProductRepository;
//using XeniaCatalogueApi.Repositories.SpecialCategoryRepository;

namespace XeniaRentalApi.Service.Socket
{
    public class CatalogueUpdateService 
    {
        //private readonly ISpecialCategoryRepository _specialCategoryRepository;
        //private readonly ICategoryRepository _categoryRepository;
        //private readonly IProductRepository _productRepository;
        //private readonly ICartRepository _cartRepository;
        //private readonly IOrderRepository _orderRepository;
        //private readonly IHubContext<CatalogueHub> _hubContext;

//        public CatalogueUpdateService(
//            ISpecialCategoryRepository specialCategoryRepository,
//            ICategoryRepository categoryRepository,
//            IProductRepository productRepository,
//            ICartRepository cartRepository,
//            IOrderRepository orderRepository,
//            IHubContext<CatalogueHub> hubContext)
//        {
//            _specialCategoryRepository = specialCategoryRepository;
//            _categoryRepository = categoryRepository;
//            _productRepository = productRepository;
//            _cartRepository = cartRepository;
//            _orderRepository = orderRepository;
//            _hubContext = hubContext;
//        }

//        public async Task SendCatalogueUpdate(string type, int companyId, int branchId, int? customerId = null, int? id = null, int? pageNumber = null, int? pageSize = null, string filter = "all", DateTime? startDate = null, DateTime? endDate = null, string? search = null, string? connectionId = null)
//        {
//            object? data = null;

//            if (type.Equals(SupportedTypes.SPECIAL_CATEGORY, StringComparison.OrdinalIgnoreCase))
//            {
//                var specialCategories = await _specialCategoryRepository.GetSpecialCategory(companyId, branchId, customerId);
//                var categories = await _categoryRepository.GetHomeCategories(companyId);

//                data = new
//                {
//                    SpecialCategories = specialCategories,
//                    Categories = categories
//                };
//            }
//            else if (type.Equals(SupportedTypes.CATEGORY, StringComparison.OrdinalIgnoreCase))
//            {
//                var categories = await _categoryRepository.GetCategories(companyId);
//                data = new { Categories = categories };
//            }
//            else if (type.Equals(SupportedTypes.PRODUCT, StringComparison.OrdinalIgnoreCase))
//            {
//                   int page = pageNumber ?? 1;
//                    int size = pageSize ?? 10;

//                    var products = await _productRepository.GetProductsByCategoryId(companyId, branchId, id, customerId, page, size, search);
//                    data = new { Products = products };
//            }
//            else if (type.Equals(SupportedTypes.OFFER_PRODUCT, StringComparison.OrdinalIgnoreCase))
//            {
//                if (id.HasValue)
//                {
//                    int page = pageNumber ?? 1;
//                    int size = pageSize ?? 10;
//                    var products = await _productRepository.GetProductsByOfferId(companyId, branchId, id.Value, customerId, page, size);
//                    data = new { Product = products };
//                }
//                else data = new { Error = "ProductId is required for OfferProduct type" };
//            }
//            else if (type.Equals(SupportedTypes.PRODUCT_DETAILS, StringComparison.OrdinalIgnoreCase))
//            {
//                if (id.HasValue)
//                {
//                    var products = await _productRepository.GetProductsByProductId(companyId, branchId, customerId, id.Value);
//                    data = new { Product = products };
//                }
//                else data = new { Error = "ProductId is required for ProductDetails type" };
//            }
//            else if (type.Equals(SupportedTypes.CARTDETAILS, StringComparison.OrdinalIgnoreCase))
//            {
//                if (customerId.HasValue)
//                {
//                    var products = await _cartRepository.GetCustomerCart(companyId, branchId, customerId.Value);
//                    data = new { Product = products };
//                }
//                else data = new { Error = "CustomerId is required" };
//            }
//            else if (type.Equals(SupportedTypes.DASHBOARD_ORDER, StringComparison.OrdinalIgnoreCase))
//            {

//                var orders = await _orderRepository.GetOrderSummary(companyId, filter, startDate, endDate);
//                data = new { Orders = orders };

//            }
//            else if (type.Equals(SupportedTypes.ORDER, StringComparison.OrdinalIgnoreCase))
//            {

//                var orders = await _orderRepository.GetOrder(companyId,branchId,filter);
//                data = new { Orders = orders };

//            }
//            else if (type.Equals(SupportedTypes.DELIVERYORDER, StringComparison.OrdinalIgnoreCase))
//            {
//                if (id.HasValue)
//                {
//                    var orders = await _orderRepository.GetDeliveryOrder(companyId, id.Value);
//                    data = new { Orders = orders };
//                }
//                else data = new { Error = "EmployeeId is required" };
//            }
//            else if (type.Equals(SupportedTypes.PICKUPORDER, StringComparison.OrdinalIgnoreCase))
//            {
//                if (id.HasValue)
//                {
//                    var orders = await _orderRepository.GetReturnOrder(companyId, id.Value);
//                    data = new { Orders = orders };
//                }
//                else data = new { Error = "EmployeeId is required" };
//            }

//            if (data != null)
//{
//    if (type.Equals(SupportedTypes.CARTDETAILS, StringComparison.OrdinalIgnoreCase)
//        || type.Equals(SupportedTypes.PRODUCT, StringComparison.OrdinalIgnoreCase)
//        || type.Equals(SupportedTypes.PRODUCT_DETAILS, StringComparison.OrdinalIgnoreCase)
//        || type.Equals(SupportedTypes.OFFER_PRODUCT, StringComparison.OrdinalIgnoreCase))
//                {
   
//        await _hubContext.Clients.Client(connectionId)
//            .SendAsync("ReceiveCatalogueUpdate", type, data);
//    }
//    else
//    {
//        await _hubContext.Clients.Group($"company-{companyId}")
//            .SendAsync("ReceiveCatalogueUpdate", type, data);
//    }
//}


        
    }

}
