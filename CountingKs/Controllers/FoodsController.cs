﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using CountingKs.Data;
using CountingKs.Data.Entities;
using CountingKs.Filters;
using CountingKs.Models;

namespace CountingKs.Controllers
{
    [CountingKsAuthorize(false)]
    public class FoodsController : BaseApiController
    {
        
        public FoodsController(ICountingKsRepository repo) : base(repo)
        {
        }

        const int PAGE_SIZE = 50;
        
        public object Get(bool includeMeasures = true, int page = 0)
        {
            IQueryable<Food> query;

            if (includeMeasures)
            {
                query = TheRepository.GetAllFoodsWithMeasures();
            }
            else
            {
                query = TheRepository.GetAllFoods();
            }


            var baseQuery = query.OrderBy(f => f.Description);

            var totalCount = baseQuery.Count();
            var totalPages = Math.Ceiling((double) totalCount/PAGE_SIZE);

            var helper = new UrlHelper(Request);

            var links = new List<LinkModel>();

            if (page > 0)
            {
                links.Add(TheModelFactory.CreateLink(helper.Link("Food", new { page = page - 1 }), "prevLink"));
            }
            if (page < totalPages - 1)
            {
                links.Add(TheModelFactory.CreateLink(helper.Link("Food", new { page = page + 1 }), "nextLink"));  
            } 


            //var prevUrl = page > 0 ? helper.Link("Food", new { page = page - 1 }) : "";
            //var nextUrl = page < totalPages - 1 ? helper.Link("Food", new { page = page + 1 }) : "";

            var results = baseQuery.OrderBy(f => f.Description)
                                   .Skip(PAGE_SIZE * page)
                                   .Take(PAGE_SIZE)
                                   .ToList()
                                   .Select(f => TheModelFactory.Create(f));

            return new
            {
                TotalCount = totalCount,
                TotalPage = totalPages,
                Links = links,
                //PrevPageUrl = prevUrl,
                //NextPageUrl = nextUrl,
                Results = results
            };
        }

        public FoodModel Get(int foodid)
        {
            return TheModelFactory.Create(TheRepository.GetFood(foodid));
        }
    }
}
