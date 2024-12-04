import { createRouter, createWebHashHistory } from 'vue-router'
import Recommend from '@/views/main/recommend.vue'
import Market from '@/views/main/market.vue'
import Login from '@/views/login.vue'
import Message from '@/views/message/message.vue'
import Release from '@/views/release/release.vue'
import My from '@/views/my/my.vue'

const routes = [
  {
    path: '/',
    name: 'login',
    component: Login,
  },
  {
    path: '/recommend',
    name: 'recommend',
    component: Recommend
  },
  {
    path: "/market",
    name: "market",
    component: Market
  },
  {
    path: "/message",
    name: "message",
    component: Message
  },
  {
    path: "/release",
    name: "release",
    component: Release
  },
  {
    path: "/my",
    name: "my",
    component: My
  }  //TODO:管理员页面实现
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router