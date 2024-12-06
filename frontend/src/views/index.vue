<template>
  <div class="container" id="container">
    <div class="top">
      <header class="mask-paper">
        <a style="display: flex">
          <img class="logo" style="width: 72px;height: 72px;margin-top: 20px" src="@/assets/logo1.png"/>
          <span style="line-height: 100px;font-weight: bolder;font-size: 30px;margin-left: 20px;letter-spacing: 4px">闲宝交易平台</span>
        </a>
        <div class="tool-box">
        </div>
        <div class="input-box" id="sujContainer" v-show="$route.path === '/dashboard'">
          <input
              type="text"
              v-model="key"
              class="search-input"
              placeholder="搜索你想要的"

              @focus="focusInput"
              @keyup.enter="searchPage"
              ref="SearchInput"
          />
          <div class="input-button">
            <div class="search-icon" v-if="key === ''">
              <i class="el-icon-search" style="width: 1em; height: 1em; margin-right: 8px;margin-top: 5px"></i>
            </div>
            <div class="close-icon" v-else @click="clearKeyword">
              <i class="el-icon-circle-close" style="width: 1em; height: 1em; margin-right: 8px;margin-top: 5px"></i>
            </div>
          </div>
        </div>
        <div class="right"></div>
      </header>
    </div>
    <!--    main left and container-->
    <div class="main">
      <div class="side-bar">
        <ul class="channel-list">
          <li :class="$route.path === '/dashboard'?'active-channel':''">
            <a class="link-wrapper" @click="toDashboard()">

              <div style="width: 1.5em; height: 1.5em; margin-right: 8px;display: flex;align-items: center;justify-content: center">
                <i class="el-icon-house"></i>
              </div>
              <span class="channel"
              >推荐</span
              ></a>
          </li>

          <li  :class="$route.path === '/followtrend'?'active-channel':''" v-if="loginStatus">
            <a class="link-wrapper" @click="toTrend()">
              <div style="width: 1.5em; height: 1.5em; margin-right: 8px;display: flex;align-items: center;justify-content: center">
                <i class="el-icon-star-off"></i>
              </div>
              <span class="channel">
              动态</span>
            </a>
          </li>
          <li :class="$route.path === '/notice'?'active-channel':''" v-if="loginStatus">
            <a class="link-wrapper" @click="toMessage()">
              <div style="width: 1.5em; height: 1.5em; margin-right: 8px;display: flex;align-items: center;justify-content: center">
                <el-badge v-if="noReadMessage" is-dot class="item"><i class="el-icon-bell"></i></el-badge>
                <i v-else class="el-icon-bell"></i>
              </div>
              <span class="channel">消息</span>
            </a>
          </li>
          <li :class="$route.path === '/publish'?'active-channel':''" v-if="loginStatus">
            <a class="link-wrapper" @click="toPush()">
              <div style="width: 1.5em; height: 1.5em; margin-right: 8px;display: flex;align-items: center;justify-content: center">
                <i class="el-icon-circle-plus-outline"></i>
              </div>

              <span class="channel">
              发布</span
              >
            </a>
          </li>
          <li style="background-color: #ff2e4d; border-radius: 999px;" v-if="!loginStatus">
            <a class="link-wrapper"   @click="toggleLogin()">
              <i class="el-icon-user" style="width: 1.5em; height: 1.5em; margin-right: 8px"></i>
              <span class="channel" style="color: white">
              登录</span
              >
            </a>
          </li>
          <li v-else :class="$route.path === '/user'?'active-channel':''">
            <a class="link-wrapper" @click="toUser">
              <el-image style="width: 1.5em; height: 1.5em; margin-right: 8px;border-radius: 100%" :src="$store.state.user.userInfo.avatar"></el-image>
<!--              <i class="el-icon-user" style="width: 1em; height: 1em; margin-right: 8px"></i>-->
              <span class="channel">
              我的</span
              >
            </a>
          </li>
        </ul>
        <!--user information-->
        <div class="information-container">
          <div class="information-pad" v-show="moreStatus">
            <div class="container">
              <div>
                <div>
                  <div class="group-wrapper">
                    <div class="group-header">设置</div>
<!--                    <div class="menu-item hover-effect">-->
<!--                      <span>深色模式</span>-->
<!--                      <div class="multistage-toggle component">-->
<!--                        <button class="toggle-item active">-->
<!--                          <div class="icon-wrapper">-->
<!--                          </div>-->
<!--                        </button>-->
<!--                      </div>-->
<!--                    </div>-->
                    <div class="menu-item hover-effect" >
                      <a @click="logout">
                        <i class="el-icon-turn-off"></i>
                        <span style="margin-left: 10px">退出登录</span>
                      </a>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="information-wrapper" @click="toggleMore()">
            <i class="el-icon-more" style="width: 1em; height: 1em; margin-right: 8px"></i>
            <span class="channel"> 更多</span>
          </div>
        </div>
      </div>
      <!-- main -->
      <div class="main-content with-side-bar">
        <router-view />
      </div>
    </div>
    <Login v-if="showLogin" @click-child="loginClose"></Login>
  </div>
