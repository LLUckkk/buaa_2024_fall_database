<template>
  <div class="app-container">
    <div class="demo-input-suffix">
      <el-input
          placeholder="用户昵称、闲宝号"
          prefix-icon="el-icon-search"
          v-model="page_param.key"
          @input="getQueryList"
          style="width: 300px;margin: 5px"
      >
      </el-input>

    </div>
    <el-table
        v-loading="loading"
        :data="userData.records"
        element-loading-text="Loading" border fit highlight-current-row>
      <el-table-column align="center" label="序号" type="index">
      </el-table-column>
      <el-table-column align="center" label="头像">
        <template slot-scope="scope">
          <el-avatar :size="60" :src="scope.row.avatar"></el-avatar>
        </template>
      </el-table-column>
      <el-table-column align="center" label="昵称">
        <template slot-scope="scope">
          {{ scope.row.nickName }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="闲宝号">
        <template slot-scope="scope">
          {{ scope.row.number }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="手机号">
        <template slot-scope="scope">
          {{ scope.row.phone }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="状态">
        <template slot-scope="scope">
          <el-tag type="success" v-if="scope.row.status===9">正常</el-tag>
          <el-tag type="danger" v-if="scope.row.status===0">停用</el-tag>
        </template>
      </el-table-column>
      <el-table-column align="center" label="创建时间">
        <template slot-scope="scope">
          {{ $utils.convert.parseTime(scope.row.createTime) }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="更新时间">
        <template slot-scope="scope">
          {{ $utils.convert.parseTime(scope.row.updateTime) }}
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center">
        <template slot-scope="scope">
          <el-button v-if="scope.row.status===9" size="mini" type="danger" @click="handleDisable(scope.row)">禁用</el-button>
          <el-button v-if="scope.row.status===0" size="mini" type="success" @click="handleAble(scope.row)">启用</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination style="display: flex;justify-content: center;margin-top: 10px"
                   layout="total,prev, pager, next,jumper" :total="userData.total"
                   :current-page.sync="userData.current"
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
      userData: {},
      loading: true
    }
  },
  created() {
    this.getUserList()
  },
  methods: {
    getUserList() {
      this.$api.webUser.getWebUserPage(this.page_param).then(res => {
        this.userData = res.result
        this.loading = false
      })
    },
    handleCurrentChange(val) {
      this.page_param.pageNumber = val
      this.getUserList()
    },
    getQueryList(val) {
      this.page_param.key = val
      this.getUserList()
    },
    handleDisable(row) {
      this.$confirm('此操作将停用此用户账号, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.webUser.disableUser(row.id).then(res => {
          this.getUserList();
          this.$message({type: 'success', message: '操作成功!'});
        })
      }).catch(() => {
        this.$message({type: 'info', message: '已取消'});
      });
    },
    handleAble(row){
      this.$confirm('此操作将启用此用户账号, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.webUser.ableUser(row.id).then(res => {
          this.getUserList();
          this.$message({type: 'success', message: '操作成功!'});
        })
      }).catch(() => {
        this.$message({type: 'info', message: '已取消'});
      });
    }
  },
}

</script>

