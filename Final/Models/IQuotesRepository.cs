using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Models
{
    public interface IQuotesRepository
    {
        IQueryable<Quote> Quotes { get;}

        void EditQuote(Quote q);

        void DeleteQuote(Quote q);

        void AddQuote(Quote q);
    }
}
