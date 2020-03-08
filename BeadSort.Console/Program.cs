using System;
using Iot.Device.Uln2003;

namespace BeadSort.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            const int bluePin = 4;
            const int pinkPin = 17;
            const int yellowPin = 27;
            const int orangePin = 22;

            using (Uln2003 motor = new Uln2003(bluePin, pinkPin, yellowPin, orangePin))
            {
                // // Set the motor speed to 15 revolutions per minute.
                // motor.RPM = 15;
                // // Set the motor mode.  
                // motor.Mode = StepperMode.HalfStep;
                // // The motor rotate 2048 steps clockwise (180 degrees for HalfStep mode).
                // motor.Step(2048);

                // motor.Mode = StepperMode.FullStepDualPhase;
                // motor.RPM = 8;
                // // The motor rotate 2048 steps counterclockwise (360 degrees for FullStepDualPhase mode).
                // motor.Step(-2048);

                // motor.Mode = StepperMode.HalfStep;
                // motor.RPM = 1;
                // motor.Step(4096);

                motor.RPM = 1;
                motor.Step(4);
            }
        }
    }
}
