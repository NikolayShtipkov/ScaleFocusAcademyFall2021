using System;
using UserAuthApp.BLL.Interfaces;

namespace UserAuthApp.BLL.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
