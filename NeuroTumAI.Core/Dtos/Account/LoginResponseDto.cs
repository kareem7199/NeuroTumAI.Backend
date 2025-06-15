using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Dtos.Account
{
	public class LoginResponseDto
	{
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
