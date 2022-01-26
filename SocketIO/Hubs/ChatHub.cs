using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;

namespace SocketIO.Hubs
{
    public class ChatHub : Hub
    {
        public static List<Guest> Users = new List<Guest>()
        {
            Guest.All,
        };
        public static object _lock = new object();
        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync().Wait();
            string guestId = Context.ConnectionId;
            string guestName = (from c in Context.GetHttpContext().User.Claims
                                where c.Type == ClaimTypes.Name
                                select c).FirstOrDefault()?.Value;
            string notice = $"{guestName} 進入聊天室!";
            lock (_lock)
            {
                Users.Add(new Guest(guestId, guestName));
            }
            this.RefreshGuestList(notice);
            return Task.CompletedTask;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string notice = "";
            lock (_lock)
            {
                Guest user = (from u in Users
                              where u.GuestId == Context.ConnectionId
                              select u).FirstOrDefault();
                Users.Remove(user);
                notice = $"{user.GuestName} 離開聊天室!";
            }
            this.RefreshGuestList(notice);
            return base.OnDisconnectedAsync(exception);
        }
        public Task RefreshGuestList(string notice)
        {
            var pa = new { Users, notice };
            Task t = Clients.All.SendAsync("guestList", pa);
            return t;
        }
        public Task Send(ChatMessage msg)
        {
            Task t = null;
            if (msg.ReceiverId.Trim() == "-")
            {
                t = Clients.All.SendAsync("incomming", msg);
            }
            else
            {
                t = Clients.Client(msg.ReceiverId).SendAsync("incomming", msg);
                Clients.Client(msg.GuestId).SendAsync("incomming", msg);
            }
            return t;
        }
    }

    public class Guest
    {
        public static Guest All 
        { 
            get
            {
                return new Guest() { GuestId = "-", GuestName = "所有人" };
            }
        }
        public string GuestId { get; set; }
        public string GuestName { get; set; }
        public bool IsSelected { get; set; } = false;
        public Guest()
        {

        }
        public Guest(string guestId, string guestName)
        {
            this.GuestId = guestId;
            this.GuestName = guestName;
        }

    }

    public class ChatMessage
    {
        public string MsgType { get; set; }
        public string GuestId { get; set; }
        public string GuestName { get; set; }
        public string Message { get; set; }
        public string ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public long SendTime { get; set; }
    }
}
