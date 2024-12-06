<template>
  <div class="app-container">
    <div class="demo-input-suffix">
      <el-input
          placeholder="商品标题、商品类型"
          prefix-icon="el-icon-search"
          v-model="page_param.key"
          @input="getQueryList"
          style="width: 300px;margin: 5px"
      >
      </el-input>
    </div>

    <el-menu :default-active="page_param.status" class="el-menu-demo" mode="horizontal" @select="handleSelect">
      <el-menu-item index="">全部</el-menu-item>
      <el-menu-item index="1">待审核</el-menu-item>
      <el-menu-item index="2">审核失败</el-menu-item>
      <el-menu-item index="9">上线</el-menu-item>
      <el-menu-item index="12">售出</el-menu-item>
    </el-menu>

    <el-table v-loading="loading" :data="infoData.records" element-loading-text="Loading" border fit highlight-current-row>

      <el-table-column align="center" label="序号" type="index">
      </el-table-column>
      <el-table-column align="center" label="图片">
        <template slot-scope="scope">
         <el-image style="height: 100px;width: 100px;border-radius: 10px" :src="scope.row.image"></el-image>
        </template>
      </el-table-column>

      <el-table-column align="center" label="标题">
        <template slot-scope="scope">
          {{ scope.row.title }}
        </template>
      </el-table-column>

      <el-table-column align="center" label="类型">
        <template slot-scope="scope">
          {{ scope.row.typeName }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="商品价格">
        <template slot-scope="scope">
          {{ $utils.convert.to_price(scope.row.price) }} 元
        </template>
      </el-table-column>
      <el-table-column align="center" label="状态">
        <template slot-scope="scope">
          {{ $utils.convert.to_text(scope.row.status, $utils.pro.productStatus) }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="创建时间">
        <template slot-scope="scope">
          {{ $utils.convert.parseTime(scope.row.createTime) }}
        </template>
      </el-table-column>

      <el-table-column label="操作" align="center">
        <template slot-scope="scope">
          <el-button
              size="mini"
              type="primary"
              @click="toDetail(scope.row)">查看详情
          </el-button>
          <br v-if="scope.row.status === 9">
          <el-button
              v-if="scope.row.status === 9"
              size="mini"
              type="warning"
              style="margin-top: 10px"
              @click="down(scope.row)">下架商品
          </el-button>
          <div v-if="scope.row.status === 1">
            <el-button
                style="margin-top: 10px"
                v-if="scope.row.status === 1"
                size="mini"
                type="success"
                @click="pass(scope.row)">审核通过
            </el-button>
            <br>
            <el-button
                style="margin-top: 10px"
                v-if="scope.row.status === 1"
                size="mini"
                type="danger"
                @click="fail(scope.row)">审核失败
            </el-button>
          </div>
        </template>
      </el-table-column>
      <el-table-column align="center" label="平台补贴" >
        <template slot-scope="scope">

            <el-button v-if="scope.row.productVoucher"
                       size="mini" :disabled="scope.row.status !== 9"
                       type="primary"
                       @click="addVoucher(scope.row)">编辑优惠卷
            </el-button>
            <el-button v-else
                       size="mini" :disabled="scope.row.status !== 9"
                       type="success"
                       @click="addVoucher(scope.row)">添加优惠卷
            </el-button>
            <br v-if="scope.row.productVoucher">
            <el-button  v-if="scope.row.productVoucher"
                       size="mini"
                       type="danger"
                        style="margin-top: 10px"
                        :disabled="scope.row.status !== 9"
                       @click="delVoucher(scope.row)">删除优惠卷
            </el-button>

        </template>
      </el-table-column>
    </el-table>
    <el-pagination style="display: flex;justify-content: center;margin-top: 10px"
                   layout="total,prev, pager, next,jumper" :total="infoData.total"
                   :current-page.sync="infoData.current"
                   @current-change="handleCurrentChange">
    </el-pagination>
    <el-dialog title="添加优惠卷" :visible.sync="dialog">
      <el-form :model="formData">

        <el-form-item label="折扣金额" label-width="120px">
          <el-input-number required controls-position="right" v-model="formData.voucherValue" :precision="2" :step="1" :min="0"></el-input-number>
        </el-form-item>
        <el-form-item label="抢购时间" label-width="120px">
          <el-date-picker
              v-model="formData.time"
              type="datetimerange"
              range-separator="至"
              start-placeholder="开始日期"
              end-placeholder="结束日期">
          </el-date-picker>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button type="primary" @click="postData">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
export default {
  data() {
    return {
      page_param: {
        key: "",
        pageNumber: 1,
        pageSize: 10,
        status:'',
      },
      formData:{
        productId:'',
        voucherValue:0,
        beginTime:'',
        endTime:'',
        time:'',
      },
      dialog:false,
      infoData: {},
      loading: true
    }
  },
  created() {
    this.getProductInfo()
  },
  methods: {
    getProductInfo() {
      this.$api.productInfo.getProductInfoPage(this.page_param).then(res => {
        this.infoData = res.result
        this.loading = false
        this.infoData.records.forEach(item => {
          if (item.image) {
            item.image = JSON.parse(item.image)[0]
          }
        })
      })
    },
    addVoucher(item){
      if(item.productVoucher){
        this.formData.voucherValue = this.$utils.convert.to_price(item.productVoucher.voucherValue)
        this.formData.time = [item.productVoucher.beginTime , item.productVoucher.endTime ]
      }
      this.formData.productId = item.id
      this.dialog = true
    },
    handleSelect(key){
      this.page_param.status = key
      this.page_param.pageNumber = 0;
      this.getProductInfo()
    },
    postData(){
      let data = {...this.formData}
      let start = new Date(data.time[0]);
      data.beginTime = Math.floor(start.getTime() / 1000);
      let end = new Date(data.time[1]);
      data.endTime = Math.floor(end.getTime() / 1000);
      this.$api.productInfo.addVoucher(data).then(res=>{
        this.$message({type: 'success', message: '操作成功!'});
        this.dialog = false
        this.getProductInfo()
      })
    },
    pass(item) {
      this.$confirm('确认审核通过?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.productInfo.passProduct(item.id).then(res => {
          this.$message({
            type: 'success',
            message: '操作成功!'
          });
          this.getProductInfo()
        })
      })
    },
    down(item){
      this.$confirm('确认下架?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.productInfo.downProduct(item.id).then(res => {
          this.$message({
            type: 'success',
            message: '操作成功!'
          });
          this.getProductInfo()
        })
      })
    },
    fail(item) {
      this.$confirm('确认审核失败?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.productInfo.failProduct(item.id).then(res => {
          this.$message({
            type: 'success',
            message: '操作成功!'
          });
          this.getProductInfo()
        })
      })
    },
    delVoucher(item){
      this.$confirm('确认删除?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.productInfo.delVoucher(item.productVoucher.id).then(res => {
          this.$message({type: 'success', message: '删除成功!'});
          this.getProductInfo()
        })
      })
    },
    handleCurrentChange(val) {
      this.page_param.pageNumber = val
      this.getProductInfo()
    },
    getQueryList(val) {
      this.page_param.key = val
      this.getProductInfo()
    },
    toDetail(row) {
      this.$router.push('/product/product_detail?id='+row.id)
    }
  }
}

</script>

