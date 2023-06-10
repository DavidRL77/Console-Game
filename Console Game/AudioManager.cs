using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace ConsoleGame
{
    public class AudioManager : IDisposable
    {
        private Dictionary<string, Channel> channels;

        public AudioManager(params string[] channelNames)
        {
            channels = new Dictionary<string, Channel>();
            foreach(string channel in channelNames)
            {
                channels.Add(channel, new Channel(null, 1));
            }
        }

        public void SetChannelVolume(string channelName, float volume)
        {
            channels[channelName].Volume = volume;
        }

        public void Play(string channelName, WaveBundle waveBundle)
        {

            Channel channel = channels[channelName];
            channel.Stop();
            channel.WaveBundle = waveBundle;
            channel.Play();
        }

        public void Pause(string channel)
        {
            channels[channel].Pause();
        }

        public void Resume(string channel)
        {
            channels[channel].Resume();
        }

        public void Stop(string channelName)
        {
            Channel channel = channels[channelName];
            channel.Stop();
        }

        public void Dispose()
        {
            foreach(Channel channel in channels.Values)
            {
                channel.Dispose();
            }
        }
    }

    public class WaveBundle : IDisposable
    {
        public WaveOut waveOut;
        public WaveStream waveStream;

        public float Volume { get => _volume; set => SetVolume(value); }
        private float _volume;

        public WaveBundle(WaveOut waveOut, WaveStream waveStream, float volume)
        {
            this.waveOut = waveOut;
            this.waveStream = waveStream;
            this.Volume = volume;

            this.waveOut.Init(waveStream);

        }

        public void SetVolume(float volume)
        {
            _volume = volume;
            waveOut.Volume = volume;
        }

        public void Dispose()
        {
            waveOut.Dispose();
            waveStream.Dispose();
        }
    }

    /// <summary>
    /// Handles stopping, playing, pausing, and volume of a wave bundle
    /// </summary>
    public class Channel : IDisposable
    {
        public WaveBundle WaveBundle { get => _waveBundle; set => SetWaveBundle(value); }
        private WaveBundle _waveBundle;

        public float Volume { get => _volume; set => SetVolume(value); }
        private float _volume;

        public PlaybackState playbackState => GetPlaybackState();

        public Channel(WaveBundle waveBundle, float volume)
        {
            this.WaveBundle = waveBundle;
            this.Volume = volume;
        }

        public void Stop()
        {
            if(WaveBundle == null) return;
            WaveBundle.waveOut.Stop();
        }

        public void Play()
        {
            if(WaveBundle == null) return;
            WaveBundle.waveStream.Position = 0;
            WaveBundle.waveOut.Play();
        }

        public void Pause()
        {
            if(WaveBundle == null) return;
            WaveBundle.waveOut.Pause();
        }

        public void Resume()
        {
            if(WaveBundle == null) return;
            WaveBundle.waveOut.Resume();
        }

        public void SetWaveBundle(WaveBundle waveBundle)
        {
            _waveBundle = waveBundle;
            if(WaveBundle != null) WaveBundle.waveOut.Volume = Volume * WaveBundle.Volume;
        }

        public void SetVolume(float volume)
        {
            _volume = volume;
            if(WaveBundle != null) WaveBundle.waveOut.Volume = Volume * WaveBundle.Volume;
        }

        public PlaybackState GetPlaybackState()
        {
            if(WaveBundle == null) return PlaybackState.Stopped;
            return WaveBundle.waveOut.PlaybackState;
        }

        public void Dispose()
        {
            WaveBundle.Dispose();
        }

    }
}
