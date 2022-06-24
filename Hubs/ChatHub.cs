﻿using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace PlantShop.Hubs
{
    public class ChatHub:Hub
    {
            public async Task SendMessage(string username, string massage)
            {
                await Clients.All.SendAsync("ReceiveMessage", username, massage);
            }
        
    }
}
