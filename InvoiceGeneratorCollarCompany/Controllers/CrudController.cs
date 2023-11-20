using Microsoft.AspNetCore.Mvc;
using InvoiceGeneratorCollarCompany.Repositories;

namespace InvoiceGeneratorCollarCompany.Controllers
{
    public class CrudController : Controller
    {
        private readonly ILogger<CrudController> _logger;
        private readonly ICrudRepository _crudRepository;



        public CrudController(ILogger<CrudController> logger, ICrudRepository crudrepository)
        {
            _logger = logger;
            _crudRepository = crudrepository;
        }
    }
}
