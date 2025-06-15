using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace NeuroTumAI.Core.Helpers
{
	public static class GeoHelper
	{
		public static double CalculateDistance(double userLatitude, double userLongitude, double clinicLatitude, double clinicLongitude)
		{
			var userLocation = new Point((double)userLongitude, (double)userLatitude) { SRID = 4326 };
			var clinicLocation = new Point((double)clinicLongitude, (double)clinicLatitude) { SRID = 4326 };

			return userLocation.Distance(clinicLocation) / 1000;
		}
	}
}
