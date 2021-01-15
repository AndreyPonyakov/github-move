using System;

namespace Domain.Entites
{
    public class Observation
    {
        public Observation(DateTime timeStamp, decimal currentLoad)
        {
            TimeStamp = timeStamp;
            CurrentLoad = currentLoad;
        }

        public DateTime TimeStamp { get; set; }
        public decimal CurrentLoad { get; set; }
    }
}
