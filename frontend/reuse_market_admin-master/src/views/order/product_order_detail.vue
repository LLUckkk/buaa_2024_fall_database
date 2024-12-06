<template>
  <div class="app-container">
    <div class="header">
      <span class="header-icon"></span><span class="header-title">商品订单详情</span>
    </div>
    <div class="desc">
      <div class="header-title" style="margin-top: 10px;margin-bottom: 15px;font-size: 18px">订单信息</div>

      <el-descriptions :column="3" border>
        <el-descriptions-item label="订单号">{{ productOrder.orderNumber }}</el-descriptions-item>
        <el-descriptions-item label="商品标题">{{ productOrder.productTitle }}</el-descriptions-item>
        <el-descriptions-item label="商品标题">{{ productOrder.productTitle }}</el-descriptions-item>
        <el-descriptions-item label="商品详情">{{ productOrder.productInfo }}</el-descriptions-item>
        <el-descriptions-item label="支付金额">{{ $utils.convert.to_price(productOrder.productMoney) }}</el-descriptions-item>

        <el-descriptions-item label="创建时间">
          {{ $utils.convert.formatISOTime(productOrder.createTime) }}
        </el-descriptions-item>
        <el-descriptions-item label="更新时间">
          {{ $utils.convert.formatISOTime(productOrder.updateTime) }}
        </el-descriptions-item>
        <el-descriptions-item label="支付状态">

          <el-tag size="small"> {{ $utils.convert.to_text(productOrder.payStatus, $utils.pro.paymentStatus) }}</el-tag>
        </el-descriptions-item>
        <el-descriptions-item label="交易状态">
          <el-tag size="small"> {{ $utils.convert.to_text(productOrder.dealStatus, $utils.pro.orderStatus) }}</el-tag>
        </el-descriptions-item>
      </el-descriptions>
      <el-divider></el-divider>
      <div class="header-title" style="margin-top: 10px;margin-bottom: 15px;font-size: 18px"> 快递信息</div>

      <el-descriptions :column="3" border>
        <el-descriptions-item label="发货方式">{{ productOrder.postMode}}</el-descriptions-item>
        <el-descriptions-item label="自提码" v-if="productOrder.postMode === '用户自提'">{{ productOrder.postSelfCode}}</el-descriptions-item>
        <el-descriptions-item label="物流公司" v-if="productOrder.postMode === '物流发货'">{{ productOrder.shipCompany}}</el-descriptions-item>
        <el-descriptions-item label="快递单号" v-if="productOrder.postMode === '物流发货'">{{ productOrder.shipNum}}</el-descriptions-item>
        <el-descriptions-item label="收货人姓名">{{ productOrder.postUsername}}</el-descriptions-item>
        <el-descriptions-item label="收货人联系电话">{{ productOrder.postPhone}}</el-descriptions-item>
        <el-descriptions-item label="收货地址" v-if="productOrder.postMode === '物流发货'">{{ productOrder.postAddress}}</el-descriptions-item>
      </el-descriptions>
      <div v-if="productOrder.dealStatus===11">
        <el-divider></el-divider>
        <div class="header-title" style="margin-top: 10px;margin-bottom: 15px;font-size: 18px">评价信息</div>
          <div>
            <div class="user-info">
              <el-rate :disabled="true" v-model="productOrder.evaScore" :texts="['极差', '失望', '一般', '满意', '极好']" show-text></el-rate>
            </div>
            <div class="interaction-content" style="margin-top: 25px">
              <el-input :disabled="true" type="textarea" placeholder="你的评价可以帮到其他买家哦!" maxlength="150" :autosize="{minRows: 5}" show-word-limit v-model="productOrder.evaContent"></el-input>
            </div>
          </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      paymentOrder: {},
      paymenetPay: {},
      productOrder: {},
    }
  },
  created() {
    this.getProductInfo();
  },
  methods: {
    getProductInfo() {
      this.$api.productOrder.getProductOrderDetail(this.$route.query.id).then(res => {
        this.paymentOrder = res.result.paymentOrder
        this.paymenetPay = res.result.paymenetPay
        this.productOrder = res.result.productOrder
      })
    },
  }
}
</script>
<style scoped lang="scss">
.app-container {
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

  .desc {
    padding: 20px;
  }
}
</style>