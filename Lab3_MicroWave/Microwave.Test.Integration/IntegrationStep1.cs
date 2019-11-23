using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    // 
    [TestFixture]
    [Author("I4SWTGrp2")]
    public class IntegrationStep1
    {

        private UserInterface userInterface;
        private Door door;
        private Light light;

        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        private IDisplay display;

        private ICookController cooker;

        private IOutput output;

        [SetUp]
        public void Setup()
        {
            powerButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();
            display = Substitute.For<IDisplay>();
            cooker = Substitute.For<ICookController>();
            output = Substitute.For<IOutput>();

            door = new Door();
            light = new Light(output);

            userInterface = new UserInterface(
                powerButton, timeButton, startCancelButton,
                door,
                display,
                light,
                cooker);
        }

        [Test]
        public void DoorOpen_LightOne()
        {
            door.Open();
            output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void DoorClosed_LightOff()
        {
            door.Open();
            door.Close();
            output.Received(1).OutputLine("Light is turned off");
        }
    }
}