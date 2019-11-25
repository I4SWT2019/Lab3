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
        public void OnPowerPressed_OnStartPressed__LightOff()
        {
            powerButton.Press();
            startCancelButton.Press();

            output.Received(1).OutputLine("Light is turned off");
        }

        [Test]
        public void OnPowerPressed_OnTimePressed_OnStartPressed__LightOn()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void OnPowerPressed_OnTimePressed_OnStartPressed_OnCancelPressed__LightOff()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            startCancelButton.Press();

            output.Received(1).OutputLine("Light is turned off");
        }
    }
}