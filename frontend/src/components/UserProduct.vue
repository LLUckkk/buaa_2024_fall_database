<template>
  <div class="feeds-container">
    <Waterfall :list="list" :width="230" :hasAroundGutter="false" :animationDelay="0" :animationEffect="''" :animationDuration="0" style="max-width: 1260px;">
      <template #default="{item}">
        <div class="card  animate__animated animate__bounceIn">
          <el-image @click="toMain(item.productId)" style="border-radius: 10px;width: 230px;height: 157px;cursor: pointer;" :src="item.image" fit="cover"></el-image>
          <div class="footer">
            <a class="title"><span>{{ item.title }}</span></a>
              <div style="display:flex;justify-content: space-between;margin-bottom: 5px">
              <!-- <span v-if="activeTab !== 'publish' && activeTab !== 'collect'  " style="font-size: 12px;color: #3a64ff;cursor: pointer" @click="toOrder(item)">查看订单</span> -->
              <span v-if="activeTab === 'publish' && (item.status === 1 || item.status === 2) && item.status !== 12"  
                    style="font-size: 12px;color: #3a64ff;cursor: pointer" 
                    @click="del(item)">删除</span>
              <span v-if="activeTab === 'publish' && item.status === 9 && item.status !== 12"  
                    style="font-size: 12px;color: #3a64ff;cursor: pointer" 
                    @click="disable(item)">下架</span>
              <span v-if="activeTab === 'publish' && item.status !== 9 && item.status !== 12"  
                    style="font-size: 12px;color: #3a64ff;cursor: pointer; margin-left: 10px;" 
                    @click="enable(item)">重新上架</span>
            </div>
            <div class="price">
              <span style="color: red">￥{{ $utils.convert.to_price(item.price) }}</span>
              <el-tag type="success" v-if="activeTab === 'publish' && item.status ===9 " style="font-size: 12px;color: #67C23A;margin-left: 10px" >上线</el-tag>
              <el-tag type="danger" v-if="activeTab === 'publish' && item.status ===2 " style="font-size: 12px;color: red;margin-left: 10px" >已下架</el-tag>
              <el-tag type="warning" v-if="activeTab === 'publish' && item.status ===12 " style="font-size: 12px;color: #E6A23C;margin-left: 10px" >已卖出</el-tag>
            </div>
            <!-- <div v-if="activeTab ==='purchase'" style="display: flex;justify-content: flex-end;font-size: 15px;margin-top: 5px">
              <button v-if="item.dealStatus === 1 " class="publishBtn" type="submit"><span class="btn-content">订单失败</span></button>
              <button @click="toEvaluate(item)" v-if="item.dealStatus === 9 " class="publishBtn" type="submit"><span class="btn-content">去评价</span></button>
              <button v-if="item.dealStatus === 11 " class="publishBtn" type="submit"><span class="btn-content">评价完成</span></button>
              <button v-if="item.dealStatus === 3 &&item.postMode === '物流发货' " class="publishBtn" type="submit"><span class="btn-content">待发货</span></button>
              <button v-if="item.dealStatus === 3 &&item.postMode === '用户自提' " class="publishBtn" type="submit"><span class="btn-content">待上传提货地址</span></button>
              <button @click="toPay(item)" v-if="item.dealStatus === 0 || item.dealStatus === 2" class="publishBtn" type="submit"><span class="btn-content">待付款</span></button>
              <button @click="confirmSelf(item)" v-if="item.dealStatus === 4 &&item.postMode === '用户自提'" class="publishBtn" type="submit"><span class="btn-content">确认提货：{{ item.postSelfCode }} </span></button>
              <button @click="confirmPost(item)" v-if="item.dealStatus === 4 &&item.postMode === '物流发货' " class="publishBtn" type="submit"><span class="btn-content">确认收货</span></button>
            </div> -->
            <!-- <div v-if="activeTab ==='sell'" style="display: flex;justify-content: flex-end;font-size: 15px;margin-top: 5px">
              <button v-if="item.dealStatus === 0 || item.dealStatus === 2" class="publishBtn" type="submit"><span class="btn-content">待付款</span></button>
              <button v-if="item.dealStatus === 1 " class="publishBtn" type="submit"><span class="btn-content">订单失败</span></button>
              <button v-if="item.dealStatus === 9 " class="publishBtn" type="submit"><span class="btn-content">交易成功</span></button>
              <button v-if="item.dealStatus === 11 " class="publishBtn" type="submit"><span class="btn-content">交易成功</span></button>
              <button v-if="item.dealStatus === 4 &&item.postMode === '物流发货' " class="publishBtn" type="submit"><span class="btn-content">待收货</span></button>
              <button v-if="item.dealStatus === 4 &&item.postMode === '用户自提' " class="publishBtn" type="submit"><span class="btn-content">待提货: {{ item.postSelfCode }}</span></button>
              <button @click="post(item)" v-if="item.dealStatus === 3 &&item.postMode === '物流发货'" class="publishBtn" type="submit"><span class="btn-content">去发货</span></button>
              <button @click="post(item)" v-if="item.dealStatus === 3 &&item.postMode === '用户自提'" class="publishBtn" type="submit"><span class="btn-content">上传提货地址</span></button>
            </div> -->
          </div>
        </div>
      </template>
    </Waterfall>
    <Main @main_close="closeMain" v-if="mainShow" :productId="productId"></Main>
    <!--抽屉 用户编辑-->
    <div>
      <el-drawer destroy-on-close @close-drawer="closeDrawer" :show-close="false" :size="'400px'" :visible.sync="drawer" direction="rtl">
        <post_edit @close-drawer="closeDrawer" :productOrderId="productOrderId" :type="type"></post_edit>
      </el-drawer>
    </div>
  </div>
