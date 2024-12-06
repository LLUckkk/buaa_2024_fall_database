import { createRouter, createWebHistory } from 'vue-router'
import Login from '../views/login.vue'
import recommend from '@/views/recommend/recommend.vue'
import market from '@/views/market/market.vue'
import my from '@/views/my/my.vue'
import message from '@/views/message/message.vue'
import release from '@/views/release/release.vue'

const routes = [
  {
    path: '/',
    redirect: '/login',
  },
  {
    path: '/login',
    name: 'login',
    component: Login,
  },
  {
    path: '/recommend',
    name: 'recommend', 
    component: recommend,
  },
  {
    path: '/market',
    name: 'market', 
    component: market,
  },
  {
    path: '/message',
    name: 'message', 
    component: message,
  },
  {
    path: '/my',
    name: 'my', 
    component: my,
  },
  {
    path: '/release',
    name: 'release',
    component: release
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
