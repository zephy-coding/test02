﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AForgetest
{
    public interface INetworkChatCodec : IDisposable
    {
        string Name { get; }
        bool IsAvailable { get; }
        int BitsPerSecond { get; }
        WaveFormat RecordFormat { get; }
        byte[] Encode(byte[] data, int offset, int length);
        byte[] Decode(byte[] data, int offset, int length);
    }
}
