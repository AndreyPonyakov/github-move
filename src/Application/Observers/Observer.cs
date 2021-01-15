using System;
using System.Collections.Concurrent;
using System.Threading;
using Domain.Entites;
using Infrastructure.Interfaces;
using NLog;

namespace Application.Observers
{
    public class Observer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IScrapper _scrapper;
        private IStorage _storage;
        private readonly double _intervalMinutes;
        private Timer _timer;
        private ConcurrentQueue<Observation> _observations = new ConcurrentQueue<Observation>();

        public Observer(IScrapper scrapper, IStorage storage, double intervalMinutes)
        {
            _intervalMinutes = intervalMinutes;
            _scrapper = scrapper;
            _storage = storage;
        }

        public void Run()
        {
            if (_timer == null)
            {
                _timer = new Timer(
                e => { 
                    MakeObservation();
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(_intervalMinutes));
            }
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        private void MakeObservation()
        {            
            Logger.Trace("Making an observation...");
            
            // ToDo if an event is raised before
            // the previous event is finished processing, the second
            // event should be ignored.

            decimal value = _scrapper.GetData();
            Logger.Trace("Recived data {0} from {1}", value, _scrapper.Name);
            var observation = new Observation(DateTime.Now, value);

            _observations.Enqueue(observation);

            SaveResult();
        }

        private void SaveResult()
        {
            while (!_observations.IsEmpty)
            {
                Observation observation;
                if (_observations.TryDequeue(out observation))
                {
                    _storage.Save(observation);
                }                
            }
        }
    }
}