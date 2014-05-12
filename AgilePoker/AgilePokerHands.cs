using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AgilePoker.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace AgilePoker
{
    public class AgilePokerHands
    {
        private readonly static Lazy<AgilePokerHands> _instance = new Lazy<AgilePokerHands>(() => new AgilePokerHands(GlobalHost.ConnectionManager.GetHubContext<AgilePokerHub>().Clients));
        private readonly ConcurrentDictionary<AgilePokerTablePlayer, AgilePokerHand> _pokerHands = new ConcurrentDictionary<AgilePokerTablePlayer, AgilePokerHand>();

        private AgilePokerHands(IHubConnectionContext clients)
        {
            Clients = clients;

            _pokerHands.Clear();
            var pokerHands = GetPokerHandsFromCache();
            pokerHands.ForEach(x => _pokerHands.TryAdd(new AgilePokerTablePlayer
                { PlayerUniqueName = x.UniqueName, TableName = x.TableName }, x));
        }

        private List<AgilePokerHand> GetPokerHandsFromCache()
        {
            
            if (HttpRuntime.Cache["AgilePokerTables"] != null)
            {
                var tables =
                    JsonConvert.DeserializeObject<List<AgilePokerTable>>(
                        HttpRuntime.Cache["AgilePokerTables"].ToString());
                return tables.SelectMany(t => t.Players.Select(x => new AgilePokerHand
                    {
                        Hand = x.SelectedCard,
                        UserPreferredName = x.PreferredName,
                        UniqueName = x.UniqueName,
                        TableName = t.TableName
                    })).ToList();
            }
            return new List<AgilePokerHand>();
        }

        public static AgilePokerHands Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public IEnumerable<AgilePokerHand> GetAllHands(string tableName)
        {
            return _pokerHands.Values.Where(x => x.TableName == tableName);
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }

        private void BroadcastPokerHands(string tableName)
        {
            Clients.All.broadcastPokerHands(GetAllHands(tableName));
        }
    }
}