using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TeleprompterConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTeleprompter().Wait();
        }

        private static async Task ShowTeleprompter(TelePrompterConfig telePrompterConfig)
        {
            var words = ReadFrom("sampleQuotes.txt");
            foreach(var word in words)
            {
                Console.Write(word);
                if (!string.IsNullOrWhiteSpace(word))
                {
                    await Task.Delay(telePrompterConfig.DelayInMilliseconds);
                }
            }
            telePrompterConfig.SetDone();
        }

        private static async Task GetInput(TelePrompterConfig telePrompterConfig)
        {
            void work()
            {
                do
                {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == '>')
                    {
                        telePrompterConfig.UpdateDelay(-10);
                    }
                    else if (key.KeyChar == '<')
                    {
                        telePrompterConfig.UpdateDelay(10);
                    }
                    else if (key.KeyChar == 'X' || key.KeyChar == 'x')
                    {
                        telePrompterConfig.SetDone();
                    }
                } while (!telePrompterConfig.Done);
            }
            await Task.Run(work);
        }

        private static async Task RunTeleprompter()
        {
            var config = new TelePrompterConfig();
            var displayTask = ShowTeleprompter(config);

            var speedTask = GetInput(config);
            await Task.WhenAny(displayTask, speedTask);
        }

        static IEnumerable<string> ReadFrom(string file)
        {
            string line;
            using var reader = File.OpenText(file);
            while ((line = reader.ReadLine()) != null)
            {
                var words = line.Split(' ');
                var lineLenght = 0;
                foreach(var word in words)
                {
                    yield return word + " ";
                    lineLenght += word.Length + 1;
                    if(lineLenght > 70)
                    {
                        yield return Environment.NewLine;
                        lineLenght = 0;
                    }
                }
                yield return Environment.NewLine;
            }
        }
    }
}
