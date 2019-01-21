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
import { debug } from 'util';

  export default {
    components: {
      'nav-bar': NavBar,
      'footer-bar': Footer
    },
    methods: {
    },
    computed: {
      lang: function () {
        return this.$i18n.locale;
      }
    },
    created() {
    }
  }
</script>

<style>
</style>
