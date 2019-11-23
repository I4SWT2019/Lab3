using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IntegrationStep2
    {
        private UserInterface userInterface;
        private Light light;
        private Door door;

        private Button powerButton;
        private Button timeButton;
        private Button startCancelButton;


        private IDisplay display;

        private ICookController cooker;

        private IOutput output;

        [SetUp]
        public void Setup()
        {
            cooker = Substitute.For<ICookController>();
            output = Substitute.For<IOutput>();
            display = Substitute.For<IDisplay>();

            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();

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
        public void SetPower__DoorOpened_LightOn()
        {
            powerButton.Press();
            door.Open();

            output.Received(1).OutputLine("Light is turned on");
        }
    }
}