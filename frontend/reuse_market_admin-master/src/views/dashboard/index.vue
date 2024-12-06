<template>
  <div class="dashboard-container">
    <el-row>
      <!--左侧-->
      <el-col :span="8">
        <div class="grid-content bg-purple">
          <!--用户信息-->
          <div class="user-info">
            <el-card class="box-card">
              <div class="userInfo">
                <img src="@/assets/images/user1.png" alt="">
                <div class="userRight">
                  <p><span style="font-size: 13px;color: #909399">姓名：</span>{{ userInfo.name }}</p>
                  <p><span style="font-size: 13px;color: #909399">角色：</span>{{ userInfo.roleName }}</p>
                  <p><span style="font-size: 13px;color: #909399;">角色代码：</span><el-tag type="mini">{{ userInfo.roleCode }}</el-tag></p>
                </div>
              </div>
              <el-divider></el-divider>
              <div class="loginInfo">
                <div style="font-weight: bolder;font-size: 18px;color: #409EFF;">欢迎登录 闲宝后台管理系统 ！</div>
                <p style="margin-top: 20px">上次登录时间：<span>{{ $utils.convert.parseTime(userInfo.updateTime) }}</span></p>
              </div>
            </el-card>
          </div>
          <!--订单统计-->
          <div class="orderCount">
            <el-card class="box-card" style="height: 410px;margin-top: 20px;overflow-y:scroll">
              <el-table :data="tableData" style="width: 100%;height: 100%;">
                <el-table-column  v-for="(val,key) in tableLabel" :prop="key" :key="key" :label="val"></el-table-column>
              </el-table>
            </el-card>
          </div>
        </div>
      </el-col>
      <!--右侧-->
      <el-col :span="16">
        <div class="grid-content bg-purple-light">
          <div class="detail-top">
            <el-card class="box-card" v-for="(item,index) in countData" :key="index" :body-style="{display:'flex',padding:0}" style="margin-right: 30px;height:80px;margin-bottom: 20px;width: 30%">
              <div class="detail-left" :style="`background-color:${item.color}`">
                <i class="icon" :class="`el-icon-${item.icon}`"></i>
              </div>
              <div class="detail-right">
                <div class="title">{{ item.name }} <i class="icon" :class="`el-icon-${item.titleIcon}`" :style="`color:${item.color}`"></i></div>
                <div class="money">{{ item.value }}</div>
              </div>
            </el-card>
          </div>
        </div>
        <el-card style="height: 275px;margin-left: 20px;">
          <div ref="echart1" style="height:295px;"></div>
        </el-card>
        <div class="graph">
          <el-card>
            <div ref="echart2" style="height: 250px"></div>
          </el-card>
          <el-card>
            <div ref="echart3" style="height: 250px"></div>
          </el-card>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import {mapState} from "vuex";
import * as echarts from 'echarts';
import Pro from "@/utils/pro";

