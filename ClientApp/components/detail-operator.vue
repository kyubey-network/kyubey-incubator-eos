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
      <el-input-number v-model="buyInputVal" controls-position="right" :precision="4" :step="0.0001" :min="0" style="width:274px;" v-bind:placeholder="$t('enter the buy quantity')"></el-input-number>
      <button type="button" class="btn btn-info buy-btn" @click="exchange">{{$t('Buy')}}</button>
      <span class="current-state">{{$t('Price')}}：{{info.currentPrice}} EOS</span> <span class="current-state ">{{$t('Balance')}}：{{info.eosBalance}} EOS / {{info.tokenBalance}} {{tokenId}}</span><a @click="refresh"><icon class="ml-1 refreash-btn" icon="sync-alt" /></a>
    </div>
    <div class="top-right-row">
      <button type="button" class="btn btn-outline-info big-btn" @click="viewWhitePaper">{{$t('WhitePaper')}}</button>
      <button type="button" class="btn btn-outline-info big-btn" @click="goDex">{{$t('Exchange')}}</button>
    </div>

    <el-dialog :visible.sync="$root.eosExchangeIsShow" custom-class="modal-custom" width="400px">
      <div class="container small" style="color: rgb(180, 180, 180);">
        <div class="row">
          <div class="col-12 d-flex flex-column align-items-center justify-content-between">
            <p class="text-center mb-3">{{$t('Scan code to complete the transaction')}}</p>
            <div id="exchangeQRCode" class="qrcode"
                 v-loading="!$root.signalr.connected"
                 element-loading-text="Loading"
                 element-loading-spinner="el-icon-loading"
                 element-loading-background="#f6f6f6"></div>
            <div class="qrcode-mask d-flex flex-column align-items-center justify-content-center" v-if="!qrcodeIsValid">
              <p>{{$t('QR code is expired')}}</p>
            </div>
            <p class="text-center mt-2 mb-2">{{$t('EOS Account')}}: {{$root.eosUsername}}</p>
            <p class="text-center mb-0">{{$t('Trading Volume')}}： {{buyInputVal}} EOS</p>
          </div>
        </div>
      </div>
    </el-dialog>
  </div>
</template>
<script>
  import { formatDate } from '../common/date.js'
  import { mapActions, mapState, mapGetters, mapMutations } from 'vuex'
  import QRCode from 'qrcodejs2';
  export default {
    data() {
      return {
        currentDate: new window.Date(),
        infoBoxLoading: true,
        buyInputVal: 0,
        tokenId: null,
        qrcodeIsValid: false,
        qrcodeTimer: null,
        info: {

        },
        qrCodeLoading: false
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
        var _this = this;
        //_this.$t('or');
        if (this.info.whitePaper == null) {
          this.$message({
            type: 'error',
            message: _this.$t('No white papers uploaded')
          });
          return;
        }
        window.open(location.protocol + "//" + location.host + "//" + this.info.whitePaper, '_blank');
      },
      goDex() {
        var _this = this;
        if (this.projectState == 0) {
          this.$message({
            type: 'error',
            message: _this.$t('to be continued')
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
            message: _this.$t('Sorry, this crowdfunding is over!')
          });
          return;
        }

        if (this.projectState == 0) {
          this.$message({
            type: 'error',
            message: _this.$t('to be continued')
          });
          return;
        }

        if (!this.isEosLogin) {
          this.$message({
            type: 'error',
            message: _this.$t('Please login')
          });
          return;
        }

        if (_this.buyInputVal == 0) {
          this.$message({
            type: 'error',
            message: _this.$t('Please enter a valid EOS buy quantity!')
          });
          return;
        }

        if (_this.buyInputVal > _this.info.eosBalance) {
          this.$message({
            type: 'info',
            message: _this.$t('The number of purchases cannot be greater than the current balance')
          });
          return;
        }

        this.$confirm(_this.$t('exchange.confirm', { n: _this.buyInputVal }), _this.$t('prompt'), {
          confirmButtonText: _this.$t('confirm'),
          cancelButtonText: _this.$t('cancel'),
          type: 'warning'
        }).then(() => {
          _this.exchangeSubmit();
        }).catch(() => {
          this.$message({
            type: 'info',
            message: _this.$t('exchange failed')
          });
        });
      },
      exchangeSubmit: function () {
        var contractAccount = this.info.contract;
        var amount = this.buyInputVal;
        var buyMemo = this.info.buyMemo;
        if (this.loginMode == 'Simple Wallet') {
          this.qrCodeBuy(contractAccount, amount, buyMemo);
        }
        else
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
            }).then(trx => {
              this.$message({
                type: 'success',
                message: _this.$t('exchange succeed')
              });
            }).catch(error => {
              this.$message({
                type: 'error',
                message: error
              });
            });
        })
      },
      qrCodeBuy: function (contract_account, amount, memo) {
        var _this = this;
        var uuid = _this.$root.eosLoginUuid;
        _this.$root.eosExchangeIsShow = true;
        this.$nextTick(() => {
          var from = this.account.name;
          var to = contract_account;

          var exchangeContent = this._getExchangeRequestObj(from, to, amount, 'eosio.token', "EOS", 4, uuid, memo);
          document.getElementById("exchangeQRCode").innerHTML = "";
          new QRCode("exchangeQRCode", {
            text: JSON.stringify(exchangeContent),
            width: 200,
            height: 200,
            colorDark: "#000000",
            colorLight: "#ffffff",
            correctLevel: QRCode.CorrectLevel.L
          });

          var _this = this;
          this.qrcodeIsValid = true;
          clearTimeout(_this.qrcodeTimer);
          _this.qrcodeTimer = setTimeout(function () {
            _this.qrcodeIsValid = false;
          }, 3 * 60 * 1000);
        })
      },
      _getExchangeRequestObj: function (from, to, amount, contract, symbol, precision, uuid, dappData) {
        var _this = this;
        var resultObj = {
          "protocol": "SimpleWallet",
          "version": "1.0",
          "dappName": "Kyubey",
          "dappIcon": `${_this.$root.currentHost}/img/KYUBEY_logo.png`,
          "action": "transfer",
          "from": from,
          "to": to,
          "amount": amount,
          "contract": contract,
          "symbol": symbol,
          "precision": precision,
          "dappData": dappData,
          "desc": `${symbol} exchange`,
          "expired": new Date().getTime() + (3 * 60 * 1000),
          "callback": `${_this.$root.currentHost}/api/v1/simplewallet/callback/exchange?uuid=${uuid}&sign=`
        };
        return resultObj;
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
        account: state => state.loginState.eosLoginState.account,
        loginMode: state => state.loginState.eosLoginState.loginMode
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
      }
    },
    created() {
      this.tokenId = this.$route.params.id;
      this.getInfo();
    },
    mounted() {
    }
  };
</script>

<style>
  .modal-custom .el-dialog__body { padding: 0; padding: 0 30px 30px 30px; color: #000000; font-size: 14px; }
</style>

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
