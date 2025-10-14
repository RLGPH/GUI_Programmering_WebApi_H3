using GUI_Programmering_WebApi.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI_Programmering_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly GuiWebApiDatabaseContext _context;

        public CustomersController(GuiWebApiDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var Customers = await _context.Customers
                .Select( c => new CustomerDTO
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Phone = c.Phone
                }
                ).ToListAsync();

            return Ok(Customers);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var dto = new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone
            };

            return Ok(dto);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerDTO CustDto)
        {
            if (id != CustDto.CustomerId)
                return BadRequest();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            customer.FirstName = CustDto.FirstName;
            customer.LastName = CustDto.LastName;
            customer.Email = CustDto.Email;
            customer.Phone = CustDto.Phone;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerDTO CustDto)
        {
            var customer = new Customer
            {
                FirstName = CustDto.FirstName,
                LastName = CustDto.LastName,
                Email = CustDto.Email,
                Phone = CustDto.Phone,
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // map back to DTO
            var result = new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                FirstName = CustDto.FirstName,
                LastName = CustDto.LastName,
                Email = CustDto.Email,
                Phone = CustDto.Phone,
            };

            return CreatedAtAction(nameof(GetCustomer), new { id = result.CustomerId }, result);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
