<template>
  <div style="background-color: #f7f7f7;height: 100%;width: 100%; padding: 15px;">
    <div class="user-card">
      <el-form ref="form" :model="form">
        <div class="description">
          <el-descriptions :column="1" :colon="false" direction="vertical">
            <el-descriptions-item label="姓名" v-if="type==='用户自提'">
              <input class="input" type="text" placeholder="请输入联系人姓名" v-model="form.username"/>
            </el-descriptions-item>
            <el-descriptions-item label="电话" v-if="type==='用户自提'">
              <input class="input" type="text" placeholder="请输入联系人电话" v-model="form.phone"/>
            </el-descriptions-item>
            <el-descriptions-item label="地址" v-if="type==='用户自提'">
              <textarea class="textarea" placeholder="请输入提货地址" v-model="form.address"></textarea>
            </el-descriptions-item>
            <el-descriptions-item label="物流公司" v-if="type==='物流发货'">
              <input class="input" type="text" placeholder="请填写物流公司" v-model="form.postCompany"/>
            </el-descriptions-item>
            <el-descriptions-item label="物流单号" v-if="type==='物流发货'">
              <input class="input" type="text" placeholder="请填写物流单号" v-model="form.postNum"/>
            </el-descriptions-item>
          </el-descriptions>
        </div>
        <el-form-item>
          <el-button type="primary" @click="onSubmit" size="mini">确认</el-button>
          <el-button size="mini" @click="cancel">取消</el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>
<script>
export default {
  props: {
    productOrderId: '',
    type: '',
  },
  data() {
    return {
      form: {
        productOrderId: '',
        username: '',
        phone: '',
        address: '',
        postCompany: '',
        postNum: '',
      },
    }
  },
  created() {
    this.form.productOrderId = this.productOrderId
  },
  methods: {
    onSubmit() {
      if (this.type === '物流发货') {
        this.createPost()
      } else if (this.type === '用户自提') {
        this.createPostSelf()
      }
    },
    createPostSelf() {
      this.$api.productOrder.postSelfProductOrder(this.form).then(res => {
        this.cancel();
      })
    },
    createPost() {
      this.$api.productOrder.postProductOrder(this.form).then(res => {
        this.cancel();
      })
    },
    cancel() {
      this.$emit('close-drawer')
    },
  }
}
</script>
<style scoped lang="less">
.user-card {
  height: 100%;
  border-radius: 10px;
  background-color: white;
  padding: 10px;

  .description {


    .input {
      padding: 12px 16px 12px 16px;
      width: 60%;
      height: 25px;
      line-height: 25px;
      background: rgba(0, 0, 0, 0.03);
      caret-color: rgba(51, 51, 51, 0.3);
      border-radius: 10px;
      border: none;
      outline: none;
      resize: none;
      color: #333;
    }

    .textarea {
      padding: 12px 16px 12px 16px;
      width: 100%;
      height: 70px;
      line-height: 16px;
      background: rgba(0, 0, 0, 0.03);
      caret-color: rgba(51, 51, 51, 0.3);
      border-radius: 10px;
      border: none;
      outline: none;
      resize: none;
      color: #333;
    }
  }
}

/deep/ .avatar-uploader .el-upload {
  border: 1px dashed #d9d9d9;
  border-radius: 100%;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}

/deep/ .avatar-uploader .el-upload:hover {
  border-color: #409EFF;
}

/deep/ .avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 100px;
  height: 100px;
  line-height: 100px;
  text-align: center;
}

/deep/ .avatar {
  width: 100px;
  height: 100px;
  display: block;
}
</style>