import Index from "@/views/index.vue";
import Logout from "@/views/logout/index.vue";


export default [
  {
    path: '/',
    component: Index,
    redirect: '/home',
    name:'home',
    children: [
      {
        path: '/home',
        name: 'home',
        component: () => import('@/views/dashboard/index'),
        meta: {
          title: '首页',
          icon: 'el-icon-s-platform',
        }
      }
    ]
  },
  //product
  {
    path: '/product',
    component: Index,
    redirect: '/product/product_list',
    name: 'product',
    meta: {title: '商品管理', icon: 'el-icon-goods', role: ['admin']},
    children: [
      {
        path: '/product/product_list',
        name: 'product_list',
        component: () => import('@/views/product/product_list.vue'),
        meta: {title: '商品列表', icon: 'el-icon-s-goods'}
      },
      {
        path: '/product/product_detail',
        name: 'product_list',
        component: () => import('@/views/product/product_detail.vue'),
        meta: {title: '商品详情'},
        hidden: true
      }
    ]
  },
  //order
  {
    path: '/order',
    component: Index,
    redirect: '/order/product_order_list',
    name: 'order',
    meta: {title: '订单管理', icon: 'el-icon-s-order', role: ['admin']},
    children: [
      {
        path: '/order/product_order_list',
        name: 'product_order_list',
        component: () => import('@/views/order/product_order_list.vue'),
        meta: {title: '商品订单', icon: 'el-icon-sell'}
      },
      {
        path: '/order/product_order_detail',
        name: 'product_order_list',
        component: () => import('@/views/order/product_order_detail.vue'),
        meta: {title: '商品订单详情'},
        hidden: true
      },
      {
        path: '/order/payment_order_list',
        name: 'payment_order_list',
        component: () => import('@/views/order/payment_order_list.vue'),
        meta: {title: '支付订单', icon: 'el-icon-bank-card'}
      },
      {
        path: '/order/payment_order_detail',
        name: 'payment_order_list',
        component: () => import('@/views/order/payment_order_detail.vue'),
        meta: {title: '支付订单详情'},
        hidden: true
      },
      {
        path: '/order/product_order_evaluate',
        name: 'product_order_evaluate',
        component: () => import('@/views/order/product_order_evaluate.vue'),
        meta: {title: '订单评价', icon: 'el-icon-s-release'}
      },
    ]
  },
  //user
  {
    path: '/user',
    component: Index,
    redirect: '/user/list',
    name: 'user',
    meta: {title: '用户管理', icon: 'el-icon-user',role: ['admin']},
    children: [
      {
        path: '/user/list',
        name: 'user',
        component: () => import('@/views/user/user_list.vue'),
        meta: {title: '用户列表', icon: 'el-icon-user'}
      },
      {
        path: '/user/check',
        name: 'user_check',
        component: () => import('@/views/user/user_check.vue'),
        meta: {title: '资料审核', icon: 'el-icon-folder-checked'}
      }
    ]
  },
  //comment
  {
    path: '/comment',
    component: Index,
    redirect: '/comment/list',
    name: 'comment',
    meta: {role: ['admin']},
    children: [
      {
        path: '/comment/list',
        name: 'comment',
        component: () => import('@/views/comment/comment_list.vue'),
        meta: {title: '评论管理', icon: 'el-icon-chat-line-square', role: ['admin']}
      }
    ]
  },
  //config
  {
    path: '/config',
    component: Index,
    redirect: '/config/user',
    name: 'config',
    meta: {title: '系统配置', icon: 'el-icon-s-operation', role: ['system']},
    children: [
      {
        path: '/config/user',
        name: 'Table',
        component: () => import('@/views/config/admin_user_list.vue'),
        meta: {title: '人员管理', icon: 'el-icon-user-solid', role: ['system']}
      },
      // {
      //   path: '/config/role',
      //   name: 'Tree',
      //   component: () => import('@/views/config/role_list.vue'),
      //   meta: {title: '角色管理', icon: 'el-icon-s-custom', role: ['system']}
      // }
      // ,
      {
        path: '/product/productType',
        name: 'ProductType',
        component: () => import('@/views/product/product_type_list.vue'),
        meta: {title: '商品分类', icon: 'el-icon-menu'}
      }
    ]
  },

  //nested
  // {
  //   path: '/nested',
  //   component: Index,
  //   redirect: '/nested/menu1',
  //   name: 'Nested',
  //   meta: {
  //     title: 'Nested',
  //     icon: 'nested'
  //   },
  //   children: [
  //     {
  //       path: '/nested/menu1',
  //       component: () => import('@/views/nested/menu1/index'),
  //       name: 'Menu1',
  //       meta: {title: 'Menu1'},
  //       children: [
  //         {
  //           path: '/nested/menu1/menu1-1',
  //           component: () => import('@/views/nested/menu1/menu1-1'),
  //           name: 'Menu1-1',
  //           meta: {title: 'Menu1-1'}
  //         },
  //         {
  //           path: '/nested/menu1/menu1-2',
  //           component: () => import('@/views/nested/menu1/menu1-2'),
  //           name: 'Menu1-2',
  //           meta: {title: 'Menu1-2'},
  //           children: [
  //             {
  //               path: '/nested/menu1/menu1-2/menu1-2-1',
  //               component: () => import('@/views/nested/menu1/menu1-2/menu1-2-1'),
  //               name: 'Menu1-2-1',
  //               meta: {title: 'Menu1-2-1'}
  //             },
  //             {
  //               path: '/nested/menu1/menu1-2/menu1-2-2',
  //               component: () => import('@/views/nested/menu1/menu1-2/menu1-2-2'),
  //               name: 'Menu1-2-2',
  //               meta: {title: 'Menu1-2-2'}
  //             }
  //           ]
  //         },
  //         {
  //           path: '/nested/menu1/menu1-3',
  //           component: () => import('@/views/nested/menu1/menu1-3'),
  //           name: 'Menu1-3',
  //           meta: {title: 'Menu1-3'}
  //         }
  //       ]
  //     },
  //     {
  //       path: '/nested/menu2',
  //       component: () => import('@/views/nested/menu2/index'),
  //       name: 'Menu2',
  //       meta: {title: 'menu2'}
  //     }
  //   ]
  // },
  {
    path: '/logout',
    name: 'logout',
    component: Logout,
    meta: {title: '退出登录', icon: 'el-icon-turn-off'}

  },

  //放在最后一个
  {path: '*', redirect: '/404', hidden: true}
]