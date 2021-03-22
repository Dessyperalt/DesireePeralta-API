using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using DesireePeraltaAPI.Models;

namespace DesireePeraltaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly ExchangeRateContext _context;

        public ExchangeRatesController(ExchangeRateContext context)
        {
            _context = context;
        }

        [HttpGet("[action]/{Currency}")]
        public async Task<ActionResult<ExchangeRate>> Currency(string currency)
        {
            using (var client = new HttpClient())
            {
                try
                {
                     var currencyrate = from m in _context.Currency
                                  select m;

                     if (!String.IsNullOrEmpty(currency))
                     {
                         currencyrate = currencyrate.Where(s => s.ISOCode.Contains(currency));
                     }              

                     if (currencyrate?.Any() == false)
                     {
                         return NotFound($"This currency is not available.");
                     }                    
                        
                    var currencydescription = currencyrate.First().Description.Trim();

                    if (currency == "BRL")
                    {
                        currencydescription = "Dolar";                    
                    }

                    client.BaseAddress = new Uri("https://www.bancoprovincia.com.ar");

                    var response = await client.GetAsync($"/Principal/{currencydescription}");

                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();

                    if (currency == "BRL")
                    {
                        List<string> value = stringResult.Split(',').ToList();

                        double buyReal = Double.Parse(String.Concat(value[0].Where(x => x == '.' || Char.IsDigit(x)))) / 4;

                        double sellReal = Double.Parse(String.Concat(value[1].Where(x => x == '.' || Char.IsDigit(x)))) / 4;

                        stringResult = "[\"" + buyReal + "\",\"" + sellReal + "\"," + value[2];
                    }

                    return Ok(stringResult);

                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting ExchangeRate from BancoProvincia: {httpRequestException.Message}");
                }
            }
        }
    }
}
