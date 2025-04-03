using Checkmarx.API.Models;
using Checkmarx.API.SAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Checkmarx.API
{
    internal static class Mapper
    {
        internal static T Map<T>(object oldObject) where T : new()
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

        #region SOAP

        internal static SoapSingleResultData MapSoapSingleResultData(PortalSoap.CxWSSingleResultData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var result = Map<SoapSingleResultData>(data);

            switch (data.ResultStatus)
            {
                case PortalSoap.CompareStatusType.New:
                    result.ResultStatus = PortalSoap.CompareStatusType.New;
                    break;
                case PortalSoap.CompareStatusType.Reoccured:
                    result.ResultStatus = PortalSoap.CompareStatusType.Reoccured;
                    break;
                case PortalSoap.CompareStatusType.Fixed:
                    result.ResultStatus = PortalSoap.CompareStatusType.Fixed;
                    break;
                default:
                    throw new NotSupportedException($"Priority API result status \"{data.ResultStatus.ToString()}\" not supported.");
            }

            return result;
        }

        internal static PrioritySingleResultData MapPrioritySingleResultData(cxPriorityWebService.CxWSSingleResultDataPriority data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var result = Map<PrioritySingleResultData>(data);

            switch (data.ResultStatus)
            {
                case cxPriorityWebService.CompareStatusType.New:
                    result.ResultStatus = PortalSoap.CompareStatusType.New;
                    break;
                case cxPriorityWebService.CompareStatusType.Reoccured:
                    result.ResultStatus = PortalSoap.CompareStatusType.Reoccured;
                    break;
                case cxPriorityWebService.CompareStatusType.Fixed:
                    result.ResultStatus = PortalSoap.CompareStatusType.Fixed;
                    break;
                default:
                    throw new NotSupportedException($"Priority API result status \"{data.ResultStatus.ToString()}\" not supported.");
            }

            return result;
        }

        #endregion

        #region OData

        #region Scans

        internal static IEnumerable<CxDataRepository97.Scan> MapOdataScans(IQueryable<CxDataRepository.Scan> data)
        {
            foreach (var item in data)
                yield return MapOdataScan(item);
        }

        internal static CxDataRepository97.Scan MapOdataScan(CxDataRepository.Scan data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var result = Map<CxDataRepository97.Scan>(data);

            if (int.TryParse(data.OwningTeamId, out int owningTeamId))
                result.OwningTeamId = owningTeamId;

            return result;
        }

        internal static Scan ConvertScanFromOData(CxDataRepository97.Scan scan)
        {
            return new Scan
            {
                Comment = scan.Comment,
                Id = scan.Id,
                IsLocked = scan.IsLocked,
                IsIncremental = scan.IsIncremental.HasValue ? scan.IsIncremental.Value : false,
                InitiatorName = scan.InitiatorName,
                OwningTeamId = scan.OwningTeamId.ToString(),
                PresetId = scan.PresetId,
                PresetName = scan.PresetName,
                IsPublic = scan.IsPublic,
                ScanType = new FinishedScanStatus
                {
                    Id = scan.ScanType,
                    //Value = 
                },
                ScanState = new ScanState
                {
                    LanguageStateCollection = scan.ScannedLanguages.Select(language => new LanguageStateCollection
                    {
                        LanguageName = language.LanguageName
                    }).ToList(),

                    LinesOfCode = scan.LOC.GetValueOrDefault(),
                    CxVersion = scan.ProductVersion,
                },
                Origin = scan.Origin,
                ScanRisk = scan.RiskScore,
                DateAndTime = new DateAndTime
                {
                    EngineFinishedOn = scan.EngineFinishedOn,
                    EngineStartedOn = scan.EngineStartedOn,
                    StartedOn = scan.ScanRequestedOn,
                    FinishedOn = scan.ScanCompletedOn
                },
                Results = new SASTResults
                {
                    Critical = (uint)scan.Critical,
                    High = (uint)scan.High,
                    Medium = (uint)scan.Medium,
                    Low = (uint)scan.Low,
                    Info = (uint)scan.Info,

                    CriticalToVerify = (uint)scan.Results.Where(x => x.Severity == CxDataRepository97.Severity.Critical && x.StateId == (int)CxClient.ResultState.ToVerify).Count(),
                    HighToVerify = (uint)scan.Results.Where(x => x.Severity == CxDataRepository97.Severity.High && x.StateId == (int)CxClient.ResultState.ToVerify).Count(),
                    MediumToVerify = (uint)scan.Results.Where(x => x.Severity == CxDataRepository97.Severity.Medium && x.StateId == (int)CxClient.ResultState.ToVerify).Count(),
                    LowToVerify = (uint)scan.Results.Where(x => x.Severity == CxDataRepository97.Severity.Low && x.StateId == (int)CxClient.ResultState.ToVerify).Count(),

                    ToVerify = (uint)scan.Results.Where(x => x.StateId == (int)CxClient.ResultState.ToVerify).Count(),
                    NotExploitableMarked = (uint)scan.Results.Where(x => x.StateId == (int)CxClient.ResultState.NonExploitable).Count(),
                    PNEMarked = (uint)scan.Results.Where(x => x.StateId == (int)CxClient.ResultState.ProposedNotExploitable).Count(),
                    OtherStates = (uint)scan.Results.Where(x => x.StateId != (int)CxClient.ResultState.Confirmed && x.StateId != (int)CxClient.ResultState.Urgent && x.StateId != (int)CxClient.ResultState.NonExploitable && x.StateId != (int)CxClient.ResultState.ProposedNotExploitable && x.StateId != (int)CxClient.ResultState.ToVerify).Count(),

                    FailedLoC = (int)scan.FailedLOC.GetValueOrDefault(),
                    Loc = (int)scan.LOC.GetValueOrDefault()
                }
            };
        }

        internal static Scan ConvertScanFromOData(CxDataRepository.Scan scan)
        {
            return new Scan
            {
                Comment = scan.Comment,
                Id = scan.Id,
                IsLocked = scan.IsLocked,
                IsIncremental = scan.IsIncremental.HasValue ? scan.IsIncremental.Value : false,
                InitiatorName = scan.InitiatorName,
                OwningTeamId = scan.OwningTeamId.ToString(),
                PresetId = scan.PresetId,
                PresetName = scan.PresetName,
                IsPublic = scan.IsPublic,
                ScanType = new FinishedScanStatus
                {
                    Id = scan.ScanType,
                    //Value = 
                },
                ScanState = new ScanState
                {
                    LanguageStateCollection = scan.ScannedLanguages.Select(language => new LanguageStateCollection
                    {
                        LanguageName = language.LanguageName
                    }).ToList(),

                    LinesOfCode = scan.LOC.GetValueOrDefault(),
                    CxVersion = scan.ProductVersion,
                },
                Origin = scan.Origin,
                ScanRisk = scan.RiskScore,
                DateAndTime = new DateAndTime
                {
                    EngineFinishedOn = scan.EngineFinishedOn,
                    EngineStartedOn = scan.EngineStartedOn,
                    StartedOn = scan.ScanRequestedOn,
                    FinishedOn = scan.ScanCompletedOn
                },
                Results = new SASTResults
                {
                    Critical = 0,
                    High = (uint)scan.High,
                    Medium = (uint)scan.Medium,
                    Low = (uint)scan.Low,
                    Info = (uint)scan.Info,

                    CriticalToVerify = 0,
                    HighToVerify = (uint)scan.Results.Where(x => x.Severity == CxDataRepository.Severity.High && x.StateId == (int)CxClient.ResultState.ToVerify).Count(),
                    MediumToVerify = (uint)scan.Results.Where(x => x.Severity == CxDataRepository.Severity.Medium && x.StateId == (int)CxClient.ResultState.ToVerify).Count(),
                    LowToVerify = (uint)scan.Results.Where(x => x.Severity == CxDataRepository.Severity.Low && x.StateId == (int)CxClient.ResultState.ToVerify).Count(),

                    ToVerify = (uint)scan.Results.Where(x => x.StateId == (int)CxClient.ResultState.ToVerify).Count(),
                    NotExploitableMarked = (uint)scan.Results.Where(x => x.StateId == (int)CxClient.ResultState.NonExploitable).Count(),
                    PNEMarked = (uint)scan.Results.Where(x => x.StateId == (int)CxClient.ResultState.ProposedNotExploitable).Count(),
                    OtherStates = (uint)scan.Results.Where(x => x.StateId != (int)CxClient.ResultState.Confirmed && x.StateId != (int)CxClient.ResultState.Urgent && x.StateId != (int)CxClient.ResultState.NonExploitable && x.StateId != (int)CxClient.ResultState.ProposedNotExploitable && x.StateId != (int)CxClient.ResultState.ToVerify).Count(),

                    FailedLoC = (int)scan.FailedLOC.GetValueOrDefault(),
                    Loc = (int)scan.LOC.GetValueOrDefault()
                }
            };
        }

        #endregion

        #region Results

        internal static IEnumerable<CxDataRepository97.Result> MapOdataResults(IQueryable<CxDataRepository.Result> data)
        {
            foreach (var item in data)
                yield return MapOdataResult(item);
        }

        internal static IEnumerable<CxDataRepository97.Result> MapOdataResults(IEnumerable<CxDataRepository.Result> data)
        {
            foreach (var item in data)
                yield return MapOdataResult(item);
        }

        internal static CxDataRepository97.Result MapOdataResult(CxDataRepository.Result data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var result = Map<CxDataRepository97.Result>(data);

            switch (data.Severity)
            {
                case CxDataRepository.Severity.Critical:
                    result.Severity = CxDataRepository97.Severity.Critical;
                    break;
                case CxDataRepository.Severity.High:
                    result.Severity = CxDataRepository97.Severity.High;
                    break;
                case CxDataRepository.Severity.Medium:
                    result.Severity = CxDataRepository97.Severity.Medium;
                    break;
                case CxDataRepository.Severity.Low:
                    result.Severity = CxDataRepository97.Severity.Low;
                    break;
                case CxDataRepository.Severity.Info:
                    result.Severity = CxDataRepository97.Severity.Info;
                    break;
                default:
                    throw new NotSupportedException($"Priority API result status \"{data.Severity.ToString()}\" not supported.");
            }

            return result;
        }

        internal static IEnumerable<CxDataRepository97.Result> MapOdataResults(IQueryable<Checkmarx.API.SAST.OData.Result> data)
        {
            foreach (var item in data)
                yield return MapOdataResult(item);
        }

        internal static IEnumerable<CxDataRepository97.Result> MapOdataResults(IEnumerable<Checkmarx.API.SAST.OData.Result> data)
        {
            foreach (var item in data)
                yield return MapOdataResult(item);
        }

        internal static CxDataRepository97.Result MapOdataResult(Checkmarx.API.SAST.OData.Result data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var result = Map<CxDataRepository97.Result>(data);

            switch (data.Severity)
            {
                case SAST.OData.Severity.Critical:
                    result.Severity = CxDataRepository97.Severity.Critical;
                    break;
                case SAST.OData.Severity.High:
                    result.Severity = CxDataRepository97.Severity.High;
                    break;
                case SAST.OData.Severity.Medium:
                    result.Severity = CxDataRepository97.Severity.Medium;
                    break;
                case SAST.OData.Severity.Low:
                    result.Severity = CxDataRepository97.Severity.Low;
                    break;
                case SAST.OData.Severity.Info:
                    result.Severity = CxDataRepository97.Severity.Info;
                    break;
                default:
                    throw new NotSupportedException($"Priority API result status \"{data.Severity.ToString()}\" not supported.");
            }

            return result;
        }

        #endregion

        #endregion
    }
}
