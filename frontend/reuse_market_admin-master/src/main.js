import Vue from 'vue'

import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
Vue.use(ElementUI)


import '@/styles/index.scss' // global css
import App from './App'
import store from './store'
import router from './router'
import 'default-passive-events'

Vue.config.productionTip = false

import api from "@/api";
Vue.prototype.$api = api;

import utils from "@/utils/index"
Vue.prototype.$utils = utils;

new Vue({
  el: '#app',
  store,
  router,
  render: h => h(App)
})
