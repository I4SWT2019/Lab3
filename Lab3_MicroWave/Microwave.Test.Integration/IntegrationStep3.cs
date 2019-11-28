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
    public class IntegrationStep3
    {
        private CookController cooker;

        private ITimer timer;

        private Display display;
        private PowerTube powerTube;

        private IUserInterface userInterface;

        private IOutput output;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();

            timer = Substitute.For<ITimer>();

            powerTube = new PowerTube(output);
            display = new Display(output);

            cooker = new CookController(timer, display, powerTube);
            // Normally userinterface will use cooker here
            userInterface = Substitute.For<IUserInterface>();
            // Userinterface dependency would then be added to cooker
            cooker.UI = userInterface;

        }

        [Test]
        public void StartCooking_OutputContains7Percent_TimerReceived60()
        {
            cooker.StartCooking(50,60);

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains($"7 %")));
            timer.Received(1).Start(60);
        }

        [Test]
        public void StartCooking_OutputContains100percent_TimerReceived300()
        {
            cooker.StartCooking(700,300);

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains($"100 %")));
            timer.Received(1).Start(300);
        }

        [Test]
        public void Stop_OutputContainsOff_TimerReceivedStop()
        {
            cooker.StartCooking(50,60);
            cooker.Stop();

            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
            timer.Received(1).Stop();
        }
    }
}