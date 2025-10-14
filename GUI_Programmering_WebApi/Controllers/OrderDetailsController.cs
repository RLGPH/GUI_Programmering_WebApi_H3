using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GUI_Programmering_WebApi.Models;

namespace GUI_Programmering_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly GuiWebApiDatabaseContext _context;

        public OrderDetailsController(GuiWebApiDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> GetOrderDetails()
        {
            var details = await _context.OrderDetails
                .Select(od => new OrderDetailDTO
                {
                    OrderDetailId = od.OrderDetailId,
                    OrderId = od.OrderId,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                })
                .ToListAsync();

            return Ok(details);
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailDTO>> GetOrderDetail(int id)
        {
            var od = await _context.OrderDetails.FindAsync(id);
            if (od == null)
                return NotFound();

            var dto = new OrderDetailDTO
            {
                OrderDetailId = od.OrderDetailId,
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            };

            return Ok(dto);
        }

        // PUT: api/OrderDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetailDTO OrderDeDto)
        {
            if (id != OrderDeDto.OrderDetailId)
                return BadRequest();

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
                return NotFound();

            orderDetail.ProductId = OrderDeDto.ProductId;
            orderDetail.OrderId = OrderDeDto.OrderId;
            orderDetail.Quantity = OrderDeDto.Quantity;
            orderDetail.UnitPrice = OrderDeDto.UnitPrice;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/OrderDetails
        [HttpPost]
        public async Task<ActionResult<OrderDetailDTO>> PostOrderDetail(OrderDetailDTO dto)
        {
            var od = new OrderDetail
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };

            _context.OrderDetails.Add(od);
            await _context.SaveChangesAsync();

            dto.OrderDetailId = od.OrderDetailId;
            return CreatedAtAction(nameof(GetOrderDetail), new { id = dto.OrderDetailId }, dto);
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var od = await _context.OrderDetails.FindAsync(id);
            if (od == null)
                return NotFound();

            _context.OrderDetails.Remove(od);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderDetailId == id);
        }
    }
}
