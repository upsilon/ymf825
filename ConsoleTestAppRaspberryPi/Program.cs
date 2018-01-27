using Mono.Options;
using Pigpio;
using Pigpio.Api;
using Pigpio.IO;
using System;
using System.Threading;
using Ymf825;
using Ymf825.IO;

namespace ConsoleTestAppRaspberryPi
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = ParseOptions(args);
            if (options == null)
                return;

            Console.WriteLine("Image type: {0}bit", Environment.Is64BitProcess ? "64" : "32");

            Ymf825.Ymf825 ymf825 = null;

            switch (options.Value.Type)
            {
                case "pigpio":
                    {
                        var pigpioApi = new PigpioNativeApi();
                        var spiDevice = new PigpioSpi(new PigpioClient(pigpioApi));
                        ymf825 = new Ymf825Pigpio(spiDevice);
                    }
                    break;
                case "pigpiod":
                    {
                        var (_, hostname, port) = options.Value;
                        var pigpioApi = new PigpioSocketApi(new PigpioSocket(hostname, port));
                        var spiDevice = new PigpioSpi(new PigpioClient(pigpioApi));
                        ymf825 = new Ymf825Pigpio(spiDevice);
                    }
                    break;
                case "wiringpi":
                default:
                    {
                        var spiDevice = new WiringPiSpi();
                        ymf825 = new Ymf825WiringPi(spiDevice);
                    }
                    break;
            }

            var driver = new Ymf825Driver(ymf825);
            SamplePlay(driver);
        }

        private static (string Type, string Hostname, int Port)? ParseOptions(string[] args)
        {
            var type = (string)null;
            var hostname = "localhost";
            var port = 8888;
            var help = false;

            var options = new OptionSet
            {
                { "t|type=", "Specify connection type (wiringpi, pigpio, pigpiod) [required]", x => type = x },
                { "h|host=", "The hostname of pigpiod [default: localhost]", x => hostname = x },
                { "p|port=", "The port number for pigpiod connection [default: 8888]", (int x) => port = x },
                { "help", "Show this help message", _ => help = true },
            };

            try
            {
                options.Parse(args);
            }
            catch (OptionException ex)
            {
                Console.WriteLine($"{args[0]}: {ex.Message}");
                Console.WriteLine($"Try `{args[0]} --help' for more information.");
                return null;
            }

            if (type == null)
                help = true;

            if (help)
            {
                options.WriteOptionDescriptions(Console.Out);
                return null;
            }

            return (type, hostname, port);
        }

        private static void SamplePlay(Ymf825Driver driver)
        {
            driver.EnableSectionMode();

            Console.WriteLine("Software Reset");
            driver.ResetSoftware();

            {
                Console.WriteLine("Tone Init");
                var tones = new ToneParameterCollection { [0] = ToneParameter.GetSine() };

                driver.Section(() =>
                {
                    driver.WriteContentsData(tones, 0);
                    driver.SetSequencerSetting(SequencerSetting.AllKeyOff | SequencerSetting.AllMute | SequencerSetting.AllEgReset |
                                                SequencerSetting.R_FIFOR | SequencerSetting.R_SEQ | SequencerSetting.R_FIFO);
                }, 1);

                driver.Section(() =>
                {
                    driver.SetSequencerSetting(SequencerSetting.Reset);

                    driver.SetToneFlag(0, false, true, true);
                    driver.SetChannelVolume(31, true);
                    driver.SetVibratoModuration(0);
                    driver.SetFrequencyMultiplier(1, 0);
                });
            }

            var noteon = new Action<int>(key =>
            {
                Ymf825Driver.GetFnumAndBlock(key, out var fnum, out var block, out var correction);
                Ymf825Driver.ConvertForFrequencyMultiplier(correction, out var integer, out var fraction);
                var freq = Ymf825Driver.CalcFrequency(fnum, block);
                Console.WriteLine("key: {0}, freq: {4:f1} Hz, fnum: {5:f0}, block: {6}, correction: {1:f3}, integer: {2}, fraction: {3}", key, correction, integer, fraction, freq, fnum, block);

                driver.Section(() =>
                {
                    driver.SetVoiceNumber(0);
                    driver.SetVoiceVolume(15);
                    driver.SetFrequencyMultiplier(integer, fraction);
                    driver.SetFnumAndBlock((int)Math.Round(fnum), block);
                    driver.SetToneFlag(0, true, false, false);
                });
            });

            var noteoff = new Action(() =>
            {
                driver.Section(() => driver.SetToneFlag(0, false, false, false));
            });

            var index = 0;
            var score = new[]
            {
                60, 62, 64, 65, 67, 69, 71, 72,
                72, 74, 76, 77, 79, 81, 83, 84,
                84, 83, 81, 79, 77, 76, 74, 72,
                72, 71, 69, 67, 65, 64, 62, 60
            };
            while (true)
            {
                const int noteOnTime = 250;
                const int sleepTime = 0;

                noteon(score[index]);
                Thread.Sleep(noteOnTime);
                noteoff();

                Thread.Sleep(sleepTime);

                if (Console.KeyAvailable)
                    break;

                index++;
                if (index >= score.Length)
                    index = 0;
            }

            driver.ResetHardware();
        }
    }
}
