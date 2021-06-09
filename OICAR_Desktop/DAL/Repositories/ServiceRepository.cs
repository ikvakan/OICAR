﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OICAR_Desktop.Model;

namespace OICAR_Desktop.DAL
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
     

        public ServiceRepository(ModelContainer modelContainer) : base(modelContainer)
        {
           
        }

        public IEnumerable<Service> GetServiceByServiceType(int id)
        {
            return db.Service.Where(s => s.Service_TypeIdServiceType == id).ToList();
        }
    }
}
