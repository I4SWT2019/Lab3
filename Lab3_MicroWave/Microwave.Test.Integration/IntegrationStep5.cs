using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microwave.Test.Unit;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using MicrowaveOvenClasses.Boundary;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    [Author("I4SWTGrp2")]
    class IntegrationStep5
    {
        private CookController cooker;
        private PowerTube powerTube;
        private Display display;

        private IOutput output;
        private IUserInterface userInterface;

        private Timer timer;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();

            powerTube = new PowerTube(output);
            display = new Display(output);

            userInterface = Substitute.For<IUserInterface>();

            timer = new Timer();

            cooker = new CookController(timer, display,powerTube, userInterface);
        }

        [Test]
        public void StartCooking_TimerReceived0001_DisplayShows0001()
        {
            cooker.StartCooking(50, 1);
            display.ShowTime(0,1);
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("00:01")));
        }

        [Test]
        public void StartCooking_TimerReceived0101_DisplayShows0101()
        {
            cooker.StartCooking(50, 61);
            display.ShowTime(1, 1);
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("01:01")));
        }

        [Test]
        public void StartCooking31Sec_Wait30Sec_DisplayShows0001()
        {
            cooker.StartCooking(50,31);
            System.Threading.Thread.Sleep(30100);
            output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("00:01")));
        }
    }
}
