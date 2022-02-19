# SignalR聊天室

採用Core MVC框架製做的Web即時聊天室。前端UI使用Vue.js撰寫，並使用SignalR技術來達成前後端的雙向呼叫。為了確保「在線訪客」清單可以具有thread safe的效果，本站使用一個靜態的ConcurrentDictionary物件來管理所有的在線訪客。

網站已上架至微軟的Azure雲端平台。

註：原先採用Python Flask的SocketIO模組撰寫，但因為Python Anywhere平台不支援Web Socket以及該平台的高網路延池，故改採用SignalR技術撰寫。

https://bbljchatroom.azurewebsites.net/
