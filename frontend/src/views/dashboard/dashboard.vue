<!--主页，推荐页-->
<template>
  <div class="feeds-page">
    <!-- 添加全屏loading -->
    <div class="fullscreen-loading" v-if="skeleton">
      <div class="loading-spinner">
        <div class="bounce1"></div>
        <div class="bounce2"></div>
        <div class="bounce3"></div>
      </div>
    </div>

    <!--top-->
    <div class="channel-container">
      <div class="scroll-container channel-scroll-container">
        <div class="content-container">
          <div class="channel-wrapper">
            <div class="channel" 
                 v-for="item in menuList"
                 :key="item.typeCode"
                 :class="{'active': activeMenu === item.typeCode}"
                 @click="changeMenu(item)">
              <span class="channel-text">{{ item.typeName }}</span>
              <div class="active-bar" v-show="activeMenu === item.typeCode"></div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!--loading-->
    <div class="loading-container"></div>
    <!-- feed container -->
    <div class="feeds-container">
      <el-backtop target=".feeds-container" @click="refresh"></el-backtop>
      <div class="feeds-loading-top" v-show="topLoading">
        <i class="el-icon-loading" style="width: 1.2em; height: 1.2em">

        </i>
      </div>
      <Waterfall :list="list" :width="180" :hasAroundGutter="false" style="max-width: 1200px; margin: 0 auto;" :delay="50"
        :animationEffect="'animate__zoomIn'">
        <template #default="{ item }">
          <div class="card animate__animated animate__zoomIn" @click="toMain(item.id)">
            <div class="card-img-wrapper">
              <el-image 
                class="card-img"
                :src="item.image" 
                fit="cover">
                <template #placeholder>
                  <div class="image-placeholder">
                    <i class="el-icon-picture-outline"></i>
                  </div>
                </template>
              </el-image>
              <div class="card-overlay">
                <span class="view-detail">查看详情</span>
              </div>
            </div>
            <div class="footer">
              <h3 class="title">{{ item.title }}</h3>
              <div class="price">￥{{ item.price }}</div>
              <div class="author-wrapper">
                <div class="author">
                  <img class="author-avatar" :src="item.userInfo.avatar" />
                  <span class="name">{{ item.userInfo.nickname }}</span>
                </div>
                <div class="like-wrapper">
                  <i class="el-icon-thumb"></i>
                  <span class="count">{{ item.likeCount }}人想要</span>
                </div>
              </div>
            </div>
          </div>
        </template>

      </Waterfall>
      <!--loading-->
      <el-collapse-transition>
        <div class="feeds-loading" v-show="busy && !noMore">
          <span style="text-align: center;color:#cacdd5;font-size: 13px;">加载中</span><i class="el-icon-loading"
            style="font-size: 13px;margin-left: 10px"></i>
        </div>
      </el-collapse-transition>
      <div v-show="noMore" style="text-align: center;color:#cacdd5;font-size: 13px;margin: 3vh">没有更多了</div>
    </div>
    <transition 
      name="main-transition"
      enter-active-class="animate__animated animate__fadeIn animate__faster"
      leave-active-class="animate__animated animate__fadeOut animate__faster"
    >
      <Main 
        @main_close="closeMain" 
        v-if="mainShow" 
        :productId="productId"
        style="--animate-duration: 0.3s;"
      ></Main>
    </transition>
  </div>

</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue'
import { LazyImg, Waterfall } from 'vue-waterfall-plugin-next'
import 'vue-waterfall-plugin-next/dist/style.css'
import Main from "@/views/main/main.vue"
import screenUtil from "@/utils/screenUtil"
import api from "@/api"
import utils from "@/utils"
import 'animate.css'
import emitter from '@/utils/eventBus'
import { ElMessage } from 'element-plus'
// 状态定义
const show = ref(false)
const skeleton = ref(true)
const busy = ref(false)
const topLoading = ref(false)
const mainShow = ref(false)
const noMore = ref(false)
const productId = ref('')
const activeMenu = ref('')
const list = ref([])
const menuList = ref([{ typeCode: '', typeName: '全部' }])

const page_param = ref({
  pageNumber: 1,
  pageSize: 15,
  typeCode: '',
  key: '',
})

