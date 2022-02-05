using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;
using System.Collections.Concurrent;

namespace SocketIO.Hubs
{
    public class ChatHub : Hub
    {
        //public static List<Guest> Users = new List<Guest>()
        //{
        //    Guest.All,
        //};
        public static ConcurrentDictionary<string, Guest> Users { get; set; } = new ConcurrentDictionary<string, Guest>
        (
            new Dictionary<string, Guest>() 
            { 
                {"-", Guest.All }
            }
        );
        public static object _lock = new object();
        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync().Wait();
            string guestId = Context.ConnectionId;
            string guestName = (from c in Context.GetHttpContext().User.Claims
                                where c.Type == ClaimTypes.Name
                                select c).FirstOrDefault()?.Value;
            string notice = $"{guestName} 進入聊天室!";
            Guest g = new Guest();
            g.GuestId = guestId;
            g.GuestName = guestName;
            Users[guestId] = g;
            this.RefreshGuestList(notice);
            return Task.CompletedTask;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string notice = "";
            Guest removingGuest = new Guest();
            if (Users.TryRemove(Context.ConnectionId, out removingGuest))
            {
                notice = $"{removingGuest.GuestName} 離開聊天室!";
            }
            this.RefreshGuestList(notice);
            return base.OnDisconnectedAsync(exception);
        }
        public Task RefreshGuestList(string notice)
        {
            List<Guest> users = new List<Guest>();
            foreach (var u in Users)
            {
                users.Add(u.Value);
            }
            var pa = new { users, notice };
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
