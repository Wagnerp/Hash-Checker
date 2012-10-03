using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crypto = System.Security.Cryptography;

namespace System.Security.Cryptography
{
    class CRC16Generator : Crypto.HashAlgorithm
    {
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            throw new NotImplementedException();
        }

        protected override byte[] HashFinal()
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
