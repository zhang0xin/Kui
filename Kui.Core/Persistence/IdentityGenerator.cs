using System;

namespace Kui.Core.Persistence
{
    public class IdentityGenerator
    {
        public static ulong NewGuid()
        {
            return BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 0);
        }
    }
}