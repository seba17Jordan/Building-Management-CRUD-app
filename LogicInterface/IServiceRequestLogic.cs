﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicInterface
{
    public interface IServiceRequestLogic
    {
        ServiceRequest CreateServiceRequest(ServiceRequest serviceRequest);

        //IEnumerable<ServiceRequest> GetAllServiceRequests(string category);
    }
}
