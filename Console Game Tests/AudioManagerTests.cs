using ConsoleGame;
using NAudio.Wave;

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

        //[TestMethod]
        //public void LoopingTest()
        //{
        //    AudioManager audioManager = new AudioManager("sfx", "music");

        //    audioManager.Play("music", streams["angel"]);
        //}
    }
}