<template>
  <div class="container ">
    <div class="header"><span class="header-icon"></span><span class="header-title">我的动态</span></div>
    <el-timeline style="margin-top: 10px">
      <!--下面的trendList 为了便于显示自己新建了一个-->
      <!-- <el-timeline-item v-for="(trend, index) in test_trends" :key="index" type="primary" :color="trend.color"
        size="normal" placement="top" :timestamp="$utils.convert.parseTime(String(trend.createTime))"> -->
      <el-timeline-item v-for="(trend, index) in trendList" :key="index" 
        class="trend-item-animation"
        :style="{ animationDelay: index * 50 + 'ms' }"
        type="primary" :color="trend.color"
        size="normal" placement="top" :timestamp="$utils.convert.parseTime(String(trend.createTime))">
        <div class="trend-type" v-if="trend.type === 'publish'">上新了一件宝贝</div>
        <div class="trend-type" v-if="trend.type === 'sell'">卖出了一件宝贝</div>
        <div class="trend-type" v-if="trend.type === 'buy'">买到了一件宝贝</div>
        <div class="trend-container" @click="toMain(trend.id)">
          <div class="trend-item">
            <!--            <a class="user-avatar">-->
            <!--              <img class="avatar-item"/>-->
            <!--            </a>-->
            <div class="main" v-if="trend.id">
              <div class="details-box">
                <el-image style="border-radius: 10px;width: 100%;height: 100%" :src=trend.image
                  fit="cover"></el-image>
              </div>
              <div class="info">
                <div class="user-info">
                  <!-- <a>麦当劳</a> -->
                  <a>{{ trend.title }}</a>
                </div>
                <!--                <div class="interaction-hint"><span>1天前</span></div>-->
                <div class="interaction-content">{{ trend.intro }}</div>
                <!-- <div class="interaction-content">肯德基疯狂星期五V我四十</div> -->
                <!-- <div class="interaction-price">￥40</div> -->
                <div class="interaction-price">￥{{ $utils.convert.to_price(trend.price) }}</div>
                <!-- <div class="interaction-price">￥{{ $utils.convert.to_price(trend.price) }}</div> -->
              </div>
            </div>
            <div class="main" v-else>
              <span style="font-size: 13px;color:#909399"> 宝贝已被删除</span>
            </div>
          </div>
        </div>
      </el-timeline-item>
    </el-timeline>
    <!--点击后展示商品详情-->
    <transition 
      name="main-transition"
      enter-active-class="animate__animated animate__fadeIn animate__faster"
      leave-active-class="animate__animated animate__fadeOut animate__faster"
    >
      <Main 
        @main_close="closeMain" 
        v-if="mainShow" 
        :productId="productId"
      ></Main>
    </transition>
  </div>
</template>
<script setup>

</script>
<script>
import image from "@/api/image";
import Main from "@/views/main/main.vue";
import api from "@/api";
import utils from "@/utils";
export default {
  components: { Main },
  data() {
    return {
      mainShow: false,
      productId: '',
      trendList: []
    }
  },
  created() {
    this.$api = api;
    this.$utils = utils;
    this.getTrend()
  },
  methods: {
    getTrend() {
      this.$api.trend.getTrendProduct().then(res => {
        this.trendList = res.data;
        if(this.trendList.length === 0){
          console.log("is empty!");
        } else {
          console.log("not empty!");
        }
        this.trendList.forEach(item => {
          if (item.image) {
            item.image = JSON.parse(item.image)[0]
          }
        })
      })
    },
    toMain(val) {
      this.productId = val
      this.mainShow = true
    },
    closeMain() {
      this.mainShow = false
    }
  }
}
</script>

<style lang="less" scoped>
.container {
  .header {
    padding: 15px 0;
    line-height: 16px;
    font-size: 16px;
    font-weight: 400;

    .header-icon {
      position: relative;
      top: 2px;
      display: inline-block;
      width: 6px;
      height: 19px;
      background: #3a64ff;
      border-radius: 3px;
      margin-right: 2px;
    }

    .header-title {
      line-height: 20px;
      font-size: 20px;
    }
  }

  flex: 1;
  padding: 0 24px;
  padding-top: 72px;
  width: 67%;
  height: 100vh;
  margin: 0 auto;

  .feeds-loading {
    margin: 3vh;
    text-align: center;
  }

  .trend-type {
    color: #909399;
    font-weight: bold;
  }

  .trend-container {
    cursor: pointer;

    .trend-item {
      display: flex;
      flex-direction: row;
      padding-top: 10px;
      max-width: 100vw;

      .user-avatar {
        margin-right: 24px;
        flex-shrink: 0;

        .avatar-item {
          width: 48px;
          height: 48px;
          display: flex;
          align-items: center;
          justify-content: center;
          cursor: pointer;
          border-radius: 100%;
          border: 1px solid rgba(0, 0, 0, 0.08);
          object-fit: cover;
        }
      }

      .main {
        flex-grow: 1;
        flex-shrink: 1;
        display: flex;
        flex-direction: row;
        padding-bottom: 12px;
        border-bottom: 1px solid rgba(0, 0, 0, 0.08);

        .details-box {
          width: 20%;
        }

        .info {
          flex-grow: 1;
          flex-shrink: 1;
          margin-left: 10px;
          width: 75%;

          .user-info {
            display: flex;
            flex-direction: row;
            align-items: center;
            font-size: 16px;
            font-weight: 600;
            margin-bottom: 4px;

            a {
              color: #333;
            }
          }

          .interaction-hint {
            font-size: 14px;
            color: rgba(51, 51, 51, 0.6);
            margin-bottom: 8px;
          }

          .interaction-content {
            display: flex;
            font-size: 14px;
            color: #333;
            margin-bottom: 12px;
            line-height: 140%;
            cursor: pointer;
            display: -webkit-box;
            -webkit-box-orient: vertical;
            -webkit-line-clamp: 3;
            word-break: break-all;
            overflow: hidden;
            margin-top: 15px;
          }

          .interaction-price {
            position: absolute;
            bottom: 10px;
            font-weight: bolder;
            font-size: 16px;
            color: red;
          }

          .interaction-footer {
            margin: 8px 12px 0 0;
            padding: 0 12px;
            display: flex;
            justify-content: space-between;
            align-items: center;

            .icon-item {
              display: flex;
              justify-content: left;
              align-items: center;
              color: rgba(51, 51, 51, 0.929);

              .count {
                margin-left: 3px;
              }
            }
          }
        }
      }
    }
  }

  .card {
    animation-duration: 0.5s;
    animation-fill-mode: both;
    transition: all 0.2s ease-out;
    
    &:hover {
      transform: translateY(-4px);
      box-shadow: 0 6px 20px rgba(0,0,0,0.08);
      transition: all 0.2s ease-out;

      .card-overlay {
        opacity: 1;
        transition: opacity 0.2s ease-out;
      }
    }
  }

  // Main 组件的过渡动画样式
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

  .trend-item-animation {
    animation: slideInUp 0.3s ease-out forwards;
    opacity: 0;
  }

  @keyframes slideInUp {
    from {
      opacity: 0;
      transform: translateY(20px);
    }
    to {
      opacity: 1;
      transform: translateY(0);
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
    transform: scale3d(0.95, 0.95, 0.95);
  }
  to {
    opacity: 1;
    transform: scale3d(1, 1, 1);
  }
}
</style>
