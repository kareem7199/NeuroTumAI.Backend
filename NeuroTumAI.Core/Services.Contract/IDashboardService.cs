using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Dashboard;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IDashboardService
	{
		Task<DashboardStatsDto> GetStatisticsAsync();
	}
}