</template>

<script>
import Login from "@/pages/login.vue";
import Cookies from "js-cookie";
import user from "@/store/user";
export default {
  computed: {
    user() {
      return user
    }
  },
  components: {Login},
  data(){
    return{
      noReadMessage:false,
      moreStatus:false,
      showLogin:false,
      key:"",
      loginStatus:false,
    }
  },
  created() {
    this.$eventBus.$on('noReadMessage', this.handleMessage);
    let token = Cookies.get('web-token')
    if(token){
      this.showLogin = false
      this.loginStatus = true
    }else{
      this.showLogin = true
      this.loginStatus = false
    }
    this.getNoReadMessage()
    this.getNoReadMessageFlush()
  },
  methods: {
    clearKeyword(){
      this.key =''
    },
    getNoReadMessageFlush() {
      this.timer = setInterval(() => {
        this.getNoReadMessage()
      }, 2000)
    },
    handleMessage(val){
      if(val > 0) {
        this.noReadMessage = true
      }else{
        this.noReadMessage = false
      }
    },
    getNoReadMessage(){
      this.$api.chatList.getNoReadCount().then(res=>{
        if(res.result > 0) {
          this.noReadMessage = true
        }else{
          this.noReadMessage = false
        }
      })
    },
    toDashboard() {
      this.$router.push("/dashboard")
    },
    toTrend() {
      this.$router.push("/market")
    },
    toMessage() {
      this.$router.push("/message")
    },
    toPush() {
      this.$router.push("/release")
    },
    toUser() {
      this.$router.push("/my")
    },
    toggleMore() {
      this.moreStatus = !this.moreStatus
    },
    toggleLogin(){
      this.showLogin = !this.showLogin
    },
    changeInput(){
      this.searchPage()
    },
    focusInput(){},
    searchPage(){
      this.$eventBus.$emit('keyChanged', this.key);
    },
    loginClose(val){
      this.showLogin = false;
      this.loginStatus = val;
    },
    logout(){
      this.$api.user.logout().then(res=>{
        Cookies.remove("web-token")
        sessionStorage.clear();
        this.$store.commit("clearUserInfo")
        this.$router.push("/login")
      })
    },
  },
  destroyed() {
    clearInterval(this.timer)
    this.$eventBus.$off('noReadMessage', this.handleMessage);
  }
}
</script>

<style lang="less" scoped>
a {
  text-decoration: none;
  color: rgba(51, 51, 51, 0.8);
}

