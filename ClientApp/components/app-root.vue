<template>
  <div id="app" class="container-fluid">
    <nav-bar></nav-bar>
    <router-view></router-view>
    <footer-bar></footer-bar>
  </div>
</template>

<script>
  import NavBar from './nav-bar'
  import Footer from './footer'
  import { mapActions, mapState, mapMutations, mapGetters } from 'vuex'
  const signalR = require("@aspnet/signalr");

  export default {
    components: {
      'nav-bar': NavBar,
      'footer-bar': Footer
    },
    data() {
      return {
        currentHost: location.protocol + "//" + location.host,
        signalr: {
          simplewallet: {
            connection: null,
            listeners: []
          },
          connected: false
        },
        eosLoginUuid: null,
        eosLoginIsShow: false,
        eosExchangeIsShow: false
      };
    },
    methods: {
      ...mapActions({
        eosLogin: 'loginState/eosLogin'
      }),
      toBeContinued: function () {
        this.$message({
          message: 'to be continued.',
          center: true
        });
      },
      generateUUID: function () {
        var s = [];
        var hexDigits = "0123456789abcdef";
        for (var i = 0; i < 36; i++) {
          s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
        }
        s[14] = "4";
        s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);
        s[8] = s[13] = s[18] = s[23] = "-";

        var uuid = s.join("");
        return uuid;
      },
      initSignalR: function () {
        var self = this;

        self.signalr.simplewallet.connection = new signalR.HubConnectionBuilder()
          .configureLogging(signalR.LogLevel.Trace)
          .withUrl('/signalr/simplewallet', {})
          .withHubProtocol(new signalR.JsonHubProtocol())
          .build();

        // TODO: Receiving some signals for updating query view.
        self.signalr.simplewallet.connection.on('simpleWalletLoginSucceeded', (account) => {
          var accountObject = {
            name: account
          };

          self.eosLogin({ account: accountObject, loginMode: 'Simple Wallet', eos: null, requiredFields: null, eosScatter: null });
          self.eosLoginIsShow = false;
        });

        self.signalr.simplewallet.connection.on('simpleWalletExchangeSucceeded', () => {
          self.eosExchangeIsShow = false;

          self.$message({
            type: 'success',
            message: self.$t('exchange succeed')
          });
        });

        self.signalr.simplewallet.connection.start().then(function () {
          self.signalr.connected = true;
          return self.signalr.simplewallet.connection.invoke('bindUUID', self.eosLoginUuid);          
        });

        self.signalr.simplewallet.connection.onclose(async () => {
          self.signalr.connected = false;
          await self.restartSignalR();
        });
      },
      restartSignalR: async function () {
        var self = this;
        try {
          await self.signalr.simplewallet.connection.start();
          self.signalr.connected = true;
          self.signalr.simplewallet.connection.invoke('bindUUID', self.eosLoginUuid);
          console.log('reconnected');
        } catch (err) {
          console.warn(err);
          if (err.statusCode > 500 || err.statusCode == 0) {
            setTimeout(() => self.restartSignalR(), 2000);
          }
        }
      },
    },
    computed: {
      lang: function () {
        return this.$i18n.locale;
      },
      ...mapGetters({
        isEosLogin: 'loginState/isEosLogin',
        eosUsername: 'loginState/eosUsername',
      }),
    },
    created() {
      this.eosLoginUuid = this.generateUUID();
      this.initSignalR();      

      //var accountObject = {
      //  name: ""
      //};
      //this.eosLogin({ account: accountObject, loginMode: 'Simple Wallet', eos: null, requiredFields: null, eosScatter: null });
    }
  }
</script>

<style>

  #app { padding: 0; }
</style>
