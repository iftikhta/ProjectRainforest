﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TaxServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITaxService" in both code and config file together.
    [ServiceContract]
    public interface ITaxService
    {
        //[OperationContract]
        //void DoWork();


        [OperationContract]
        double CalculateTax(double preTax);
    }
}
