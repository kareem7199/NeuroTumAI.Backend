using System;
using System.Collections.Concurrent;
using System.Globalization;
using Microsoft.Extensions.Localization;
using NeuroTumAI.Core.Services.Contract;

namespace NeuroTumAI.Service.Services.LocalizationService
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IStringLocalizerFactory _localizerFactory;
        private readonly ConcurrentDictionary<Type, IStringLocalizer> _localizerCache;

        public LocalizationService(IStringLocalizerFactory localizerFactory)
        {
            _localizerFactory = localizerFactory;
            _localizerCache = new ConcurrentDictionary<Type, IStringLocalizer>();
        }

        private IStringLocalizer GetLocalizer<T>()
        {
            var resourceType = typeof(T);
            string resourceName = resourceType.FullName ?? resourceType.Name;
            string assemblyName = resourceType.Assembly.GetName().Name;

            return _localizerCache.GetOrAdd(resourceType, _ => _localizerFactory.Create(resourceName, assemblyName));
        }

        public string GetCurrentLanguage()
        {
            return CultureInfo.CurrentUICulture.Name;
        }

        public string GetMessage<T>(string key)
        {
            var localizedString = GetLocalizer<T>()[key];
            return localizedString.ResourceNotFound ? $"[{key}]" : localizedString.Value;
        }

        public string GetMessage<T>(string key, params object[] args)
        {
            var localizedString = GetLocalizer<T>()[key, args];
            return localizedString.ResourceNotFound ? $"[{key}]" : localizedString.Value;
        }
    }
}
