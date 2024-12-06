<template>
  <div class="app-container">
    <div class="header">
      <span class="header-icon"></span><span class="header-title">支付订单详情</span>
    </div>
    <div class="desc">
      <div class="header-title" style="margin-top: 10px;margin-bottom: 15px;font-size: 18px">支付信息</div>

      <el-descriptions :column="3" border>
        <el-descriptions-item label="支付订单号">{{ paymentOrder.orderNumber }}</el-descriptions-item>
        <el-descriptions-item label="支付金额">{{$utils.convert.to_price(paymentOrder.payPrice) }}</el-descriptions-item>
        <el-descriptions-item label="支付信息">{{ paymentOrder.payTypeInfo}}</el-descriptions-item>
        <el-descriptions-item label="支付类型">{{ paymentOrder.payTypeName }}</el-descriptions-item>
        <el-descriptions-item label="支付方式">{{ paymentOrder.paymentType }}</el-descriptions-item>
        <el-descriptions-item label="创建时间">
          {{ $utils.convert.formatISOTime(paymentOrder.timeCreate) }}
        </el-descriptions-item>
        <el-descriptions-item label="更新时间">
          {{ $utils.convert.formatISOTime(paymentOrder.timeUpdate) }}
        </el-descriptions-item>
        <el-descriptions-item label="订单完成时间">
          {{ $utils.convert.formatISOTime(paymentOrder.timeFinish) }}
        </el-descriptions-item>
        <el-descriptions-item label="支付状态">
          <el-tag size="small"> {{ $utils.convert.to_text(paymentOrder.paymentStatus, $utils.pro.paymentStatus) }}</el-tag>
        </el-descriptions-item>
      </el-descriptions>
      <el-divider></el-divider>
      <div class="header-title" style="margin-top: 10px;margin-bottom: 15px;font-size: 18px">买家信息</div>

      <el-descriptions :column="3" border>
        <el-descriptions-item label="用户昵称">{{ user.nickName }}</el-descriptions-item>
        <el-descriptions-item label="闲宝号">{{ user.number }}</el-descriptions-item>
        <el-descriptions-item label="联系电话">{{ user.phone }}</el-descriptions-item>
      </el-descriptions>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      paymentOrder: {},
      user:{},
    }
  },
  created() {
    this.getProductInfo();
  },
  methods: {
    getProductInfo() {
      this.$api.paymentOrder.getPaymentOrderDetail(this.$route.query.id).then(res => {
        this.paymentOrder = res.result.paymentOrder
        this.user = res.result.user
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