<template>
  <div>
    <a
      class="guestlist-toggler"
      data-bs-toggle="collapse"
      href="#guestList"
      role="button"
      aria-expanded="false"
      aria-controls="guestList"
    >
      <i class="fas fa-user green"></i> 在線訪客
      <i class="fas fa-angle-double-down"></i>
      <span>to: {{ this.$data.selectedName }}</span>
    </a>
    <div ref="guestList" class="collapse guestList" id="guestList">
      <Guest
        @guestClicked="guestClicked"
        v-for="guest in guests"
        :key="guest.guestId"
        :guest-name="guest.guestName"
        :guest-id="guest.guestId"
        :is-selected="guest.isSelected"
      />
    </div>
  </div>
</template>

<script>
import Guest from "@/components/Guest.vue";
export default {
  name: "GuessList",
  components: {
    Guest,
  },
  data() {
    let model = {
      selectedId: "-",
      selectedName: "所有人",
      guests: [
        {
          guestId: "-",
          guestName: "所有人",
          isSelected: true,
        },
      ],
    };
    return model;
  },
  mounted() {},

  methods: {
    /** @param guestId {String} @param guestName {String} */
    guestClicked(guestId, guestName) {
      this.$data.selectedName = guestName;
      this.$data.selectedId = guestId;
      console.log({guestName, guestId});
      this.$emit("guestClicked", guestId, guestName);
      this.$data.guests.forEach((guest) => {
        if (guest.guestId == guestId) {
          guest.isSelected = true;
        } else {
          guest.isSelected = false;
        }
      });
      /** @type {HTMLDivElement} */
      let guestList = this.$refs.guestList;
      guestList.classList.remove("show");
    },

    /** @param newGuests {Array} */
    refreshGuestList(newGuests) {
      this.$data.guests.splice(0);
      for (let i = 0; i < newGuests.length; i++) {
        this.$data.guests.push(newGuests[i]);
      }
    },
  },
};
</script>

<style scoped>
.guestlist-toggler {
  color: black;
  text-decoration: none;
  margin: 5px;
  display: block;
  width: calc(100% - 10px);
  border: 1px solid black;
}
.green {
  color: green;
}
.guestList {
  cursor: pointer;
  position: fixed;
  z-index: 2;
  margin: 5px;
  width: calc(100% - 10px);
  border: 1px solid black;
  background-color: white;
  max-height: 70vh;
  overflow: auto;
}
</style>