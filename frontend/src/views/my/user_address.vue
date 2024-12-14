<template>
  <div style="padding: 20px;">
    <div style="display: flex;align-items: center;justify-content: space-between">
      <div style="font-size: 25px;color: black;">我的地址</div>
      <div>
        <el-button size="mini" type="primary" @click="toAdd" style="margin-left: 20px">
          添加地址
        </el-button>
      </div>
    </div>
    <div style="margin-top: 20px">
      <div v-if="addressList.length === 0" class="empty-state">
        <el-empty description="暂无地址信息" />
      </div>

      <div v-else v-for="(item, index) in addressList" 
           @click="select(item)"
           :key="item.uuid" 
           class="address-card"
           :class="{'address-card--active': item.id === address.id}">
        <div class="address-card__content">
          <div class="address-card__index">
            <el-tag type="info" size="small" effect="plain">{{ index + 1 }}</el-tag>
          </div>
          <div class="address-card__details">
            <div class="address-card__row">
              <div class="address-card__info">
                <el-icon class="address-card__icon"><User /></el-icon>
                <span class="address-card__text" @click.stop="editItem(item)">{{ item.name }}</span>
              </div>
              <div class="address-card__info address-card__info--phone">
                <el-icon class="address-card__icon"><Phone /></el-icon>
                <span class="address-card__text address-card__text--phone" @click.stop="editItem(item)">{{ item.phone }}</span>
              </div>
            </div>
            <div class="address-card__row">
              <div class="address-card__info address-card__info--address">
                <el-icon class="address-card__icon"><LocationInformation /></el-icon>
                <el-tooltip 
                  :content="item.address" 
                  placement="top" 
                  :show-after="200"
                  :hide-after="0"
                  class="address-tooltip"
                >
                  <span class="address-card__text" @click.stop="editItem(item)">{{ item.address }}</span>
                </el-tooltip>
              </div>
              <div class="address-card__actions" @click.stop>
                <el-button 
                  type="danger" 
                  size="default" 
                  icon="Delete" 
                  circle
                  @click="del(item)"
                />
              </div>
            </div>
          </div>
        </div>
      </div>

      <div v-if="button" class="dialog-footer">
        <el-button @click="cancel">取 消</el-button>
        <el-button type="primary" @click="confirm">确 定</el-button>
      </div>
    </div>

    <el-dialog 
      title="添加地址" 
      v-model="dialog" 
      :close-on-click-modal="false"
      width="500px"
      class="address-dialog"
    >
      <el-form 
        :model="form" 
        :rules="rules"
        ref="formRef"
        label-width="100px"
        @submit.prevent
      >
        <el-form-item label="姓名" prop="name">
          <el-input v-model="form.name" placeholder="请输入姓名"></el-input>
        </el-form-item>
        <el-form-item label="电话" prop="phone">
          <el-input v-model="form.phone" placeholder="请输入电话号码"></el-input>
        </el-form-item>
        <el-form-item label="详细地址" prop="address">
          <el-input 
            v-model="form.address" 
            type="textarea"
            :rows="3"
            placeholder="请输入详细地址"
          ></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <div class="dialog-footer">
          <el-button @click="closeDialog">取 消</el-button>
          <el-button type="primary" @click="submitForm">确 定</el-button>
        </div>
      </template>
    </el-dialog>

    <el-dialog 
      title="修改信息" 
      v-model="editDialog" 
      :close-on-click-modal="false"
      width="500px"
      class="address-dialog"
    >
      <el-form 
        :model="form" 
        :rules="rules"
        ref="formRef"
        label-width="100px"
        @submit.prevent
      >
        <el-form-item label="姓名" prop="name">
          <el-input v-model="form.name" placeholder="请输入姓名"></el-input>
        </el-form-item>
        <el-form-item label="电话" prop="phone">
          <el-input v-model="form.phone" placeholder="请输入电话号码"></el-input>
        </el-form-item>
        <el-form-item label="详细地址" prop="address">
          <el-input 
            v-model="form.address" 
            type="textarea"
            :rows="3"
            placeholder="请输入详细地址"
          ></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <div class="dialog-footer">
          <el-button @click="closeDialog">取 消</el-button>
          <el-button type="primary" @click="submitForm">确 定</el-button>
        </div>
      </template>
    </el-dialog>
  </div>
