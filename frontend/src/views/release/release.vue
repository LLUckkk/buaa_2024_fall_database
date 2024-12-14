<template>
  <div class="release-container">
    <div class="release-card">
      <!-- 标题区域 -->
      <div class="card-header">
        <div class="title-bar"></div>
        <h2>发布闲置</h2>
      </div>

      <!-- 表单区域 -->
      <el-form 
        :model="formData" 
        :rules="rules" 
        ref="form"
        label-position="top"
        class="release-form"
      >
        <!-- 图片上传区 -->
        <div class="upload-section">
          <h3>商品图片</h3>
          <el-upload
            v-model:file-list="fileList"
            :http-request="uploadImage"
            list-type="picture-card"
            :on-preview="handlePreview"
            :on-remove="handleRemove"
            :on-success="handleUploadSuccess"
          >
            <div class="upload-tip">
              <i class="el-icon-plus"></i>
              <span>添加图片</span>
            </div>
          </el-upload>
        </div>

        <!-- 基本信息区 -->
        <div class="basic-info">
          <el-form-item prop="title">
            <el-input
              v-model="formData.title"
              maxlength="20"
              show-word-limit
              placeholder="请输入商品标题"
            />
          </el-form-item>

          <el-form-item prop="intro">
            <el-input
              v-model="formData.intro"
              type="textarea"
              :rows="4"
              maxlength="200"
              show-word-limit
              placeholder="描述商品的品牌型号、使用情况等信息"
            />
          </el-form-item>
        </div>

        <!-- 商品详情区 -->
        <div class="product-details">
          <el-row :gutter="20">
            <el-col :span="12">
              <el-form-item label="商品分类" prop="type">
                <el-select v-model="formData.type" placeholder="选择分类" class="full-width">
                  <el-option
                    v-for="item in menuList"
                    :key="item.id"
                    :label="item.typeName"
                    :value="item.typeCode"
                  />
                </el-select>
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item label="商品位置" prop="city">
                <el-select v-model="formData.city" placeholder="选择位置" class="full-width">
                  <el-option
                    v-for="item in placeList"
                    :key="item.id"
                    :label="item.typeName"
                    :value="item.typeCode"
                  />
                </el-select>
              </el-form-item>
            </el-col>
          </el-row>

          <el-row>
            <el-col :span="12">
              <el-form-item label="价格">
                <el-input-number
                  v-model="formData.price"
                  :precision="2"
                  :step="1"
                  :min="0"
                  controls-position="right"
                  class="price-input"
                />
              </el-form-item>
            </el-col>
          </el-row>
        </div>

        <!-- 操作按钮区 -->
        <div class="action-buttons">
          <el-button type="primary" @click="publish">发布商品</el-button>
          <el-button @click="$router.push('/')">取消</el-button>
        </div>
      </el-form>
    </div>
  </div>
</template>

<script>
//import gdMapUtil from "@/utils/gdMapUtil";
//import Address_edit from "@/views/release/address_edit.vue";
import { ElNotification } from "element-plus";
import api from "@/api";

