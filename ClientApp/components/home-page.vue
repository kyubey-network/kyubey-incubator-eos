<template>
  <div>
    <div id="homeSlider" class="carousel slide" data-ride="carousel">
      <ol class="carousel-indicators">
        <li data-target="#homeSlider" data-slide-to="0" class="active"></li>
        <li data-target="#homeSlider" data-slide-to="1"></li>
        <li data-target="#homeSlider" data-slide-to="2"></li>
      </ol>
      <div class="carousel-inner">
        <div class="carousel-item active">
          <img class="d-block w-100" src="/img/banners/cryptomeetup/bg.png" alt="First slide">
        </div>
        <div class="carousel-item">
          <img class="d-block w-100" src="/img/banners/cryptomeetup/bg.png" alt="Second slide">
        </div>
        <div class="carousel-item">
          <img class="d-block w-100" src="/img/banners/cryptomeetup/bg.png" alt="Third slide">
        </div>
      </div>
      <a class="carousel-control-prev" href="#homeSlider" role="button" data-slide="prev">
        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
        <span class="sr-only">Previous</span>
      </a>
      <a class="carousel-control-next" href="#homeSlider" role="button" data-slide="next">
        <span class="carousel-control-next-icon" aria-hidden="true"></span>
        <span class="sr-only">Next</span>
      </a>
    </div>
    <incubatorList></incubatorList>
  </div>
</template>

<script>
  import incubatorList from './incubator-list.vue'
  export default {
    components: {
      incubatorList
    },
    data() {
      return {}
    }
  }
</script>

<style>
  .carousel-inner { width: 100%; max-height: 350px !important; }
  .carousel img { max-height: 350px; margin: 0 auto; }
  #app { padding: 0; }
  #homeSlider { margin-top: 60px; }
</style>
