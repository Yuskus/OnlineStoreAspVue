<template>
    <div class="item" :item="item" :key="index" :isBounce="isBounce" @click="clickOnItem(index)" >
        <div :class="{ bounce: isBounce }">
            <div><img src="../../assets/item.jpg" /></div>
            <div class="item-desc">{{ item.name }}</div>
            <div class="item-desc">{{ item.category }}</div>
            <div class="item-desc">{{ item.price }}</div>
        </div>
    </div>
</template>

<script>
export default {
    props: [ 'item', 'index', 'isBounce' ],
    emits: [ 'item-click' ],
    methods: {
        async clickOnItem(index) {
            this.$emit('item-click', index);
        }
    }
}
</script>

<style scoped>
  img {
    border-radius: 20px 20px 0px 0px;
  }

  @keyframes bounce {
    0%, 40%, 100% {
      transform: translateY(0);
    }
    20% {
      transform: translateY(-20px);
    }
    60% {
      transform: translateY(-10px);
    }
  }

  .item {
    background-color: #f0f5fa;
    align-items: center;
    box-shadow: 1px 2px 4px rgba(0, 50, 100, 0.2);
    align-content: space-around;
    margin: 10px;
    border-radius: 20px 20px 20px 20px;
  }

  .item:hover {
    box-shadow: 4px 8px 16px rgba(0, 50, 100, 0.4);
  }

  .bounce {
    animation: bounce 1s;
  }

  .item div {
    align-self: center;
  }

  .item-desc {
    padding: 10px;
  }
</style>