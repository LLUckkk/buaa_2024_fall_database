<template>
  <div style="padding: 20px;" >
    <div style="display: flex;align-items: center;justify-content: space-between">
      <div style="font-size: 25px;color: black;">我的地址</div>
      <div >
        <el-button size="mini" type="primary" @click="toAdd" style="margin-left: 20px">
          添加地址
        </el-button>
      </div>
    </div>
    <div style="margin-top: 20px">
      <div v-for="(item,index) in addressList" @click="select(item)"
           style="margin-bottom: 15px;background-color: #f7f7f7" :key="item.uuid"
           class="depot-item" :class="item.id===address.id?'active':''">
        <div class="ok-box-flex">
          <div class="depotFont" style="padding: 10px;width: 100%;display: flex">
            <div style="width: 5px">
              <el-tag type="info" size="mini">{{ index + 1 }}</el-tag>
            </div>
            <div style="margin-left: 20px;display: flex">
              <div style="margin-left: 5px;">
                <div style="display: flex">
                  <div style="width: 90px;">
                    <i class="el-icon-s-custom" style="margin-right: 8px; color: #727272"></i>
                    {{ item.name }}
                  </div>
                  <div style="margin-left: 20px;width: 200px">
                    <i class="el-icon-phone" style="margin-right: 8px; color: #727272"></i>
                    {{ item.phone }}
                  </div>
                </div>
                <div>
                  <i class="el-icon-s-home" style=" color: #727272;margin-top: 20px"></i>
                  {{ item.address }} <i class="el-icon-delete" style="color: #727272;margin-left: 10px" @click="del(item)"></i>
                </div>
              </div>
            </div>
          </div>

        </div>
      </div>
      <div slot="footer" v-if="button" class="dialog-footer" style="display: flex;justify-content: center">
        <el-button @click="cancel">取 消</el-button>
        <el-button type="primary" @click="confirm()">确 定</el-button>
      </div>
    </div>

    <div class="add">
      <el-dialog title="添加地址" :visible.sync="dialog" append-to-body>
        <el-form :model="form">
          <el-form-item label="姓名" :label-width="'100px'">
            <el-input v-model="form.name" autocomplete="off"></el-input>
          </el-form-item>
          <el-form-item label="电话" :label-width="'100px'">
            <el-input v-model="form.phone" autocomplete="off"></el-input>
          </el-form-item>
          <el-form-item label="详细地址" :label-width="'100px'">
            <el-input v-model="form.address" autocomplete="off"></el-input>
          </el-form-item>
        </el-form>
        <div slot="footer" class="dialog-footer">
          <el-button @click="dialog = false">取 消</el-button>
          <el-button type="primary"  @click="add()">确 定</el-button>
        </div>
      </el-dialog>
    </div>
  </div>
</template>

<script>


import {Notification} from "element-plus";

export default {
props:{
  button:{
    type:Boolean,
    default:false
  }
},
  data() {
    return {
      dialog: false,
      addressList: [],
      address: {
        id: "",
        name: '',
        phone: '',
        address: '',
      },
      form: {
        name: '',
        phone: '',
        address: '',
      },

    }
  },
  created() {
  this.form = {
    name: '',
    phone: '',
    address: '',
  }
    this.getList()
  },
  methods: {
    getList() {
      this.$api.userAddress.getAddressList().then(res => {
        this.addressList = res.result;
      })
    },
    add() {
      this.$api.userAddress.saveAddress(this.form).then(res => {
        this.getList()
        this.dialog = false
        Notification({type: 'success', title: '我的地址', message: '添加成功'})
      })
    },
    del(item){
      this.$api.userAddress.deleteAddress(item.id).then(res=>{
        this.getList()
        Notification({type: 'success', title: '我的地址', message: '删除成功'})
      })
    },
    toAdd() {
      this.dialog = true
    },
    select(item) {
      this.address = item
    },
    confirm(){
      this.$emit('confirm',this.address)
    },
    cancel(){
      this.$emit('close-drawer')
    },
  }
}
</script>
<style scoped>
 .add .el-dialog__wrapper {
  z-index: 90000000 ;
}
.depot-item {
  /* 初始状态样式 */
  border: 1px solid transparent;
  border-radius: 3px;
  transition: border-color 0.3s ease-in-out; /* 添加过渡效果 */
  /* 设置可点击样式，可根据需要自定义 */
  cursor: pointer;
}

.depot-item:hover {
  border-color: rgb(9, 109, 217);
}

.depotFont {
  transition: border-color 0.3s ease-in-out; /* 添加过渡效果 */
  /* 设置可点击样式，可根据需要自定义 */
  cursor: pointer;
}

.depotFont:hover {
  color: #2d6cd3;
}

.active {
  border-color: rgb(9, 109, 217);
  color: #2d6cd3;
}

.top {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  width: 100%;
  padding-top: 20px;
  padding-left: 30px;
  background-color: #eeeeee
}

</style>