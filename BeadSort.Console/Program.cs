using System;
using Iot.Device.Uln2003;
using Iot.Device.Ws28xx;
using System.Device.Spi;
using System.Drawing;
using Iot.Device.Graphics;
using System.Collections.Generic;
using System.Linq;
using MMALSharp;
using MMALSharp.Handlers;
using MMALSharp.Common;
using System.Threading.Tasks;

namespace BeadSort.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Starting");
            const int lightCount = 12;
            var settings = new SpiConnectionSettings(0,0){
                ClockFrequency = 2_400_000, 
                Mode = SpiMode.Mode0, 
                DataBitLength = 8
            };

            // Create a Neo Pixel x8 stick on spi 0.0
            var spi = SpiDevice.Create(settings);

            var neo = new Ws2812b(spi, lightCount);

            var img = neo.Image;
            
            img.Clear();
            for(var x=0; x<lightCount; x++){
                img.SetPixel(x, 0, Color.White);
            }
            neo.Update();
            
            MMALCamera cam = MMALCamera.Instance;
            for(var y=0;y<50;y++){
                using (var imgCaptureHandler = new ImageStreamCaptureHandler($"_testing{y}.jpg")){
                    await cam.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
                }
                Step();
            }

            img.Clear();
            neo.Update();
            cam.Cleanup();

        }

        static void Img(string imgName){
            var r = new Dictionary<int,int>();
            var g = new Dictionary<int,int>();
            var b = new Dictionary<int,int>();
            var bm = new Bitmap(imgName);
            for(var x = 0; x < bm.Width; x++){
                for(var y = 0; y<bm.Height; y++){
                    var c = bm.GetPixel(x,y);
                    var t = (int)c.R;
                    if(r.ContainsKey(t)){
                        r[t]++;
                    }
                    else {
                        r.Add(t, 1);
                    }
                    t = (int)c.G;
                    if(g.ContainsKey(t)){
                        g[t]++;
                    }
                    else {
                        g.Add(t, 1);
                    }
                    t = (int)c.B;
                    if(b.ContainsKey(t)){
                        b[t]++;
                    }
                    else {
                        b.Add(t, 1);
                    }
                }
            }
            foreach(var z in r.OrderByDescending(o=>o.Value).Take(10)) {
                System.Console.WriteLine($"r{z.Key}:{z.Value}");
            }
            foreach(var z in g.OrderByDescending(o=>o.Value).Take(10)){
                System.Console.WriteLine($"g{z.Key}:{z.Value}");
            }
            foreach(var z in b.OrderByDescending(o=>o.Value).Take(10)){
                System.Console.WriteLine($"b{z.Key}:{z.Value}");
            }
        }

        static void Flash(){
            const int lightCount = 12;
            var settings = new SpiConnectionSettings(0,0){
                ClockFrequency = 2_400_000, 
                Mode = SpiMode.Mode0, 
                DataBitLength = 8
            };

            
            // Create a Neo Pixel x8 stick on spi 0.0
            var spi = SpiDevice.Create(settings);

            var neo = new Ws2812b(spi, lightCount);

            // Display basic colors for 5 sec
            var img = neo.Image;
            
            img.Clear();
            for(var x=0; x<lightCount; x++){
            System.Console.WriteLine("White: " + x.ToString());
                img.SetPixel(x, 0, Color.White);
            }
            neo.Update();
            
            System.Threading.Thread.Sleep(10000);
            img.Clear();
            neo.Update();

        }

        static void Step(){
            
            const int bluePin = 4;
            const int pinkPin = 17;
            const int yellowPin = 27;
            const int orangePin = 22;

            using (Uln2003 motor = new Uln2003(bluePin, pinkPin, yellowPin, orangePin))
            {
                // Set the motor speed to 15 revolutions per minute.
                motor.RPM = 12;
                // Set the motor mode.  
                motor.Mode = StepperMode.HalfStep;
                // The motor rotate 2048 steps clockwise (180 degrees for HalfStep mode).
                motor.Step(4096/8);
                // motor.Mode = StepperMode.FullStepDualPhase;
                // motor.RPM = 8;
                // // The motor rotate 2048 steps counterclockwise (360 degrees for FullStepDualPhase mode).
                // motor.Step(-2048);

                // motor.Mode = StepperMode.HalfStep;
                // motor.RPM = 1;
                // motor.Step(4096);

                // motor.RPM = 1;
                // motor.Step(4);
            }
        }
    }
}
