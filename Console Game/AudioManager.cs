using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    public class AudioManager : IDisposable
    {
        private Dictionary<string, WaveOut> channels;

        public AudioManager(Dictionary<string, WaveOut> channels)
        {
            this.channels = channels;
        }

        public AudioManager(params string[] channelNames)
        {
            channels = new Dictionary<string, WaveOut>();
            foreach(string channel in channelNames)
            {
                channels.Add(channel, new WaveOut());
            }
        }

        public void SetChannelVolume(string channel, float volume)
        {
            channels[channel].Volume = volume;
        }

        public void Play(string channel, WaveStream waveStream)
        {
            if(waveStream == null) return;

            WaveOut waveOut = channels[channel];
            Stop(channel);

            //Resets the stream
            waveStream.Position = 0;

            waveOut.Init(waveStream);
            waveOut.Play();
        }

        public void Play(string channel, WaveStream waveStream, float volume)
        {
            SetChannelVolume(channel, volume);
            Play(channel, waveStream);
        }

        public void Pause(string channel)
        {
            channels[channel].Pause();
        }

        public void Resume(string channel)
        {
            channels[channel].Resume();
        }

        public void Stop(string channel)
        {
            WaveOut waveOut = channels[channel];
            if(waveOut.PlaybackState == PlaybackState.Playing) waveOut.Stop();
        }

        public void Dispose()
        {
            foreach(WaveOut waveOut in channels.Values)
            {
                waveOut.Dispose();
            }
        }
    }
}
