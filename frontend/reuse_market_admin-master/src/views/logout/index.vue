<template>
</template>

<script>
import Cookies from "js-cookie";
import {resetRouter} from "@/router";
export default {
  mounted() {
    this.$api.user.logout().then(res=>{
      Cookies.remove("admin-token")
      sessionStorage.clear();
      // sessionStorage.removeItem("store")
      this.$store.commit("clearUserInfo")
      this.$store.commit("CLEAR_ROUTES")
      resetRouter()
      this.$router.push("/login")
    })
  }
}
</script>

<style scoped lang="scss">

</style>