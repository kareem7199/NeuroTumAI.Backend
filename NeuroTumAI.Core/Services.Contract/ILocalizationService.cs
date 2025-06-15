using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTumAI.Core.Services.Contract
{
	public interface ILocalizationService
	{
		string GetCurrentLanguage();
		string GetMessage<T>(string key);
		string GetMessage<T>(string key, params object[] args);
	}
}
