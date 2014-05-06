using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirportData.OurAirports;

namespace AirportData.Tests.OurAirports
{
    [TestClass]
    public class RunwayTests
    {
        [TestMethod]
        public void TestCreateFromString()
        {
            string testString = "244585,20947,\"KRNT\",5382,200,\"ASPH-CONC-G\",1,0,\"16\",47.5005,-122.217,24,174,300,\"34\",47.4858,-122.215,32,354,340";
            var expected = new Runway();

            expected.Id = 244585;
            expected.AirportId = 20947;
            expected.Length = 5382;
            expected.Width = 200;
            expected.Surface = "ASPH-CONC-G";
            expected.Lighted = true;
            expected.Closed = false;
            
            expected.End1 = new RunwayEnd();
            expected.End1.Identifier = 16;
            expected.End1.Latitude = 47.5005;
            expected.End1.Longitude = -122.217;
            expected.End1.Elevation = 24;
            expected.End1.DisplacedThreshold = 300;
            
            expected.End2 = new RunwayEnd();
            expected.End2.Identifier = 34;
            expected.End2.Latitude = 47.4858;
            expected.End2.Longitude = -122.215;
            expected.End2.Elevation = 32;
            expected.End2.DisplacedThreshold = 340;

            var actual = Runway.CreateFromString(testString);

            if (expected.Id != actual.Id
                || expected.AirportId != actual.AirportId
                || expected.Length != actual.Length 
                || expected.Width != actual.Width
                || expected.Surface != actual.Surface 
                || expected.Lighted != actual.Lighted
                || expected.Closed != actual.Closed
                || expected.End1.Identifier != actual.End1.Identifier
                || expected.End1.Latitude != actual.End1.Latitude
                || expected.End1.Longitude != actual.End1.Longitude 
                || expected.End1.Elevation != actual.End1.Elevation 
                || expected.End1.DisplacedThreshold != actual.End1.DisplacedThreshold
                || expected.End2.Identifier != actual.End2.Identifier
                || expected.End2.Latitude != actual.End2.Latitude
                || expected.End2.Longitude != actual.End2.Longitude 
                || expected.End2.Elevation != actual.End2.Elevation 
                || expected.End2.DisplacedThreshold != actual.End2.DisplacedThreshold)
            {
                Assert.Fail();
            }
            
        }
    }
}
