<template>
  <div class="container">
    <div class="push-container">
      <el-form label-position="left" class="check-form" :model="formData" :rules="rules" ref="form" label-width="80px"
        :show-message="false" status-icon :hide-required-asterisk="true" size="mini">
        <div class="header"><span class="header-icon"></span><span class="header-title">发布闲置</span></div>
        <div class="img-list">
          <el-upload v-model:file-list="fileList" action="/api/upload/image" list-type="picture-card"
            :on-preview="handlePreview" :on-remove="handleRemove" :on-success="handleUploadSuccess">
            <div style="line-height: 80px;font-size: 12px;color: #9999b3">+添加优质图</div>
          </el-upload>

          <el-dialog :visible.sync="dialog.visible">
            <img width="100%" :src="dialog.imageUrl" alt="预览图像">
          </el-dialog>
        </div>
        <div class="push-content">
          <el-form-item label-width="0" prop="title">
            <el-input style="margin-left: 15px;" v-model="formData.title" maxlength="20" show-word-limit type="text"
              placeholder="填写商品标题，可能会更容易卖出哦~" class="input-title" />
          </el-form-item>
          <el-form-item label-width="0" prop="intro">
            <el-input style="margin-left: 15px;" v-model="formData.intro" :rows="5" maxlength="200" show-word-limit
              type="textarea" placeholder="描述下宝贝的品牌型号、货品来源..." class="input-content" />
          </el-form-item>
        </div>
        <div class="btns">
          <!--          <button class="css-fm44j css-osq2ks dyn">-->
          <!--            <span class="btn-content"># 话题</span></button-->
          <!--          >-->
          <!--          <button class="css-fm44j css-osq2ks dyn">-->
          <!--          <span class="btn-content"-->
          <!--          ><div class="smile"></div>-->
          <!--            表情</span-->
          <!--          >-->
          <!--          </button>-->
          <el-form-item label="商品分类" prop="type" style="width:300px ">
            <el-select v-model="formData.type" placeholder="选择商品分类">
              <el-option v-for="(item, index) in menuList" :key="item.id" :label="item.typeName"
                :value="item.typeCode"></el-option>
            </el-select>
          </el-form-item>
          <el-form-item label="商品位置" prop="type" style="width:300px ">
            <el-select v-model="formData.city" placeholder="选择商品位置">
              <el-option v-for="(item, index) in placeList" :key="item.id" :label="item.typeName"
                :value="item.typeCode"></el-option>
            </el-select>
          </el-form-item>
          <el-form-item label="价格">
            <el-input-number required controls-position="right" v-model="formData.price" :precision="2" :step="1"
              :min="0"></el-input-number>
          </el-form-item>
        </div>
        <div style="margin-left: 20px">
          <el-divider style="width: 576px" />
        </div>
        <div class="submit">
          <button class="publishBtn" type="submit" @click.prevent="publish()">
            <span class="btn-content">发布</span>
          </button>
          <button class="clearBtn" @click="$router.push('/')">
            <span class="btn-content">取消</span>
          </button>
        </div>
      </el-form>
    </div>

    <!--抽屉 地址编辑-->
    <div>
      <el-drawer @close-drawer="closeDrawer" :show-close="false" :visible.sync="drawer" direction="rtl">
        <address_edit @close-drawer="closeDrawer"></address_edit>
      </el-drawer>
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
        title: "string",
        intro: "string",
        image: "string",
        price: 0,
        originalPrice: 0,
        postType: 0,
        adcode: "string",
        province: "string",
        city: "string",
        district: "string",
        type: "string"
      },
    }
  },
  created() {
    //this.getCity()
    //this.getMenuList()
    this.$api = api;
  },
  methods: {
    publish() {
      //图片处理
      if (this.fileList.length > 0) {
        //alert(this.fileList[0].url);
        //let _fileList = this.fileList.map(file => file.response.result.url);
        let _fileList = this.fileList.map(file => file.data);
        this.formData.image = JSON.stringify(_fileList)
      }
      this.$refs.form.validate(valid => {
        if (valid) {
          this.$api.product.createProductInfo(this.formData).then(res => {
            this.$router.push("/my")
            ElNotification({ type: 'success', title: '航游集市', message: '商品发布成功' })
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
/deep/ .push-content .el-input--mini .el-input__inner {
  height: 40px;
}

.check-form .el-form-item__content {
  margin-left: 0 !important;
}

/deep/ .el-drawer__header {
  display: none;
}

/deep/ .el-upload-list--picture-card .el-upload-list__item {
  width: 80px;
  height: 80px;
}

/deep/ .el-upload-list__item.is-success .el-upload-list__item-status-label {
  display: none;
}

/deep/ .el-upload--picture-card {
  width: 80px;
  height: 80px;
}

.container {
  flex: 1;
  padding-top: 72px;

  .push-container {
    margin-left: 12vw;
    width: 600px;
    border-radius: 8px;
    box-sizing: border-box;
    box-shadow: var(--el-box-shadow-lighter);

    .header {
      padding: 15px 0;
      line-height: 16px;
      font-size: 16px;
      font-weight: 400;

      .header-icon {
        position: relative;
        top: 2px;
        display: inline-block;
        width: 6px;
        height: 19px;
        background: #ff2442;
        border-radius: 3px;
        margin-right: 2px;
      }

      .header-title {
        line-height: 20px;
        font-size: 20px;
      }
    }

    .img-list {
      width: 540px;
      margin-left: 20px;
      padding: 0;
    }

    .push-content {
      padding: 10px 6px 10px 6px;
      position: relative;

      .scroll-tag-container {
        position: absolute;
        width: 98%;
        background-color: #fff;
        z-index: 99999;
        border: 1px solid #f4f4f4;
        height: 300px;
        overflow: auto;

        .scrollbar-tag-item {
          display: flex;
          align-items: center;
          height: 30px;
          margin: 10px;
          text-align: center;
          border-radius: 4px;
          padding-left: 2px;
          color: #484848;
          font-size: 14px;
        }

        .scrollbar-tag-item:hover {
          background-color: #f8f8f8;
        }
      }

      .input-title {
        margin-bottom: 5px;
        font-size: 12px;
      }

      .input-content {
        font-size: 12px;
      }
    }

    .btns {
      padding: 0 12px 0px 20px;

      button {
        min-width: 62px;
        width: 62px;
        margin: 0 6px 0 0;
        height: 18px;
      }

      .css-fm44j {
        -webkit-font-smoothing: antialiased;
        appearance: none;
        font-family: RedNum, RedZh, RedEn, -apple-system;
        vertical-align: middle;
        text-decoration: none;
        border: 1px solid rgb(217, 217, 217);
        outline: none;
        user-select: none;
        cursor: pointer;
        display: inline-flex;
        -webkit-box-pack: center;
        justify-content: center;
        -webkit-box-align: center;
        align-items: center;
        margin-right: 16px;
        border-radius: 4px;
        background-color: white;
        color: rgb(38, 38, 38);
        height: 24px;
        font-size: 12px;
      }
    }

    .categorys {
      padding: 0 12px 10px 12px;
    }

    .submit {
      padding: 0 12px 10px 20px;
      margin-top: 10px;

      button {
        width: 80px;
        height: 32px;
        font-size: 14px;
      }

      .publishBtn {
        background-color: #ff2442;
        color: #fff;
        border-radius: 4px;
        cursor: pointer;
      }

      .clearBtn {
        cursor: pointer;
        border-radius: 4px;
        margin-left: 10px;
        border: 1px solid rgb(217, 217, 217);
      }
    }
  }
}
</style>
