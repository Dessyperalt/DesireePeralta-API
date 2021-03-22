using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesireePeraltaAPI.Models;
using DesireePeraltaAPI.Controllers;


namespace DesireePeraltaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyPurchasesController : ControllerBase
    {
        private readonly CurrencyPurchaseContext _context;

        private ExchangeRatesController _currencyController;

        private readonly ExchangeRateContext _exchangeRateContext;

        public CurrencyPurchasesController(CurrencyPurchaseContext context,ExchangeRatesController currencyController, ExchangeRateContext exchangeRateContext)
        {
            _context = context;

            _currencyController = currencyController;

            _exchangeRateContext = exchangeRateContext;
        }        

        // GET: api/CurrencyPurchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyPurchase>>> GetCurrencyPurshaseItems()
        {
            return await _context.CurrencyPurshaseItems.ToListAsync();
        }

        // GET: api/CurrencyPurchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyPurchase>> GetCurrencyPurchase(int id)
        {
            var currencyPurchase = await _context.CurrencyPurshaseItems.FindAsync(id);

            if (currencyPurchase == null)
            {
                return NotFound();
            }

            return currencyPurchase;
        }

        // PUT: api/CurrencyPurchases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurrencyPurchase(int id, CurrencyPurchase currencyPurchase)
        {
            if (id != currencyPurchase.ID)
            {
                return BadRequest();
            }

            _context.Entry(currencyPurchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyPurchaseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CurrencyPurchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CurrencyPurchase>> PostCurrencyPurchase(CurrencyPurchase currencyPurchase)
        {
            var rate = _currencyController.Currency(currencyPurchase.currencyCode).Result;

            List<string> value = ((Microsoft.AspNetCore.Mvc.ObjectResult)rate.Result).Value.ToString().Split(',').ToList();

            if (value[0].Contains("not available"))
            {
                return BadRequest("This currency is not available.");
            }

            if (currencyPurchase.exchangeRate <= 0)
            {
                var value1 = String.Concat(value[0].Where(x => x == '.' || Char.IsDigit(x)));

                currencyPurchase.exchangeRate = Decimal.Parse(value1); 
            }

            currencyPurchase.total = Math.Round(currencyPurchase.amount / currencyPurchase.exchangeRate, 2);

            currencyPurchase.date = DateTime.UtcNow;

            var limitCurrencyUser = from m in _context.CurrencyPurshaseItems
                               select m;

            limitCurrencyUser = limitCurrencyUser.Where(s => s.userID.Equals(currencyPurchase.userID) & s.date.Month.Equals(currencyPurchase.date.Month) & s.currencyCode.Contains(currencyPurchase.currencyCode));

            decimal totalCurrencyMonth = 0;

            if (limitCurrencyUser?.Any() == true)
            {
                totalCurrencyMonth = limitCurrencyUser.Sum(s => s.total);
            }

            var limitCurrency = from n in _exchangeRateContext.Currency
                            select n;

            limitCurrency = limitCurrency.Where(t => t.ISOCode.Contains(currencyPurchase.currencyCode));

            decimal currencyl = limitCurrency.First().Limit;

            if (currencyl > 0)
            {
                if ((currencyPurchase.total + totalCurrencyMonth) > currencyl)
                {
                    return BadRequest("You can't buy more than " + currencyl + " " + currencyPurchase.currencyCode + ", you already bought " 
                        + totalCurrencyMonth + " " + currencyPurchase.currencyCode + " this month.");
                }
            }

            _context.CurrencyPurshaseItems.Add(currencyPurchase);

            await _context.SaveChangesAsync();

            CreatedAtAction("GetCurrencyPurchase", new { id = currencyPurchase.ID }, currencyPurchase);

            return Ok("You purchased " + currencyPurchase.total + " " + currencyPurchase.currencyCode);
        }

        // DELETE: api/CurrencyPurchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrencyPurchase(int id)
        {
            var currencyPurchase = await _context.CurrencyPurshaseItems.FindAsync(id);
            if (currencyPurchase == null)
            {
                return NotFound();
            }

            _context.CurrencyPurshaseItems.Remove(currencyPurchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CurrencyPurchaseExists(int id)
        {
            return _context.CurrencyPurshaseItems.Any(e => e.ID == id);
        }
    }
}
