using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroTumAI.Core.Dtos.Account;
using NeuroTumAI.Core.Entities.Admin;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface IAdminService
	{
		Task<string> LoginAdminAsync(LoginDto model);
		Task<Admin> GetAdminByIdAsync(int id);
	}
}
