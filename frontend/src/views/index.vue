<template>
  <div class="container" id="container">
    <div class="top">
      <header class="mask-paper">
        <a class="logo-container">
          <img class="logo" src="@/assets/logo_neww.png" />
          <span class="brand-name">
            <span class="brand-primary">航游</span>
            <span class="brand-secondary">集市</span>
          </span>
        </a>
        <div class="input-box" v-show="$route.path === '/dashboard'">
          <div class="search-wrapper">
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
              <el-icon v-if="key === ''" class="search-icon" style = "margin-top: 43px;margin-left: -10px;"><Search /></el-icon>
              <el-icon v-else @click="clearKeyword" class="close-icon"><CircleClose /></el-icon>
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
          <li :class="$route.path === '/dashboard' ? 'active-channel' : ''">
            <a class="link-wrapper" @click="toDashboard()">
              <div
                style="width: 1.5em; height: 1.5em; margin-right: 8px;display: flex;align-items: center;justify-content: center">
                <el-icon size="large" color=#ff2442>
                  <ShoppingBag />
                </el-icon>
              </div>
              <span class="channel">推荐</span>
            </a>
          </li>
          <li :class="$route.path === '/followtrend' ? 'active-channel' : ''">
            <a class="link-wrapper" @click="toTrend()">
              <div
                style="width: 1.5em; height: 1.5em; margin-right: 8px;display: flex;align-items: center;justify-content: center">
                <el-icon size="large" color=#ff2442>
                  <Sunrise />
                </el-icon>
              </div>
              <span class="channel">
                动态</span>
            </a>
          </li>
          <li :class="$route.path === '/message' ? 'active-channel' : ''">
            <a class="link-wrapper" @click="toMessage()">
              <div
                style="width: 1.5em; height: 1.5em; margin-right: 8px;display: flex;align-items: center;justify-content: center">
                <el-badge v-if="noReadMessage" is-dot class="item">
                  <el-icon size="large" color=#ff2442>
                    <ChatLineRound />
                  </el-icon></el-badge>
                <i v-else class="el-icon-bell">
                  <el-icon size="large" color=#ff2442>
                    <ChatLineRound />
                  </el-icon>
                </i>
              </div>
              <span class="channel">消息</span>
            </a>
          </li>
          <li :class="$route.path === '/release' ? 'active-channel' : ''">
            <!-- <li :class="$route.path === '/publish'?'active-channel':''" v-if="loginStatus"> -->
            <a class="link-wrapper" @click="toPush()">
              <div
                style="width: 1.5em; height: 1.5em; margin-right: 8px;display: flex;align-items: center;justify-content: center">
                <el-icon size="large" color=#ff2442>
                  <Pointer />
                </el-icon>
              </div>
              <span class="channel">
                发布</span>
            </a>
          </li>
          <li :class="$route.path === '/my' ? 'active-channel' : ''">
            <!-- <li v-else :class="$route.path === '/my'?'active-channel':''"> -->
            <a class="link-wrapper" @click="toUser()">
              <!-- <el-image style="width: 1.5em; height: 1.5em; margin-right: 8px;border-radius: 100%"
                :src="$store.state.user.userInfo.avatar"></el-image> -->
              <!-- <el-image style="width: 1.5em; height: 1.5em; margin-right: 8px;border-radius: 100%"></el-image> -->
              <div
                style="width: 1.5em; height: 1.5em; margin-right: 8px;display: flex;align-items: center;justify-content: center">
                <el-icon size="large" color=#ff2442>
                  <User />
                </el-icon>
              </div>
              <span class="channel">
                我的</span>
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
                    <div class="menu-item hover-effect">
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
    <!-- <Login v-if="showLogin" @click-child="loginClose"></Login> -->
  </div>
</template>

<script>
import Login from "@/views/login.vue";
import Cookies from "js-cookie";
import user from "@/store/user";
import { Search } from "@element-plus/icons-vue";
import api from "@/api";
import emitter from '@/utils/eventBus'

