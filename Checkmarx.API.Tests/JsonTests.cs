using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Checkmarx.API.Tests
{
    [TestClass]
    public class JsonTests
    {
        [TestMethod]
        public void ReadJsonFileTest()
        {
            // read swagger api json file
            var jsonFile = File.ReadAllText("C:\\Users\\pedropo\\OneDrive - Checkmarx\\Checkmarx.API\\v1.json");

            // parse json file
            var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonFile);

            foreach (var item in jsonObject.paths)
            {
                Trace.WriteLine((string)item.Name);

                foreach (var subitem in ((JProperty)item).Descendants())
                {
                    foreach (JToken token in subitem.Children())
                    {
                        // Trace.WriteLine("\t-" + ); 
                    }
                }
            }

            foreach (var def in jsonObject.definitions)
            {
                Trace.WriteLine((string)def.Name);
            }

            // ((JArray)jsonObject.paths).Add();

            File.WriteAllText("C:\\Users\\pedropo\\OneDrive - Checkmarx\\Checkmarx.API\\v1_Copy.json", Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject));
        }


    }
}
