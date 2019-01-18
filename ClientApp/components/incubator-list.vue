<template>
    <div class="container">
        <div v-for="x in list.data.data">
          <h1>{{ x.id }}</h1>
          <p>{{x.introduction}}</p>
        </div>
    </div>
</template>

<script>
  export default {
    computed: {
    },

    data() {
      return {
        list: null,
        skip: 0,
        take: 12,
        lang: 'zh'
      }
    },

    methods: {
    },

    async created() {
      this.list = await this.$http.get(`/api/v1/lang/${this.lang}/Incubator/list?skip=${this.skip}&take=${this.take}`)
      
    }
  }
</script>

<style>

</style>
