<template>
  <div>
    <el-dialog :visible.sync="isShow" custom-class="login-modal-custom" width="800px">
      <div class="container small" style="color: rgb(180, 180, 180);">
        <div class="row">
          <div class="col-5 d-flex flex-column align-items-center justify-content-between">
            <div class="plugin-login">
              <img src="/img/scatter_logo.png" alt="scatter_logo"> <button type="button" class="btn btn-outline-info mb-3" @click="scatterLoginAsync">Scatter登录</button>
            </div>
            <p class="text-center">Scatter是一款EOS钱包， 请您确定已经安装Scatter. 参考：https://get-scatter.com</p>
          </div>
          <div class="col-2 d-flex flex-column align-items-center justify-content-between">
            <span class="my-line"></span>
            <span>或</span>
            <span class="my-line"></span>
          </div>
          <div class="col-5 d-flex flex-column align-items-center justify-content-between">
            <p class="text-center mb-3">手机钱包扫码登录</p>
            <div id="loginQRCode" class="qrcode"></div>
            <!---->
            <div class="d-flex align-items-center justify-content-end login-app-icon-container">
              <img class="logo" src="/img/wallet/pocket_logo.png" alt="pocket_logo" title="版本号至少为 Android 0.4.6、iOS 0.4.4">
              <img class="logo" src="/img/wallet/math_logo.png" alt="math_logo" title="版本号至少为 1.6.5">
              <img class="logo" src="/img/wallet/meetone_logo.png" alt="meetone_logo" title="版本号至少为 1.1.8">
            </div> <p class="text-center">请确保手机钱包为支持扫码授权的版本</p>
          </div>
        </div>
      </div>
    </el-dialog>
  </div>
</template>
<script>
  import ScatterJS from 'scatterjs-core';
  import ScatterEOS from 'scatterjs-plugin-eosjs';
  import Eos from 'eosjs'
  import { debug } from 'util';
  import { mapActions, mapState } from 'vuex'

  ScatterJS.plugins(new ScatterEOS());


  export default {
    data() {
      return {
        isShow: false,
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
          if (!connected) return false;
          const scatter = ScatterJS.scatter;
          const requiredFields = { accounts: [_this.eosNetwork] };
          scatter.getIdentity(requiredFields).then(() => {
            const account = scatter.identity.accounts.find(x => x.blockchain === 'eos');
            const eos = scatter.eos(_this.eosNetwork, Eos, {});
            _this.eosLogin({ account, loginMode: "scatter", eos, requiredFields });
            _this.isShow = false;
          }).catch(error => {
            console.error(error);
          });
        });
      },
      logoutScatterAsync() {
        return ScatterJS.scatter.forgetIdentity();
      },
    },
    mounted() {

    },
    created: function () {
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
</style>
