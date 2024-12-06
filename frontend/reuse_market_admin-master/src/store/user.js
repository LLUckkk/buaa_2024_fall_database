import user from '@/api/user'
export default {
  state: {
    userInfo: {
    }
  },
  actions: {
    getUserInfo(context) {
      return new Promise(resolve => {
        user.getUserInfo().then(res => {
          context.commit('setUserInfo', res.result)
          resolve()
        })
      })
    }
  },
  mutations: {
    setUserInfo(state, userInfo) {
      state.userInfo = userInfo
    },
    clearUserInfo(state){
      state.userInfo = {}

    }
  },
}


