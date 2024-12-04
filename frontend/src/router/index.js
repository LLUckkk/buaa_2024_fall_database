
import VueRouter from "vue-router";
import Vue from 'vue'
import user from "@/router/route";
import {Notification} from 'element-ui';
import Cookies from "js-cookie";
import store from "@/store";
const routes = [
  ...user,
]

const router = new VueRouter({
  // mode: 'history',
  scrollBehavior: () => ({left: 0, top: 0}),
  routes: routes
})


router.beforeEach((to, from, next) => {
  if (to.path === '/login' || to.path === '/404' ) {
    //1.不需要登录
    next()
  } else {
    //2.需要登录
    if (Cookies.get('web-token')) {
      //判断是否还有状态
      if(Object.keys(store.state.user.userInfo).length === 0){
        store.dispatch('getUserInfo').then(res=>{
          next()
        })
      }else{
        //已经登陆
        next()
      }
    } else {
      //未登录
      next({path: '/login'})
      Notification({type: 'error', title: '警告', message: '请登陆后再次尝试...'})
    }
  }
});

// const originalPush = VueRouter.prototype.publish
// VueRouter.prototype.publish = function publish(location) {
//   return originalPush.call(this, location).catch(err => err)
// }
Vue.use(VueRouter)
export default router

