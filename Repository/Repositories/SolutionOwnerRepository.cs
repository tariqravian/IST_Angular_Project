﻿using System.Data.Entity;
using IST.Interfaces.Repository;
using IST.Models.DomainModels;
using IST.Repository.BaseRepository;
using Microsoft.Practices.Unity;

namespace IST.Repository.Repositories
{
    public class SolutionOwnerRepository : BaseRepository<SolutionOwner>, ISolutionOwnerRepository
    {
        public SolutionOwnerRepository(IUnityContainer container) : base(container)
        {
        }

        protected override IDbSet<SolutionOwner> DbSet => db.SolutionOwners;
    }
}