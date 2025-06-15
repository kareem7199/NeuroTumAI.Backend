using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Helpers;

namespace NeuroTumAI.Core.Specifications.ClinicSpecs
{
	public class NearbyClinicsSpecifications : BaseSpecifications<Clinic>
	{
        public NearbyClinicsSpecifications(ClinicSpecParams specParams)
			:base(C => C.IsApproved == true && (string.IsNullOrEmpty(specParams.Search) || C.Doctor.ApplicationUser.FullName.ToLower().Contains(specParams.Search)))
        {
			var userLocation = new Point(specParams.Longitude, specParams.Latitude) { SRID = 4326 };

			AddOrderBy(C => C.Location.Distance(userLocation));
			Includes.Add(C => C.Doctor);
			Includes.Add(c => c.Doctor.ApplicationUser);
			Includes.Add(C => C.Doctor.Reviews);
			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
		}
	}
}