// 方法定义
const getProductList = async () => {
  try {
    const res = await api.product.getProductList(page_param.value)
    if (!Array.isArray(res.data)) {
      throw new Error('返回数据格式错误')
    }
    
    let productList = res.data
    productList.forEach(item => {
      if (item.image) {
        try {
          item.image = JSON.parse(item.image)[0]
        } catch (e) {
          console.warn('图片数据解析失败:', e)
          item.image = ''
        }
      }
    })
    
    if (productList.length === 0) {
      noMore.value = true
    } else {
      list.value.push(...productList)
    }
  } catch (error) {
    console.error('获取商品列表失败:', error)
    ElMessage.error('获取数据失败，请稍后重试')
  } finally {
    skeleton.value = false
    busy.value = false
    topLoading.value = false
  }
}

const handleKeyChange = (newKey) => {
  page_param.value.pageNumber = 1
  busy.value = false
  list.value = []
  page_param.value.key = newKey
  getProductList()
}

const getMenuList = () => {
  api.productType.getTypeList().then(res => {
    menuList.value.push(...res.data)
  })
}

const refresh = () => {
  topLoading.value = true;
}

const changeMenu = async (item) => {
  try {
    // 重置状态
    page_param.value.pageNumber = 1
    list.value = []
    busy.value = true // 设置为 true 显示加载状态
    noMore.value = false // 重置没有更多数据的状态
    
    page_param.value.typeCode = item.typeCode
    activeMenu.value = item.typeCode
    
    // 显示顶部加载动画
    topLoading.value = true
    
    await getProductList()
  } catch (error) {
    console.error('切换菜单失败:', error)
  }
}

const windowScroll = () => {
  //获取三个值
  let scrollTop = screenUtil.getScrollTop()
  let clientHeight = screenUtil.getClientHeight()
  let scrollHeight = screenUtil.getScrollHeight()
  //如果满足公式则，确实到底了
  if (scrollTop + clientHeight >= scrollHeight - 1) {
    if (!busy.value) {
      busy.value = true
      loadMoreData()
    }
  }
}

const loadMoreData = () => {
  page_param.value.pageNumber += 1;
  getProductList();
}

const closeMain = () => {
  mainShow.value = false
  // // 重新加载逻辑
  // window.location.reload()
  // // 或者其他刷新页面的方法
  // 重置数据并重新获取
  page_param.value.pageNumber = 1
  list.value = []
  getProductList()
}

const toMain = (val) => {
  productId.value = val
  mainShow.value = true
}

// 生命周期钩子
onMounted(() => {
  getMenuList()
  topLoading.value = true
  window.addEventListener('scroll', windowScroll, true)
  // 初始化时设置 skeleton 为 true
  skeleton.value = true
  getProductList()
  emitter.on('keyChanged', handleKeyChange)
})

onUnmounted(() => {
  window.removeEventListener("scroll", windowScroll)
  emitter.off('keyChanged', handleKeyChange)
})
</script>

