﻿//using NHLStats.Core.Data;
//using NHLStats.Core.Models;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace NHLStats.Data.InMemory
//{
//    public class PlayerRepository : IPlayerRepository
//    {
//        private readonly List<Player> _players = new List<Player> {
//            new Player() { Id = 1, Name = "Connor McDavid" }
//        };

//        public Task<Player> Get(int id)
//        {
//            return Task.FromResult(_players.FirstOrDefault(p => p.Id == id));
//        }

//        public Task<Player> GetByName(string name)
//        {
//            return Task.FromResult(_players.FirstOrDefault(p => p.Name == name));
//        }

//        public Task<Player> GetByDynamic(object name)
//        {
//            return Task.FromResult(_players.FirstOrDefault(p => p.Name == name));
//        }

//        public Task<Player> GetRandom()
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task<List<Player>> All()
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task<Player> Add(Player player)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
