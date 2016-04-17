```cs 
using Twitchiedll.IRC.Events;
```

# OnRawMessage
- Called on IRC query message, event or PRIVMSG
```cs
void OnRawMessage(string RawMessage) { }
```

# OnMessage
- Called when someone sends PRIVMSG
```cs
void OnMessage(MessageEventArgs e) { }
```

# OnPing
- Called when buffer contains PING
- About every 5 minutes Twitch server will send you **PING :tmi.twitch.tv** and you should send back **PONG :tmi.twitch.tv** so connection keeps alive.
```cs
void OnPing(string RawMessage) { }
```

# OnDisconnect
- Called when you disconnect
```cs
void OnDisconnect(DisconnectEventArgs e) { }
```

# OnRoomState
- Called when joining a channel and every time one of the chat room settings, like slow mode, change.
```cs
void OnRoomState(RoomStateEventArgs e) { }
```

# OnMode
- Called when someone gained or lost operator
```cs
void OnMode(ModeEventArgs e) { }
```

# OnNames
- Called when joining server, contains all users on channel, unless there is more than 1,000 then only list of moderators on channel.
```cs
void OnNames(NamesEventArgs e) { }
```

# OnJoin
- Called when someone or you join channel
```cs
void OnJoin(JoinEventArgs e) { }
```

# OnPart
- Called when someone or you leaves channel
```cs
void OnPart(PartEventArgs e) { }
```

# OnNotice
- Called when received notices from the server - could be about state change (example. slowmode enabled, or you someone getting banned **REKT**)
```cs
void OnNotice(NoticeEventArgs e) { }
```

# OnSubscribe
- Called when someone resubscribes or subscribes on channel.
```cs
void OnSubscribe(SubscriberEventArgs e) { }
```

# OnHostTarget
- Called when host started or stopped.
```cs
void OnHostTarget(HostTargetEventArgs e) { }
```
