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
      <el-table-column align="center" label="闲宝号">
        <template slot-scope="scope">
          {{ scope.row.number }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="申请修改头像">
        <template slot-scope="scope">
          <el-avatar :size="60" v-if="scope.row.checkAvatar" :src="scope.row.checkAvatar"></el-avatar>
          <el-avatar :size="60"  v-else :src="scope.row.avatar"></el-avatar>
        </template>
      </el-table-column>
      <el-table-column align="center" label="申请修改昵称">
        <template slot-scope="scope">
          {{ scope.row.checkNickName}}
        </template>
      </el-table-column>
      <el-table-column align="center" label="申请修改简介">
        <template slot-scope="scope">
          {{ scope.row.checkIntro}}
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

      <el-table-column label="操作" align="center">
        <template slot-scope="scope">
          <el-button v-if="scope.row.checkStatus===0" size="mini" type="success" @click="handleDisable(scope.row)">审核通过</el-button>
          <el-button v-if="scope.row.checkStatus===0" size="mini" type="danger" @click="handleAble(scope.row)">审核失败</el-button>
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
        checkStatus:0,
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
      this.$confirm('确认审核通过?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.webUser.successInfo(row.id).then(res => {
          this.getUserList();
          this.$message({type: 'success', message: '操作成功!'});
        })
      }).catch(() => {
        this.$message({type: 'info', message: '已取消'});
      });
    },
    handleAble(row) {
      this.$confirm('确认审核失败?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.webUser.failInfo(row.id).then(res => {
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

