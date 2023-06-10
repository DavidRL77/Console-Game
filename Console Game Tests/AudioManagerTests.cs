using ConsoleGame;
using NAudio.Wave;
using System.Diagnostics;

namespace Console_Game_Tests
{
    [TestClass]
    public class AudioManagerTests
    {
        Dictionary<string, WaveStream> streams = new Dictionary<string, WaveStream>()
            {
                {"white_space", new AudioFileReader(@"D:\Users\david\Music\OMORI\002 - WHITE SPACE.mp3")},
                {"sleepover", new AudioFileReader(@"D:\Users\david\Music\OMORI\003 - Lost At A Sleepover.mp3")},
                {"angel", new AudioFileReader(@"D:\Users\david\Music\SFX\angel.mp3")}
            };

        Dictionary<string, WaveStream> steps = new Dictionary<string, WaveStream>()
        {
            {"0", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.001.wav") },
            {"1", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.002.wav") },
            {"2", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.003.wav") },
            {"3", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.004.wav") },
            {"4", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.005.wav") },
            {"5", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.006.wav") },
            {"6", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.007.wav") },
            {"7", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.008.wav") },
            {"8", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.009.wav") },
            {"9", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.010.wav") },
            {"10", new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.011.wav") }
        };

        [TestMethod]
        public void MultipleSoundsTest()
        {
            AudioManager audioManager = new AudioManager("sfx", "music");
            
            audioManager.Play("music", streams["white_space"]);
            Thread.Sleep(10000);
            audioManager.Play("music", streams["white_space"]);
            Thread.Sleep(10000);
            audioManager.Play("music", streams["sleepover"]);
            Thread.Sleep(10000);
            audioManager.Play("sfx", streams["angel"]);
            Thread.Sleep(100);
            audioManager.Play("sfx", streams["angel"]);
            Thread.Sleep(100);
            audioManager.Play("sfx", streams["angel"]);
            Thread.Sleep(100);

        }

        [TestMethod]
        public void VolumeTest()
        {
            AudioManager audioManager = new AudioManager("sfx", "music");

            audioManager.Play("music", streams["white_space"]);
            Thread.Sleep(10000);
            audioManager.SetChannelVolume("music", 0.1f);
            Thread.Sleep(10000);
            audioManager.Play("music", streams["sleepover"]);
            Thread.Sleep(10000);
            audioManager.SetChannelVolume("music", 1f);
            Thread.Sleep(1000);
        }

        [TestMethod]
        public void FreezingBugTest()
        {
            AudioManager audioManager = new AudioManager("sfx", "music");

            for(int i = 0; i < 200; i++)
            {
                audioManager.Play("sfx", streams["angel"]);
                Thread.Sleep(5);
            }

            for(int i = 0; i < 77; i++)
            {
                KeyValuePair<string, WaveStream> kvp = steps.RandomElement();
                audioManager.Play("sfx", kvp.Value);

                int delay = Utils.rnd.Next(1000);
                Thread.Sleep(delay);
            }
        }
    }
}