import constantRoutes from "@/router/constantRoutes ";
import addRoutes from "@/router/addRoutes";

export default {
  state: {
    routes: constantRoutes,
    addRouters: []
  },
  actions: {
    GenerateRoutes(context, data) {
      const {roles} = data
      return new Promise(resolve => {
        const accessedRouters = addRoutes.filter(route => {
          // if (roles.indexOf('admin') >= 0) return true;
          if (hasPermission(roles, route)) {
            if (route.children && route.children.length > 0) {
              route.children = route.children.filter(child => {
                if (hasPermission(roles, child)) {
                  return true
                }else{
                  return false;
                }
              });
              return route
            } else {
              return route
            }
          }
          return false;
        });
        context.commit('SET_ROUTERS', accessedRouters);
        resolve();
      })
    }
  },
  mutations: {
    SET_ROUTERS: (state, routers) => {
      state.addRouters = routers;
      state.routes = constantRoutes.concat(routers)
    },
    CLEAR_ROUTES: (state) => {
      state.addRouters = [];
      state.routes = constantRoutes
    },
  },
}

function hasPermission(roles, route) {
  if (route.meta && route.meta.role) {
    return roles.some(role => route.meta.role.indexOf(role) >= 0)
  } else {
    return true
  }
}