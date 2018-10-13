using SammysAuto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SammysAuto.ViewModel
{
    public class CarAndServicesViewModel
    {
        // for displaying the Car info
        public int carId { get; set; }
        public string VIN { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Style { get; set; }
        public int Year { get; set; }
        public string UserId { get; set; }

        // for adding a new service
        public Service NewServiceObj { get; set; }

        // for the list of services
        public IEnumerable<Service> PastServicesObj { get; set; }

        // for the dropdownlist displaying the service types
        public List<ServiceType> ServiceTypesObj { get; set; }
    }
}
