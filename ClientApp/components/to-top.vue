<template>
  <div>
    <img v-if="scrollBar>position" @click="toTop(speed)" class="img-top" src="/img/icon/TOP.png" />
  </div>
</template>

<script>
  export default {
    components: {
    },
    props: [
      "speed",
      "position"
    ],
    data() {
      return {
        scrollBar: 0
      }
    },
    methods: {
      scrollToTop() {
        var scrollTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop;
        this.scrollBar = scrollTop;
      },
      toTop(i) {
        document.documentElement.scrollTop -= i;
        if (document.documentElement.scrollTop > 0) {
          var c = setTimeout(() => this.toTop(i), 16);
        } else {
          clearTimeout(c);
        }
      }
    },
    mounted() {
      window.addEventListener('scroll', this.scrollToTop)
    },
    destroyed() {
      window.removeEventListener('scroll', this.scrollToTop);
    }
  }
</script>

<style>
  .img-top { position: fixed; right: 40px; bottom: 60px; border: 1px ridge #D7D7D7; }
    .img-top:hover { cursor: pointer; }
</style>
