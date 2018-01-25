using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ymf825.IO
{
    public interface ISpi : IDisposable
    {
        bool IsDisposed { get; }

        /// <summary>
        /// CS ピンを有効にする際の対象となる、ピン位置を表す整数値を設定します。
        /// </summary>
        /// <param name="pin">ピン位置を表す整数値。有効範囲は 0x08 から 0xf8 です。</param>
        void SetCsTargetPin(byte pin);

        /// <summary>
        /// デバイスの送信キューをフラッシュし、コマンドを即時に実行します。
        /// </summary>
        void Flush();

        /// <summary>
        /// アドレスとデータを書き込むコマンドを送信キューに追加します。
        /// </summary>
        /// <param name="address">アドレスを表す 1 バイトの整数値。</param>
        /// <param name="data">データを表す 1 バイトの整数値。</param>
        void Write(byte address, byte data);

        /// <summary>
        /// アドレスと可変長のデータを書き込むコマンドを送信キューに追加します。
        /// </summary>
        /// <param name="address">アドレスを表す 1 バイトの整数値。</param>
        /// <param name="data">データが格納されている <see cref="byte"/> 型の配列。</param>
        /// <param name="offset">配列を読み出しを開始するオフセット値。</param>
        /// <param name="count">配列から読み出すバイト数。</param>
        void BurstWrite(byte address, byte[] data, int offset, int count);

        /// <summary>
        /// アドレスを指定して SPI デバイスから 1 バイトを読み出します。
        /// このコマンドは即時に実行されます。
        /// </summary>
        /// <param name="address">アドレスを表す 1 バイトの整数値。</param>
        /// <returns>SPI デバイスから返却されたデータ。</returns>
        byte Read(byte address);

        /// <summary>
        /// YMF825 をハードウェアリセットします。
        /// この命令は即時に実行されます。
        /// </summary>
        void ResetHardware();
    }
}
