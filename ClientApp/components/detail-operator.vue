<template>
  <div v-loading="infoBoxLoading"
       element-loading-text="Loading"
       element-loading-spinner="el-icon-loading"
       element-loading-background="#f6f6f6">
    <!--<div class="top-right-row">
      <h2 class="token-owner">Itzikzikzik</h2>
      <span class="top-right-tip">浙江 杭州</span>
      <span class="top-right-tip">游戏</span>
    </div>-->
    <div v-if="projectState==0">
      <div class="top-right-progress top-right-row">
        <div class="top-right-progress-target">{{info.infoBoxLoading}} EOS</div>
        <el-progress :text-inside="true" :stroke-width="18" :percentage="0" style="margin-top:11px;margin-bottom:12px;" color="#17a2b8"></el-progress>
        <span class="progress-tip1">{{$t('Target')}}</span>
      </div>
      <div class="top-right-progress top-right-row">
        <h2 class="tip">{{info.beginTime | formatDateTime}}</h2>
        <span class="progress-tip1">{{$t('Start Time')}}</span>
      </div>
      <div class="top-right-progress top-right-row">
        <h2 class="tip">0人</h2>
        <span class="progress-tip1">{{$t('Like users')}}</span>
      </div>
    </div>
    <div v-if="projectState==1">
      <div class="top-right-progress top-right-row">
        <div class="top-right-progress-target">{{$t('Raised')}} {{info.currentRaised}} EOS</div>
        <el-progress :text-inside="true" :stroke-width="18" :percentage="progressPercent" style="margin-top:11px;margin-bottom:12px;" color="#17a2b8"></el-progress>
        <span class="progress-tip1">{{$t('Target')}} {{info.target}} EOS</span>
      </div>
      <div class="top-right-progress top-right-row">
        <h2 class="tip">{{info.remainingDay}} {{$t('Day(s)')}}</h2>
        <span class="progress-tip1">{{$t('Remaining Days')}}</span>
      </div>
      <div class="top-right-progress top-right-row">
        <h2 class="tip">{{info.supporterCount}}{{$t('User(s)')}}</h2>
        <span class="progress-tip1">{{$t('Supporters')}}</span>
      </div>
    </div>
    <div v-if="projectState==2">
      <div class="top-right-progress top-right-row">
        <div class="top-right-progress-target">{{$t('Raised')}} {{info.currentRaised}} EOS</div>
        <el-progress :text-inside="true" :stroke-width="18" :percentage="progressPercent" style="margin-top:11px;margin-bottom:12px;" color="#17a2b8"></el-progress>
        <span class="progress-tip1">{{$t('Target')}} {{info.target}} EOS</span>
      </div>
      <div class="top-right-progress top-right-row">
        <h2 class="tip">{{info.remainingDay}} {{$t('Day(s)')}}</h2>
        <span class="progress-tip1">{{$t('Remaining Days')}}</span>
      </div>
      <div class="top-right-progress top-right-row">
        <h2 class="tip">{{info.supporterCount}} {{$t('User(s)')}}</h2>
        <span class="progress-tip1">{{$t('Supporters')}}</span>
      </div>
    </div>
    <div class="top-right-box flex-center">
      <div class="box-item">
        <div class="box-item-val">
          {{info.totalSupply}}
        </div>
        <div class="box-item-name">
          {{$t('Total Supply')}}
        </div>
      </div>
      <div class="box-item">
        <div class="box-item-val">
          {{info.protocol}}
        </div>
        <div class="box-item-name">
          {{$t('Protocol')}}
        </div>
      </div>
      <div class="box-item">
        <div class="box-item-val">
          {{info.contract}}
        </div>
        <div class="box-item-name">
          {{$t('Contract')}}
        </div>
      </div>
      <div class="box-item">
        <icon icon="star" />
      </div>
    </div>
    <div class="top-right-row">
      <el-input-number v-model="buyInputVal" controls-position="right" :precision="4" :step="0.0001" :min="0" style="width:274px;" placeholder="输入购买数量"></el-input-number>
      <button type="button" class="btn btn-info buy-btn" @click="exchange">{{$t('Buy')}}</button>
      <span class="current-state">{{$t('Price')}}：{{info.currentPrice}} EOS</span> <span class="current-state ">{{$t('Balance')}}：{{info.eosBalance}} EOS / {{info.tokenBalance}} {{tokenId}}</span><a @click="refresh"><icon class="ml-1 refreash-btn" icon="sync-alt" /></a>
    </div>
    <div class="top-right-row">
      <button type="button" class="btn btn-outline-info big-btn" @click="viewWhitePaper">{{$t('WhitePaper')}}</button>
      <button type="button" class="btn btn-outline-info big-btn" @click="goDex">{{$t('Exchange')}}</button>
    </div>
  </div>
