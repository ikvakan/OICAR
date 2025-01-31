﻿using OICAR_Desktop.DAL.Interfaces;
using OICAR_Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OICAR_Desktop.DAL
{
    class UniteOfWork : IUniteOfWork
    {
        private readonly ModelContainer contex;

        public IClientRepository Clients { get; set; }
        public IServiceRepository Services { get; set; }
        public IWorkerRepository Workers { get; set; }

        public ISpecializationRepository Specializations { get; set; }

        public IWorkerSpecializationRepository WorkerSpecializations { get; set; }

        public IServiceTypeRepository ServiceTypes { get; set; }

        public IApponitmentRepository Apponitments { get; set; }

        public UniteOfWork(ModelContainer modelContainer)
        {
            contex = modelContainer;
            Clients = new ClientRepository(contex);
            Services = new ServiceRepository(contex);
            Workers = new WorkerRepository(contex);
            Specializations = new SpecializationRepositroy(contex);
            WorkerSpecializations = new WorkerSpecializationsRepository(contex);
            ServiceTypes = new ServiceTypeRepository(contex);
            Apponitments = new ApponitmentRepository(contex);
        }

       

        public void Dispose()
        {
            contex.Dispose();
        }

        public int SaveChanges()
        {
           return contex.SaveChanges();
        }
    }
}
