<template>
  <div class="app-container">
    <div class="demo-input-suffix">
      <el-input
          placeholder="用户名、姓名、角色名称"
          prefix-icon="el-icon-search"
          v-model="page_param.key"
          @input="getQueryList"
          style="width: 200px;margin: 5px"
      >
      </el-input>
      <el-button type="primary" @click="add">添加管理员</el-button>
    </div>
    <el-table
        v-loading="loading"
        :data="userData.records"
        element-loading-text="Loading" border fit highlight-current-row>

      <el-table-column align="center" label="序号" type="index">
      </el-table-column>
      <el-table-column align="center" label="用户名">
        <template slot-scope="scope">
          {{ scope.row.username }}
        </template>
      </el-table-column>

      <el-table-column align="center" label="姓名">
        <template slot-scope="scope">
          {{ scope.row.name }}
        </template>
      </el-table-column>
      <el-table-column align="center" label="角色名称">
        <template slot-scope="scope">
          {{ scope.row.roleName }}
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
          <el-button
              size="mini"
              @click="edit(scope.row)">编辑
          </el-button>
          <el-button
              size="mini"
              type="danger"
              @click="handleDelete(scope.row)">删除
          </el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination style="display: flex;justify-content: center;margin-top: 10px"
                   layout="total,prev, pager, next,jumper" :total="userData.total"
                   :current-page.sync="userData.current"
                   @current-change="handleCurrentChange">
    </el-pagination>
    <el-dialog :title="formTitle" :visible.sync="formVisible">
      <el-form :model="formData">
        <el-form-item label="用户名" label-width="120px">
          <el-input v-model="formData.username"></el-input>
        </el-form-item>
        <el-form-item  label="密码" label-width="120px">
          <el-input type="password" v-model="formData.password"></el-input>
        </el-form-item>
        <el-form-item label="姓名" label-width="120px">
          <el-input v-model="formData.name"></el-input>
        </el-form-item>
        <el-form-item label="角色" label-width="120px">
          <el-select v-model="formData.roleCode" placeholder="请选择角色">
            <el-option v-for="item in roleOptions" :key="item.roleCode" :label="item.roleName" :value="item.roleCode">
            </el-option>
          </el-select>
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
      roleOptions:[],
      formData: {
        id: "",
        username: "",
        name: "",
        password: "",
        roleCode: '',
      },
      formTitle: "",
      formVisible: false,
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
      this.$api.user.getUserList(this.page_param).then(res => {
        this.userData = res.result
        this.loading = false
      })
    },
    getRoleList(){
      this.$api.role.getRoleList().then(res=>{
        this.roleOptions = res.result
      })
    },
    add() {
      Object.keys(this.formData).forEach(key => (this.formData[key] = ''))
      this.formTitle = "添加用户"
      this.formVisible = true;
      this.getRoleList()
    },
    handleCurrentChange(val) {
      this.page_param.pageNumber = val
      this.getUserList()
    },
    edit(item) {
      this.formData.id = item.id
      this.formData.username = item.username
      this.formData.name = item.name
      this.formData.roleCode = item.roleCode

      this.formTitle = "编辑用户"
      this.formVisible = true;
      this.getRoleList()
    },
    getQueryList(val) {
      this.page_param.key = val
      this.getUserList()
    },
    postData() {
      if(this.formTitle === '添加用户'){
        this.$api.user.createUser(this.formData).then(res => {
          this.formVisible = false
          this.getUserList();
        })
      }else if(this.formTitle === '编辑用户'){
        this.$api.user.editUser(this.formData).then(res => {
          this.formVisible = false
          this.getUserList();
        })
      }
    },
    handleDelete(row) {
      this.$confirm('此操作将永久删除该用户, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.user.delUser({id: row.id}).then(res => {
          this.getUserList();
          this.$message({type: 'success', message: '删除成功!'});
        })
      }).catch(() => {
        this.$message({type: 'info', message: '已取消删除'});
      });

    }
  }
}

</script>

