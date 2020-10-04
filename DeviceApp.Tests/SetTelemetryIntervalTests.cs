using DeviceApp.Services;
using Microsoft.Azure.Devices.Client;
using System;
using Xunit;
using Microsoft.Azure.Devices;
using System.Text;

namespace DeviceApp.Tests
{
    public class SetTelemetryIntervalTests
    {
        [Fact]
        public void SetTelemetryInterval_ShouldReturnOkStatusCode()
        {
            var arrange = Encoding.UTF8.GetBytes("5");
            var response = DeviceService.SetTelemetryInterval(new MethodRequest("SetTelemetryInterval", arrange), null).GetAwaiter().GetResult();

            Assert.Equal(200, response.Status);
        }




        //[Theory]
        //[InlineData("SetTelemetryInterval", "10", 200)]

        //public void SetTelemetryInterval_Test(string methodName, string payload, int statusCode)
        //{
        //    var arrange = Encoding.UTF8.GetBytes("5");
        //    var response = DeviceService.SetTelemetryInterval(new MethodRequest(methodName, arrange), null).GetAwaiter().GetResult();
        //    Assert.Equal(200, response.Status);
        //}
    }
}
