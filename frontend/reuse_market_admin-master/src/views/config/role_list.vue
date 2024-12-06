<template>
  <div class="app-container">
    <div class="demo-input-suffix">
      <el-input
          placeholder="请输入内容"
          prefix-icon="el-icon-search"
          v-model="page_param.key"
          @input="getQueryList"
          style="width: 200px;margin: 5px"
      >
      </el-input>
      <el-button type="primary" @click="add">添加角色</el-button>
    </div>
    <el-table
        v-loading="loading"
        :data="roleData.records"
        element-loading-text="Loading" border fit highlight-current-row>

      <el-table-column align="center" label="序号" type="index">
      </el-table-column>
      <el-table-column align="center" label="角色代码">
        <template slot-scope="scope">
          {{ scope.row.roleCode }}
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

            <el-button slot="reference" size="mini" @click="handleDelete(scope.row)" type="danger">删除</el-button>

        </template>
      </el-table-column>

    </el-table>
    <el-pagination style="display: flex;justify-content: center;margin-top: 10px"
                   layout="total,prev, pager, next,jumper" :total="roleData.total"
                   :current-page.sync="roleData.current"
                   @current-change="handleCurrentChange"
    >
    </el-pagination>
    <el-dialog :title="formTitle" :visible.sync="formVisible">
      <el-form :model="formData">
        <el-form-item label="角色代码" label-width="120px">
          <el-input v-model="formData.roleCode" :disabled="formTitle==='编辑角色'"></el-input>
        </el-form-item>
        <el-form-item label="角色名称" label-width="120px">
          <el-input v-model="formData.roleName"></el-input>
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
      formData: {
        id: "",
        roleCode: "",
        roleName: "",
      },
      formTitle: "",
      formVisible: false,
      page_param: {
        key: "",
        pageNumber: 1,
        pageSize: 10,
      },
      roleData: {},
      loading: true
    }
  },
  created() {
    this.getRoleList()
  },
  methods: {
    getRoleList() {
      this.$api.role.getRolePage(this.page_param).then(res => {
        this.roleData = res.result
        this.loading = false
      })
    },
    add() {
      this.formData = {}
      this.formVisible = true
      this.formTitle = '添加角色'
    },
    edit(item) {
      this.formData.id = item.id
      this.formData.roleCode = item.roleCode
      this.formData.roleName = item.roleName

      this.formVisible = true
      this.formTitle = '编辑角色'
    },
    getQueryList(val) {
      this.page_param.key = val
      this.getRoleList()
    },
    postData() {
      if (this.formTitle === '添加角色') {
        this.$api.role.createRole(this.formData).then(res => {
          this.formVisible = false
          this.getRoleList();
        })
      } else if (this.formTitle === '编辑角色') {
        this.$api.role.editRole(this.formData).then(res => {
          this.formVisible = false
          this.getRoleList();
        })
      }
    },
    handleCurrentChange(val){
      this.page_param.pageNumber = val
      this.getRoleList()
    },
    handleDelete(row) {
      this.$api.role.delRole({id: row.id}).then(res => {
        this.getRoleList();
      })
    }
  }
}

</script>

