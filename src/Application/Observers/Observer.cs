using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Domain.Entites;
using Infrastructure.Interfaces;
using NLog;

namespace Application.Observers
{
    public sealed class Observer : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IScrapper _scrapper;
        private IStorage _storage;
        private readonly double _intervalMinutes;
        private Timer _timer;
        private ConcurrentQueue<Observation> _observations = new ConcurrentQueue<Observation>();

        public Observer(IScrapper scrapper, IStorage storage, double intervalMinutes)
        {
            if (scrapper == null)
                throw new ArgumentNullException("scrapper");
            if (storage == null)
                throw new ArgumentNullException("storage");

            _intervalMinutes = intervalMinutes;
            _scrapper = scrapper;
            _storage = storage;
        }

        public void Dispose()
        {
            if (_storage != null)
            {
                _storage.Dispose();
                _storage = null;
            }

            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
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
            try
            {            
                Logger.Trace("Making an observation...");
                Logger.Trace(Thread.CurrentThread.ManagedThreadId);

                var observation = _scrapper.GetData();
                Logger.Info($"Recived an observation ({observation.TimeStamp.ToString("u")}, {observation.CurrentLoad})  from {_scrapper.Name}");

                _observations.Enqueue(observation);

                SaveResult();
            }
            catch(Exception ex)
            {
                Logger.Error(ex, "Making an observation failed");
            }
        }

        private void SaveResult()
        {
            var observations = GetObservationsFromConcurrentCollection();

            foreach (var o in observations)
            {
                try
                {
                    _storage.Save(o);
                    Logger.Trace($"Observation ({o.TimeStamp.ToString("u")}, {o.CurrentLoad}) was saved");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Observation saving failed");
                    _observations.Enqueue(o);
                }
            }
        }

        private IEnumerable<Observation> GetObservationsFromConcurrentCollection()
        {
            var observations = new List<Observation>();
            while (!_observations.IsEmpty)
            {
                Observation observation;
                if (_observations.TryDequeue(out observation))
                {
                    observations.Add(observation);
                }
            }

            return observations;
        }
    }
}