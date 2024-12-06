import Vue from 'vue'
import Vuex from 'vuex'
import user from "@/store/user";
import roleRouter from "@/store/roleRouter";


Vue.use(Vuex)

const store = new Vuex.Store({
  modules: {
    user,
    roleRouter,
  },
})

export default store