</template>
<script>
  import { formatDate } from '../common/date.js'
  import { mapActions, mapState, mapGetters, mapMutations } from 'vuex'
  export default {
    data() {
      return {
        currentDate: new window.Date(),
        infoBoxLoading: true,
        buyInputVal: 0,
        tokenId: null,
        info: {

        }
      };
    },
    methods: {
      async getInfo() {
        var _this = this;
        await this.$http.get(`/api/v1/lang/${_this.$root.lang}/Incubator/info/${_this.tokenId}?username=${_this.$root.eosUsername}`).then(res => {
          if (res.status == 200) {
            _this.info = res.data;
            _this.infoBoxLoading = false;
          }
        });
      },
      refresh() {
        this.infoBoxLoading = true;
        this.getInfo();
      },
      viewWhitePaper() {
        if (this.info.whitePaper == null) {
          this.$message({
            type: 'error',
            message: '暂未上传白皮书'
          });
          return;
        }
        window.open(location.protocol + "//" + location.host + "//" + this.info.whitePaper, '_blank');
      },
      goDex() {
        if (this.projectState == 0) {
          this.$message({
            type: 'error',
            message: '暂未开放,敬请期待!'
          });
          return;
        }
        window.open('https://www.kyubey.net/exchange/' + this.tokenId, '_blank');
      },
      exchange() {
        var _this = this;
        if (this.projectState == 2) {
          this.$message({
            type: 'error',
            message: '抱歉,该众筹已经结束!'
          });
          return;
        }

        if (this.projectState == 0) {
          this.$message({
            type: 'error',
            message: '暂未开放,敬请期待!'
          });
          return;
        }

        if (!this.isEosLogin) {
          this.$message({
            type: 'error',
            message: '请登录'
          });
          return;
        }

        if (_this.buyInputVal == 0) {
          this.$message({
            type: 'error',
            message: '请输入有效的EOS买入数量!'
          });
          return;
        }

        if (_this.buyInputVal > _this.info.balance) {
          this.$message({
            type: 'info',
            message: '买入的数量不能大于当前的余额'
          });
          return;
        }

        this.$confirm(`此操作将会花费${_this.buyInputVal}EOS, 是否继续?`, '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          _this.exchangeSubmit();
        }).catch(() => {
          this.$message({
            type: 'info',
            message: '交易失败'
          });
        });
      },
      exchangeSubmit: function () {
        var contractAccount = this.info.contract;
        var amount = this.buyInputVal;
        var buyMemo = this.info.buyMemo;

        this.scatterBuy(contractAccount, amount, buyMemo);
      },
      scatterBuy: function (contract_account, amount, memo) {
        var _this = this;
        _this.eos().contract('eosio.token', { requiredFields: _this.requiredFields }).then(contract => {
          return contract.transfer(
            _this.account.name,
            contract_account,
            parseFloat(amount).toFixed(4) + ' EOS',
            memo,
            {
              authorization: [`${_this.account.name}@${_this.account.authority}`]
            });
        })
      }
    },
    filters: {
      formatDateTime: function (timeStr) {
        if (typeof timeStr == 'undefined') {
          return null;
        }
        let date = new window.Date(timeStr);
        return formatDate(date, 'yyyy-MM-dd hh:mm')
      }
    },
    computed: {
      ...mapGetters({
        isEosLogin: 'loginState/isEosLogin'
      }),
      ...mapState({
        eos: state => state.loginState.eosLoginState.eos,
        requiredFields: state => state.loginState.eosLoginState.requiredFields,
        account: state => state.loginState.eosLoginState.account
      }),
      progressPercent: function () {
        if (typeof this.info.target == 'undefined' || this.info.target == 0) {
          return 0;
        }
        return parseFloat((this.info.currentRaised * 100 / this.info.target).toFixed(2));
      },
      projectState: function () {
        if (typeof this.info.deadLine != 'undefined' && new window.Date(this.info.deadLine) < this.currentDate) {
          return 2;
        }
        else if (typeof this.info.beginTime != 'undefined'
          && typeof this.info.deadLine != 'undefined'
          && new window.Date(this.info.beginTime) <= this.currentDate
          && new window.Date(this.info.deadLine) >= this.currentDate) {
          return 1;
        }
        return 0;
      }
    },
    watch: {
      '$root.isEosLogin': function () {
        var _this = this;
        _this.getInfo();
      },
    },
    created() {
      this.tokenId = this.$route.params.id;
      this.getInfo();
    }
  };
</script>

<style scoped>
  .refreash-btn:hover { -webkit-animation: rotate 1.4s linear infinite; }

  @-webkit-keyframes rotate {
    0% { -webkit-transform: rotate(0deg); }
    25% { -webkit-transform: rotate(90deg); }
    50% { -webkit-transform: rotate(180deg); }
    75% { -webkit-transform: rotate(270deg); }
    100% { -webkit-transform: rotate(360deg); }
  }

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
