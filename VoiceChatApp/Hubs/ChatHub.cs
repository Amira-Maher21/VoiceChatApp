using Microsoft.AspNetCore.SignalR;
using VoiceChatApp.Service;

namespace VoiceChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly CallLogService _callLogService;
        private static Dictionary<string, string> userNames = new();

        public ChatHub(CallLogService callLogService)
        {
            _callLogService = callLogService;
        }

        public async Task<int> StartCallLog(string targetConnectionId)
        {
            var caller = userNames[Context.ConnectionId];
            var receiver = userNames[targetConnectionId];
            var room = userRooms[Context.ConnectionId];
            return await _callLogService.StartCallAsync(caller, receiver, room);
        }

        public async Task EndCallLog(int callId)
        {
            await _callLogService.EndCallAsync(callId);
        }


        private static Dictionary<string, string> userRooms = new();

        public async Task JoinRoom(string username, string room)
        {
            userNames[Context.ConnectionId] = username;
            userRooms[Context.ConnectionId] = room;

            await Groups.AddToGroupAsync(Context.ConnectionId, room);

            await Clients.Group(room).SendAsync("UserJoined", Context.ConnectionId, username);
            await SendUserList(room);
        }


        private async Task SendUserList(string room)
        {
            var usersInRoom = userRooms
                .Where(u => u.Value == room)
                .Select(u => new
                {
                    ConnectionId = u.Key,
                    Username = userNames.ContainsKey(u.Key) ? userNames[u.Key] : "غير معروف"
                })
                .ToList();

            await Clients.Group(room).SendAsync("UserList", usersInRoom);
        }


        public async Task SendSignal(string targetConnectionId, string type, object data)
        {
            await Clients.Client(targetConnectionId).SendAsync("ReceiveSignal", Context.ConnectionId, type, data);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (userRooms.TryGetValue(Context.ConnectionId, out string room))
            {
                userNames.Remove(Context.ConnectionId);
                userRooms.Remove(Context.ConnectionId);

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
                await Clients.Group(room).SendAsync("UserLeft", Context.ConnectionId);
                await SendUserList(room);
            }

            await base.OnDisconnectedAsync(exception);
        }








    }
}



