<template>
  <div class="app-container">
    <div class="demo-input-suffix">
      <el-input
          placeholder="分类代码、分类名称"
          prefix-icon="el-icon-search"
          v-model="page_param.key"
          @input="getQueryList"
          style="width: 300px;margin: 5px"
      >
      </el-input>
      <el-button type="primary" @click="add">添加商品分类</el-button>
    </div>
    <el-table
        v-loading="loading"
        :data="typeData.records"
        element-loading-text="Loading" border fit highlight-current-row>

      <el-table-column align="center" label="序号" type="index">
      </el-table-column>
      <el-table-column align="center" label="分类代码">
        <template slot-scope="scope">
          {{ scope.row.typeCode }}
        </template>
      </el-table-column>

      <el-table-column align="center" label="分类名称">
        <template slot-scope="scope">
          {{ scope.row.typeName }}
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
          <el-button size="mini" @click="edit(scope.row)" type="primary">编辑</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination style="display: flex;justify-content: center;margin-top: 10px"
                   layout="total,prev, pager, next,jumper" :total="typeData.total"
                   :current-page.sync="typeData.current"
                   @current-change="handleCurrentChange">
    </el-pagination>
    <el-dialog :title="formTitle" :visible.sync="formVisible">
      <el-form :model="formData">
        <el-form-item label="分类代码"  label-width="120px">
          <el-input v-model="formData.typeCode" :disabled="formTitle==='编辑分类'"></el-input>
        </el-form-item>
        <el-form-item label="分类名称" label-width="120px">
          <el-input v-model="formData.typeName"></el-input>
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
        typeName: "",
        typeCode:"",
      },
      formTitle: "",
      formVisible: false,
      page_param: {
        key: "",
        pageNumber: 1,
        pageSize: 10,
      },
      typeData: {},
      loading: true
    }
  },
  created() {
    this.getTypeList()
  },
  methods: {
    getTypeList() {
      this.$api.productType.getProductTypePage(this.page_param).then(res => {
        this.typeData = res.result
        this.loading = false
      })
    },
    handleCurrentChange(val) {
      this.page_param.pageNumber = val
      this.getTypeList()
    },
    add() {
      Object.keys(this.formData).forEach(key => (this.formData[key] = ''))
      this.formTitle = "添加分类"
      this.formVisible = true;
    },
    edit(item) {
      this.formData.id = item.id
      this.formData.typeName = item.typeName
      this.formData.typeCode = item.typeCode
      this.formTitle = "编辑分类"
      this.formVisible = true;
    },
    getQueryList(val) {
      this.page_param.key = val
      this.getTypeList()
    },
    postData() {
      if(this.formTitle === '添加分类'){
        this.$api.productType.createProdcutType(this.formData).then(res => {
          this.formVisible = false
          this.getTypeList();
        })
      }else if(this.formTitle === '编辑分类'){
        this.$api.productType.editProductType(this.formData).then(res => {
          this.formVisible = false
          this.getTypeList();
        })
      }
    },
  }
}

</script>

