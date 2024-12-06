import Vue from 'vue'
import VueRouter from "vue-router";
import NProgress from "nprogress";
import 'nprogress/nprogress.css'
import Cookies from "js-cookie";
import {Notification} from 'element-ui';
import constantRoutes from "@/router/constantRoutes ";
import store from "@/store";

NProgress.configure({showSpinner: false})

const createRouter=()=>
  new VueRouter({
    routes: constantRoutes,
    scrollBehavior: () => ({left: 0, top: 0}),
  });

const router = createRouter();
router.beforeEach( (to, from, next) => {
  NProgress.start() //进度条
  if (to.path === '/login' || to.path === '/404') {
    //1.不需要认证
    next()
  } else {
    //2.需要认证
    if (Cookies.get('admin-token')) {
      if (store.state.roleRouter.addRouters.length === 0) {
        store.dispatch('getUserInfo').then(res => {
          //拿到用户角色信息
          let {roleCode} = store.state.user.userInfo
          let roles = [roleCode]
          store.dispatch('GenerateRoutes', {roles}).then(res => {
            store.state.roleRouter.addRouters.forEach(item => {
              router.addRoute(item)
            })
            next({...to, replace: true})
          })
        })
      } else {
        next()
      }
    } else {
      next({path: '/login'})
      Notification({type: 'error', title: '警告', message: '请登陆...'})
    }
  }
})

router.afterEach(() => {
  NProgress.done()//进度条结束
})
//不重复跳转当前路由
const originalPush = VueRouter.prototype.push
VueRouter.prototype.push = function push(location) {
  return originalPush.call(this, location).catch(err => err)
}

//重置路由函数 export
export function resetRouter() {
  const newRouter = createRouter();
  router.matcher = newRouter.matcher; //替换成新的空路由
}

Vue.use(VueRouter)
export default router
