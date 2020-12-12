using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TaxServiceApplication
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TaxService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TaxService.svc or TaxService.svc.cs at the Solution Explorer and start debugging.
    public class TaxService : ITaxService
    {
        double taxrate = 1.13;
        public double CalculateTax(double preTax)
        {
            return preTax * taxrate;
        }

  
    }
}
