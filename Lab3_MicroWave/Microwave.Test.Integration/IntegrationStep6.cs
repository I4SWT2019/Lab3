using System;
using System.IO;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IntegrationStep6
    {

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
        public void Setup()
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

            cooker.UI =userInterface;

            _sw = Substitute.For<StringWriter>();

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
            timer.Expired += Raise.EventWith(this, EventArgs.Empty);
            door.Open();
            door.Close();


            _sw.Received(3).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
            _sw.Received(3).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display shows: 50 W")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display shows: 01:00")));
            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Display cleared")));
            _sw.Received(3).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("PowerTube works with 7 %")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));
            _sw.Received(3).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Display cleared")));
            _sw.Received(3).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
            _sw.Received(3).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
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

            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display shows: 50 W")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display cleared")));
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

            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display shows: 50 W")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display cleared")));
            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
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


            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display shows: 50 W")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display shows: 01:00")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display cleared")));
            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
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

            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display shows: 50 W")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display shows: 01:00")));
            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Display cleared")));
            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("PowerTube works with 7 %")));
            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Display cleared")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));
            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
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


            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display shows: 50 W")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display shows: 01:00")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("Display cleared")));
            _sw.Received(2).WriteLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("PowerTube works with 7 %")));
            _sw.Received(1).WriteLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));
        }


    }
}