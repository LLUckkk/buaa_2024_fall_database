<template>
  <div class="dashboard-container">
    <el-row :gutter="20">
      <!-- 左侧用户信息卡片 -->
      <el-col :span="6">
        <el-card class="user-card">
          <div class="user-info">
            <el-avatar :size="80" icon="el-icon-user-solid" color="#409EFF"></el-avatar>
            <div class="info-detail">
              <p>姓名: {{ userInfo.name }}</p>
              <p>角色: {{ userInfo.role }}</p>
              <p>角色代码: <el-tag size="small">{{ userInfo.roleCode }}</el-tag></p>
            </div>
          </div>
          <el-divider></el-divider>
          <div class="welcome-info">
            <h3>欢迎登录 航游集市后台管理系统！</h3>
            <p>上次登录时间: {{ lastLoginTime }}</p>
          </div>
          
          <!-- 商品分类表格 -->
          <div class="category-table">
            <el-table :data="categoryData" style="width: 100%">
              <el-table-column prop="name" label="商品分类"></el-table-column>
              <el-table-column prop="today" label="今日"></el-table-column>
              <el-table-column prop="month" label="本月"></el-table-column>
              <el-table-column prop="total" label="总计"></el-table-column>
            </el-table>
          </div>
        </el-card>
      </el-col>

      <!-- 右侧统计信息 -->
      <el-col :span="18">
        <!-- 数据卡片 -->
        <div class="stat-cards">
          <el-row :gutter="20">
            <el-col :span="8" v-for="item in statCards" :key="item.title">
              <el-card class="stat-card">
                <div class="card-content">
                  <div class="card-value">{{ item.value }}</div>
                  <div class="card-title">{{ item.title }}</div>
                </div>
              </el-card>
            </el-col>
          </el-row>
        </div>

        <!-- 折线图 -->
        <el-card class="chart-card">
          <div ref="lineChart" style="height: 300px"></div>
        </el-card>

        <!-- 柱状图和饼图 -->
        <el-row :gutter="20" class="bottom-charts">
          <el-col :span="12">
            <el-card>
              <div ref="barChart" style="height: 300px"></div>
            </el-card>
          </el-col>
          <el-col :span="12">
            <el-card>
              <div ref="pieChart" style="height: 250px"></div>
            </el-card>
          </el-col>
        </el-row>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import * as echarts from 'echarts'

export default {
  name: 'DashBoard',
  data() {
    return {
      userInfo: {
        name: '李四',
        role: '系统管理员',
        roleCode: 'system'
      },
      lastLoginTime: '2024-06-01 20:40:43',
      statCards: [
        { title: '本日订单数', value: '2' },
        { title: '本日销售额', value: '878.00' },
        { title: '本日新增商品数', value: '1' },
        { title: '本月订单数', value: '2' },
        { title: '本月销售额', value: '878.00' },
        { title: '本月新增商品数', value: '1' }
      ],
      categoryData: [
        { name: '母婴', today: '0', month: '0', total: '1' },
        { name: '食品生鲜', today: '0', month: '0', total: '1' },
        { name: '卡券', today: '0', month: '0', total: '0' },
        { name: '宠物花卉', today: '0', month: '0', total: '2' }
      ]
    }
  },
  mounted() {
    this.initCharts()
  },
  methods: {
    initCharts() {
      // 初始化折线图
      const lineChart = echarts.init(this.$refs.lineChart)
      lineChart.setOption({
        title: {
          text: '销售趋势',
          left: 'center'
        },
        xAxis: {
          type: 'category',
          data: ['2024-12-12', '2024-12-13', '2024-12-14', '2024-12-15', '2024-12-16', '2024-12-17', '2024-12-18']
        },
        yAxis: {
          type: 'value'
        },
        legend: {
          data: ['电子产品', '书籍资料', '服装首饰', '食物饮料', '生活用品', '学习用品', '其他'],
          bottom: '0',
          left: 'center'
        },
        series: [
          {
            name: '电子产品',
            type: 'line',
            data: [4.4, 3.4 ,2.2, 4.5 ,1.3 ,3.9 ,4.5]
          },
          {
            name: '书籍资料',
            type: 'line',
            data: [2.5, 3.0,3.9, 4.5, 1.0, 1.5, 2]
          },
          {
            name: '服装首饰',
            type: 'line',
            data: [5.4, 1.8, 2.5, 3.8, 2.9, 2.0, 1.1]
          },
          {
            name: '食物饮料',
            type: 'line',
              data: [0.6, 1.9, 2.5, 2.8, 1.69, 2.4, 2.5]
          },
          {
            name: '生活用品',
            type: 'line',
            data: [0.4, 1.2, 1.5, 1.8, 1.9, 2.0, 2.1]
          },
          {
            name: '学习用品',
            type: 'line',
            data: [1.4, 1.7, 5.0, 4.5, 1.2, 2.8, 3.7]
          },
          {
            name: '其他',
            type: 'line',
            data: [1.4, 2.2, 3.5, 0.8, 1.9, 2.8, 3.1]
          }
        ]
      })

      // 初始化柱状图
      const barChart = echarts.init(this.$refs.barChart)
      barChart.setOption({
        title: {
          text: '用户统计',
          left: 'center',
          top: '5%'
        },
        xAxis: {
          type: 'category',
          data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
        },
        yAxis: {
          type: 'value'
        },
        legend: {
          data: ['新增用户', '活跃用户'],
          orient: 'vertical',
          top: '10%',
          left: 'left'
        },
        grid: {
          top: '120px',
          bottom: '40px',
          left: '40px',
          right: '40px',
          containLabel: true
        },
        series: [
          {
            name: '新增用户',
            type: 'bar',
            data: [1, 0, 0, 4, 0, 2, 0]
          },
          {
            name: '活跃用户',
            type: 'bar',
            data: [0, 0, 0, 2, 3, 2, 0]
          }
        ]
      })

      // 初始化饼图
      const pieChart = echarts.init(this.$refs.pieChart)
      pieChart.setOption({
        title: {
          text: '销售额占比',
          left: 'center'
        },
        legend: {
          orient: 'vertical',
          left: 'left'
        },
        series: [
          {
            type: 'pie',
            radius: '50%',
            data: [
              { value: 60, name: '电商' },
              { value: 27, name: '运动户外' },
              { value: 13, name: '其他' }
            ]
          }
        ]
      })
    }
  }
}
</script>

<style lang="scss" scoped>
.dashboard-container {
  padding: 20px;

  .user-card {
    .user-info {
      display: flex;
      align-items: center;
      margin-bottom: 20px;

      .info-detail {
        margin-left: 20px;
        
        p {
          margin: 5px 0;
        }
      }
    }

    .welcome-info {
      text-align: center;
      margin: 20px 0;
    }
  }

  .stat-cards {
    margin-bottom: 20px;

    .stat-card {
      .card-content {
        text-align: center;

        .card-value {
          font-size: 24px;
          font-weight: bold;
          color: #409EFF;
        }

        .card-title {
          margin-top: 10px;
          color: #666;
        }
      }
    }
  }

  .chart-card {
    margin-bottom: 20px;
  }

  .bottom-charts {
    .el-card {
      margin-bottom: 20px;
    }
  }
}
</style>