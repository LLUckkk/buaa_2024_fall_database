<template>
  <div class="app-wrapper">
    <SideBar class="sidebar-container"/>
    <div class="main-container">
      <navbar/>
      <app-main/>
    </div>
  </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import AppMain from "@/components/AppMain.vue";
import SideBar from "@/components/SideBar.vue";
export default {
  name: 'Index',
  components: {AppMain, Navbar, SideBar},
}
</script>

<style lang="scss" scoped>
@import "~@/styles/mixin";
@import "~@/styles/variables";

.app-wrapper {
  @include clearfix;
  position: relative;
  height: 100%;
  width: 100%;

  &.mobile.openSidebar {
    position: fixed;
    top: 0;
  }
}

.drawer-bg {
  background: #000;
  opacity: 0.3;
  width: 100%;
  top: 0;
  height: 100%;
  position: absolute;
  z-index: 999;
}

.fixed-header {
  position: fixed;
  top: 0;
  right: 0;
  z-index: 9;
  width: calc(100% - #{$sideBarWidth});
  transition: width 0.28s;
}

.hideSidebar .fixed-header {
  width: calc(100% - 54px)
}

.mobile .fixed-header {
  width: 100%;
}
</style>
