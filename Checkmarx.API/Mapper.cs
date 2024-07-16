using Checkmarx.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Checkmarx.API
{
    public static class Mapper
    {
        public static T Map<T>(object oldObject) where T : new()
        {
            if (oldObject == null)
                throw new ArgumentNullException(nameof(oldObject));

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

        public static SoapSingleResultData MapSoapSingleResultData(PortalSoap.CxWSSingleResultData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var result = Map<SoapSingleResultData>(data);

            switch (data.ResultStatus)
            {
                case PortalSoap.CompareStatusType.New:
                    result.ResultStatus = ResultStatus.New;
                    break;
                case PortalSoap.CompareStatusType.Reoccured:
                    result.ResultStatus = ResultStatus.Reoccured;
                    break;
                case PortalSoap.CompareStatusType.Fixed:
                    result.ResultStatus = ResultStatus.Fixed;
                    break;
                default:
                    throw new NotSupportedException($"Priority API result status \"{data.ResultStatus.ToString()}\" not supported.");
            }

            return result;
        }

        public static PrioritySingleResultData MapPrioritySingleResultData(cxPriorityWebService.CxWSSingleResultDataPriority data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var result = Map<PrioritySingleResultData>(data);

            switch (data.ResultStatus)
            {
                case cxPriorityWebService.CompareStatusType.New:
                    result.ResultStatus = ResultStatus.New;
                    break;
                case cxPriorityWebService.CompareStatusType.Reoccured:
                    result.ResultStatus = ResultStatus.Reoccured;
                    break;
                case cxPriorityWebService.CompareStatusType.Fixed:
                    result.ResultStatus = ResultStatus.Fixed;
                    break;
                default:
                    throw new NotSupportedException($"Priority API result status \"{data.ResultStatus.ToString()}\" not supported.");
            }

            return result;
        }
    }
}
