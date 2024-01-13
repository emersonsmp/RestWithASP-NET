﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestWithASPNET.Hypermedia.Filters
{
    public class HyperMediaFilter: ResultFilterAttribute
    {
        private readonly HyperMediaFiltersOptions _hyperMediaFilterOptions;

        public HyperMediaFilter(HyperMediaFiltersOptions hyperMediaFilterOptions)
        {
            _hyperMediaFilterOptions = hyperMediaFilterOptions;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            TryEnrichResult(context);
            base.OnResultExecuting(context);
        }

        private void TryEnrichResult(ResultExecutingContext context)
        {
            if(context.Result is OkObjectResult objectResult)
            {
                var enricher = _hyperMediaFilterOptions.ContentResponseEnricherList.FirstOrDefault(x => x.CanEnrich(context));

                if(enricher != null)
                {
                    Task.FromResult(enricher.Enrich(context));
                }
            }
        }
    }
}
