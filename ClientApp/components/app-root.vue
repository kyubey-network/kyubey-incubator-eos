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

  export default {
    components: {
      'nav-bar': NavBar,
      'footer-bar': Footer
    },
    methods: {

    },
    data() {

    },
    created() {
      
    }
  }
</script>

<style>
</style>
