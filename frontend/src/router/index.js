import { createRouter, createWebHistory } from 'vue-router'
import Login from '../views/login.vue'
import followtrend from '@/views/followtrend/followtrend.vue'
import my from '@/views/my/my.vue'
import message from '@/views/message/message.vue'
import release from '@/views/release/release.vue'
import dashboard from '@/views/dashboard/dashboard.vue'
import index from '@/views/index.vue'
//临时添加
import main from '@/views/main/main.vue'
import orderCreate from '@/views/order/order-create.vue'
import orderDetail from '@/views/order/order-detail.vue'
import orderEvaluate from '@/views/order/order-evaluate.vue'
import paymentPay from '@/views/order/payment-pay.vue'
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
    path: '/main',
    name: 'main',
    component: main,
  },
  {
    path: '/dashboard',
    name: 'dashboard',
    component: dashboard,
  },
  {
    path: "/index",
    name: "index",
    component: index,
    redirect: "/dashboard",
    children: [
      {
        path: "/dashboard",
        name: "dashboard",
        component: dashboard,
      },
      {
        path: "/followTrend",
        name:"followtrend",
        component: followtrend,
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
      },
      {
        path: '/orderCreate',
        name: 'orderCreate',
        component: orderCreate
      },
      {
        path: '/orderDetail',
        name: 'orderDetail',
        component: orderDetail
      },
      {
        path: '/orderDetail',
        name: 'orderDetail',
        component: orderDetail
      },
      {
        path: '/orderEvaluate',
        name: 'orderEvaluate',
        component: orderEvaluate
      },
      {
        path: '/payentPay',
        name: 'paymentPay',
        component: paymentPay
      }
    ]
  },
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