export default {
  computed: {
    user() {
      return user
    }
  },
  components: { Login },
  data() {
    return {
      noReadMessage: false,
      moreStatus: false,
      showLogin: false,
      key: "",
      loginStatus: false,
      emitter: emitter
    }
  },
  created() {
    this.$api = api;
    //this.$eventBus.$on('noReadMessage', this.handleMessage);
    let token = Cookies.get('web-token')
    if (token) {
      this.showLogin = false
      this.loginStatus = true
    } else {
      this.showLogin = true
      this.loginStatus = false
    }
    this.getNoReadMessage()
    this.getNoReadMessageFlush()
  },
  methods: {
    clearKeyword() {
      this.key = ''
    },
    getNoReadMessageFlush() {
      this.timer = setInterval(() => {
        this.getNoReadMessage()
      }, 2000)
    },
    handleMessage(val) {
      if (val > 0) {
        this.noReadMessage = true
      } else {
        this.noReadMessage = false
      }
    },
    getNoReadMessage() {
      this.$api.chatList.getNoReadCount().then(res => {
        if (res.data > 0) {
          this.noReadMessage = true
        } else {
          this.noReadMessage = false
        }
      })
    },
    toDashboard() {
      this.$router.push("/dashboard")
    },
    toTrend() {
      this.$router.push("/followtrend")
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
    toggleLogin() {
      this.showLogin = !this.showLogin
      this.$router.push("/login")
    },
    changeInput() {
      this.searchPage()
    },
    focusInput() { },
    searchPage() {
      this.emitter.emit('keyChanged', this.key)
    },
    loginClose(val) {
      this.showLogin = false;
      this.loginStatus = val;
    },
    logout() {
      this.$api.user.logout().then(res => {
        this.$router.push("/login")
      })
    },
  },
  destroyed() {
    clearInterval(this.timer)
    //this.$eventBus.$off('noReadMessage', this.handleMessage);
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
      display: flex;
      width: 220px;
      flex-direction: column;
      justify-content: space-between;
      height: calc(100vh - 92px);
      margin-top: 72px;
      position: fixed;
      overflow: visible;
      padding: 24px 16px;
      margin-left: -16px;
      left: 24px;
      
      background: #fff;
      backdrop-filter: blur(8px);
      
      border: 1px solid rgba(0, 0, 0, 0.08);
      box-shadow: 
        0 4px 16px rgba(0, 0, 0, 0.06),
        0 1px 3px rgba(0, 0, 0, 0.02),
        inset 0 0 0 1px rgba(255, 255, 255, 0.5);
      
      border-radius: 16px;
      
      &::before {
        content: '';
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: #fff;
        border-radius: inherit;
        z-index: -1;
      }
      
      &::after {
        content: '';
        position: absolute;
        top: -1px;
        left: -1px;
        right: -1px;
        bottom: -1px;
        background: linear-gradient(135deg,
          rgba(0, 0, 0, 0.03),
          transparent 50%
        );
        border-radius: inherit;
        z-index: -2;
      }

      .channel-list {
        padding: 0 8px;
        
        li {
          margin-bottom: 12px;
          transition: all 0.3s ease;
          
          .link-wrapper {
            height: 48px;
            display: flex;
            align-items: center;
            padding: 0 12px;
            border-radius: 12px;
            transition: all 0.3s ease;
            
            .el-icon, 
            .el-badge,
            .el-image {
              flex-shrink: 0;
              width: 24px;
              height: 24px;
              margin-right: 12px;
              display: flex;
              align-items: center;
              justify-content: center;
            }
            
            .channel {
              font-size: 16px;
              line-height: 1.5;
              font-weight: 500;
              white-space: nowrap;
              background: linear-gradient(45deg, #666, #999);
              -webkit-background-clip: text;
              -webkit-text-fill-color: transparent;
              transition: all 0.3s ease;
            }
            
            &:hover {
              background: linear-gradient(45deg,
                rgba(255, 36, 66, 0.04),
                rgba(255, 107, 129, 0.03)
              );
              transform: translateX(4px);
            }
          }
          
          &.active-channel {
            .link-wrapper {
              background: linear-gradient(45deg,
                rgba(255, 36, 66, 0.08),
                rgba(255, 107, 129, 0.05)
              );
              box-shadow: 0 2px 8px rgba(255, 36, 66, 0.03);
              
              .channel {
                background: linear-gradient(45deg, #ff6b81, #ff8c9a);
                -webkit-background-clip: text;
                -webkit-text-fill-color: transparent;
                font-weight: 600;
              }
              
              .el-icon {
                transform: scale(1.1);
              }
            }
          }
        }
      }

      .information-container {
        margin-top: auto;
        padding: 16px 8px;
        border-top: 1px solid rgba(255, 36, 66, 0.04);
        position: relative;
        
        .information-pad {
          position: absolute;
          bottom: 100%;
          left: 0;
          width: 100%;
          padding: 8px;
          
          .container {
            background: rgba(255, 255, 255, 0.98);
            backdrop-filter: blur(8px);
            border-radius: 12px;
            border: 1px solid rgba(255, 36, 66, 0.04);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
            
            .group-wrapper {
              padding: 8px 0;
              
              .group-header {
                padding: 8px 16px;
                font-size: 14px;
                color: rgba(51, 51, 51, 0.6);
              }
              
              .menu-item {
                padding: 12px 16px;
                transition: all 0.3s ease;
                cursor: pointer;
                
                a {
                  display: flex;
                  align-items: center;
                  color: #333;
                  
                  i {
                    margin-right: 8px;
                  }
                }
                
                &:hover {
                  background: linear-gradient(45deg,
                    rgba(255, 36, 66, 0.04),
                    rgba(255, 107, 129, 0.03)
                  );
                }
              }
            }
          }
        }

        .information-wrapper {
          padding: 0 16px;
          height: 48px;
          display: flex;
          align-items: center;
          border-radius: 12px;
          cursor: pointer;
          transition: all 0.3s ease;
          
          &:hover {
            background: linear-gradient(45deg,
              rgba(255, 36, 66, 0.08),
              rgba(255, 107, 129, 0.06)
            );
          }
          
          i {
            width: 24px;
            height: 24px;
            margin-right: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
          }
          
          .channel {
            font-size: 16px;
            line-height: 1.5;
          }
        }
      }
    }

    .main-content {
      width: 100%;
      
      &.with-side-bar {
        margin-top: 20px;
        border-radius: 16px;
        background: transparent;
        margin-left: -26px;

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
}

.mask-paper {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  max-width: 1728px;
  height: 72px;
  padding: 0 24px;
  background: #fff;
  backdrop-filter: blur(10px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.02);
  transition: all 0.3s ease;
  border-bottom: 1px solid rgba(0, 0, 0, 0.05);
  
  &::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: linear-gradient(45deg, 
      rgba(255, 36, 66, 0.03),
      rgba(255, 107, 129, 0.02)
    );
    z-index: 0;
  }

  &::after {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    height: 1px;
    background: linear-gradient(90deg,
      rgba(255, 36, 66, 0),
      rgba(255, 36, 66, 0.1) 50%,
      rgba(255, 36, 66, 0)
    );
  }

  .logo-container,
  .input-box,
  .right {
    position: relative;
    z-index: 1;
  }

  .logo-container {
    display: flex;
    align-items: center;
    gap: 12px;
    
    .logo {
      width: 90px;
      height: 72px;
      object-fit: contain;
      transition: transform 0.3s ease;
      
      &:hover {
        transform: scale(1.05);
      }
    }

    .brand-name {
      font-size: 30px;
      font-weight: 700;
      letter-spacing: 2px;
      font-family: '微软雅黑';
      
      .brand-primary {
        color: rgb(51.2, 126.4, 204);
        background: linear-gradient(45deg, rgb(51.2, 126.4, 204), #4a90e2);
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
      }
      
      .brand-secondary {
        color: #ff2442;
        background: linear-gradient(45deg, #ff2442, #ff6b81);
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
      }
    }
  }

  .input-box {
    .search-wrapper {
      position: relative;
      width: 500px;
      height: 44px;
      
      .search-input {
        width: 100%;
        height: 100%;
        padding: 0 48px 0 20px;
        font-size: 16px;
        border: 2px solid transparent;
        border-radius: 22px;
        background: rgba(0, 0, 0, 0.02);
        transition: all 0.3s ease;
        
        &:focus {
          border-color: #ff2442;
          background: #fff;
          box-shadow: 0 0 0 4px rgba(255,36,66,0.05);
        }
        
        &::placeholder {
          color: rgba(0,0,0,0.4);
        }
      }

      .input-button {
        position: absolute;
        right: 12px;
        top: 50%;
        transform: translateY(-50%);
        display: flex;
        align-items: center;
        justify-content: center;
        width: 32px;
        height: 32px;
        border-radius: 50%;
        cursor: pointer;
        transition: all 0.3s ease;
        
        .el-icon {
          font-size: 20px;
          color: rgba(0,0,0,0.5);
          transition: all 0.3s ease;
          
          &:hover {
            color: #ff2442;
            transform: scale(1.1);
          }
        }
      }
    }
  }
}
</style>