.container {
  max-width: 1728px;
  background-color: #fff;
  margin: 0 auto;

  .top {
    display: flex;
    flex-direction: column;
    justify-content: center;
    width: 100vw;
    height: 72px;
    position: fixed;
    left: 0;
    top: 0;
    z-index: 10;
    align-items: center;
    background: #fff;

    header {
      position: relative;
      display: flex;
      align-items: center;
      justify-content: space-between;
      width: 100%;
      max-width: 1728px;
      height: 72px;
      padding: 0 16px 0 24px;
      z-index: 10;

      .tool-box {
        width: 24px;
        height: 70px;
        position: absolute;
        left: 5px;
        top: 0;
      }

      .input-box {
        height: 40px;
        position: fixed;
        left: 50%;
        transform: translate(-50%);

        @media screen and (max-width: 695px) {
          display: none;
        }

        @media screen and (min-width: 960px) and (max-width: 1191px) {
          width: calc(-36px + 50vw);
        }

        @media screen and (min-width: 1192px) and (max-width: 1423px) {
          width: calc(-33.6px + 40vw);
        }

        @media screen and (min-width: 1424px) and (max-width: 1727px) {
          width: calc(-42.66667px + 33.33333vw);
        }

        @media screen and (min-width: 1728px) {
          width: 533.33333px;
        }

        .search-input {
          padding: 0 84px 0 16px;
          width: 100%;
          height: 100%;
          font-size: 16px;
          line-height: 120%;
          color: #333;
          caret-color: #ff2442;
          background: rgba(0, 0, 0, 0.03);
          border-radius: 999px;
        }

        .input-button {
          position: absolute;
          right: 0;
          top: 0;
          display: flex;
          align-items: center;
          justify-content: center;
          height: 100%;
          color: rgba(51, 51, 51, 0.8);
          cursor: pointer;
          .close-icon .search-icon {
            width: 40px;
            height: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            color: rgba(51, 51, 51, 0.8);
          }
        }
      }
    }
  }

  .main {
    display: flex;

    .side-bar {
      @media screen and (max-width: 695px) {
        display: none;
      }
      @media screen and (min-width: 696px) and (max-width: 959px) {
        display: none;
      }

      @media screen and (min-width: 960px) and (max-width: 1191px) {
        width: calc(-18px + 25vw);
        margin-left: 12px;
      }

      @media screen and (min-width: 1192px) and (max-width: 1423px) {
        width: calc(-16.8px + 20vw);
        margin-left: 12px;
      }

      @media screen and (min-width: 1424px) and (max-width: 1727px) {
        width: calc(-21.33333px + 16.66667vw);
        margin-left: 16px;
      }

      @media screen and (min-width: 1728px) {
        width: 266.66667px;
        margin-left: 16px;
      }

      height: calc(100vh - 72px);
      overflow-y: scroll;
      background-color: #fff;
      display: flex;
      flex-direction: column;
      flex-shrink: 0;
      padding-top: 16px;
      margin-top: 72px;
      position: fixed;
      overflow: visible;


      .channel-list {
        min-height: auto;
        -webkit-user-select: none;
        user-select: none;

        .active-channel {
          background-color: rgba(0, 0, 0, 0.03);
          border-radius: 999px;
          color: #333;
        }

        li:hover {
          background-color: rgba(0, 0, 0, 0.03);
          border-radius: 999px;
          color: #333;
        }

        li {
          padding-left: 16px;
          min-height: 48px;
          display: flex;
          align-items: center;
          cursor: pointer;
          margin-bottom: 8px;
          color: rgba(51, 51, 51, 0.6);

          .link-wrapper {
            display: flex;
            width: 100%;
            height: 48px;
            align-items: center;
          }

          .message-count {
            margin-left: 7rem;
            background-color: red;
            width: 20px;
            height: 20px;
            font-size: 14px;
            line-height: 20px;
            text-align: center;
            border-radius: 50%;
            color: #fff;
          }
        }

        .channel {
          font-size: 17px;
          font-weight: 600;
          margin-left: 12px;
          color: #333;
        }
      }

      .information-container {
        display: inline-block;
        width: 70%;
        color: #333;
        font-size: 16px;
        position: absolute;
        bottom: 0;

        .information-pad {
          z-index: 16;
          margin-bottom: 4px;
          width: 100%;

          .container {
            width: 100%;
            background: #fff;
            box-shadow: 0 4px 32px 0 rgba(0, 0, 0, 0.08), 0 1px 4px 0 rgba(0, 0, 0, 0.04);
            border-radius: 12px;

            .divider {
              margin: 0px 12px;
              list-style: none;
              height: 0;
              border: 1px solid rgba(0, 0, 0, 0.08);
              border-width: 1px 0 0;
            }

            .group-wrapper {
              padding: 4px;

              .group-header {
                display: flex;
                align-items: center;
                padding: 0 12px;
                font-weight: 400;
                height: 32px;
                color: rgba(51, 51, 51, 0.6);
                font-size: 12px;
              }

              .menu-item {
                height: 40px;
                color: rgba(51, 51, 51, 0.8);
                font-size: 16px;
                border-radius: 8px;
                display: flex;
                align-items: center;
                padding: 0 12px;
                font-weight: 400;
                cursor: pointer;
                .icon {
                  color: rgba(51, 51, 51, 0.3);
                  margin-left: auto;
                }

                .component {
                  margin-left: auto;
                }

                .multistage-toggle {
                  position: relative;
                  background: rgba(0, 0, 0, 0.03);
                  display: flex();
                  padding: 2px;
                  border-radius: 999px;
                  cursor: pointer;

                  .active {
                    background: #fff;
                    box-shadow: 0 2px 8px 0 rgba(0, 0, 0, 0.04),
                    0 1px 2px 0 rgba(0, 0, 0, 0.02);
                    color: #333;
                  }

                  .toggle-item {
                    border-radius: 999px;
                    background: transparent;
                    color: rgba(51, 51, 51, 0.6);

                    .icon-wrapper {
                      width: 24px;
                      height: 24px;
                      display: flex;
                      align-items: center;
                      justify-content: center;
                      cursor: pointer;
                    }
                  }
                }
              }
              .menu-item:hover{
                background-color: rgba(0, 0, 0, 0.03);
                border-radius: 999px;
                color: #333;
              }
            }
          }
        }

        .information-wrapper {
          -webkit-user-select: none;
          user-select: none;
          cursor: pointer;
          position: relative;
          margin-bottom: 20px;
          height: 48px;
          width: 100%;
          display: flex;
          font-weight: 600;
          align-items: center;
          border-radius: 999px;
        }
      }
    }

    .main-content {
      width: 100%;
    }

    .main-content {
      @media screen and (min-width: 960px) and (max-width: 1191px) {
        padding-left: calc(-6px + 25vw);
      }

      @media screen and (min-width: 1192px) and (max-width: 1423px) {
        padding-left: calc(-4.8px + 20vw);
      }

      @media screen and (min-width: 1424px) and (max-width: 1727px) {
        padding-left: calc(-5.33333px + 16.66667vw);
      }

      @media screen and (min-width: 1728px) {
        padding-left: 282.66667px;
      }
    }
  }
}
</style>