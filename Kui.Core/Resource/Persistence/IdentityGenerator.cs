using System;

namespace Kui.Core.Resource.Persistence
{
    public class IdentityGenerator
    {
        public static ulong NewGuid()
        {
            return BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 0);
        }
    }
}