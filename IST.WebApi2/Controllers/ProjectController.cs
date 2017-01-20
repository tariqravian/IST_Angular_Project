﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using IST.Interfaces.IServices;
using IST.Models.Common.DropDown;
using IST.Models.RequestModels;
using IST.WebApi2.ModelMappers;
using IST.WebApi2.Models;
using IST.WebBase.Mvc;

namespace IST.WebApi2.Controllers
{
    [AllowAnonymous]
    public class ProjectController : BaseController
    {
        #region Private
        private readonly ISolutionService solutionService;
        private readonly IFilterCategoryService filterCategoryService;
        #endregion

        #region Constructor
        public ProjectController(ISolutionService solutionService, IFilterCategoryService filterCategoryService)
        {
            this.solutionService = solutionService;
            this.filterCategoryService = filterCategoryService;
        }
        #endregion

        #region Public

        [ValidateFilter]
        public IHttpActionResult Get([FromUri]SolutionSearchRequest searchRequest)
        {
            if (searchRequest == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid Bad Request");
            }
            if (searchRequest.FilterIds == null || !searchRequest.FilterIds.Any())
            {
                searchRequest.FilterIds = new List<int>();
            }
            var response = solutionService.Search(searchRequest);
            var toReturn = new ProjectListView
            {
                Data = response.Data.ToList().Select(x => x.ClientCreateFrom()).ToList(),
                FilterCategories = filterCategoryService.GetAll().Select(x => x.MapFromServerToClient()).ToList(),
                recordsFiltered = response.FilteredCount,
                recordsTotal = response.TotalCount
            };
            return Ok(toReturn);
        }
        public IHttpActionResult Get(int id)
        {
            var baseData = solutionService.GetBaseData(id);
            var viewModel = new SolutionViewModel
            {
                SolutionModel = baseData.Solution?.MapFromServerToClient(),
                Tags = baseData.Tags.Select(x => x.MapFromServerToClient()),
                Filters = baseData.Filters.Select(x => x.MapFromServerToClient()),
                SolutionTypes = baseData.SolutionTypes.ToList(),
                SolutionOwners = baseData.SolutionOwners.ToList()
            };
            return Ok(viewModel);
        }
        [Route("api/ProjectBaseData")]
        public IHttpActionResult Get(string name)
        {
            var response = solutionService.SearchByName(name).ToList().Select(s => new DropDownModel
            {
                DisplayName = s.Name,
                Id = s.Id,
                Subtitle = s.Description
            });
            return Ok(response);
        }

        [HttpPost]
        public IHttpActionResult Post(SolutionModel model)
        {
            if (model.IsFavorite)
            {
                //if solution is already favorite, remove it from favorite
                var status = solutionService.SaveFavorite(model.Id, false);
                return Ok(status);
            }
            else
            {
                //if solution is not already favorite, mark it favorite
                var status = solutionService.SaveFavorite(model.Id, true);
                return Ok(status);
            }
        }


        #endregion
    }
}