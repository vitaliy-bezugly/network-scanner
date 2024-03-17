﻿using IpScanner.Models.Enums;
using IpScanner.Models;
using IpScanner.Infrastructure.ContentCreators;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Mappers;
using IpScanner.Infrastructure.UnitTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace IpScanner.Infrastructure.UnitTests
{
    [TestClass]
    public class DevicesJsonContentCreatorUnitTests
    {
        [TestMethod]
        public void CreateContent_ShouldReturnJson_WhenDevicesAreProvided()
        {
            // Arrange
            var devices = new Device[]
            {
                new Device(IPAddress.Any),
                new Device(IPAddress.Parse("192.168.0.106")),
                new Device(DeviceStatus.Online, "host.docker.internal", IPAddress.Parse("192.168.0.105"), "CyberTAN Technology Inc.", PhysicalAddress.Parse("283926CF58E9"), string.Empty, System.DateTime.UtcNow)
            };

            var creator = new DevicesJsonContentCreator();

            // Act
            string actual = creator.CreateContent(devices);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(string.IsNullOrEmpty(actual));
            AssertDevicesJsonContentMatchesExpected(devices, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void CreateContent_ShouldThrowArgumentNullException_WhenDevicesAreNull()
        {
            // Arrange
            var creator = new DevicesJsonContentCreator();

            // Act
            creator.CreateContent(null);
        }

        private void AssertDevicesJsonContentMatchesExpected(Device[] expectedDevices, string jsonContent)
        {
            Device[] deserializedDevices = jsonContent.FromJson<DeviceEntity[]>()
                .Select(x => x.ToDomain())
                .ToArray();

            Assert.AreEqual(expectedDevices.Length, deserializedDevices.Length);
            for (int i = 0; i < expectedDevices.Length; i++)
            {
                Assert.AreEqual(expectedDevices[i], deserializedDevices[i], $"Devices at index {i} do not match.");
            }
        }
    }
}
