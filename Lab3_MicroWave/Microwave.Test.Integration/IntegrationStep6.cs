using System;
using System.IO;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IntegrationStep6
    {
        private StringWriter _sWriter;

        private Door door;
        private Button powerButton;
        private Button timeButton;
        private Button startCancelButton;

        private UserInterface userInterface;
        private CookController cooker;

        private Light light;
        private PowerTube powerTube;
        private Display display;

        private Output output;

        private ITimer timer;

        private StringWriter _sw;

        [SetUp]
        public void setup()
        {
            timer = Substitute.For<ITimer>();

            door = new Door();
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();

            output = new Output();
            light = new Light(output);
            powerTube = new PowerTube(output);
            display = new Display(output);

            cooker = new CookController(timer, display, powerTube, userInterface);

            userInterface = new UserInterface(powerButton, timeButton, startCancelButton, 
                                door, display, light, cooker);

            _sw = Substitute.For<StringWriter>();

        }

        //6 tests, 5 extensions and 1 UC based

        [Test]
        public void NotingPressed_DidNotReceiveInput_NothingWritten()
        {
            Console.SetOut(_sw);

            _sw.DidNotReceive();
        }

        //UC 
        [Test]
        public void OnPowerPress_OnTimePress_OnStartCancelPress_Received()
        {
            Console.SetOut(_sw);

            door.Open();
            door.Close();
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            door.Open();
            door.Close();

            _sw.Received(9).WriteLine(Arg.Any<string>());
        }

        //Extension 1, start button pressed during Power setup
        [Test]
        public void OnPowerPress_OnStartCancelPress_Receive()
        {
            Console.SetOut(_sw);

            door.Open();
            door.Close();
            powerButton.Press();
            startCancelButton.Press();

            _sw.Received(4).WriteLine(Arg.Any<string>());
        }


        //Extension 2, door opened under power setup
        [Test]
        public void OnPowerPress_OnOpenDoor_Receive()
        {
            Console.SetOut(_sw);

            door.Open();
            door.Close();
            powerButton.Press();
            door.Open();

            _sw.Received(5).WriteLine(Arg.Any<string>());
        }

        //Extension 2, door opened under time setup
        [Test]
        public void OnPowerPress_OnTimePress_OnOpenDoor_Receive()
        {
            Console.SetOut(_sw);

            door.Open();
            door.Close();
            powerButton.Press();
            timeButton.Press();
            door.Open();

            _sw.Received(6).WriteLine(Arg.Any<string>());
        }

        //Extension 3, StartStop button pressed during cooking.
        [Test]
        public void OnPowerPress_OnTimePress_OnStartStopBtnPressTwice_Receive()
        {
            Console.SetOut(_sw);

            door.Open();
            door.Close();
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            startCancelButton.Press();

            _sw.Received(10).WriteLine(Arg.Any<string>());
        }

        //Extension 4, door opened under time setup
        [Test]
        public void OnPowerPress_OnTimePress_OnStartStopBtnPress_OnDoorOpen_Receive()
        {
            Console.SetOut(_sw);

            door.Open();
            door.Close();
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            door.Open();


            _sw.Received(8).WriteLine(Arg.Any<string>());
        }


    }
}