</template>
<script>
import { LazyImg, Waterfall } from "vue-waterfall-plugin-next";
import "vue-waterfall-plugin-next/dist/style.css";
import Main from "@/views/main/main.vue";
import post_edit from "@/components/post_edit.vue";
import {ElNotification} from 'element-plus';
import api from '@/api';
import utils from '@/utils';
 
export default {
  components: {post_edit, Waterfall, LazyImg, Main},
  props: {
    activeTab: '',
  },
  data() {
    return {
      drawer: false,
      type: '',
      productOrderId: '',
      list: [],
      productId: '',
      mainShow: false,
    }
  },
  created() {
    this.$api = api
    this.$utils = utils
    this.getList()
  },
  watch: {
    activeTab() {
      this.getList()
    },
  
  },
  methods: {
    getList() {
      this.list = []
      if (this.activeTab === 'publish') this.getPublish()
      if (this.activeTab === 'sell') this.getSell()
      if (this.activeTab === 'purchase') this.getPurchase()
      if (this.activeTab === 'collect') this.getCollect()
    },
    closeDrawer() {
      this.drawer = false
      this.getList()
    },
    getPublish() {
      this.$api.product.getMyProductList().then(res => {
        this.list = res.data
        this.list.forEach(item => {
          if (item.image) {
            item.image = JSON.parse(item.image)[0]
          }
          item.productId = item.id
        })
      })
    },
    disable(item) {
      this.$confirm('此操作将下架该商品, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.product.disableProduct(item.id).then(res => {
          ElNotification({type: 'success', title: '航游集市', message: '下架成功'})
          this.getList();
          if(this.reload) {
            this.reload();
          }
        })
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '已取消下架'
        });
      });
    },
    del(item) {
      this.$confirm('此操作将永久删除该商品, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.product.delProductInfo(item.id).then(res => {
          ElNotification({type: 'success', title: '航游集市', message: '删除成功'})
          this.getList();
          if(this.reload) {
            this.reload();
          }
        })
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '已取消删除'
        });
      });
    },
    toEvaluate(item) {
      this.$router.push("/orderEvaluate?orderId=" + item.id)
    },
    toOrder(item) {
      this.$router.push('/orderDetail?orderId=' + item.id)
    },
    confirmSelf(item) {
      this.$api.productOrder.userSelfProduct(item.id).then(res => {
        this.getList();
      })
    },
    post(item) {
      this.productOrderId = item.id
      this.type = item.postMode
      this.drawer = true
    },
    confirmPost(item) {
      this.$api.productOrder.confirmPost(item.id).then(res => {
        this.getList()
      })
    },
    toPay(item) {
      this.$router.push("/paymentPay?paymentOrderId=" + item.payOrderId)
    },
    getSell() {
      this.$api.productOrder.getMySellProductOrder().then(res => {
        this.list = res.data
        this.handleData(this.list)
      })
    },
    getPurchase() {
      this.$api.productOrder.getMyBuyProductOrder().then(res => {
        this.list = res.data
        this.handleData(this.list)
      })
    },
    getCollect(){
      this.$api.product.getCollectProduct().then(res=>{
        this.list = res.data
        this.list.forEach(item => {
          if (item.image) {
            item.image = JSON.parse(item.image)[0]
          }
          item.productId = item.id
        })
      })
    },
    handleData(list) {
      list.forEach(item => {
        if (item.productImg) {
          item.image = JSON.parse(item.productImg)[0]
        }
        item.originalPrice = item.productPrice
        item.title = item.productTitle
        item.price = item.buyMoneyAll
      })
    },
    closeMain() {
      this.mainShow = false
    },
    toMain(val) {
      this.productId = val
      this.mainShow = true
    },
    enable(item) {
      this.$confirm('此操作将重新上架该商品, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.product.enableProduct(item.id).then(res => {
          ElNotification({type: 'success', title: '航游集市', message: '重新上架成功'})
          this.getList();
          if(this.reload) {
            this.reload();
          }
        })
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '已取消重新上架'
        });
      });
    },
  }
}
</script>
<style lang="less" scoped>
.animate__animated {
  animation-duration: 1.5s; //动画持续时间
  //animation-delay: 0.5s !important; //动画延迟时间
  //animation-iteration-count: 2 !important;
}
.feeds-container {
  position: relative;
  transition: width 0.5s;
  margin: 0 auto;

  .card {
    position: relative;

    background-color: #f7f7f7;
    border-radius: 8px;
  }

  .noteImg {
    width: 240px;
    max-height: 300px;
    object-fit: cover;
    border-radius: 8px;
  }

  .footer {
    padding: 12px;

    button {
      padding: 5px;
      height: 25px;
      font-size: 12px;
    }

    .publishBtn {
      background-color: #3a64ff;
      color: #fff;
      border-radius: 4px;
      cursor: pointer;
    }

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
      color: #333;
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
          margin-left: 2px;
        }
      }
    }
  }
}
</style>