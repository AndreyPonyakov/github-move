using System;
using Infrastructure.Interfaces;

namespace Application.Observers
{
    public class Observer
    {
        private IScrapper _scrapper;
        private IStorage _storage;

        public Observer(IScrapper scrapper, IStorage storage)
        {
            _scrapper = scrapper;
            _storage = storage;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}