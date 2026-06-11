using System;

namespace TmsApi.Services
{
    public class TmsDatabaseException : Exception
    {
        public TmsDatabaseException(string message) : base(message) { }
    }
}