</template>

<script>
import { ref } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import api from "@/api"

export default {
  name: 'UserAddress',
  props: {
    button: {
      type: Boolean,
      default: false
    }
  },

  setup() {
    const formRef = ref(null)
    const rules = {
      name: [
        { required: true, message: '请输入姓名', trigger: 'blur' },
        { min: 2, max: 20, message: '长度在 2 到 20 个字符', trigger: 'blur' }
      ],
      phone: [
        { required: true, message: '请输入电话号码', trigger: 'blur' }
      ],
      address: [
        { required: true, message: '请输入详细地址', trigger: 'blur' },
        { min: 5, max: 100, message: '长度在 5 到 100 个字符', trigger: 'blur' }
      ]
    }

    return {
      formRef,
      rules
    }
  },

  data() {
    return {
      dialog: false,
      editDialog: false,
      addressList: [],
      address: {
        id: "",
        name: '',
        phone: '',
        address: '',
      },
      form: {
        id: '',
        name: '',
        phone: '',
        address: '',
      },
      isEdit: false,
    }
  },

  created() {
    this.$api = api
    this.getList()
  },

  methods: {
    async getList() {
      try {
        const res = await this.$api.userAddress.getAddressList()
        this.addressList = res.data
      } catch (error) {
        if (error.message) {
          ElMessage.error(`获取地址列表失败：${error.message}`)
        } else {
          ElMessage.error('获取地址列表失败：网络异常，请稍后重试')
        }
      }
    },

    async submitForm() {
      if (!this.formRef) return
      try {
        await this.formRef.validate()
        if (this.isEdit) {
          await this.$api.userAddress.updateAddress(this.form)
          ElMessage.success('修改成功')
        } else {
          await this.$api.userAddress.saveAddress(this.form)
          ElMessage.success('添加成功')
        }
        this.getList()
        this.closeDialog()
      } catch (error) {
        if (error.message) {
          ElMessage.error(`${this.isEdit ? '修改' : '添加'}失败：${error.message}`)
        } else {
          ElMessage.error(`${this.isEdit ? '修改' : '添加'}失败：网络异常，请稍后重试`)
        }
      }
    },

    async del(item) {
      try {
        await ElMessageBox.confirm('确定要删除该地址吗？', '提示', {
          type: 'warning'
        })
        await this.$api.userAddress.deleteAddress(item.id)
        this.getList()
        ElMessage.success('删除成功')
      } catch (error) {
        if (error !== 'cancel') {
          if (error.message) {
            ElMessage.error(`删除失败：${error.message}`)
          } else {
            ElMessage.error('删除失败：网络异常，请稍后重试')
          }
        }
      }
    },

    toAdd() {
      this.isEdit = false
      this.dialog = true
      this.form = {
        id: '',
        name: '',
        phone: '',
        address: '',
      }
    },

    closeDialog() {
      this.dialog = false
      this.editDialog = false
      this.isEdit = false
      if (this.formRef) {
        this.formRef.resetFields()
      }
    },

    select(item) {
      this.address = item
    },

    confirm() {
      if (!this.address.id) {
        ElMessage.warning('请选择一个地址')
        return
      }
      this.$emit('confirm', this.address)
    },

    cancel() {
      this.$emit('close-drawer')
    },

    editItem(item) {
      this.isEdit = true
      this.editDialog = true
      this.form = {
        id: item.id,
        name: item.name,
        phone: item.phone,
        address: item.address,
      }
    },
  }
}
</script>

