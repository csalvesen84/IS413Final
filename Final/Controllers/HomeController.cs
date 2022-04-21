using Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IQuotesRepository repo { get; set; }

        private int randomID { get; set; }

        public HomeController(ILogger<HomeController> logger, IQuotesRepository temp)
        {

            _logger = logger;
            repo = temp;

            List <int> randomOptions = repo.Quotes.Select(x => x.QuoteId).Distinct().ToList();
            var random = new Random();

            int index = random.Next(randomOptions.Count);

            randomID = randomOptions[index];
        }

        public IActionResult Index()
        {
            var quotes = repo.Quotes.OrderBy(x => x.Author).ToList();
            return View(quotes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Quote submission)
        {
            if (ModelState.IsValid)
            {
                repo.AddQuote(submission);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Detail(int quote)
        {
            var detailQuote = repo.Quotes.Single(x => x.QuoteId == quote);
            ViewBag.Date = "";


            if (detailQuote.Date != null)
            {
                string SDate = detailQuote.Date.ToString();
                var spaceIndex = SDate.IndexOf(" ");
                ViewBag.Date = SDate.Substring(0, spaceIndex);
            }
            return View(detailQuote);
        }

        [HttpGet]
        public IActionResult Edit(int quote)
        {
            var quoteEdit = repo.Quotes.Single(x => x.QuoteId == quote);

            return View("Add", quoteEdit);
        }

        [HttpPost]
        public IActionResult Edit (Quote editQuote)
        {
            repo.EditQuote(editQuote);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete (int quote)
        {
            var delQuote = repo.Quotes.Single(x => x.QuoteId == quote);

            return View(delQuote);
        }

        [HttpPost]
        public IActionResult Delete(Quote deleteQuote)
        {
            repo.DeleteQuote(deleteQuote);

            return RedirectToAction("Index");
        }

        public IActionResult Random()
        {
            var randomQuoteModel = repo.Quotes.Single(x => x.QuoteId == randomID);

            return View(randomQuoteModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
