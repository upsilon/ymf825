﻿using System;
using Ymf825.IO;

namespace Ymf825
{
    public abstract class Ymf825 : IDisposable
    {
        public Ymf825Spi SpiInterface { get; }
        
        public bool IsDisposed { get; protected set; }

        protected Ymf825(int spiDeviceIndex)
        {
            SpiInterface = new Ymf825Spi(spiDeviceIndex, csPin);
        }

        public virtual void Write(byte address, byte data)
        {
            if (address >= 0x80)
                throw new ArgumentOutOfRangeException(nameof(address));

            SpiInterface.Write(address, data);
        }

        public virtual void BurstWrite(byte address, byte[] data, int offset, int count)
        {
            if (address >= 0x80)
                throw new ArgumentOutOfRangeException(nameof(address));

            SpiInterface.BurstWrite(address, data, offset, count);
        }

        public virtual byte Read(byte address)
        {
            if (address >= 0x80)
                throw new ArgumentOutOfRangeException(nameof(address));

            return SpiInterface.Read((byte)(address | 0x80));
        }

        public virtual void ResetHardware()
        {
            SpiInterface.ResetHardware();
        }

        public abstract void ChangeTargetDevice(TargetDevice target);

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                SpiInterface.Dispose();
            }

            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
