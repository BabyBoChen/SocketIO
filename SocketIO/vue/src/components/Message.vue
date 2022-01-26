<template>
  <div v-if="this.$props.msgType=='message'" class="message">
      <span><b>{{this.$props.guestName}}</b>對-{{this.$props.receiverName}}-說: </span>
      <span>{{this.$props.message}}</span>
  </div>
  <div v-else-if="this.$props.msgType=='notice'" class="notice">
      <span>{{this.$props.message}}</span>
  </div>
</template>

<script>
export default {
    name:"Message",
    props:{
        msgType:String,
        guestId:String,
        guestName:String,
        receiverId:String,
        receiverName:String,
        message:String,
        sendTime:String,
    },
    data(){
        let model = {

        };
        return model;
    },
}
</script>

<style>
.message{
    font-size: large;
}
.notice{
    font-size: large;
}
</style>