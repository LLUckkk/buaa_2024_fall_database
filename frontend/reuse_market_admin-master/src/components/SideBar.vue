<template>
  <div>
    <el-scrollbar wrap-class="scrollbar-wrapper">
      <el-menu
          :default-active="$route.name"
          :collapse="false"
          :background-color="variables.menuBg"
          :text-color="variables.menuText"
          :unique-opened="false"
          :active-text-color="variables.menuActiveText"
          :collapse-transition="false"
          mode="vertical"
      >
        <sidebar-item v-for="route in routes"  :key="route.path" :item="route" />
      </el-menu>
    </el-scrollbar>
  </div>
</template>

<script>

import SidebarItem from './SideBarItem.vue'
import variables from '@/styles/variables.scss'
import {mapState} from "vuex";



export default {
  components: {SidebarItem},
  data() {
    return {
      defaultActive: ''
    }
  },

  computed: {
    ...mapState({
      routes: state => state.roleRouter.routes
    }),
    variables() {
      return variables
    },
  },

}
</script>