<style lang="less" scoped>
.feeds-page {
  flex: 1;
  padding: 0 24px;
  padding-top: 72px;

  .channel-container {
    display: flex;
    justify-content: space-between;
    align-items: center;
    user-select: none;
    -webkit-user-select: none;

    .channel-scroll-container {
      backdrop-filter: blur(20px);
      background-color: transparent;
      width: calc(100vw - 24px);
      border-radius: 12px;
      
      position: relative;
      overflow: hidden;
      display: flex;
      user-select: none;
      -webkit-user-select: none;
      align-items: center;
      font-size: 16px;
      color: rgba(51, 51, 51, 0.8);
      height: 40px;
      white-space: nowrap;
      height: 72px;

      .content-container::-webkit-scrollbar {
        display: none;
      }

      .content-container {
        display: flex;
        overflow-x: scroll;
        overflow-y: hidden;
        white-space: nowrap;
        color: rgba(51, 51, 51, 0.8);

        .active {
          font-weight: 600;
          background: rgba(0, 0, 0, 0.03);
          border-radius: 999px;
          color: #333;
          font-size: 17px;
        }

        .channel {
          height: 45px;
          display: flex;
          justify-content: center;
          align-items: center;
          padding: 0 20px;
          cursor: pointer;
          -webkit-user-select: none;
          user-select: none;
          font-size: 16px;
        }
      }
    }
  }

  .feeds-container {
    position: relative;
    transition: width 0.5s;
    margin: 0 auto;
    overflow-y: auto;
    height: 77vh;
    padding: 16px;

    @media screen and (max-width: 1200px) {
      max-width: 900px;
    }
    
    @media screen and (max-width: 992px) {
      max-width: 720px;
    }
    
    @media screen and (max-width: 768px) {
      max-width: 540px;
    }

    .feeds-loading {
      text-align: center;
    }

    .feeds-loading-top {
      line-height: 6vh;
      height: 6vh;
    }

    .feeds-loading-top {
      -webkit-animation: move_1 0.5s;
    }

    @-webkit-keyframes move_1 {
      0% {
        -webkit-transform: translateY(-20px);
        opacity: 0;
      }
    }

    .noteImg {
      width: 240px;
      max-height: 300px;
      object-fit: cover;
      border-radius: 8px;
    }

    .card {
      position: relative;
      cursor: pointer;

      .top-tag-area {
        position: absolute;
        left: 12px;
        top: 12px;
        z-index: 4;

        .top-wrapper {
          background: #ff2442;
          border-radius: 999px;
          font-weight: 500;
          color: #fff;
          line-height: 120%;
          font-size: 12px;
          padding: 5px 8px;
          display: flex;
          align-items: center;
          justify-content: center;
        }
      }
    }

    .footer {
      padding: 10px 12px;

      .title {
        margin-bottom: 6px;
        word-break: break-all;
        display: -webkit-box;
        -webkit-box-orient: vertical;
        -webkit-line-clamp: 2;
        overflow: hidden;
        font-weight: 500;
        font-size: 15px;
        line-height: 1.4;
        height: 42px;
        color: #333;
      }

      .price {
        margin-bottom: 8px;
        word-break: break-all;
        display: -webkit-box;
        -webkit-box-orient: vertical;
        -webkit-line-clamp: 2;
        overflow: hidden;
        font-weight: 600;
        font-size: 17px;
        color: #ff4d4f;
        line-height: 1.2;
      }

      .author-wrapper {
        height: 28px;
        display: flex;
        align-items: center;
        justify-content: space-between;
        color: rgba(51, 51, 51, 0.8);
        font-size: 12px;
        transition: color 1s;

        .author {
          display: flex;
          align-items: center;
          color: inherit;
          overflow: hidden;
          text-overflow: ellipsis;
          white-space: nowrap;
          margin-right: 12px;

          .author-avatar {
            margin-right: 6px;
            width: 28px;
            height: 28px;
            border-radius: 50%;
            border: 1px solid #f0f0f0;
            flex-shrink: 0;
            object-fit: cover;
          }

          .name {
            color: #666;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            font-size: 13px;
            max-width: 80px;
          }
        }

        .like-wrapper {
          position: relative;
          display: flex;
          align-items: center;
          padding-left: 8px;

          .count {
            color: #666;
            margin-left: 2px;
            white-space: nowrap;
          }

          i {
            font-size: 15px;
            color: #666;
            margin-right: 2px;
          }
        }
      }
    }
  }

  .floating-btn-sets {
    position: fixed;
    display: flex;
    flex-direction: column;
    width: 40px;
    grid-gap: 8px;
    gap: 8px;
    right: 24px;
    bottom: 24px;

    .back-top {
      width: 40px;
      height: 40px;
      background: #fff;
      border: 1px solid rgba(0, 0, 0, 0.08);
      border-radius: 100px;
      color: rgba(51, 51, 51, 0.8);
      display: flex;
      align-items: center;
      justify-content: center;
      // transition: background 0.2s;
      cursor: pointer;
    }

    .reload {
      width: 40px;
      height: 40px;
      background: #fff;
      border: 1px solid rgba(0, 0, 0, 0.08);
      box-shadow: 0 2px 8px 0 rgba(0, 0, 0, 0.1),
        0 1px 2px 0 rgba(0, 0, 0, 0.02);
      border-radius: 100px;
      color: rgba(51, 51, 51, 0.8);
      display: flex;
      align-items: center;
      justify-content: center;
      //transition: background 0.2s;
      cursor: pointer;
    }
  }

  // 添加全屏加载动画
  .fullscreen-loading {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(255,255,255,0.9);
    z-index: 999;
    display: flex;
    align-items: center;
    justify-content: center;

    .loading-spinner {
      > div {
        width: 12px;
        height: 12px;
        background-color: #409EFF;
        border-radius: 100%;
        display: inline-block;
        animation: bounce 1.4s infinite ease-in-out both;
        margin: 0 3px;
      }
      .bounce1 { animation-delay: -0.32s; }
      .bounce2 { animation-delay: -0.16s; }
    }
  }

  // 优化分类菜单样式
  .channel-container {
    .channel-scroll-container {
      background: rgba(255,255,255,0.95);
      box-shadow: 0 2px 8px rgba(0,0,0,0.06);
      
      .channel-wrapper {
        display: flex;
        padding: 0 20px;

        .channel {
          position: relative;
          padding: 0 24px;
          height: 50px;
          display: flex;
          align-items: center;
          cursor: pointer;
          transition: all 0.3s;

          &:hover {
            color: #409EFF;
          }

          &.active {
            color: #409EFF;
            font-weight: 600;

            .active-bar {
              position: absolute;
              bottom: 0;
              left: 50%;
              transform: translateX(-50%);
              width: 20px;
              height: 3px;
              background: #409EFF;
              border-radius: 2px;
            }
          }
        }
      }
    }
  }

  // 优化商品卡片样式
  .card {
    background: #fff;
    border-radius: 12px;
    overflow: hidden;
    transition: all 0.2s ease-out;
    box-shadow: 0 2px 12px rgba(0,0,0,0.04);
    animation-duration: 0.8s;
    animation-fill-mode: both;

    &:hover {
      transform: translateY(-4px);
      box-shadow: 0 6px 20px rgba(0,0,0,0.08);
      transition: all 0.2s ease-out;

      .card-overlay {
        opacity: 1;
        transition: opacity 0.2s ease-out;
      }
    }

    .card-img-wrapper {
      position: relative;
      padding-top: 75%;

      .card-img {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        object-fit: cover;
      }

      .card-overlay {
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0,0,0,0.3);
        display: flex;
        align-items: center;
        justify-content: center;
        opacity: 0;
        transition: opacity 0.2s ease-out;

        .view-detail {
          color: #fff;
          font-size: 14px;
          padding: 6px 16px;
          border: 1px solid #fff;
          border-radius: 20px;
        }
      }
    }

    .footer {
      padding: 8px 10px;    
      
      .title {
        font-size: 17px;             
        line-height: 1.3;    
        height: 42px;                
        margin-bottom: 4px;  
        color: #2c2c2c;            
        font-family: "PingFang SC", "Microsoft YaHei", "Helvetica Neue", sans-serif;
        font-weight: 600;          
        letter-spacing: 0.3px;      
        transition: color 0.2s;     
        
        &:hover {
          color: #000000;         
        }
      }
      
      .price {
        margin-bottom: 8px;
        word-break: break-all;
        display: -webkit-box;
        -webkit-box-orient: vertical;
        -webkit-line-clamp: 2;
        overflow: hidden;
        font-weight: 600;
        font-size: 17px;
        color: #ff4d4f;
        line-height: 1.2;
      }

      .author-wrapper {
        height: 28px;
        display: flex;
        align-items: center;
        justify-content: space-between;
        color: rgba(51, 51, 51, 0.8);
        font-size: 12px;
        transition: color 1s;

        .author {
          display: flex;
          align-items: center;
          color: inherit;
          overflow: hidden;
          text-overflow: ellipsis;
          white-space: nowrap;
          margin-right: 12px;

          .author-avatar {
            margin-right: 6px;
            width: 28px;
            height: 28px;
            border-radius: 50%;
            border: 1px solid #f0f0f0;
            flex-shrink: 0;
            object-fit: cover;
          }

          .name {
            color: #666;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            font-size: 13px;
            max-width: 80px;
          }
        }

        .like-wrapper {
          position: relative;
          display: flex;
          align-items: center;
          padding-left: 8px;

          .count {
            color: #666;
            margin-left: 2px;
            white-space: nowrap;
          }

          i {
            font-size: 15px;
            color: #666;
            margin-right: 2px;
          }
        }
      }
    }
  }

  // 添加 Main 组件的过渡动画样式
  .main-transition {
    &-enter-active,
    &-leave-active {
      transition: all 0.3s ease-out;
    }

    &-enter-from {
      opacity: 0;
      transform: scale(0.98);
    }

    &-leave-to {
      opacity: 0;
      transform: scale(0.98);
    }
  }
}

// 确保 Main 组件的动画层级正确
:deep(.main-component) {
  z-index: 1000;
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(255, 255, 255, 0.98);
  animation-duration: 0.3s !important;
}

@keyframes bounce {
  0%, 80%, 100% { transform: scale(0); }
  40% { transform: scale(1.0); }
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes zoomIn {
  from {
    opacity: 0;
    transform: scale3d(0.3, 0.3, 0.3);
  }
  50% {
    opacity: 1;
  }
}

@keyframes bounceIn {
  from {
    opacity: 0;
    transform: scale3d(0.97, 0.97, 0.97);
  }
  50% {
    opacity: 0.8;
    transform: scale3d(1.03, 1.03, 1.03);
  }
  to {
    opacity: 1;
    transform: scale3d(1, 1, 1);
  }
}
</style>
