<template>
  <div class="app-container">
    <div class="demo-input-suffix">
      <el-input
          placeholder="所属商品、发布昵称、评论内容"
          prefix-icon="el-icon-search"
          v-model="page_param.key"
          @input="getQueryList"
          style="width: 300px;margin: 5px"
      >
      </el-input>
    </div>
    <el-table v-loading="loading" :data="infoData.records" element-loading-text="Loading" border fit highlight-current-row>
      <el-table-column align="center" label="序号" type="index">
      </el-table-column>
      <el-table-column align="center" width="190" label="所属商品">
        <template slot-scope="scope">
          {{ scope.row.productId }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="发布用户昵称">
        <template slot-scope="scope">
          {{ scope.row.pubNickname }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="评论内容">
        <template slot-scope="scope">
          {{ scope.row.content }}
        </template>
      </el-table-column>
      <el-table-column align="center" width="190" label="发表时间">
        <template slot-scope="scope">
          {{ $utils.convert.parseTime(scope.row.createTime) }}
        </template>
      </el-table-column>
      <el-table-column label="操作" width="200" align="center">
        <template slot-scope="scope">
          <el-button
              size="mini"
              type="danger"
              @click="handleDisable(scope.row)">删除
          </el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination style="display: flex;justify-content: center;margin-top: 10px"
                   layout="total,prev, pager, next,jumper" :total="infoData.total"
                   :current-page.sync="infoData.current"
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
      infoData: {},
      loading: true
    }
  },
  created() {
    this.getComment()
  },
  methods: {
    getComment() {
      this.$api.comment.getCommentPage(this.page_param).then(res => {
        this.infoData = res.result
        this.loading = false
      })
    },
    getQueryList(val) {
      this.page_param.key = val
      this.getComment()
    },
    handleCurrentChange(val) {
      this.page_param.pageNumber = val
      this.getComment()
    },
    handleDisable(row) {
      this.$confirm('此操作将永久删除该条评论, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.comment.delComment(row.id).then(res=>{
          this.getComment();
         1
        })
      }).catch(() => {
        this.$message({type: 'info', message: '已取消删除'});
      });
    }
  }
}

</script>

