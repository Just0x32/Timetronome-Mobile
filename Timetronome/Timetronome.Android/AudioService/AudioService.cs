using Android.Content.Res;
using Android.Media;
using System;
using System.IO;
using System.Threading;
using Timetronome.Droid.Services;
using Timetronome.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioService))]

namespace Timetronome.Droid.Services
{
    internal class AudioService : IAudio
    {
        AudioTrack audioTrack;
        private byte[] audioBuffer;

        public void Initialize(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
                throw new ArgumentNullException(nameof(assetName));

            int bufferSize = AudioTrack.GetMinBufferSize(44100, ChannelOut.Stereo, Encoding.Pcm16bit);

            AssetManager assets = Android.App.Application.Context.Assets;

            using (BinaryReader binaryReader = new BinaryReader(assets.Open(assetName)))
            {
                audioBuffer = binaryReader.ReadBytes(bufferSize);
            }

            audioTrack = new AudioTrack.Builder()
                .SetAudioAttributes(new AudioAttributes.Builder()
                    .SetUsage(AudioUsageKind.Media)
                    .SetContentType(AudioContentType.Music)
                    .Build())
                .SetAudioFormat(new AudioFormat.Builder()
                    .SetEncoding(Encoding.Pcm16bit)
                    .SetSampleRate(44100)
                    .SetChannelMask(ChannelOut.Stereo)
                    .Build())
                .SetBufferSizeInBytes(audioBuffer.Length)
                .Build();

            audioTrack.Play();
        }

        public void PlayTrack()
        {
            audioTrack.Write(audioBuffer, 0, audioBuffer.Length);
            Thread.Sleep(100);
            audioTrack.Release();
            audioTrack.Dispose();

            audioTrack = new AudioTrack.Builder()
                .SetAudioAttributes(new AudioAttributes.Builder()
                    .SetUsage(AudioUsageKind.Media)
                    .SetContentType(AudioContentType.Music)
                    .Build())
                .SetAudioFormat(new AudioFormat.Builder()
                    .SetEncoding(Encoding.Pcm16bit)
                    .SetSampleRate(44100)
                    .SetChannelMask(ChannelOut.Stereo)
                    .Build())
                .SetBufferSizeInBytes(audioBuffer.Length)
                .Build();

            audioTrack.Play();
        }
    }
}