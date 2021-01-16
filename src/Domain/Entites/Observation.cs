using System;

namespace Domain.Entites
{
    public class Observation
    {
        public Observation(DateTimeOffset timeStamp, decimal currentLoad)
        {
            TimeStamp = timeStamp;
            CurrentLoad = currentLoad;
        }

        public DateTimeOffset TimeStamp { get; set; }
        public decimal CurrentLoad { get; set; }
    }
}