export default {
  //components: {Address_edit},
  components: {},
  data() {
    return {
      drawer: false,
      dialog: {
        visible: false,
        imageUrl: '',
      },
      location: "",
      fileList: [],
      menuList: [{ typeCode: '', typeName: '全部' }, { typeCode: '1', typeName: '电子产品' }, { typeCode: '2', typeName: '书籍资料' },
      { typeCode: '3', typeName: '服装首饰' }, { typeCode: '4', typeName: '食物饮品' }, { typeCode: '5', typeName: '生活用品' },
      { typeCode: '6', typeName: "学习用品" }, { typeCode: '7', typeName: "其他" }],
      placeList: [{ typeCode: '1', typeName: '新北区' }, { typeCode: '2', typeName: '大运村' },
      { typeCode: '3', typeName: '合一区' }, { typeCode: '4', typeName: '沙河西区' }, { typeCode: '5', typeName: '沙河东区' }],
      formData: {
        title: "",
        intro: "",
        image: "",
        price: 0,
        originalPrice: 0,
        postType: 0,
        adcode: "",
        province: "",
        city: "",
        district: "",
        type: "",
        likeCount: 0
      },
    }
  },
  created() {
    //this.getCity()
    //this.getMenuList()
    this.$api = api;
  },
  methods: {
    uploadImage(options) {
      const { file } = options
      let formData = new FormData();
      formData.append('file', file);
      this.$api.upload.uploadImage(formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      }).then(res => {
        options.onSuccess(res.data, file)
      }).catch(() => {
        options.onError(new Error('Upload failed'), file)
      })
    },
    publish() {
      //图片处理
      if (this.fileList.length > 0) {
        let _fileList = this.fileList.map(file => file.url);
        this.formData.image = JSON.stringify(_fileList)
      }

      // 添加数据验证
      if (!this.formData.price) {
        this.formData.price = 0
      }
      if (!this.formData.originalPrice) {
        this.formData.originalPrice = this.formData.price // 如未设置原价，使用当前价格
      }
      if (!this.formData.likeCount) {
        this.formData.likeCount = 0
      }

      this.$refs.form.validate(valid => {
        if (valid) {
          // 打印发送的数据，用于调试
          console.log('准备发送的商品数据:', this.formData)
          
          this.$api.product.createProductInfo(this.formData).then(res => {
            console.log('创建商品响应:', res) // 添加响应日志
            this.$router.push("/my")
            ElNotification({ type: 'success', title: '航游集市', message: '商品发布成功' })
          }).catch(err => {
            console.error('创建商品失败:', err) // 添加错误日志
            ElNotification({ type: 'error', title: '航游集市', message: '商品发布失败，请重试' })
          })
        }
      })
    },
    getMenuList() {
      this.$api.productType.getTypeList().then(res => {
        this.menuList = res.data
      })
    },
    selectAdd() {
      this.drawer = true;
    },
    getCity() {
      // gdMapUtil.getCity().then(res=>{
      //   this.formData.province = res.province
      //   this.formData.city = res.city
      //   this.formData.adcode = res.adcode
      // });
    },

    handlePreview(file) {
      //alert("preview will exe!!");
      this.dialog.imageUrl = file.url;
      //alert(file.url);
      this.dialog.visible = true;
    },
    handleRemove(file, fileList) {
      this.$api.image.removeImage(file.response.result.name).then(res => {
        this.fileList = fileList
      })
    },
    handleUploadSuccess(response, file, fileList) {
      //alert("success!");
      file.url = response
      //alert(file.url)
      this.fileList = fileList
    },
    closeDrawer(changeAddress) {
      if (changeAddress) {
        l.GeocoderGetAddress(changeAddress.location.lng, changeAddress.location.lat).then(res => {
          this.formData.adcode = res.regeocode.addressComponent.adcode
          this.formData.province = res.regeocode.addressComponent.province
          this.formData.city = res.regeocode.addressComponent.city
          this.formData.district = res.regeocode.addressComponent.district
        })
      }
    },
  },
  computed: {
    rules() {
      return {
        postType: [{ required: true, message: '请选择发货方式~', trigger: 'blur' }],
        intro: [{ required: true, message: '有商品描述才能发布哦~', trigger: 'blur' }],
        title: [{ required: true, message: '有商品标题才能发布哦~', trigger: 'blur' }],
        type: [{ required: true, message: '选择商品分类才能发布哦~', trigger: 'blur' }],
      }
    }
  }
}
</script>
<style lang="less" scoped>
.release-container {
  min-height: 100vh;
  background-color: #f5f7fa;
  padding: 40px 0;
  margin-top: 30px;
  font-family: 'Helvetica Neue', Arial, sans-serif;
  font-size: 16px;

  .release-card {
    max-width: 800px;
    margin: 0 auto;
    background: #fff;
    border-radius: 8px;
    box-shadow: 0 2px 12px rgba(0, 0, 0, 0.1);
    padding: 30px;
    margin-top: 10px;
    font-family: 'Helvetica Neue', Arial, sans-serif;

    .card-header {
      display: flex;
      align-items: center;
      margin-bottom: 30px;

      .title-bar {
        width: 4px;
        height: 20px;
        background: #409EFF;
        margin-right: 10px;
        border-radius: 2px;
      }

      h2 {
        margin: 0;
        font-size: 28px;
        font-weight: 600;
        color: #333;
      }
    }

    .release-form {
      .upload-section {
        margin-bottom: 30px;
      }

      .basic-info {
        margin-bottom: 30px;
      }

      .product-details {
        margin-bottom: 30px;

        .price-input {
          width: 100%;
          max-width: 200px;
        }

        :deep(.el-form-item__label) {
          padding: 0;
          margin-bottom: 8px;
          margin-right: 280px;
          font-size: 14px;
          color: #666;
        }
      }

      .full-width {
        width: 100%;
      }

      .action-buttons {
        text-align: center;
        margin-top: 40px;

        .el-button {
          font-size: 16px;
          padding: 10px 20px;
        }
      }
    }
  }
}
</style>
