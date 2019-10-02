using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MonitorMyAPI.Models;

namespace MonitorMyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly TelemetryClient _telemetry;
        private readonly AdventureWorksLTContext _context;

        public CustomersController(ILogger<CustomersController> logger, TelemetryClient telemetry, AdventureWorksLTContext context)
        {
            _logger = logger;
            _telemetry = telemetry;
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IList<CustomerLookup>>> GetCustomers()
        {
            _telemetry.TrackEvent("RetrieveCustomers");

            IList<CustomerLookup> customers = null ;

            try
            {
                customers = await _context.Customer.Take(20).Select(e =>
                    new CustomerLookup
                    {
                        CustomerId = e.CustomerId,
                        Title = e.Title,
                        FirstName = e.FirstName,
                        LastName = e.LastName
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR: Unhandled exception occured in the CustomersController.GetCustomers method.");
            }

            return Ok(customers);
        }

        //// GET: api/Customers/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Customer>> GetCustomer(int id)
        //{
        //    var customer = await _context.Customer.FindAsync(id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return customer;
        //}

        //// PUT: api/Customers/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCustomer(int id, Customer customer)
        //{
        //    if (id != customer.CustomerId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(customer).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CustomerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Customers
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        //{
        //    _context.Customer.Add(customer);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        //}

        //// DELETE: api/Customers/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        //{
        //    var customer = await _context.Customer.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Customer.Remove(customer);
        //    await _context.SaveChangesAsync();

        //    return customer;
        //}

        //private bool CustomerExists(int id)
        //{
        //    return _context.Customer.Any(e => e.CustomerId == id);
        //}
    }
}
