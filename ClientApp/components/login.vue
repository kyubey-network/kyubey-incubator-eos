<template>
  <div>
    <el-dialog :visible.sync="$root.eosLoginIsShow" custom-class="login-modal-custom" width="800px">
      <div class="container small" style="color: rgb(180, 180, 180);">
        <div class="row">
          <div class="col-5 d-flex flex-column align-items-center justify-content-between">
            <div class="plugin-login">
              <img src="/img/scatter_logo.png" alt="scatter_logo"> <button type="button" class="btn btn-outline-info mb-3" @click="scatterLoginAsync">{{$t('Scatter login')}}</button>
            </div>
            <p class="text-center">{{$t('Scatter.Text')}}</p>
          </div>
          <div class="col-2 d-flex flex-column align-items-center justify-content-between">
            <span class="my-line"></span>
            <span>{{$t('or')}}</span>
            <span class="my-line"></span>
          </div>
          <div class="col-5 d-flex flex-column align-items-center justify-content-between">
            <p class="text-center mb-3">{{$t('QrCode.Tip1')}}</p>
            <div id="loginQRCode" class="qrcode"
                 v-loading="!$root.signalr.connected"
                 element-loading-text="Loading"
                 element-loading-spinner="el-icon-loading"
                 element-loading-background="#f6f6f6"></div>
            <div class="qrcode-mask d-flex flex-column align-items-center justify-content-center" v-if="!qrcodeIsValid">
              <p>{{$t('QR code is expired')}}</p>
              <a @click="refreshLoginQRCode">{{$t('Refresh')}}</a>
            </div>
            <div class="d-flex align-items-center justify-content-end login-app-icon-container">
              <img class="logo" src="/img/wallet/pocket_logo.png" alt="pocket_logo" v-bind:title="$t('tp.tip')">
              <img class="logo" src="/img/wallet/math_logo.png" alt="math_logo" v-bind:title="$t('math.tip')">
              <img class="logo" src="/img/wallet/meetone_logo.png" alt="meetone_logo" v-bind:title="$t('meetone.tip')">
            </div> <p class="text-center">{{$t('QrCode.Tip2')}}</p>
          </div>
        </div>
      </div>
    </el-dialog>
  </div>
</template>
<script>
  import ScatterJS from 'scatterjs-core';
  import QRCode from 'qrcodejs2';
  import ScatterEOS from 'scatterjs-plugin-eosjs';
  import Eos from 'eosjs'
  import { mapActions, mapState, mapMutations } from 'vuex'


  ScatterJS.plugins(new ScatterEOS());


  export default {
    data() {
      return {
        isShow: false,
        qrcodeIsValid: false,
        qrcodeTimer: null,
        currentHost: location.protocol + "//" + location.host,
        eosNetwork: {
          blockchain: 'eos',
          protocol: 'https',
          host: 'nodes.get-scatter.com',
          port: 443,
          chainId: 'aca376f206b8fc25a6ed44dbdc66547c36c6c33e3a119ffbeaef943642f0e906'
        }
      };
    },
    methods: {
      ...mapActions({
        eosLogin: 'loginState/eosLogin'
      }),
      async scatterLoginAsync() {
        var _this = this;
        ScatterJS.scatter.connect('kyubey-eos').then(connected => {
          if (!connected) {
            if (this.isShow) {
              this.$message({
                message: _this.$t('Please install a Scatter wallet'),
                type: 'error'
              });
            }
            return;
          };

          var eosScatter = ScatterJS.scatter;

          eosScatter.getIdentity(_this.eosRequiredFields).then(() => {

            const account = eosScatter.identity.accounts.find(x => x.blockchain === 'eos');

            const eos = () => eosScatter.eos(_this.eosNetwork, Eos, {});

            _this.eosLogin({ account, loginMode: "scatter", eos, requiredFields: _this.eosRequiredFields, eosScatter });
            _this.isShow = false;

          }).catch(error => {
            console.error(error);
          });
        });
      },
      logoutScatterAsync() {
        return ScatterJS.scatter.forgetIdentity();
      },
      refreshLoginQRCode() {
        this.$nextTick(() => {
          var loginContent = this._getLoginRequestObj(this.$root.eosLoginUuid);
          document.getElementById("loginQRCode").innerHTML = "";
          new QRCode("loginQRCode", {
            text: JSON.stringify(loginContent),
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
      _getLoginRequestObj: function (uuid) {
        var _this = this;
        var loginObj = {
          "protocol": "SimpleWallet",
          "version": "1.0",
          "dappName": "Kyubey",
          "dappIcon": `${_this.currentHost}/img/KYUBEY_logo.png`,
          "action": "login",
          "uuID": uuid,
          "loginUrl": `${_this.currentHost}/api/v1/simplewallet/callback/login`,
          "expired": new window.Date().getTime() + (3 * 60 * 1000),
          "loginMemo": "kyubey login"
        };
        return loginObj;
      },
    },
    watch: {
      '$root.eosLoginIsShow': function (newVal) {
        if (newVal) {
          this.refreshLoginQRCode();
        }
      }
    },
    mounted() {

    },
    computed: {
      eosRequiredFields: function () {
        var _this = this;
        return { accounts: [this.eosNetwork] };
      }
    },
    created: function () {
      this.scatterLoginAsync();
    },
  }
</script>
<style>
  .login-app-icon-container .logo { width: 1.5rem; }
  .login-modal-custom .el-dialog__body { padding: 0; padding: 0 30px 30px 30px; color: #000000; font-size: 14px; }
  .plugin-login { margin-top: 60px; text-align: center; }
    .plugin-login img { margin: 0px auto; margin-bottom: 35px; width: 4rem; display: block; }
  #loginQRCode { margin-bottom: 12px; }
  .qrcode { display: block; width: 200px; height: 200px; background-color: gray; }
  .login-app-icon-container img { margin: 0 8px 8px 8px; }
  .my-line { display: inline-block; height: 42%; width: 1px; background: #969696; opacity: 0.28809999999999997; }
  .qrcode-mask { width: 200px; height: 200px; background-color: rgba(0, 0, 0, 0.80); position: absolute; top: 34px; color: #D9D9D9; font-size: 14px; }
    .qrcode-mask a { color: #FFB500 !important; }
</style>
