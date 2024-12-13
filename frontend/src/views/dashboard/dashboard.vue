<!--主页，推荐页-->
<template>
  <div class="feeds-page">
    <!--top-->
    <div class="channel-container">
      <div class="scroll-container channel-scroll-container">
        <div class="content-container " style="margin-left: 3vh">
          <div class="channel " :class="activeMenu === item.typeCode ? 'active' : ''" v-for="(item, index) in menuList"
            :key="item.typeCode" @click="changeMenu(item)">
            {{ item.typeName }}
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
      <Waterfall :list="list" :width="230" :hasAroundGutter="false" style="max-width: 1260px; " :delay="1000"
        :animationEffect="'animate__zoomIn'">
        <template #default="{ item, url }">
          <!-- <div class="card" @click="toMain(item.id)"> -->
          <div class="card">
            <!-- <el-image style="border-radius: 10px;width: 230px;" :src="item.image" fit="cover"></el-image> -->
            <div class="footer">
              <!-- <a class="title"><span>{{ item.title }}</span></a> -->
              <a class="title"><span>test</span></a>
              <div class="price">
                <!-- <span>￥{{ $utils.convert.to_price(item.price) }}</span> -->
                <span>￥100</span>
                <!-- <span
                  style="margin-left:5px;font-size: 11px;color: #9e9e9e;font-weight: normal;text-decoration: line-through">￥{{
                    $utils.convert.to_price(item.originalPrice) }}</span> -->
              </div>
              <div class="author-wrapper">
                <a class="author">
                  <!-- <img class="author-avatar" :src="item.avatar" /> -->
                  <!-- <span class="name">{{ item.city }}{{ item.district }}</span> -->
                </a>
                <span class="like-wrapper"><i class="el-icon-thumb"></i>
                  <!-- <span class="count">{{ item.likeCount }}人想要</span> -->
                  <span class="count">0人想要</span>
                </span>
              </div>
            </div>
          </div>
        </template>
      </Waterfall>
      <!--loading-->
      <el-collapse-transition>
        <div class="feeds-loading" v-show="busy && !noMore">
          <span style="text-align: center;color:#cacdd5;font-size: 12px;">加载中</span><i class="el-icon-loading"
            style="font-size: 12px;margin-left: 10px"></i>
        </div>
      </el-collapse-transition>
      <div v-show="noMore" style="text-align: center;color:#cacdd5;font-size: 12px;margin: 3vh">没有更多了</div>
    </div>
    <Main @main_close="closeMain" v-if="mainShow" :productId="productId"></Main>
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
const getProductList = () => {
  api.product.getProductList(page_param.value).then(res => {
    let productList = res.data
    alert(productList.length)
    if (productList.length === 0) {
      setTimeout(() => {
        noMore.value = true
      }, 1000)
    } else {
      productList.forEach(item => {
        if (item.image) {
          item.image = JSON.parse(item.image)[0]
        }
      })
      list.value.push(...productList)
      busy.value = false
    }
    topLoading.value = false
  })
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

const changeMenu = (item) => {
  page_param.value.pageNumber = 1
  list.value = []
  busy.value = false
  page_param.value.typeCode = item.typeCode
  activeMenu.value = item.typeCode
  getProductList()
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
  getProductList()
})

onUnmounted(() => {
  window.removeEventListener("scroll", windowScroll)
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
        }

        .channel {
          height: 40px;
          display: flex;
          justify-content: center;
          align-items: center;
          padding: 0 16px;
          cursor: pointer;
          -webkit-user-select: none;
          user-select: none;
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
      padding: 12px;

      .title {
        margin-bottom: 8px;
        word-break: break-all;
        display: -webkit-box;
        -webkit-box-orient: vertical;
        -webkit-line-clamp: 2;
        overflow: hidden;
        font-weight: 500;
        font-size: 14px;
        line-height: 140%;
        color: black;
      }

      .price {
        margin-bottom: 8px;
        word-break: break-all;
        display: -webkit-box;
        -webkit-box-orient: vertical;
        -webkit-line-clamp: 2;
        overflow: hidden;
        font-weight: bolder;
        font-size: 16px;
        color: red;
        line-height: 140%;
      }

      .author-wrapper {
        display: flex;
        align-items: center;
        justify-content: space-between;
        height: 20px;
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
            width: 20px;
            height: 20px;
            border-radius: 20px;
            border: 1px solid rgba(0, 0, 0, 0.08);
            flex-shrink: 0;
            object-fit: cover;
          }

          .name {
            color: #9e9e9e;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
          }
        }

        .like-wrapper {
          position: relative;
          display: flex;
          align-items: center;

          .count {
            color: #9e9e9e;
            margin-left: 2px;
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
}
</style>
