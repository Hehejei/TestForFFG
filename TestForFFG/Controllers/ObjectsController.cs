using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestForFFG.Models;
using System.Linq;

namespace TestForFFG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectsController : ControllerBase
    {
        private readonly DbCntx _context;

        private void openConnection()
        {
            _context.Database.OpenConnection();
            _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.objects ON;");
        }

        private void closeConnection()
        {
            _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.objects OFF;");
            _context.Database.CloseConnection();
        }

        public ObjectsController(DbCntx context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetObjects")]
        public async Task<ActionResult<IEnumerable<Obj>>> GetObjects()
        {
            return await _context.objects.ToListAsync();
        }

        [HttpPost]
        [Route("InsertObjects")]
        public async Task<ActionResult> InsertObjects([FromQuery] IEnumerable<ObjectView> objectsView)
        {
            _context.objects.RemoveRange(_context.objects);

            var numbers = from objView in objectsView
                       orderby objView.Code ascending
                       select objView;

            int i = 0;

            foreach (var objView in numbers)
            {
                i++;

                await _context.objects.AddAsync(new Obj
                {
                    Id = i,
                    Code = objView.Code,
                    Value = objView.Value,
                });

            }

            openConnection();

            await _context.SaveChangesAsync();

            closeConnection();


            return StatusCode(201);
        }

    }
}
