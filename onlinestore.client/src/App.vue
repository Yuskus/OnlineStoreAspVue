<template>
  <component :is="layout">
    <router-view />
  </component>
</template>

<script>
  export default {
    computed: {
      layout() {
        return this.$route.meta.layout || "Default"
      }
    }
  }
</script>

<style scoped>
  
</style>
