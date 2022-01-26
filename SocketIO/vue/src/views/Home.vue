<template>
  <div class="wrapper">
    <form ref="frmLogin" @submit.prevent>
      <img src="flask-logo.png" />
      <h1>登入聊天室</h1>
      <label>暱稱：</label>
      <input
        type="text"
        name="guestName"
        placeholder="請輸入您的暱稱(上限20個英數或中文字)"
        required
        pattern="^[ 0-9A-Za-z\u3000\u3400-\u4DBF\u4E00-\u9FFF]{1,20}$"
      />
      <br />
      <label>通關密語：</label>
      <input type="password" name="password" placeholder="請輸入通關密語" required />
      <br />
      <button type="submit" class="btn btn-success" @click="login()">登入</button>
      <button type="reset" class="btn btn-warning">重填</button>
    </form>
  </div>
</template>

<script>
import endpoint from "@/router/apiEndpoints.js";

export default {
  name: "Home",
  methods: {
    async login() {
      /** @type {HTMLFormElement} */
      let frmLogin = this.$refs.frmLogin;
      if (frmLogin.checkValidity()) {
        let formData = new FormData(frmLogin);
        let isLogin = await fetch(endpoint.login, {
          method: "POST",
          body: formData,
        })
          .then(function (res) {
            return res.json();
          })
          .then(function (res) {
            console.log(res);
            return res["isLogin"];
          })
          .catch(function (err) {
            console.log(err);
            return false;
          });

        if (isLogin) {
          this.$router.push("/chatroom");
        } else {
          alert("請輸入正確的通關密語!");
        }
      }
    },
  },
};
</script>

<style scoped>
.wrapper {
  margin: auto;
  width: 100%;
  max-width: 700px;
  text-align: center;
}
img {
  margin: 20px;
  width: calc(100% - 40px);
  max-width: 450px;
}
input {
  width: 20rem;
  margin: 10px;
}

button {
  margin: 10px;
}
</style>
