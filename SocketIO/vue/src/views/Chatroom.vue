<template>
  <GuestList ref="guestList" @guestClicked="guestClicked" />
  <div ref="messageWrapper" class="message-wrapper">
    <Message
      v-for="msg in messages"
      :key="msg.sendTime"
      :guest-id="msg.guestId"
      :guest-name="msg.guestName"
      :message="msg.message"
      :receiverId="msg.receiverId"
      :receiverName="msg.receiverName"
      :msg-type="msg.msgType"
    />
  </div>
  <InputBox class="inputbox" @submit="onSend" :place-holder="this.$data.guestName" />
</template>

<script>
//<img alt="Vue logo" src="../assets/logo.png">
// @ is an alias to /src
import GuestList from "@/components/GuestList.vue";
import Message from "@/components/Message.vue";
import InputBox from "@/components/InputBox.vue";
import endpoint from "@/router/apiEndpoints.js";

export default {
  name: "Chatroom",
  data() {
    let model = {
      socket: null,
      guestId: "-",
      guestName: "unknown",
      selectedId: "-",
      selectedName: "所有人",
      messages: [
        // {
        //   msgType: "notice" || "message",
        //   guestId: "",
        //   guestName: "",
        //   receiverId: "",
        //   receiverName: "",
        //   message: "",
        //   sendTime: Date.now(),
        // }
      ],
      guests: [
        {
          guestId: "-",
          guestName: "所有人",
          isSelected: false,
        },
      ],
    };
    return model;
  },
  components: {
    InputBox,
    GuestList,
    Message,
  },
  async mounted() {
    //避開this關鍵字
    let chatroom = this;
    let model = this.$data;
    let guestList = this.$refs.guestList;

    //後端檢查訪客是否有登入資訊(session cookie)
    let isLogin = await fetch(endpoint.verify, {
      method: "GET",
    })
      .then(function (res) {
        return res.json();
      })
      .then(function (res) {
        model.guestName = res["guestName"];
        return res["isLogin"];
      })
      .catch(function (err) {
        console.log(err);
        return false;
      });

    //如果沒有登入資訊，跳轉到首頁
    if (!isLogin) {
      this.$router.push("/");
    } else {
      //建立signalR連線物件
      model.socket = new signalR.HubConnectionBuilder().withUrl(endpoint.chatHub).build();
      //註冊server端傳送的訪客清單事件
      model.socket.on("guestList",function(json){
        //清空訪客清單
        model.guests.splice(0);
        //重新加入所有訪客
        json["users"].forEach((user) => {
          model.guests.push(user);
        });
        //更新guestList元件
        guestList.refreshGuestList(model.guests);
        let notice = {
          msgType: "notice",
          guestId: "",
          guestName: "",
          receiverId: "",
          receiverName: "",
          message: json["notice"],
          sendTime: Date.now(),
        };
        //顯示訪客登入或登出的通知
        model.messages.push(notice);
      });
      //註冊server端傳送訊息事件
      model.socket.on("incomming",function(json){
        model.messages.push(json);
      });
      //開始連線
      model.socket.start()
        .then(function () {
          console.log("connecting to websocket...");
          //取得connectionId
          model.guestId = model.socket.connection.connectionId;
          //確保訪客確實斷線
          window.addEventListener("beforeunload", function () {
            try{
              model.socket.stop();
            } catch(ex) {}
          });
        })
        .catch(function (err) {
            return console.error(err.toString());
        });
    }
  },

  updated() {
    /** @type {HTMLDivElement} */
    let messageWrapper = this.$refs.messageWrapper;
    messageWrapper.scrollTop = messageWrapper.scrollHeight;
  },

  beforeUnmount() {
    try{
      this.$data.socket.stop();
    } catch(ex) {

    }
  },
  methods: {
    onSend(message) {
      let sendingMsg = {
        msgType: "message",
        guestId: this.$data.guestId,
        guestName: this.$data.guestName,
        message: message,
        receiverId: this.$data.selectedId,
        receiverName: this.$data.selectedName,
        sendTime: Date.now(),
      };
      this.$data.socket.send("send", sendingMsg);
    },
    /** @param guestId {String} @param guestName {String} */
    guestClicked(guestId, guestName) {
      this.$data.selectedId = guestId;
      this.$data.selectedName = guestName;
    },
  },
};
</script>

<style scoped>
.message-wrapper {
  margin: 5px;
  width: calc(100% - 10px);
  overflow: auto;
  height: calc(90vh - 4.75rem - 10px);
  max-height: calc(90vh - 4.75rem - 10px);
}
.inputbox {
  margin: 5px;
  width: calc(100% - 10px);
}
</style>




//如果有登入資訊，開始websocket連線
// this.$data.socket = io();
// this.$data.socket.on("connect", function () {
//   console.log("connecting to websocket...");
//   //取得connectionId
//   model.guestId = this.id;
//   //監聽server端傳送的訪客清單事件
//   model.socket.on("guestList", function (json) {
//     //清空訪客清單
//     model.guests.splice(0);
//     let all = {
//       guestId: "-",
//       guestName: "所有人",
//       isSelected: false,
//     };
//     //重新加入所有訪客
//     model.guests.push(all);
//     json["users"].forEach((user) => {
//       model.guests.push(user);
//     });
//     //更新guestList元件
//     guestList.refreshGuestList(model.guests);
//     let notice = {
//       msgType: "notice",
//       guestId: "",
//       guestName: "",
//       receiverId: "",
//       receiverName: "",
//       message: json["notice"],
//       sendTime: Date.now(),
//     };
//     //顯示訪客登入或登出的通知
//     model.messages.push(notice);
//   });
//   //監聽server端傳送的訊息事件
//   model.socket.on("incomming", function (json) {
//     model.messages.push(json);
//   });
// });

// //確保訪客確實斷線
// window.addEventListener("beforeunload", function () {
//   model.socket.disconnect();
// });