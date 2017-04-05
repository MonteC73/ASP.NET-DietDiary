﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CountingKs.Data;
using CountingKs.Models;
using CountingKs.Services;

namespace CountingKs.Controllers
{
    public class DiaryEntriesController : BaseApiController
    {
        private ICountingKsIdentityService _identityService;

        public DiaryEntriesController(ICountingKsRepository repo, ICountingKsIdentityService identityService ) : base(repo)
        {
            _identityService = identityService;
        }

        public IEnumerable<DiaryEntryModel> Get(DateTime diaryId)
        {
            var results = TheRepository.GetDiaryEntries(_identityService.CurrentUser, diaryId.Date)
                                       .ToList()
                                       .Select(e => TheModelFactory.Create(e));
            return results;
        }

        public HttpResponseMessage Get(DateTime diaryId, int id)
        {
            var result = TheRepository.GetDiaryEntry(_identityService.CurrentUser, diaryId.Date, id);

            if (result == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(result));
        }

        public HttpResponseMessage Post(DateTime diaryId, [FromBody]DiaryEntryModel model)
        {
            try
            {
                var entity = TheModelFactory.Parse(model);

                if (entity == null)
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read diary entry in body");

                var diary = TheRepository.GetDiary(_identityService.CurrentUser, diaryId);

                if (diary == null)
                    Request.CreateResponse(HttpStatusCode.NotFound);


                if (diary.Entries.Any(e => e.Measure.Id == entity.Measure.Id))
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Duplicats are not allowed");

                diary.Entries.Add(entity);
                if (TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(entity));
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not save to the database");
                }
                
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}