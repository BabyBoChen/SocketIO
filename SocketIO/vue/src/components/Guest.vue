<template>
  <div ref="guestWrapper" class="guestWrapper" @click="guestClicked()">
    <label class="guest-label">暱稱：</label>
    <div class="guest-name">{{ this.$props.guestName }}</div>
    <label class="guest-label">連線ID：</label>
    <div class="guest-id">{{ this.$props.guestId }}</div>
  </div>
</template>

<script>
export default {
  name: "Guest",
  props: {
    guestName: String,
    guestId: String,
    isSelected: Boolean,
  },
  mounted() {
    if (this.$props.isSelected) {
      /** @type {HTMLDivElement} */
      let wrapper = this.$refs.guestWrapper;
      wrapper.style.backgroundColor = "rgba(107, 168, 50,0.2)";
    }
  },
  watch: {
    /** @param newVal {Boolean}
     * @param oldVal {Boolean}
     */
    isSelected: function (newVal, oldVal) {
      /** @type {HTMLDivElement} */
      let wrapper = this.$refs.guestWrapper;
      if (newVal) {
        wrapper.style.backgroundColor = "rgba(107, 168, 50,0.2)";
      } else {
        wrapper.style.backgroundColor = "";
      }
    },
  },
  data() {
    let model = {};
    return model;
  },
  methods: {
    guestClicked() {
      this.$emit("guestClicked", this.$props.guestId, this.$props.guestName);
    },
  },
};
</script>

<style scoped>
.guestWrapper {
  cursor: pointer;
  display: flex;
}

.guestWrapper * {
  cursor: pointer;
  margin: 5px;
}

.guestWrapper:hover {
  background-color: rgba(107, 168, 50, 0.2);
}
</style>