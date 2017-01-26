﻿using System;
using System.Collections.Generic;
using System.Linq;
using IST.Interfaces.IServices;
using IST.Interfaces.Repository;
using IST.Models.DomainModels;

namespace IST.Implementation.Services
{
    public class FilterService : IFilterService
    {
        #region Private

        private readonly IFilterRepository filterRepository;

        #endregion

        #region Constructor

        public FilterService(IFilterRepository filterRepository)
        {
            this.filterRepository = filterRepository;
        }

        #endregion

        #region Public

        public IEnumerable<Filter> GetAll()
        {
            return filterRepository.GetAll();
        }

        public Filter FindFilterById(int filterId)
        {
            return filterRepository.Find(filterId);
        }
        
        public bool SaveOrUpdateFilter(Filter filter)
        {
            if (filter.Id > 0)
            {
                var filterToUpdate = filterRepository.Find(filter.Id);
                filterToUpdate.DisplayValue = filter.DisplayValue;
                filterToUpdate.FilterCategoryId = filter.FilterCategoryId;
                filterToUpdate.RecLastUpdatedById = filter.RecLastUpdatedById;
                filterToUpdate.RecLastUpdatedOn = filter.RecLastUpdatedOn;
                filterRepository.Update(filterToUpdate);
            }
            else
            {
                filterRepository.Add(filter);
            }
            filterRepository.SaveChanges();
            return true;
        }

        public bool DeleteFilter(int filterId)
        {
            var toDelete = filterRepository.Find(filterId);
            if (toDelete.Solutions.Any())
            {
                throw new Exception("You cannot delete " + toDelete.DisplayValue + " as it is being used in Solutions.");
            }
            filterRepository.Delete(toDelete);
            filterRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}