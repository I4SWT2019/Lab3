using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
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

        private Output output;

        private Timer timer;
    }
}