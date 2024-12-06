<template>
  <div class="app-container">
    <div class="demo-input-suffix">
      <el-input
          placeholder="请输入订单编号"
          prefix-icon="el-icon-search"
          v-model="page_param.key"
          @input="getQueryList"
          style="width: 200px;margin: 5px"
      >
      </el-input>
    </div>
    <el-table v-loading="loading" :data="orderData.records" element-loading-text="Loading" border fit highlight-current-row>
      <el-table-column align="center" label="序号" type="index">
      </el-table-column>
      <el-table-column align="center" label="订单编号">
        <template slot-scope="scope">
          {{ scope.row.orderNumber }}
        </template>
      </el-table-column>

      <el-table-column align="center" label="支付金额">
        <template slot-scope="scope">
          {{ $utils.convert.to_price(scope.row.payPrice) }} 元
        </template>
      </el-table-column>
      <el-table-column align="center" label="支付类型">
        <template slot-scope="scope">
          {{ scope.row.payTypeName }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="支付状态">
        <template slot-scope="scope">
          {{ $utils.convert.to_text(scope.row.paymentStatus, $utils.pro.paymentStatus) }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="创建时间">
        <template slot-scope="scope">
          {{$utils.convert.formatISOTime(scope.row.timeCreate) }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="更新时间">
        <template slot-scope="scope">
          {{ $utils.convert.formatISOTime(scope.row.timeUpdate) }}
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center">
        <template slot-scope="scope">
          <el-button size="mini" type="primary" @click="toDetail(scope.row)">查看详情</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination style="display: flex;justify-content: center;margin-top: 10px"
                   layout="total,prev, pager, next,jumper" :total="orderData.total"
                   :current-page.sync="orderData.current"
                   @current-change="handleCurrentChange">
    </el-pagination>
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
      },
      orderData: {},
      loading: true
    }
  },
  created() {
    this.getPaymentOrder()
  },
  methods: {
    getPaymentOrder() {
      this.$api.paymentOrder.getPaymentOrderPage(this.page_param).then(res => {
        this.orderData = res.result
        this.loading = false
      })
    },
    handleCurrentChange(val) {
      this.page_param.pageNumber = val
      this.getPaymentOrder()
    },
    getQueryList(val) {
      this.page_param.key = val
      this.getPaymentOrder()
    },
    postData() {
      if (this.formTitle === '添加用户') {
        this.$api.user.createUser(this.formData).then(res => {
          this.formVisible = false
          this.getPaymentOrder();
        })
      } else if (this.formTitle === '编辑用户') {
        this.$api.user.editRole(this.formData).then(res => {
          this.formVisible = false
          this.getPaymentOrder();
        })
      }
    },
    toDetail(row){
      this.$router.push('/order/payment_order_detail?id=' + row.id)
    },
    handleDelete(row) {
      this.$api.user.delUser({id: row.id}).then(res => {
        this.getPaymentOrder();
      })
    }
  }
}

</script>

