using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestApi.Models;
using System.Collections.Generic;
using System;
using RestApi.Lib;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(AppDbContext context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            try
            {
                var customerList = await _context.Customers.ToListAsync();
                foreach (var customer in customerList)
                {
                    customer.Email = EncryptionService.Decrypt(customer.Email);
                    customer.PhoneNumber = EncryptionService.Decrypt(customer.PhoneNumber);
                }
                return Ok(customerList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customers");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer(int id)
        {
            try
            {
                var customer = _context.Customers.Find(id);
                if (customer == null)
                    return NotFound();

                customer.Email = EncryptionService.Decrypt(customer.Email);
                customer.PhoneNumber = EncryptionService.Decrypt(customer.PhoneNumber);
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching customer with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public ActionResult<Customer> CreateCustomer(Customer customer)
        {
            try
            {
                customer.Email = EncryptionService.Encrypt(customer.Email);
                customer.PhoneNumber = EncryptionService.Encrypt(customer.PhoneNumber);
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
                return BadRequest();

            customer.Email = EncryptionService.Encrypt(customer.Email);
            customer.PhoneNumber = EncryptionService.Encrypt(customer.PhoneNumber);
            _context.Entry(customer).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Customers.Any(c => c.Id == id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating customer with id {id}");
                return StatusCode(500, "Internal server error");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Customer> DeleteCustomer(int id)
        {
            try
            {
                var customer = _context.Customers.Find(id);
                if (customer == null)
                    return NotFound();

                _context.Customers.Remove(customer);
                _context.SaveChanges();

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting customer with id {id}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
