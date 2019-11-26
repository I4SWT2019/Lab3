using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IntegrationStep4
    {
        private CookController cooker;

        private UserInterface userInterface;

        private PowerTube powerTube;
        private Light light;
        private Display display;

        private IOutput output;

        private ITimer timer;

        private Door door;

        private Button powerButton;
        private Button timeButton;
        private Button startCancelButton;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();

            timer = Substitute.For<ITimer>();

            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();

            door = new Door();

            powerTube = new PowerTube(output);
            light = new Light(output);
            display = new Display(output);

            cooker = new CookController(timer, display, powerTube);

            userInterface = new UserInterface(
                powerButton, timeButton, startCancelButton, door,
                display, light,
                cooker);

            cooker.UI = userInterface;
        }

        [Test]
        public void OnPowerPressed_OnTimePressed_OnStartCancelPressed_OutputContains7percent_OutputContainsClear_TimerReceived60()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("7 %")));
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("01:00")));
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("clear")));
            timer.Received(1).Start(60);
        }

        [Test]
        public void MicrowaveIsStarted_OnDoorOpen()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            door.Open();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("clear")));
            timer.Received(1).Stop();
        }

        [Test]
        public void MicrowaveIsStarted_OnStartCancelPressed()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            startCancelButton.Press();

            // Both the Light and the PowerTube is turned off
            output.Received(2).OutputLine(Arg.Is<string>(str => str.Contains("off")));
            // On startCancelButton.Press display is always cleared, is called twice
            output.Received(2).OutputLine(Arg.Is<string>(str => str.Contains("clear")));
            timer.Received(1).Stop();
        }

        [Test]
        public void MicrowaveIsStarted_TimerExpires()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            // Both the Light and the PowerTube is turned off 
            output.Received(2).OutputLine(Arg.Is<string>(str => str.Contains("off")));
            output.Received(2).OutputLine(Arg.Is<string>(str => str.Contains("clear")));
            
        }
    }
}