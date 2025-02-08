<template>
  <div class="container">
    <button v-if="isNormalPage(1)" type="button" @click="goToPage(1)">&lt;&lt;</button>
    <button v-else type="button" disabled>&lt;&lt;</button>

    <button v-if="isNormalPage(thisPage - 1)" type="button" @click="goToPrev()">&lt;</button>
    <button v-else type="button" disabled>&lt;</button>
    
    <button type="button" v-for="page in nearbyPages()" :key="page" :class="{active: thisPage === page}" @click="goToPage(page)">{{ page }}</button>

    <button v-if="isNormalPage(thisPage + 1)" type="button" @click="goToNext()">&gt;</button>
    <button v-else type="button" disabled>&gt;</button>

    <button v-if="isNormalPage(totalPages)" type="button" @click="goToPage(totalPages)">&gt;&gt;</button>
    <button v-else type="button" disabled>&gt;&gt;</button>
  </div>
</template>

<script>
  export default {
    name: "Pagination",
    props: {
      current: {
        type: Number,
        required: true
      },
      totalPages: {
        type: Number,
        required: true
      }
    },
    data() {
      return {
        thisPage: 1
      }
    },
    methods: {
      goToPage(number) {
        if (!this.isNormalPage(number)) return;
        this.thisPage = number;
        this.$emit('page-changed', number);
      },
      goToPrev() {
        this.goToPage(this.thisPage - 1);
      },
      goToNext() {
        this.goToPage(this.thisPage + 1);
      },
      isNormalPage(number) {
        return number <= this.totalPages && number >= 1 && number !== this.thisPage;
      },
      nearbyPages() {
        let arr = [];
        for (let i = Math.max(1, this.thisPage - 2); i <= Math.min(this.totalPages, this.thisPage + 2); i++) {
          arr.push(i);
        }
        return arr;
      }
    }
  };
</script>

<style scoped>
  .container {
    min-height: 40px;
    padding: 15px;
    display: flex;
    justify-content: center;
  }

  button {
    min-height: 50px;
    min-width: 50px;
    font-size: 16px;
    margin: 2px;
    padding: 0px 8px;
    border: 1px solid #666688;
    background-color: #f0f8ff;
    color: #666688;
    border-radius: 25px;
  }

  button:disabled {
    background-color: #eef0f2;
    color: #888899;
    border: 1px solid #8888aa;
  }

  button:hover:enabled {
    color: #333355;
    background-color: #d3deed;
  }

  .active {
    font-size: 18px;
    border: 2px solid #666688;
    background-color: #c5ddfc;
  }
</style>