export default {
  name: 'Dashboard',
  data() {
    return {
      tableData: [],
      tableLabel: {
        type: '商品分类',
        todayBuy: '今日销量',
        monthBuy: '月销量',
        totalBuy: '总销量'
      },
      countData: [
        {
          name: "本日订单数",
          value: 0,
          icon: "s-order",
          color: "#409EFF",
          titleIcon:"sunny",
        },
        {
          name: "本日销售额",
          value: 0,
          icon: "s-data",
          color: "#67C23A",
          titleIcon: "sunny",
        },
        {
          name: "本日新增商品数",
          value: 0,
          icon: "goods",
          color: "#F56C6C",
          titleIcon: "sunny",
        },
        {
          name: "本月订单数",
          value: 0,
          icon: "s-order",
          color: "#409EFF",
          titleIcon: "finished",
        },
        {
          name: "本月销售额",
          value: 0,
          icon: "s-data",
          color: "#67C23A",
          titleIcon: "finished",
        },
        {
          name: "本月新增商品数",
          value: 0,
          icon: "goods",
          color: "#F56C6C",
          titleIcon: "finished",
        },
      ],
      userData: [],
      //饼图
      videoData: [],
      // 折线图
      orderData: {date: [], data: []},
    }
  },
  computed: {
    ...mapState({
      userInfo: state => state.user.userInfo
    })
  },
  mounted() {
    this.getData().then(res => {
      this.initEchart()
    })
  },
  methods: {
    getData() {
      return new Promise(resolve => {
        Promise.all([
          this.$api.stat.getCountData(),
          this.$api.stat.getOrderData(),
          this.$api.stat.getUserData(),
          this.$api.stat.getVideoData(),
          this.$api.stat.getTableData()
        ]).then(([countDataRes, orderDataRes, userDataRes, videoDataRes, tableDataRes]) => {
          this.countData[0].value = countDataRes.result.orderTodayCount;
          this.countData[1].value = this.$utils.convert.to_price(countDataRes.result.moneyToday)
          this.countData[2].value = countDataRes.result.productTodayCount;
          this.countData[3].value = countDataRes.result.orderMonthCount;
          this.countData[4].value = this.$utils.convert.to_price(countDataRes.result.moneyMonth)
          this.countData[5].value = countDataRes.result.productMonthCount;
          this.orderData.date = Object.keys(orderDataRes.result).reverse();
          let values = Object.values(orderDataRes.result).reverse();
          this.orderData.data = values.map(innerArray => {
            return innerArray.reduce((acc, currentObj) => {
              let key = Object.keys(currentObj)[0];
              acc[key] = currentObj[key];
              return acc;
            }, {});
          });
          this.userData = userDataRes.result;
          this.videoData = videoDataRes.result;
          this.tableData = tableDataRes.result;
          resolve();
        });
      })
    },
    initEchart() {
      const echart01 = echarts.init(this.$refs.echart1);
      const echart1Option = {
        xAxis: {data: this.orderData.date},
        legend: {data: Object.keys(this.orderData.data[0])},
        yAxis: {},
        title: {
          text: '订\n\n单\n\n销\n\n售\n\n数\n\n量',
          width:1,
          textStyle: { // 主标题样式
            fontSize: 13,
            color: '#606266',
            fontStyle: 'normal', // 字体风格,'normal','italic','oblique'
            fontWeight: 'bold', //字体粗细 'normal','bold','bolder','lighter',数值：100-700
            overflow: 'break',
          }
        },
      }
      echart1Option.series = []
      Object.keys(this.orderData.data[0]).forEach(key => {
        echart1Option.series.push({
          name: key,
          type: 'line',
          data: this.orderData.data.map(item => item[key])
        })
      })
      echart01.setOption(echart1Option)
      //echart2
      const echart02 = echarts.init(this.$refs.echart2);
      const echart2Option = {
        legend: {
          // 图例文字颜色
          textStyle: {
            color: "#333",
          },
        },
        grid: {
          left: "20%",
        },
        // 提示框
        tooltip: {
          trigger: "axis",
        },
        xAxis: {
          type: "category", // 类目轴
          data: this.userData.map(item => item.date),
          axisLine: {
            lineStyle: {
              color: "#17b3a3",
            },
          },
          axisLabel: {
            interval: 0,
            color: "#333",
          },
        },
        yAxis: [
          {
            type: "value",
            axisLine: {
              lineStyle: {
                color: "#17b3a3",
              },
            },
          },
        ],
        color: ["#2ec7c9", "#b6a2de"],
        series: [
          {
            name: '新增用户',
            type: 'bar',
            data: this.userData.map(item => item.new)
          },
          {
            name: '活跃用户',
            type: 'bar',
            data: this.userData.map(item => item.active)
          }
        ],
      }
      echart02.setOption(echart2Option)
      //echart3
      const echart03 = echarts.init(this.$refs.echart3);
      const echart3Option = {
        tooltip: {
          trigger: "item",
        },
        noData: {
          text: '暂无数据',
          textStyle: {
            fontSize: 24,
            color: '#333'
          }
        },
        title: {
          left: 'center',
          text: '销售额占比',
          textStyle: { // 主标题样式
            fontSize: 13,
            color: '#606266',
            fontStyle: 'normal', // 字体风格,'normal','italic','oblique'
            fontWeight: 'bold', //字体粗细 'normal','bold','bolder','lighter',数值：100-700
          }
        },
        legend: {
          orient: 'vertical',
          left: 'left'
        },
        color: [
          "#0f78f4",
          "#dd536b",
          "#9462e5",
          "#a6a6a6",
          "#e1bb22",
          "#39c362",
          "#3ed1cf",
        ],
        series: [
          {
            data: this.videoData,
            type: 'pie',
            radius: ['30%', '70%'],
            label: {
              show: true, // 显示标签
              position: 'inside', // 在饼图内部显示标签
              formatter: '{b}：{d}%'
            },
            itemStyle: {
              // 设置空心的边框宽度和颜色
              borderWidth: 2,
              borderColor: '#fff'
            },
            emphasis: {
              itemStyle: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
              }
            }
          }
        ],
      }
      echart03.setOption(echart3Option)
    },

  }
}
</script>

<style lang="scss" scoped>
.dashboard {
  &-container {
    margin: 30px;
  }

  &-text {
    font-size: 30px;
    line-height: 46px;
  }
}

.user-info {
  .userInfo {
    height: 140px;
    display: flex;
    align-items: center;
    padding: 10px;


    img {
      height: 150px;
      width: 150px;

    }

    .userRight {
      margin-left: 40px;
    }
  }

  .loginInfo {
    padding: 10px;
    line-height: 20px;

    p {
      margin-top: 5px;

      span {
        margin-left: 50px;
      }
    }
  }
}

.detail-top {
  display: flex;
  flex-wrap: wrap;
  margin-left: 20px;

  .detail-left {
    width: 80px;
    height: 80px;
    text-align: center;
    line-height: 80px;
    font-size: 30px;
  }

  .detail-right {
    margin-left: 20px;
    display: flex;
    height: 80px;
    flex-direction: column;
    .title {
      margin-top: 10px;
      height: 30px;

      font-size: 15px;
    }
    .money {
      height: 30px;

      font-size: 25px;

    }
  }
}

.graph {
  margin-left: 20px;
  margin-top: 20px;
  display: flex;
  justify-content: space-between;

  .el-card {
    width: 48%;
    height: 260px;
  }
}
</style>
