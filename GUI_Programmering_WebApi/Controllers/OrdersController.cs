using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GUI_Programmering_WebApi.Models;

namespace GUI_Programmering_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly GuiWebApiDatabaseContext _context;

        public OrdersController(GuiWebApiDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _context.Orders
                .Select(o => new OrderDTO
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    CustomerId = o.CustomerId
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            var dto = new OrderDTO
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId
            };

            return Ok(dto);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderDTO OrderDto)
        {
            if (id != OrderDto.OrderId)
                return BadRequest();

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            order.OrderDate = OrderDto.OrderDate;
            order.CustomerId = OrderDto.CustomerId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> PostOrder(OrderDTO dto)
        {
            var order = new Order
            {
                OrderDate = dto.OrderDate,
                CustomerId = dto.CustomerId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            dto.OrderId = order.OrderId;
            return CreatedAtAction(nameof(GetOrder), new { id = dto.OrderId }, dto);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
