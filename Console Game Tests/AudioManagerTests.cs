using ConsoleGame;
using NAudio.Wave;
using System.Diagnostics;

namespace Console_Game_Tests
{
    [TestClass]
    public class AudioManagerTests
    {
        Dictionary<string, WaveBundle> streams = new Dictionary<string, WaveBundle>()
            {
                {"white_space", new WaveBundle(new WaveOut(),new AudioFileReader(@"D:\Users\david\Music\OMORI\002 - WHITE SPACE.mp3"), 1)},
                {"sleepover", new WaveBundle(new WaveOut(),new AudioFileReader(@"D:\Users\david\Music\OMORI\003 - Lost At A Sleepover.mp3"), 1)},
                {"lower_sleepover", new WaveBundle(new WaveOut(),new AudioFileReader(@"D:\Users\david\Music\OMORI\003 - Lost At A Sleepover.mp3"), 0.5f) },
                {"angel", new WaveBundle(new WaveOut(),new AudioFileReader(@"D:\Users\david\Music\SFX\angel.mp3"), 1)}
            };

        Dictionary<string, WaveBundle> steps = new Dictionary<string, WaveBundle>()
        {
            {"0", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.001.wav"), 1) },
            {"1", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.002.wav"), 1) },
            {"2", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.003.wav"), 1) },
            {"3", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.004.wav"), 1) },
            {"4", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.005.wav"), 1) },
            {"5", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.006.wav"), 1) },
            {"6", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.007.wav"), 1) },
            {"7", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.008.wav"), 1) },
            {"8", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.009.wav"), 1) },
            {"9", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.010.wav"), 1) },
            {"10", new WaveBundle(new WaveOut(),new AudioFileReader(@"C:\Worlds\Test\assets\sound_files\steps\Concrete.011.wav"), 1) }
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
            audioManager.Play("sfx", streams["sleepover"]);
            Thread.Sleep(10000);

        }

        [TestMethod]
        public void PauseTest()
        {
            AudioManager audioManager = new AudioManager("sfx", "music");

            audioManager.Play("music", streams["white_space"]);
            Thread.Sleep(10000);
            audioManager.Pause("music");
            Thread.Sleep(5000);
            audioManager.Resume("music");
            Thread.Sleep(10000);
            audioManager.Play("music", streams["sleepover"]);
            Thread.Sleep(10000);
            audioManager.Pause("music");
            Thread.Sleep(5000);
            audioManager.Play("music", streams["white_space"]);
            Thread.Sleep(10000);
            audioManager.Pause("music");
            Thread.Sleep(5000);
            audioManager.Play("music", streams["white_space"]);
            Thread.Sleep(10000);
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
            audioManager.Play("music", streams["lower_sleepover"]);
            Thread.Sleep(10000);
            audioManager.SetChannelVolume("music", 1f);
            Thread.Sleep(10000);
            audioManager.Play("music", streams["sleepover"]);
            Thread.Sleep(10000);
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
                KeyValuePair<string, WaveBundle> kvp = steps.RandomElement();
                audioManager.Play("sfx", kvp.Value);

                int delay = Utils.rnd.Next(1000);
                Thread.Sleep(delay);
            }
        }
    }
}