<style scoped>
.address-card {
  background-color: var(--el-color-primary-light-9);
  border-radius: 8px;
  margin-bottom: 16px;
  padding: 16px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.05);
  transition: all 0.3s ease;
  border: 2px solid transparent;
  cursor: pointer;
}

.address-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 16px 0 rgba(0, 0, 0, 0.1);
  background-color: var(--el-color-primary-light-8);
}

.address-card--active {
  border-color: var(--el-color-primary);
  background-color: var(--el-color-primary-light-7);
}

.address-card__content {
  display: flex;
  gap: 16px;
  width: 100%;
  overflow: hidden;
}

.address-card__index {
  padding-top: 4px;
}

.address-card__details {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 12px;
  min-width: 0;
  overflow: hidden;
}

.address-card__row {
  display: flex;
  align-items: center;
  gap: 12px;
  width: 100%;
  overflow: hidden;
}

.address-card__info {
  display: flex;
  align-items: center;
  gap: 8px;
  min-width: 120px;
}

.address-card__info--address {
  flex: 1;
  min-width: 0;
  max-width: calc(100% - 60px);
  overflow: hidden;
  text-align: left;
}

.address-card__icon {
  color: var(--el-text-color-secondary);
  font-size: 16px;
}

.address-card__text {
  color: var(--el-text-color-primary);
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  display: block;
  width: 100%;
  text-align: left;
  cursor: pointer;
}

.address-card__text:hover {
  color: var(--el-color-primary);
}

.address-card__actions {
  opacity: 0;
  transition: opacity 0.3s ease;
  margin-right: 20px;
  flex-shrink: 0;
}

.address-card:hover .address-card__actions {
  opacity: 1;
}

.address-tooltip {
  display: block;
  flex: 1;
  min-width: 0;
  overflow: hidden;
}

.dialog-footer {
  display: flex;
  justify-content: center;
  gap: 16px;
  margin-top: 20px;
}

.empty-state {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 40px 0;
}

.address-dialog :deep(.el-dialog__body) {
  padding-top: 20px;
}

.address-card__info--phone {
  flex-shrink: 0;
  min-width: auto;
  margin-left: -20px;
}

.address-card__text--phone {
  white-space: normal;
  overflow: visible;
  text-overflow: clip;
}

.address-dialog :deep(.el-dialog__body) {
  padding-top: 20px;
}

.address-dialog :deep(.el-form-item) {
  margin-bottom: 22px;
}

.address-dialog :deep(.el-form-item__label) {
  font-weight: 500;
  color: var(--el-text-color-regular);
}

.address-dialog :deep(.el-input__inner) {
  height: 40px;
  line-height: 40px;
}

.address-dialog :deep(.el-textarea__inner) {
  min-height: 80px;
  font-family: inherit;
}

.address-dialog :deep(.el-form-item:last-child) {
  margin-bottom: 0;
}

.dialog-footer {
  display: flex;
  justify-content: center;
  gap: 16px;
  margin-top: 20px;
  padding-top: 20px;
  border-top: 1px solid var(--el-border-color-lighter);
}

.dialog-footer .el-button {
  min-width: 100px;
  padding: 12px 20px;
  font-size: 14px;
}

.address-dialog :deep(.el-input__wrapper),
.address-dialog :deep(.el-textarea__inner) {
  box-shadow: none;
  border: 1px solid var(--el-border-color);
  transition: all 0.3s ease;
}

.address-dialog :deep(.el-input__wrapper:hover),
.address-dialog :deep(.el-textarea__inner:hover) {
  border-color: var(--el-border-color-hover);
}

.address-dialog :deep(.el-input__wrapper:focus-within),
.address-dialog :deep(.el-textarea__inner:focus) {
  border-color: var(--el-color-primary);
  box-shadow: 0 0 0 1px var(--el-color-primary-light-8);
}

.address-dialog :deep(.el-dialog__title) {
  font-size: 18px;
  font-weight: 600;
  color: var(--el-text-color-primary);
}

.address-dialog :deep(.el-dialog) {
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}
</style>