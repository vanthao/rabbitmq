using System;
using System.Reflection;
using System.Configuration;

namespace Ncua.CUOnline.EventServiceHost
{
    static class ConfigurationHelper
    {
       
        private static string GetConfigurationValue(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            var configurationValue = ConfigurationManager.AppSettings[key];

            if (configurationValue == null)
                throw new ConfigurationErrorsException(string.Format("AppSettings does not contain setting for key ={0}", key));

            return configurationValue;
        }

        internal static string ServiceName
        {
            get { return GetConfigurationValue("ServiceName"); }
        }

        internal static string ServiceDisplayName
        {
            get { return GetConfigurationValue("ServiceDisplayName"); }
        }

        internal static string ServiceDescription
        {
            get { return GetConfigurationValue("ServiceDescription"); }
        }
    }
}
