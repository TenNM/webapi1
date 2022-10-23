using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : Controller
    {
        HttpClient httpClient = new HttpClient();
        string uri = "https://www.cbr-xml-daily.ru/daily_json.js";

        //----------------------------------------------------------------------

        /// <summary>
        /// All data from JSON
        /// </summary>
        /// <returns>"Currencies" class</returns>
        [HttpGet("allData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllData()
        {
            var c = await GetJSON_Async(uri, httpClient);

            if (c != null) return Ok(c);
            return NotFound("External API error");

        }//all

        /// <summary>
        /// All currencies or one currency depending on parameter availability
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>Dictionary or one record</returns>
        [HttpGet]
        [Route("currencies")]
        [Route("currencies/{pos?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrencies(int? pos = null)
        {
            var c = await GetJSON_Async(uri, httpClient);

            if (null == c) return NotFound("External API error");

            var dict = c.Valute;
            if (null == pos) return Ok(dict);

            if (pos < 0 || pos > dict.Count - 1) return NotFound("Wrong position");

            var res = dict.ElementAt(pos ?? 0);
            return Ok(res.Value);

        }//cur

        /// <summary>
        /// Currency by char code
        /// </summary>
        /// <param name="charCode"></param>
        /// <returns>Value like double</returns>
        [HttpGet("currency/{charCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrencyByCharCode(string charCode)
        {
            var c = await GetJSON_Async(uri, httpClient);
            if (null == c) return NotFound("External API error");
            var oneCurrency = new Currency();

            var res = c.Valute.TryGetValue(charCode, out oneCurrency);

            if (res) return Ok(oneCurrency.Value);
            return NotFound("Unknown currency");
        }//cur

        //----------------------------------------------------------------------

        private async Task<Currencies> GetJSON_Async(string uri, HttpClient httpClient)
        {
            return await httpClient.GetFromJsonAsync<Currencies>(uri);

        }//json

    }//class
}
