using CxDataRepository;
using cxPortalWebService93;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Checkmarx.API
{
    public static class Utils
    {
        public static Uri GetLink(this CxWSSingleResultData result)
        {
            return new Uri(result.PathId.ToString());
        }

        /// <summary>
        /// https://.checkmarx.net/cxwebclient/ViewerMain.aspx?scanid=1032992&projectid=12&pathid=1
        /// </summary>
        /// <param name="result"></param>
        /// <param name="clientV93"></param>
        /// <returns></returns>
        public static Uri GetLink(this PortalSoap.CxWSSingleResultData result, CxClient clientV93, long projectId, long scanId)
        {
            return new Uri(clientV93.SASTServerURL, $"cxwebclient/ViewerMain.aspx?scanid={scanId}&projectid={projectId}&pathid={result.PathId}");
        }

        public static Uri GetLink(this CxAuditWebServiceV9.CxWSResultPath resultPath, CxClient clientV93, long projectId, long scanId)
        {
            return new Uri(clientV93.SASTServerURL, $"cxwebclient/ViewerMain.aspx?scanid={scanId}&projectid={projectId}&pathid={resultPath.PathId}");
        }

        public static Uri GetLink(this Result item, CxClient clientV93, long? projectId, long? scanId)
        {
            return new Uri(clientV93.SASTServerURL, $"cxwebclient/ViewerMain.aspx?scanid={scanId}&projectid={projectId}&pathid={item.PathId}");
        }

        public static Uri GetLink(this Checkmarx.API.SAST.OData.Result item, CxClient clientV94, long projectId, long scanId)
        {
            return new Uri(clientV94.SASTServerURL, $"cxwebclient/ViewerMain.aspx?scanid={scanId}&projectid={projectId}&pathid={item.PathId}");
        }

        public static T Map<T>(object oldObject) where T : new()
        {
            // Create a new object of type TDATA
            T newObject = new T();
            try
            {
                // If the old object is null, just return the new object
                if (oldObject == null) return newObject;

                // Get the type of the new object and the type of the old object passed in
                Type newObjType = typeof(T);
                Type oldObjType = oldObject.GetType();

                // Get a list of all the properties in the new object
                var propertyList = newObjType.GetProperties();

                // If the new object has properties
                if (propertyList.Length > 0)
                {
                    // Loop through each property in the new object
                    foreach (var newObjProp in propertyList)
                    {
                        // Get the corresponding property in the old object
                        var oldProp = oldObjType.GetProperty(newObjProp.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.ExactBinding);

                        // If there is a corresponding property in the old object and it can be read and the new object's property can be written to
                        if (oldProp != null && oldProp.CanRead && newObjProp.CanWrite)
                        {
                            // assign property type of both object to new variables
                            var oldPropertyType = oldProp.PropertyType;
                            var newPropertyType = newObjProp.PropertyType;

                            //check if property is nullable or not. if property is nullable then get it's original data type from generic argument
                            if (oldPropertyType.IsGenericType && oldPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) oldPropertyType = oldPropertyType.GetGenericArguments()[0];

                            if (newPropertyType.IsGenericType && newPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) newPropertyType = newPropertyType.GetGenericArguments()[0];

                            //check type of both property if match then set value
                            if (newPropertyType == oldPropertyType)
                            {
                                // Get the value of the property in the old object
                                var value = oldProp.GetValue(oldObject);

                                // Set the value of the property in the new object
                                newObjProp.SetValue(newObject, value);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            // Return the new object
            return newObject;
        }
    }
}
