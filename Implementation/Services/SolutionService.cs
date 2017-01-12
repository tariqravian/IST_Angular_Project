﻿using System;
using System.Collections.Generic;
using System.Linq;
using IST.Interfaces.IServices;
using IST.Interfaces.Repository;
using IST.Models.Common.DropDown;
using IST.Models.DomainModels;
using IST.Models.RequestModels;
using IST.Models.ResponseModels;

namespace IST.Implementation.Services
{
    /// <summary>
    /// QuantityScale Service
    /// </summary>
    public sealed class SolutionService : ISolutionService
    {
        #region Private
        private readonly ISolutionRepository solutionRepository;
        private readonly ITagRepository tagRepository;
        private readonly IFilterRepository filterRepository;
        private readonly ISolutionTypeRepository solutionTypeRepository;
        private readonly ISolutionOwnerRepository solutionOwnerRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public SolutionService(ITagRepository tagRepository, IFilterRepository filterRepository,
            ISolutionRepository solutionRepository, ISolutionTypeRepository solutionTypeRepository, ISolutionOwnerRepository solutionOwnerRepository)
        {
            
            this.tagRepository = tagRepository;
            this.filterRepository = filterRepository;
            this.solutionRepository = solutionRepository;
            this.solutionOwnerRepository = solutionOwnerRepository;
            this.solutionTypeRepository = solutionTypeRepository;
        }

        #endregion

        #region Public

        public SearchTemplateResponse<Solution> Search(SolutionSearchRequest searchRequest)
        {
            return solutionRepository.Search(searchRequest);
        }

        public Solution GetById(int id)
        {
            return solutionRepository.GetById(id);
        }

        public SolutionBaseData GetBaseData(int id)
        {
            var basedata = new SolutionBaseData
            {
                Solution = id > 0 ? solutionRepository.GetById(id) : null,
                SolutionOwners = solutionOwnerRepository.GetAll().Select(x => new DropDownModel { Id = x.Id, DisplayName = x.DisplayValue }),
                SolutionTypes = solutionTypeRepository.GetAll().Select(x => new DropDownModel { Id = x.Id, DisplayName = x.DisplayValue }),
                Tags = tagRepository.GetAll().Select(x => new DropDownModel { Id = x.Id, DisplayName = x.DisplayValue }),
                Filters = filterRepository.GetAll().Select(x => new DropDownModel { Id = x.Id, DisplayName = x.DisplayValue }),
            };
            return basedata;
        }

        public SolutionBaseData GetFilterData()
        {
            var filterdata = new SolutionBaseData
            {
                SolutionTypes =
                    solutionTypeRepository.GetAll()
                        .Select(x => new DropDownModel {Id = x.Id, DisplayName = x.DisplayValue})
            };
            return filterdata;
        }
        public bool SaveOrUpdate(SolutionCreateResponseModel response)
        {
            #region Update Case
            if (response.Solution.Id > 0)
            {
                var solutionToUpdate = solutionRepository.Find(response.Solution.Id);
                solutionToUpdate.Name = response.Solution.Name;
                solutionToUpdate.Location = response.Solution.Location;
                solutionToUpdate.MaintentanceHours = response.Solution.MaintentanceHours;
                solutionToUpdate.Active = response.Solution.Active;
                solutionToUpdate.TypeId = response.Solution.TypeId;
                solutionToUpdate.OwnerId = response.Solution.OwnerId;
                solutionToUpdate.Image = response.Solution.Image;
                solutionToUpdate.SecurityInfo = response.Solution.SecurityInfo;
                solutionToUpdate.Description = response.Solution.Description;
                UpdateTags(solutionToUpdate, response.TagIds);
                UpdateFilters(solutionToUpdate, response.FilterIds);
                solutionRepository.Update(solutionToUpdate);
            }
            #endregion

            #region Add Case
            else
            {
                response.Solution.Tags = response.TagIds?.Select(tagId => tagRepository.Find(tagId)).ToList();
                response.Solution.Filters = response.FilterIds?.Select(filterId => filterRepository.Find(filterId)).ToList();
                solutionRepository.Add(response.Solution);
            }
            #endregion

            solutionRepository.SaveChanges();
            return true;
        }

        private void AddFilters(Solution solution)
        {
            var result = filterRepository.GetByFilterIds(solution.Filters.Select(x => x.Id).ToArray());
            solution.Filters = new List<Filter>();
            foreach (var newfilter in result)
            {
                solution.Filters.Add(newfilter);
            }
        }

        private void AddTags(Solution solution)
        {
            var result = tagRepository.GetByTagIds(solution.Tags.Select(x => x.Id).ToArray());
            solution.Tags = new List<Tag>();
            foreach (var newtag in result)
            {
                solution.Tags.Add(newtag);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Takes Solution to Update and Update Filters based on Client List
        /// </summary>
        /// <param name="solutionToUpdate"></param>
        /// <param name="filterIds"></param>
        private void UpdateFilters(Solution solutionToUpdate, List<int> filterIds)
        {
            var newFilters = filterIds?.Select(filterId => filterRepository.Find(filterId)).ToList();

            var result = solutionToUpdate.Filters.Where(oldFilterId =>
            {
                return newFilters != null && newFilters.All(newFilterId => newFilterId.Id != oldFilterId.Id);
            }).ToList();


            if (filterIds != null)
            {
                //Add Items which are not in Db List
                foreach (var newFilter in newFilters)
                {
                    if (solutionToUpdate.Filters.Any(x => x.Id == newFilter.Id))
                        continue;
                    solutionToUpdate.Filters.Add(newFilter);
                }

                //Delete Items which are not in Client List
                foreach (var filter in result)
                {
                    solutionToUpdate.Filters.Remove(filter);
                }
            }
            else
            {
                //Remove All Items from Database if Clientlist is Empty
                solutionToUpdate.Filters.Clear();
            }
        }

        /// <summary>
        /// Takes Solution to Update and Update Tags based on Client List
        /// </summary>
        /// <param name="solutionToUpdate"></param>
        /// <param name="tagIds"></param>
        private void UpdateTags(Solution solutionToUpdate, List<int> tagIds)
        {
            var newTags = tagIds?.Select(tagId => tagRepository.Find(tagId)).ToList();

            var result = solutionToUpdate.Tags.Where(oldTagId =>
            {
                return newTags != null && newTags.All(newTagId => newTagId.Id != oldTagId.Id);
            }).ToList();


            if (tagIds != null)
            {
                //Add Items which are not in Db List
                foreach (var newTag in newTags)
                {
                    if (solutionToUpdate.Tags.Any(x => x.Id == newTag.Id))
                        continue;
                    solutionToUpdate.Tags.Add(newTag);
                }

                //Delete Items which are not in Client List
                foreach (var tag in result)
                {
                    solutionToUpdate.Tags.Remove(tag);
                }
            }
            else
            {
                //Remove All Items from Database if Clientlist is Empty
                solutionToUpdate.Tags.Clear();
            }
        }

        #endregion
    }
}
