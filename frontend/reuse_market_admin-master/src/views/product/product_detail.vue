<template>
  <div class="app-container">
    <div class="header">
      <span class="header-icon"></span><span class="header-title">商品详情</span>
    </div>
    <div class="desc">
      <div class="header-title" style="margin-top: 10px;margin-bottom: 15px;font-size: 18px">商品信息</div>

      <el-descriptions :column="3" border>
        <el-descriptions-item label="商品图片"  >
          <el-carousel height="250px" indicator-position="none">
            <el-carousel-item v-for="item in info.image" :key="item">
            <el-image :src="item" fit="cover" style="height: 100%;width: 100%"></el-image>
            </el-carousel-item>
          </el-carousel>
        </el-descriptions-item>
        <el-descriptions-item label="商品名称">{{ info.title }}</el-descriptions-item>
        <el-descriptions-item label="商品描述">{{ info.intro }}</el-descriptions-item>
        <el-descriptions-item label="价格">{{ $utils.convert.to_price(info.price) }} 元</el-descriptions-item>
        <el-descriptions-item label="原价">{{ $utils.convert.to_price(info.originalPrice) }} 元</el-descriptions-item>
        <el-descriptions-item label="商品类型">{{ info.typeName }}</el-descriptions-item>
        <el-descriptions-item label="发货方式">{{ info.postType===0?'邮寄':'自提' }}</el-descriptions-item>
        <el-descriptions-item label="想要人数">{{ info.likeCount }} 人</el-descriptions-item>
        <el-descriptions-item label="商品位置">{{ info.province }} {{ info.city }} {{ info.district }}</el-descriptions-item>
        <el-descriptions-item label="创建时间">
          {{ $utils.convert.parseTime(info.createTime) }}
        </el-descriptions-item>
        <el-descriptions-item label="更新时间">
          {{ $utils.convert.parseTime(info.updateTime) }}
        </el-descriptions-item>
        <el-descriptions-item label="状态">
            {{ $utils.convert.to_text(info.status, $utils.pro.productStatus) }}
        </el-descriptions-item>
      </el-descriptions>
      <el-divider></el-divider>
      <div class="header-title" style="margin-top: 10px;margin-bottom: 15px;font-size: 18px">卖家信息</div>
      <el-descriptions :column="3" border>
        <el-descriptions-item label="卖家闲宝号">{{ user.number }}</el-descriptions-item>
        <el-descriptions-item label="卖家昵称">{{ user.nickName }}</el-descriptions-item>
        <el-descriptions-item label="卖家电话">{{ user.phone }}</el-descriptions-item>
      </el-descriptions>
    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      info: {},
      user: {},
    }
  },
  created() {
    this.getProductInfo();
  },
  methods: {
    getProductInfo() {
      this.$api.productInfo.getProductInfoDetail(this.$route.query.id).then(res => {
        this.info = res.result.productInfo
       this.info.image = JSON.parse(this.info.image)
        this.user = res.result.user
      })
    },
    pass(){
      this.$confirm('确认审核通过?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.productInfo.passProduct(this.$route.query.id).then(res => {
          this.$message({
            type: 'success',
            message: '审核通过!'
          });
          this.$router.push('/product/product_list')
        })
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