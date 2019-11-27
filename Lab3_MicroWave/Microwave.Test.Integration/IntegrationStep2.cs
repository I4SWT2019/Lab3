using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    [Author("I4SWTGrp2")]
    public class IntegrationStep2
    {
        private UserInterface userInterface;

        private Door door;

        private Button powerButton;
        private Button timeButton;
        private Button startCancelButton;


        private Light light;
        private Display display;

        private ICookController cooker;

        private IOutput output;

        [SetUp]
        public void Setup()
        {
            cooker = Substitute.For<ICookController>();
            output = Substitute.For<IOutput>();

            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();

            door = new Door();

            light = new Light(output);
            display = new Display(output);

            userInterface = new UserInterface(
                powerButton, timeButton, startCancelButton,
                door,
                display,
                light,
                cooker);
        }

        [Test]
        public void OnPowerPressedOnce_OutputContains50()
        {
            powerButton.Press();
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("50")));
        }

        [Test]
        public void OnPowerPressedTwice_OutputContains100()
        {
            powerButton.Press();
            powerButton.Press();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("100")));
        }

        [Test]
        public void OnPowerPressed_PowerExceeds700GoesTo50_OutputContains50()
        {
            for (int i = 50; i <= 750; i += 50)
            {
                powerButton.Press();
            }

            output.Received(2).OutputLine("Display shows: 50 W");
        }

        [Test]
        public void OnTimePressedOnce_OutputContains0100()
        {
            powerButton.Press();
            timeButton.Press();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("01:00")));
        }

        [Test]
        public void OnTimePressedTwice_DisplayReceivedShowTime2_0()
        {
            powerButton.Press();
            timeButton.Press();
            timeButton.Press();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("02:00")));
        }

        [Test]
        public void OnPowerPress_OnTimePressed_OnStartCancelPressed_LightOn_CookerStartCooking()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
            cooker.Received(1).StartCooking(Arg.Is(50), Arg.Is(60));
        }

        // PowerPressedStartpressed
        // PowerPressedDoorOpen
        // PowerPressedTimePressedDoorOpen
    }
}