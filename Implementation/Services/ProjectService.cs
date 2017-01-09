﻿using System;
using System.Collections.Generic;
using IST.Interfaces.IServices;
using IST.Interfaces.Repository;
using IST.Models.DomainModels;
using IST.Models.RequestModels;
using IST.Models.ResponseModels;

namespace IST.Implementation.Services
{
    /// <summary>
    /// QuantityScale Service
    /// </summary>
    public sealed class ProjectService : IProjectService
    {
        #region Private
        private readonly IStudentRepository studentRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IAllergyRepository allergyRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectService(IStudentRepository repository,
            IProjectRepository projectRepository, IAllergyRepository allergyRepository)
        {

            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            this.studentRepository = repository;
            this.allergyRepository = allergyRepository;
            this.projectRepository = projectRepository;
        }

        #endregion

        #region Public
        
        public bool Delete(int site)
        {
            var toDelete = studentRepository.GetById(site);
            if (toDelete == null)
            {
                throw new Exception("Student may have already been deleted.");
            }
            toDelete.IsDeleted = true;
            studentRepository.SaveChanges();
            return true;
        }

        public SearchTemplateResponse<Solution> Search(ProjectSearchRequest searchRequest)
        {
            return projectRepository.Search(searchRequest);
        }

        public Solution GetById(int id)
        {
            return projectRepository.GetById(id);
        }

        #endregion
    }
}
