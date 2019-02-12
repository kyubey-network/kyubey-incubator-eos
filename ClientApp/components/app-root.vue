<template>
  <div id="app" class="container-fluid">
    <nav-bar></nav-bar>
    <router-view></router-view>
    <footer-bar></footer-bar>
  </div>
</template>

<script>
  import $ from 'jquery'
  import NavBar from './nav-bar'
  import Footer from './footer'
  import { mapActions, mapState, mapMutations, mapGetters } from 'vuex'

  export default {
    components: {
      'nav-bar': NavBar,
      'footer-bar': Footer
    },
    methods: {
      toBeContinued: function () {
        this.$message({
          message: 'to be continued.',
          center: true
        });
      }
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
    }
  }
</script>

<style>

  #app { padding: 0; }
</style>
