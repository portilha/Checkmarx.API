using System;
using System.Collections.Generic;
using System.Text;

namespace Checkmarx.API.SAST
{
    /// <summary>Configuration group name associated with the settings to be retrieved</summary>
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.4.3.0 (Newtonsoft.Json v11.0.0.0)")]
    public enum Group
    {
        [System.Runtime.Serialization.EnumMember(Value = @"None")]
        None = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"AppSecCoach")]
        AppSecCoach = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"AuthorizationService")]
        AuthorizationService = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"Portal")]
        Portal = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"Scanning")]
        Scanning = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"SystemSettings")]
        SystemSettings = 5,

        [System.Runtime.Serialization.EnumMember(Value = @"QueueSettings")]
        QueueSettings = 6,

        [System.Runtime.Serialization.EnumMember(Value = @"Audit")]
        Audit = 7,

        [System.Runtime.Serialization.EnumMember(Value = @"Logging")]
        Logging = 8,

        [System.Runtime.Serialization.EnumMember(Value = @"DataRetention")]
        DataRetention = 9,

        [System.Runtime.Serialization.EnumMember(Value = @"Reports")]
        Reports = 10,

    }
}
