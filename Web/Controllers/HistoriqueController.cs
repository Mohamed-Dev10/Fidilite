using BRICOMA.ECOMMERCE.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BRICOMA.ECOMMERCE.Web.Controllers
{
    [Authorize]
    public class HistoriqueController : Controller
    {
        private readonly IClienteBOService _clienteBOService;

        public HistoriqueController(IClienteBOService clienteBOService)
        {
            _clienteBOService = clienteBOService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Data(int page = 1, int pageSize = 10)
        {
            var result = await _clienteBOService.GetAuditLogs(page, pageSize);
            var data = result.Data;
            return Json(new
            {
                items = data?.Items?.Select(a => new
                {
                    a.Id,
                    a.UserName,
                    a.Operation,
                    a.EntityType,
                    a.EntityCode,
                    a.Detail,
                    DateOperation = a.DateOperation.ToString("dd/MM/yyyy HH:mm")
                }) ?? Enumerable.Empty<object>(),
                totalCount = data?.TotalCount ?? 0,
                page = data?.Page ?? 1,
                pageSize = data?.PageSize ?? pageSize,
                totalPages = data?.TotalPages ?? 0
            });
        }
    }
}
