<template>
    <div class="container top-slider">
      <h1>{{$t('Contact us')}}</h1>
        <div>
          <h2>
            {{$t('How can you contact us?')}}
          </h2>
          <p v-html="$t('Contact us html')">
          </p>
        </div>
    </div>
</template>

<script>
</script>

<style  scoped>
  .container h1 {
    font-size: 28px;
    font-weight: 600;
    line-height: 40px;
    padding-top: 58px;
    margin-bottom: 54px;
  }
  .container h2 {
    font-size: 16px;
    font-weight: 600;
    line-height: 22px;
    margin-bottom: 9px;
  }
  .container p {
    font-size: 12px;
    font-weight: 400;
    line-height: 17px;
  }
  .container {
    min-height: 792px;
  }
</style>
