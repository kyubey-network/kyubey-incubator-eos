<template>
  <div>
    <!--<div class="top-right-row">
      <h2 class="token-owner">Itzikzikzik</h2>
      <span class="top-right-tip">浙江 杭州</span>
      <span class="top-right-tip">游戏</span>
    </div>-->
    <div class="top-right-progress top-right-row">
      <div class="top-right-progress-target">已筹 {{info.currentRaised}} EOS</div>
      <el-progress :text-inside="true" :stroke-width="18" :percentage="progressPercent" style="margin-top:11px;margin-bottom:12px;" color="#17a2b8"></el-progress>
      <span class="progress-tip1">目标 {{info.target}} EOS</span>
    </div>
    <div class="top-right-progress top-right-row">
      <h2 class="tip">{{info.remainingDay}}天</h2>
      <span class="progress-tip1">剩余时间</span>
    </div>
    <div class="top-right-progress top-right-row">
      <h2 class="tip">{{info.supporterCount}}人</h2>
      <span class="progress-tip1">支持人数</span>
    </div>
    <div class="top-right-box flex-center">
      <div class="box-item">
        <div class="box-item-val">
          {{info.totalSupply}}
        </div>
        <div class="box-item-name">
          发行总量
        </div>
      </div>
      <div class="box-item">
        <div class="box-item-val">
          {{info.protocol}}
        </div>
        <div class="box-item-name">
          使用协议
        </div>
      </div>
      <div class="box-item">
        <div class="box-item-val">
          {{info.contract}}
        </div>
        <div class="box-item-name">
          合约名称
        </div>
      </div>
      <div class="box-item">
        <icon icon="star" />
      </div>
    </div>
    <div class="top-right-row">
      <el-input-number v-model="buyInputVal" controls-position="right" :precision="4" :step="0.0001" :min="0" style="width:274px;" placeholder="输入购买数量"></el-input-number>
      <button type="button" class="btn btn-info buy-btn">购买</button>
      <span class="current-state">当前单价：{{info.currentPrice}} EOS</span> <span class="current-state ">当前持有：{{info.balance}} EOS</span>
    </div>
    <div class="top-right-row">
      <button type="button" class="btn btn-outline-info big-btn">白皮书</button>
      <button type="button" class="btn btn-outline-info big-btn">交易</button>
    </div>
  </div>
</template>
<script>
  export default {
    data() {
      return {
        buyInputVal: 0,
        tokenId: null,
        info: {

        }
      };
    },
    methods: {
      async getInfo() {
        var _this = this;
        await this.$http.get(`/api/v1/lang/${_this.$root.lang}/Incubator/info/${_this.tokenId}`).then(res => {
          if (res.status == 200) {
            _this.info = res.data;
          }
        });
      }
    },
    computed: {
      progressPercent: function () {
        if (typeof this.info.target == 'undefined' || this.info.target == 0) {
          return 0;
        }
        return (this.info.currentRaised * 100 / this.info.target).toFixed(2);
      }
    },
    created() {
      this.tokenId = this.$route.params.id;
      this.getInfo();
    }
  };
</script>

<style scoped>
  .top-right-tip { font-size: 12px; }

  .top-right-progress { margin-top: 38px; }
  .token-owner { font-size: 22px; margin: 0; }
  .top-right-progress-target { font-size: 24px; font-weight: 500; color: #17a2b8; line-height: 33px; }

  .top-right-row { margin-bottom: 31px; }
    .top-right-row h2 { font-size: 24px; font-weight: 400; }
  .progress-tip1 { font-size: 14px; color: rgba(0,0,0,1); font-weight: 400; }

  .top-right-box { width: 438px; height: 85px; border-radius: 2px; border: 1px solid rgba(221,221,221,1); margin-bottom: 25px; }
    .top-right-box .box-item { min-width: 108px; display: inline-block; text-align: center; }
      .top-right-box .box-item:first-child { margin-left: 15px; }
      .top-right-box .box-item .box-item-val { font-size: 18px; font-weight: 500; }
      .top-right-box .box-item .box-item-name { font-size: 16px; font-weight: 400; }
      .top-right-box .box-item:last-child { border-left: 1px solid rgba(221,221,221,1); min-width: 80px; margin-left: 15px; }

  .flex-center { display: flex; align-items: center; }

  .buy-btn { width: 124px; height: 50px; margin-left: 35px; }
  .current-state { font-size: 14px; font-weight: 400; color: rgba(0,0,0,1); line-height: 20px; }
    .current-state:last-child { margin-left: 27px; }
  .big-btn { width: 201px; height: 50px; }
  .top-right-row .big-btn:last-child { margin-left: 30px; }
</style>
