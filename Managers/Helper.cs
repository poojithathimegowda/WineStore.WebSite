using System.Net.Http;
using WineStore.WebSite.Models.Admin;

namespace WineStore.WebSite.Managers
{
    public static class Helper
    {
    


        public static TOutput ConvertToObjectType<TOutput>(IFormCollection collection) where TOutput : new()
        {
            TOutput output = new TOutput();

            foreach (var key in collection.Keys)
            {
                var propertyInfo = typeof(TOutput).GetProperty(key);
                if (propertyInfo != null)
                {
                    try
                    {
                        var value = Convert.ChangeType(collection[key].ToString(), propertyInfo.PropertyType);
                        propertyInfo.SetValue(output, value);
                    }
                    catch (InvalidCastException)
                    {
                        // Handle the error or ignore if the property is optional
                    }
                }
            }

            return output;
        }

    }
}





