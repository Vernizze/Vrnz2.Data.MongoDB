using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Vrnz2.Data.MongoDB.Extensions
{
    public static class ElementExtension
    {
        public static string GetBsonElementName(this string propertName, Type modelType)
        {
            var result = string.Empty;
            if (modelType == null || string.IsNullOrEmpty(propertName)) return result;

            try
            {
                result = modelType.GetProperties()
                                  .Select(p => p.GetCustomAttribute<BsonElementAttribute>())
                                  .Where(jp => jp.ElementName.ToUpper() == propertName.ToUpper())
                                  .Select(r => r.ElementName).FirstOrDefault();
            }
            catch (Exception)
            {
                result = string.Empty;
            }

            return result;
        }
    }
}
